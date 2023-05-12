using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_USUARIO
    {
        public string USU_TIPO_DOC { get; set; }
        public int USU_NO_DOC_PK { get; set; }
        public string USU_NOMBRE { get; set; }
        public string USU_APELLIDO { get; set; }
        public int USU_TELEFONO_RES { get; set; }
        public int USU_TELEFONO_CEL { get; set; }
        public string USU_DIRECCION { get; set; }
        public string USU_CIUDAD { get; set; }
        public string USU_DEPTO { get; set; }
        public string USU_PAIS { get; set; }
        public string USU_PROFESION { get; set; }

        /*LOS SIGUIENTES REGISTROS SE USARAN PARA EL LOGUIN*/

        public string USU_CORREO { get; set; }
        public string USU_CLAVE { get; set; }

        /*FILAS EXCLUSIVAS EN CASO QUE FUERA UN EMPLEADO EL REGISTRO
        EN CUESTION*/
        public string USU_TIPO { get; set; }
        public int USU_DETALLE_TIPO { get; set; }
        public string USU_PUESTO { get; set; }
        public string USU_SUELDO { get; set; }
     

    }
}
