using System.Collections.Generic;
using System.Linq;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_PRODUCTO
    {
        private CD_Producto objCapaDato = new CD_Producto();
        public List<MUEB_PRODUCTO> Listar()
        {
            return objCapaDato.Listar();

        }

        public int Registrar(MUEB_PRODUCTO obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.PROD_NOMBRE) || string.IsNullOrEmpty(obj.PROD_DESCRIPCION) || string.IsNullOrEmpty(obj.PROD_MATERIAL)
                || string.IsNullOrEmpty(obj.PROD_ALTO) || string.IsNullOrEmpty(obj.PROD_ANCHO) || string.IsNullOrEmpty(obj.PROD_PROFUNDIDAD) || string.IsNullOrEmpty(obj.PROD_COLOR) ||
                string.IsNullOrEmpty(obj.PROD_PESO))
            {
                Mensaje = "No se ha podido crear el nuevo producto";
                return 0;
            }
            MUEB_CATEGORIA oCategoria = new MUEB_CATEGORIA();
            oCategoria = new CN_Categoria().Listar().Where(c => c.CAT_ID == obj.PROD_CATEGORIA_FK).FirstOrDefault();
            if (oCategoria == null)
            {
                Mensaje = "Ingrese un ID de categoria valido";
                return 0;
            }

            MUEB_PROVEEDOR oProveedor = new MUEB_PROVEEDOR();
            oProveedor = new CN_Proveedor().Listar().Where(p => p.PROV_ID == obj.PROD_PROVEEDOR).FirstOrDefault();
            if (oProveedor == null)
            {
                Mensaje = "Ingrese un ID de proveedor valido";
                return 0;
            }

            if (Listar().Any(u => u.PROD_ID_PK == obj.PROD_ID_PK))
            {
                Mensaje = "No se ha podido crear el nuevo producto / producto ya esta registrado con el mismo id";
                return 0;
            }
            else
            {
                return objCapaDato.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(MUEB_PRODUCTO obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.PROD_NOMBRE) || string.IsNullOrEmpty(obj.PROD_DESCRIPCION) || string.IsNullOrEmpty(obj.PROD_MATERIAL)
                || string.IsNullOrEmpty(obj.PROD_ALTO) || string.IsNullOrEmpty(obj.PROD_ANCHO) || string.IsNullOrEmpty(obj.PROD_PROFUNDIDAD) || string.IsNullOrEmpty(obj.PROD_COLOR) ||
                string.IsNullOrEmpty(obj.PROD_PESO))
            {
                Mensaje = "No se ha podido editar el producto";

            }

            MUEB_CATEGORIA oCategoria = new MUEB_CATEGORIA();
            oCategoria = new CN_Categoria().Listar().Where(c => c.CAT_ID == obj.PROD_CATEGORIA_FK).FirstOrDefault();
            if(oCategoria == null)
            {
                Mensaje = "Ingrese un ID de categoria valido";
                return false;
            }

            MUEB_PROVEEDOR oProveedor = new MUEB_PROVEEDOR();
            oProveedor = new CN_Proveedor().Listar().Where(p => p.PROV_ID == obj.PROD_PROVEEDOR).FirstOrDefault();
            if(oProveedor == null)
            {
                Mensaje = "Ingrese un ID de proveedor valido";
                return false;
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                Mensaje = "No se ha podido editar el producto";
                return false;
            }

        }


        public bool GuardarDatosImagen(MUEB_PRODUCTO obj, out string Mensaje)
        {

            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }



    }
}