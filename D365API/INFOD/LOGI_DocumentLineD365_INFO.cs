using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365API.INFOD
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ProxyBase", Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.AX.KernelInterop")]
    [System.SerializableAttribute()]
    public class LOGI_DocumentLineD365_INFO
    {
        public string CuentaContable { get; set; }
        public string Texto { get; set; }
        public Decimal? Debito { get; set; }
        public Decimal? Credito { get; set; }
        public string GrupoImpuestos { get; set; }
        public string GrupoImpuestosArt { get; set; }
        public string CodImpuesto { get; set; }
        public string Vehiculo { get; set; }
        public string DimArea { get; set; }
        public string DimCanal { get; set; }
        public string DimCeco { get; set; }
        public string DimCuc { get; set; }
        public string DimDepto { get; set; }
        public string DimFilTer { get; set; }
        public string DimLinProd { get; set; }
        public string DimParticipable { get; set; }
        public string DimSku { get; set; }
        public string DimSucursal { get; set; }
    }
}
