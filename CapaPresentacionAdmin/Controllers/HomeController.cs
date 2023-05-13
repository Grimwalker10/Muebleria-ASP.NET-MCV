using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using CapaNegocio;
using CapaEntidad;
using CapaDatos;
using System.Web.Services.Description;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize] //Hace que no se muestren las vistan si no esta autorizado.
    public class HomeController : Controller
    {
        //public string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.20)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));User Id=LOSALPESBD;Password=123456;";
        public string connectionString = "Data Source=localhost ;User ID= LOSALPESBD;Password= 123456;";

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Usuarios()
        {
            return View();
        }
        public ActionResult EMPLEADOX()
        {
            ViewBag.Title = "EMPLEADOX";
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios(MUEB_USUARIO entidad)
        {

            List<MUEB_USUARIO> oLista = new List<MUEB_USUARIO>();
            oLista = new CN_Usuarios().Listar();

            return Json(new {data = oLista}, JsonRequestBehavior.AllowGet);
        }



        /*METODO PARA REGISTRAR USUAIROS ---------------------------------------------------------------------- byron*/
        public int Registrar_Usuario(MUEB_USUARIO obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (OracleConnection con = new OracleConnection(Conexion.connectionString)) /*Aca intente ponerlo desde oracle, no se si sea asi*/
                {
                    OracleCommand cmd = new OracleCommand("PA_INS_EMPLEADO", con);

                    cmd.Parameters.Add("USU_NO_DOC_PK", obj.USU_NO_DOC_PK);
                    cmd.Parameters.Add("USU_TIPO_DOC", obj.USU_TIPO_DOC);
                    cmd.Parameters.Add("USU_NOMBRE", obj.USU_NOMBRE);
                    cmd.Parameters.Add("USU_APELLIDO", obj.USU_APELLIDO);
                    cmd.Parameters.Add("USU_TELEFONO_RES", obj.USU_TELEFONO_RES);
                    cmd.Parameters.Add("USU_TELEFONO_CEL", obj.USU_TELEFONO_CEL);
                    cmd.Parameters.Add("USU_DIRECCION", obj.USU_DIRECCION);
                    cmd.Parameters.Add("USU_DEPTO", obj.USU_DEPTO);
                    cmd.Parameters.Add("USU_PAIS", obj.USU_PAIS);
                    cmd.Parameters.Add("USU_PROFESION", obj.USU_PROFESION);

                    cmd.Parameters.Add("Resultado", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                    /*CHATGPT DICE QUE ASI SE DEBE DE HACER EN ORACLE ------------------------------------------------------------------*/
                    /* cmd.Parameters.Add("USU_NO_DOC_PK", OracleDbType.Number).Value = obj.USU_NO_DOC_PK;
                     cmd.Parameters.Add("USU_TIPO_DOC", OracleDbType.Varchar2).Value = obj.USU_TIPO_DOC;
                     cmd.Parameters.Add("USU_NOMBRE", OracleDbType.Varchar2).Value = obj.USU_NOMBRE;
                     cmd.Parameters.Add("USU_APELLIDO", OracleDbType.Varchar2).Value = obj.USU_APELLIDO;
                     cmd.Parameters.Add("USU_TELEFONO_RES", OracleDbType.Varchar2).Value = obj.USU_TELEFONO_RES;
                     cmd.Parameters.Add("USU_TELEFONO_CEL", OracleDbType.Varchar2).Value = obj.USU_TELEFONO_CEL;
                     cmd.Parameters.Add("USU_DIRECCION", OracleDbType.Varchar2).Value = obj.USU_DIRECCION;
                     cmd.Parameters.Add("USU_DEPTO", OracleDbType.Varchar2).Value = obj.USU_DEPTO;
                     cmd.Parameters.Add("USU_PAIS", OracleDbType.Varchar2).Value = obj.USU_PAIS;
                     cmd.Parameters.Add("USU_PROFESION", OracleDbType.Varchar2).Value = obj.USU_PROFESION;*/

                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.                  
                        OracleCommand cmdCommit = new OracleCommand("COMMIT", con);
                        cmdCommit.ExecuteNonQuery();
                    

                }

            }
            catch (Exception ex)
            {

                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;

        }

        /*METODO PARA ACTUALIZAR USUAIROS ---------------------------------------------------------------------- byron*/
        public int Editar_Usuario(MUEB_USUARIO obj, out string Mensaje)
        {
            bool resultado = false;
            int idautogenerado = 0;
            Mensaje = string.Empty;


            try
            {
                using (OracleConnection con = new OracleConnection(Conexion.connectionString)) /*Aca intente ponerlo desde oracle, no se si sea asi*/
                {
                    OracleCommand cmd = new OracleCommand("PA_UPD_EMPLEADO", con);/*Aca llamo al procedimiento almacenado*/

                    cmd.Parameters.Add("USU_NO_DOC_PK", obj.USU_NO_DOC_PK);
                    cmd.Parameters.Add("USU_TIPO_DOC", obj.USU_TIPO_DOC);
                    cmd.Parameters.Add("USU_NOMBRE", obj.USU_NOMBRE);
                    cmd.Parameters.Add("USU_APELLIDO", obj.USU_APELLIDO);
                    cmd.Parameters.Add("USU_TELEFONO_RES", obj.USU_TELEFONO_RES);
                    cmd.Parameters.Add("USU_TELEFONO_CEL", obj.USU_TELEFONO_CEL);
                    cmd.Parameters.Add("USU_DIRECCION", obj.USU_DIRECCION);
                    cmd.Parameters.Add("USU_DEPTO", obj.USU_DEPTO);
                    cmd.Parameters.Add("USU_PAIS", obj.USU_PAIS);
                    cmd.Parameters.Add("USU_PROFESION", obj.USU_PROFESION);

                    cmd.Parameters.Add("Resultado", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                    /*CHATGPT DICE QUE ASI SE DEBE DE HACER EN ORACLE ------------------------------------------------------------------*/
                    /* cmd.Parameters.Add("USU_NO_DOC_PK", OracleDbType.Number).Value = obj.USU_NO_DOC_PK;
                     cmd.Parameters.Add("USU_TIPO_DOC", OracleDbType.Varchar2).Value = obj.USU_TIPO_DOC;
                     cmd.Parameters.Add("USU_NOMBRE", OracleDbType.Varchar2).Value = obj.USU_NOMBRE;
                     cmd.Parameters.Add("USU_APELLIDO", OracleDbType.Varchar2).Value = obj.USU_APELLIDO;
                     cmd.Parameters.Add("USU_TELEFONO_RES", OracleDbType.Varchar2).Value = obj.USU_TELEFONO_RES;
                     cmd.Parameters.Add("USU_TELEFONO_CEL", OracleDbType.Varchar2).Value = obj.USU_TELEFONO_CEL;
                     cmd.Parameters.Add("USU_DIRECCION", OracleDbType.Varchar2).Value = obj.USU_DIRECCION;
                     cmd.Parameters.Add("USU_DEPTO", OracleDbType.Varchar2).Value = obj.USU_DEPTO;
                     cmd.Parameters.Add("USU_PAIS", OracleDbType.Varchar2).Value = obj.USU_PAIS;
                     cmd.Parameters.Add("USU_PROFESION", OracleDbType.Varchar2).Value = obj.USU_PROFESION;*/

                    //Ejecuta el COMMIT para no tener los datos en la ram y se reflejen en la base de datos.                  
                    OracleCommand cmdCommit = new OracleCommand("COMMIT", con);
                    cmdCommit.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {

                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;





        }



















    }
}