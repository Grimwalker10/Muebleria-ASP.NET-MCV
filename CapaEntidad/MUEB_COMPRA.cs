using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_COMPRA
    {
        public int COM_ID_PK { get; set; }
        public DateTime COM_FECHA_PEDIDO { get; set; }
        public DateTime COM_FECHA_ENTREGA { get; set; }
        public int COM_TOTAL { get; set; }
    }
}

