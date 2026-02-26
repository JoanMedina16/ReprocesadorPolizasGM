using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.INFOD
{
    public enum SistemaOrigen
    {
        [Description("Sistema OPEADM")]
        [DefaultValue(0)]
        Sistema_OPE_ADM = 0,

        [Description("Sistema AX 2012")]
        [DefaultValue(1)]
        Sistema_AX2009 = 1,

        [Description("Sistema AX 2012")]
        [DefaultValue(2)]
        Sistema_AX2012 = 2,
    }
    public enum NoYes
    {
        [Description("No")]
        [DefaultValue(0)]
        No = 0,

        [Description("Sí")]
        [DefaultValue(1)]
        Yes = 1,
    }
    public enum EInvoiceCFDIReferenceType
    {
        [Description("No establecido")]
        [DefaultValue(0)]
        NoEstablecido = 0,

        [Description("Nota de crédito")]
        [DefaultValue(1)]
        CreditNote = 1,

        [Description("Nota de debito")]
        [DefaultValue(2)]
        DebitNote = 2,

        [Description("GoodsReturn")]
        [DefaultValue(3)]
        GoodsReturn = 3,

        [Description("Substitution")]
        [DefaultValue(4)]
        Substitution = 4,

        [Description("GoodsTransfer")]
        [DefaultValue(5)]
        GoodsTransfer = 5,

        [Description("Invoice")]
        [DefaultValue(6)]
        Invoice = 6,

        [Description("Prepayment")]
        [DefaultValue(7)]
        Prepayment = 7,

        [Description("PaymentInInstallments")]
        [DefaultValue(8)]
        PrepaymPaymentInInstallmentsent = 8,

        [Description("DeferredPayment")]
        [DefaultValue(9)]
        DeferredPayment = 9,
    }
    public enum BBITipoFact
    {
        [Description("Factura")]
        [DefaultValue(0)]
        Factura = 0,

        [Description("NotaCred")]
        [DefaultValue(1)]
        NotaCred = 1,

        [Description("Cancelacion")]
        [DefaultValue(2)]
        Cancelacion = 2,
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
}
