using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Helpers
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "conexionOK", WrapperNamespace = "http://schemas.microsoft.com/dynamics/2011/01/services", IsWrapped = true)]
    public partial  class conexionOK
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
}
