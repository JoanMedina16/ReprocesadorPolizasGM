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
   public class LOGI_Bancos_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Bancos_AD oBancoAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Bancos_PD.cs";
        const string CONST_MODULO = "Bancos";

        public LOGI_Bancos_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oBancoAD = new LOGI_Bancos_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaBancos(ref List<LOGI_Catalogos_INFO> lstBancos, LOGI_Catalogos_INFO oBanco)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                if (!string.IsNullOrEmpty(oBanco.sEmpresas))
                    oBanco.sEmpresas = oBanco.sEmpresas.TrimEnd(',');
                sReponse = oBancoAD.RecuperaBancos(ref oConnection, ref lstBancos, oBanco, out sConsultaSql, true);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaBancos", CONST_CLASE, CONST_MODULO, sConsultaSql);
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
