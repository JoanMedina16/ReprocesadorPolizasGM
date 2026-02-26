using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.MOTF
{
    public class LOGI_PorteViaje_INFO
    {
        public Int32 PorteId { get; set; }
        public string Fechacreacion { get; set; }
        public Decimal Subtotal { get; set; }
        public Decimal Total { get; set; }
        public string Folioviaje { get; set; }
        public int cuentaclie { get; set; }
        public int subclie { get; set; }
        public string Cliente { get; set; }
        public int cuentaori { get; set; }
        public string Origen { get; set; }
        public int cuentadest { get; set; }
        public string Destino { get; set; }
        public int cuentaoper { get; set; }
        public string Operador { get; set; }
        public int facturable { get; set; }
    }
}
