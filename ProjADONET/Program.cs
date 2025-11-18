using Microsoft.Data.SqlClient;
using ProjADONET;

var connection = new SqlConnection(DBConnection.GetConnectionString());

#region insert
var pessoa = new Pessoa("Caillou", "1171717171", new DateOnly(2004, 12, 19));

var sqlInsertPessoa = $"INSERT INTO Pessoas (nome, cpf, dataNascimento) " +
    $"VALUES (@Nome, @CPF, @DataNascimento) SELECT SCOPE_IDENTITY();";

connection.Open();

var command = new SqlCommand(sqlInsertPessoa, connection);

command.Parameters.AddWithValue("@Nome", pessoa.Nome);
command.Parameters.AddWithValue("@CPF", pessoa.CPF);
command.Parameters.AddWithValue("@DataNascimento", pessoa.DataNascimento);

int pessoaId = Convert.ToInt32(command.ExecuteScalar());

//int linhas = command.ExecuteNonQuery();

//if (linhas > 0)
//{
//    Console.WriteLine("Pessoa Inserida com sucesso!");
//}
//else
//{
//    Console.WriteLine("Erro ao inserir pessoa!");
//}

var telefones = new List<Telefone>()
{
    new Telefone("11", "987654321", "Celular", pessoaId),
    new Telefone("11", "912345678", "Casa", pessoaId)
};

foreach (var telefone in telefones)
{
    var sqlInsertTelefone = @"
            INSERT INTO Telefones (ddd, numero, tipo, pessoaId)
            VALUES (@DDD, @Numero, @Tipo, @PessoaId);
        ";

    using var commandTelefone = new SqlCommand(sqlInsertTelefone, connection);
    commandTelefone.Parameters.AddWithValue("@DDD", telefone.DDD);
    commandTelefone.Parameters.AddWithValue("@Numero", telefone.Numero);
    commandTelefone.Parameters.AddWithValue("@Tipo", telefone.Tipo);
    commandTelefone.Parameters.AddWithValue("@PessoaId", telefone.PessoaId);

    commandTelefone.ExecuteNonQuery();
}

var enderecos = new List<Endereco>
{
    new Endereco("Avenida Conhecida", "123", "Apto 1", "Ponte", "Cidade Z", "SP", "12345-678", pessoaId),
    new Endereco("Rua Bonita", "456", "", "Bairro João de Barro", "Cidade A", "SP", "87654-321", pessoaId)
};

foreach (var endereco in enderecos)
{
    var sqlInsertEndereco = @"
        INSERT INTO Enderecos (logradouro, numero, complemento, bairro, cidade, estado, cep, pessoaId)
        VALUES (@Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @Estado, @Cep, @PessoaId)
    ";

    using var commandEndereco = new SqlCommand(sqlInsertEndereco, connection);
    commandEndereco.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
    commandEndereco.Parameters.AddWithValue("@Numero", string.IsNullOrWhiteSpace(endereco.Numero) ?
        DBNull.Value : endereco.Numero);
    commandEndereco.Parameters.AddWithValue("@Complemento", string.IsNullOrWhiteSpace(endereco.Complemento) ?
        DBNull.Value : endereco.Complemento);
    commandEndereco.Parameters.AddWithValue("@Bairro", endereco.Bairro);
    commandEndereco.Parameters.AddWithValue("@Cidade", endereco.Cidade);
    commandEndereco.Parameters.AddWithValue("@Estado", endereco.Estado);
    commandEndereco.Parameters.AddWithValue("@Cep", endereco.Cep);
    commandEndereco.Parameters.AddWithValue("@PessoaId", endereco.PessoaId);

    commandEndereco.ExecuteNonQuery();
}

connection.Close();
#endregion
#region select
connection.Open();

var sqlSelectPessoas = @"
    SELECT 
        p.id AS PessoaId, p.nome, p.cpf, p.dataNascimento,
        t.id AS TelefoneId, t.ddd, t.numero, t.tipo, t.pessoaId AS TelefonePessoaId,
        e.id AS EnderecoId, e.logradouro, e.numero, e.complemento, e.bairro, e.cidade, e.estado, 
        e.cep, e.pessoaId AS EnderecoPessoaId
    FROM Pessoas p
    LEFT JOIN Telefones t ON p.id = t.pessoaId
    LEFT JOIN Enderecos e ON p.id = e.pessoaId
    ORDER BY p.id";

