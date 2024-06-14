using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Proveedor
    {

        private CD_Proveedor objCapaDato = new CD_Proveedor();
        public List<MUEB_PROVEEDOR> Listar()
        {
            return objCapaDato.Listar();

        }

        public int Registrar(MUEB_PROVEEDOR obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.PROV_NOMBRE) || string.IsNullOrEmpty(obj.PROV_TELEFONO_CEL) || string.IsNullOrEmpty(obj.PROV_TELEFONO_CEL)
                || string.IsNullOrEmpty(obj.PROV_DIRECCION) || string.IsNullOrEmpty(obj.PROV_CIUDAD) || string.IsNullOrEmpty(obj.PROV_DEPTO))
            {
                Mensaje = "No se ha podido crear el nuevo producto";
                return 0;
            }
            if (Listar().Any(u => u.PROV_ID ==obj.PROV_ID))
            {
                Mensaje = "No se ha podido crear el nuevo proveedor / proveedor ya esta registrado con el mismo id";
                return 0;
            }
            else
            {
                return objCapaDato.Registrar(obj, out Mensaje);
            }


        }


        public bool Editar(MUEB_PROVEEDOR obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.PROV_DIRECCION) || string.IsNullOrEmpty(obj.PROV_NOMBRE))
            {
                Mensaje = "La Direccion y nombre del Proveedor no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }


        public bool Eliminar(int idproveedor, out string Mensaje)
        {
            return objCapaDato.Eliminar(idproveedor, out Mensaje);
        }



        public List<MUEB_PROVEEDOR> ListarProveedorporCategoria(int idcategoria)
        {

            return objCapaDato.ListarProveedorporCategoria(idcategoria);
        }


    }
}
