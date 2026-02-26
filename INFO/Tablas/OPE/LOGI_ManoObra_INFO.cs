using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.OPE
{
   public class LOGI_ManoObra_INFO
    {
        public int cia { set; get; }
        public int concepto { get; set; }
        public DateTime fecha { get; set; }
        public Decimal importe { get; set; }
        public int empleado { get; set; }
        public string folio { get; set; }

    }
}
