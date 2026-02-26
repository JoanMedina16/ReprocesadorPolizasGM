using AD;
using AD.Objetos.OPE;
using INFO.Objetos.SAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Herramientas
{
   public class LOGI_Historico_XMLS_PD
    { 

        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Historico_XMLS_AD oHistoricoAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "BIL_Historico_XMLS_PD.cs";
        const string CONST_MODULO = "XMLS Historicos";


        public LOGI_Historico_XMLS_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oHistoricoAD = new LOGI_Historico_XMLS_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string RecuperaCFDIs(LOGI_XMLS_INFO objFormato, ref Int32 iRegistros, int anio = 0, int mes = 0)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oHistoricoAD.RecuperaCFDIs(ref oConnection, objFormato, out sConsultaSql, ref iRegistros, anio, mes);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaCFDIs", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaCFDIS(ref List<LOGI_XMLS_INFO> lstCfdis, LOGI_XMLS_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oHistoricoAD.ListaArchivosXML(ref oConnection, ref lstCfdis, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaCFDIS", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string NuevoDocumento(LOGI_XMLS_INFO objFormato, int anio, int mes)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oHistoricoAD.NuevoDocumento(ref oConnection, objFormato, anio, mes, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "NuevoDocumento", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido crear la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string EditaDocumento(LOGI_XMLS_INFO objFormato)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oHistoricoAD.ActualizaDocumento(ref oConnection, objFormato, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "NuevoDocumento", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido crear la información {0}", ex.Message);
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