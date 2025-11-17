using Microsoft.Data.SqlClient;
using ProjADONET;

var connection = new SqlConnection(DBConnection.GetConnectionString());
#region insert
var pessoa = new Pessoa("Felipe Moreira", "72345678900", new DateOnly(2004, 12, 19));

var sqlInsertPessoa = $"INSERT INTO Pessoas (nome, cpf, dataNascimento) " +
    $"VALUES (@Nome, @CPF, @DataNascimento)";

connection.Open();

var command = new SqlCommand(sqlInsertPessoa, connection);

command.Parameters.AddWithValue("@Nome", pessoa.Nome);
command.Parameters.AddWithValue("@CPF", pessoa.CPF);
command.Parameters.AddWithValue("@DataNascimento", pessoa.DataNascimento);

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

var reader = command.ExecuteReader();

while (reader.Read())
{
    var pessoaDaLista = new Pessoa(
        reader.GetString(1),
        reader.GetString(2),
        DateOnly.FromDateTime(reader.GetDateTime(3))
    );
    pessoaDaLista.SetId(reader.GetInt32(0));
    Console.WriteLine(pessoaDaLista);
}
reader.Close();

connection.Close();
#endregion
#region update
connection.Open();

var sqlUpdatePessoa = "UPDATE Pessoas SET nome = @Nome WHERE id = @Id";

command = new SqlCommand(sqlUpdatePessoa, connection);
command.Parameters.AddWithValue("@Nome", "Felipe M. Silva");
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

connection.Close();
#endregion