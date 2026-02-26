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
    public class LOGI_Modulos_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Modulos_AD oModuloAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Modulos_PD.cs";
        const string CONST_MODULO = "Modulos";

        public LOGI_Modulos_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oModuloAD = new LOGI_Modulos_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string Listamodulos(ref List<LOGI_Modulos_INFO> lstModulos)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oModuloAD.Listamodulos(ref oConnection, ref lstModulos, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listamodulos", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string Listapermisosusuario(ref List<LOGI_Modulos_INFO> lstModulos, int UsuarioID, string sClavemodulo = "")
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oModuloAD.Listapermisosusuario(ref oConnection, ref lstModulos, UsuarioID, out sConsultaSql, sClavemodulo);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listapermisosusuario", CONST_CLASE, CONST_MODULO, sConsultaSql);
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
