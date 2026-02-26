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
    public class LOGI_Proveedores_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Proveedores_AD oProveedores= null;
        const string CONST_CLASE = "LOGI_Proveedores_PD.cs";
        const string CONST_MODULO = "Clientes";

        public LOGI_Proveedores_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oProveedores = new LOGI_Proveedores_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaProveedores(string sUsuarioID, LOGI_Catalogos_INFO oParam, ref List<LOGI_Catalogos_INFO> lstProveedores)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oProveedores.RecuperaProveedores(ref oConnection, ref lstProveedores, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaProveedores", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
