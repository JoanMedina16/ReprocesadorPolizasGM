using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Helpers.D365FOBBIContaServices
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
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarPolizaContContractEnc))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarPolizaContContractLin))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarPolizaContContract))]
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
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarPolizaContContractEnc))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarPolizaContContractLin))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BBICargarPolizaContContract))]
    public partial class XppObjectBase : ProxyBase
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarPolizaContContractEnc", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public partial class BBICargarPolizaContContractEnc : XppObjectBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CompaniaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescripcionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaTransField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdRegistroField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NoYes ImpuestosIncField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MonedaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreDiarioField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBISistemaOrigen SistemaOrigenField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal TipoCambioField;

   

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
        public BBISistemaOrigen SistemaOrigen
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
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarPolizaContContractLin", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public partial class BBICargarPolizaContContractLin : XppObjectBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodImpuestoField;

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
    [System.Runtime.Serialization.DataContractAttribute(Name = "BBICargarPolizaContContract", Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.AX.Application")]
    [System.SerializableAttribute()]
    public partial class BBICargarPolizaContContract : XppObjectBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBICargarPolizaContContractEnc EncabezadoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BBICargarPolizaContContractLin[] LineasField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public BBICargarPolizaContContractEnc Encabezado
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
        public BBICargarPolizaContContractLin[] Lineas
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
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", ConfigurationName = "BBICargarPolizaContService")]
    public interface BBICargarPolizaContService
    {

        // CODEGEN: Se está generando un contrato de mensaje, ya que el mensaje conexionOK tiene encabezados.
        [System.ServiceModel.OperationContractAttribute(Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarPolizaContService" +
            "/conexionOK", ReplyAction = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarPolizaContService" +
            "/conexionOKResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Fault), Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarPolizaContService" +
            "/Fault", Name = "Fault", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        conexionOKResponse conexionOK(conexionOK request);

        // CODEGEN: Se está generando un contrato de mensaje, ya que el mensaje cargarPoliza tiene encabezados.
        [System.ServiceModel.OperationContractAttribute(Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarPolizaContService" +
            "/cargarPoliza", ReplyAction = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarPolizaContService" +
            "/cargarPolizaResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Fault), Action = "http://schemas.microsoft.com/dynamics/2011/01/services/BBICargarPolizaContService" +
            "/Fault", Name = "Fault", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        cargarPolizaResponse cargarPoliza(cargarPoliza request);
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
    [System.ServiceModel.MessageContractAttribute(WrapperName = "cargarPoliza", WrapperNamespace = "http://schemas.microsoft.com/dynamics/2011/01/services", IsWrapped = true)]
    public partial class cargarPoliza
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        public CallContext CallContext;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", Order = 0)]
        public BBICargarPolizaContContract _contrato;

        public cargarPoliza()
        {
        }

        public cargarPoliza(CallContext CallContext, BBICargarPolizaContContract _contrato)
        {
            this.CallContext = CallContext;
            this._contrato = _contrato;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "cargarPolizaResponse", WrapperNamespace = "http://schemas.microsoft.com/dynamics/2011/01/services", IsWrapped = true)]
    public partial class cargarPolizaResponse
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
        public Infolog Infolog;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", Order = 0)]
        public string result;

        public cargarPolizaResponse()
        {
        }

        public cargarPolizaResponse(Infolog Infolog, string result)
        {
            this.Infolog = Infolog;
            this.result = result;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface BBICargarPolizaContServiceChannel : BBICargarPolizaContService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class BBICargarPolizaContServiceClient : System.ServiceModel.ClientBase<BBICargarPolizaContService>, BBICargarPolizaContService
    {

        public BBICargarPolizaContServiceClient()
        {
        }

        public BBICargarPolizaContServiceClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public BBICargarPolizaContServiceClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public BBICargarPolizaContServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public BBICargarPolizaContServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        conexionOKResponse BBICargarPolizaContService.conexionOK(conexionOK request)
        {
            return base.Channel.conexionOK(request);
        }

        public Infolog conexionOK(CallContext CallContext, out string result)
        {
            conexionOK inValue = new conexionOK();
            inValue.CallContext = CallContext;
            conexionOKResponse retVal = ((BBICargarPolizaContService)(this)).conexionOK(inValue);
            result = retVal.result;
            return retVal.Infolog;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        cargarPolizaResponse BBICargarPolizaContService.cargarPoliza(cargarPoliza request)
        {
            return base.Channel.cargarPoliza(request);
        }

        public Infolog cargarPoliza(CallContext CallContext, BBICargarPolizaContContract _contrato, out string result)
        {
            cargarPoliza inValue = new cargarPoliza();
            inValue.CallContext = CallContext;
            inValue._contrato = _contrato;
            cargarPolizaResponse retVal = ((BBICargarPolizaContService)(this)).cargarPoliza(inValue);
            result = retVal.result;
            return retVal.Infolog;
        }
    }
}
