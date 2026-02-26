using D365API.Helpers;
using D365API.Helpers.D365FOBBICGCxPServices;
using D365API.Helpers.D365FOBBIContaServices;
using D365API.Helpers.D365FOBBICxCServices;
using D365API.INFOD;
using INFO.Enums;
using INFO.Objetos.D365;
using INFO.Objetos.OPE;
using INFO.Objetos.SAT;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using INFO.Tablas.LOG;
using INFO.Tablas.OPE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PD.Herramientas;
using PD.Objetos.OPE;
using PD.Tablas.D365;
using PD.Tablas.EQUIV;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

namespace D365API
{
    public class LOGI_TMSConnection_D365
    {
        public string sMensaje { get; set; }
        public bool bConectado { get; set; }
        public LOGI_Session_D365 oSession = null;
        public LOGI_Error_D365 oError { get; set; }
        internal string CONST_OPE_CONNECTION = string.Empty; ///Aloja la cadena de conexion para el sistema (ADMLOG04)
        internal string CONST_EQUIV_CONNECTION = string.Empty; ///Aloja la cadena de conexión para los catálogos de equivalencia        
        internal string CONST_LOG_CONNECTION = string.Empty; ///Aloja la cadena de conexión para los catálogos de equivalencia        
        internal string CONST_PATH_JSONS = string.Empty; ///Aloja la cadena de conexión para los catálogos de equivalencia        
        internal int intentos { get; set; }
        internal LOGI_Credencial_D365 oCreds = null;
        internal RestClient oCliente = null;
        internal RestRequest oRequest = null;
        internal IRestResponse oResponse = null;
       // internal RestResponseMock oResponseAPI = null;
        internal const string CONST_USUARIO = "620"; //PERFIL DE USUARIO QUE REPRESENTA USUARIO WINDOWS
        internal const int CONST_EMPRESA = 67; //REPRESENTA EL CODIGO DE LA EMPRESA CON LA CUAL SE ESTÁ TRABAJANDO (LOGISTICA DEL MAYAB)
        internal LOGI_Tools_PD oTools = null;
        internal RichTextBox rchConsole = null;
        internal List<LOGI_Catalogos_INFO> lstProveedores = new List<LOGI_Catalogos_INFO>();
        internal List<LOGI_Catalogos_INFO> lstClientes = new List<LOGI_Catalogos_INFO>();
        internal LOGI_Documentos_PD oDocontrol = null;
        internal String sCadenamensaje = string.Empty;
        internal LOGI_ConfiguracionD365_INFO oConfig;
        internal LOGI_EventosBitacora_PD oEventosBitacora = null;
        internal static readonly string sPathFechaSinc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sincriniza.json");
        public GMTApi oObjAPI;
        GestorPolizas gestor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oCredential"></param>
        public LOGI_TMSConnection_D365(LOGI_Credencial_D365 oCredential, string CONNECTION_OPE, string CONNECTION_EQUIV, string CONNECTION_LOG, string CONST_PATH_JSONS, LOGI_ConfiguracionD365_INFO oConfiguracion, RichTextBox rchConsole = null)
        {
            bConectado = false;
            this.oCreds = oCredential;
            CONST_OPE_CONNECTION = CONNECTION_OPE;
            CONST_EQUIV_CONNECTION = CONNECTION_EQUIV;
            CONST_LOG_CONNECTION = CONNECTION_LOG;
            this.CONST_PATH_JSONS = CONST_PATH_JSONS;
            oTools = new LOGI_Tools_PD();
            this.rchConsole = rchConsole;
            oEventosBitacora = new LOGI_EventosBitacora_PD(CONST_LOG_CONNECTION, CONST_OPE_CONNECTION);
            new LOGI_Proveedores_PD(CONNECTION_EQUIV).ListaProveedores("", new LOGI_Catalogos_INFO { iActivo = 1, iEmpresa = 67 }, ref lstProveedores);
            new LOGI_Clientes_PD(CONNECTION_EQUIV).ListaClientes("", new LOGI_Catalogos_INFO { iActivo = 1, iEmpresa = 67 }, ref lstClientes);
            oDocontrol = new LOGI_Documentos_PD(CONST_OPE_CONNECTION);
            this.oConfig = oConfiguracion;
            oObjAPI = new GMTApi(oConfig, CONNECTION_LOG);
        }


