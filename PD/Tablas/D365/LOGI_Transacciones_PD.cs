using AD;
using AD.Tablas.D365;
using INFO.Tablas.D365;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.D365
{
   public class LOGI_Transacciones_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Transacciones_AD oTransaccion = null;
        const string CONST_CLASE = "LOGI_Transacciones_PD.cs";
        const string CONST_MODULO = "Transacciones D365";

        public LOGI_Transacciones_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oTransaccion = new LOGI_Transacciones_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaTransacciones(string sUsuarioID, LOGI_Transacciones_INFO oParam, ref List<LOGI_Transacciones_INFO> lstTransacciones)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oTransaccion.ListaTransacciones(ref oConnection, ref lstTransacciones, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaTransacciones", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ActualizaTransaccion(string sUsuarioID, LOGI_Transacciones_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oTransaccion.ActualizaTransaccion(ref oConnection, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ActualizaTransaccion", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaJSON(string sUsuarioID,ref LOGI_Transacciones_INFO oParam, String asiento_id)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oTransaccion.RecuperaResponse(ref oConnection, ref oParam, asiento_id, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaJSON", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
