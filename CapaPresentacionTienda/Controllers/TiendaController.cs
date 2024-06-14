using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

using System.IO;
using System.Text.RegularExpressions;
using CapaDatos;
using System.Web.Services.Description;
using System.Collections;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Pago()
        {
            return View();
        }



        //DETALLE PRODUCTO ------------------------------------------------------------------------------------
        public ActionResult DetalleProducto(int idproducto = 0)
        {

            MUEB_PRODUCTO oProducto = new MUEB_PRODUCTO();
            bool conversion;


            oProducto = new CN_PRODUCTO().Listar().Where(p => p.PROD_ID_PK == idproducto).FirstOrDefault();


            if (oProducto != null)
            {
                oProducto.PROD_BASE64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.PROD_REF_FOTO, oProducto.PROD_NOMBRE_IMAGEN), out conversion);
                oProducto.PROD_EXTENSION = Path.GetExtension(oProducto.PROD_NOMBRE_IMAGEN);
            }

            return View(oProducto);
        }


        //LISTAR CATEGORIAS----------------------------------------------
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<MUEB_CATEGORIA> lista = new List<MUEB_CATEGORIA>();
            lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        //LISTAR MARACA POR CATEGORUA----------------------------------------------
        [HttpPost]
        public JsonResult ListarProveedorporCategoria(int idcategoria)
        {
            List<MUEB_PROVEEDOR> lista = new List<MUEB_PROVEEDOR>();
            lista = new CN_Proveedor().ListarProveedorporCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        //LISTAR MARACA POR PRODUCTO----------------------------------------------
        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<MUEB_PRODUCTO> lista = new List<MUEB_PRODUCTO>();

            if(idcategoria == 0) {
                if(idmarca == 0)
                {
                    lista = new CD_Producto().Listar();
                }
                else
                {
                    lista = new CD_Producto().Listar().FindAll(producto => producto.PROD_PROVEEDOR == idmarca);
                }

            }
            else
            {
                if (idmarca == 0)
                {
                    lista = new CD_Producto().Listar().FindAll(producto => producto.PROD_CATEGORIA_FK == idcategoria);
                }
                else
                {
                    lista = new CD_Producto().Listar().FindAll(producto => producto.PROD_CATEGORIA_FK == idcategoria && producto.PROD_PROVEEDOR == idmarca);
                }
            }

            foreach (MUEB_PRODUCTO producto in lista)
            {
                bool conversion;
                string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(producto.PROD_REF_FOTO, producto.PROD_NOMBRE_IMAGEN), out conversion);
                producto.PROD_BASE64 = textoBase64;
            }

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;


        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {

            int idcliente = ((MUEB_CLIENTE)Session["Cliente"]).CLI_ID_PK;
            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);
            bool respuesta = false;
            string mensaje = string.Empty;
            if (existe) {
                mensaje = "El producto ya existe en el carrito";

            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((MUEB_CLIENTE)Session["Cliente"]).CLI_ID_PK;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new {cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }

       [HttpGet]
        public JsonResult ListarProductosCarrito()
        {

            int idcliente = ((MUEB_CLIENTE)Session["Cliente"]).CLI_ID_PK;
            

            List<MUEB_CARRITO> oLista = new List<MUEB_CARRITO>();
            bool conversion;

            oLista = new CN_Carrito().ListarProducto(idcliente).Select(oc => new MUEB_CARRITO()
            {
                oProducto = new MUEB_PRODUCTO()
                {
                    PROD_ID_PK = oc.oProducto.PROD_ID_PK, 
                    PROD_NOMBRE = oc.oProducto.PROD_NOMBRE,                 
                    PROD_PRECIO = oc.oProducto.PROD_PRECIO,
                    PROD_REF_FOTO = oc.oProducto.PROD_REF_FOTO,
                    PROD_BASE64 = CN_Recursos.ConvertirBase64( Path.Combine( oc.oProducto.PROD_REF_FOTO, oc.oProducto.PROD_NOMBRE_IMAGEN), out conversion),
                    PROD_EXTENSION = Path.GetExtension(oc.oProducto.PROD_NOMBRE_IMAGEN)


        },
                CAR_CANTIDAD = oc.CAR_CANTIDAD

            }).ToList();
            var jsonResult = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



        [HttpPost]
        public JsonResult OperecionCarrito(int idproducto, bool sumar)
        {

            int idcliente = ((MUEB_CLIENTE)Session["Cliente"]).CLI_ID_PK;
            
            bool respuesta = false;

            string mensaje = string.Empty;
            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((MUEB_CLIENTE)Session["Cliente"]).CLI_ID_PK;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }


        public ActionResult Carrito() { 
            return View();
        }


        [HttpPost]
        public JsonResult ProcesarPago(List<MUEB_CARRITO> oListarCarrito, MUEB_VENTA  oVenta)
        {
            decimal total = 0;
            DataTable detale_venta = new DataTable();
            detale_venta.Locale = new CultureInfo("es-PE");
            detale_venta.Columns.Add("DETVEN_ID_PRODUCTO", typeof(string));
            detale_venta.Columns.Add("DETVEN_CANTIDAD", typeof(int));
            detale_venta.Columns.Add("DETVEN_TOTAL", typeof(int));// ESTO LO SACA DEL TYPE


            foreach(MUEB_CARRITO oCarrito in oListarCarrito)
            {
                decimal subtotal = Convert.ToInt32(oCarrito.CAR_CANTIDAD.ToString()) * oCarrito.oProducto.PROD_PRECIO;
                total += subtotal;
                detale_venta.Rows.Add(new object[]{
                    oCarrito.oProducto.PROD_ID_PK,
                     oCarrito.CAR_CANTIDAD,
                     subtotal
                });
            }
            oVenta.VEN_MONTOTOTAL = total;
            oVenta.VEN_ID_CLIENTE_FK = ((MUEB_CLIENTE)Session["Cliente"]).CLI_ID_PK;
            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detale_venta;

            var jsonResult = Json(new { Status = true, Link = "/Tienda/hola" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }




        [HttpPost]
        public JsonResult ProcesarPago1(List<string> oListarCarrito)
        {

            var jsonResult = Json(new { Status = true, Link = "ok" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



        [HttpPost]
        public JsonResult ProcesarPago3(List<MUEB_CARRITO> oListarCarrito, MUEB_VENTA oVenta)
        {
            try
            {
                // Lógica para procesar el pago y realizar otras operaciones

                // Ejemplo de respuesta exitosa
                var response = new { Status = true, Link = "/exito" };
                return Json(response);
            }
            catch (Exception ex)
            {
                // Manejo de errores y respuesta de error
                var errorResponse = new { Status = false, ErrorMessage = ex.Message };
                return Json(errorResponse);
            }
        }


        //--------------------LA PAGACION-------------------------------------
        public async Task<ActionResult> PagoEfectuado()
        {
            string idtelefono = Request.QueryString["IdTransaccion"];
            bool status = Convert.ToBoolean( Request.QueryString["status"]);
            ViewData["Status"] = status;

            if (status)
            {
                MUEB_VENTA oVenta = (MUEB_VENTA)TempData["Venta"];
                DataTable detale_venta = (DataTable)TempData["DetalleVenta"];
                oVenta.VEN_TELEFONO = idtelefono;
                string mensaje= string.Empty;
                bool respuesta = new CN_Venta().Registrar(oVenta, detale_venta, out mensaje);
                ViewData["IdTransaccion"] = oVenta.VEN_TELEFONO;

            }
            return View();


        }














    }
}