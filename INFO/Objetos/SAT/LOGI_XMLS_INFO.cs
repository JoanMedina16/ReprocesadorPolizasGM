using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.SAT
{
   public class LOGI_XMLS_INFO
    {
        public string LugarExpedicion { get; set; }
        public string MetodoPago { get; set; }
        public string TipoDeComprobante { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotalbase { get; set; }
        public string Moneda { get; set; }
        public string Certificado { get; set; }
        public string FormaPago { get; set; }
        public string CondicionesdePago { get; set; }
        public string Sello { get; set; }
        public string Fecha { get; set; }
        public string Folio { get; set; }
        public string UUID { get; set; }
        public string Serie { get; set; }
        public string Version { get; set; }
        public PersonaCFDI Emisor { get; set; }
        public PersonaCFDI Receptor { get; set; }
        public List<LOGI_ConceptosCFDI_INFO> Conceptos { get; set; }
        public ImpuestosCFDI Impuestos { get; set; }
        public string NoCertificado { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal TipoCambio { get; set; }
        public ComplementoCFDI Complemento { get; set; }
        public LOGI_CfdiRelacionadosCFDI_INFO CfdiRelacionados { get; set; }
        public String CFDIContent { get; set; }
        public bool bContent { get; set; }
        public string sFoliosIN { get; set; }
        public string sSeriesIN { get; set; }
    }
}
