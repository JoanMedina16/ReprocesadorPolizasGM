using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.SAT
{
    public class LOGI_CfdiRelacionadosCFDI_INFO
    {
        public List<LOGI_DocumentoRelacionadoCFDI_INFO> CfdiRelacionados { get; set; }
        public string TipoRelacion { get; set; }
    }
    public class LOGI_DocumentoRelacionadoCFDI_INFO
    {
        public string UUID { get; set; }
    }
    public class InformacionAduaneraConceptoCFDI
    {

        public string NumeroPedimento { get; set; }
    }
    public class PersonaCFDI
    {

        public string Calle { get; set; }
        public string CodigoPostal { get; set; }
        public string Curp { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Nombre { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string NumRegIdTrib { get; set; }
        public string Pais { get; set; }
        public string RegimenFiscal { get; set; }
        public string ResidenciaFiscal { get; set; }
        public string Rfc { get; set; }
        public string UsoCFDI { get; set; }
    }


    public class ComplementoCFDI
    {
        public PagosCFDI Pagos { get; set; }
        public TimbreFiscalCFDI TimbreFiscalDigital { get; set; }
    }
    public class PagosCFDI
    {

        public List<PagoCFDI> Pago { get; set; }
        public string Version { get; set; }
    }
    public class PagoCFDI
    {

        public string CtaBeneficiario { get; set; }
        public string CtaOrdenante { get; set; }
        public List<DocumentoPagoRelacionadoCFDI> DocumentosRelacionados { get; set; }
        public string FechaPago { get; set; }
        public string FormaDePagoP { get; set; }
        public ImpuestosCFDI Impuestos { get; set; }
        public string MonedaP { get; set; }
        public decimal Monto { get; set; }
        public string RfcEmisorCtaBen { get; set; }
        public string RfcEmisorCtaOrd { get; set; }
        public decimal TipoCambioP { get; set; }
        public string Version { get; set; }
    }
    public class DocumentoPagoRelacionadoCFDI
    {

        public string Folio { get; set; }
        public string IdDocumento { get; set; }
        public decimal ImpPagado { get; set; }
        public decimal ImpSaldoAnt { get; set; }
        public decimal ImpSaldoInsoluto { get; set; }
        public string MetodoDePagoDR { get; set; }
        public string MonedaDR { get; set; }
        public int NumParcialidad { get; set; }
        public string Serie { get; set; }
        public decimal TipoCambioDR { get; set; }
    }
    public class ImpuestosCFDI
    {
        public List<RetencionCFDI> Retenciones { get; set; }
        public decimal TotalImpuestosRetenidos { get; set; }
        public decimal TotalImpuestosTrasladados { get; set; }
        public List<TrasladoCFDI> Traslados { get; set; }
    }
    public class TrasladoCFDI
    {

        public decimal Importe { get; set; }
        public string Impuesto { get; set; }
        public decimal TasaOCuota { get; set; }
        public string TipoFactor { get; set; }
    }

    public class RetencionCFDI
    {
        public decimal Importe { get; set; }
        public string Impuesto { get; set; }
    }

    public class ImpuestoConceptoCFDI
    {
        public decimal Base { get; set; }
        public decimal Importe { get; set; }
        public string Impuesto { get; set; }
        public decimal TasaOCuota { get; set; }
        public string TipoFactor { get; set; }
        public decimal ValorUnitario { get; set; }
    }

    public class TimbreFiscalCFDI
    {
        public string FechaTimbrado { get; set; }
        public string NoCertificadoSAT { get; set; }
        public string RfcProvCertif { get; set; }
        public string SelloCFD { get; set; }
        public string SelloSAT { get; set; }
        public string UUID { get; set; }
        public string Version { get; set; }
    }
}
