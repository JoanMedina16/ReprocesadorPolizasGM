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
   public class LOGI_Sucursales_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Sucursales_AD oSucursalAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Sucursales_PD.cs";
        const string CONST_MODULO = "Sucursales";

        public LOGI_Sucursales_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oSucursalAD = new LOGI_Sucursales_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaSucursales(ref List<LOGI_Catalogos_INFO> lstSucursales, LOGI_Catalogos_INFO oSucursal)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                if (!string.IsNullOrEmpty(oSucursal.sEmpresas))
                    oSucursal.sEmpresas = oSucursal.sEmpresas.TrimEnd(',');
                sReponse = oSucursalAD.RecuperaSucursales(ref oConnection, ref lstSucursales, oSucursal, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaSucursales", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }
        public string AgregaSucursal(LOGI_Catalogos_INFO oSucursal)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oSucursalAD.NuevoSucursal(ref oConnection, oSucursal, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "AgregaSucursal", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        } 
        public string EditaSucursal(LOGI_Catalogos_INFO oSucursal)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oSucursalAD.EditaSucursal(ref oConnection, oSucursal, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "EditaSucursal", CONST_CLASE, CONST_MODULO, sConsultaSql);
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

