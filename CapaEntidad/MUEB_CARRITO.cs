using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_CARRITO
    {
        public int CAR_ID { get; set; }

        public int CAR_CLI_ID_FK { get; set; }

        public int CAR_PROD_ID_FK { get; set; }
        public int CAR_CANTIDAD { get; set; }

        public MUEB_PRODUCTO oProducto{ get; set; }



    }

}
