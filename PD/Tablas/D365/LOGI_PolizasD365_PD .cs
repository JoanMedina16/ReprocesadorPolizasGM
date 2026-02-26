using AD;
using AD.Tablas.D365;
using AD.Tablas.EQUIV;
using AD.Tablas.FUEL;
using INFO.Enums;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.D365
{
    public class LOGI_PolizasD365_PD
    {

        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Polizas_detalle_AD oPolizadetalle = null;
        internal LOGI_PolizasD365_AD oPolizas = null;
        const string CONST_CLASE = "LOGI_PolizasD365_PD.cs";
        const string CONST_MODULO = "Polizas D365";
        const int CONST_EMPRESA = 67;///Siempre corresponde a 67 - Logística del Mayab

        public LOGI_PolizasD365_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oPolizadetalle = new LOGI_Polizas_detalle_AD();
            oPolizas = new LOGI_PolizasD365_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string Crearegistro(string sUsuarioID, LOGI_PolizasD365_INFO oParam, out string sresponse)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sresponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                string CONST_TABLE = oTool.GetEnumDefaultValue((eDocumentoTMS)oParam.tipo);
                sReponse = oPolizas.Creadocumento(ref oConnection, CONST_TABLE, sUsuarioID, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Creaerror", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaPolizas(string sUsuarioID, LOGI_PolizasD365_INFO oParam, ref List<LOGI_PolizasD365_INFO> lstPolizas, out string sresponse)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sresponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                //string CONST_TABLE = oTool.GetEnumDefaultValue((eDocumentoTMS)oParam.tipo);
                sReponse = oPolizas.ListaPolizas(ref oConnection, ref lstPolizas, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaPolizas", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
