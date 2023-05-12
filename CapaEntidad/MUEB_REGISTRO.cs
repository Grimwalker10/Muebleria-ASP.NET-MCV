using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MUEB_REGISTRO
    {
        public int REG_ID { get; set; }
        public int REG_USU_ID_FK { get; set; }
        public string REG_PROD_ID_FK { get; set; }
        public TimeSpan REG_FECHA { get; set; }

        public MUEB_USUARIO USU_NO_DOC_PK { get; set; }
        public MUEB_PRODUCTO PROD_ID_PK { get; set; }
    }
}
