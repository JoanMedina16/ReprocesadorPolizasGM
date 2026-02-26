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
   public class LOGI_Fondofijo_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Fondofijo_AD oFondofijoAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Fondofijo_PD.cs";
        const string CONST_MODULO = "Fondo fijo";

        public LOGI_Fondofijo_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oFondofijoAD = new LOGI_Fondofijo_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaFondofijo(ref List<LOGI_Catalogos_INFO> lstFondofijo, LOGI_Catalogos_INFO oFondo)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                if (!string.IsNullOrEmpty(oFondo.sEmpresas))
                    oFondo.sEmpresas = oFondo.sEmpresas.TrimEnd(',');
                sReponse = oFondofijoAD.RecuperaFondofijo(ref oConnection, ref lstFondofijo, oFondo, out sConsultaSql, true);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaFondofijo", CONST_CLASE, CONST_MODULO, sConsultaSql);
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
