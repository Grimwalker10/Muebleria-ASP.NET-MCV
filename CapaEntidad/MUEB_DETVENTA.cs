using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_DETVENTA
    {

        public int DETVEN_ID_PK { get; set; }
        public int DETVEN_ID_VENTA_FK { get; set; }
        public MUEB_PRODUCTO oProducto { get; set; }

        public int DETVEN_CANTIDAD { get; set; }
        public decimal DETVEN_TOTAL { get; set; }

     



    }

}
