using AD;
using AD.Objetos.D365;
using AD.Tablas.D365;
using INFO.Objetos.D365;
using INFO.Tablas.D365;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.D365
{
    public class LOGI_Plantilla_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Plantilla_AD oPlantillaAD = null;
        const string CONST_CLASE = "LOGI_Plantilla_PD.cs";
        const string CONST_MODULO = "Plantilla dispersion";

        public LOGI_Plantilla_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oPlantillaAD = new LOGI_Plantilla_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaPlantillas(string sUsuarioID, LOGI_Plantilla_INFO oParam, ref List<LOGI_Plantilla_INFO> lstPlantillas, int iTopresultados=-1)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oPlantillaAD.ListaPlantillaAsistentes(ref oConnection, ref lstPlantillas, oParam, out sConsultaSql, iTopresultados);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaPlantillas", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string Eliminaplantilla(string sUsuarioID, LOGI_Plantilla_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                LOGI_Dispersion_INFO oDispersion = new LOGI_Dispersion_INFO();
                oConnection.OpenConnection();
                oConnection.StarTransacction();
                sReponse = oPlantillaAD.ActualizaPlantilla(ref oConnection, sUsuarioID, oParam, out sConsultaSql);
                oDispersion.usado = 0;
                oDispersion.FolioAsistente = oParam.FolioAsistente;
                sReponse = new LOGI_Dispersiones_AD().ReactivaPoliza(ref oConnection, oDispersion, out sConsultaSql);
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnection.RollBackTransacction();
                oTool.LogError(ex, "ListaPlantillas", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
