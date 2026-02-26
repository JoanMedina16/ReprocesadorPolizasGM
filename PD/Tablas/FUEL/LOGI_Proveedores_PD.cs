using AD;
using AD.Tablas.FUEL;
using INFO.Tablas.FUEL;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.FUEL
{
  public  class LOGI_Proveedores_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Proveedores_AD oProveedors = null;
        const string CONST_CLASE = "LOGI_Proveedores_PD.cs";
        const string CONST_MODULO = "Catálogo de proveedores";

        public LOGI_Proveedores_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oProveedors = new LOGI_Proveedores_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string Listaproveedores(string sUsuarioID, ref List<LOGI_Proveedores_INFO> lstProveedores, LOGI_Proveedores_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oProveedors.Listaproveedores(ref oConnection, ref lstProveedores, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listaproveedores", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ObtenerDiasPAgo(string sUsuarioID, ref string sDiasPAGO, string sCodigo)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oProveedors.ListaProveedorDias(ref oConnection, ref sDiasPAGO, sCodigo, out sConsultaSql);
            }
            catch (Exception ex)
            {
                sDiasPAGO = "SIN DIA DE PAGO";
                oTool.LogError(ex, "ObtenerDiasPAgo", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
