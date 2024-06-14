using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_VENTA
    {
        public int VEN_ID_PK { get; set; }
        public int VEN_ID_CLIENTE_FK { get; set; }
        public int VEN_TOTALPROD { get; set; }
        public decimal VEN_MONTOTOTAL { get; set; }
        public string VEN_CONTACTO { get; set; }
        public string VEN_TELEFONO { get; set; }
        public string VEN_DIRECCION { get; set; }
        public string VEN_FECHA_VENTA { get; set; }
      
        public List<MUEB_DETVENTA> oDetalleVenta { get; set; }




    }

}
