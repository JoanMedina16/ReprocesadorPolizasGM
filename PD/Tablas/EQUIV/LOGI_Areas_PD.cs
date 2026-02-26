using AD;
using AD.Tablas.EQUIV;
using INFO.Tablas.EQUIV;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.EQUIV
{
   public class LOGI_Areas_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Areas_AD oAreaAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Areas_PD.cs";
        const string CONST_MODULO = "Areas";

        public LOGI_Areas_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oAreaAD = new LOGI_Areas_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaAreas(ref List<LOGI_Catalogos_INFO> lstAreas, LOGI_Catalogos_INFO oArea)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oAreaAD.RecuperaAreas(ref oConnection, ref lstAreas, oArea, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaAreas", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }
        public string AgregaArea(LOGI_Catalogos_INFO oArea)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oAreaAD.NuevaArea(ref oConnection, oArea, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "AgregaArea", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        } 
        public string EditaArea(LOGI_Catalogos_INFO oArea)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oAreaAD.EditaArea(ref oConnection, oArea, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "EditaArea", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
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

