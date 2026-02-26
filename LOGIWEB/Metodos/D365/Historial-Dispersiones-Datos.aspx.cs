using INFO.Objetos;
using INFO.Objetos.SAT;
using INFO.Tablas;
using INFO.Tablas.D365;
using PD.Herramientas;
using PD.Tablas.D365;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.D365
{
    public partial class Historial_Dispersiones_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaHistorial(LOGI_Plantilla_INFO oParam)
        {
            int iTopresultados = -1;
            List<LOGI_Plantilla_INFO> lstPlantillas = new List<LOGI_Plantilla_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {


                if (string.IsNullOrEmpty(oParam.fechainicio) && string.IsNullOrEmpty(oParam.fechafin))
                {
                    //cuando no tenemos los filtros agregados forzamos a que muestre los resultados de los últimos tres días
                    oParam.fechainicio = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy");
                    oParam.fechafin = DateTime.Now.ToString("dd/MM/yyyy");
                    iTopresultados = 150;
                }

                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Plantilla_PD(oTool.CONST_CONNECTION).ListaPlantillas(oUser.iUsuario.ToString(), oParam, ref lstPlantillas, iTopresultados);
                    if (lstPlantillas.Count > 0)
                    {
                        LOGI_ConfiguracionD365_INFO oConfig = new LOGI_ConfiguracionD365_INFO();
                        new LOGI_ConfiguracionD365_PD(oTool.CONST_CONNECTION).ListaConfiguracion(oUser.iUsuario.ToString(), ref oConfig);
                        lstPlantillas.ForEach(x=> {
                            x.pathcabecera = string.Format(@"{0}\{1}", oConfig.plantilla, x.pathcabecera);
                            x.pathdetalle = string.Format(@"{0}\{1}", oConfig.plantilla, x.pathdetalle);
                            x.pathcabecera = LOGI_Rijndael_PD.EncryptRijndael(x.pathcabecera);
                            x.pathdetalle = LOGI_Rijndael_PD.EncryptRijndael(x.pathdetalle);
                            x.pathcabecera = LOGI_WebTools_PD.Base64Encode(x.pathcabecera);
                            x.pathdetalle = LOGI_WebTools_PD.Base64Encode(x.pathdetalle);
                        });
                    }
                    oResponse.data = lstPlantillas;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Eliminaplantilla(LOGI_Plantilla_INFO oParam)
        {
            List<LOGI_Plantilla_INFO> lstPlantillas = new List<LOGI_Plantilla_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Plantilla_PD(oTool.CONST_CONNECTION).Eliminaplantilla(oUser.iUsuario.ToString(), oParam);
                    oResponse.data = lstPlantillas;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }



        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO GeneraListadoFacturas(string sExcelPath)
        {
            List<LOGI_XMLS_INFO> lstCfdis = new List<LOGI_XMLS_INFO>();
            LOGI_XMLS_INFO oParam = new LOGI_XMLS_INFO();
             LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    string sResponse = "";
                    DataTable DsExcel = new DataTable();
                    //descomponemos el excel y extraemos la priemara columna 
                    string sFullpath = string.Format(@"{0}\{1}", oTool.RepoTemporal(), sExcelPath);
                    if (File.Exists(sFullpath))
                    {
                        if (oTool.RecuperaDataExcelV2(sFullpath, out sResponse, ref DsExcel))
                        {
                            string sFoliosIN = string.Empty, sSeriesIN = string.Empty;
                            foreach (DataRow row in DsExcel.Rows)
                            {

                                string sCandena = Convert.ToString(row[0]).Trim();
                            }
                           /* if (lstPolizas.Count > 0)
                            {
                                LOGI_CargaInicial_PD oCargaInicial = new LOGI_CargaInicial_PD(oTool.CONST_CONNECTION);
                                oResponse.estatus = oCargaInicial.ProcesaCargaInicial(oTool.UsuarioSession().iUsuario.ToString(), lstPolizas);
                                oResponse.mensaje = oResponse.estatus;
                            }
                            else throw new Exception("Asegurese de que no haya datos vacios");*/

                        }
                        else oResponse.mensaje = "No se ha encontrado la información del archivo Excel" ;
                    }
                    else oResponse.mensaje = "No se ha encontrado el archivo Excel con la información de facturas";

                   
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        #endregion "WEBMETHODS"
    }
}