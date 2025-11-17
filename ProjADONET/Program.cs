using Microsoft.Data.SqlClient;
using ProjADONET;

var connection = new SqlConnection(DBConnection.GetConnectionString());

var pessoa = new Pessoa("Felipe Moreira", "12345678900", new DateOnly(2004, 12, 19));

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

while (reader.Read())
{
    foreach (var pessoaDaLista in reader)
    {
        Console.WriteLine(pessoaDaLista);
    }
}

connection.Close();