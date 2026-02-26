using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365API.INFOD
{

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarFacturaCxPContract", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public class LOGI_ContratoD365_INFO : XppObjectBase
    {
        //public LOGI_DocumentD365_INFO EncabezadoField { get; set; }
        //public List<LOGI_DocumentLineD365_INFO> Lineas { get; set; }


        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private LOGI_DocumentD365_INFO EncabezadoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private List<LOGI_DocumentLineD365_INFO> LineasField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public LOGI_DocumentD365_INFO Encabezado
        {
            get
            {
                return this.EncabezadoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EncabezadoField, value) != true))
                {
                    this.EncabezadoField = value;
                    this.RaisePropertyChanged("Encabezado");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<LOGI_DocumentLineD365_INFO> Lineas
        {
            get
            {
                return this.LineasField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LineasField, value) != true))
                {
                    this.LineasField = value;
                    this.RaisePropertyChanged("Lineas");
                }
            }
        }
    }


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ProxyBase", Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.AX.KernelInterop")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(XppObjectBase))]
    public partial class ProxyBase : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "XppObjectBase", Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.Ax.Xpp")]
    [System.SerializableAttribute()]
    public partial class XppObjectBase : ProxyBase
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarFacturaCxPContractEnc", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public class LOGI_DocumentD365_INFO : XppObjectBase
    {
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AprobadoPorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BancoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodProveedorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CompaniaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CondPagoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescripcionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DesctoPagoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimAreaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimCanalField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimCecoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimCucField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimDeptoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimFilTerField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimLinProdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimParticipableField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimSkuField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimSucursalField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DoctoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EspePagoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FacturaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaDoctoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaFacturaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaVencimientoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaTransField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int FolioField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FormaPagoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long IdFactReferenciaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdRegistroField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NoYes ImpuestosIncField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MonedaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreDiarioField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PerfilAsientoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RFCField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SerieField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NoYes SinComprobanteField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SistemaOrigen SistemaOrigenField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal SubtotalField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TextoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal TipoCambioField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBITipoFact TipoFactField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal TotalField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UUIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string XMLFacturaField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AprobadoPor
        {
            get
            {
                return this.AprobadoPorField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AprobadoPorField, value) != true))
                {
                    this.AprobadoPorField = value;
                    this.RaisePropertyChanged("AprobadoPor");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Banco
        {
            get
            {
                return this.BancoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BancoField, value) != true))
                {
                    this.BancoField = value;
                    this.RaisePropertyChanged("Banco");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CodProveedor
        {
            get
            {
                return this.CodProveedorField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CodProveedorField, value) != true))
                {
                    this.CodProveedorField = value;
                    this.RaisePropertyChanged("CodProveedor");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Compania
        {
            get
            {
                return this.CompaniaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CompaniaField, value) != true))
                {
                    this.CompaniaField = value;
                    this.RaisePropertyChanged("Compania");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CondPago
        {
            get
            {
                return this.CondPagoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CondPagoField, value) != true))
                {
                    this.CondPagoField = value;
                    this.RaisePropertyChanged("CondPago");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Descripcion
        {
            get
            {
                return this.DescripcionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DescripcionField, value) != true))
                {
                    this.DescripcionField = value;
                    this.RaisePropertyChanged("Descripcion");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DesctoPago
        {
            get
            {
                return this.DesctoPagoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DesctoPagoField, value) != true))
                {
                    this.DesctoPagoField = value;
                    this.RaisePropertyChanged("DesctoPago");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimArea
        {
            get
            {
                return this.DimAreaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimAreaField, value) != true))
                {
                    this.DimAreaField = value;
                    this.RaisePropertyChanged("DimArea");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimCanal
        {
            get
            {
                return this.DimCanalField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimCanalField, value) != true))
                {
                    this.DimCanalField = value;
                    this.RaisePropertyChanged("DimCanal");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimCeco
        {
            get
            {
                return this.DimCecoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimCecoField, value) != true))
                {
                    this.DimCecoField = value;
                    this.RaisePropertyChanged("DimCeco");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimCuc
        {
            get
            {
                return this.DimCucField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimCucField, value) != true))
                {
                    this.DimCucField = value;
                    this.RaisePropertyChanged("DimCuc");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimDepto
        {
            get
            {
                return this.DimDeptoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimDeptoField, value) != true))
                {
                    this.DimDeptoField = value;
                    this.RaisePropertyChanged("DimDepto");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimFilTer
        {
            get
            {
                return this.DimFilTerField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimFilTerField, value) != true))
                {
                    this.DimFilTerField = value;
                    this.RaisePropertyChanged("DimFilTer");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimLinProd
        {
            get
            {
                return this.DimLinProdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimLinProdField, value) != true))
                {
                    this.DimLinProdField = value;
                    this.RaisePropertyChanged("DimLinProd");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimParticipable
        {
            get
            {
                return this.DimParticipableField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimParticipableField, value) != true))
                {
                    this.DimParticipableField = value;
                    this.RaisePropertyChanged("DimParticipable");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimSku
        {
            get
            {
                return this.DimSkuField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimSkuField, value) != true))
                {
                    this.DimSkuField = value;
                    this.RaisePropertyChanged("DimSku");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DimSucursal
        {
            get
            {
                return this.DimSucursalField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DimSucursalField, value) != true))
                {
                    this.DimSucursalField = value;
                    this.RaisePropertyChanged("DimSucursal");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Docto
        {
            get
            {
                return this.DoctoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DoctoField, value) != true))
                {
                    this.DoctoField = value;
                    this.RaisePropertyChanged("Docto");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EspePago
        {
            get
            {
                return this.EspePagoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EspePagoField, value) != true))
                {
                    this.EspePagoField = value;
                    this.RaisePropertyChanged("EspePago");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Factura
        {
            get
            {
                return this.FacturaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FacturaField, value) != true))
                {
                    this.FacturaField = value;
                    this.RaisePropertyChanged("Factura");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaDocto
        {
            get
            {
                return this.FechaDoctoField;
            }
            set
            {
                if ((this.FechaDoctoField.Equals(value) != true))
                {
                    this.FechaDoctoField = value;
                    this.RaisePropertyChanged("FechaDocto");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaFactura
        {
            get
            {
                return this.FechaFacturaField;
            }
            set
            {
                if ((this.FechaFacturaField.Equals(value) != true))
                {
                    this.FechaFacturaField = value;
                    this.RaisePropertyChanged("FechaFactura");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaVencimiento
        {
            get
            {
                return this.FechaVencimientoField;
            }
            set
            {
                if ((this.FechaVencimientoField.Equals(value) != true))
                {
                    this.FechaVencimientoField = value;
                    this.RaisePropertyChanged("FechaVencimiento");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaTrans
        {
            get
            {
                return this.FechaTransField;
            }
            set
            {
                if ((this.FechaTransField.Equals(value) != true))
                {
                    this.FechaTransField = value;
                    this.RaisePropertyChanged("FechaTrans");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Folio
        {
            get
            {
                return this.FolioField;
            }
            set
            {
                if ((this.FolioField.Equals(value) != true))
                {
                    this.FolioField = value;
                    this.RaisePropertyChanged("Folio");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FormaPago
        {
            get
            {
                return this.FormaPagoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FormaPagoField, value) != true))
                {
                    this.FormaPagoField = value;
                    this.RaisePropertyChanged("FormaPago");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long IdFactReferencia
        {
            get
            {
                return this.IdFactReferenciaField;
            }
            set
            {
                if ((this.IdFactReferenciaField.Equals(value) != true))
                {
                    this.IdFactReferenciaField = value;
                    this.RaisePropertyChanged("IdFactReferencia");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IdRegistro
        {
            get
            {
                return this.IdRegistroField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IdRegistroField, value) != true))
                {
                    this.IdRegistroField = value;
                    this.RaisePropertyChanged("IdRegistro");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public NoYes ImpuestosInc
        {
            get
            {
                return this.ImpuestosIncField;
            }
            set
            {
                if ((this.ImpuestosIncField.Equals(value) != true))
                {
                    this.ImpuestosIncField = value;
                    this.RaisePropertyChanged("ImpuestosInc");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Moneda
        {
            get
            {
                return this.MonedaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MonedaField, value) != true))
                {
                    this.MonedaField = value;
                    this.RaisePropertyChanged("Moneda");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NombreDiario
        {
            get
            {
                return this.NombreDiarioField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NombreDiarioField, value) != true))
                {
                    this.NombreDiarioField = value;
                    this.RaisePropertyChanged("NombreDiario");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PerfilAsiento
        {
            get
            {
                return this.PerfilAsientoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PerfilAsientoField, value) != true))
                {
                    this.PerfilAsientoField = value;
                    this.RaisePropertyChanged("PerfilAsiento");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RFC
        {
            get
            {
                return this.RFCField;
            }
            set
            {
                if ((object.ReferenceEquals(this.RFCField, value) != true))
                {
                    this.RFCField = value;
                    this.RaisePropertyChanged("RFC");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Serie
        {
            get
            {
                return this.SerieField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SerieField, value) != true))
                {
                    this.SerieField = value;
                    this.RaisePropertyChanged("Serie");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public NoYes SinComprobante
        {
            get
            {
                return this.SinComprobanteField;
            }
            set
            {
                if ((this.SinComprobanteField.Equals(value) != true))
                {
                    this.SinComprobanteField = value;
                    this.RaisePropertyChanged("SinComprobante");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public SistemaOrigen SistemaOrigen
        {
            get
            {
                return this.SistemaOrigenField;
            }
            set
            {
                if ((this.SistemaOrigenField.Equals(value) != true))
                {
                    this.SistemaOrigenField = value;
                    this.RaisePropertyChanged("SistemaOrigen");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Subtotal
        {
            get
            {
                return this.SubtotalField;
            }
            set
            {
                if ((this.SubtotalField.Equals(value) != true))
                {
                    this.SubtotalField = value;
                    this.RaisePropertyChanged("Subtotal");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Texto
        {
            get
            {
                return this.TextoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TextoField, value) != true))
                {
                    this.TextoField = value;
                    this.RaisePropertyChanged("Texto");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TipoCambio
        {
            get
            {
                return this.TipoCambioField;
            }
            set
            {
                if ((this.TipoCambioField.Equals(value) != true))
                {
                    this.TipoCambioField = value;
                    this.RaisePropertyChanged("TipoCambio");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public BBITipoFact TipoFact
        {
            get
            {
                return this.TipoFactField;
            }
            set
            {
                if ((this.TipoFactField.Equals(value) != true))
                {
                    this.TipoFactField = value;
                    this.RaisePropertyChanged("TipoFact");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Total
        {
            get
            {
                return this.TotalField;
            }
            set
            {
                if ((this.TotalField.Equals(value) != true))
                {
                    this.TotalField = value;
                    this.RaisePropertyChanged("Total");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UUID
        {
            get
            {
                return this.UUIDField;
            }
            set
            {
                if ((object.ReferenceEquals(this.UUIDField, value) != true))
                {
                    this.UUIDField = value;
                    this.RaisePropertyChanged("UUID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string XMLFactura
        {
            get
            {
                return this.XMLFacturaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.XMLFacturaField, value) != true))
                {
                    this.XMLFacturaField = value;
                    this.RaisePropertyChanged("XMLFactura");
                }
            }
        }
        /*public string company { get; set; }
        public string recordId { get; set; }
        public SistemaOrigen? SistemaOrigen { get; set; }
        public string IdRegistro { get; set; }
        public string Compania { get; set; }
        public string Descripcion { get; set; }
        public string NombreDiario { get; set; }
        public NoYes? ImpuestosInc { get; set; }
        public DateTime? FechaTrans { get; set; }
        public string CodProveedor { get; set; }
        public string CodProveedorPedido { get; set; }
        public string CodCliente { get; set; }
        public string CodClientePedido { get; set; }
        public string PerfilAsiento { get; set; }
        public DateTime? FechaFactura { get; set; }
        public DateTime? FechaDocto { get; set; }
        public string Serie { get; set; }
        public int? Folio { get; set; }
        public NoYes? SinComprobante { get; set; }
        public string Factura { get; set; }
        public String UUID { get; set; }
        public string UsoCFDI { get; set; }
        public string MetodoPagoSAT { get; set; }
        public string UUIDRelacionado { get; set; }
        public EInvoiceCFDIReferenceType? TipoRelacionUUID { get; set; }
        public int? Parcialidad { get; set; }
        public String XMLFactura { get; set; }
        public string Docto { get; set; }
        public string AprobadoPor { get; set; }
        public BBITipoFact? TipoFact { get; set; }
        public Int64 IdFactReferencia { get; set; }
        public string RFC { get; set; }
        public string CondPago { get; set; }
        public string DesctoPago { get; set; }
        public string EspePago { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string FormaPago { get; set; }
        public string Banco { get; set; }
        public string Descripción { get; set; }
        public string Texto { get; set; }
        public Decimal? Subtotal { get; set; }
        public Decimal? Total { get; set; }
        public string Moneda { get; set; }
        public Decimal? TipoCambio { get; set; }
        public string Orden { get; set; }
        public string Referencia { get; set; }*/
        public List<LOGI_DocumentLineD365_INFO> Lineas { get; set; }
    }
}
