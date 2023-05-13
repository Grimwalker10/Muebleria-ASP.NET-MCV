using CapaEntidad;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace CapaDatos
{
    public class CD_Usuarios
    {

        public List<MUEB_USUARIO> Listar()
        {
            List<MUEB_USUARIO> lista = new List<MUEB_USUARIO>();

            // Crear una conexión a la base de datos Oracle
            OracleConnection connection = new OracleConnection(Conexion.connectionString);

            string query = "SELECT USU_TIPO_DOC, USU_NO_DOC_PK, USU_NOMBRE, USU_APELLIDO, USU_TELEFONO_RES, USU_TELEFONO_CEL, USU_DIRECCION, USU_CIUDAD, USU_DEPTO, USU_PAIS, USU_PROFESION, USU_CORREO, USU_CLAVE, USU_TIPO, USU_DETALLE_TIPO, USU_PUESTO, USU_SUELDO FROM MUEB_USUARIO";
            OracleCommand command = new OracleCommand(query, connection);

            // Abrir la conexión a la base de datos
            connection.Open();//Dios Mio Santificado sea tu nombre esta mamada ya jala

            // Ejecutar el comando y almacenar los resultados en un objeto DataReader
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(
                    new MUEB_USUARIO()
                    {
                        USU_TIPO_DOC = reader["USU_TIPO_DOC"].ToString(),
                        USU_NO_DOC_PK = Convert.ToInt32(reader["USU_NO_DOC_PK"]),
                        USU_NOMBRE = reader["USU_NOMBRE"].ToString(),
                        USU_APELLIDO = reader["USU_APELLIDO"].ToString(),
                        USU_TELEFONO_RES = Convert.ToInt32(reader["USU_TELEFONO_RES"].ToString()),
                        USU_TELEFONO_CEL = Convert.ToInt32(reader["USU_TELEFONO_CEL"].ToString()),
                        USU_DIRECCION = reader["USU_DIRECCION"].ToString(),
                        USU_CIUDAD = reader["USU_CIUDAD"].ToString(),
                        USU_DEPTO = reader["USU_DEPTO"].ToString(),
                        USU_PAIS = reader["USU_PAIS"].ToString(),
                        USU_PROFESION = reader["USU_PROFESION"].ToString(),
                        USU_CORREO = reader["USU_CORREO"].ToString(),
                        USU_CLAVE = reader["USU_CLAVE"].ToString(),
                        USU_TIPO = reader["USU_TIPO"].ToString(),
                        USU_DETALLE_TIPO = Convert.ToInt32(reader["USU_DETALLE_TIPO"]),
                        USU_PUESTO = reader["USU_PUESTO"].ToString(),
                        USU_SUELDO = reader["USU_SUELDO"].ToString()
                    }

                    );
            }

            // Cerrar el DataReader y la conexión a la base de datos
            reader.Close();
            connection.Close();


            return lista;
        }

            
        

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using(OracleConnection oconexion = new OracleConnection(Conexion.connectionString)) 
                {
                    OracleCommand cmd = new OracleCommand("UPDATE MUEB_USUARIO SET USU_CLAVE = :nuevaclave WHERE USU_NO_DOC_PK = :idusuario", oconexion);
                    cmd.Parameters.Add(new OracleParameter(":nuevaclave", nuevaclave));
                    cmd.Parameters.Add(new OracleParameter(":idusuario", idusuario));
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.
                    if(resultado)
                    {
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", oconexion);
                        cmdCommit.ExecuteNonQuery();
                    }


                }

            }catch (Exception ex)
            {
                resultado = false;
                Mensaje= ex.Message;
            }
            return resultado;
        }

        public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("UPDATE MUEB_USUARIO SET USU_CLAVE = :nuevaclave WHERE USU_NO_DOC_PK = :idusuario", oconexion);
                    cmd.Parameters.Add(new OracleParameter(":nuevaclave", clave));
                    cmd.Parameters.Add(new OracleParameter(":idusuario", idusuario));
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

    }
}