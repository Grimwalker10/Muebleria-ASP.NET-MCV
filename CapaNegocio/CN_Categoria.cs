using System.Collections.Generic;
using System.Linq;
using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDato = new CD_Categoria();
        public List<MUEB_CATEGORIA> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(MUEB_CATEGORIA obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.CAT_DESCRIPCION))
            {
                Mensaje = "La Descripcion de la Categoria no puede ser vacio";
            }



            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {

                return 0;
            }



        }

        public bool Editar(MUEB_CATEGORIA obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.CAT_DESCRIPCION))
            {
                Mensaje = "La Descripcion de la Categoria no puede ser vacio";
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



        public bool Eliminar(int idcategoria, out string Mensaje)
        {
            return objCapaDato.Eliminar(idcategoria, out Mensaje);
        }



    }
}
