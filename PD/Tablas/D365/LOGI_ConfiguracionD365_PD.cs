using AD;
using AD.Tablas.D365;
using INFO.Tablas.D365;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.D365
{
  public  class LOGI_ConfiguracionD365_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_ConfiguracionD365_AD oConfiguracionAD = null;
        const string CONST_CLASE = "LOGI_ConfiguracionD365_PD.cs";
        const string CONST_MODULO = "Configuración D365";

        public LOGI_ConfiguracionD365_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oConfiguracionAD = new  LOGI_ConfiguracionD365_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaConfiguracion(string sUsuarioID, ref LOGI_ConfiguracionD365_INFO oConfiguracion)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oConfiguracionAD.ListaConfiguracion(ref oConnection, ref oConfiguracion, out sConsultaSql);
                oConfiguracion.URLApi = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.URLApi);
                oConfiguracion.URLApilogin = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.URLApilogin);
                oConfiguracion.usuariod365 = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.usuariod365);
                oConfiguracion.passusrd365 = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.passusrd365);
                oConfiguracion.ciad365 = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.ciad365);
                oConfiguracion.aprobador = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.aprobador);
                oConfiguracion.Conexion_eqv = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.Conexion_eqv);
                oConfiguracion.conexionzap = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.conexionzap);
                oConfiguracion.aprobadordisper = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.aprobadordisper);
                oConfiguracion.cuentas_soport_mail = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.cuentas_soport_mail);
                oConfiguracion.password_mail = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.password_mail);
                oConfiguracion.api_tms = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.api_tms);
                oConfiguracion.url_tms = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.url_tms);
                oConfiguracion.host_tms = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.host_tms);

                //oConfiguracion.urlAPIGM= oConfiguracion.urlAPIGM;
                //oConfiguracion.EndPointConsulta = oConfiguracion.EndPointConsulta;
                //oConfiguracion.EndPointRespuesta = oConfiguracion.EndPointRespuesta;
                //oConfiguracion.rfcAPI = oConfiguracion.rfcAPI;
                oConfiguracion.UsuarioAPIGM = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.UsuarioAPIGM);
                oConfiguracion.PasswordAPIGM = LOGI_Rijndael_PD.DecryptRijndael(oConfiguracion.PasswordAPIGM);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaConfiguracion", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ActualizaConfiguracion(string sUsuarioID, LOGI_ConfiguracionD365_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                oParam.URLApi = LOGI_Rijndael_PD.EncryptRijndael(oParam.URLApi);
                oParam.URLApilogin = LOGI_Rijndael_PD.EncryptRijndael(oParam.URLApilogin);
                oParam.usuariod365 = LOGI_Rijndael_PD.EncryptRijndael(oParam.usuariod365);
                oParam.passusrd365 = LOGI_Rijndael_PD.EncryptRijndael(oParam.passusrd365);
                oParam.ciad365 = LOGI_Rijndael_PD.EncryptRijndael(oParam.ciad365);
                oParam.aprobador = LOGI_Rijndael_PD.EncryptRijndael(oParam.aprobador);
                oParam.Conexion_eqv = LOGI_Rijndael_PD.EncryptRijndael(oParam.Conexion_eqv);
                oParam.conexionzap = LOGI_Rijndael_PD.EncryptRijndael(oParam.conexionzap);
                oParam.aprobadordisper = LOGI_Rijndael_PD.EncryptRijndael(oParam.aprobadordisper);
                oParam.cuentas_soport_mail = LOGI_Rijndael_PD.EncryptRijndael(oParam.cuentas_soport_mail);
                oParam.password_mail = LOGI_Rijndael_PD.EncryptRijndael(oParam.password_mail);
                oParam.api_tms = LOGI_Rijndael_PD.EncryptRijndael(oParam.api_tms);
                oParam.url_tms = LOGI_Rijndael_PD.EncryptRijndael(oParam.url_tms);
                oParam.host_tms = LOGI_Rijndael_PD.EncryptRijndael(oParam.host_tms);

                //oParam.urlAPIGM = oParam.urlAPIGM;
                //oParam.EndPointConsulta = oParam.EndPointConsulta;
                //oParam.EndPointRespuesta = oParam.EndPointRespuesta;
                //oParam.rfcAPI = oParam.rfcAPI;
                oParam.UsuarioAPIGM = LOGI_Rijndael_PD.EncryptRijndael(oParam.UsuarioAPIGM);
                oParam.PasswordAPIGM = LOGI_Rijndael_PD.EncryptRijndael(oParam.PasswordAPIGM);

                sReponse = oConfiguracionAD.ActualizaConfiguracion(ref oConnection, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ActualizaConfiguracion", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ActualizaDisponibilid(string sUsuarioID, LOGI_ConfiguracionD365_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oConfiguracionAD.Actualizadisponibilidad(ref oConnection, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ActualizaDisponibilid", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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

