using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjADONET
{
    internal class DBConnection
    {
        public string ConnectionString { get; private set; } = String.Empty;
        
        public static string GetConnectionString()
        {
            return "Data Source=localhost;Initial Catalog=AulaADO;User Id = sa;Password = SqlServer@2022; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;";
        }
    }
}
