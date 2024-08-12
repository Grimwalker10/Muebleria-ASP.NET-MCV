using CapaEntidad;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuarios
    {

        public List<MUEB_USUARIO> Listar()
        {
            List<MUEB_USUARIO> lista = new List<MUEB_USUARIO>();

            // Crear una conexión a la base de datos Oracle
            OracleConnection connection = new OracleConnection(Conexion.connectionString);

            string query = "SELECT USU_ID_PK, USU_TIPO_DOC, USU_NO_DOC, USU_NOMBRE, USU_APELLIDO, USU_TELEFONO_RES, USU_TELEFONO_CEL, USU_DIRECCION, USU_CIUDAD, USU_DEPTO, USU_PAIS, USU_PROFESION, USU_CORREO, USU_CLAVE, USU_PUESTO, USU_SUELDO FROM MUEB_USUARIO";
            OracleCommand command = new OracleCommand(query, connection);

            // Abrir la conexión a la base de datos
            connection.Open();//Dios Mio Santificado sea tu nombre esta mamada ya jala

            var task = Task.Run(async () => 
            {

                // Ejecutar el comando y almacenar los resultados en un objeto DataReader
                OracleDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(
                        new MUEB_USUARIO()
                        {
                            USU_ID_PK = Convert.ToInt32(reader["USU_ID_PK"]),
                            USU_TIPO_DOC = reader["USU_TIPO_DOC"].ToString(),
                            USU_NO_DOC = Convert.ToInt32(reader["USU_NO_DOC"]),
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
                            USU_PUESTO = reader["USU_PUESTO"].ToString(),
                            USU_SUELDO = reader["USU_SUELDO"].ToString()
                        }

                        );
                }

                // Cerrar el DataReader
                reader.Close();

            });

            task.GetAwaiter().GetResult();

            // Cerrar  la conexión a la base de datos
            connection.Close();


            return lista;
        }


        
        public int Registrar(MUEB_USUARIO obj, out string Mensaje)
        {
            int idautogenerado = 0;
            bool respuesta = false;
            Mensaje = string.Empty;
            int UltimoCantidadRegistrado = Listar().Count() + 1;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_INS_USUARIO", oconexion);
                    cmd.Parameters.Add("SP_ID_PK", OracleDbType.Int32).Value = UltimoCantidadRegistrado;
                    cmd.Parameters.Add("SP_TIPO_DOC", OracleDbType.Varchar2).Value = obj.USU_TIPO_DOC;
                    cmd.Parameters.Add("SP_NO_DOC", OracleDbType.Int32).Value = obj.USU_NO_DOC;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.USU_NOMBRE;
                    cmd.Parameters.Add("SP_APELLIDO", OracleDbType.Varchar2).Value = obj.USU_APELLIDO;
                    cmd.Parameters.Add("SP_TEL_RES", OracleDbType.Varchar2).Value = Convert.ToString(obj.USU_TELEFONO_RES);
                    cmd.Parameters.Add("SP_TEL_CEL", OracleDbType.Varchar2).Value = Convert.ToString(obj.USU_TELEFONO_CEL);
                    cmd.Parameters.Add("SP_DIRECCION", OracleDbType.Varchar2).Value = obj.USU_DIRECCION;
                    cmd.Parameters.Add("SP_CIUDAD", OracleDbType.Varchar2).Value = obj.USU_CIUDAD;
                    cmd.Parameters.Add("SP_DEPTO", OracleDbType.Varchar2).Value = obj.USU_DEPTO;
                    cmd.Parameters.Add("SP_PAIS", OracleDbType.Varchar2).Value = obj.USU_PAIS;
                    cmd.Parameters.Add("SP_PROFESION", OracleDbType.Varchar2).Value = obj.USU_PROFESION;
                    cmd.Parameters.Add("SP_CORREO", OracleDbType.Varchar2).Value = obj.USU_CORREO;
                    cmd.Parameters.Add("SP_CLAVE", OracleDbType.Varchar2).Value = obj.USU_CLAVE;

                    cmd.Parameters.Add("SP_PUESTO", OracleDbType.Varchar2).Value = obj.USU_PUESTO;
                    cmd.Parameters.Add("SP_SUELDO", OracleDbType.Varchar2).Value = obj.USU_SUELDO;
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false ;
                    idautogenerado = UltimoCantidadRegistrado;
                    Mensaje = "Se ha creado correctamente el empleado";

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
        

        public bool Editar(MUEB_USUARIO obj, out string Mensaje)
        {
            bool repuesta_verdadera = false;
            bool respuesta = false;

            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_UPD_USUARIO", oconexion);
                    cmd.Parameters.Add("SP_ID_PK", OracleDbType.Int32).Value = obj.USU_ID_PK;
                    cmd.Parameters.Add("SP_TIPO_DOC", OracleDbType.Varchar2).Value = obj.USU_TIPO_DOC;
                    cmd.Parameters.Add("SP_NO_DOC", OracleDbType.Int32).Value = obj.USU_NO_DOC;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.USU_NOMBRE;
                    cmd.Parameters.Add("SP_APELLIDO", OracleDbType.Varchar2).Value = obj.USU_APELLIDO;
                    cmd.Parameters.Add("SP_TEL_RES", OracleDbType.Varchar2).Value = Convert.ToString(obj.USU_TELEFONO_RES);
                    cmd.Parameters.Add("SP_TEL_CEL", OracleDbType.Varchar2).Value = Convert.ToString(obj.USU_TELEFONO_CEL);
                    cmd.Parameters.Add("SP_DIRECCION", OracleDbType.Varchar2).Value = obj.USU_DIRECCION;
                    cmd.Parameters.Add("SP_CIUDAD", OracleDbType.Varchar2).Value = obj.USU_CIUDAD;
                    cmd.Parameters.Add("SP_DEPTO", OracleDbType.Varchar2).Value = obj.USU_DEPTO;
                    cmd.Parameters.Add("SP_PAIS", OracleDbType.Varchar2).Value = obj.USU_PAIS;
                    cmd.Parameters.Add("SP_PROFESION", OracleDbType.Varchar2).Value = obj.USU_PROFESION;
                    cmd.Parameters.Add("SP_CORREO", OracleDbType.Varchar2).Value = obj.USU_CORREO;
                    cmd.Parameters.Add("SP_CLAVE", OracleDbType.Varchar2).Value = obj.USU_CLAVE;

                    cmd.Parameters.Add("SP_PUESTO", OracleDbType.Varchar2).Value = obj.USU_PUESTO;
                    cmd.Parameters.Add("SP_SUELDO", OracleDbType.Varchar2).Value = obj.USU_SUELDO;

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


        public bool Eliminar(int idusuario, out string Mensaje)
        {
            bool respuesta = false;
            bool verdaderarespuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_DEL_USUARIO", oconexion);
                    cmd.Parameters.Add("SP_ID_PK", OracleDbType.Int32).Value = idusuario;

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


        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("UPDATE MUEB_USUARIO SET USU_CLAVE = :nuevaclave WHERE USU_ID_PK = :idusuario", oconexion);
                    cmd.Parameters.Add(new OracleParameter(":nuevaclave", nuevaclave));
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
                    OracleCommand cmd = new OracleCommand("UPDATE MUEB_USUARIO SET USU_CLAVE = :nuevaclave WHERE USU_ID_PK = :idusuario", oconexion);
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