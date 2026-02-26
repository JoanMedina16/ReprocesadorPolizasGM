using AD;
using AD.Objetos.MOTF;
using INFO.Objetos.MOTF;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Objetos.MOTF
{
    public class LOGI_PortesViaje_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_PortesViaje_AD oViajeData = null;
        const string CONST_CLASE = "LOGI_PortesViaje_PD.cs";
        const string CONST_MODULO = "Portes viajes";

        public LOGI_PortesViaje_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oViajeData = new LOGI_PortesViaje_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaBandejaportesViaje(string sUsuarioID, LOGI_PorteViaje_INFO oViaje, ref List<LOGI_PorteViaje_INFO> lstViajes)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oViajeData.ListaPortesViajes(ref oConnection, ref lstViajes, oViaje, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaBandejaportesViaje", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
