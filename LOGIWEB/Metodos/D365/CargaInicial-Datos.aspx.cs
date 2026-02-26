using INFO.Enums;
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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.D365
{
    public partial class CargaInicial_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Siempre que se accede a este archivo por url se redirige al login
            Response.Redirect("../../login.aspx");
        }

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaPolizas(LOGI_Polizas_INFO Poliza)
        {
            List<LOGI_Polizas_INFO> lstPolizas = new List<LOGI_Polizas_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    Poliza.id_tipo_doc = Convert.ToInt32(eDocumentoPolizas.CARGA_INICIAL);
                        Poliza.estatus = -1;
                    oResponse.estatus = new LOGI_Polizas_PD(oTool.CONST_CONNECTION).ListaPolizas(oUser.iUsuario.ToString(), Poliza, ref lstPolizas);
                    oResponse.data = lstPolizas;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO CargaAsientosInicial(String sArchivo)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                try
                {


                    if (oTool.ValidaAcceso(CONST_MODULO))
                    {
                        string sResponse = "";
                        DataTable DsExcel = new DataTable();
                        oResponse.estatus = "ERROR";
                        string sFullpath = string.Format(@"{0}\{1}", oTool.RepoTemporal(), sArchivo);
                        if (File.Exists(sFullpath))
                        {
                            List<LOGI_Polizas_INFO> lstPolizas = new List<LOGI_Polizas_INFO>();
                            LOGI_Polizas_INFO temp = null;
                            if (oTool.RecuperaDataExcelV2(sFullpath, out sResponse, ref DsExcel))
                            {
                                foreach (DataRow row in DsExcel.Rows)
                                {
                                    temp = new LOGI_Polizas_INFO();

                                    temp.recIdD365 = Convert.ToString(row[0]).Trim();
                                    temp.uuid = Convert.ToString(row[1]).Trim();
                                    temp.serie = Convert.ToString(row[2]).Trim();
                                    temp.folio = Convert.ToString(row[3]).Trim();

                                    if (!string.IsNullOrEmpty(temp.recIdD365) && !string.IsNullOrEmpty(temp.uuid) && !string.IsNullOrEmpty(temp.serie) && !string.IsNullOrEmpty(temp.folio))
                                        lstPolizas.Add(temp);
                                }
                                if (lstPolizas.Count > 0)
                                {
                                    LOGI_CargaInicial_PD oCargaInicial = new LOGI_CargaInicial_PD(oTool.CONST_CONNECTION);
                                    oResponse.estatus = oCargaInicial.ProcesaCargaInicial(oTool.UsuarioSession().iUsuario.ToString(), lstPolizas);
                                    oResponse.mensaje = oResponse.estatus;
                                }
                                else throw new Exception("Asegurese de que no haya datos vacios");

                            }
                            else
                                throw new Exception(sResponse);

                        }
                        else oResponse.mensaje = "El archivo EXCEL no se ha podido cargar, favor de reintentarlo";
                    }
                    else oResponse.estatus = "-2";
                }
                catch (Exception ex)
                {
                    oResponse.estatus = "ERROR";
                    oResponse.mensaje = ex.Message;
                }
            }
            return oResponse;
        }



        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Extraeremesabebidas(String sArchivo)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                try
                {


                    if (oTool.ValidaAcceso(CONST_MODULO))
                    {
                        string sResponse = "";
                        DataTable DsExcel = new DataTable();
                        oResponse.estatus = "ERROR";
                        string sFullpath = string.Format(@"{0}\{1}", oTool.RepoTemporal(), sArchivo);
                        if (File.Exists(sFullpath))
                        {
                            List<LOGI_XMLS_INFO> lstPolizas = new List<LOGI_XMLS_INFO>();
                            LOGI_XMLS_INFO temp = null;
                            if (oTool.RecuperaDataExcelV2(sFullpath, out sResponse, ref DsExcel))
                            {
                                string Folio = string.Empty, Serie = string.Empty, sFolioExcel = string.Empty;
                                int Linieas = 0;
                                foreach (DataRow row in DsExcel.Rows)
                                {
                                    sFolioExcel = Convert.ToString(row[0]).Trim();
                                    if (!string.IsNullOrEmpty(sFolioExcel))
                                    {
                                        Folio += string.Format("{0},", Regex.Replace(sFolioExcel, @"[A-Za-z ]", string.Empty).Trim());
                                        Serie += string.Format("'{0}',", Regex.Replace(sFolioExcel, @"[0-9\-]", string.Empty).Trim());
                                        Linieas++;
                                    }
                                }
                                temp = new LOGI_XMLS_INFO();
                                temp.sFoliosIN = Folio.TrimEnd(',');
                                temp.sSeriesIN = Serie.TrimEnd(',');
                                temp.bContent = false;
                                oResponse.estatus = new LOGI_Historico_XMLS_PD(oTool.CONST_CONNECTION).ListaCFDIS(ref lstPolizas, temp);
                                if (oResponse.estatus.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    new CargaInicial_Datos().GrabaFacturas(temp);
                                    if (lstPolizas.Count == Linieas)
                                        oResponse.mensaje = string.Format("Se han encontrado el total de las facturas proporcionadas en el archivo. Presiona el botón de descargar para visualizarlas en archivo .ZIP");
                                    else
                                        oResponse.mensaje = string.Format("Se han encontrado un total de <b>{0} factura(s) de {1} factura(s)</b> proporcionadas en archivo. Presiona el botón de descargar para visualizarlas en archivo .ZIP", lstPolizas.Count, Linieas);
                                    oResponse.data = lstPolizas;
                                }else oResponse.mensaje = oResponse.estatus; 

                            }
                            else
                                throw new Exception(sResponse);

                        }
                        else oResponse.mensaje = "El archivo EXCEL no se ha podido cargar, favor de reintentarlo";
                    }
                    else oResponse.estatus = "-2";
                }
                catch (Exception ex)
                {
                    oResponse.estatus = "ERROR";
                    oResponse.mensaje = ex.Message;
                }
            }
            return oResponse;
        }
        void GrabaFacturas(LOGI_XMLS_INFO oTemp)
        {
            try
            {
                Session["FACTURAS_BEBIDAS"] = oTemp;
            }
            catch { }
        }
        #endregion "WEBMETHODS"
    }
}