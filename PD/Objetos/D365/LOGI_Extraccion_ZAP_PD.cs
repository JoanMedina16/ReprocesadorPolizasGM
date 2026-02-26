using AD;
using AD.Objetos.D365;
using INFO.Objetos.D365;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Objetos.D365
{
    public class LOGI_Extraccion_ZAP_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Extraccion_ZAP_AD oCuentas = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Extraccion_ZAP_PD.cs";
        const string CONST_MODULO = "Extraccion ZAP";

        public LOGI_Extraccion_ZAP_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oCuentas = new LOGI_Extraccion_ZAP_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string Listacuentas(ref List<LOGI_Extraccion_ZAP_INFO> lstcuentas, LOGI_Extraccion_ZAP_INFO oCuenta)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                //oTool.LogProceso(String.Format("INICIO {0} FIN {1}",oCuenta.fecha_inicio, oCuenta.fecha_final), "Listacuentas", CONST_CLASE, CONST_MODULO, sConsultaSql);

                sReponse = oCuentas.Listacuentasdiario(ref oConnection, ref lstcuentas, oCuenta, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listacuentas", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }


        public string ListaDimensionVehiculo(ref List<LOGI_Extraccion_Vehiculo_INFO> lstVehiculos, string Descripciones, string Vouchers)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oCuentas.ListaVehiculo(ref oConnection, ref lstVehiculos, Descripciones, Vouchers, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaDimensionVehiculo", CONST_CLASE, CONST_MODULO, sConsultaSql);
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
