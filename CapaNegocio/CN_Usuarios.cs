using System.Collections.Generic;


using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();
        public List<MUEB_USUARIO> Listar()
        {
            return objCapaDato.Listar();

        }


        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }

        public bool ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idusuario, nuevaclave, out Mensaje);

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


    }
}

