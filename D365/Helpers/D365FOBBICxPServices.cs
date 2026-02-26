using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Helpers
{
    using System.Runtime.Serialization;
    using System;


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "CallContext", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
    [System.SerializableAttribute()]
    public partial class CallContext : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CompanyField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LanguageField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PartitionKeyField;

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

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Company
        {
            get
            {
                return this.CompanyField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CompanyField, value) != true))
                {
                    this.CompanyField = value;
                    this.RaisePropertyChanged("Company");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Language
        {
            get
            {
                return this.LanguageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LanguageField, value) != true))
                {
                    this.LanguageField = value;
                    this.RaisePropertyChanged("Language");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MessageId
        {
            get
            {
                return this.MessageIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MessageIdField, value) != true))
                {
                    this.MessageIdField = value;
                    this.RaisePropertyChanged("MessageId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PartitionKey
        {
            get
            {
                return this.PartitionKeyField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PartitionKeyField, value) != true))
                {
                    this.PartitionKeyField = value;
                    this.RaisePropertyChanged("PartitionKey");
                }
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
    [System.Runtime.Serialization.DataContractAttribute(Name = "Infolog", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
    [System.SerializableAttribute()]
    public partial class Infolog : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private InfologEntry[] EntriesField;

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

        [System.Runtime.Serialization.DataMemberAttribute()]
        public InfologEntry[] Entries
        {
            get
            {
                return this.EntriesField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EntriesField, value) != true))
                {
                    this.EntriesField = value;
                    this.RaisePropertyChanged("Entries");
                }
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
    [System.Runtime.Serialization.DataContractAttribute(Name = "InfologEntry", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
    [System.SerializableAttribute()]
    public partial class InfologEntry : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private InfologType TypeField;

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

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MessageField, value) != true))
                {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public InfologType Type
        {
            get
            {
                return this.TypeField;
            }
            set
            {
                if ((this.TypeField.Equals(value) != true))
                {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "InfologType", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
    public enum InfologType : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Info = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Warning = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Error = 2,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ProxyBase", Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.AX.KernelInterop")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(XppObjectBase))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarFacturaCxPContractEnc))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarFacturaCxPContractLin))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarFacturaCxPContract))]
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
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarFacturaCxPContractEnc))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarFacturaCxPContractLin))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarFacturaCxPContract))]
    public partial class XppObjectBase : ProxyBase
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarFacturaCxPContractEnc", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public partial class BBICargarFacturaCxPContractEnc : XppObjectBase
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
        private NoYes? SinComprobanteField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBISistemaOrigen? SistemaOrigenField;

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
        public NoYes? SinComprobante
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
        public BBISistemaOrigen? SistemaOrigen
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
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarFacturaCxPContractLin", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public partial class BBICargarFacturaCxPContractLin : XppObjectBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodImpuestoField; 
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AprobadoPorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal CreditoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CuentaContableField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal DebitoField;

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
        private string VehiculoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimLinProdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimParticipableField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimSkuField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DimSucursalField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GrupoImpuestosField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GrupoImpuestosArtField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TextoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UUIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaDoctoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NoYes SinComprobanteField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FacturaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string XMLFacturaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DoctoField;



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
        public string CodImpuesto
        {
            get
            {
                return this.CodImpuestoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CodImpuestoField, value) != true))
                {
                    this.CodImpuestoField = value;
                    this.RaisePropertyChanged("CodImpuesto");
                }
            }
        } 
        
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
        public decimal Credito
        {
            get
            {
                return this.CreditoField;
            }
            set
            {
                if ((this.CreditoField.Equals(value) != true))
                {
                    this.CreditoField = value;
                    this.RaisePropertyChanged("Credito");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CuentaContable
        {
            get
            {
                return this.CuentaContableField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CuentaContableField, value) != true))
                {
                    this.CuentaContableField = value;
                    this.RaisePropertyChanged("CuentaContable");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Debito
        {
            get
            {
                return this.DebitoField;
            }
            set
            {
                if ((this.DebitoField.Equals(value) != true))
                {
                    this.DebitoField = value;
                    this.RaisePropertyChanged("Debito");
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
        public string Vehiculo
        {
            get
            {
                return this.VehiculoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.VehiculoField, value) != true))
                {
                    this.VehiculoField = value;
                    this.RaisePropertyChanged("Vehiculo");
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
        public string GrupoImpuestos
        {
            get
            {
                return this.GrupoImpuestosField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GrupoImpuestosField, value) != true))
                {
                    this.GrupoImpuestosField = value;
                    this.RaisePropertyChanged("GrupoImpuestos");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GrupoImpuestosArt
        {
            get
            {
                return this.GrupoImpuestosArtField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GrupoImpuestosArtField, value) != true))
                {
                    this.GrupoImpuestosArtField = value;
                    this.RaisePropertyChanged("GrupoImpuestosArt");
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
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarFacturaCxPContract", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public partial class BBICargarFacturaCxPContract : XppObjectBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBICargarFacturaCxPContractEnc EncabezadoField;


        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBICargarFacturaCxPContractEnc[] EncabezadoFieldLineas;


        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBICargarFacturaCxPContractLin[] LineasField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public BBICargarFacturaCxPContractEnc Encabezado
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
        public BBICargarFacturaCxPContractLin[] Lineas
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

        [System.Runtime.Serialization.DataMemberAttribute()]
        public BBICargarFacturaCxPContractEnc[] Lineascabecera
        {
            get
            {
                return this.EncabezadoFieldLineas;
            }
            set
            {
                if ((object.ReferenceEquals(this.EncabezadoFieldLineas, value) != true))
                {
                    this.EncabezadoFieldLineas = value;
                    this.RaisePropertyChanged("Lineas");
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "NoYes", Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.Ax.Xpp")]
    public enum NoYes : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        No = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Yes = 1,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBISistemaOrigen", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    public enum BBISistemaOrigen : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        OpeAdm = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AX2009 = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AX2012 = 2,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBITipoFact", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    public enum BBITipoFact : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Factura = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotaCred = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Cancelacion = 2,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Fault", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
    [System.SerializableAttribute()]
    public partial class Fault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ExceptionMessageField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ExceptionTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid RequestIdField;

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

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExceptionMessage
        {
            get
            {
                return this.ExceptionMessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ExceptionMessageField, value) != true))
                {
                    this.ExceptionMessageField = value;
                    this.RaisePropertyChanged("ExceptionMessage");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExceptionType
        {
            get
            {
                return this.ExceptionTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ExceptionTypeField, value) != true))
                {
                    this.ExceptionTypeField = value;
                    this.RaisePropertyChanged("ExceptionType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid RequestId
        {
            get
            {
                return this.RequestIdField;
            }
            set
            {
                if ((this.RequestIdField.Equals(value) != true))
                {
                    this.RequestIdField = value;
                    this.RaisePropertyChanged("RequestId");
                }
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", ConfigurationName = "BBICargarFacturaCxPService")]
    public interface BBICargarFacturaCxPService
    {

        // CODEGEN: Se está generando un contrato de mensaje, ya que el mensaje conexionOK tiene encabezados.
        [System.ServiceModel.OperationContractAttribute(Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarFacturaCxPService" +
            "/conexionOK", ReplyAction = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarFacturaCxPService" +
            "/conexionOKResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Fault), Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarFacturaCxPService" +
            "/Fault", Name = "Fault", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        conexionOKResponse conexionOK(conexionOK request);

        // CODEGEN: Se está generando un contrato de mensaje, ya que el mensaje cargarFactura tiene encabezados.
        [System.ServiceModel.OperationContractAttribute(Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarFacturaCxPService" +
            "/cargarFactura", ReplyAction = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarFacturaCxPService" +
            "/cargarFacturaResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Fault), Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarFacturaCxPService" +
            "/Fault", Name = "Fault", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        cargarFacturaResponse cargarFactura(cargarFactura request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "conexionOK", WrapperNamespace = "http://schemas.microsoft.com/dynamics/2011/01/services", IsWrapped = true)]
    public partial class conexionOK
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        public CallContext CallContext;

        public conexionOK()
        {
        }

        public conexionOK(CallContext CallContext)
        {
            this.CallContext = CallContext;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "conexionOKResponse", WrapperNamespace = "http://schemas.microsoft.com/dynamics/2011/01/services", IsWrapped = true)]
    public partial class conexionOKResponse
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        public Infolog Infolog;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", Order = 0)]
        public string result;

        public conexionOKResponse()
        {
        }

        public conexionOKResponse(Infolog Infolog, string result)
        {
            this.Infolog = Infolog;
            this.result = result;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "cargarFactura", WrapperNamespace = "http://schemas.microsoft.com/dynamics/2011/01/services", IsWrapped = true)]
    public partial class cargarFactura
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        public CallContext CallContext;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", Order = 0)]
        public BBICargarFacturaCxPContract _contrato;

        public cargarFactura()
        {
        }

        public cargarFactura(CallContext CallContext, BBICargarFacturaCxPContract _contrato)
        {
            this.CallContext = CallContext;
            this._contrato = _contrato;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "cargarFacturaResponse", WrapperNamespace = "http://schemas.microsoft.com/dynamics/2011/01/services", IsWrapped = true)]
    public partial class cargarFacturaResponse
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        public Infolog Infolog;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", Order = 0)]
        public string result;

        public cargarFacturaResponse()
        {
        }

        public cargarFacturaResponse(Infolog Infolog, string result)
        {
            this.Infolog = Infolog;
            this.result = result;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface BBICargarFacturaCxPServiceChannel : BBICargarFacturaCxPService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class BBICargarFacturaCxPServiceClient : System.ServiceModel.ClientBase<BBICargarFacturaCxPService>, BBICargarFacturaCxPService
    {

        public BBICargarFacturaCxPServiceClient()
        {
        }

        public BBICargarFacturaCxPServiceClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public BBICargarFacturaCxPServiceClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public BBICargarFacturaCxPServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public BBICargarFacturaCxPServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        conexionOKResponse BBICargarFacturaCxPService.conexionOK(conexionOK request)
        {
            return base.Channel.conexionOK(request);
        }

        public Infolog conexionOK(CallContext CallContext, out string result)
        {
            conexionOK inValue = new conexionOK();
            inValue.CallContext = CallContext;
            conexionOKResponse retVal = ((BBICargarFacturaCxPService)(this)).conexionOK(inValue);
            result = retVal.result;
            return retVal.Infolog;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        cargarFacturaResponse BBICargarFacturaCxPService.cargarFactura(cargarFactura request)
        {
            return base.Channel.cargarFactura(request);
        }

        public Infolog cargarFactura(CallContext CallContext, BBICargarFacturaCxPContract _contrato, out string result)
        {
            cargarFactura inValue = new cargarFactura();
            inValue.CallContext = CallContext;
            inValue._contrato = _contrato;
            cargarFacturaResponse retVal = ((BBICargarFacturaCxPService)(this)).cargarFactura(inValue);
            result = retVal.result;
            return retVal.Infolog;
        }
    }
}