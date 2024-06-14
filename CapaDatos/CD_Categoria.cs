using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace CapaDatos
{
    public class CD_Categoria
    {

        //LISTAR CATEGORIA-------------------------------------------------------------------------------------

        public List<MUEB_CATEGORIA> Listar()
        {
            List<MUEB_CATEGORIA> lista = new List<MUEB_CATEGORIA>();

            // Crear una conexión a la base de datos Oracle
            OracleConnection connection = new OracleConnection(Conexion.connectionString);

            string query = "SELECT CAT_ID, CAT_NOMBRE, CAT_DESCRIPCION FROM MUEB_CATEGORIA";
            OracleCommand command = new OracleCommand(query, connection);

            // Abrir la conexión a la base de datos
            connection.Open();//Dios Mio Santificado sea tu nombre esta mamada ya jala

            // Ejecutar el comando y almacenar los resultados en un objeto DataReader
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(
                    new MUEB_CATEGORIA()
                    {
                        CAT_ID = Convert.ToInt32(reader["CAT_ID"]),
                        CAT_NOMBRE = reader["CAT_NOMBRE"].ToString(),
                        CAT_DESCRIPCION = reader["CAT_DESCRIPCION"].ToString()

                    }

                    );
            }

            // Cerrar el DataReader y la conexión a la base de datos
            reader.Close();
            connection.Close();


            return lista;
        }

        //REGISTRAR CATEGORIA-------------------------------------------------------------------------------------

        public int Registrar(MUEB_CATEGORIA obj, out string Mensaje)
        {
            int idautogenerado = 0;
            bool respuesta = false;
            Mensaje = string.Empty;
            int UltimoCantidadRegistrado = Listar().Count() + 1;
            try
            {

                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_INS_CATEGORIA", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Int32).Value = UltimoCantidadRegistrado;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.CAT_NOMBRE;
                    cmd.Parameters.Add("SP_DESCRIPCION", OracleDbType.Varchar2).Value = obj.CAT_DESCRIPCION;


                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                    idautogenerado = UltimoCantidadRegistrado;
                    Mensaje = "Se ha creado correctamente la categoria";
                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.
                    if (respuesta)
                    {
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", oconexion);
                        cmdCommit.ExecuteNonQuery();

                    }
                }

            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }


        //EDITAR CATEGORIA ---------------------------------------------------------------------------------------------------

        public bool Editar(MUEB_CATEGORIA obj, out string Mensaje)
        {
            bool respuesta = false;
            bool verdaderarespuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_UPD_CATEGORIA", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Int32).Value = obj.CAT_ID;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.CAT_NOMBRE;
                    cmd.Parameters.Add("SP_DESCRIPCION", OracleDbType.Varchar2).Value = obj.CAT_DESCRIPCION;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                    verdaderarespuesta = true;
                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.
                    if (respuesta)
                    {
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", oconexion);
                        cmdCommit.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                verdaderarespuesta = false;
                Mensaje = ex.Message;
            }
            return verdaderarespuesta;
        }

        //ELIMINAR -------------------------------------------------------------------
        public bool Eliminar(int idcategoria, out string Mensaje)
        {
            bool respuesta = false;
            bool verderarespuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_DEL_CATEGORIA", oconexion);

                    cmd.Parameters.Add("SP_ID", OracleDbType.Int32).Value = idcategoria;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                    verderarespuesta = true;
                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.
                    if (respuesta)
                    {
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", oconexion);
                        cmdCommit.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                verderarespuesta = false;
                Mensaje = ex.Message;
            }
            return verderarespuesta;
        }



    }
}
