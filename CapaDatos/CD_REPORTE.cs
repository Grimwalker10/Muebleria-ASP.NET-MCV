using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using CapaEntidad;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<MUEB_REPORTE> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<MUEB_REPORTE> lista = new List<MUEB_REPORTE>();
            int idventa;
            if (string.IsNullOrEmpty(idtransaccion))
            {
                idventa = 0;
            }
            else
            {
                idventa = Convert.ToInt32(idtransaccion);
            }

            using (OracleConnection connection = new OracleConnection(Conexion.connectionString))
            {


                // Crear el comando para ejecutar el procedimiento almacenado
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PA_REPORTE_VENTAS";

                // Configurar los parámetros de entrada
                command.Parameters.Add("SP_FECHA_INICIO", OracleDbType.Varchar2).Value = fechainicio;
                command.Parameters.Add("SP_FECHA_FIN", OracleDbType.Varchar2).Value = fechafin;
                command.Parameters.Add("SP_ID_VEN_ID_PK", OracleDbType.Int32).Value = idventa;

                // Parámetro de salida
                OracleParameter outCursorParam = new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor);
                outCursorParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outCursorParam);

                connection.Open();

                // Ejecutar el comando y almacenar los resultados en un objeto DataReader
                OracleDataReader reader = command.ExecuteReader();

                // Recorrer el cursor y obtener los datos
                while (reader.Read())
                {
                    lista.Add(new MUEB_REPORTE()
                    {
                        FechaVenta = reader["FechaVenta"].ToString(),
                        Cliente = reader["NombreCompleto"].ToString(),
                        Producto = reader["PROD_NOMBRE"].ToString(),
                        Precio = Convert.ToDecimal(reader["PROD_PRECIO"], new CultureInfo("es-PE")),
                        Cantidad = Convert.ToInt32(reader["DETVEN_CANTIDAD"].ToString()),
                        Total = Convert.ToDecimal(reader["DETVEN_TOTAL"], new CultureInfo("es-PE")),
                        IdTransaccion = reader["VEN_ID_PK"].ToString()

                    });

                }

                // Cerrar el reader y la conexión
                reader.Close();
                connection.Close();
            }

            return lista;
        }
    }
}

