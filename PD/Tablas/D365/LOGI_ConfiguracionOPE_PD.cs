using AD.Tablas.D365;
using AD;
using INFO.Tablas.D365;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.D365
{
    public class LOGI_ConfiguracionOPE_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_ConfiguracionOPE_AD oConfiguracionAD = null;
        const string CONST_CLASE = "LOGI_ConfiguracionOPE_PD.cs";
        const string CONST_MODULO = "Configuración OPEADM";

        public LOGI_ConfiguracionOPE_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oConfiguracionAD = new LOGI_ConfiguracionOPE_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaConfiguracion(string sUsuarioID, ref LOGI_ConfiguracionOPE_INFO oConfiguracion)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oConfiguracionAD.ListaConfiguracion(ref oConnection, ref oConfiguracion, out sConsultaSql); 
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaConfiguracion", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ActualizaConfiguracion(string sUsuarioID, LOGI_ConfiguracionOPE_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oConfiguracionAD.ActualizaConfiguracion(ref oConnection, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ActualizaConfiguracion", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

    }
}
