using D365.Helpers;
using D365.INFOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2011/01/services", ConfigurationName = "PruebaWSSatelitesD365FO.D365FOBBICxCServices.BBICargarFacturaCxCService")]
    public interface BBICargarFacturaService
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
        cargarFacturaResponse cargarFactura(cargarFactura request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public class LOGI_EntradasDiario_D365 : System.ServiceModel.ClientBase<BBICargarFacturaService>, BBICargarFacturaService
    {

        public LOGI_EntradasDiario_D365()
        {
        }

        public LOGI_EntradasDiario_D365(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public LOGI_EntradasDiario_D365(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public LOGI_EntradasDiario_D365(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public LOGI_EntradasDiario_D365(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {

        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        conexionOKResponse BBICargarFacturaService.conexionOK(conexionOK request)
        {
            return base.Channel.conexionOK(request);
        }

        public Infolog conexionOK(CallContext CallContext, out string result)
        {
            conexionOK inValue = new conexionOK();
            inValue.CallContext = CallContext;
            conexionOKResponse retVal = ((BBICargarFacturaService)(this)).conexionOK(inValue);
            result = retVal.result;
            return retVal.Infolog;
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public cargarFacturaResponse cargarFactura(cargarFactura request)
        {
            return base.Channel.cargarFactura(request);
        }
        public Infolog cargarFactura(CallContext CallContext, LOGI_ContratoD365_INFO _contrato, out string result)
        {
            cargarFactura inValue = new cargarFactura();
            inValue.CallContext = CallContext;
            //inValue._contrato = _contrato;
            cargarFacturaResponse retVal = ((BBICargarFacturaService)(this)).cargarFactura(inValue);
            result = retVal.result;
            return retVal.Infolog;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public conexionOKResponse conexionOK(conexionOK request)
        {
            return base.Channel.conexionOK(request);
        }
    }
}
