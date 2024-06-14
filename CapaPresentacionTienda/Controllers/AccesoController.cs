using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.Web.Security;
using Microsoft.Ajax.Utilities;

namespace CapaPresentacionAdmin.Controllers
{
    //cliente-----------------------------------------------------------------------------------
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Confirmar_CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult Confirmar_Reestablecer()
        {
            return View();
        }
        //REGISTRA---------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult Registrar(MUEB_CLIENTE objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["CLI_NOMBRE"] = string.IsNullOrEmpty(objeto.CLI_NOMBRE) ? "" : objeto.CLI_NOMBRE;
            ViewData["CLI_APELLIDO"] = string.IsNullOrEmpty(objeto.CLI_APELLIDO) ? "" : objeto.CLI_APELLIDO;
            ViewData["CLI_CORREO"] = string.IsNullOrEmpty(objeto.CLI_CORREO) ? "" : objeto.CLI_CORREO;

            if (objeto.CLI_CLAVE != objeto.CLI_CONFIRMARCLAVE)
            {

                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }


            resultado = new CN_Cliente().Registrar(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }
        //LOGIN---------------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            MUEB_CLIENTE oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(item => item.CLI_CORREO == correo && item.CLI_CLAVE == clave).FirstOrDefault();


            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();

            }
            else
            {

                if (oCliente.CLI_REESTABLECER)
                {
                    TempData["IdCliente"] = oCliente.CLI_ID_PK;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {

                    FormsAuthentication.SetAuthCookie(oCliente.CLI_CORREO, false);

                    Session["Cliente"] = oCliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");


                }

            }
        }

        //REESTABLECER---------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {


            MUEB_CLIENTE cliente = new MUEB_CLIENTE();

            cliente = new CN_Cliente().Listar().Where(item => item.CLI_CORREO == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado a ese correo";
                return View();
            }


            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.CLI_ID_PK, correo, out mensaje);

            if (respuesta)
            {

                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }
            else
            {

                ViewBag.Error = mensaje;
                return View();
            }


        }


        //CAMBIAR CLAVE---------------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult CambiarClave(string correo, string clave, string nuevaclave, string confirmarclave)
        {

            if (correo.IsNullOrWhiteSpace())
            {
                ViewBag.Error = "Porfavor complete todos los campos.";
                return View();
            }

            MUEB_CLIENTE oCLIENTE = new MUEB_CLIENTE();

            oCLIENTE = new CN_Cliente().Listar().Where(u => u.CLI_CORREO == correo).FirstOrDefault();


            if (oCLIENTE == null)
            {
                ViewBag.Error = "Correo no correcto o registrado.";
                return View();
            }
            else
            {
                int idCLIENTE = oCLIENTE.CLI_ID_PK;

                if (!clave.Equals(oCLIENTE.CLI_CLAVE))
                {
                    ViewBag.Error = "La contraseña no coincide con el correo del CLIENTE ingresado!";
                    return View();
                }

                else if (!nuevaclave.Equals(confirmarclave) || clave.IsNullOrWhiteSpace() || nuevaclave.IsNullOrWhiteSpace() || confirmarclave.IsNullOrWhiteSpace())
                {
                    ViewBag.Error = "La contraseña no coincide!";
                    return View();
                }
                else
                {
                    ViewBag.Error = null;

                    string mensaje = string.Empty;


                    bool respuesta = new CN_Cliente().CambiarClave(idCLIENTE, nuevaclave, out mensaje);

                    if (respuesta)
                    {
                        ViewBag.Error = null;
                        return RedirectToAction("Confirmar_CambiarClave");
                    }
                    else
                    {
                        ViewBag.Error = "No se ha podido cambiar la clave :( ";
                        return View();
                    }



                }

            }

        }


        //CERRAR ---------------------------------------------------------------------------------------------------------------------

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }


    }
}