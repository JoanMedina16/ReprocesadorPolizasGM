using AD;
using AD.Tablas.CAT;
using AD.Tablas.D365;
using INFO.Tablas.CAT;
using INFO.Tablas.D365;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace PD.Tablas.D365
{
   public class LOGI_Requisiciones_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_ConexionSql_AD oConnLogi = null;
        internal LOGI_Requisiciones_AD oRequisicion = null;
        const string CONST_CLASE = "LOGI_Documentos_PD.cs";
        const string CONST_MODULO = "Documentos D365";

        public LOGI_Requisiciones_PD(string sConnectionZAP, string sConnLogi)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnectionZAP);
            oConnLogi = new LOGI_ConexionSql_AD(sConnLogi);
            oRequisicion = new LOGI_Requisiciones_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string Listarequisiones(int PerfilID, ref List<LOGI_Requisicion_INFO> lstRequisiciones, LOGI_Requisicion_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            List<LOGI_Matrizrol_INFO> lstMatriz = new List<LOGI_Matrizrol_INFO>();
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();

                if (string.IsNullOrEmpty(oParam.FECHA_INICIO) && string.IsNullOrEmpty(oParam.FECHA_FINAL))
                {
                    oParam.FECHA_INICIO = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                    oParam.FECHA_FINAL = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else {
                    oParam.FECHA_INICIO = Convert.ToDateTime(oParam.FECHA_INICIO).ToString("yyyy-MM-dd");
                    oParam.FECHA_FINAL = Convert.ToDateTime(oParam.FECHA_FINAL).ToString("yyyy-MM-dd");
                }

                if (PerfilID != 1)
                {
                    new LOGI_Permisos_AD().RecuperaMatrizByPerfil(ref oConnLogi, ref lstMatriz, PerfilID, out sConsultaSql);
                    if (lstMatriz.Count > 0)
                    {
                        foreach (LOGI_Matrizrol_INFO data in lstMatriz)
                        {
                            oParam.MINIMO_TOTAL = Convert.ToDouble(data.rango_inicial);
                            oParam.MAXIMO_TOTAL = Convert.ToDouble(data.rango_final);
                            oParam.MAIACCOUNT = data.cuenta_inicial;
                            oRequisicion.Listarequisiciones(ref oConnection, ref lstRequisiciones, oParam, out sConsultaSql);
                        }
                        sReponse = lstRequisiciones.Count > 0 ? "OK" : "SIN RESULTADOS";
                    }
                    else throw new Exception("No se ha encontrado la configuración de la matriz de cuentas");
                }
                else sReponse = oRequisicion.Listarequisiciones(ref oConnection, ref lstRequisiciones, oParam, out sConsultaSql);

            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listarequisiones", PerfilID.ToString(), CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
                if (oConnLogi != null)
                    oConnLogi.CloseConnection();
            }
            return sReponse;
        }

        public string Listarequisiondetalle(string sUsuarioID, ref List<LOGI_Requisicion_Line_INFO> lstRequisiciones, string Foliorequi)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oRequisicion.Listarequisiciondetalle(ref oConnection, ref lstRequisiciones, Foliorequi, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listarequisiones", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
