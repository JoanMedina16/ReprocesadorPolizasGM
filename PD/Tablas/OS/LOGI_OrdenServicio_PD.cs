using AD;
using AD.Tablas.OS;
using INFO.Tablas.OS;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.OS
{
    public class LOGI_OrdenServicio_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_OrdenServicio_AD Ordenes = null;
        const string CONST_CLASE = "LOGI_OrdenServicio_PD.cs";
        const string CONST_MODULO = "Ordenes de servicio";

        public LOGI_OrdenServicio_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            Ordenes = new LOGI_OrdenServicio_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaOrdenes(string sUsuarioID, ref List<LOGI_OrdenServicio_INFO> lstOrdenes, LOGI_OrdenServicio_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = Ordenes.Listaordenes(ref oConnection, ref lstOrdenes, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaOrdenes", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
