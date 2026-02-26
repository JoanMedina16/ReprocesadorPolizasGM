using D365;
using D365.INFOD;
using INFO.Enums;
using INFO.Objetos;
using INFO.Objetos.LMAE;
using INFO.Objetos.SAT;
using INFO.Tablas;
using INFO.Tablas.D365;
using INFO.Tablas.OPE;
using PD.Herramientas;
using PD.Objetos.LMAE;
using PD.Objetos.OPE;
using PD.Tablas.D365;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.D365
{
    public partial class PolizaContable_Datos : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //Siempre que se accede a este archivo por url se redirige al login
            Response.Redirect("../../login.aspx");
        }

        public static int CONST_MODULO = 130;

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaPolizas(LOGI_Polizas_INFO Poliza)
        {
            int iTopresultados = 0;
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
                    if (oUser.iPerfil != 1)
                    {
                        if (string.IsNullOrEmpty(Poliza.sDocumentosIN))
                        {
                            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
                            new LOGI_Documentos_PD(oTool.CONST_CONNECTION).ListaDocumentosXUsuario(oUser.iUsuario.ToString(), ref lstDocumentos, "", oUser.iUsuario);
                            foreach (LOGI_Documentos_INFO x in lstDocumentos)
                                Poliza.sDocumentosIN += string.Format("{0},", x.numerodoc);
                            Poliza.sDocumentosIN = Poliza.sDocumentosIN.TrimEnd(',');
                        }
                    }

                    if (!((Poliza.estatus == 1 && !(string.IsNullOrEmpty(Poliza.sDocumentosIN) && string.IsNullOrEmpty(Poliza.folioserie) && string.IsNullOrEmpty(Poliza.FolioAsiento) && string.IsNullOrEmpty(Poliza.FechaConInicio) && string.IsNullOrEmpty(Poliza.FechaConFin) && string.IsNullOrEmpty(Poliza.FechaCreInicio) && string.IsNullOrEmpty(Poliza.FechaCreFin))) || (Poliza.estatus == 2 && !(string.IsNullOrEmpty(Poliza.sDocumentosIN) && string.IsNullOrEmpty(Poliza.folioserie) && string.IsNullOrEmpty(Poliza.FechaCreInicio) && string.IsNullOrEmpty(Poliza.FechaCreFin)))))
                    {
                        Poliza.FechaCreInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd/MM/yyyy");
                        Poliza.FechaCreFin = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    //else //if (string.IsNullOrEmpty(Poliza.FechaCreInicio) && string.IsNullOrEmpty(Poliza.FechaCreFin))
                    //{
                    //    //cuando no tenemos los filtros agregados forzamos a que muestre los resultados de los últimos tres días
                    //    Poliza.FechaCreInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd/MM/yyyy");
                    //    Poliza.FechaCreFin = DateTime.Now.ToString("dd/MM/yyyy");
                    //}


                        oResponse.estatus = new LOGI_Polizas_PD(oTool.CONST_CONNECTION).ListaPolizas(oUser.iUsuario.ToString(), Poliza, ref lstPolizas, iTopresultados);
                    oResponse.data = lstPolizas;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO InsertaPeticion(LOGI_Peticion_INFO Peticion)
        {
            int iTopresultados = 0;
            List<LOGI_Peticion_INFO> lstPeticiones = new List<LOGI_Peticion_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();

            Peticion.FechaInicial = DateTime.ParseExact(Peticion.FechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    if (!string.IsNullOrEmpty(Peticion.Proceso))
                    {
                        Peticion.Usuario = oUser.iUsuario.ToString();
                        oResponse.estatus = new LOGI_Polizas_PD(oTool.CONST_CONNLOG).InsertaPeticion(oUser.iUsuario.ToString(), Peticion, ref lstPeticiones, iTopresultados);
                        oResponse.data = lstPeticiones;
                        oResponse.mensaje = oResponse.estatus;
                    }
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO CargaXML(String polizaID, String sArchivo)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = "ERROR";
                    string sFullpath = string.Format(@"{0}\{1}", oTool.RepoTemporal(), sArchivo);
                    if (File.Exists(sFullpath))
                    {
                        List<LOGI_Polizas_INFO> lstPolizas = new List<LOGI_Polizas_INFO>();
                        LOGI_Polizas_INFO oParam = new LOGI_Polizas_INFO();
                        oParam.FolioAsiento = polizaID;
                        oParam.estatus = -1;
                        new LOGI_Polizas_PD(oTool.CONST_CONNECTION).ListaPolizas(oUser.iUsuario.ToString(), oParam, ref lstPolizas);
                        if (lstPolizas.Count == 1)
                        {
                            oParam = new LOGI_Polizas_INFO();
                            oParam = lstPolizas[0];
                            string response = string.Empty;
                            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
                            if (new LOGI_Tools_PD().DevuelveXMLObject(out response, ref oXML, sPathXML: sFullpath, sEmisor: oParam.rfc, total: oParam.total, IVA: oParam.impuesto))
                            {
                                bool bCarga = false;
                                lstPolizas = new List<LOGI_Polizas_INFO>();
                                oParam = new LOGI_Polizas_INFO();
                                oParam.uuid = oXML.Complemento.TimbreFiscalDigital.UUID;
                                oParam.estatus = -1;
                                new LOGI_Polizas_PD(oTool.CONST_CONNECTION).ListaPolizas(oUser.iUsuario.ToString(), oParam, ref lstPolizas);
                                if (lstPolizas.Count == 1 && lstPolizas[0].FolioAsiento == polizaID)
                                    bCarga = true;
                                else if (lstPolizas.Count == 0)
                                    bCarga = true;

                                if (bCarga)
                                {
                                    bool bContinua = true;
                                    if (lstPolizas.Count == 1)
                                    {
                                        if (lstPolizas[0].uuid.Equals(oXML.Complemento.TimbreFiscalDigital.UUID, StringComparison.InvariantCultureIgnoreCase))
                                            bContinua = false;
                                    }
                                    if (bContinua)
                                    {
                                        oParam = new LOGI_Polizas_INFO();
                                        oParam.FolioAsiento = polizaID;
                                        oParam.uuid = oXML.Complemento.TimbreFiscalDigital.UUID;
                                        oParam.folio = oXML.Folio;
                                        oParam.serie = oXML.Serie;
                                        oParam.docxml = oXML.CFDIContent;
                                        oParam.estatus = -1;
                                        oResponse.estatus = new LOGI_Polizas_PD(oTool.CONST_CONNECTION).Actualizapoliza(oUser.iUsuario.ToString(), oParam);
                                        oResponse.mensaje = oResponse.estatus;
                                    }
                                    else oResponse.mensaje = string.Format(@"El archivo XML con UUID ""{0}"" ya se encuentra utilizado por este registro que se intenta actualizar, favor de verificar y reintentarlo nuevamente.", oXML.Complemento.TimbreFiscalDigital.UUID);
                                }
                                else oResponse.mensaje = string.Format(@"El archivo XML con UUID ""{0}"" ya se encuentra registrado para otro movimiento contable, favor de verificar y reintentarlo nuevamente.", oXML.Complemento.TimbreFiscalDigital.UUID);
                            }
                            else oResponse.mensaje = string.Format("El archivo XML no se ha podido cargar. {0}", response);
                        }
                        else oResponse.mensaje = "La información de la póliza no ha podido ser validada, favor de reintentarlo";
                    }
                    else oResponse.mensaje = "El archivo XML no se ha podido cargar, favor de reintentarlo";
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO CargaViaticosLineaXML(String polizaID, int Linea, String sTokendocto, String sArchivo)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = "ERROR";
                    string sFullpath = string.Format(@"{0}\{1}", oTool.RepoTemporal(), sArchivo);
                    if (File.Exists(sFullpath))
                    {
                        List<LOGI_Polizas_detalle_INFO> lstDetalle = new List<LOGI_Polizas_detalle_INFO>();
                        LOGI_Polizas_detalle_INFO oParam = new LOGI_Polizas_detalle_INFO();
                        oParam.FolioAsiento = polizaID;
                        oParam.linea = Linea;
                        oParam.tipo_documento = Convert.ToInt32(eDocumentoPolizas.COMPROBACIÓN_DE_VIATICOS);
                        new LOGI_Polizas_detalle_PD(oTool.CONST_CONNECTION).ListadetallePoliza(oTool.CONST_EQUIV_CONNECTION, oUser.iUsuario.ToString(), oParam, ref lstDetalle);

                        if (lstDetalle.Count == 1)
                        {
                            oParam = lstDetalle[0];

                            Decimal totalEvaluar = oParam.cargo;
                            #region "busqueda de iguales, iva y linea de cargo para sumar y validar importe de XML"                            
                            lstDetalle = new List<LOGI_Polizas_detalle_INFO>();
                            LOGI_Polizas_detalle_INFO oDoctoRef = new LOGI_Polizas_detalle_INFO();
                            oDoctoRef.refdoc = sTokendocto;
                            oDoctoRef.tipo_documento = Convert.ToInt32(eDocumentoPolizas.COMPROBACIÓN_DE_VIATICOS);
                            new LOGI_Polizas_detalle_PD(oTool.CONST_CONNECTION).ListadetallePoliza(oTool.CONST_EQUIV_CONNECTION, oUser.iUsuario.ToString(), oDoctoRef, ref lstDetalle);
                            if (lstDetalle.Count > 0)
                                totalEvaluar = lstDetalle.Sum(x => x.cargo);
                            #endregion "busqueda de iguales, iva y linea de cargo para sumar y validar importe de XML"

                            string response = string.Empty;
                            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
                            bool bCarga = false;
                            if (new LOGI_Tools_PD().DevuelveXMLObject(out response, ref oXML, sPathXML: sFullpath, total: totalEvaluar))
                            {
                                lstDetalle = new List<LOGI_Polizas_detalle_INFO>();
                                oParam = new LOGI_Polizas_detalle_INFO();
                                oParam.uuid = oXML.Complemento.TimbreFiscalDigital.UUID;
                                oParam.tipo_documento = Convert.ToInt32(eDocumentoPolizas.COMPROBACIÓN_DE_VIATICOS);
                                new LOGI_Polizas_detalle_PD(oTool.CONST_CONNECTION).ListadetallePoliza(oTool.CONST_EQUIV_CONNECTION, oUser.iUsuario.ToString(), oParam, ref lstDetalle);


                                if (lstDetalle.Count == 1 && lstDetalle[0].FolioAsiento == polizaID)
                                    bCarga = true;
                                else if (lstDetalle.Count == 0)
                                    bCarga = true;

                                if (bCarga)
                                {
                                    bool bContinua = true;
                                    if (lstDetalle.Count == 1)
                                    {
                                        if (lstDetalle[0].uuid.Equals(oXML.Complemento.TimbreFiscalDigital.UUID, StringComparison.InvariantCultureIgnoreCase))
                                            bContinua = false;
                                    }
                                    if (bContinua)
                                    {
                                        oParam = new LOGI_Polizas_detalle_INFO();
                                        oParam.FolioAsiento = polizaID;
                                        oParam.uuid = oXML.Complemento.TimbreFiscalDigital.UUID;
                                        oParam.XML = oXML.CFDIContent;
                                        oParam.linea = Linea;
                                        oParam.tipo_documento = Convert.ToInt32(eDocumentoPolizas.COMPROBACIÓN_DE_VIATICOS);
                                        oResponse.estatus = new LOGI_Polizas_detalle_PD(oTool.CONST_CONNECTION).ActualizaLineaDetalle(oUser.iUsuario.ToString(), oParam);
                                        oResponse.mensaje = oResponse.estatus;
                                    }
                                    else oResponse.mensaje = string.Format(@"El archivo XML con UUID ""{0}"" ya se encuentra utilizado por este registro que se intenta actualizar, favor de verificar y reintentarlo nuevamente.", oXML.Complemento.TimbreFiscalDigital.UUID);

                                }
                                else oResponse.mensaje = string.Format(@"El archivo XML con UUID ""{0}"" ya se encuentra registrado para otro movimiento contable, favor de verificar y reintentarlo nuevamente.", oXML.Complemento.TimbreFiscalDigital.UUID);

                            }
                            else oResponse.mensaje = string.Format("El archivo XML no se ha podido cargar. {0}", response);
                        }
                        else oResponse.mensaje = "La información de la póliza no ha podido ser validada, favor de reintentarlo";
                    }
                    else oResponse.mensaje = "El archivo XML no se ha podido cargar, favor de reintentarlo";
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Eliminapolizas(List<LOGI_Polizas_INFO> lstPolizas)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    lstPolizas.ForEach(x => x.estatus = 3);
                    oResponse.estatus = new LOGI_Polizas_PD(oTool.CONST_CONNECTION).ActualizaPolizasTransaccion(oUser.iUsuario.ToString(), lstPolizas, -1);
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaComentario(LOGI_Polizas_INFO oParam)
        {
            LOGI_Polizas_INFO oPoliza = new LOGI_Polizas_INFO();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Polizas_PD(oTool.CONST_CONNECTION).RecuperaComentario(oUser.iUsuario.ToString(), oParam, ref oPoliza);
                    oResponse.data = oPoliza;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ReiniciaErrores(List<LOGI_Polizas_INFO> lstPolizas)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    lstPolizas.ForEach(x => x.estatus = 0);
                    oResponse.estatus = new LOGI_Polizas_PD(oTool.CONST_CONNECTION).ActualizaPolizasTransaccion(oUser.iUsuario.ToString(), lstPolizas);
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO RecuperaTransaccion(LOGI_Transacciones_INFO Transacc)
        {
            LOGI_Transacciones_INFO oTransaccion = new LOGI_Transacciones_INFO();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Transacciones_PD(oTool.CONST_CONNECTION).RecuperaJSON(oUser.iUsuario.ToString(), ref oTransaccion, Transacc.FolioAsiento);
                    oResponse.data = oTransaccion;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListadetallePoliza(LOGI_Polizas_detalle_INFO Detalle)
        {
            List<LOGI_Polizas_detalle_INFO> lstDetalle = new List<LOGI_Polizas_detalle_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Polizas_detalle_PD(oTool.CONST_CONNECTION).ListadetallePoliza(oTool.CONST_EQUIV_CONNECTION, oUser.iUsuario.ToString(), Detalle, ref lstDetalle);
                    oResponse.data = lstDetalle;
                    oResponse.mensaje = oResponse.estatus;
                    var oDetalle = lstDetalle.FirstOrDefault(x => x.valido == 0);
                    if (oDetalle != null)
                        oResponse.estatus = "EQUIVALENCIAS";
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaBitacoradePolizasD365(LOGI_Polizas_detalle_INFO Detalle)
        {
            List<LOGI_Polizas_detalle_INFO> lstDetalle = new List<LOGI_Polizas_detalle_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Polizas_detalle_PD(oTool.CONST_CONNECTION).ListadetallePoliza(oTool.CONST_EQUIV_CONNECTION, oUser.iUsuario.ToString(), Detalle, ref lstDetalle);
                    oResponse.data = lstDetalle;
                    oResponse.mensaje = oResponse.estatus;
                    var oDetalle = lstDetalle.FirstOrDefault(x => x.valido == 0);
                    if (oDetalle != null)
                        oResponse.estatus = "EQUIVALENCIAS";
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listamaestraunidades(LOGI_ListaMaestra_INFO oParam)
        {
            LOGI_ListaMaestra_PD oListam = null;
            List<LOGI_ListaMaestra_INFO> lstUnidades = new List<LOGI_ListaMaestra_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();

            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oParam.cia = 67; //fijo por prisasa
                    oListam = new LOGI_ListaMaestra_PD(oTool.CONST_CONNECTIONLM);
                    oResponse.estatus = "ERROR";
                    oResponse.estatus = oListam.ListaUnidadesmaestra("", oTool.CONST_EQUIV_CONNECTION, oParam, ref lstUnidades);
                    if (oResponse.estatus == "OK")
                    {
                        oResponse.data = lstUnidades;

                    }
                    else oResponse.mensaje = oResponse.estatus;

                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }



        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaConsumosliquidaciones(LOGI_LiquidacionCombus_INFO oParam)
        {
            LOGI_Liquidacion_PD oLiquidacion = null;
            List<LOGI_LiquidacionCombus_INFO> lstUnidades = new List<LOGI_LiquidacionCombus_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();

            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {

                    oLiquidacion = new LOGI_Liquidacion_PD("", oTool.CONST_CONNECTION);
                    oResponse.estatus = "ERROR";
                    oResponse.estatus = oLiquidacion.ListadodeCombustible(oUser.sUsuario, ref lstUnidades, oParam);
                    if (oResponse.estatus == "OK")
                    {
                        oResponse.data = lstUnidades;

                    }
                    else oResponse.mensaje = oResponse.estatus;

                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ProcesapolizaCombustible(List<LOGI_LiquidacionCombus_INFO> lstCargascheck, LOGI_LiquidacionCombus_INFO oParam)
        {
            LOGI_Liquidacion_PD oLiquidacion = null;
            List<LOGI_LiquidacionCombus_INFO> lstUnidades = new List<LOGI_LiquidacionCombus_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            string sresponse = string.Empty, sFolioD365 = string.Empty, FolioPoliza = string.Empty;

            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    LOGI_Credencial_D365 oCred = new LOGI_Credencial_D365();
                    LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
                    LOGI_TMSConnection_D365 oConnectionTMS = null;
                    new PolizaContable_Datos().GetConfig(oUser.sUsuario, ref oCred, ref oConfiguracion, oTool.CONST_CONNECTION);
                    oLiquidacion = new LOGI_Liquidacion_PD(oTool.CONST_CONNLOG, oTool.CONST_CONNECTION);
                    oResponse.estatus = "ERROR";
                    oResponse.estatus = oLiquidacion.CreaPolizaContableD365(oUser.sUsuario, lstCargascheck, oParam.preciolitro, ref lstUnidades, out FolioPoliza);
                    if (oResponse.estatus == "OK")
                    {
                        oResponse.estatus = "ERROR";
                        oConnectionTMS = new LOGI_TMSConnection_D365(oCred, oTool.CONST_CONNECTION, "", oTool.CONST_CONNLOG, "", null);
                        if (oConnectionTMS.onCreateLogin())
                        {
                            var lstEstacion = oParam.estacion.Split('-');

                            if (oConnectionTMS.GrabaConsumodeconbustible(lstUnidades, oParam.FechaDocto, FolioPoliza,  lstEstacion[0], lstEstacion[2], lstEstacion[3], out sFolioD365, oUser.sUsuario, out sresponse))
                            {
                                oResponse.estatus = "OK";
                                oResponse.mensaje = string.Format(@"Se creó la poliza con folio ""<b>{0}</b>"" en D365 el código de registro interno asignado es el ""{1}"". El total ""<b>{2}"" de carga(s)</b> incluídas en la poliza ya no podrán ser utilizados para la sincronización.", FolioPoliza, sFolioD365, lstCargascheck.Count);
                            }
                            else oResponse.mensaje = "No se ha podido grabar el documento " + sresponse;

                        }
                        else oResponse.mensaje = string.Format("No se ha podido establecer la conexión con D365. ERROR {0}", oConnectionTMS.sMensaje);
                    }
                    else oResponse.mensaje = "No se ha podido validar la poliza para enviar a D365 " + oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listaalmacenescombus()
        {
            LOGI_Liquidacion_PD oLiquidacion = null;
            List<LOGI_Combustibles_INFO> lstConceptos = new List<LOGI_Combustibles_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            string sresponse = string.Empty, sFolioD365 = string.Empty, FolioPoliza = string.Empty;

            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oLiquidacion = new LOGI_Liquidacion_PD(oTool.CONST_CONNLOG, oTool.CONST_CONNECTION);
                    oResponse.estatus = oLiquidacion.ListadodeAlmacenes(oUser.sUsuario, ref lstConceptos);
                    if (oResponse.estatus == "OK")
                    {
                        oResponse.data = lstConceptos;
                    }
                    else oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        bool GetConfig(string CONST_USUARIO, ref LOGI_Credencial_D365 oCred, ref LOGI_ConfiguracionD365_INFO oConfiguracion, string CONST_CONNECTION)
        {
            bool bContinuar = false;
            oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_ConfiguracionD365_PD oCnfcontrol = new LOGI_ConfiguracionD365_PD(CONST_CONNECTION);
            if (oCnfcontrol.ListaConfiguracion(CONST_USUARIO, ref oConfiguracion) == "OK")
            {
                oCred.api_login = oConfiguracion.URLApilogin;
                oCred.api = oConfiguracion.URLApi;
                oCred.resource = oConfiguracion.URLApi;
                oCred.username = oConfiguracion.usuariod365;
                oCred.password = oConfiguracion.passusrd365;
                oCred.client_id = oConfiguracion.clientID;
                oCred.ciad365 = oConfiguracion.ciad365;
                oCred.aprobador = oConfiguracion.aprobador;
                oCred.cuentaviaticos = oConfiguracion.cuentaviatico;
                bContinuar = true;
            }
            return bContinuar;
        }
        #endregion "WEBMETHODS"
    }
}