using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_PRODUCTO
    {
        public string PROD_ID_PK { get; set; }
        public string PROD_NOMBRE { get; set; }
        public string PROD_DESCRIPCION { get; set; }
        public string PROD_TIPO { get; set; }
        public string PROD_MATERIAL { get; set; }
        public string PROD_ALTO { get; set; }
        public string PROD_ANCHO { get; set; }
        public string PROD_PROFUNDIDAD { get; set; }
        public string PROD_COLOR { get; set; }
        public string PROD_PESO { get; set; }
        public string PROD_PROVEEDOR { get; set; }

        /*LA FOTO SERA UNA REFERENCIA SE DEBERA GUARDAR EN UNA CARPETA DENTRO DEL SERVIDOR
   INTERNO GENERADO EN EL PROYECTO*/

        public string PROD_REF_FOTO { get; set; }
        public int PROD_PRECIO { get; set; }
        public int PROD_UNIDADES { get; set; }

        


    }
}

