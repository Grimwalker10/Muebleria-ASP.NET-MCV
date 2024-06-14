using System.Collections.Generic;
using System.Linq;
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


        public int Registrar(MUEB_USUARIO obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.USU_TIPO_DOC) || string.IsNullOrEmpty(obj.USU_NOMBRE) || string.IsNullOrEmpty(obj.USU_DIRECCION) || string.IsNullOrEmpty(obj.USU_CIUDAD) 
                || string.IsNullOrEmpty(obj.USU_DEPTO) || string.IsNullOrEmpty(obj.USU_PAIS) || string.IsNullOrEmpty(obj.USU_PROFESION) || string.IsNullOrEmpty(obj.USU_CORREO))
            {
                Mensaje = "No se ha podido crear el nuevo usuario (capa Negocio)";
                return 0;
            }
            if(Listar().Any(u => u.USU_NO_DOC == obj.USU_NO_DOC || u.USU_ID_PK == obj.USU_ID_PK))
            {
                Mensaje = "No se ha podido crear el nuevo usuario/ usuario ya registrado con el mismo id o numero de documento.";
                return 0;
            }
            else
            {
                string clave = "default123";
                obj.USU_CLAVE = clave;
                return objCapaDato.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(MUEB_USUARIO obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.USU_TIPO_DOC) || string.IsNullOrEmpty(obj.USU_NOMBRE) || string.IsNullOrEmpty(obj.USU_DIRECCION) || string.IsNullOrEmpty(obj.USU_CIUDAD)
                || string.IsNullOrEmpty(obj.USU_DEPTO) || string.IsNullOrEmpty(obj.USU_PAIS) || string.IsNullOrEmpty(obj.USU_PROFESION) || string.IsNullOrEmpty(obj.USU_CORREO) ||
                string.IsNullOrEmpty(obj.USU_CLAVE))
            {
                Mensaje = "No se ha podido editar el empleado";
                
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                Mensaje = "No se ha podido crear empleado";
                return false;
            }

        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
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

