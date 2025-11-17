using Microsoft.Data.SqlClient;
using ProjADONET;

var connection = new SqlConnection(DBConnection.GetConnectionString());

var pessoa = new Pessoa("João Silva", "123.456.789-00", new DateOnly(1990, 5, 15));



connection.Open();



connection.Close();