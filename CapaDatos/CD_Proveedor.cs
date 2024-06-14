using CapaEntidad;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace CapaDatos
{
    public class CD_Proveedor
    {
        public List<MUEB_PROVEEDOR> Listar()
        {
            List<MUEB_PROVEEDOR> lista = new List<MUEB_PROVEEDOR>();

            // Crear una conexión a la base de datos Oracle
            OracleConnection connection = new OracleConnection(Conexion.connectionString);

            string query = "SELECT PROV_ID, PROV_NOMBRE, PROV_TELEFONO_CEL, PROV_DIRECCION, PROV_CIUDAD, PROV_DEPTO, PROV_PAIS FROM MUEB_PROVEEDOR";
            OracleCommand command = new OracleCommand(query, connection);

            // Abrir la conexión a la base de datos
            connection.Open();//Dios Mio Santificado sea tu nombre esta mamada ya jala

            // Ejecutar el comando y almacenar los resultados en un objeto DataReader
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(
                    new MUEB_PROVEEDOR()
                    {
                        PROV_ID = Convert.ToInt32(reader["PROV_ID"]),
                        PROV_NOMBRE = reader["PROV_NOMBRE"].ToString(),
                        PROV_TELEFONO_CEL = reader["PROV_TELEFONO_CEL"].ToString(),
                        PROV_DIRECCION = reader["PROV_DIRECCION"].ToString(),
                        PROV_CIUDAD = reader["PROV_CIUDAD"].ToString(),
                        PROV_DEPTO = reader["PROV_DEPTO"].ToString(),
                        PROV_PAIS = reader["PROV_PAIS"].ToString()

                    }

                    );
            }

            // Cerrar el DataReader y la conexión a la base de datos
            reader.Close();
            connection.Close();


            return lista;
        }


        public int Registrar(MUEB_PROVEEDOR obj, out string Mensaje)
        {
            int idautogenerado = 0;
            bool respuesta = false;
            Mensaje = string.Empty;
            int UltimoCantidadRegistrado = Listar().Count() + 1;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_INS_PROVEEDOR", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Int32).Value = UltimoCantidadRegistrado;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.PROV_NOMBRE;
                    cmd.Parameters.Add("SP_TEL_CEL", OracleDbType.Varchar2).Value = obj.PROV_TELEFONO_CEL;
                    cmd.Parameters.Add("SP_DIRECCION", OracleDbType.Varchar2).Value = obj.PROV_DIRECCION;
                    cmd.Parameters.Add("SP_CIUDAD", OracleDbType.Varchar2).Value = obj.PROV_CIUDAD;
                    cmd.Parameters.Add("SP_DEPTO", OracleDbType.Varchar2).Value = obj.PROV_DEPTO;
                    cmd.Parameters.Add("SP_PAIS", OracleDbType.Varchar2).Value = obj.PROV_PAIS;

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


        public bool Editar(MUEB_PROVEEDOR obj, out string Mensaje)
        {
            bool repuesta_verdadera = false;
            bool respuesta = false;

            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_UPD_PROVEEDOR", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Int32).Value = obj.PROV_ID;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.PROV_NOMBRE;
                    cmd.Parameters.Add("SP_TEL_CEL", OracleDbType.Int32).Value = obj.PROV_TELEFONO_CEL;
                    cmd.Parameters.Add("SP_DIRECCION", OracleDbType.Varchar2).Value = obj.PROV_DIRECCION;
                    cmd.Parameters.Add("SP_CIUDAD", OracleDbType.Varchar2).Value = obj.PROV_CIUDAD;
                    cmd.Parameters.Add("SP_DEPTO", OracleDbType.Varchar2).Value = obj.PROV_DEPTO;
                    cmd.Parameters.Add("SP_PAIS", OracleDbType.Varchar2).Value = obj.PROV_PAIS;

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

        public bool Eliminar(int idproveedor, out string Mensaje)
        {
            bool respuesta = false;
            bool verdaderarespuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_DEL_PROVEEDOR", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Varchar2).Value = idproveedor;

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



        //LISTAR PROVEEDOR POR CATEGORIA-------------------------------------------------------------------

        public List<MUEB_PROVEEDOR> ListarProveedorporCategoria(int idcategoria)
        {
            List<MUEB_PROVEEDOR> lista = new List<MUEB_PROVEEDOR>();

            try
            {
                using (OracleConnection connection = new OracleConnection(Conexion.connectionString))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT DISTINCT M.PROV_ID, M.PROV_NOMBRE FROM MUEB_PRODUCTO PROD");
                    sb.AppendLine("INNER JOIN MUEB_CATEGORIA C ON C.CAT_ID = PROD.PROD_CATEGORIA_FK");
                    sb.AppendLine("INNER JOIN MUEB_PROVEEDOR M ON M.PROV_ID = PROD.PROD_PROVEEDOR");
                    sb.AppendLine("WHERE C.CAT_ID = CASE WHEN :idcategoria = 0 THEN C.CAT_ID ELSE :idcategoria  END");

                    OracleCommand cmd = new OracleCommand(sb.ToString(), connection);

                    cmd.Parameters.Add(":idcategoria", OracleDbType.Int32).Value = idcategoria;
                    cmd.CommandType = CommandType.Text;

                    connection.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new MUEB_PROVEEDOR()
                            {
                                PROV_ID = Convert.ToInt32(reader["PROV_ID"]),
                                PROV_NOMBRE = reader["PROV_NOMBRE"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente
                Console.WriteLine("Error: " + ex.Message);
                lista = new List<MUEB_PROVEEDOR>();
            }

            return lista;
        }



    }






}
