using CapaEntidad;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace CapaDatos
{
    public class CD_Cliente
    {
        public List<MUEB_CLIENTE> Listar()
        {
            List<MUEB_CLIENTE> lista = new List<MUEB_CLIENTE>();

            // Crear una conexión a la base de datos Oracle
            OracleConnection connection = new OracleConnection(Conexion.connectionString);

            string query = "SELECT CLI_ID_PK, CLI_TIPO_DOC, CLI_NO_DOC, CLI_NOMBRE, CLI_APELLIDO, CLI_TELEFONO_RES, CLI_TELEFONO_CEL, CLI_DIRECCION, CLI_CIUDAD, CLI_DEPTO, CLI_PAIS, CLI_PROFESION, CLI_CORREO, CLI_CLAVE, CLI_CONFIRMARCLAVE, CLI_REESTABLECER FROM MUEB_CLIENTE";
            OracleCommand command = new OracleCommand(query, connection);

            // Abrir la conexión a la base de datos
            connection.Open();//Dios Mio Santificado sea tu nombre esta mamada ya jala

            // Ejecutar el comando y almacenar los resultados en un objeto DataReader
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(
                    new MUEB_CLIENTE()
                    {
                        CLI_ID_PK = Convert.ToInt32(reader["CLI_ID_PK"]),
                        CLI_TIPO_DOC = reader["CLI_TIPO_DOC"].ToString(),
                        CLI_NO_DOC = reader["CLI_NO_DOC"].ToString(),
                        CLI_NOMBRE = reader["CLI_NOMBRE"].ToString(),
                        CLI_APELLIDO = reader["CLI_APELLIDO"].ToString(),
                        CLI_TELEFONO_RES = reader["CLI_TELEFONO_RES"].ToString(),
                        CLI_TELEFONO_CEL = reader["CLI_TELEFONO_CEL"].ToString(),
                        CLI_DIRECCION = reader["CLI_DIRECCION"].ToString(),
                        CLI_CIUDAD = reader["CLI_CIUDAD"].ToString(),
                        CLI_DEPTO = reader["CLI_DEPTO"].ToString(),
                        CLI_PAIS = reader["CLI_PAIS"].ToString(),
                        CLI_PROFESION = reader["CLI_PROFESION"].ToString(),
                        CLI_CORREO = reader["CLI_CORREO"].ToString(),
                        CLI_CLAVE = reader["CLI_CLAVE"].ToString(),
                        CLI_CONFIRMARCLAVE = reader["CLI_CONFIRMARCLAVE"].ToString(),
                        CLI_REESTABLECER = Convert.ToBoolean(reader["CLI_REESTABLECER"].ToString())

                    }

                    );
            }

            // Cerrar el DataReader y la conexión a la base de datos
            reader.Close();
            connection.Close();


            return lista;
        }





        public int Registrar(MUEB_CLIENTE obj, out string Mensaje)
        {
            int idautogenerado = 0;
            bool respuesta = false;
            Mensaje = string.Empty;
            int UltimoCantidadRegistrado = Listar().Count() + 1;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_INS_CLIENTE", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Int32).Value = UltimoCantidadRegistrado;
                    cmd.Parameters.Add("SP_TIPO_DOC", OracleDbType.Varchar2).Value = obj.CLI_TIPO_DOC;
                    cmd.Parameters.Add("SP_NO_DOC", OracleDbType.Varchar2).Value = obj.CLI_NO_DOC;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.CLI_NOMBRE;
                    cmd.Parameters.Add("SP_APELLIDO", OracleDbType.Varchar2).Value = obj.CLI_APELLIDO;
                    cmd.Parameters.Add("SP_TEL_RES", OracleDbType.Varchar2).Value = obj.CLI_TELEFONO_RES;
                    cmd.Parameters.Add("SP_TEL_CEL", OracleDbType.Varchar2).Value = obj.CLI_TELEFONO_CEL;
                    cmd.Parameters.Add("SP_DIRECCION", OracleDbType.Varchar2).Value = obj.CLI_DIRECCION;
                    cmd.Parameters.Add("SP_CIUDAD", OracleDbType.Varchar2).Value = obj.CLI_CIUDAD;
                    cmd.Parameters.Add("SP_DEPTO", OracleDbType.Varchar2).Value = obj.CLI_DEPTO;
                    cmd.Parameters.Add("SP_PAIS", OracleDbType.Varchar2).Value = obj.CLI_PAIS;
                    cmd.Parameters.Add("SP_PROFESION", OracleDbType.Varchar2).Value = obj.CLI_PROFESION;
                    cmd.Parameters.Add("SP_CORREO", OracleDbType.Varchar2).Value = obj.CLI_CORREO;
                    cmd.Parameters.Add("SP_CLAVE", OracleDbType.Varchar2).Value = obj.CLI_CLAVE;
                    cmd.Parameters.Add("SP_CONFIRMARCLAVE", OracleDbType.Varchar2).Value = "CRUD";
                    cmd.Parameters.Add("SP_REESTABLECER", OracleDbType.Varchar2).Value = "FALSE";

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                    idautogenerado = UltimoCantidadRegistrado;
                    Mensaje = "Se ha creado correctamente el proveedor";

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


        public bool Editar(MUEB_CLIENTE obj, out string Mensaje)
        {
            bool repuesta_verdadera = false;
            bool respuesta = false;

            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_UPD_CLIENTE", oconexion);
                    cmd.Parameters.Add("SP_ID_PK", OracleDbType.Int32).Value = obj.CLI_ID_PK;
                    cmd.Parameters.Add("SP_TIPO_DOC", OracleDbType.Varchar2).Value = obj.CLI_TIPO_DOC;
                    cmd.Parameters.Add("SP_NO_DOC", OracleDbType.Int32).Value = obj.CLI_NO_DOC;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.CLI_NOMBRE;
                    cmd.Parameters.Add("SP_APELLIDO", OracleDbType.Varchar2).Value = obj.CLI_APELLIDO;
                    cmd.Parameters.Add("SP_TEL_RES", OracleDbType.Varchar2).Value = obj.CLI_TELEFONO_RES;
                    cmd.Parameters.Add("SP_TEL_CEL", OracleDbType.Varchar2).Value = obj.CLI_TELEFONO_CEL;
                    cmd.Parameters.Add("SP_DIRECCION", OracleDbType.Varchar2).Value = obj.CLI_DIRECCION;
                    cmd.Parameters.Add("SP_CIUDAD", OracleDbType.Varchar2).Value = obj.CLI_CIUDAD;
                    cmd.Parameters.Add("SP_DEPTO", OracleDbType.Varchar2).Value = obj.CLI_DEPTO;
                    cmd.Parameters.Add("SP_PAIS", OracleDbType.Varchar2).Value = obj.CLI_PAIS;
                    cmd.Parameters.Add("SP_PROFESION", OracleDbType.Varchar2).Value = obj.CLI_PROFESION;
                    cmd.Parameters.Add("SP_CORREO", OracleDbType.Varchar2).Value = obj.CLI_CORREO;
                    cmd.Parameters.Add("SP_CLAVE", OracleDbType.Varchar2).Value = obj.CLI_CLAVE;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                    repuesta_verdadera = true;


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
                repuesta_verdadera = false;
                Mensaje = ex.Message.ToString();
            }
            return repuesta_verdadera;
        }



        //METODO DE CAMBIAR CLAVE -----------------------------------------------------------------------------------------------------------
        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("UPDATE MUEB_CLIENTE SET CLI_CLAVE = :nuevaclave WHERE CLI_ID_PK = :idcliente", oconexion);
                    cmd.Parameters.Add(new OracleParameter(":nuevaclave", nuevaclave));
                    cmd.Parameters.Add(new OracleParameter(":idcliente", idcliente));
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.
                    if (resultado)
                    {
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", oconexion);
                        cmdCommit.ExecuteNonQuery();
                    }


                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        //METODO DE REESTABLECER CLAVE -----------------------------------------------------------------------------------------------------------

        public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("UPDATE MUEB_CLIENTE SET CLI_CLAVE = :nuevaclave WHERE CLI_NO_DOC = :idcliente", oconexion);
                    cmd.Parameters.Add(new OracleParameter(":nuevaclave", clave));
                    cmd.Parameters.Add(new OracleParameter(":idcliente", idcliente));
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.
                    if (resultado)
                    {
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", oconexion);
                        cmdCommit.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        public bool Eliminar(int idcliente, out string Mensaje)
        {
            bool respuesta = false;
            bool verdaderarespuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_DEL_CLIENTE", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Varchar2).Value = idcliente;

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




    }
}
