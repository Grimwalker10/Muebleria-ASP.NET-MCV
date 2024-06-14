using CapaEntidad;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;


namespace CapaDatos
{
    public class CD_Carrito
    {

        public List<MUEB_CARRITO> Listar()
        {
            List<MUEB_CARRITO> lista = new List<MUEB_CARRITO>();

            // Crear una conexión a la base de datos Oracle
            OracleConnection connection = new OracleConnection(Conexion.connectionString);

            string query = "SELECT CAR_ID, CAR_CLI_ID_FK,CAR_PROD_ID_FK,CAR_CANTIDAD FROM MUEB_CARRITO";
            OracleCommand command = new OracleCommand(query, connection);

            // Abrir la conexión a la base de datos
            connection.Open();//Dios Mio Santificado sea tu nombre esta mamada ya jala

            // Ejecutar el comando y almacenar los resultados en un objeto DataReader
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(
                    new MUEB_CARRITO()
                    {
                        CAR_ID = Convert.ToInt32(reader["CAR_ID"]),
                        CAR_CLI_ID_FK = Convert.ToInt32(reader["CAR_CLI_ID_FK"]),
                        CAR_PROD_ID_FK = Convert.ToInt32(reader["CAR_PROD_ID_FK"]),
                        CAR_CANTIDAD = Convert.ToInt32(reader["CAR_CANTIDAD"])

                    }

                    );
            }

            // Cerrar el DataReader y la conexión a la base de datos
            reader.Close();
            connection.Close();


