using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_Polizas_detalle_INFO
    {

        public String FolioAsiento { get; set; }
        public int linea { get; set; }
        public int mayor { get; set; }
        public int cuenta { get; set; }
        public int scuenta { get; set; }
        public string cuenta_AX { get; set; }
        public string GrupoImpuesto { get; set; }
        public string ImpuestoArt { get; set; }
        public Decimal cargo { get; set; }
        public Decimal abono { get; set; }
        public Decimal importe { get; set; }
        public string sucursal { get; set; }
        public string sucursal_D365 { get; set; }
        public string centrocosto { get; set; }
        public string centrocosto_D365 { get; set; }
        public string departamento { get; set; }
        public string departamento_D365 { get; set; }
        public string area { get; set; }
        public string area_D365 { get; set; }
        public string vehiculo { get; set; }
        public string vehiculo_D365 { get; set; }
        public string filialtercero_D365 { get; set; }
        public string descrip { get; set; }
        public String XML { get; set; }
        public string refdoc { get; set; }
        public string mensaje { get; set; }
        public int valido { get; set; }
        public int tipo_documento { get; set; }
        public string uuid { get; set; }
        public int usaxml { get; set; }

        public int IVA16 { get; set; }
        public int esfrontera { get; set; }
        public int esdeducible { get; set; }
        public int usaiva { get; set; }
    }
}
