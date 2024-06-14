using CapaEntidad;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace CapaDatos
{
    public class CD_Producto
    {

        public List<MUEB_PRODUCTO> Listar()
        {
            List<MUEB_PRODUCTO> lista = new List<MUEB_PRODUCTO>();

            // Crear una conexión a la base de datos Oracle
            OracleConnection connection = new OracleConnection(Conexion.connectionString);

            string query = "SELECT PROD_ID_PK, PROD_REFERENCIA, PROD_NOMBRE, PROD_DESCRIPCION, PROD_CATEGORIA_FK, PROD_MATERIAL, PROD_ALTO, PROD_ANCHO, PROD_PROFUNDIDAD, PROD_COLOR, PROD_PESO, PROD_PROVEEDOR, PROD_REF_FOTO, PROD_BASE64, PROD_EXTENSION, PROD_NOMBRE_IMAGEN, PROD_PRECIO, PROD_UNIDADES FROM MUEB_PRODUCTO";
            OracleCommand command = new OracleCommand(query, connection);

            // Abrir la conexión a la base de datos
            connection.Open();//Dios Mio Santificado sea tu nombre esta mamada ya jala

            // Ejecutar el comando y almacenar los resultados en un objeto DataReader
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(
                    new MUEB_PRODUCTO()
                    {
                        PROD_ID_PK = Convert.ToInt32(reader["PROD_ID_PK"]),
                        PROD_REFERENCIA = reader["PROD_REFERENCIA"].ToString(),
                        PROD_NOMBRE = reader["PROD_NOMBRE"].ToString(),
                        PROD_DESCRIPCION = reader["PROD_DESCRIPCION"].ToString(),
                        PROD_CATEGORIA_FK = Convert.ToInt32(reader["PROD_CATEGORIA_FK"]),
                        PROD_MATERIAL = reader["PROD_MATERIAL"].ToString(),
                        PROD_ALTO = reader["PROD_ALTO"].ToString(),
                        PROD_ANCHO = reader["PROD_ANCHO"].ToString(),
                        PROD_PROFUNDIDAD = reader["PROD_PROFUNDIDAD"].ToString(),
                        PROD_COLOR = reader["PROD_COLOR"].ToString(),
                        PROD_PESO = reader["PROD_PESO"].ToString(),
                        PROD_PROVEEDOR = Convert.ToInt32(reader["PROD_PROVEEDOR"]),
                        PROD_REF_FOTO = reader["PROD_REF_FOTO"].ToString(),
                        PROD_BASE64 = reader["PROD_BASE64"].ToString(),
                        PROD_EXTENSION = reader["PROD_EXTENSION"].ToString(),
                        PROD_NOMBRE_IMAGEN = reader["PROD_NOMBRE_IMAGEN"].ToString(),
                        PROD_PRECIO = Convert.ToInt32(reader["PROD_PRECIO"]),
                        PROD_UNIDADES = Convert.ToInt32(reader["PROD_UNIDADES"])

                    }

                    );
            }

            // Cerrar el DataReader y la conexión a la base de datos
            reader.Close();
            connection.Close();


            return lista;
        }



        public int Registrar(MUEB_PRODUCTO obj, out string Mensaje)
        {
            int idautogenerado = 0;
            bool respuesta = false;
            Mensaje = string.Empty;
            int UltimoCantidadRegistrado = Listar().Count() + 1;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_INS_PRODUCTO", oconexion);
                    cmd.Parameters.Add("SP_ID_PK", OracleDbType.Int32).Value = UltimoCantidadRegistrado;
                    cmd.Parameters.Add("SP_REFERENCIA", OracleDbType.Varchar2).Value = obj.PROD_REFERENCIA;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.PROD_NOMBRE;
                    cmd.Parameters.Add("SP_DESCRIPCION", OracleDbType.Varchar2).Value = obj.PROD_DESCRIPCION;
                    cmd.Parameters.Add("SP_CATEGORIA_FK", OracleDbType.Int32).Value = obj.PROD_CATEGORIA_FK;
                    cmd.Parameters.Add("SP_MATERIAL", OracleDbType.Varchar2).Value = obj.PROD_MATERIAL;
                    cmd.Parameters.Add("SP_ALTO", OracleDbType.Varchar2).Value = obj.PROD_ALTO;
                    cmd.Parameters.Add("SP_ANCHO", OracleDbType.Varchar2).Value = obj.PROD_ANCHO;
                    cmd.Parameters.Add("SP_PROFUNDIDAD", OracleDbType.Varchar2).Value = obj.PROD_PROFUNDIDAD;
                    cmd.Parameters.Add("SP_COLOR", OracleDbType.Varchar2).Value = obj.PROD_COLOR;
                    cmd.Parameters.Add("SP_PESO", OracleDbType.Varchar2).Value = obj.PROD_PRECIO;
                    cmd.Parameters.Add("SP_ID_PROVEEDOR", OracleDbType.Int32).Value = obj.PROD_PROVEEDOR;
                    cmd.Parameters.Add("SP_REF_FOTO", OracleDbType.Varchar2).Value = "0";
                    cmd.Parameters.Add("SP_BASE64", OracleDbType.Varchar2).Value = "0";
                    cmd.Parameters.Add("SP_EXTENSION", OracleDbType.Varchar2).Value = "0";
                    cmd.Parameters.Add("SP_NOMBRE_IMAGEN", OracleDbType.Varchar2).Value = "0";
                    cmd.Parameters.Add("SP_PRECIO", OracleDbType.Int32).Value = obj.PROD_PRECIO;
                    cmd.Parameters.Add("SP_UNIDADES", OracleDbType.Int32).Value = obj.PROD_UNIDADES;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                    idautogenerado = UltimoCantidadRegistrado;
                    Mensaje = "Se ha creado correctamente el producto";

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


        public bool Editar(MUEB_PRODUCTO obj, out string Mensaje)
        {
            bool repuesta_verdadera = false;
            bool respuesta = false;

            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_UPD_PRODUCTO", oconexion);
                    cmd.Parameters.Add("SP_ID_PK", OracleDbType.Int32).Value = obj.PROD_ID_PK;
                    cmd.Parameters.Add("SP_REFERENCIA", OracleDbType.Varchar2).Value = obj.PROD_REFERENCIA;
                    cmd.Parameters.Add("SP_NOMBRE", OracleDbType.Varchar2).Value = obj.PROD_NOMBRE;
                    cmd.Parameters.Add("SP_DESCRIPCION", OracleDbType.Varchar2).Value = obj.PROD_DESCRIPCION;
                    cmd.Parameters.Add("SP_CATEGORIA_FK", OracleDbType.Varchar2).Value = obj.PROD_CATEGORIA_FK;
                    cmd.Parameters.Add("SP_MATERIAL", OracleDbType.Varchar2).Value = obj.PROD_MATERIAL;
                    cmd.Parameters.Add("SP_ALTO", OracleDbType.Varchar2).Value = obj.PROD_ALTO;
                    cmd.Parameters.Add("SP_ANCHO", OracleDbType.Varchar2).Value = obj.PROD_ANCHO;
                    cmd.Parameters.Add("SP_PROFUNDIDAD", OracleDbType.Varchar2).Value = obj.PROD_PROFUNDIDAD;
                    cmd.Parameters.Add("SP_COLOR", OracleDbType.Varchar2).Value = obj.PROD_COLOR;
                    cmd.Parameters.Add("SP_PESO", OracleDbType.Varchar2).Value = obj.PROD_PESO;
                    cmd.Parameters.Add("SP_PROVEEDOR", OracleDbType.Int32).Value = obj.PROD_PROVEEDOR;
                    cmd.Parameters.Add("SP_REF_FOTO", OracleDbType.Varchar2).Value = obj.PROD_REF_FOTO;
                    cmd.Parameters.Add("SP_BASE64", OracleDbType.Varchar2).Value = obj.PROD_BASE64;
                    cmd.Parameters.Add("SP_EXTENSION", OracleDbType.Varchar2).Value = obj.PROD_EXTENSION;
                    cmd.Parameters.Add("SP_NOMBRE_IMAGEN", OracleDbType.Varchar2).Value = obj.PROD_NOMBRE_IMAGEN;
                    cmd.Parameters.Add("SP_PRECIO", OracleDbType.Int32).Value = obj.PROD_PRECIO;
                    cmd.Parameters.Add("SP_UNIDADES", OracleDbType.Int32).Value = obj.PROD_UNIDADES;

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



        public bool GuardarDatosImagen(MUEB_PRODUCTO obj, out string Mensaje)
        {

            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {

                    string query = "UPDATE MUEB_PRODUCTO SET PROD_REF_FOTO = :ruta, PROD_NOMBRE_IMAGEN = :nombreimagen WHERE PROD_ID_PK = :idproducto";

                    OracleCommand cmd = new OracleCommand(query, oconexion);
                    cmd.Parameters.Add(new OracleParameter(":ruta", obj.PROD_REF_FOTO));
                    cmd.Parameters.Add(new OracleParameter(":nombreimagen", obj.PROD_NOMBRE_IMAGEN));
                    cmd.Parameters.Add(new OracleParameter(":idproducto", obj.PROD_ID_PK));
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", oconexion);
                        cmdCommit.ExecuteNonQuery();
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
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


        public bool Eliminar(int idproducto, out string Mensaje)
        {
            bool respuesta = false;
            bool verdaderarespuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_DEL_PRODUCTO", oconexion);
                    cmd.Parameters.Add("SP_ID", OracleDbType.Int32).Value = idproducto;

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