            return lista;
        }


        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            bool respuesta = false;

            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("SP_EXISTE_CARRITO", oconexion);
                    cmd.Parameters.Add("SP_ID_CLIENTE", idcliente);
                    cmd.Parameters.Add("SP_ID_PRODUCTO", idproducto);
                    cmd.Parameters.Add("SP_RESULTADO", OracleDbType.Boolean).Direction = ParameterDirection.Output;


                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();


                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                    resultado = Convert.ToBoolean(cmd.Parameters["SP_RESULTADO"].Value);

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
                resultado = false;
            }
            return resultado;
        }
        
        public bool OperacionCarrito( int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;
            int resultado2 = 0;
            int convert = 0;
            if (sumar)
            {
                convert = 1;
            }
          else
            {
                convert = 0;
           }

          int idcarrito;
            MUEB_CARRITO carrito = new CD_Carrito().Listar().Where(c => c.CAR_PROD_ID_FK == idproducto && c.CAR_CLI_ID_FK == idcliente).FirstOrDefault();
            if(carrito != null)
            {
                idcarrito = carrito.CAR_ID;
            }
            else
            {
                idcarrito = new CD_Carrito().Listar().Count() + 1;
            }


            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {


                    OracleCommand cmd = new OracleCommand("PA_OPERACION_CARRITO", oconexion);

                    // Parámetros de entrada
                   
                    cmd.Parameters.Add("SP_ID_CLIENTE", OracleDbType.Int32).Value = idcliente; // Reemplaza con el valor adecuado
                    cmd.Parameters.Add("SP_ID_PRODUCTO", OracleDbType.Int32).Value = idproducto; // Reemplaza con el valor adecuado
                    cmd.Parameters.Add("SP_SUMAR", OracleDbType.Int32).Value = convert; // Reemplaza con el valor adecuado

                    // Parámetros de salida
                    cmd.Parameters.Add("SP_MENSAJE", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("SP_RESULTADO", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("SP_ID_CARRITO", OracleDbType.Int32).Value = idcarrito;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // Obtener los valores de salida
                    string mensaje = cmd.Parameters["SP_MENSAJE"].Value.ToString();
                    OracleDecimal resultadoOracleDecimal = (OracleDecimal)cmd.Parameters["SP_RESULTADO"].Value;
                    resultado2 = resultadoOracleDecimal.ToInt32();
                    if (resultado2 == 0)
                    {
                        resultado = false;
                    }
                    else
                    {
                        resultado = true;
                    }


                    // Utiliza los valores de salida como necesites
                    Console.WriteLine("Mensaje: " + mensaje);
                    Console.WriteLine("Resultado: " + resultado);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }


        public int CantidadEnCarrito(int idcliente)
        {
            int resultado = 0;
            bool respuesta = false;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("SELECT COUNT (*) FROM MUEB_CARRITO WHERE CAR_CLI_ID_FK = :idcliente", oconexion);
                    cmd.Parameters.Add(new OracleParameter(":idcliente", idcliente));

                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    resultado = Convert.ToInt32(cmd.ExecuteScalar());


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
                resultado = 0;

            }
            return resultado;
        }


        //LISTAR PROVEEDOR POR CATEGORIA-------------------------------------------------------------------
        public List<MUEB_CARRITO> ListarProducto(int idcliente)
        {
            List<MUEB_CARRITO> lista = new List<MUEB_CARRITO>();

            try
            {
                OracleConnection connection = new OracleConnection(Conexion.connectionString);


                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT P.PROD_ID_PK, P.PROD_NOMBRE, P.PROD_PRECIO, C.CAR_CANTIDAD, P.PROD_REF_FOTO, P.PROD_NOMBRE_IMAGEN FROM  MUEB_CARRITO C");
                sb.AppendLine("INNER JOIN MUEB_PRODUCTO P ON P.PROD_ID_PK = C.CAR_PROD_ID_FK");
                sb.AppendLine("WHERE C.CAR_CLI_ID_FK = :idcliente");

                OracleCommand command = new OracleCommand(sb.ToString(), connection);
                command.Parameters.Add(":idcliente", OracleDbType.Int32).Value = idcliente;
                command.CommandType = CommandType.Text;

                connection.Open();

                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(
                        new MUEB_CARRITO()
                        {
                            oProducto = new MUEB_PRODUCTO()
                            {
                                PROD_ID_PK = Convert.ToInt32(reader["PROD_ID_PK"]),
                                PROD_NOMBRE = reader["PROD_NOMBRE"].ToString(),
                                PROD_PRECIO = Convert.ToInt32(reader["PROD_PRECIO"]),
                                PROD_REF_FOTO = reader["PROD_REF_FOTO"].ToString(),
                                PROD_NOMBRE_IMAGEN = reader["PROD_NOMBRE_IMAGEN"].ToString()
                            },
                            CAR_CANTIDAD = Convert.ToInt32(reader["CAR_CANTIDAD"])
                        });
                }
            }
            catch (Exception ex)
            {
               string ayuda = ex.Message; 
                lista = new List<MUEB_CARRITO>();
            }

            return lista;
        }




        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            int resultado2 = 0;
            bool respuesta = false;
            int carritocount = 0;
            MUEB_CARRITO carrito = new CD_Carrito().Listar().Where(c => c.CAR_CLI_ID_FK == idcliente && c.CAR_PROD_ID_FK == idproducto).FirstOrDefault();
            if (carrito != null)
            {
                carritocount = carrito.CAR_ID;
            }
            else
            {
                respuesta = false;
            }

            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_ELIMINAR_CARRITO", oconexion);


                    // Parámetros de entrada
                    cmd.Parameters.Add("SP_ID_CARRITO", OracleDbType.Int32).Value = carritocount; // Reemplaza con el valor adecuado
                    cmd.Parameters.Add("SP_ID_CLIENTE", OracleDbType.Int32).Value = idcliente; // Reemplaza con el valor adecuado
                    cmd.Parameters.Add("SP_ID_PRODUCTO", OracleDbType.Int32).Value = idproducto; // Reemplaza con el valor adecuado
                    cmd.Parameters.Add("SP_RESULTADO", OracleDbType.Int32).Direction = ParameterDirection.Output;
                
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();


                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                    OracleDecimal resultadoOracleDecimal = (OracleDecimal)cmd.Parameters["SP_RESULTADO"].Value;
                    resultado2 = resultadoOracleDecimal.ToInt32();
                    if (resultado2 == 0)
                    {
                        resultado = false;
                    }
                    else
                    {
                        resultado = true;
                    }


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
                resultado = false;
            }
            return resultado;
        }

    }
}




