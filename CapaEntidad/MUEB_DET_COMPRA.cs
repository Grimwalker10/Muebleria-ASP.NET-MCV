using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_DET_COMPRA
    {
        public int DETCOM_ID_FK { get; set; }
        public string DETCOM_ID_PRODUCTO_FK { get; set; }
        public int DETCOM_CANTIDAD { get; set; }
        public string PROD_TIPO { get; set; }

        public MUEB_COMPRA COM_ID_PK { get; set; }
        public MUEB_PRODUCTO PROD_ID_PK { get; set; }
    }
}
