using INFO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_PolizasD365_INFO
    {
        public int IdRegistro { get; set; }
        public String folio { get; set; }
        public String mensaje { get; set; }
        public String socio { get; set; }
        public String nombresocio { get; set; }
        public String rfc { get; set; }
        public string factura { get; set; }
        public string uuid { get; set; }
        public string facturaref { get; set; }
        public string uuidref { get; set; }
        public DateTime fechaconta { get; set; }
        public DateTime fecharegistro { get; set; }
        public Double impuesto { get; set; }
        public Decimal subtotal { get; set; }
        public Decimal total { get; set; }
        public long recid { get; set; }
        public int estatus { get; set; }
        public eDocumentoTMS tipo { get; set; }
        public String JSONPATH { get; set; }
 public string URL { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }

        public int errores { get; set; }
        public int enviados { get; set; }
        public int enviado { get; set; }
    }
}