        /// <summary>
        /// Descripción: Petición post realizada hacia servicio de Dynamics 365 para recibir el access token para 
        /// poder interacturar con la creación de diario contable. La información de credenciales se encuentra parametrizable por 
        /// base de datos.
        /// Autor: 
        /// Fecha: 28/04/2022
        /// </summary>
        /// <returns></returns>
        public bool onCreateLogin()
        {
            bool bContinuar = false;
            this.sMensaje = string.Empty;
            try
            {
                if (this.bConectado)
                    return this.bConectado;

                oCliente = new RestClient(oCreds.api_login);
                oRequest = new RestRequest(Method.POST);
                oRequest.AddHeader("Content-Type", "multipart/form-data");
                oRequest.AddParameter("grant_type", "password", ParameterType.GetOrPost);
                oRequest.AddParameter("client_id", oCreds.client_id, ParameterType.GetOrPost);
                oRequest.AddParameter("username", oCreds.username, ParameterType.GetOrPost);
                oRequest.AddParameter("password", oCreds.password, ParameterType.GetOrPost);
                oRequest.AddParameter("resource", oCreds.resource, ParameterType.GetOrPost);

                ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
                oResponse = oCliente.Execute(oRequest);
                this.sMensaje = oResponse.Content;
                if (oResponse.StatusCode == HttpStatusCode.OK)
                {
                    oSession = JsonConvert.DeserializeObject<LOGI_Session_D365>(this.sMensaje);
                    this.bConectado = true;
                    bContinuar = true;
                    intentos = 0;
                }
                else
                {
                    try
                    {
                        oError = JsonConvert.DeserializeObject<LOGI_Error_D365>(this.sMensaje);
                        this.sMensaje = string.Format("{0} - {1}", oError.error, oError.error_description);
                        Console.WriteLine("Error en: " + oCreds.api_login + " " + sMensaje);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                if (this.sMensaje.Contains("Internal Server"))
                    intentos++;
                if (rchConsole != null)
                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);

                Console.WriteLine("Error en: " +sMensaje);
            }
            return bContinuar;
        }
        #region NUEVO
        //BITACORA QUE SE UTILIZA PARA EL RESPONSE DE LA API GMS
        protected void ActualizarLogPolizas(string idPoliza, string status, string msj, string TipoPoliza, string Folio, string NumPoliza, DateTime FechaContable)
        {
            gestor.AgregarRegistro(idPoliza, status, DateTime.Now, msj + " " + TipoPoliza, Folio, NumPoliza, FechaContable);
        }
        protected bool Proceso(LOGI_ConfiguracionD365_INFO oConf,DateTime date, string cnx, string Proceso, int ProFacturas = 1, int ProCancelacionFacturas = 1, int ProNotasCredito = 1, int ProMovimientosBancarios = 1, int ProPasivos = 0)
        {
            bool statusProcesamiento = false;
            List<BBICargarFacturaCxCContractEnc> lstLotes = null;
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();


            gestor = new GestorPolizas();
            string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty;

            oObjAPI.responsePolizas(date);
            oResponse = oObjAPI.getPolizas(date, Proceso, "1");
            this.sMensaje = oResponse.Content;
            if (oResponse.StatusCode == HttpStatusCode.OK && !oResponse.Content.Contains("faultstring"))
            {
                this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);
                LOGI_ResponseTMS_INFO oResponse = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                if (oResponse.Result == null)
                {
                    sCadenamensaje = "";// oResponse.Result[0].Mensaje;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                    else
                        oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                    statusProcesamiento = false;
                }
                else
                {
                    int TotalPaginas = oResponse.TotalPaginas;
                    int PaginaActual = oResponse.PaginaActual;

                    LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
                    oDocontrol.ListaDocumentos(CONST_USUARIO, ref lstDocumentos, oParamdoc);
                    //     oObjAPI.Bitacora("Total de Páginas de: " + Proceso, "Se han encontrado " + TotalPaginas + " Páginas. Se empieza el proceso de migración. Usuario:" + CONST_USUARIO);
                    sCadenamensaje = string.Format("Se han encontrado un total de {0} movimientos para el período seleccionado", oResponse.Result[0].Encabezado.Count);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                    else
                        oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    for (int i = PaginaActual; i <= TotalPaginas; i++)
                    {
                        string Content = string.Empty;
                        LOGI_ResponseTMS_INFO oPolizas = null;
                        if (i == PaginaActual)
                        {
                            Content = this.sMensaje;
                            oPolizas = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(Content,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                        }
                        else
                        {
                            GMTApi gmt = new GMTApi(oConf, cnx);
                            oObjAPI.responsePolizas(date);
                            Content = gmt.getPolizas(date, Proceso, i.ToString()).Content;
                            Content = Content.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);
                            oPolizas = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(Content,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                        }

                        GMTApi gmtAp = new GMTApi(oConf, cnx);
                        gmtAp.BitacoraAPI(oPolizas.Result[0].Encabezado.ToString(), "");
                        if (ProMovimientosBancarios == 1 && Proceso == "Anticipos")
                        {
                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oPolizas.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProMovimientosBancarios" && !string.IsNullOrEmpty(x.IdRegistro)).ToList();

                            
                            if (lstLotes.Count > 0)
                            {
                                oObjAPI.BitacoraAPI("Se encontraron " + lstLotes.Count + " Anticipos para procesar", "Se inicia el procesamiento.");

                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para reposición de fondo fijo", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                                this.CreaLoteReposicionFondo(lstLotes, out sresponse);
                            }
                            //estadoProcesos.ProMovimientosBancarios = true;
                            statusProcesamiento = true;

                        }

                        if (ProFacturas == 1 && Proceso == "Facturacion")
                        {
                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oPolizas.Result[0].Encabezado.Where(x => x.ProcesoLigado.Equals("ProFacturas", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30 && string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();

                             if (lstLotes.Count > 0)
                            {oObjAPI.BitacoraAPI("Se encontraron " + lstLotes.Count + " Facturas para procesar", "Se inicia el procesamiento.");
                           
                                LOGI_Documentos_INFO oDocumento = null;
                                oDocumento = new LOGI_Documentos_INFO();
                                oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 3);
                                if (oDocumento == null)
                                    throw new Exception("No se ha encontrado la configuración de documentos de tipo ingreso para enviar a D365");

                                sCadenamensaje = string.Format(@"Se encontraron un total de {0} procesos para polizas contables de ingreso", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                                this.CreaLoteIngresosD365(lstLotes, oDocumento.diario, oDocumento.metodo, out sresponse, bCreaArchivoJSON: false);//bCreaArchivoJSON
                            }

                            // estadoProcesos.ProFacturas = true;
                            statusProcesamiento = true;
                        }

                        if (ProPasivos == 1 && Proceso == "Pasivos")
                        {

                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oPolizas.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProPasivos" && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30).ToList();
                                                         
                            if (lstLotes.Count > 0)
                            {
                                oObjAPI.BitacoraAPI("Se encontraron " + lstLotes.Count + " Pasivos para procesar en la pagina"+i.ToString()+ " con fecha:"+date.ToString(), "Se inicia el procesamiento. ");
                           
                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para polizas contables de tipo pasivos", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                                this.CreaLotePagosD365(lstLotes, out sresponse);
                                oObjAPI.BitacoraAPI("Se termina el procesamiento de Pasivos", "");

                            }

                            //estadoProcesos.ProPasivos = true;
                            statusProcesamiento = true;
                        }

                        if (ProCancelacionFacturas == 1 && Proceso == "Facturacion")
                        {
                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oPolizas.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProFacturas" && !string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();

                            oObjAPI.BitacoraAPI("Se encontraron " + lstLotes.Count + " Facturas Canceladas para procesar", "Se inicia el procesamiento.");
                            
                            if (lstLotes.Count > 0)
                            {
                                LOGI_Documentos_INFO oDocumento = null;
                                oDocumento = new LOGI_Documentos_INFO();
                                oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 8);
                                if (oDocumento == null)
                                    throw new Exception("No se ha encontrado la configuración de documentos de tipo cancelación de ingreso para enviar a D365");


                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para cancelación de facturas", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                                this.CreaLoteCancelacionIngresosD365(lstLotes, out sresponse);
                            }

                            //estadoProcesos.ProCancelacionFacturas = true;
                            statusProcesamiento = true;
                        }
                        //PENDIENTE DEFINIR
                        if (ProNotasCredito == 1 && Proceso == "Notas de credito")
                        {
                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oPolizas.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProNotasCredito" && !string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();

                            oObjAPI.BitacoraAPI("Se encontraron " + lstLotes.Count + " Notas de Crédito para procesar", "Se inicia el procesamiento.");
                            
                            if (lstLotes.Count > 0)
                            {
                                LOGI_Documentos_INFO oDocumento = null;
                                oDocumento = new LOGI_Documentos_INFO();
                                oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 5);
                                if (oDocumento == null)
                                    throw new Exception("No se ha encontrado la configuración de documentos de tipo notas de credito para enviar a D365");


                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para notas de créditos", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                                this.CreaLoteNotasCreditoIngresosD365(lstLotes, out sresponse);
                            }

                            //estadoProcesos.ProNotasCredito = true;
                            statusProcesamiento = true;

                        }

                        oObjAPI.responsePolizas(date);
                    }
                }

            }
            else
            {
                statusProcesamiento = false;

                sCadenamensaje +=". No se ha podido leer la información proveniente de TMS. "+ oResponse.Content;
                if (rchConsole != null)
                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                else
                    oTools.LogProceso(sCadenamensaje, "ProcesoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
            }
            return statusProcesamiento;
        }
        
        public bool DescargaEjecutaProcesos(string tipoProceso, string cnxBitacora, string sFechaInicio, string sFechaFinal, bool bLeeTXT = false, bool bCreaArchivoJSON = true, int ProFacturas = 1, int ProCancelacionFacturas = 1, int ProNotasCredito = 1, int ProMovimientosBancarios = 1, int ProPasivos = 0)
        {
            //// cnxBitacora = "Data Source=(local);Initial Catalog=OTMLog;Persist Security Info=True;User ID=sa;Password=12345678;Connection Timeout=120";
            //LOGI_EstadoControlProcesos_INFO estadoProcesos = new LOGI_EstadoControlProcesos_INFO
            //{
            //    ProFacturas = ProFacturas == 0,
            //    ProCancelacionFacturas = ProCancelacionFacturas == 0,
            //    ProNotasCredito = ProNotasCredito == 0,
            //    ProMovimientosBancarios = ProMovimientosBancarios == 0,
            //    ProPasivos = ProPasivos == 0
            //};

            string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
                sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
            LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
            String sJSONPATHFile = string.Empty;
            bool procesado = false;
            DateTime startDate = Convert.ToDateTime(sFechaInicio);
            DateTime endDate = Convert.ToDateTime(sFechaFinal);
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                try
                {
                    if (!bLeeTXT)
                    {
                        sCadenamensaje = string.Format("Procesando lote de movimientos en el período {0} hasta {1}", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "DescargaEjecutaProcesos", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                        GMTApi oObjAPI = new GMTApi(oConfig, cnxBitacora);
                        Proceso(oConfig,date, cnxBitacora, tipoProceso, ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                        procesado = true;

                        //estadoProcesos.ProFacturas = Proceso(date, cnxBitacora, "Facturacion", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                        //estadoProcesos.ProPasivos = Proceso(date, cnxBitacora, "Pasivos", ProPasivos, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                        //estadoProcesos.ProMovimientosBancarios = Proceso(date, cnxBitacora, "Anticipos", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                        ////PENDIENTE VALIDAR
                        //estadoProcesos.ProCancelacionFacturas = Proceso(date, cnxBitacora, "Facturacion", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);

                        //   estadoProcesos.ProNotasCredito = Proceso(date, cnxBitacora, "Notas de credito", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                        

                        //oTools.LogProceso(oResponse.Content, "DescargaEjecutaProcesos", "LOGI_TMSConnection_D365.cs", "DAPI365", CONST_USUARIO);
                        //this.sMensaje = oResponse.Content;
                    }
                    else
                    {
                        procesado = false;
                        oResponse.StatusCode = HttpStatusCode.OK;
                        sJSONPATHFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt"); // relative path
                        this.sMensaje = sJSONPATHFile;
                    }
                }
                catch (Exception ex)
                {
                    procesado = false;
                    sERROR = string.Format(@"El proceso de replica no se ha realizado. ERROR {0}", ex.Message);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                    else
                        oTools.LogError(ex, "DescargaEjecutaProcesos", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                }
                finally
                {
                    GC.Collect();
                    if (bLeeTXT)
                    {
                        try
                        {
                            string sPathJSONtmp = string.Format(@"{0}\JSON\Procesados\{1}_JSON.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("ddMMyyyyhhssmm"));
                            File.Move(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt", sPathJSONtmp);
                        }
                        catch { }
                    }
                }
            }

            return procesado;
        }

        #endregion NUEVO

        public LOGI_EstadoControlProcesos_INFO DescargaEjecutaMovimientosIngreso(LOGI_ConfiguracionD365_INFO oConf,string cnxBitacora, string sFechaInicio, string sFechaFinal, bool bLeeTXT = false, bool bCreaArchivoJSON = true, int ProFacturas = 1, int ProCancelacionFacturas = 1, int ProNotasCredito = 1, int ProMovimientosBancarios = 1, int ProPasivos = 0)
        {
            LOGI_EstadoControlProcesos_INFO estadoProcesos = new LOGI_EstadoControlProcesos_INFO
            {
                ProFacturas = ProFacturas == 0,
                ProCancelacionFacturas = ProCancelacionFacturas == 0,
                ProNotasCredito = ProNotasCredito == 0,
                ProMovimientosBancarios = ProMovimientosBancarios == 0,
                ProPasivos = ProPasivos == 0
            };

            string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
                sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
            LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
             String sJSONPATHFile = string.Empty;

            DateTime startDate = Convert.ToDateTime(sFechaInicio);
            DateTime endDate = Convert.ToDateTime(sFechaFinal);
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                try
                {
                    if (!bLeeTXT)
                    {
                        sCadenamensaje = string.Format("Procesando lote de movimientos en el período {0} hasta {1}", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "DescargaEjecutaMovimientosIngreso", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                        GMTApi oObjAPI = new GMTApi(oConf, cnxBitacora);
                        oObjAPI.responsePolizas(date);
                        //    Proceso(date, cnxBitacora, tipoProceso, ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                        if (Convert.ToBoolean(ProMovimientosBancarios))
                        {
                            try
                            {
                                estadoProcesos.ProMovimientosBancarios = Proceso(oConf,date, cnxBitacora, "Anticipos", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                            }
                            catch (Exception ex)
                            {
                                sERROR = string.Format(@"El proceso de replica de Anticipos no se ha realizado. ERROR {0}", ex.Message);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                                else
                                    oTools.LogError(ex, "DescargaEjecutaMovimientosIngreso", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            }
                        }
                        if (Convert.ToBoolean(ProFacturas))
                        {
                            try
                            {
                                estadoProcesos.ProFacturas = Proceso( oConf,date, cnxBitacora, "Facturacion", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                            }
                            catch (Exception ex)
                            {
                                sERROR = string.Format(@"El proceso de replica de Facturacion no se ha realizado. ERROR {0}", ex.Message);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                                else
                                    oTools.LogError(ex, "DescargaEjecutaMovimientosIngreso", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            }
                        }
                        if (Convert.ToBoolean(ProPasivos))
                        {
                            try
                            {
                                estadoProcesos.ProPasivos = Proceso(oConf,date, cnxBitacora, "Pasivos", ProPasivos, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                            }
                            catch (Exception ex)
                            {
                                sERROR = string.Format(@"El proceso de replica de Pasivos no se ha realizado. ERROR {0}", ex.Message);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                                else
                                    oTools.LogError(ex, "DescargaEjecutaMovimientosIngreso", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            }
                        }
                        if (Convert.ToBoolean(ProNotasCredito))
                        {
                            try
                            {
                       
                                estadoProcesos.ProNotasCredito = Proceso(oConf,date, cnxBitacora, "Notas de credito", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);
                            }
                            catch (Exception ex)
                            {
                                sERROR = string.Format(@"El proceso de replica de Notas de credito no se ha realizado. ERROR {0}", ex.Message);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                                else
                                    oTools.LogError(ex, "DescargaEjecutaMovimientosIngreso", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            }
                        }
                        if (Convert.ToBoolean(ProCancelacionFacturas))
                        {
                            try
                            {
                                estadoProcesos.ProCancelacionFacturas = Proceso(oConf,date, cnxBitacora, "Facturacion", ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProMovimientosBancarios, ProPasivos);

                            }
                            catch (Exception ex)
                            {
                                sERROR = string.Format(@"El proceso de replica de Facturas Canceladas no se ha realizado. ERROR {0}", ex.Message);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                                else
                                    oTools.LogError(ex, "DescargaEjecutaMovimientosIngreso", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            }
                        }

                        oTools.LogProceso(oResponse.Content, "DescargaEjecutaMovimientosIngreso", "LOGI_TMSConnection_D365.cs", "DAPI365", CONST_USUARIO);
                        //this.sMensaje = oResponse.Content;
                    }
                    else
                    {
                        oResponse.StatusCode = HttpStatusCode.OK;
                        sJSONPATHFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt"); // relative path
                        this.sMensaje = sJSONPATHFile;
                    }
                }
                catch (Exception ex)
                {
                    sERROR = string.Format(@"El proceso de replica no se ha realizado. ERROR {0}", ex.Message);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                    else
                        oTools.LogError(ex, "DescargaEjecutaMovimientosIngreso", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                }
                finally
                {
                    GC.Collect();
                    if (bLeeTXT)
                    {
                        try
                        {
                            string sPathJSONtmp = string.Format(@"{0}\JSON\Procesados\{1}_JSON.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("ddMMyyyyhhssmm"));
                            File.Move(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt", sPathJSONtmp);
                        }
                        catch { }
                    }
                }
            }

            return estadoProcesos;
        }


        public void DescargaEjecutaMovimientosPasivos(string cnxBitacora, string sFechaInicio, string sFechaFinal, bool bLeeTXT = false, bool bIgnoraFechas = false)
        {
            string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
                sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
            List<BBICargarFacturaCxCContractEnc> lstLotes = null;
            LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
            String sJSONPATHFile = string.Empty;
            DateTime startDate = Convert.ToDateTime(sFechaInicio);
            DateTime endDate = Convert.ToDateTime(sFechaFinal);
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                try
                {
                    if (!bIgnoraFechas)
                    {
                        if (!CondicionaEjecucion(sFechaInicio))
                        {
                            sCadenamensaje = string.Format("La fecha a ejecutar {0} hasta {1}, se encuentra fuera del alcance de sincronización", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa: "PASIVOS");
                            continue;
                        }
                    }

                    //oEventosBitacora.CreaprocesoNotificacionERRORES(CONST_USUARIO);
                    if (!bLeeTXT)
                    {
                        sCadenamensaje = string.Format("Procesando lote de movimientos en el período {0} hasta {1}", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa:"PASIVOS");

                        //Se recupera la información de los documentos que se pueden interfazar hacia Dynamics D365
                        //SE ACTUALIZA EL ESTATUS DE LAS POLIZAS QUE HAN SIDO GUARDADAS CORRECTAMENTE
                        oObjAPI.responsePolizas(date);
                        oResponse = oObjAPI.getPolizasPasivos(date, "Pasivos", "1");
                        this.sMensaje = oResponse.Content;
                       
                    }
                    else
                    {
                        oResponse.StatusCode = HttpStatusCode.OK;
                        sJSONPATHFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt"); // relative path
                        this.sMensaje = sJSONPATHFile;
                    }
                    if (oResponse.StatusCode == HttpStatusCode.OK && !oResponse.Content.Contains("faultstring"))
                    { 
                        this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);

                        LOGI_ResponseTMS_INFO oPolizas = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje,
                         new JsonSerializerSettings
                         {
                             NullValueHandling = NullValueHandling.Ignore
                         });

                        if (oPolizas.Result == null)///if (oResponse.Result[0].Encabezado == null)
                        {
                            sCadenamensaje = oPolizas.Result[0].Mensaje;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                        else
                        {
                            int TotalPaginas = oPolizas.TotalPaginas;
                            int PaginaActual = oPolizas.PaginaActual;
                            for (int i = PaginaActual; i <= TotalPaginas; i++)
                            {
                                string Content = string.Empty;                               
                                if (i == PaginaActual)
                                {
                                    oPolizas = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje,new JsonSerializerSettings{NullValueHandling = NullValueHandling.Ignore});
                                }
                                else
                                {
                                    oObjAPI.responsePolizas(date);
                                    oResponse = oObjAPI.getPolizas(date, "Pasivos", i.ToString());
                                    this.sMensaje = oResponse.Content;
                                    this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);

                                    if (this.oResponse.StatusCode == HttpStatusCode.OK && !oResponse.Content.Contains("faultstring"))
                                    {
                                        oPolizas = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                                    }
                                }
                                oDocontrol.ListaDocumentos(CONST_USUARIO, ref lstDocumentos, oParamdoc);
                                sCadenamensaje = string.Format("Se han encontrado un total de {0} movimientos para el período seleccionado", oPolizas.Result[0].Encabezado.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa: "PASIVOS");


                                lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                                lstLotes = oPolizas.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProPasivos" && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30).ToList();
                                if (lstLotes.Count > 0)
                                {
                                    sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para polizas contables de tipo pasivos", lstLotes.Count);
                                    if (rchConsole != null)
                                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                    else
                                        oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                                    this.CreaLotePagosD365(lstLotes, out sresponse);
                                }

                                oObjAPI.responsePolizas(date);
                            }

                        }
                    }
                    else
                    {
                        sCadenamensaje = "No se ha podido leer la información proveniente de TMS";
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                        else
                            oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                    }
                }
                catch (Exception ex)
                {
                    sERROR = string.Format(@"El proceso de replica no se ha realizado. ERROR {0}", ex.Message);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                    else
                        oTools.LogError(ex, "DescargaMovimientoTMS", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                }
                finally
                {
                    GC.Collect();
                    if (bLeeTXT)
                    {
                        try
                        {
                            string sPathJSONtmp = string.Format(@"{0}\JSON\Procesados\{1}_JSON.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("ddMMyyyyhhssmm"));
                            File.Move(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt", sPathJSONtmp);
                        }
                        catch { }
                    }
                }
            }
        }
 
        bool CondicionaEjecucion(string sFechaInicio)

        {
            DateTime fechaActual = DateTime.Now;
            DateTime oBloque = Convert.ToDateTime(sFechaInicio);

            if (oBloque.Day <= fechaActual.Day)
                return true;
            else if (fechaActual.Day > oBloque.Day)
                return true;
            else
                return false;
        }

        public void DescargaMovimientoTMS(string sFechaInicio, string sFechaFinal, bool bLeeTXT = false)
        {
            //bool bCreado = false;
            //int PROCESADOS = 0, APLICADOS = 0, FALLIDOS = 0;
            string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
                sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
            List<BBICargarFacturaCxCContractEnc> lstLotes = null;
            LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
            LOGI_Documentos_INFO oDocumento = null;
            String sJSONPATHFile = string.Empty;

            DateTime startDate = Convert.ToDateTime(sFechaInicio);
            DateTime endDate = Convert.ToDateTime(sFechaFinal);
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                try
                {
                    //oEventosBitacora.CreaprocesoNotificacionERRORES(CONST_USUARIO);
                    if (!bLeeTXT)
                    {
                        sCadenamensaje = string.Format("Procesando lote de movimientos en el período {0} hasta {1}", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                        //Se recupera la información de los documentos que se pueden interfazar hacia Dynamics D365
                        oCliente = new RestClient(oConfig.url_tms);
                        oCliente.Timeout = 900000000;
                        oRequest = new RestRequest(Method.POST);
                        oRequest.AddHeader("Content-Type", "application/json");
                        oRequest.AddParameter("OutputFormat", "JSON", ParameterType.GetOrPost);
                        oRequest.AddParameter("RFCEmpresa", oConfig.rfc_tms, ParameterType.GetOrPost);
                        oRequest.AddParameter("ApiKey", oConfig.api_tms, ParameterType.GetOrPost);
                        oRequest.AddParameter("Parametros", string.Format(@"{{ ""Clase"":""ClsProPolizas"", ""Metodo"":""GetPolizasDetalle"", ""Parametros"":{{ ""dFechaInicial"":""{0}"", ""dFechaFinal"":""{1}""  }}}}", date.ToString("yyyyMMdd"), date.ToString("yyyyMMdd")), ParameterType.GetOrPost);
                        oRequest.AddParameter("resource", oCreds.resource, ParameterType.GetOrPost);
                        ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
                        oResponse = oCliente.Execute(oRequest);
                        this.sMensaje = oResponse.Content;
                    }
                    else
                    {
                        oResponse.StatusCode = HttpStatusCode.OK;
                        sJSONPATHFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt"); // relative path
                        this.sMensaje = sJSONPATHFile;
                    }
                    if (oResponse.StatusCode == HttpStatusCode.OK)
                    {
                        this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);
                        this.sMensaje = this.sMensaje.Replace("OSKSN", "1");

                        LOGI_ResponseTMS_INFO oResponse = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje,
                         new JsonSerializerSettings
                         {
                             NullValueHandling = NullValueHandling.Ignore
                         });
                        if (oResponse.Result[0].Encabezado == null)
                        {
                            sCadenamensaje = oResponse.Result[0].Mensaje;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                        else
                        {

                            oDocontrol.ListaDocumentos(CONST_USUARIO, ref lstDocumentos, oParamdoc);
                            sCadenamensaje = string.Format("Se han encontrado un total de {0} movimientos para el período seleccionado", oResponse.Result[0].Encabezado.Count);
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            var lstProcesos = oResponse.Result[0].Encabezado.GroupBy(x => new { x.ProcesoLigado }).ToList();
                            sCadenamensaje = string.Format("Se han encontrado un total de {0} grupos de procesos provenientes de TMS", lstProcesos.Count);
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);


                            sCadenamensaje = string.Format(@"Validando grupo de movimientos de polizas contables de ingresos");
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();


                            sCadenamensaje = string.Format(@"Validando grupo de movimientos bancarios, reposición de fondo fijo");
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProMovimientosBancarios" && !string.IsNullOrEmpty(x.IdRegistro)).ToList();
                            if (lstLotes.Count > 0)
                            {
                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para reposición de fondo fijo", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                                this.CreaLoteReposicionFondo(lstLotes, out sresponse);
                            }



                            
                            lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado.Equals("ProFacturas", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30 && string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();
                            if (lstLotes.Count > 0)
                            {
                                oTools.m_ConsoleLine(rchConsole, "dentro del grupo", eType.proceso);

                                oDocumento = new LOGI_Documentos_INFO();
                                oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 3);
                                if (oDocumento == null)
                                    throw new Exception("No se ha encontrado la configuración de documentos de tipo ingreso para enviar a D365");

                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para polizas contables de ingreso", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                                this.CreaLoteIngresosD365(lstLotes, oDocumento.diario, oDocumento.metodo, out sresponse, false);
                            }

                           
                            sCadenamensaje = string.Format(@"Validando grupo de cancelación de facturas");
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProFacturas" && !string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();
                            if (lstLotes.Count > 0)
                            {
                                oDocumento = new LOGI_Documentos_INFO();
                                oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 8);
                                if (oDocumento == null)
                                    throw new Exception("No se ha encontrado la configuración de documentos de tipo cancelación de ingreso para enviar a D365");


                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para cancelación de facturas", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                                this.CreaLoteCancelacionIngresosD365(lstLotes, out sresponse);
                            }



                            sCadenamensaje = string.Format(@"Validando grupo de notas de crédito");
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProNotasCredito" && !string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();
                            if (lstLotes.Count > 0)
                            {
                                oDocumento = new LOGI_Documentos_INFO();
                                oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 5);
                                if (oDocumento == null)
                                    throw new Exception("No se ha encontrado la configuración de documentos de tipo notas de credito para enviar a D365");


                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para notas de créditos", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                                this.CreaLoteNotasCreditoIngresosD365(lstLotes, out sresponse);
                            }

                             

                            sCadenamensaje = string.Format(@"Validando grupo de movimientos de polizas de tipo pasivos");
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProPasivos" && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30).ToList();
                            if (lstLotes.Count > 0)
                            {
                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para polizas contables de tipo pasivos", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                                this.CreaLotePagosD365(lstLotes, out sresponse);
                            }

                            
                            sCadenamensaje = string.Format(@"Validando grupo de movimientos de liquidacion para comprobación de viaticos");
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                            lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProLiquidaciones").ToList();
                            if (lstLotes.Count > 0)
                            {
                                sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para comprobación de viaticos", lstLotes.Count);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                else
                                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                                this.CreaLoteComprobacionGastos(lstLotes, out sresponse);
                                this.CreaconsumoCombustibleLiquidacion(lstLotes, out sresponse);
                            }

                            
                           /*  sCadenamensaje = string.Format(@"Validando movimientos de MANO DE OBRA");
                             if (rchConsole != null)
                                 oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                             else
                                 oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                             lstLotes = new List<BBICargarFacturaCxCContractEnc>();
                             lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProPasivos" && x.XMLFactura == "MQ==").ToList();
                             if (lstLotes.Count > 0)
                             {
                                 sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para enviar como grupo contable", lstLotes.Count);
                                 if (rchConsole != null)
                                     oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                                 else
                                     oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                                 this.CreaEntradasContables(lstLotes, out sresponse, false);
                             }*/

                        }
                    }
                    else
                    {
                        sCadenamensaje = "No se ha podido leer la información proveniente de TMS";
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                        else
                            oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                    }
                }
                catch (Exception ex)
                {
                    sERROR = string.Format(@"El proceso de replica no se ha realizado. ERROR {0}", ex.Message);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                    else
                        oTools.LogError(ex, "DescargaMovimientoTMS", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                }
                finally
                {
                    GC.Collect();
                    if (bLeeTXT)
                    {
                        try
                        {
                            string sPathJSONtmp = string.Format(@"{0}\JSON\Procesados\{1}_JSON.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("ddMMyyyyhhssmm"));
                            File.Move(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt", sPathJSONtmp);
                        }
                        catch { }
                    }
                }
            }
        }

        void CreaLoteNotasCreditoIngresosD365(List<BBICargarFacturaCxCContractEnc> lstLote, out string sresponse)
        {
            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
            BBICargarFacturaCxCContract oContrato = new BBICargarFacturaCxCContract();
            sresponse = string.Empty;
            string sEndPoint = string.Empty, sERROR = string.Empty,
               sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty, sResultadoSQL = string.Empty;
            LOGI_Bitacora_INFO oLog = null;



            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {

                try
                {
                    if (!this.bConectado)
                        onCreateLogin();

                    oLog = new LOGI_Bitacora_INFO();
                    oContrato = new BBICargarFacturaCxCContract();
                    oContrato.Encabezado = new BBICargarFacturaCxCContractEnc();
                    oContrato.Encabezado.SistemaOrigen = D365API.Helpers.D365FOBBICxCServices.BBISistemaOrigen.OpeAdm;
                    oContrato.Encabezado.IdRegistro = oAsiento.IdRegistro;

                    oLog = new LOGI_Bitacora_INFO();
                    oLog.sesion_d365 = oSession.access_token;
                    oLog.folioD365 = oContrato.Encabezado.IdRegistro;
                    LOGI_Bitacora_INFO oBitacoraRef = new LOGI_Bitacora_INFO();
                    sResultadoSQL = new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraCxC(CONST_USUARIO, oLog, ref oBitacoraRef);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sCadenamensaje = string.Format(@"El movimiento nota de crédito con folio {0} ya cuenta con un registro de contabilidad", oLog.folioD365);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaLoteNotasCreditoIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        continue;

                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);


                    oContrato.Encabezado.NombreDiario = "684CARFACT";
                    oContrato.Encabezado.Compania = oCreds.ciad365;
                    oContrato.Encabezado.Orden = "NA";
                    oContrato.Encabezado.Referencia = "NA";

                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                    oContrato.Encabezado.FechaFactura = oAsiento.FechaFactura;
                    oContrato.Encabezado.CodCliente = oAsiento.CodCliente;
                    oContrato.Encabezado.TipoFact = D365API.Helpers.D365FOBBICxCServices.BBITipoFact.NotaCred;

                    oContrato.Encabezado.DesctoPago = "";
                    oContrato.Encabezado.Subtotal = oAsiento.Subtotal;
                    oContrato.Encabezado.Total = oAsiento.Total;
                    oContrato.Encabezado.Moneda = "MXN";
                    oContrato.Encabezado.TipoCambio = 1.0M;
                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                    oContrato.Encabezado.TipoRelacionUUID = EInvoiceCFDIReferenceType_MX.Invoice;
                    oContrato.Encabezado.SinComprobante = Helpers.D365FOBBICxCServices.NoYes.No;



                    if (!string.IsNullOrEmpty(oAsiento.XMLFactura))
                    {
                        oXML = new LOGI_XMLS_INFO();
                        if (!oTools.DevuelveXMLObject(out sresponse, ref oXML, sContentXML: oTools.Base64ToText(oAsiento.XMLFactura)))
                            throw new Exception(sresponse);

                        oContrato.Encabezado.Serie = oXML.Serie;
                        oContrato.Encabezado.XMLFactura = oXML.CFDIContent;
                        oContrato.Encabezado.Folio = Convert.ToInt32(oXML.Folio);
                        oContrato.Encabezado.Factura = string.Format("{0}{1}", oXML.Serie, oXML.Folio);
                        oContrato.Encabezado.Docto = string.Format("{0}{1}", oXML.Serie, oXML.Folio);
                        oContrato.Encabezado.Texto = string.Format("Nota de crédito {0}{1}", oXML.Serie, oXML.Folio);
                        oContrato.Encabezado.UsoCFDI = oAsiento.UsoCFDI;
                        oContrato.Encabezado.MetodoPagoSAT = oXML.MetodoPago;
                        oContrato.Encabezado.FechaDocto = Convert.ToDateTime(oXML.Fecha);
                        oContrato.Encabezado.Descripcion = oAsiento.Descripcion;
                        oContrato.Encabezado.UUID = oXML.Complemento.TimbreFiscalDigital.UUID;
                    }
                    else throw new Exception("El XML no se ha podido recuperar");

                    oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.Length > 60 ? oContrato.Encabezado.Descripcion.Substring(0, 59) : oContrato.Encabezado.Descripcion;
                    oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.ToUpper();

                    //SE REQUIERE CALCULAR LA FECHA DE VENCIMIENTO SEGÚN EL PLAZO DE CREDITO SOBRE EL CLIENTE
                    oContrato.Encabezado.FechaVencimiento = oAsiento.FechaVencimiento;


                    ///CUANDO EL DOCUMENTO CORRESPONDE A UNA NOTA DE CRÉDITO O REFACTURACIÓN SE REALIZA LA BÚSQUEDA DEL DOCUMENTO ASOCIADO (VIAJE O FACTURA VARIA) 

                    if (!string.IsNullOrEmpty(oAsiento.Referencia))
                    {
                        //SOLO SI ES DE TIPO NOTAS DE CREDITO SE CAMBIA EL GIRO DEL TIPO DE DOCUMENTO 
                        oContrato.Encabezado.TipoRelacionUUID = EInvoiceCFDIReferenceType_MX.CreditNote;
                        oLog = new LOGI_Bitacora_INFO();
                        oLog.seriefolio = oAsiento.Referencia;
                        oBitacoraRef = new LOGI_Bitacora_INFO();
                        if (new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraCxC(CONST_USUARIO, oLog, ref oBitacoraRef).Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                        {

                            oContrato.Encabezado.IdFactReferencia = Convert.ToInt64(oBitacoraRef.proceso_id_d365);
                            oContrato.Encabezado.UUIDRelacionado = oAsiento.UUIDRelacionado;
                        }
                        else throw new Exception("No se ha encontrado el documento asociado para la nota de crédito");
                    }
                    else throw new Exception("La nota de crédito no tiene un documento fiscal asociado");

                    try
                    {

                        oContrato.Encabezado.DimSucursal = oAsiento.Lineas.FirstOrDefault(x => !string.IsNullOrEmpty(x.DimSucursal)).DimSucursal;
                    }
                    catch
                    {
                        throw new Exception("No se ha encontrado la información de la unidad,valida la configuración de sucursal, centro de costo y equivalencia. Asegurate que el documento está ligado con una unidad");
                    }

                    List<BBICargarFacturaCxCContractLin> lstListaContrato = new List<BBICargarFacturaCxCContractLin>();
                    BBICargarFacturaCxCContractLin oLinea = new BBICargarFacturaCxCContractLin();
                    foreach (BBICargarFacturaCxCContractLin oPartida in oAsiento.Lineas)
                    {
                        if (oPartida.Debito > 0)
                        {

                            oLinea.Debito = oPartida.Debito;
                            oLinea.CuentaContable = oPartida.CuentaContableD365;
                            oLinea.DimCeco = oPartida.DimCeco;
                            oLinea.Vehiculo = oPartida.Vehiculo;
                            oLinea.DimSucursal = oPartida.DimSucursal;
                            oLinea.DimDepto = oPartida.DimDepto;
                            oLinea.DimArea = oPartida.DimArea;
                            oLinea.DimFilTer = oPartida.DimFilTer;
                            oLinea.Texto = oPartida.Texto;
                            oLinea.GrupoImpuestos = oPartida.GrupoImpuestos;
                            oLinea.GrupoImpuestosArt = oPartida.GrupoImpuestosArt;
                            oLinea.CodImpuesto = oPartida.CodImpuesto;

                            if (string.IsNullOrEmpty(oPartida.DimCeco) || string.IsNullOrEmpty(oPartida.Vehiculo) || string.IsNullOrEmpty(oPartida.DimSucursal) ||
                       string.IsNullOrEmpty(oPartida.DimDepto) || string.IsNullOrEmpty(oPartida.DimArea) || string.IsNullOrEmpty(oPartida.DimFilTer))
                                throw new Exception("Una o varias líneas de dimensiones financieras no se encuentran configuradas. (Centro de costos, Vehículo o Sucursal)");

                            if (string.IsNullOrEmpty(oPartida.GrupoImpuestos) || string.IsNullOrEmpty(oPartida.GrupoImpuestosArt) ||
                                string.IsNullOrEmpty(oPartida.CodImpuesto))
                                throw new Exception("Los códigos de impuestos no se han configurado. " +oContrato.Encabezado.IdPoliza);

                            lstListaContrato.Add(oLinea);
                        }
                    }
                    oContrato.Encabezado.Lineas = null;
                    oContrato.Lineas = lstListaContrato.ToArray();

                    sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, "BBICxCServices");
                    System.ServiceModel.Channels.Binding oBindig;
                    EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
                    oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                    oBindig.ReceiveTimeout = TimeSpan.MaxValue;
                    oBindig.SendTimeout = TimeSpan.MaxValue;
                    BBICargarFacturaCxCServiceClient oClienteD365 = new BBICargarFacturaCxCServiceClient(
                    binding: oBindig, endpointAddress);
                    Helpers.D365FOBBICxCServices.CallContext oContexto = new Helpers.D365FOBBICxCServices.CallContext();
                    oContexto.Company = oContrato.Encabezado.Compania;

                    sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                        Helpers.D365FOBBICxCServices.Infolog oInfoLog = oClienteD365.cargarFactura(oContexto, oContrato, out sresponse);
                        bool bCreado = this.RecuperaMensaje(ref sresponse);
                        if (!bCreado)
                        {
                            sCadenamensaje = this.sMensaje;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteNotasCreditoIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            this.CreamensajeLOG(eDocumentoTMS.NOTAS_DE_CREDITO, oXML.Receptor.Rfc, oXML.Receptor.Nombre, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaDocto, oContrato.Encabezado.Docto, oContrato.Encabezado.UUIDRelacionado, oXML.Complemento.TimbreFiscalDigital.UUID, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);

                        }
                        else
                        {
                            oLog.proceso_id_d365 = this.sMensaje.Split('|')[1].ToString();
                            oLog.seriefolio = string.Format("C{0}{1}", oAsiento.Serie, oAsiento.Folio);
                            oLog.sesion_d365 = oSession.access_token;
                            oLog.folioD365 = oContrato.Encabezado.IdRegistro;
                            new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaBitacoraCxC(CONST_USUARIO, oLog);
                            sCadenamensaje = string.Format(@"El diario contable con folio ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", oAsiento.IdRegistro, this.sMensaje);
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteNotasCreditoIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }

                }
                catch (Exception ex)
                {

                    sCadenamensaje = ex.Message;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaLoteNotasCreditoIngresosD365", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                    if (this.sMensaje.Contains("Internal Server"))
                        intentos++;
                    else
                    {
                        try
                        {
                            this.CreamensajeLOG(eDocumentoTMS.NOTAS_DE_CREDITO, oXML.Receptor.Rfc, oXML.Receptor.Nombre, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaDocto, oContrato.Encabezado.Docto, oContrato.Encabezado.UUIDRelacionado, oXML.Complemento.TimbreFiscalDigital.UUID, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);

                        }
                        catch (Exception exp)
                        {
                            sCadenamensaje = exp.Message;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                            else
                                oTools.LogError(exp, "CreaLoteNotasCreditoIngresosD365=>CATCH", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }
                }
                finally
                {
                    if (intentos >= 2)
                        bConectado = false;

                }
            }

        }
        public bool CreaEntradasContables(List<BBICargarFacturaCxCContractEnc> lstLote, out string response, bool bCombustible)

        {
            bool bContinuar = false;
            response = string.Empty;
            BBICargarPolizaContContract oContrato = new BBICargarPolizaContContract();
            List<BBICargarPolizaContContractLin> lstListaContrato = null;
            LOGI_Bitacora_INFO oLog = null;
            LOGI_ManoObra_INFO oMano = null;
            BBICargarPolizaContContractLin oLinea = null;
            string sJSON = string.Empty, sResultadoSQL = string.Empty;
            string sURLEndPoint = string.Empty;



            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {
                try
                {
                    if (!this.bConectado)
                        onCreateLogin();

                    if (string.IsNullOrEmpty(oAsiento.CodProveedor))
                        throw new Exception("No se ha encontrado información del código del mecanico");

                    oContrato = new BBICargarPolizaContContract();
                    oContrato.Encabezado = new BBICargarPolizaContContractEnc();
                    oContrato.Encabezado.SistemaOrigen = D365API.Helpers.D365FOBBIContaServices.BBISistemaOrigen.OpeAdm;
                    oContrato.Encabezado.IdRegistro = bCombustible ? string.Format("LM.GSTO{0}", oAsiento.Folio) : string.Format("LM.MOGSTO{0}", oAsiento.Folio);

                    if (bCombustible)
                        oContrato.Encabezado.Descripcion = string.Format("Combustible costo para entrada {0}", oContrato.Encabezado.IdRegistro);
                    else
                        oContrato.Encabezado.Descripcion = string.Format("Mano de obra para entrada {0}", oContrato.Encabezado.IdRegistro);
                    oContrato.Encabezado.NombreDiario = "687CTOCOM";
                    oContrato.Encabezado.Compania = oCreds.ciad365;
                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBIContaServices.NoYes.No;
                    oContrato.Encabezado.FechaTrans = oAsiento.FechaFactura.AddMonths(-1);//.AddDays(-5);
                    oContrato.Encabezado.Moneda = "MXN";
                    oContrato.Encabezado.TipoCambio = 1.0M;

                    ///Valida si existe en la bitacora LOCAL
                    oLog = new LOGI_Bitacora_INFO();
                    oLog.sesion_d365 = oSession.access_token;
                    oLog.folioD365 = oContrato.Encabezado.IdRegistro;
                    sResultadoSQL = new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ValidaManoRegistrada(oLog);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sCadenamensaje = string.Format(@"El movimiento de mano de obra para el folio {0} ya cuenta con un registro de contabilidad", oLog.folioD365);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaEntradasContables", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        continue;

                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);
                    ///Proceso comentado no se integra contabilidad de mano de obra
                    /*lstListaContrato = new List<BBICargarPolizaContContractLin>();

                    foreach (BBICargarFacturaCxCContractLin oPartida in oAsiento.Lineas)
                    {
                        if (oPartida.Debito == 0 && oPartida.Credito == 0)
                            continue;

                        oLinea = new BBICargarPolizaContContractLin();
                        oLinea.Texto = oPartida.Texto;
                        oLinea.CuentaContable = oPartida.CuentaContableD365;
                        oLinea.Debito = oPartida.Debito;
                        oLinea.Credito = oPartida.Credito;
                        oLinea.DimArea = oPartida.DimArea; //d.area_D365 == "NO EXISTE" ? string.Empty : d.area_D365;
                        oLinea.DimCeco = oPartida.DimCeco; //d.centrocosto_D365 == "NO EXISTE" ? string.Empty : d.centrocosto_D365;
                        oLinea.DimSucursal = oPartida.DimSucursal;
                        oLinea.DimDepto = oPartida.DimDepto;
                        oLinea.DimFilTer = oPartida.DimFilTer;
                        oLinea.Vehiculo = oPartida.Vehiculo;


                        if (string.IsNullOrEmpty(oPartida.DimCeco) || string.IsNullOrEmpty(oPartida.Vehiculo) || string.IsNullOrEmpty(oPartida.DimSucursal) ||
                               string.IsNullOrEmpty(oPartida.DimDepto) || string.IsNullOrEmpty(oPartida.DimArea) || string.IsNullOrEmpty(oPartida.DimFilTer))
                            throw new Exception("Una o varias líneas de dimensiones financieras no se encuentran configuradas. (Centro de costos, Vehículo o Sucursal)");

                        lstListaContrato.Add(oLinea);
                    }

                    oContrato.Lineas = lstListaContrato.ToArray();


                    sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, "BBIContaServices");
                    System.ServiceModel.Channels.Binding oBindig;
                    EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
                    oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                    BBICargarPolizaContServiceClient oClienteD365 = new BBICargarPolizaContServiceClient(
                    binding: oBindig, endpointAddress);
                    Helpers.D365FOBBIContaServices.CallContext oContexto = new Helpers.D365FOBBIContaServices.CallContext();
                    oContexto.Company = oContrato.Encabezado.Compania;
                    using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                        Helpers.D365FOBBIContaServices.Infolog oInfoLog = oClienteD365.cargarPoliza(oContexto, oContrato, out response);
                        //bContinuar = this.RecuperaMensaje(ref response);
                        bool bCreado = this.RecuperaMensaje(ref response);
                        if (!bCreado)
                        {
                            sCadenamensaje = this.sMensaje;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaEntradasContables", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            this.CreamensajeLOG(eDocumentoTMS.REGISTRO_MANO_DE_OBRA, oAsiento.CodProveedor, "", oContrato.Encabezado.IdRegistro, oAsiento.FechaDocto, "", "", "", oConfig.URLApi, oAsiento.Total, oAsiento.Subtotal, sCadenamensaje, sJSON);
                        }
                        else
                        {*/
                            oLog.proceso_id_d365 = this.sMensaje.Split('|')[1].ToString();
                            oMano = new LOGI_ManoObra_INFO();
                            oMano.cia = CONST_EMPRESA;
                            oMano.fecha = oAsiento.FechaFactura;
                            oMano.empleado = Convert.ToInt32(oAsiento.CodProveedor);
                            oMano.importe = oContrato.Lineas.Sum(x => x.Debito);
                            oMano.concepto = 159;
                            oMano.folio = oContrato.Encabezado.IdRegistro.Length > 12 ? oContrato.Encabezado.IdRegistro.Substring(0, 11) : oContrato.Encabezado.IdRegistro;
                            new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaNomina(CONST_USUARIO, oMano, oLog);
                            sCadenamensaje = string.Format(@"El diario contable con folio ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", oAsiento.IdRegistro, this.sMensaje);
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaEntradasContables", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        //}

                    //}
                }

                catch (Exception ex)
                {

                    sCadenamensaje = ex.Message;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaEntradasContables", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    response = ex.Message;
                    this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                    if (this.sMensaje.Contains("Internal Server"))
                        intentos++;
                    else
                    {
                        try
                        {
                            this.CreamensajeLOG(eDocumentoTMS.REGISTRO_MANO_DE_OBRA, oAsiento.CodProveedor, "", oContrato.Encabezado.IdRegistro, oAsiento.FechaDocto, "", "", "", oConfig.URLApi, oAsiento.Total, oAsiento.Subtotal, sCadenamensaje, sJSON);

                        }
                        catch (Exception exp)
                        {
                            sCadenamensaje = exp.Message;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                            else
                                oTools.LogError(exp, "CreaEntradasContables=>CATCH", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }
                }
                finally
                {
                    if (intentos >= 2)
                        bConectado = false;

                }
            }

            return bContinuar;
        }


        public bool GrabaConsumodeconbustible(List<LOGI_LiquidacionCombus_INFO> lstDocumentos, string sFechaDocto, string FolioAsiento, string sCuentaELA, string sCuentaAFA, string sAlmacen, out string sFolioD365, string sUsuario, out string response)

        {
            bool bContinuar = false;
            response = string.Empty;
            sFolioD365 = string.Empty;
            BBICargarPolizaContContract oContrato = new BBICargarPolizaContContract();
            List<BBICargarPolizaContContractLin> lstListaContrato = null;
            BBICargarPolizaContContractLin oLinea = null;
            string sJSON = string.Empty, sResultadoSQL = string.Empty;
            string sURLEndPoint = string.Empty;

            oContrato = new BBICargarPolizaContContract();
            oContrato.Encabezado = new BBICargarPolizaContContractEnc();
            oContrato.Encabezado.SistemaOrigen = D365API.Helpers.D365FOBBIContaServices.BBISistemaOrigen.OpeAdm;
            oContrato.Encabezado.IdRegistro = FolioAsiento;
            oContrato.Encabezado.Descripcion = string.Format("Combustible costo para entrada {0}", oContrato.Encabezado.IdRegistro);
            oContrato.Encabezado.NombreDiario = "687CTOCOM";
            oContrato.Encabezado.Compania = oCreds.ciad365;
            oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBIContaServices.NoYes.No;
            oContrato.Encabezado.FechaTrans = Convert.ToDateTime(sFechaDocto);
            oContrato.Encabezado.Moneda = "MXN";
            oContrato.Encabezado.TipoCambio = 1.0M;

            ///Valida si existe en la bitacora LOCAL
            lstListaContrato = new List<BBICargarPolizaContContractLin>();

            foreach (LOGI_LiquidacionCombus_INFO oPartida in lstDocumentos)
            {

                oLinea = new BBICargarPolizaContContractLin();
                oLinea.Texto = oPartida.comentarios;
                oLinea.CuentaContable = sCuentaELA.Trim();
                oLinea.Debito = oPartida.total;
                oLinea.DimArea = oPartida.area;
                oLinea.DimCeco = oPartida.centro;
                oLinea.DimSucursal = oPartida.sucursal;
                oLinea.DimDepto = oPartida.depto;
                oLinea.DimFilTer = oPartida.filial;
                oLinea.Vehiculo = oPartida.vehiculo;
                lstListaContrato.Add(oLinea);
            }
            var oContrapartida = lstListaContrato.FirstOrDefault(x => x.Debito > 0);
            oLinea = new BBICargarPolizaContContractLin();
            oLinea.Texto = "CONSUMO DE ALMACÉN AUTOMATICO";
            oLinea.CuentaContable = sCuentaAFA.Trim();
            oLinea.Credito = lstListaContrato.Sum(x => x.Debito);
            oLinea.DimArea = oContrapartida.DimArea;
            oLinea.DimCeco = oContrapartida.DimCeco;
            oLinea.DimSucursal = sAlmacen;
            oLinea.DimDepto = oContrapartida.DimDepto;
            oLinea.DimFilTer = oContrapartida.DimFilTer;
            oLinea.Vehiculo = "";
            lstListaContrato.Add(oLinea);

            oContrato.Lineas = lstListaContrato.ToArray();


            sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, "BBIContaServices");
            System.ServiceModel.Channels.Binding oBindig;
            EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
            oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
            BBICargarPolizaContServiceClient oClienteD365 = new BBICargarPolizaContServiceClient(
            binding: oBindig, endpointAddress);
            Helpers.D365FOBBIContaServices.CallContext oContexto = new Helpers.D365FOBBIContaServices.CallContext();
            oContexto.Company = oContrato.Encabezado.Compania;
            using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                Helpers.D365FOBBIContaServices.Infolog oInfoLog = oClienteD365.cargarPoliza(oContexto, oContrato, out response);
                //bContinuar = this.RecuperaMensaje(ref response);
                bool bCreado = this.RecuperaMensaje(ref response);
                if (!bCreado)
                {
                    sCadenamensaje = this.sMensaje;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                    else
                        oTools.LogProceso(sCadenamensaje, "GrabaConsumodeconbustible", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                }
                else
                {
                    sFolioD365 = this.sMensaje.Split('|')[1].ToString();
                    LOGI_LiquidacionBitacora_INFO oBitacora = new LOGI_LiquidacionBitacora_INFO();
                    oBitacora.folioTMS = FolioAsiento;
                    oBitacora.recID = this.sMensaje.Split('|')[1].ToString();
                    oBitacora.SessionID = oSession.access_token;
                    new LOGI_Liquidacion_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaMovimientoCombustiblePoliza(CONST_USUARIO, lstDocumentos, oBitacora, sUsuario);
                    sCadenamensaje = string.Format(@"El diario contable con folio ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", FolioAsiento, this.sMensaje);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                    else
                        oTools.LogProceso(sCadenamensaje, "GrabaConsumodeconbustible", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                    bContinuar = true;
                }

            }

            return bContinuar;
        }

        void CreaLoteReposicionFondo(List<BBICargarFacturaCxCContractEnc> lstLote, out string sresponse, string sCodigoemp = "")
        {
            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
            BBICargarFacturaCxCContract oContrato = new BBICargarFacturaCxCContract();
            sresponse = string.Empty;
            string sEndPoint = string.Empty, sERROR = string.Empty,
               sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty, Vehiculo = string.Empty, sResultadoSQL = string.Empty;
            LOGI_Bitacora_INFO oLog = null;
            LOGI_FondoFijo_INFO oFondo = null;
            string sNombreoperador= string.Empty, sRFC = string.Empty; 


            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {

                try
                {
                    sResultadoSQL = string.Empty;
                    oFondo = new LOGI_FondoFijo_INFO();
                    oLog = new LOGI_Bitacora_INFO();
                    oLog.folioD365 = oAsiento.IdRegistro;



                    String datos = oAsiento.Texto;
                    var lstDatos = datos.Split('|');

                    if (string.IsNullOrEmpty(lstDatos[0]) && string.IsNullOrEmpty(lstDatos[1]))
                        throw new Exception("Una o más líneas de información del operador no se encuentra configurado");


                    var lstOperador = lstDatos[1].ToString().Split('-');

                    if (string.IsNullOrEmpty(lstOperador[0]) && string.IsNullOrEmpty(lstOperador[1]) && string.IsNullOrEmpty(lstOperador[2]) && string.IsNullOrEmpty(lstOperador[3]))
                        throw new Exception("Una o más líneas de información del operador no se ha configurado. "+oAsiento.IdPoliza);

                    if (lstOperador[3].Trim().Length <= 2)
                        throw new Exception("El RFC del operador no se encuentra configurado. " + oAsiento.IdPoliza);
                    sRFC = lstOperador[3].Trim();

                    if (lstOperador[1].Trim().Length <= 2)
                        throw new Exception("El nombre del operador no se encuentra configurado. " + oAsiento.IdPoliza);
                    sNombreoperador = lstOperador[1].Trim();

                    if (lstOperador[0].Trim().Length <= 2)
                        throw new Exception("El código del operador no se encuentra configurado. " + oAsiento.IdPoliza);


                    if (lstOperador[2].Trim().Length <= 2)
                        throw new Exception(string.Format("La cuenta de banco del operador {0} - {1} no se encuentra configurada", sRFC, sNombreoperador));


                    try
                    {
                        Vehiculo = oAsiento.Lineas.FirstOrDefault(x => !string.IsNullOrEmpty(x.Vehiculo)).Vehiculo;
                        if (string.IsNullOrEmpty(Vehiculo))
                            throw new Exception("La unidad no está configurada. " + oAsiento.IdPoliza);
                    }
                    catch { throw new Exception("La unidad no está configurada. " + oAsiento.IdPoliza); }

                    LOGI_Bitacora_INFO oBitacoraRef = new LOGI_Bitacora_INFO();
                    sResultadoSQL = new LOGI_FondoFijo_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraGasto(CONST_USUARIO, oLog, ref oBitacoraRef);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sCadenamensaje = string.Format(@"El movimiento comprobación de viaticos {0} ya cuenta con un registro de poliza y asiento diario D365", oLog.folioD365);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaLoteReposicionFondo", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        continue;
                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);


                    oFondo.Folio = oAsiento.Serie; //string.Format("{0}{1}", oAsiento.Serie, oAsiento.Folio);
                    oFondo.OperadorId = lstOperador[0].Trim();
                    oFondo.FolioTMS = oAsiento.IdRegistro;
                    oFondo.Nombreoperador = lstOperador[1].Trim();
                    oFondo.TractorId = Vehiculo.Trim();
                    oFondo.Cuenta = lstOperador[2].Trim();
                    oFondo.Importe = oAsiento.Lineas.Sum(x => x.Debito);
                    oFondo.UsuarioId = "67-4292"; //usuario fijo para proceso de reposición
                    if (!string.IsNullOrEmpty(sCodigoemp))
                        oFondo.UsuarioId = string.Format("67-{0}", sCodigoemp);

                    sResultadoSQL = new LOGI_FondoFijo_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).GenerarPolizasReposicionFondoFijo(oFondo);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ActualizarLogPolizas(oAsiento.IdPoliza, "1", "Póliza cargada correctamente", "Anticipos- ", oAsiento.Folio.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);
                        sCadenamensaje = string.Format(@"El diario para reposición de fondo fijo con folio ""{0}"" se ha creado con éxito. Para el operador {1}", oAsiento.IdRegistro, lstOperador[1].Trim());
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaLoteReposicionFondo", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                    }
                    else
                        ActualizarLogPolizas(oAsiento.IdPoliza, "0", "La póliza no se pudo guardar"+sResultadoSQL, "Anticipos- CreaLoteReposicionFondo", oAsiento.Folio.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);


                }
                catch (Exception ex)
                {
                    ActualizarLogPolizas(oAsiento.IdPoliza, "0", ex.Message, "Anticipos-", oAsiento.Folio.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);
                    sCadenamensaje = ex.Message;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaLoteReposicionFondo", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                    if (this.sMensaje.Contains("Internal Server"))
                        intentos++;
                    else
                    {
                        try
                        {
                            sCadenamensaje = ex.Message;
                            this.CreamensajeLOG(eDocumentoTMS.DISPERSION_ANTICIPOS, sRFC, sNombreoperador, oAsiento.IdRegistro, DateTime.Now, "", "", "", oConfig.URLApi, oAsiento.Total, oAsiento.Subtotal, sCadenamensaje, sJSON);
                        }
                        catch (Exception exp)
                        {
                            sCadenamensaje = exp.Message;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                            else
                                oTools.LogError(exp, "CreaLoteReposicionFondo=>CATCH", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }
                }
                finally
                {
                    if (intentos >= 2)
                        bConectado = false;

                }
            }

        }


        void CreaLoteCancelacionIngresosD365(List<BBICargarFacturaCxCContractEnc> lstLote, out string sresponse)
        {
            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
            BBICargarFacturaCxCContract oContrato = new BBICargarFacturaCxCContract();
            sresponse = string.Empty;
            string sEndPoint = string.Empty, sERROR = string.Empty,
               sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty, sResultadoSQL = string.Empty;
            LOGI_Bitacora_INFO oLog = null;


            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {

                try
                {

                    if (!this.bConectado)
                        onCreateLogin();

                    oContrato = new BBICargarFacturaCxCContract();
                    oContrato.Encabezado = new BBICargarFacturaCxCContractEnc();
                    oContrato.Encabezado.SistemaOrigen = D365API.Helpers.D365FOBBICxCServices.BBISistemaOrigen.OpeAdm;
                    oContrato.Encabezado.IdRegistro = string.Format("LM.CAC{0}00{1}", oAsiento.Serie, oAsiento.Folio);//oEncabezado.FolioAsiento;
                    oContrato.Encabezado.NombreDiario = "684CARFACT";
                    oContrato.Encabezado.Compania = oCreds.ciad365;
                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                    oContrato.Encabezado.FechaFactura = oAsiento.FechaFactura;
                    oContrato.Encabezado.CodCliente = oAsiento.CodCliente;
                    oContrato.Encabezado.TipoFact = D365API.Helpers.D365FOBBICxCServices.BBITipoFact.Cancelacion;
                    oContrato.Encabezado.DesctoPago = "";
                    oContrato.Encabezado.Subtotal = oAsiento.Subtotal; //oEncabezado.subtotal;
                    oContrato.Encabezado.Total = oAsiento.Total; //oEncabezado.total;
                    oContrato.Encabezado.Moneda = "MXN";
                    oContrato.Encabezado.TipoCambio = 1.0M;
                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                    oContrato.Encabezado.SinComprobante = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                    oContrato.Encabezado.Orden = "NA";
                    oContrato.Encabezado.Referencia = "NA";


                    oContrato.Encabezado.TipoRelacionUUID = EInvoiceCFDIReferenceType_MX.Invoice;
                    oContrato.Encabezado.Descripcion = string.Format("CANCELACIÓN. {0}", oAsiento.Descripcion.Replace("CXC FACTURA", string.Empty).Trim());
                    oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.Length > 60 ? oContrato.Encabezado.Descripcion.Substring(0, 59) : oContrato.Encabezado.Descripcion;
                    oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.ToUpper();
                    try
                    {
                        oContrato.Encabezado.DimSucursal = oAsiento.Lineas.FirstOrDefault(x => !string.IsNullOrEmpty(x.DimSucursal)).DimSucursal;
                    }
                    catch
                    {
                        throw new Exception("No se ha encontrado la información de la unidad,valida la configuración de sucursal, centro de costo y equivalencia. Valida que la cancelación este ligado a un movimiento de factura. NumPoliza" + oContrato.Encabezado.NumeroPoliza);
                    }

                oLog = new LOGI_Bitacora_INFO();
                    oLog.sesion_d365 = oSession.access_token;
                    oLog.folioD365 = oContrato.Encabezado.IdRegistro;
                    LOGI_Bitacora_INFO oBitacoraRef = new LOGI_Bitacora_INFO();
                    sResultadoSQL = new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraCxC(CONST_USUARIO, oLog, ref oBitacoraRef);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sCadenamensaje = string.Format(@"El movimiento cancelación de factura {0} ya cuenta con un registro de contabilidad", oLog.folioD365);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaLoteCancelacionIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        continue;

                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);



                    oLog = new LOGI_Bitacora_INFO();
                    oLog.seriefolio = string.Format("{0}{1}", oAsiento.Serie, oAsiento.Folio);
                    oBitacoraRef = new LOGI_Bitacora_INFO();
                    if (new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraCxC(CONST_USUARIO, oLog, ref oBitacoraRef).Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        oContrato.Encabezado.IdFactReferencia = Convert.ToInt64(oBitacoraRef.proceso_id_d365);
                        oContrato.Encabezado.UUIDRelacionado = oAsiento.UUIDRelacionado;
                        oContrato.Encabezado.Folio = Convert.ToInt32(oAsiento.Folio);
                        oContrato.Encabezado.Serie = oAsiento.Serie;
                        string FolioSerie = string.Format("C{0}{1}", oAsiento.Serie, oAsiento.Folio);
                        oContrato.Encabezado.Factura = FolioSerie;
                        oContrato.Encabezado.Docto = FolioSerie;
                        oContrato.Encabezado.Texto = string.Format("CANCELACIÓN {0}", FolioSerie);

                    }
                    else throw new Exception("El movimiento de factura no se ha creado");




                    List<BBICargarFacturaCxCContractLin> lstListaContrato = new List<BBICargarFacturaCxCContractLin>();
                    BBICargarFacturaCxCContractLin oLinea = new BBICargarFacturaCxCContractLin();
                    foreach (BBICargarFacturaCxCContractLin oPartida in oAsiento.Lineas)
                    {
                        if (oPartida.Credito > 0)
                        {
                            if (string.IsNullOrEmpty(oPartida.DimArea))
                                oPartida.DimArea = "SE";
                            if (string.IsNullOrEmpty(oPartida.DimDepto))
                                oPartida.DimDepto = "SE05";


                            if (string.IsNullOrEmpty(oPartida.DimCeco) || string.IsNullOrEmpty(oPartida.Vehiculo) || string.IsNullOrEmpty(oPartida.DimSucursal) ||
                    string.IsNullOrEmpty(oPartida.DimDepto) || string.IsNullOrEmpty(oPartida.DimArea) || string.IsNullOrEmpty(oPartida.DimFilTer))
                                throw new Exception("Una o varias líneas de dimensiones financieras no se encuentran configuradas. (Centro de costos, Vehículo o Sucursal)");

                            if (string.IsNullOrEmpty(oPartida.GrupoImpuestos) || string.IsNullOrEmpty(oPartida.GrupoImpuestosArt) ||
                                string.IsNullOrEmpty(oPartida.CodImpuesto))
                                throw new Exception("Los códigos de impuestos no se han configurado. "+oContrato.Encabezado.IdPoliza);

                            oLinea.Debito = oPartida.Credito;
                            oLinea.CuentaContable = oPartida.CuentaContableD365;
                            oLinea.DimCeco = oPartida.DimCeco;
                            oLinea.Vehiculo = oPartida.Vehiculo;
                            oLinea.DimSucursal = oPartida.DimSucursal;
                            oLinea.DimDepto = oPartida.DimDepto;
                            oLinea.DimArea = oPartida.DimArea;
                            oLinea.DimFilTer = oPartida.DimFilTer;
                            oLinea.Texto = oPartida.Texto;
                            oLinea.GrupoImpuestos = oPartida.GrupoImpuestos;
                            oLinea.GrupoImpuestosArt = oPartida.GrupoImpuestosArt;
                            oLinea.CodImpuesto = oPartida.CodImpuesto;

                            lstListaContrato.Add(oLinea);
                        }
                    }
                    oContrato.Encabezado.Lineas = null;
                    oContrato.Lineas = lstListaContrato.ToArray();

                    sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, "BBICxCServices");
                    System.ServiceModel.Channels.Binding oBindig;
                    EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
                    oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                    oBindig.ReceiveTimeout = TimeSpan.MaxValue;
                    oBindig.SendTimeout = TimeSpan.MaxValue;
                    BBICargarFacturaCxCServiceClient oClienteD365 = new BBICargarFacturaCxCServiceClient(
                    binding: oBindig, endpointAddress);
                    Helpers.D365FOBBICxCServices.CallContext oContexto = new Helpers.D365FOBBICxCServices.CallContext();
                    oContexto.Company = oContrato.Encabezado.Compania;

                    sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                        Helpers.D365FOBBICxCServices.Infolog oInfoLog = oClienteD365.cargarFactura(oContexto, oContrato, out sresponse);
                        bool bCreado = this.RecuperaMensaje(ref sresponse);
                        if (!bCreado)
                        {
                            //POLIZAS  Para el nuevo response
                            string idPoliza = oContrato.Encabezado.IdPoliza;
                            string NumPoliza = oContrato.Encabezado.NumeroPoliza;
                            ActualizarLogPolizas(idPoliza, "0", "No se pudo cargar la Factura", "FacturaCancelada- CreaLoteCancelacionIngresosD365", oContrato.Encabezado.IdRegistro, NumPoliza, oContrato.Encabezado.FechaFactura);
                            sCadenamensaje = this.sMensaje;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteCancelacionIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            this.CreamensajeLOG(eDocumentoTMS.CANCELACION_DE_INGRESOS, oContrato.Encabezado.CodCliente, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaFactura, oContrato.Encabezado.Docto, oContrato.Encabezado.UUIDRelacionado, oContrato.Encabezado.UUIDRelacionado, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);


                        }
                        else
                        {
                           
                            oLog.proceso_id_d365 = this.sMensaje.Split('|')[1].ToString();
                            oLog.seriefolio = string.Format("C{0}{1}", oAsiento.Serie, oAsiento.Folio);
                            oLog.sesion_d365 = oSession.access_token;
                            oLog.folioD365 = oContrato.Encabezado.IdRegistro;
                            new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaBitacoraCxC(CONST_USUARIO, oLog);
                            sCadenamensaje = string.Format(@"El diario contable con folio ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", oAsiento.IdRegistro, this.sMensaje);
                             string idPoliza = oContrato.Encabezado.IdPoliza;
                            string NumPoliza = oContrato.Encabezado.NumeroPoliza;
                            ActualizarLogPolizas(idPoliza, "1", sCadenamensaje, "FacturaCancelada- CreaLoteCancelacionIngresosD365", oContrato.Encabezado.IdRegistro, NumPoliza, oContrato.Encabezado.FechaFactura);
                           
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteCancelacionIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }

                }
                catch (Exception ex)
                {
                    ActualizarLogPolizas(oContrato.Encabezado.IdPoliza, "0", sCadenamensaje, "FacturaCancelada- CreaLoteCancelacionIngresosD365", oContrato.Encabezado.IdRegistro, oContrato.Encabezado.NumeroPoliza, oContrato.Encabezado.FechaFactura);

                    sCadenamensaje = ex.Message;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaLoteCancelacionIngresosD365", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                    if (this.sMensaje.Contains("Internal Server"))
                        intentos++;
                    else
                    {
                        try
                        {
                            this.CreamensajeLOG(eDocumentoTMS.CANCELACION_DE_INGRESOS, oContrato.Encabezado.CodCliente, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaFactura, oContrato.Encabezado.Docto, oContrato.Encabezado.UUIDRelacionado, oContrato.Encabezado.UUIDRelacionado, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);

                        }
                        catch (Exception exp)
                        {
                            sCadenamensaje = exp.Message;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                            else
                                oTools.LogError(exp, "CreaLoteCancelacionIngresosD365=>CATCH", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }
                }
                finally
                {
                    if (intentos >= 2)
                        bConectado = false;

                }
            }

        }

        void CreaLoteIngresosD365(List<BBICargarFacturaCxCContractEnc> lstLote, string sDiarioContable, string sMetodoAPI, out string sresponse, bool bCreaArchivoJSON)
        {
            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
            BBICargarFacturaCxCContract oContrato = new BBICargarFacturaCxCContract();
            sresponse = string.Empty;
            string sEndPoint = string.Empty, sERROR = string.Empty,
               sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty, sResultadoSQL = string.Empty;
            LOGI_Bitacora_INFO oLog;
            LOGI_Tiempos_INFO oTiempo =  ObtenerArchivoTiempo();
            oTiempo.fechainicio = DateTime.Now;

            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {

                try
                {
                    if (!this.bConectado)
                        onCreateLogin();


                    oContrato = new BBICargarFacturaCxCContract();
                    oXML = new LOGI_XMLS_INFO();
                    //sCadenamensaje = string.Format("Procesando transacción del cliente {0} - {1} para el registro {2}", oAsiento.CodCliente, oAsiento.Descripcion, oAsiento.IdRegistro);
                    sCadenamensaje = string.Format("Procesando transacción del cliente {0} - {1} para el registro {2} . IdPoliza: {3}", oAsiento.CodCliente, oAsiento.Descripcion, oAsiento.IdRegistro,oAsiento.IdPoliza);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.proceso);
                    else
                        oTools.LogProceso(sCadenamensaje, "CreaLoteIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    if (oAsiento.XMLFactura != null)
                    {
                        if (!oTools.DevuelveXMLObject(out sresponse, ref oXML, sContentXML: oTools.Base64ToText(oAsiento.XMLFactura)))
                            throw new Exception(sresponse);
                        oAsiento.XMLFactura = oXML.CFDIContent;
                    }
                        oContrato.Encabezado = oAsiento;
                    


                    //sUUID = oCFDI.Complemento.TimbreFiscalDigital.UUID;
                    oContrato.Encabezado.IdRegistro = string.Format(@"LM/{0}{1}", oAsiento.Serie, oAsiento.Folio);
                    oContrato.Encabezado.Serie = oXML.Serie;
                    oContrato.Encabezado.XMLFactura = oXML.CFDIContent;
                    oContrato.Encabezado.Folio = Convert.ToInt32(oXML.Folio);
                    oContrato.Encabezado.Factura = string.Format("{0}{1}", oXML.Serie, oXML.Folio);
                    oContrato.Encabezado.Docto = string.Format("{0}{1}", oXML.Serie, oXML.Folio);
                    oContrato.Encabezado.UsoCFDI = oXML.Receptor.UsoCFDI;
                    oContrato.Encabezado.MetodoPagoSAT = oXML.MetodoPago;
                    oContrato.Encabezado.FechaDocto = Convert.ToDateTime(oXML.Fecha);
                    oContrato.Encabezado.UUID = oXML.Complemento.TimbreFiscalDigital.UUID;


                    ///Valida si existe en la bitacora LOCAL
                    oLog = new LOGI_Bitacora_INFO();
                    oLog.sesion_d365 = oSession.access_token;
                    oLog.folioD365 = oContrato.Encabezado.IdRegistro;
                    oLog.seriefolio = string.Format("{0}{1}", oAsiento.Serie, oAsiento.Folio);
                    LOGI_Bitacora_INFO oBitacoraRef = new LOGI_Bitacora_INFO();
                    sResultadoSQL = new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraCxC(CONST_USUARIO, oLog, ref oBitacoraRef);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sCadenamensaje = string.Format(@"El movimiento de facturación con folio {0} ya cuenta con un registro de contabilidad", oLog.folioD365);
                       // GestorPolizas gestor = new GestorPolizas();
                        gestor.AgregarRegistro(oAsiento.IdPoliza, "1", DateTime.Now, "La póliza se ha creado con anterioridad", oContrato.Encabezado.IdRegistro, oAsiento.NumeroPoliza, oAsiento.FechaFactura);

                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaLoteIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        continue;
                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);


                    oContrato.Encabezado.SistemaOrigen = Helpers.D365FOBBICxCServices.BBISistemaOrigen.OpeAdm;
                    oContrato.Encabezado.Orden = string.IsNullOrEmpty(oAsiento.Orden) ? DateTime.Now.ToString("yyyyMMddHHss") : oAsiento.Orden;
                    oContrato.Encabezado.Referencia = string.IsNullOrEmpty(oAsiento.Referencia) ? DateTime.Now.ToString("yyyyMMddHHss") : oAsiento.Referencia;
                    oContrato.Encabezado.NombreDiario = sDiarioContable; //"684CARFACT";
                    oContrato.Encabezado.UUID = oAsiento.UUID;
                    oContrato.Encabezado.ImpuestosInc = Helpers.D365FOBBICxCServices.NoYes.Yes;
                    try
                    {
                        oContrato.Encabezado.DimSucursal = oAsiento.Lineas.FirstOrDefault(x => !string.IsNullOrEmpty(x.DimSucursal)).DimSucursal;
                    }
                    catch
                    {
                        throw new Exception("No se ha encontrado la información de la unidad,valida la configuración de sucursal, centro de costo y equivalencia. Valida que la factura no está ligada a más de un viaje.");
                    }
                    oContrato.Encabezado.Total = oAsiento.Total;
                    oContrato.Encabezado.TipoCambio = 1;
                    oContrato.Encabezado.Descripcion = string.IsNullOrEmpty(oAsiento.Descripcion) ? oAsiento.Texto : oAsiento.Descripcion;


                    oContrato.Encabezado.SinComprobante = Helpers.D365FOBBICxCServices.NoYes.No;
                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                    oContrato.Encabezado.TipoRelacionUUID = EInvoiceCFDIReferenceType_MX.Invoice;
                    oContrato.Encabezado.TipoFact = D365API.Helpers.D365FOBBICxCServices.BBITipoFact.Factura;

                    List<BBICargarFacturaCxCContractLin> lstListaContrato = new List<BBICargarFacturaCxCContractLin>();
                    BBICargarFacturaCxCContractLin oLinea = new BBICargarFacturaCxCContractLin();
                    foreach (BBICargarFacturaCxCContractLin oPartida in oAsiento.Lineas)
                    {
                        if (oPartida.Credito > 0)
                        {

                            if (string.IsNullOrEmpty(oPartida.DimCeco) || string.IsNullOrEmpty(oPartida.Vehiculo) || string.IsNullOrEmpty(oPartida.DimSucursal) ||
                        string.IsNullOrEmpty(oPartida.DimDepto) || string.IsNullOrEmpty(oPartida.DimArea) || string.IsNullOrEmpty(oPartida.DimFilTer))
                                throw new Exception("Una o varias líneas de dimensiones financieras no se encuentran configuradas. (Centro de costos, Vehículo o Sucursal)");

                            if (string.IsNullOrEmpty(oPartida.GrupoImpuestos) || string.IsNullOrEmpty(oPartida.GrupoImpuestosArt) ||
                                string.IsNullOrEmpty(oPartida.CodImpuesto))
                                throw new Exception("Los códigos de impuestos no se han configurado");

                            oLinea.Credito = oPartida.Credito;
                            oLinea.CuentaContable = oPartida.CuentaContableD365;
                            oLinea.DimCeco = oPartida.DimCeco;
                            oLinea.Vehiculo = oPartida.Vehiculo;
                            oLinea.DimSucursal = oPartida.DimSucursal;
                            oLinea.DimDepto = oPartida.DimDepto;
                            oLinea.DimArea = oPartida.DimArea;
                            oLinea.DimFilTer = oPartida.DimFilTer;
                            oLinea.Texto = oPartida.Texto;
                            oLinea.GrupoImpuestos = oPartida.GrupoImpuestos;
                            oLinea.GrupoImpuestosArt = oPartida.GrupoImpuestosArt;
                            oLinea.CodImpuesto = oPartida.CodImpuesto;
                            lstListaContrato.Add(oLinea);
                        }
                    }

                    oContrato.Encabezado.Lineas = null;
                    oContrato.Lineas = lstListaContrato.ToArray();

                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, sMetodoAPI); //"BBICxCServices");
                    System.ServiceModel.Channels.Binding oBindig;
                    EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
                    oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                    oBindig.ReceiveTimeout = TimeSpan.MaxValue;
                    oBindig.SendTimeout = TimeSpan.MaxValue;
                    BBICargarFacturaCxCServiceClient oClienteD365 = new BBICargarFacturaCxCServiceClient(
binding: oBindig, endpointAddress);
                    Helpers.D365FOBBICxCServices.CallContext oContexto = new Helpers.D365FOBBICxCServices.CallContext();

                    if (oClienteD365.State != CommunicationState.Opened)
                    {
                        //Console.WriteLine("Abriendo conexión del cliente...");
                        oClienteD365.Open();
                        //Console.WriteLine($"Estado después de Open: {oClienteD365.State}");
                    }
                    // Verificar que InnerChannel esté disponible
                    var innerChannelState = ((ICommunicationObject)oClienteD365.InnerChannel).State;
                    //Console.WriteLine($"Estado del InnerChannel: {innerChannelState}");

                    if (innerChannelState == CommunicationState.Faulted)
                    {
                        throw new InvalidOperationException("InnerChannel en estado Faulted. No se puede usar OperationContextScope.");
                    }

                    oContexto.Company = oContrato.Encabezado.Compania;

                    sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    ////POLIZAS Correctas Para el nuevo response
                    //dynamic json = JsonConvert.DeserializeObject(sJSON);
                    //string idPoliza = json["Encabezado"]?["IdPoliza"]?.ToString();
                    //GestorPolizas gestor = new GestorPolizas();
                    //gestor.CargarArchivo();
                    //gestor.AgregarRegistro(idPoliza, "1", DateTime.Now, "Factura OK");

                    //PENDIENTE VALIDAR
                    using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                    {

                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                        Helpers.D365FOBBICxCServices.Infolog oInfoLog = oClienteD365.cargarFactura(oContexto, oContrato, out sresponse);
                        bool bCreado = this.RecuperaMensaje(ref sresponse);

                        if (!bCreado)
                        {
                            // Obtener IdPoliza
                            string idPoliza = oContrato.Encabezado.IdPoliza;
                            string NumPoliza = oContrato.Encabezado.NumeroPoliza;
                            //GestorPolizas gestor = new GestorPolizas();
                            gestor.AgregarRegistro(idPoliza, "0", DateTime.Now, "No se ha creado la Póliza",oContrato.Encabezado.IdRegistro, NumPoliza, oContrato.Encabezado.FechaFactura);
                            sCadenamensaje = this.sMensaje;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            this.CreamensajeLOG(eDocumentoTMS.FACTURACION_DE_VIAJES, oXML.Receptor.Rfc, oXML.Receptor.Nombre, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaDocto, oContrato.Encabezado.Docto, "", oXML.Complemento.TimbreFiscalDigital.UUID, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);

                        }
                        else
                        {
                            if (oXML.Receptor.Rfc.Trim().Equals("EBE7711037Y5", StringComparison.InvariantCultureIgnoreCase))
                            {
                                //Inserta el XML del cliente Bebidas para posteriormente ejecutar la remesa (proceso de cobro hacia la embotelladora)
                                this.GrabadocumentoXML(oXML);
                            }
                            oTiempo.ultimfechasinc = oContrato.Encabezado.FechaDocto;
                            oLog.proceso_id_d365 = this.sMensaje.Split('|')[1].ToString();
                            oLog.seriefolio = string.Format("{0}{1}", oAsiento.Serie, oAsiento.Folio);
                            new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaBitacoraCxC(CONST_USUARIO, oLog);
                            sCadenamensaje = string.Format(@"El diario contable con folio ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", oAsiento.IdRegistro, this.sMensaje);
                            
                            //POLIZAS Correctas Para el nuevo response
                            string idPoliza = oContrato.Encabezado.IdPoliza;
                            string NumPoliza = oContrato.Encabezado.NumeroPoliza;
                            ActualizarLogPolizas(idPoliza, "1", "Póliza cargada correctamente", "Factura- CreaLoteIngresosD365", oContrato.Encabezado.IdRegistro, NumPoliza, oContrato.Encabezado.FechaFactura);

                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteIngresosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //POLIZAS con ERROR Para el nuevo response
                   // GestorPolizas gestor = new GestorPolizas();
                    gestor.AgregarRegistro(oAsiento.IdPoliza, "0",DateTime.Now,ex.Message,oAsiento.FechaFactura.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);
                    //try
                    //{
                    //    if (!string.IsNullOrEmpty(oContrato.Encabezado.IdRegistro))
                    //        idreg = oContrato.Encabezado.IdRegistro;
                    //}
                    //catch { idreg = ""; }


                    sCadenamensaje = ex.Message;

                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaLoteIngresosD365", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                    if (this.sMensaje.Contains("Internal Server"))
                        intentos++;
                    else
                    {
                        try
                        {
                            this.CreamensajeLOG(eDocumentoTMS.FACTURACION_DE_VIAJES, oXML.Receptor.Rfc, oXML.Receptor.Nombre, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaDocto, oContrato.Encabezado.Docto, "", oXML.Complemento.TimbreFiscalDigital.UUID, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);

                        }
                        catch (Exception exp)
                        {
                            sCadenamensaje = exp.Message;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                            else
                                oTools.LogError(exp, "CreaLoteIngresosD365=>CATCH", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }
                }
                finally
                {
                    oTiempo.fechafinal = DateTime.Now;
                    if (intentos >= 2)
                    {
                        bConectado = false;
                        oTiempo.ultimfechasinc.AddHours(-3);
                    }
                    if (bCreaArchivoJSON)
                        GuardaArchivoTiempo(oTiempo);
                }

            }

        }

        void GrabadocumentoXML(LOGI_XMLS_INFO oFormato)
        {
            LOGI_Historico_XMLS_PD oControlXML = new LOGI_Historico_XMLS_PD(this.CONST_OPE_CONNECTION);
            Int32 iTotalBD = 0;
            string sresponseXML = string.Empty;
            try
            {
                sresponseXML = oControlXML.RecuperaCFDIs(oFormato, ref iTotalBD);
                if (sresponseXML.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                {
                    sresponseXML = oControlXML.NuevoDocumento(oFormato, DateTime.Now.Year, DateTime.Now.Month);
                    if (sresponseXML.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sresponseXML = string.Format("El archivo CFDI con UUID {0}, folio:{1} y serie: {2} se ha registrado con éxito", oFormato.Complemento.TimbreFiscalDigital.UUID, oFormato.Folio, oFormato.Serie);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sresponseXML, eType.success);
                        else
                            oTools.LogProceso(sresponseXML, "GrabadocumentoXML", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                    }

                }
                else
                {
                    sresponseXML = string.Format("El archivo CFDI con UUID {0}, folio:{1} y serie: {2} ya se encuentra registrado", oFormato.Complemento.TimbreFiscalDigital.UUID, oFormato.Folio, oFormato.Serie);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sresponseXML, eType.warning);
                    sresponseXML = oControlXML.EditaDocumento(oFormato);
                    if (sresponseXML.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sresponseXML = string.Format("Se aplicó un actualización del XML. RESULTADO {0}. {1}", sresponseXML, oFormato.CFDIContent);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sresponseXML, eType.success, bColor: false);
                        else
                            oTools.LogProceso(sresponseXML, "GrabadocumentoXML", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                    }
                }
            }
            catch (Exception ex)
            {
                sCadenamensaje = ex.Message;
                if (rchConsole != null)
                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                else
                    oTools.LogError(ex, "GrabadocumentoXML", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
            }
        }

        void CreaLotePagosD365(List<BBICargarFacturaCxCContractEnc> lstLote, out string sresponse)
        {
            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
            BBICargarFacturaCxPContract oContrato = new BBICargarFacturaCxPContract();
            sresponse = string.Empty;
            string sEndPoint = string.Empty, sERROR = string.Empty,
               sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty, sResultadoSQL = string.Empty;
            LOGI_Bitacora_INFO oLog = null;

            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {
                try
                {
                    if (!this.bConectado)
                        onCreateLogin();

                    //this.ValidaDimensionesFinancieras(oAsiento.Lineas);
                   // ActualizarLogPolizas(oAsiento.IdPoliza, "2", "IniciaProcesamiento", "Factura - CreaLotePagos365", oAsiento.Folio.ToString(), oAsiento.NumeroPoliza);

                    oContrato = new BBICargarFacturaCxPContract();
                    oContrato.Encabezado = new BBICargarFacturaCxPContractEnc();
                    string SucursalAX = string.Empty;
                    try
                    {
                        SucursalAX = oAsiento.Lineas.FirstOrDefault(x => !string.IsNullOrEmpty(x.DimSucursal) && x.Debito > 0).DimSucursal;
                    }
                    catch
                    {
                        throw new Exception("No se ha encontrado la información de la unidad,valida la configuración de sucursal, centro de costo y equivalencia.");
                    }
                    oContrato.Encabezado.SistemaOrigen = D365API.Helpers.BBISistemaOrigen.OpeAdm;
                    oContrato.Encabezado.IdRegistro = string.Format("LM/{0}-{1}{2}", SucursalAX, oAsiento.Serie, oAsiento.Folio); //oAsiento.IdRegistro;
                    oContrato.Encabezado.IdRegistro = oContrato.Encabezado.IdRegistro.Length > 30 ? oContrato.Encabezado.IdRegistro.Substring(0, 29): oContrato.Encabezado.IdRegistro;
                    oContrato.Encabezado.Descripcion = string.Format("Diario de factura para entrada {0}", oContrato.Encabezado.IdRegistro);
                    oContrato.Encabezado.NombreDiario = oAsiento.NombreDiario; //"346FACAX";///combustibles --"346FACAX";
                    oContrato.Encabezado.Compania = oCreds.ciad365;
                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.NoYes.Yes;
                    oContrato.Encabezado.FechaFactura = oAsiento.FechaFactura; 
                    oContrato.Encabezado.CodProveedor = oAsiento.CodProveedor;
 


                    oLog = new LOGI_Bitacora_INFO();
                    oLog.sesion_d365 = oSession.access_token;
                    oLog.folioD365 = oContrato.Encabezado.IdRegistro;
                    LOGI_Bitacora_INFO oBitacoraRef = new LOGI_Bitacora_INFO();
                    sResultadoSQL = new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraCxP(CONST_USUARIO, oLog, ref oBitacoraRef);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {

                        ActualizarLogPolizas(oAsiento.IdPoliza, "1", "El movimiento factura de proveedor ya cuenta con un registro de diario.", "Pasivos- CreaLotePagosD365", oAsiento.Folio.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);

                        sCadenamensaje = string.Format(@"El movimiento factura de proveedor con folio {0} ya cuenta con un registro de diario contabilidad", oLog.folioD365);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaLotePagosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa:"EXISTENTES_PASIVOS");
                        continue;

                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);




                    oXML = new LOGI_XMLS_INFO();

                    if (!oTools.DevuelveXMLObject(out sresponse, ref oXML, sContentXML: oTools.Base64ToText(oAsiento.XMLFactura)))
                        throw new Exception(sresponse);

                    oContrato.Encabezado.Docto = string.Format("{0}{1}", oXML.Serie, oXML.Folio);
                    oContrato.Encabezado.Factura = string.Format("{0}{1}", oXML.Serie, oXML.Folio);
                    oContrato.Encabezado.XMLFactura = oXML.CFDIContent;
                    oContrato.Encabezado.Texto = oAsiento.Texto;
                    oContrato.Encabezado.FechaDocto = oAsiento.FechaDocto;
                    oContrato.Encabezado.FechaVencimiento = this.CalculaVencimiento(oAsiento.CodProveedor, oAsiento.FechaDocto, oAsiento.FechaVencimiento);


                 
                    oContrato.Encabezado.SinComprobante = D365API.Helpers.NoYes.No;
                    oContrato.Encabezado.UUID = oAsiento.UUID;
                    oContrato.Encabezado.AprobadoPor = oCreds.aprobador;
                    oContrato.Encabezado.TipoFact = D365API.Helpers.BBITipoFact.Factura;//.Cancelacion;
                    oContrato.Encabezado.IdFactReferencia = 0;
                    oContrato.Encabezado.DesctoPago = "";
                    oContrato.Encabezado.Total = Math.Round( oAsiento.Total ,2);
                    oContrato.Encabezado.Subtotal = Math.Round(oAsiento.Subtotal ,2);
                    oContrato.Encabezado.Moneda = oXML.Moneda;
                    oContrato.Encabezado.TipoCambio = 1.0M;
                    oContrato.Encabezado.ImpuestosInc = D365API.Helpers.NoYes.Yes;



                    List<BBICargarFacturaCxPContractLin> lstListaContrato = new List<BBICargarFacturaCxPContractLin>();
                    BBICargarFacturaCxPContractLin oLinea = new BBICargarFacturaCxPContractLin();
                    bool bResico = false;
                    bool bContenedor = false;
                    foreach (BBICargarFacturaCxCContractLin oPartida in oAsiento.Lineas)
                    {
                        if (oPartida.Debito == 0)
                            continue;
                        if (oPartida.Texto.Contains("| DESCARGA CONTENEDOR") || oPartida.Texto.Contains("| PEAJE"))
                            oPartida.Vehiculo = "0000";

                        if (oPartida.Texto.Contains("| DESCARGA CONTENEDOR") || bContenedor)
                        {
                            oPartida.Vehiculo = "0000";
                            bContenedor = true;
                        }
                        


                        if (string.IsNullOrEmpty(oPartida.DimCeco) || string.IsNullOrEmpty(oPartida.Vehiculo) || string.IsNullOrEmpty(oPartida.DimSucursal) ||
               string.IsNullOrEmpty(oPartida.DimDepto) || string.IsNullOrEmpty(oPartida.DimArea) || string.IsNullOrEmpty(oPartida.DimFilTer))
                            throw new Exception("Una o varias líneas de dimensiones financieras no se encuentran configuradas. (Centro de costos, Vehículo o Sucursal)");

                        if (string.IsNullOrEmpty(oPartida.GrupoImpuestos) || string.IsNullOrEmpty(oPartida.GrupoImpuestosArt) ||
                            string.IsNullOrEmpty(oPartida.CodImpuesto))
                            throw new Exception("Los códigos de impuestos no se han configurado");

                        oLinea = new BBICargarFacturaCxPContractLin();
                        if (oPartida.GrupoImpuestos.Equals("PRRESICO16"))
                        {
                            bResico = true;
                            oLinea.Debito = CalcularSubtotal(oPartida.Debito, 16);
                        }
                        else
                            oLinea.Debito = Math.Round(oPartida.Debito, 2);

                        oLinea.CuentaContable = oPartida.CuentaContableD365;
                        oLinea.DimCeco = oPartida.DimCeco;
                        if (oPartida.Vehiculo == "0000")
                            oPartida.Vehiculo = string.Empty;
                        else
                            oLinea.Vehiculo = oPartida.Vehiculo;
                        oLinea.DimSucursal = oPartida.DimSucursal;
                        oLinea.DimDepto = oPartida.DimDepto;
                        oLinea.DimArea = oPartida.DimArea;
                        oLinea.DimFilTer = oPartida.DimFilTer;
                        oLinea.Texto = oPartida.Texto;
                        oLinea.GrupoImpuestos = oPartida.GrupoImpuestos;
                        oLinea.GrupoImpuestosArt = oPartida.GrupoImpuestosArt;
                        oLinea.CodImpuesto = oPartida.CodImpuesto;
                        oLinea.AprobadoPor = oCreds.aprobador;
                        lstListaContrato.Add(oLinea);
                    }

                    if (string.IsNullOrEmpty(oAsiento.CodProveedor))
                        throw new Exception("El código del proveedor D365 no se ha configurado. Núm.Poliza "+oAsiento.NumeroPoliza);
                    if (string.IsNullOrEmpty(oAsiento.NombreDiario))
                        throw new Exception("El código del diario D365 no se ha configurado. Núm.Poliza " + oAsiento.NumeroPoliza);

                    var oMtto = oAsiento.Lineas.FirstOrDefault(x => x.Texto.Contains("MANTENIMIENTO"));

                    if (bResico || oMtto != null)
                    {

                        oContrato.Encabezado.Total = Math.Round(lstListaContrato.Sum(x => x.Debito), 2);
                        oContrato.Encabezado.Subtotal = Math.Round(oAsiento.Total, 2);
                        Decimal dTotalCargo = Math.Round(lstListaContrato.Sum(x => x.Debito), 2);
                        Decimal dTotalAbono = Math.Round(oAsiento.Total, 2);
                        Decimal dTotaldiferencia = Math.Abs(dTotalCargo - dTotalAbono);
                        if (dTotaldiferencia > 0)
                        {
                            if (dTotaldiferencia >= 1)
                                throw new Exception("Se ha detectado una diferencia de más de un peso");
                            else if (dTotaldiferencia < 1)
                            {
                                sCadenamensaje = string.Format("Se ha detectado una diferencia en la poliza {0} vs {1}", dTotalCargo, dTotalAbono);
                                if (rchConsole != null)
                                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                                else
                                    oTools.LogProceso(sCadenamensaje, "CreaLotePagosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                                oContrato.Encabezado.Subtotal = oContrato.Encabezado.Total;
                            }
                        }
                    }
                    

                    //Validamos que los importes cuadren cargo vs abono 
                    oContrato.Lineas = lstListaContrato.ToArray();

                    sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, "BBICxPServices");
                    System.ServiceModel.Channels.Binding oBindig;
                    EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
                    oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");


                    oBindig.CloseTimeout = System.TimeSpan.Parse("00:50:00");
                    oBindig.OpenTimeout = System.TimeSpan.Parse("00:50:00");
                    oBindig.ReceiveTimeout = System.TimeSpan.Parse("00:50:00");
                    oBindig.SendTimeout = System.TimeSpan.Parse("00:50:00");

                    BBICargarFacturaCxPServiceClient oClienteD365 = new BBICargarFacturaCxPServiceClient(
                    binding: oBindig, endpointAddress);
                    Helpers.CallContext oContext = new Helpers.CallContext();
                    oContext.Company = oContrato.Encabezado.Compania;

                    using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                        Helpers.Infolog oInfoLog = oClienteD365.cargarFactura(oContext, oContrato, out sresponse);
                        bool bCreado = this.RecuperaMensaje(ref sresponse);

                        if (!bCreado)
                        {
                            sCadenamensaje = this.sMensaje;
                            ActualizarLogPolizas(oAsiento.IdPoliza, "0", this.sMensaje, "Pasivos -CreaLoteIngresosD365", oAsiento.Folio.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLotePagosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                            this.CreamensajeLOG(eDocumentoTMS.REGISTRO_DE_PASIVOS, oXML.Emisor.Rfc, oXML.Emisor.Nombre, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaDocto, oContrato.Encabezado.Docto, "", oXML.Complemento.TimbreFiscalDigital.UUID, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);
                        }
                        else
                        {

                            oLog.proceso_id_d365 = this.sMensaje.Split('|')[1].ToString();
                            oLog.seriefolio = string.Format("{0}{1}", oAsiento.Serie, oAsiento.Folio);
                            new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaBitacoraCxP(CONST_USUARIO, oLog);

                            ActualizarLogPolizas(oAsiento.IdPoliza, "1", "Poliza Ok_ Pasivos", "Pasivos- CreaLotePagosD365" , oAsiento.Folio.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);

                            sCadenamensaje = string.Format(@"La factura de proveedor con folio GM Transport ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", oAsiento.IdRegistro, this.sMensaje);
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLotePagosD365", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa:"PASIVOS_NUEVOS_D365");
                        }

                    }

                }
                catch (Exception ex)
                {
                    ActualizarLogPolizas(oAsiento.IdPoliza, "0", ex.Message, "Pasivos- CreaLotePagosD365", oAsiento.Folio.ToString(), oAsiento.NumeroPoliza, oAsiento.FechaFactura);
                   
                    sCadenamensaje = ex.Message;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaLotePagosD365", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                     oObjAPI.BitacoraAPI("Error en Pasivos, IdPoliza: " + oAsiento.IdPoliza, sCadenamensaje);

                    this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                    if (this.sMensaje.Contains("Internal Server"))
                        intentos++;
                    else
                    {
                        try
                        {
                            string RFC = "";
                            string TimbreFiscal = "";
                            if (!string.IsNullOrEmpty(oContrato.Encabezado.RFC))
                                RFC = oContrato.Encabezado.RFC;
                            if (!string.IsNullOrEmpty(oXML.Complemento.TimbreFiscalDigital.UUID))
                                TimbreFiscal = oXML.Complemento.TimbreFiscalDigital.UUID;

                            this.CreamensajeLOG(eDocumentoTMS.REGISTRO_DE_PASIVOS, RFC, oXML.Emisor.Nombre, oContrato.Encabezado.IdRegistro, oContrato.Encabezado.FechaDocto, oContrato.Encabezado.Docto, "", TimbreFiscal, oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);
                        }
                        catch (Exception exp)
                        {
                            sCadenamensaje = exp.Message;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                            else
                                oTools.LogError(exp, "CreaLotePagosD365=>CATCH", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }
                }
                finally
                {
                    if (intentos >= 2)
                        bConectado = false;

                }
            }

        }

        DateTime CalculaVencimiento(string sCodigoProveedor, DateTime FechaFactura, DateTime FechaVencimiento)
        {
            try
            {
                string sDiasPAGO = string.Empty;
                //CONEXIONZAP
            //    new PD.Tablas.FUEL.LOGI_Proveedores_PD(this.oConfig.conexionzap).ObtenerDiasPAgo(CONST_USUARIO, ref sDiasPAGO, sCodigoProveedor);
                return FechaFactura.AddDays(Convert.ToInt32(sDiasPAGO.Trim().Replace("P", string.Empty).Trim()));
            }
            catch {

                return FechaVencimiento; //FechaFactura.AddDays(0);
            }
        
        }
        Decimal CalcularSubtotal(decimal total, decimal tasaImpuesto)
        {
            // Dividimos el total entre (1 + tasa de impuesto) para obtener el subtotal
            decimal subtotal = total / (1 + tasaImpuesto / 100);
            decimal RESICO = subtotal * 0.0125m;

            Decimal subtotalNETO = total - RESICO; // Redondeamos a 2 decimales
            return Math.Round(subtotalNETO, 2); 
        }

        void CreaconsumoCombustibleLiquidacion(List<BBICargarFacturaCxCContractEnc> lstLote, out string sresponse)
        {

            sresponse = string.Empty;
            string sEndPoint = string.Empty, sERROR = string.Empty,
               sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty, sResultadoSQL = string.Empty;
            LOGI_LiquidacionBitacora_INFO oBitacora = null;
            //Consulta conceptos de cuentas que no deben viajar a D365 y cuentas que generan un consumo de combustible en TALLER

            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {
                try
                {

                    oBitacora = new LOGI_LiquidacionBitacora_INFO();
                    oBitacora.folioTMS = oAsiento.IdRegistro;
                    oBitacora.folioViaje = oAsiento.Serie;
                    sResultadoSQL = new LOGI_Liquidacion_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ListaBitacora(CONST_USUARIO, oBitacora);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sCadenamensaje = string.Format(@"El movimiento de consumo de combustible con folio {0} ya cuenta con un registro en bitacora de consumo", oBitacora.folioTMS);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaconsumoCombustibleLiquidacion", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        continue;

                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);

                    new LOGI_Liquidacion_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaMovimientoCombustible(CONST_USUARIO, this.GeneracontenidoCombustible(oAsiento), oBitacora);
                }
                catch (Exception ex)
                {
                    sCadenamensaje = ex.Message;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaconsumoCombustibleLiquidacion", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                }
                finally
                {
                }
            }

        }

        void CreaLoteComprobacionGastos(List<BBICargarFacturaCxCContractEnc> lstLote, out string sresponse)
        {
            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
            Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContract oContrato = new Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContract();
            sresponse = string.Empty;
            string sEndPoint = string.Empty, sERROR = string.Empty,
               sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty, sResultadoSQL = string.Empty;
            LOGI_LiquidacionBitacora_INFO oBitacora = null;
            //Consulta conceptos de cuentas que no deben viajar a D365 y cuentas que generan un consumo de combustible en TALLER
            LOGI_Bitacora_INFO oLog = null;

            List<LOGI_Combustibles_INFO> lstConceptos = new List<LOGI_Combustibles_INFO>();
             new LOGI_Liquidacion_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ListadodeAlmacenes(CONST_USUARIO, ref lstConceptos);
            if (lstConceptos.Count == 0)
                throw new Exception("No se ha encontrado el listado de almacenes");


            foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLote)
            {
                try
                {
                    if (!this.bConectado)
                        onCreateLogin();


                    oLog = new LOGI_Bitacora_INFO();
                    oLog.folioD365 = oAsiento.IdRegistro;

                    oXML = new LOGI_XMLS_INFO();
                    oContrato = new Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContract();
                    Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractFac factura;
                    List<Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractFac> listaFac;
                    Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractLin linea;
                    List<Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractLin> lista;
                    string FolioViajeTMS = oAsiento.Descripcion.ToUpper().Replace("GASTO PARA VIAJE", string.Empty);
                    string FolioOperador = string.Format("{0}-{1}", oAsiento.Texto.Trim().Split('|')[1].Trim().Split('-')[0], oAsiento.Texto.Trim().Split('|')[1].Trim().Split('-')[1]);
                    oContrato.Encabezado = new Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractEnc();
                    oContrato.Encabezado.SistemaOrigen = Helpers.D365FOBBICGCxPServices.BBISistemaOrigen.OpeAdm;
                    oContrato.Encabezado.IdRegistro = string.Format("LM/CV{0}", oAsiento.IdRegistro);
                    oContrato.Encabezado.Compania = oCreds.ciad365;
                    oContrato.Encabezado.Descripcion = string.Format("Gastos para viaje {0}", oAsiento.Serie);
                    oContrato.Encabezado.NombreDiario = "348GSTOPER";
                    oContrato.Encabezado.Texto =  string.Format("Gastos para viaje {0} | {1}", oAsiento.Serie, FolioOperador); //oAsiento.Texto;
                    oContrato.Encabezado.ImpuestosInc = Helpers.D365FOBBICGCxPServices.NoYes.Yes;
                    oContrato.Encabezado.FechaFactura = oAsiento.FechaFactura; //DateTime.ParseExact(oEncabezado.fechaContable, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    oContrato.Encabezado.TipoCuenta = BBITipoCuentaLineaDiario.Contabilidad;
                    oContrato.Encabezado.CuentaContable = oCreds.cuentaviaticos;
                    oContrato.Encabezado.Factura = oAsiento.Factura;
                    oContrato.Encabezado.Docto = oAsiento.Docto;
                    oContrato.Encabezado.AprobadoPor = oCreds.aprobador;
                    oContrato.Encabezado.TipoFact = Helpers.D365FOBBICGCxPServices.BBITipoFact.Factura;


                    LOGI_Bitacora_INFO oBitacoraRef = new LOGI_Bitacora_INFO();
                    sResultadoSQL = new LOGI_FondoFijo_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).ConsultaBitacoraComprobacionGasto(CONST_USUARIO, oLog, ref oBitacoraRef);
                    if (sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sCadenamensaje = string.Format(@"El movimiento comprobación de gastos {0} ya cuenta con un registro de poliza y asiento diario D365", oLog.folioD365);
                        if (rchConsole != null)
                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                        else
                            oTools.LogProceso(sCadenamensaje, "CreaLoteComprobacionGastos", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        continue;
                    }
                    else if (!sResultadoSQL.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception(sResultadoSQL);


                    oContrato.Encabezado.Subtotal = oAsiento.Lineas.Sum(x => x.Debito);
                    oContrato.Encabezado.Total = oAsiento.Lineas.Sum(x => x.Debito);
                    oContrato.Encabezado.Moneda = "MXN";
                    oContrato.Encabezado.TipoCambio = 1.0M; 
                    oContrato.Encabezado.DimSucursal = oAsiento.Lineas.FirstOrDefault(x => !string.IsNullOrEmpty(x.DimSucursal) && x.Debito > 0).DimSucursal;
                    oContrato.Encabezado.DimFilTer = oAsiento.Lineas.FirstOrDefault(x => !string.IsNullOrEmpty(x.DimSucursal) && x.Debito > 0).DimFilTer;
                    listaFac = new List<BBICargarFacturaCGCxPContractFac>();
                    //recorremos el detalle para detectar cuales líneas tienen XML y excluimos la cuenta #25 que corresponde al operador 
                    int conteo = 0;
                    Decimal IVA = 0;
                    foreach (BBICargarFacturaCxCContractLin oPartida in oAsiento.Lineas)
                    {
                        if (oPartida.Debito > 0)
                        {
                            if (string.IsNullOrEmpty(oPartida.DimCeco) || string.IsNullOrEmpty(oPartida.Vehiculo) || string.IsNullOrEmpty(oPartida.DimSucursal) ||
                     string.IsNullOrEmpty(oPartida.DimDepto) || string.IsNullOrEmpty(oPartida.DimArea) || string.IsNullOrEmpty(oPartida.DimFilTer))
                                throw new Exception("Una o varias líneas de dimensiones financieras no se encuentran configuradas. (Centro de costos, Vehículo o Sucursal)");

                            if (string.IsNullOrEmpty(oPartida.GrupoImpuestos) || string.IsNullOrEmpty(oPartida.GrupoImpuestosArt) ||
                                string.IsNullOrEmpty(oPartida.CodImpuesto))
                                throw new Exception("Los códigos de impuestos no se han configurado. "+oAsiento.IdPoliza);

                            //POR CADA LÍNEA AGREGAMOS EL REGISTRO CONTABLE
                            factura = new BBICargarFacturaCGCxPContractFac();
                            lista = new List<BBICargarFacturaCGCxPContractLin>();
                            linea = new BBICargarFacturaCGCxPContractLin();

                            //todas las líneas por default tienen la fecha del documento, si tienen comprobante heredan el valor de la fecha emitida del XML
                            factura.FechaDocto = oAsiento.FechaDocto;
                            factura.SinComprobante = Helpers.D365FOBBICGCxPServices.NoYes.Yes;
                            factura.Texto = String.Format("{0} - {1}", oAsiento.Factura, oAsiento.Descripcion);
                            factura.Texto = factura.Texto.Length > 60 ? factura.Texto.Substring(0, 59) : factura.Texto;
                            factura.TipoOperacion = VendorOperationType_MX.Blank;
                            factura.TipoProveedor = VendorType_MX.Blank;
                            factura.Vehiculo = oPartida.Vehiculo;
                            factura.Docto = string.Format("{0}{1}", oAsiento.Docto, conteo);
                            factura.Factura = string.Format("{0}{1}", oAsiento.Docto, conteo);

                            factura.Factura = factura.Factura.Length > 20 ? factura.Factura.Substring(0, 20) : factura.Factura;
                            factura.Docto = factura.Docto.Length > 20 ? factura.Docto.Substring(0, 20) : factura.Docto;

                            linea.Debito = oPartida.Debito;
                            linea.Credito = oPartida.Credito;

                            if (!string.IsNullOrEmpty(oPartida.RFC))
                            {
                                if (!oPartida.RFC.Equals("MQ=="))
                                {
                                    oXML = new LOGI_XMLS_INFO();

                                    if (!oTools.DevuelveXMLObject(out sresponse, ref oXML, sContentXML: oTools.Base64ToText(oPartida.RFC)))
                                        throw new Exception(sresponse);
                                    /*LOGI_XMLS_INFO oCFDI = new LOGI_XMLS_INFO();
                                    if (oTools.DevuelveXMLObject(out response, ref oCFDI, sContentXML: d.XML))
                                    {*/
                                    factura.TipoOperacion = VendorOperationType_MX.Other;
                                    factura.TipoProveedor = VendorType_MX.DomesticVendor;
                                    factura.SinComprobante = Helpers.D365FOBBICGCxPServices.NoYes.No; //este es momentaneo en lo que se agrega XML
                                    factura.Factura = string.Format("{0}{1}", oXML.Serie, oXML.Folio);
                                    factura.Docto = string.Format("{0}{1}", oXML.Serie, oXML.Folio);


                                    factura.Factura = factura.Factura.Length > 20 ? factura.Factura.Substring(0,20) : factura.Factura;
                                    factura.Docto = factura.Docto.Length > 20 ? factura.Docto.Substring(0, 20) : factura.Docto;

                                    factura.UUID = oXML.Complemento.TimbreFiscalDigital.UUID;
                                    factura.XMLFactura =  oXML.CFDIContent;
                                    factura.RFC = oXML.Emisor.Rfc;
                                    factura.NombreProveedor = oXML.Emisor.Nombre;
                                    factura.FechaDocto = Convert.ToDateTime(oXML.Fecha);
                                    linea.Debito = oXML.Total;
                                    IVA = oXML.Impuestos.TotalImpuestosTrasladados;
                                    //}
                                }
                            }

                            var oCuentaD365 = lstConceptos.FirstOrDefault(x => x.cuentatms.Equals(oPartida.CuentaContableD365, StringComparison.InvariantCultureIgnoreCase));
                            linea.CuentaContable = oCuentaD365 == null ? oPartida.CuentaContableD365 : oCuentaD365.cuentad365;
                            linea.Texto = string.Format("{0} | {1}-{2}",oAsiento.Serie, oPartida.Texto.ToUpper().Replace("GASTO PARA VIAJE"+FolioViajeTMS, string.Empty).Trim().Split('|')[0].Trim(), FolioOperador);
                            linea.Texto = linea.Texto.Length > 60 ? linea.Texto.Substring(0, 59) : linea.Texto;                           
                            linea.DimArea = oPartida.DimArea;
                            linea.DimCeco = oPartida.DimCeco;
                            linea.DimSucursal = oPartida.DimSucursal;
                            linea.DimDepto = oPartida.DimDepto;
                            linea.DimFilTer = oPartida.DimFilTer;
                            linea.Vehiculo = oPartida.Vehiculo;
                            linea.CodImpuesto = oPartida.CodImpuesto;
                            //linea.GrupoImpuestos = oPartida.GrupoImpuestos;
                            //linea.GrupoImpuestosArt = oPartida.GrupoImpuestosArt; 
                            //}
                            lista.Add(linea);
                            factura.Lineas = lista.ToArray();
                            listaFac.Add(factura);
                            conteo++;
                        }
                    }
                    oContrato.Encabezado.Total = oContrato.Encabezado.Total + IVA;
                    oContrato.Facturas = listaFac.ToArray();
                    sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, "BBICxPServices");

                    System.ServiceModel.Channels.Binding oBindig;
                    EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);

                    oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                    //oBindig.SendTimeout = new TimeSpan(1000000);
                    oBindig.ReceiveTimeout = TimeSpan.MaxValue;
                    oBindig.SendTimeout = TimeSpan.MaxValue;
                    BBICargarFacturaGCxPServicesClient oClienteD365 = new BBICargarFacturaGCxPServicesClient(
                    binding: oBindig, endpointAddress);
                    Helpers.D365FOBBICGCxPServices.CallContext oContext = new Helpers.D365FOBBICGCxPServices.CallContext();
                    oContext.Company = oContrato.Encabezado.Compania;

                    using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                        Helpers.D365FOBBICGCxPServices.Infolog oInfoLog = oClienteD365.cargarFactura(oContext, oContrato, out sresponse);
                        bool bCreado = this.RecuperaMensaje(ref sresponse);
                        if (!bCreado)
                        {
                            sCadenamensaje = this.sMensaje;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.warning);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteComprobacionGastos", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                            this.CreamensajeLOG(eDocumentoTMS.COMPROBACION_DE_GASTOS, "", "", oContrato.Encabezado.IdRegistro, DateTime.Now, oContrato.Encabezado.Docto, "", "", oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);
                        }
                        else
                        {
                            /*oLog.proceso_id_d365 = this.sMensaje.Split('|')[1].ToString();
                            oLog.seriefolio = string.Format("{0}{1}", oAsiento.Serie, oAsiento.Folio);
                            new LOGI_NominaMO_PD(this.CONST_LOG_CONNECTION, this.CONST_OPE_CONNECTION).InsertaBitacoraCxP(CONST_USUARIO, oLog);*/


                            sCadenamensaje = string.Format(@"La comprobación de gasto con folio GM Transport ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", oAsiento.IdRegistro, this.sMensaje);
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
                            else
                                oTools.LogProceso(sCadenamensaje, "CreaLoteComprobacionGastos", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }

                }
                catch (Exception ex)
                {
                    sCadenamensaje = ex.Message;
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                    else
                        oTools.LogError(ex, "CreaLoteComprobacionGastos", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

                    this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                    if (this.sMensaje.Contains("Internal Server"))
                        intentos++;
                    else
                    {
                        try
                        {
                            this.CreamensajeLOG(eDocumentoTMS.COMPROBACION_DE_GASTOS, "", "", oContrato.Encabezado.IdRegistro, DateTime.Now, oContrato.Encabezado.Docto, "", "", oConfig.URLApi, oContrato.Encabezado.Total, oContrato.Encabezado.Subtotal, sCadenamensaje, sJSON);
                        }
                        catch (Exception exp)
                        {
                            sCadenamensaje = exp.Message;
                            if (rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                            else
                                oTools.LogError(exp, "CreaLoteComprobacionGastos=>CATCH", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
                        }
                    }
                }
                finally
                {
                    if (intentos >= 2)
                        bConectado = false;

                }
            }

        }

        void CreamensajeLOG(eDocumentoTMS TIPO_DOCTO, string RFC, string Socio, string FolioAsignado, DateTime FechaContable, string Factura, string Facturaref, string UUID, string URL, Decimal Total, Decimal Subtotal, String mensaje, String JSONPATH)
        {

            try
            {
                LOGI_PolizasD365_INFO oBitacora = new LOGI_PolizasD365_INFO();

                if (!string.IsNullOrEmpty(JSONPATH))
                {
                    //corresponde al JSON enviado hacia D365
                    string sPathFOLDER = string.Format(@"{0}\{1}", DateTime.Now.Year, DateTime.Now.Month);
                    string sPATH = string.Format(@"{0}\{1}", CONST_PATH_JSONS, sPathFOLDER);
                    if (!Directory.Exists(sPATH))
                        Directory.CreateDirectory(sPATH);
                    string sJSONombre = string.Format(@"{0}_{1}.json", Convert.ToInt32(TIPO_DOCTO), FolioAsignado.Replace("/","."));
                    string sFULLPath = string.Format(@"{0}\{1}", sPATH, sJSONombre);
                    oBitacora.JSONPATH = string.Format(@"{0}\{1}", sPathFOLDER, sJSONombre);
                    if (!File.Exists(sFULLPath))
                        File.WriteAllText(sFULLPath, JSONPATH);
                }

                oBitacora.tipo = TIPO_DOCTO;
                oBitacora.rfc = RFC;
                oBitacora.socio = Socio;
                oBitacora.folio = FolioAsignado;
                oBitacora.fechaconta = FechaContable;
                oBitacora.factura = Factura;
                oBitacora.facturaref = Facturaref;
                oBitacora.uuid = UUID;
                oBitacora.URL = URL;
                oBitacora.total = Total;
                oBitacora.subtotal = Subtotal;
                oBitacora.mensaje = mensaje;
                string sResultadoSQL = oEventosBitacora.CrearegistroBitacora(CONST_USUARIO, oBitacora);
                if (!sResultadoSQL.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception(sResultadoSQL);
            }
            catch (Exception ex)
            {
                sCadenamensaje = ex.Message;
                if (rchConsole != null)
                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
                else
                    oTools.LogError(ex, "CreamensajeLOG", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
            }

        }

        List<LOGI_LiquidacionCombus_INFO> GeneracontenidoCombustible(BBICargarFacturaCxCContractEnc oAsiento)
        {
            List<LOGI_LiquidacionCombus_INFO> lstCombus = new List<LOGI_LiquidacionCombus_INFO>();
            LOGI_LiquidacionCombus_INFO oLinea = null;

            foreach (BBICargarFacturaCxCContractLin oPartida in oAsiento.Lineas)
            {
                if (string.IsNullOrEmpty(oPartida.DimCeco) || string.IsNullOrEmpty(oPartida.Vehiculo) || string.IsNullOrEmpty(oPartida.DimSucursal) ||
                      string.IsNullOrEmpty(oPartida.DimDepto) || string.IsNullOrEmpty(oPartida.DimArea) || string.IsNullOrEmpty(oPartida.DimFilTer))
                    throw new Exception("Una o varias líneas de dimensiones financieras no se encuentran configuradas. (Centro de costos, Vehículo o Sucursal)");

                oLinea = new LOGI_LiquidacionCombus_INFO();
                oLinea.area = oPartida.DimArea;
                oLinea.centro = oPartida.DimCeco;
                oLinea.cuenta = oPartida.CuentaContableD365;
                oLinea.depto = oPartida.DimDepto;
                oLinea.filial = oPartida.DimFilTer;
                oLinea.sucursal = oPartida.DimSucursal;
                oLinea.viaje = oAsiento.Serie;
                oLinea.total = oPartida.Debito;
                oLinea.vehiculo = oPartida.Vehiculo;
                oLinea.liquidacionID = oAsiento.Folio;
                oLinea.fechaLiquidacion = oAsiento.FechaDocto;
                oLinea.texto = oPartida.Texto;
                try
                {
                    oLinea.comentarios = oAsiento.Texto.Split('|')[0];
                }
                catch {
                    oLinea.comentarios = string.Empty;
                }
                lstCombus.Add(oLinea);

            }

            return lstCombus;
        }

        public LOGI_Tiempos_INFO ObtenerArchivoTiempo()
        {
            LOGI_Tiempos_INFO oTiempos = new LOGI_Tiempos_INFO();
            oTiempos.ultimfechasinc = DateTime.Now.AddHours(-4);
            if (File.Exists(sPathFechaSinc))
            {
                string jsonContent = File.ReadAllText(sPathFechaSinc);
                oTiempos = JsonConvert.DeserializeObject<LOGI_Tiempos_INFO>(jsonContent);
            }

            return oTiempos;
        }

        public void GuardaArchivoTiempo(LOGI_Tiempos_INFO oTiempo)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(oTiempo);
                File.WriteAllText(sPathFechaSinc, jsonContent);
            }
            catch { }
        }

        bool RecuperaMensaje(ref string response)
        {
            response = response.Replace("'", string.Empty);
            var spl = response.Split('|');
            this.sMensaje = response;
            return spl[0].Trim().Equals("t", StringComparison.InvariantCultureIgnoreCase);
        }


        #region METODOORIGINAL
        //public LOGI_EstadoControlProcesos_INFO DescargaEjecutaMovimientosIngreso(string cnxBitacora, string sFechaInicio, string sFechaFinal, bool bLeeTXT = false, bool bCreaArchivoJSON = true, int ProFacturas = 1, int ProCancelacionFacturas = 1, int ProNotasCredito = 1, int ProMovimientosBancarios = 1, int ProPasivos = 0)
        //{
        //    LOGI_EstadoControlProcesos_INFO estadoProcesos = new LOGI_EstadoControlProcesos_INFO
        //    {
        //        ProFacturas = ProFacturas == 0,
        //        ProCancelacionFacturas = ProCancelacionFacturas == 0,
        //        ProNotasCredito = ProNotasCredito == 0,
        //        ProMovimientosBancarios = ProMovimientosBancarios == 0,
        //        ProPasivos = ProPasivos == 0
        //    };

        //    //bool bCreado = false;
        //    //int PROCESADOS = 0, APLICADOS = 0, FALLIDOS = 0;
        //    string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
        //        sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
        //    List<BBICargarFacturaCxCContractEnc> lstLotes = null;
        //    LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
        //    List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
        //    LOGI_Documentos_INFO oDocumento = null;
        //    String sJSONPATHFile = string.Empty;

        //    DateTime startDate = Convert.ToDateTime(sFechaInicio);
        //    DateTime endDate = Convert.ToDateTime(sFechaFinal);
        //    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //    {
        //        try
        //        {
        //            if (!bLeeTXT)
        //            {
        //                sCadenamensaje = string.Format("Procesando lote de movimientos en el período {0} hasta {1}", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
        //                if (rchConsole != null)
        //                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                else
        //                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

        //                //Traer la primera página de resultados, por default pólizas de pasivos
        //               // GMTApi oObjAPI = new GMTApi();
        //               // oResponse = oObjAPI.Consume(oConfig, oCreds, date, cnxBitacora,"Pasivos");
        //               //this.sMensaje = oResponse.Content;


        //                /*         //Se recupera la información de los documentos que se pueden interfazar hacia Dynamics D365
        //                        oCliente = new RestClient(oConfig.url_tms);
        //                         oCliente.Timeout = 900000000;
        //                         oRequest = new RestRequest(Method.POST);
        //                         oRequest.AddHeader("Content-Type", "application/json");
        //                         oRequest.AddParameter("OutputFormat", "JSON", ParameterType.GetOrPost);
        //                         oRequest.AddParameter("RFCEmpresa", oConfig.rfc_tms, ParameterType.GetOrPost);
        //                        // oRequest.AddParameter("ApiKey", oConfig.api_tms, ParameterType.GetOrPost);
        //                         oRequest.AddParameter("Parametros", string.Format(@"{{ ""Clase"":""ClsProPolizas"", ""Metodo"":""GetPolizasDetalle"", ""Parametros"":{{ ""dFechaInicial"":""{0}"", ""dFechaFinal"":""{1}""  }}}}", date.ToString("yyyyMMdd"), date.ToString("yyyyMMdd")), ParameterType.GetOrPost);
        //                         oRequest.AddParameter("resource", oCreds.resource, ParameterType.GetOrPost);
        //                         ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
        //                         oResponse = oCliente.Execute(oRequest);
        //                         oTools.LogProceso(oResponse.Content, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "DAPI365", CONST_USUARIO);
        //                */
        //                oTools.LogProceso(oResponse.Content, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "DAPI365", CONST_USUARIO);
        //                this.sMensaje = oResponse.Content;
        //            }
        //            else
        //            {
        //                oResponse.StatusCode =HttpStatusCode.OK;
        //                sJSONPATHFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt"); // relative path
        //                this.sMensaje = sJSONPATHFile;
        //            }

        //            if (oResponse.StatusCode == HttpStatusCode.OK)
        //            {
        //                this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);

        //                //Encabezado de la respuesta
        //                LOGI_ResponseTMS_INFO oResponse = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje,
        //                 new JsonSerializerSettings
        //                 {
        //                     NullValueHandling = NullValueHandling.Ignore
        //                 });
        //                if (oResponse.Result[0].Encabezado == null)
        //                {
        //                    sCadenamensaje = oResponse.Result[0].Mensaje;
        //                    if (rchConsole != null)
        //                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                    else
        //                        oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //                }
        //                else
        //                {

        //                    oDocontrol.ListaDocumentos(CONST_USUARIO, ref lstDocumentos, oParamdoc);
        //                    sCadenamensaje = string.Format("Se han encontrado un total de {0} movimientos para el período seleccionado", oResponse.Result[0].Encabezado.Count);
        //                    if (rchConsole != null)
        //                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                    else
        //                        oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);


        //                    if (ProMovimientosBancarios == 1)
        //                    {
        //                        lstLotes = new List<BBICargarFacturaCxCContractEnc>();
        //                        lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProMovimientosBancarios" && !string.IsNullOrEmpty(x.IdRegistro)).ToList();
        //                        if (lstLotes.Count > 0)
        //                        {
        //                            sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para reposición de fondo fijo", lstLotes.Count);
        //                            if (rchConsole != null)
        //                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                            else
        //                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //                            this.CreaLoteReposicionFondo(lstLotes, out sresponse);
        //                        }

        //                        estadoProcesos.ProMovimientosBancarios = true;
        //                    }

        //                    if (ProFacturas == 1)
        //                    {
        //                        lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado.Equals("ProFacturas", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30 && string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();
        //                        if (lstLotes.Count > 0)
        //                        {
        //                            oDocumento = new LOGI_Documentos_INFO();
        //                            oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 3);
        //                            if (oDocumento == null)
        //                                throw new Exception("No se ha encontrado la configuración de documentos de tipo ingreso para enviar a D365");

        //                            sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para polizas contables de ingreso", lstLotes.Count);
        //                            if (rchConsole != null)
        //                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                            else
        //                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

        //                            this.CreaLoteIngresosD365(lstLotes, oDocumento.diario, oDocumento.metodo, out sresponse, bCreaArchivoJSON: bCreaArchivoJSON);
        //                        }

        //                        estadoProcesos.ProFacturas = true;
        //                    }

        //                    if (ProCancelacionFacturas == 1)
        //                    {
        //                        lstLotes = new List<BBICargarFacturaCxCContractEnc>();
        //                        lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProFacturas" && !string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();
        //                        if (lstLotes.Count > 0)
        //                        {
        //                            oDocumento = new LOGI_Documentos_INFO();
        //                            oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 8);
        //                            if (oDocumento == null)
        //                                throw new Exception("No se ha encontrado la configuración de documentos de tipo cancelación de ingreso para enviar a D365");


        //                            sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para cancelación de facturas", lstLotes.Count);
        //                            if (rchConsole != null)
        //                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                            else
        //                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

        //                            this.CreaLoteCancelacionIngresosD365(lstLotes, out sresponse);
        //                        }

        //                        estadoProcesos.ProCancelacionFacturas = true;
        //                    }

        //                    if (ProNotasCredito == 1)
        //                    {
        //                        lstLotes = new List<BBICargarFacturaCxCContractEnc>();
        //                        lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProNotasCredito" && !string.IsNullOrEmpty(x.UUIDRelacionado)).ToList();
        //                        if (lstLotes.Count > 0)
        //                        {
        //                            oDocumento = new LOGI_Documentos_INFO();
        //                            oDocumento = lstDocumentos.FirstOrDefault(x => x.numerodoc == 5);
        //                            if (oDocumento == null)
        //                                throw new Exception("No se ha encontrado la configuración de documentos de tipo notas de credito para enviar a D365");


        //                            sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para notas de créditos", lstLotes.Count);
        //                            if (rchConsole != null)
        //                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                            else
        //                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);

        //                            this.CreaLoteNotasCreditoIngresosD365(lstLotes, out sresponse);
        //                        }

        //                        estadoProcesos.ProNotasCredito = true;
        //                    }

        //                    if (ProPasivos == 1)
        //                    {
        //                        lstLotes = new List<BBICargarFacturaCxCContractEnc>();
        //                        lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProPasivos" && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30).ToList();
        //                        if (lstLotes.Count > 0)
        //                        {
        //                            sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para polizas contables de tipo pasivos", lstLotes.Count);
        //                            if (rchConsole != null)
        //                                oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                            else
        //                                oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //                            this.CreaLotePagosD365(lstLotes, out sresponse);
        //                        }

        //                        estadoProcesos.ProPasivos = true;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                sCadenamensaje = "No se ha podido leer la información proveniente de TMS";
        //                if (rchConsole != null)
        //                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
        //                else
        //                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            sERROR = string.Format(@"El proceso de replica no se ha realizado. ERROR {0}", ex.Message);
        //            if (rchConsole != null)
        //                oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
        //            else
        //                oTools.LogError(ex, "DescargaMovimientoTMS", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //        }
        //        finally
        //        {
        //            GC.Collect();
        //            if (bLeeTXT)
        //            {
        //                try
        //                {
        //                    string sPathJSONtmp = string.Format(@"{0}\JSON\Procesados\{1}_JSON.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("ddMMyyyyhhssmm"));
        //                    File.Move(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt", sPathJSONtmp);
        //                }
        //                catch { }
        //            }
        //        }
        //    }

        //    return estadoProcesos;
        //}

        //public void DescargaEjecutaMovimientosPasivos(string sFechaInicio, string sFechaFinal, bool bLeeTXT = false, bool bIgnoraFechas = false)
        //{
        //    string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
        //        sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
        //    List<BBICargarFacturaCxCContractEnc> lstLotes = null;
        //    LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
        //    List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
        //    String sJSONPATHFile = string.Empty;
        //    DateTime startDate = Convert.ToDateTime(sFechaInicio);
        //    DateTime endDate = Convert.ToDateTime(sFechaFinal);
        //    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //    {
        //        try
        //        {
        //            if (!bIgnoraFechas)
        //            {
        //                if (!CondicionaEjecucion(sFechaInicio))
        //                {
        //                    sCadenamensaje = string.Format("La fecha a ejecutar {0} hasta {1}, se encuentra fuera del alcance de sincronización", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
        //                    if (rchConsole != null)
        //                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                    else
        //                        oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa: "PASIVOS");
        //                    continue;
        //                }
        //            }

        //            //oEventosBitacora.CreaprocesoNotificacionERRORES(CONST_USUARIO);
        //            if (!bLeeTXT)
        //            {
        //                sCadenamensaje = string.Format("Procesando lote de movimientos en el período {0} hasta {1}", date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));
        //                if (rchConsole != null)
        //                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                else
        //                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa: "PASIVOS");


        //                //Se recupera la información de los documentos que se pueden interfazar hacia Dynamics D365
        //                oCliente = new RestClient(oConfig.url_tms);
        //                oCliente.Timeout = 900000000;
        //                oRequest = new RestRequest(Method.POST);
        //                oRequest.AddHeader("Content-Type", "application/json");
        //                oRequest.AddParameter("OutputFormat", "JSON", ParameterType.GetOrPost);
        //                oRequest.AddParameter("RFCEmpresa", oConfig.rfc_tms, ParameterType.GetOrPost);
        //                oRequest.AddParameter("ApiKey", oConfig.api_tms, ParameterType.GetOrPost);
        //                oRequest.AddParameter("Parametros", string.Format(@"{{ ""Clase"":""ClsProPolizas"", ""Metodo"":""GetPolizasDetalle"", ""Parametros"":{{ ""dFechaInicial"":""{0}"", ""dFechaFinal"":""{1}""  }}}}", date.ToString("yyyyMMdd"), date.ToString("yyyyMMdd")), ParameterType.GetOrPost);
        //                oRequest.AddParameter("resource", oCreds.resource, ParameterType.GetOrPost);
        //                ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
        //                oResponse = oCliente.Execute(oRequest);
        //                this.sMensaje = oResponse.Content;
        //            }
        //            else
        //            {
        //                oResponse.StatusCode = HttpStatusCode.OK;
        //                sJSONPATHFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt"); // relative path
        //                this.sMensaje = sJSONPATHFile;
        //            }
        //            if (oResponse.StatusCode == HttpStatusCode.OK)
        //            {
        //                this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);


        //                LOGI_ResponseTMS_INFO oResponse = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje,
        //                 new JsonSerializerSettings
        //                 {
        //                     NullValueHandling = NullValueHandling.Ignore
        //                 });
        //                if (oResponse.Result[0].Encabezado == null)
        //                {
        //                    sCadenamensaje = oResponse.Result[0].Mensaje;
        //                    if (rchConsole != null)
        //                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                    else
        //                        oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //                }
        //                else
        //                {

        //                    oDocontrol.ListaDocumentos(CONST_USUARIO, ref lstDocumentos, oParamdoc);
        //                    sCadenamensaje = string.Format("Se han encontrado un total de {0} movimientos para el período seleccionado", oResponse.Result[0].Encabezado.Count);
        //                    if (rchConsole != null)
        //                        oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                    else
        //                        oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO, sEmpresa: "PASIVOS");


        //                    lstLotes = new List<BBICargarFacturaCxCContractEnc>();
        //                    lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado == "ProPasivos" && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30).ToList();
        //                    if (lstLotes.Count > 0)
        //                    {
        //                        sCadenamensaje = string.Format(@"Se encontraton un total de {0} procesos para polizas contables de tipo pasivos", lstLotes.Count);
        //                        if (rchConsole != null)
        //                            oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.success);
        //                        else
        //                            oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //                        this.CreaLotePagosD365(lstLotes, out sresponse);
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                sCadenamensaje = "No se ha podido leer la información proveniente de TMS";
        //                if (rchConsole != null)
        //                    oTools.m_ConsoleLine(rchConsole, sCadenamensaje, eType.error);
        //                else
        //                    oTools.LogProceso(sCadenamensaje, "DescargaMovimientoTMS", "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            sERROR = string.Format(@"El proceso de replica no se ha realizado. ERROR {0}", ex.Message);
        //            if (rchConsole != null)
        //                oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
        //            else
        //                oTools.LogError(ex, "DescargaMovimientoTMS", CONST_USUARIO, "LOGI_TMSConnection_D365.cs", "D365", CONST_USUARIO);
        //        }
        //        finally
        //        {
        //            GC.Collect();
        //            if (bLeeTXT)
        //            {
        //                try
        //                {
        //                    string sPathJSONtmp = string.Format(@"{0}\JSON\Procesados\{1}_JSON.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("ddMMyyyyhhssmm"));
        //                    File.Move(AppDomain.CurrentDomain.BaseDirectory + @"JSON\JSON.txt", sPathJSONtmp);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}



        #endregion METODOORIGINAL


    }

    public class clArticulos{

        public List<Items> Articulos { get; set; }
    }

    public class Items {

        public int Cantidad { get; set; }
        public Double CostoUnitario { get;set;}
        public string DescripcionParte { get; set; }
        public Double ImporteTotal { get; set; }

    }

    
}


