using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDato = new CD_Cliente();
        public List<MUEB_CLIENTE> Listar()
        {
            return objCapaDato.Listar();

        }


        public int Registrar(MUEB_CLIENTE obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.CLI_NOMBRE) || string.IsNullOrEmpty(obj.CLI_APELLIDO))
            {
                Mensaje = "No se ha podido crear el nuevo producto";
                return 0;
            }

            if (Listar().Any(u => u.CLI_ID_PK == obj.CLI_ID_PK))
            {
                Mensaje = "No se ha podido crear el nuevo cliente / cliente ya esta registrado con el mismo id";
                return 0;
            }
            else
            {
                return objCapaDato.Registrar(obj, out Mensaje);
            }


        }


        public bool Editar(MUEB_CLIENTE obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.CLI_NOMBRE) || string.IsNullOrEmpty(obj.CLI_APELLIDO) || string.IsNullOrEmpty(obj.CLI_CIUDAD)
                || string.IsNullOrEmpty(obj.CLI_CORREO) || string.IsNullOrEmpty(obj.CLI_DEPTO) || string.IsNullOrEmpty(obj.CLI_DIRECCION) || string.IsNullOrEmpty(obj.CLI_PAIS) ||
                string.IsNullOrEmpty(obj.CLI_PROFESION) || string.IsNullOrEmpty(obj.CLI_TIPO_DOC))
            {
                Mensaje = "No se ha podido editar el cliente";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                Mensaje = "No se ha podido editar el cliente";
                return false;
            }

        }



        //METODO DE CAMBIAR CLAVE  DE CLIENTE------------------------------------------------------------------------------------------------
        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }



        //METODO DE REESTABLECER CLAVE DE CLIENTE------------------------------------------------------------------------------------------------


        public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idcliente, nuevaclave, out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave! </p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }

        }





        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }


    }
}
