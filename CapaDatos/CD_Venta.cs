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
    public class CD_Venta
    {

       
        public bool Registrar(MUEB_VENTA obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool repuesta_verdadera = false;
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (OracleConnection oconexion = new OracleConnection(Conexion.connectionString))
                {
                    OracleCommand cmd = new OracleCommand("PA_INS_REGVENTA", oconexion);

                    //PARAMETROS DE ENTRADA
                    cmd.Parameters.Add("SP_ID_CLIENTE", OracleDbType.Int32).Value = obj.VEN_ID_CLIENTE_FK;
                    cmd.Parameters.Add("SP_TOTAL_PRODUCTO", OracleDbType.Int32).Value = obj.VEN_TOTALPROD;
                    cmd.Parameters.Add("SP_MONTOTOTAL", OracleDbType.Int32).Value = obj.VEN_MONTOTOTAL;
                    cmd.Parameters.Add("SP_CONTACTO", OracleDbType.Varchar2).Value = obj.VEN_CONTACTO;
                    cmd.Parameters.Add("SP_TELEFONO", OracleDbType.Varchar2).Value = obj.VEN_TELEFONO;
                    cmd.Parameters.Add("SP_DIRECCION", OracleDbType.Varchar2).Value = obj.VEN_DIRECCION;

                    //PARAMETROS DE DETALLE DE LA VENTA (USANDO EL TYPE)
                    cmd.Parameters.Add("SP_DETALLEVENTA", OracleDbType.Int32).Value = DetalleVenta;

                    //PARAMETROS DE SALIDA
                    cmd.Parameters.Add("SP_RESULTADO", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("SP_MENSAJE", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;
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


        



       

    }
}