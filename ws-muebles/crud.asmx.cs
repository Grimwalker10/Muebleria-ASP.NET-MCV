using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;


namespace ws_muebles
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }

        private string connectionString;
        string direccion = "localhost";
        string usuario = "LOSALPESBD";
        string contrasenia = "12345";


        public static OracleConnection ObtenerConexion(string direccion, string usuario, string contrasenia)
        {
            string connectionString = "DATA SOURCE = localhost:1521/orcl1;USER ID= LOSALPESBD;PASSWORD=12345 ";
            OracleConnection connection = new OracleConnection(connectionString);
            return connection;
        }

        public OracleDataReader EjecutarConsulta(string consulta)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            connection.Open();
            OracleCommand command = new OracleCommand(consulta, connection);
            OracleDataReader reader = command.ExecuteReader();
            return reader;
        }

        public void CerrarConexion()
        {
            OracleConnection connection = new OracleConnection(connectionString);
            connection.Close();
        }


        /*--------------------------------------------------------------------------------*/

        [WebMethod]
        public string ConsultarBaseDeDatosOracle()
        {
            string resultado = "";

            try
            {
                OracleConnection connection = WebService1.ObtenerConexion("192.168.0.21", "BYRON_MARROQUIN", "Administrador");
                connection.Open();
                OracleCommand command = new OracleCommand("Select * from countries", connection);
                OracleDataReader reader = command.ExecuteReader();

                resultado += "<table><tr><th>Columna 1</th><th>Columna 2</th><th>Columna 3</th></tr>\n";

                while (reader.Read())
                {
                    resultado += "  " + reader["COUNTRY_ID"].ToString() + "  " + reader["COUNTRY_NAME"].ToString() + "  " + reader["REGION_ID"].ToString() + "\n";
                }

                resultado += "</table>";

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                resultado = "Error: " + ex.Message;
            }

            return resultado;
        }

    }
}
