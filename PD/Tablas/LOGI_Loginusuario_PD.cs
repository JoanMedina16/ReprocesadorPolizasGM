using AD;
using AD.Objetos;
using INFO.Tablas;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace PD.Tablas
{
    public class LOGI_Loginusuario_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Loginusuario_AD oUsuarioAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Loginusuario_PD.cs";
        const string CONST_MODULO = "Login usuario";
        string CON_DATABASE = string.Empty;

        public LOGI_Loginusuario_PD(string sConnection)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(sConnection);
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oUsuarioAD = new LOGI_Loginusuario_AD();
            oTool = new LOGI_Tools_PD();
            CON_DATABASE = builder.InitialCatalog;
        } 

        public string ValidaLoginUsuario(ref LOGI_Usuarios_INFO oUsuario, LOGI_Usuarios_INFO oParam)
        {
            
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oUsuarioAD.BuscaUsuarioSystem(ref oConnection, ref oUsuario, oParam, out sConsultaSql, CON_DATABASE);
                if (sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    oParam.sContrasenia = LOGI_EncryptaOTM_PD.Encripta(oParam.sContrasenia);
                    if (oParam.sContrasenia.Equals(oUsuario.sContrasenia, StringComparison.InvariantCultureIgnoreCase))
                    {
                        /*sReponse = oUsuarioAD.ValidaDerechosSystem(ref oConnection, out sConsultaSql, oUsuario.iUsuario);
                        if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                            sReponse = "DERECHOS";*/
                    }
                    else sReponse = "PASS";

                }
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ValidaLoginUsuario", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = "No se han podido validar los datos del usuario. Utiliza las credenciales correctas";
                //sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListausuariosOPE(ref List<LOGI_Usuarios_INFO> lstUsuarios, LOGI_Usuarios_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oUsuarioAD.ListausuariosOPE(ref oConnection,  ref lstUsuarios, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listausuarios", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaSucursal(ref LOGI_Usuarios_INFO oUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oUsuarioAD.ValidaSucursal(ref oConnection, out sConsultaSql, ref oUsuario);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaSucursal", CONST_CLASE, CONST_MODULO, sConsultaSql);
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
