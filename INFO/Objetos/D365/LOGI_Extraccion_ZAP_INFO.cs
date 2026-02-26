using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.D365
{
    public class LOGI_Extraccion_ZAP_INFO
    {
        public String diario { get; set; }
        public String documento { get; set; }
        public string RecID { get; set; }
        public string Fecha { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_final { get; set; }
        public String Display { get; set; }
        public string cuenta { get; set; }
        public string cuenta_inicio { get; set; }
        public string cuenta_fin { get; set; }
        public string sucursal { get; set; }
        public string filial { get; set; }
        public string centro { get; set; }
        public string depto { get; set; }
        public string area { get; set; }
        public string depto_distinto { get; set; }
        public string text { get; set; }
        public string vehiculo { get; set; }
        public Double debito { get; set; }
        public Double credito { get; set; }
        public string Voucher { get; set; }
        public bool DepFiscal { get; set; }
    }

    public class LOGI_Extraccion_Vehiculo_INFO
    {
        public string Vehiculo { get; set; }
        public string Descripcion { get; set; }
        public string Voucher { get; set; }
    }
}
