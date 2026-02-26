using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Helpers
{
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
}
