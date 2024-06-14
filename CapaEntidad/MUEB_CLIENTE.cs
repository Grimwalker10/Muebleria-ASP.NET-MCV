using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_CLIENTE
    {
        public int CLI_ID_PK { get; set; }
        public string CLI_TIPO_DOC { get; set; }

        public string CLI_NO_DOC { get; set; }

        public string CLI_NOMBRE { get; set; }

        public string CLI_APELLIDO { get; set; }

        public string CLI_TELEFONO_RES { get; set; }

        public string CLI_TELEFONO_CEL { get; set; }

        public string CLI_DIRECCION { get; set; }

        public string CLI_CIUDAD { get; set; }

        public string CLI_DEPTO { get; set; }

        public string CLI_PAIS { get; set; }

        public string CLI_PROFESION { get; set; }

        public string CLI_CORREO { get; set; }

        public string CLI_CLAVE { get; set; }

        public string CLI_CONFIRMARCLAVE { get; set;}

        public bool CLI_REESTABLECER { get; set; }
    }
}
