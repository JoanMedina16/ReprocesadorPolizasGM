using INFO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_Polizas_INFO
    {
        public String FolioAsiento { get; set; }
        public String FolioAsientoMatch { get; set; }
        public Int32 id_tipo_doc { get; set; }
        public int mes { get; set; }
        public int ano { get; set; }
        public string Nombredocumento { get; set; }
        public eDocumentoPolizas eTypedoc { get; set; }
        public string fechaContable { get; set; }
        public string fechaCreacion { get; set; }
        public string folio { get; set; }
        public string serie { get; set; }
        public string folioserie { get; set; }
        public String recIdD365 { get; set; }
        public int estatus { get; set; }
        public Decimal total { get; set; }
        public Decimal subtotal { get; set; }
        public Decimal impuesto { get; set; }
        public string docxml { get; set; }
        public string rfc { get; set; }
        public string nombrerfc { get; set; }
        public string uuid { get; set; }
        public string sDocumentosIN { get; set; }

        public string FechaConInicio { get; set; }
        public string FechaConFin { get; set; }
        public string FechaCreInicio { get; set; }
        public string FechaCreFin { get; set; }
        public int descartados { get; set; }
        public int iniciales { get; set; }

        public string doctoref { get; set; }
        public string uuidref { get; set; }
        public string comments { get; set; }
        public int errortimbrado { get; set; }

    }

    public class LOGI_Peticion_INFO
    {
        public string FechaInicial { get; set; }
        public string Proceso { get; set; }
        public string Usuario { get; set; }
    }
}