command = new SqlCommand(sqlSelectPessoas, connection);

var reader = command.ExecuteReader();

var pessoas = new List<Pessoa>();

while (reader.Read())
{
    pessoaId = reader.GetInt32(0);
    var pessoaExistente = pessoas.FirstOrDefault(p => p.Id == pessoaId);

    if (pessoaExistente == null)
    {
        pessoaExistente = new Pessoa(
            reader.GetString(1),
            reader.GetString(2),
            DateOnly.FromDateTime(reader.GetDateTime(3))
        );
        pessoaExistente.SetId(pessoaId);
        pessoas.Add(pessoaExistente);
    }

    if (!reader.IsDBNull(4)) // Existe telefone?
    {
        var telefone = new Telefone(
            reader.GetString(5),
            reader.GetString(6),
            reader.GetString(7),
            reader.GetInt32(8)
        );
        telefone.SetId(reader.GetInt32(4));
        if (!pessoaExistente.Telefones.Any(t => t.Id == telefone.Id))
            pessoaExistente.Telefones.Add(telefone);
    }

    if (!reader.IsDBNull(reader.GetOrdinal("EnderecoId")))
    {
        var endereco = new Endereco(
            reader.GetString(reader.GetOrdinal("logradouro")),
            reader.IsDBNull(reader.GetOrdinal("numero")) ? "" :
            reader.GetString(reader.GetOrdinal("numero")),
            reader.IsDBNull(reader.GetOrdinal("complemento")) ? "" : 
            reader.GetString(reader.GetOrdinal("complemento")),
            reader.GetString(reader.GetOrdinal("bairro")),
            reader.GetString(reader.GetOrdinal("cidade")),
            reader.GetString(reader.GetOrdinal("estado")),
            reader.GetString(reader.GetOrdinal("cep")),
            pessoaId
        );
        endereco.SetId(reader.GetInt32(reader.GetOrdinal("EnderecoId")));
        if (!pessoaExistente.Enderecos.Any(e => e.Id == endereco.Id))
            pessoaExistente.Enderecos.Add(endereco);
    }
}

reader.Close();

foreach (var p in pessoas)
{
    Console.WriteLine(p);
    foreach (var tel in p.Telefones)
    {
        Console.WriteLine("  Telefone: " + tel);
    }
    foreach (var end in p.Enderecos)
    {
        Console.WriteLine("  Endereço: " + end);
    }
    Console.WriteLine("----------------------------");
}

connection.Close();
#endregion
#region update
connection.Open();

var sqlUpdatePessoa = "UPDATE Pessoas SET nome = @Nome WHERE id = @Id";

command = new SqlCommand(sqlUpdatePessoa, connection);
command.Parameters.AddWithValue("@Nome", "Felipe M. Silva");
command.Parameters.AddWithValue("@Id", 1);

command.ExecuteNonQuery();

var sqlUpdateTelefone = "UPDATE Telefones SET numero = @Numero WHERE id = @Id";

command = new SqlCommand(sqlUpdateTelefone, connection);
command.Parameters.AddWithValue("@Numero", "987654322");
command.Parameters.AddWithValue("@Id", 1);

command.ExecuteNonQuery();

connection.Close();
#endregion
#region delete
connection.Open();

var sqlDeletePessoa = "DELETE FROM Pessoas WHERE id = @Id";

command = new SqlCommand(sqlDeletePessoa, connection);
command.Parameters.AddWithValue("@Id", 6);

command.ExecuteNonQuery();

var sqlDeleteTelefone = "DELETE FROM Telefones WHERE id = @Id";

command = new SqlCommand(sqlDeleteTelefone, connection);
command.Parameters.AddWithValue("@Id", 6);

command.ExecuteNonQuery();

connection.Close();
#endregion