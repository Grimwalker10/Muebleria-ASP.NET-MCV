using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize] //Hace que no se muestren las vistas si no esta autorizado.
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        public ActionResult Proveedor()
        {
            return View();
        }

        public ActionResult Cliente()
        {
            return View();
        }

        //Metodos de Productos -----------------------------------------------------------------------------------------------------
        [HttpGet]
        public JsonResult ListarProductos(MUEB_PRODUCTO entidad)
        {

            List<MUEB_PRODUCTO> oLista = new List<MUEB_PRODUCTO>();
            oLista = new CN_PRODUCTO().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {

            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            MUEB_PRODUCTO oProducto = new MUEB_PRODUCTO();
            oProducto = JsonConvert.DeserializeObject<MUEB_PRODUCTO>(objeto);

            if (oProducto.PROD_ID_PK == 0)
            {
                int idProductoGenerado = new CN_PRODUCTO().Registrar(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.PROD_ID_PK = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                MUEB_PRODUCTO Producto = new CN_PRODUCTO().Listar().Where(p => p.PROD_ID_PK == oProducto.PROD_ID_PK).FirstOrDefault();
                if(Producto != null)
                {
                    if(archivoImagen == null)
                    {
                        oProducto.PROD_REF_FOTO = Producto.PROD_REF_FOTO;
                        oProducto.PROD_EXTENSION = Producto.PROD_EXTENSION;
                        oProducto.PROD_NOMBRE_IMAGEN = Producto.PROD_NOMBRE_IMAGEN;

                    }
                    operacion_exitosa = new CN_PRODUCTO().Editar(oProducto, out mensaje);
                }
                else
                {
                    operacion_exitosa=false;
                }
                
            }


            if (operacion_exitosa)
            {

                if (archivoImagen != null)
                {

                    string ruta_guardar = "C:/FotosWeb";
                    //string ruta_guardar = "//192.168.1.5/C:/FotosWeb";
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.PROD_ID_PK.ToString(), extension);


                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));

                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }


                    if (guardar_imagen_exito)
                    {

                        oProducto.PROD_REF_FOTO = ruta_guardar;
                        oProducto.PROD_NOMBRE_IMAGEN = nombre_imagen;
                        bool rspta = new CN_PRODUCTO().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {

                        mensaje = "Se guardaro el producto pero hubo problemas con la imagen";
                    }


                }
            }




            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.PROD_ID_PK, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {

            bool conversion;
            MUEB_PRODUCTO oproducto = new CN_PRODUCTO().Listar().Where(p => p.PROD_ID_PK == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.PROD_REF_FOTO, oproducto.PROD_NOMBRE_IMAGEN), out conversion);


            return Json(new
            {
                conversion = conversion,
                textobase64 = textoBase64,
                extension = Path.GetExtension(oproducto.PROD_NOMBRE_IMAGEN)

            },
             JsonRequestBehavior.AllowGet
            );


        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_PRODUCTO().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }




        // Metodos de Categoria --------------------------------------------------------------------------------------------------------
        [HttpGet]
        public JsonResult ListarCategorias(MUEB_CATEGORIA entidad)
        {

            List<MUEB_CATEGORIA> oLista = new List<MUEB_CATEGORIA>();
            oLista = new CN_Categoria().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(MUEB_CATEGORIA objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.CAT_ID == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        //Metodos de Proveedor --------------------------------------------------------------------------------------------------------
        [HttpGet]
        public JsonResult ListarProveedores(MUEB_PROVEEDOR entidad)
        {

            List<MUEB_PROVEEDOR> oLista = new List<MUEB_PROVEEDOR>();
            oLista = new CN_Proveedor().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProveedor(MUEB_PROVEEDOR objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.PROV_ID == 0)
            {
                resultado = new CN_Proveedor().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Proveedor().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult EliminarProveedor(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Proveedor().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        //Metodos de Cliente --------------------------------------------------------------------------------------------------------
        [HttpGet]
        public JsonResult ListarClientes(MUEB_CLIENTE entidad)
        {

            List<MUEB_CLIENTE> oLista = new List<MUEB_CLIENTE>();
            oLista = new CN_Cliente().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCliente(MUEB_CLIENTE objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.CLI_ID_PK == 0)
            {
                resultado = new CN_Cliente().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Cliente().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult EliminarCliente(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Cliente().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


    }
}