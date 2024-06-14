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
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
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


        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            MUEB_USUARIO oUsuario = new MUEB_USUARIO();

            oUsuario = new CN_Usuarios().Listar().Where(u =>  u.USU_CORREO == correo && u.USU_CLAVE == clave).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(oUsuario.USU_CORREO, false);
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult CambiarClave(string correo, string clave, string nuevaclave, string confirmarclave)
        {

            if (correo.IsNullOrWhiteSpace())
            {
                ViewBag.Error = "Porfavor complete todos los campos.";
                return View();
            }

            MUEB_USUARIO oUsuario = new MUEB_USUARIO();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.USU_CORREO == correo).FirstOrDefault();
            

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo no correcto o registrado.";
                return View();
            }
            else
            {
                int idusuario = oUsuario.USU_ID_PK;

                if (!clave.Equals(oUsuario.USU_CLAVE))
                {
                    ViewBag.Error = "La contraseña no coincide con el correo del usuario ingresado!";
                    return View();
                }

                else if (!nuevaclave.Equals(confirmarclave) || clave.IsNullOrWhiteSpace() || nuevaclave.IsNullOrWhiteSpace() || confirmarclave.IsNullOrWhiteSpace())
                {
                    ViewBag.Error = "La contraseña no coincide!";
                    return View();
                }
                else{
                    ViewBag.Error = null;

                    string mensaje = string.Empty;


                    bool respuesta = new CN_Usuarios().CambiarClave(idusuario, nuevaclave, out mensaje);

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

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            MUEB_USUARIO oUsuario = new MUEB_USUARIO();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.USU_CORREO == correo).FirstOrDefault();

            if (oUsuario == null)
            {
                if (correo.IsNullOrWhiteSpace())
                {
                    ViewBag.Error = "Porfavor llene el campo requerido";
                    return View();
                }
                else
                {
                    ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                    return View();
                }

            }
            else
            {  
                string mensaje = string.Empty;
                int idusuario = oUsuario.USU_ID_PK;
                bool respuesta = new CN_Usuarios().ReestablecerClave(idusuario, correo, out mensaje);

                if (respuesta)
                {
                    ViewBag.Error = null;
                    return RedirectToAction("Confirmar_Reestablecer");
                }
                else
                {
                    ViewBag.Error = "No se ha podido reestablecer la clave :(";
                    return View();
                }


            }
            
        }



        public ActionResult CerrarSesion()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }


    }
}