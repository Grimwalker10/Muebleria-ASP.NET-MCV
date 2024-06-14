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
using System.Collections;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize] //Hace que no se muestren las vistas si no esta autorizado. --Diego 
    public class HomeController : Controller
    {

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
            ViewBag.Title = "EMPLEADO";
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios(MUEB_USUARIO entidad)
        {

            List<MUEB_USUARIO> oLista = new List<MUEB_USUARIO>();
            oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult GuardarUsuario(MUEB_USUARIO objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.USU_ID_PK == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            List<MUEB_REPORTE> oLista = new List<MUEB_REPORTE>();

            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetSummaryData()
        {
            int clientes = new CN_Cliente().Listar().Count(); // Función para obtener la cantidad de clientes desde tu fuente de datos
            int productos = new CN_PRODUCTO().Listar().Count(); // Función para obtener la cantidad de productos desde tu fuente de datos
            int empleados = new CN_Usuarios().Listar().Count(); // Función para obtener la cantidad de empleados desde tu fuente de datos

            var data = new
            {
                clientes = clientes,
                productos = productos,
                empleados = empleados
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<MUEB_REPORTE> oLista = new List<MUEB_REPORTE>();

            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            // Crear el documento PDF
            Document document = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();



            // Crear una tabla para los datos
            PdfPTable table = new PdfPTable(7);

            // Agregar las cabeceras de columna
            table.AddCell("Fecha Venta");
            table.AddCell("Cliente");
            table.AddCell("Producto");
            table.AddCell("Precio");
            table.AddCell("Cantidad");
            table.AddCell("Total");
            table.AddCell("Id Venta");

            // Agregar los datos de la lista
            foreach (MUEB_REPORTE rp in oLista)
            {
                table.AddCell(rp.FechaVenta);
                table.AddCell(rp.Cliente);
                table.AddCell(rp.Producto);
                table.AddCell(rp.Precio.ToString());
                table.AddCell(rp.Cantidad.ToString());
                table.AddCell(rp.Total.ToString());
                table.AddCell(rp.IdTransaccion.ToString());
            }

            // Agregar la tabla al documento
            document.Add(table);
            document.Close();

            // Obtener los bytes del documento PDF
            byte[] pdfBytes = memoryStream.ToArray();

            // Devolver el archivo PDF como resultado
            return File(pdfBytes, "application/pdf", "ReporteVenta" + DateTime.Now.ToString() + ".pdf");
        }



        /*
        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<MUEB_REPORTE> oLista = new List<MUEB_REPORTE>();

            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion); 

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-GT");

            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof (string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Id Venta", typeof(int));

            foreach(MUEB_REPORTE rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }



        }*/


        [HttpPost]
        public JsonResult EliminarEmpleado(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



    }
}