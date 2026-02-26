using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.D365
{
    public class LOGI_Estadocuentas_INFO
    {
        public string descripcion { get; set; }
        public string cuenta_inicio { get; set; }
        public string cuenta_fin { get; set; }
        public string cuentas_filtro { get; set; }
        public bool negativo { get; set; }
        public string departamento { get; set; }

        public string descarte_deptos { get; set; }
        public string area { get; set; }
        public bool DepFiscal { get; set; }
    }
}
