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
  public  class LOGI_Documentos_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Documentos_AD oDocumentosAD = null;
        const string CONST_CLASE = "LOGI_Documentos_PD.cs";
        const string CONST_MODULO = "Documentos D365";

        public LOGI_Documentos_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oDocumentosAD = new LOGI_Documentos_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaDocumentos(string sUsuarioID, ref List<LOGI_Documentos_INFO> lstDocumentos, LOGI_Documentos_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oDocumentosAD.lstDocumentos(ref oConnection, ref lstDocumentos, oParam, out sConsultaSql);
                lstDocumentos.ForEach(x => {
                    x.diario = LOGI_Rijndael_PD.DecryptRijndael(x.diario);
                    x.metodo = LOGI_Rijndael_PD.DecryptRijndael(x.metodo);});
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaDocumentos", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaDocumentosProcesa(string sUsuarioID, ref List<LOGI_DocumentosProcesa_INFO> lstDocumentos, LOGI_DocumentosProcesa_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oDocumentosAD.lstDocumentosProcesa(ref oConnection, ref lstDocumentos, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaDocumentos", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaDocumentosXUsuario(string sUsuarioID, ref List<LOGI_Documentos_INFO> lstDocumentos, string sUsuario, int iUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oDocumentosAD.lstDocumentosXusuario(ref oConnection, ref lstDocumentos, sUsuario, iUsuario, out sConsultaSql);
                lstDocumentos.ForEach(x => {
                    x.diario = LOGI_Rijndael_PD.DecryptRijndael(x.diario);
                    x.metodo = LOGI_Rijndael_PD.DecryptRijndael(x.metodo);
                });
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaDocumentos", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaDocumentosProcesaXUsuario(string sUsuarioID, ref List<LOGI_DocumentosProcesa_INFO> lstDocumentos, string sUsuario, int iUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oDocumentosAD.lstDocumentosProcesaXusuario(ref oConnection, ref lstDocumentos, sUsuario, iUsuario, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaDocumentos", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string Actualizadocumento(string sUsuarioID, LOGI_Documentos_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                oParam.diario = LOGI_Rijndael_PD.EncryptRijndael(oParam.diario);
                oParam.metodo = LOGI_Rijndael_PD.EncryptRijndael(oParam.metodo);
                sReponse = oDocumentosAD.Actualizadocumentos(ref oConnection, sUsuarioID, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Actualizadocumento", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string Creadocumento(string sUsuarioID, LOGI_Documentos_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                oParam.diario = LOGI_Rijndael_PD.EncryptRijndael(oParam.diario);
                oParam.metodo = LOGI_Rijndael_PD.EncryptRijndael(oParam.metodo);
                sReponse = oDocumentosAD.Creadocumento(ref oConnection, sUsuarioID, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Creadocumento", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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
