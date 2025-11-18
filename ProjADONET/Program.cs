using Microsoft.Data.SqlClient;
using ProjADONET;

var connection = new SqlConnection(DBConnection.GetConnectionString());

#region insert
var pessoa = new Pessoa("Felipe Moreira", "77745678900", new DateOnly(2004, 12, 19));

var sqlInsertPessoa = $"INSERT INTO Pessoas (nome, cpf, dataNascimento) " +
    $"VALUES (@Nome, @CPF, @DataNascimento) SELECT SCOPE_IDENTITY();10diagram";

connection.Open();

var command = new SqlCommand(sqlInsertPessoa, connection);

command.Parameters.AddWithValue("@Nome", pessoa.Nome);
command.Parameters.AddWithValue("@CPF", pessoa.CPF);
command.Parameters.AddWithValue("@DataNascimento", pessoa.DataNascimento);

int pessoaId = Convert.ToInt32(command.ExecuteScalar());

var telefone = new Telefone("11", "987654321", "Celular", pessoaId);

var sqlInsertTelefone = $"INSERT INTO Telefones (ddd, numero, tipo, pessoaId) " +
    $"VALUES (@DDD, @Numero, @Tipo, @PessoaId)";

command = new SqlCommand(sqlInsertTelefone, connection);
command.Parameters.AddWithValue("@DDD", telefone.DDD);
command.Parameters.AddWithValue("@Numero", telefone.Numero);
command.Parameters.AddWithValue("@Tipo", telefone.Tipo);
command.Parameters.AddWithValue("@PessoaId", telefone.PessoaId);

command.ExecuteNonQuery();

//int linhas = command.ExecuteNonQuery();

//if (linhas > 0)
//{
//    Console.WriteLine("Pessoa Inserida com sucesso!");
//}
//else
//{
//    Console.WriteLine("Erro ao inserir pessoa!");
//}

connection.Close();
#endregion
#region select
connection.Open();

var sqlSelectPessoas = "SELECT id, nome, cpf, dataNascimento FROM Pessoas";

command = new SqlCommand(sqlSelectPessoas, connection);

var readerPessoas = command.ExecuteReader();

while (readerPessoas.Read())
{
    var pessoaDaLista = new Pessoa(
        readerPessoas.GetString(1),
        readerPessoas.GetString(2),
        DateOnly.FromDateTime(readerPessoas.GetDateTime(3))
    );
    pessoaDaLista.SetId(readerPessoas.GetInt32(0));
    Console.WriteLine(pessoaDaLista);
}
readerPessoas.Close();

var sqlSelectTelefones = "SELECT id, ddd, numero, tipo, pessoaId FROM Telefones";

command = new SqlCommand(sqlSelectTelefones, connection);

var readerTelefones = command.ExecuteReader();

while (readerTelefones.Read())
{
    var telefoneDaLista = new Telefone(
        readerTelefones.GetString(1),
        readerTelefones.GetString(2),
        readerTelefones.GetString(3),
        readerTelefones.GetInt32(4)
    );
    telefoneDaLista.SetId(readerTelefones.GetInt32(0));
    Console.WriteLine(telefoneDaLista);
}
readerTelefones.Close();

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