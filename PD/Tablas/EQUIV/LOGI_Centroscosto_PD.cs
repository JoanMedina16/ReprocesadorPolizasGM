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
  public  class LOGI_Centroscosto_PD
    {

        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Centroscosto_AD oCentroAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Centroscosto_PD.cs";
        const string CONST_MODULO = "Centros de costo";

        public LOGI_Centroscosto_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oCentroAD = new LOGI_Centroscosto_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaCentroscosto(ref List<LOGI_Catalogos_INFO> lstCentros, LOGI_Catalogos_INFO oCentro)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oCentroAD.RecuperaCentroscosto(ref oConnection, ref lstCentros, oCentro, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaCentroscosto", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }
        public string AgregaCentro(LOGI_Catalogos_INFO oCentro)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oCentroAD.NuevoCentrocosto(ref oConnection, oCentro, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "AgregaCentro", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        } 
        public string EditaCentro(LOGI_Catalogos_INFO oCentro)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oCentroAD.EditaCentrocosto(ref oConnection, oCentro, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "EditaCentro", CONST_CLASE, CONST_MODULO, sConsultaSql);
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
