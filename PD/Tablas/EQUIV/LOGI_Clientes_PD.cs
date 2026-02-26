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
    public class LOGI_Clientes_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Clientes_AD oClientes= null;
        const string CONST_CLASE = "LOGI_Clientes_PD.cs";
        const string CONST_MODULO = "Clientes";

        public LOGI_Clientes_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oClientes = new LOGI_Clientes_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaClientes(string sUsuarioID, LOGI_Catalogos_INFO oParam, ref List<LOGI_Catalogos_INFO> lstClientes)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oClientes.RecuperaClientes(ref oConnection, ref lstClientes, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaClientes", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
