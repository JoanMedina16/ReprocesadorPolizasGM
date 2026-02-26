using D365.Helpers;
using D365.INFO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Request
{
    public class D365FOBBIContaServices
    {

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
             
            public Infolog cargarPoliza(CallContext CallContext, LOGI_ContratoD365_INFO _contrato, out string result)
            {
                cargarPoliza inValue = new cargarPoliza();
                inValue.CallContext = CallContext;
                inValue._contrato = _contrato;
                cargarPolizaResponse retVal = ((BBICargarPolizaContService)(this)).cargarPoliza(inValue);
                result = retVal.result;
                return retVal.Infolog;
            }

            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
            public cargarPolizaResponse cargarPoliza(cargarPoliza request)
            {
                return base.Channel.cargarPoliza(request);
            }
        }

    }
}
