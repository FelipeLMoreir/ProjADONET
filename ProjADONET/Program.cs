using Microsoft.Data.SqlClient;
using ProjADONET;

var connection = new SqlConnection(DBConnection.GetConnectionString());

var pessoa = new Pessoa("João Silva", "12345678900", new DateOnly(1990, 5, 15));

var sqlInsertPessoa = $"INSERT INTO Pessoas (nome, cpf, dataNascimento) " +
    $"VALUES (@Nome, @CPF, @DataNascimento)";

connection.Open();

var command = new SqlCommand(sqlInsertPessoa, connection);

command.Parameters.AddWithValue("@Nome", pessoa.Nome);
command.Parameters.AddWithValue("@CPF", pessoa.CPF);
command.Parameters.AddWithValue("@DataNascimento", pessoa.DataNascimento);

int linhas = command.ExecuteNonQuery();

if (linhas > 0)
{
    Console.WriteLine("Pessoa Inserida com sucesso!");
}
else
{
    Console.WriteLine("Erro ao inserir pessoa!");
}

connection.Close();

connection.Open();

var sqlSelectPessoas = "SELECT id, nome, cpf, dataNascimento FROM Pessoas";

command = new SqlCommand(sqlSelectPessoas, connection);

var reader = command.ExecuteReader();



connection.Close();