using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace CapaDatos
{
    public class Conexion
    {

        //public  static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbOracle"].ConnectionString;
        public static string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=xe)));User ID=LOSALPESBD;Password=dev123;";

    }
}

