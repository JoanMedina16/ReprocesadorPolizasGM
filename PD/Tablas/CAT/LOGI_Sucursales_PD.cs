using AD;
using AD.Tablas.CAT;
using INFO.Tablas.CAT;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.CAT
{
   public class LOGI_Sucursales_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;        
        internal LOGI_Sucursales_AD oSucursal = null;
        const string CONST_CLASE = "LOGI_Sucursales_PD.cs";
        const string CONST_MODULO = "Sucursales";

        public LOGI_Sucursales_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oSucursal = new LOGI_Sucursales_AD();            
            oTool = new LOGI_Tools_PD();
        }

        public string Listasucursales(string sUsuarioID, ref List<LOGI_Sucursales_INFO> lstSucursales)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oSucursal.ListaSucursales(ref oConnection, ref lstSucursales, out sConsultaSql);
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
