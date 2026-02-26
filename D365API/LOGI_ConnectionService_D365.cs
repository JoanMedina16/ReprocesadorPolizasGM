using D365API.Helpers;
using D365API.Helpers.D365FOBBICGCxPServices;
using D365API.Helpers.D365FOBBIContaServices;
using D365API.Helpers.D365FOBBICxCServices;
using D365API.INFOD;
using INFO.Enums;
using INFO.Objetos.SAT;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using INFO.Tablas.FUEL;
using Newtonsoft.Json;
using PD.Herramientas;
using PD.Tablas.D365;
using PD.Tablas.EQUIV;
using PD.Tablas.FUEL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D365API
{
    public class LOGI_ConnectionService_D365
    {
        public string sMensaje { get; set; }        
        public bool bConectado { get; set; }
        public LOGI_Session_D365 oSession = null;
        public LOGI_Error_D365 oError { get; set; }
        internal string CONST_OPE_CONNECTION = string.Empty; ///Aloja la cadena de conexion para el sistema (ADMLOG04)
        internal string CONST_EQUIV_CONNECTION = string.Empty; ///Aloja la cadena de conexión para los catálogos de equivalencia        
        internal int intentos { get; set; }
        internal LOGI_Credencial_D365 oCreds = null;        
        internal RestClient oCliente = null;
        internal RestRequest oRequest = null;
        internal IRestResponse oResponse = null;
        internal const string CONST_USUARIO = "620"; //PERFIL DE USUARIO QUE REPRESENTA USUARIO WINDOWS
        internal const int CONST_EMPRESA = 67; //REPRESENTA EL CODIGO DE LA EMPRESA CON LA CUAL SE ESTÁ TRABAJANDO (LOGISTICA DEL MAYAB)
        internal LOGI_Transacciones_PD oTransacCtrl = null;
        internal LOGI_Polizas_PD oPolizaCtrl = null;
        internal LOGI_Polizas_detalle_PD oDetalleCtrl = null;
        internal LOGI_Documentos_PD oDocontrol = null;
        internal LOGI_Tools_PD oTools = null;
        internal RichTextBox rchConsole = null; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oCredential"></param>
        public LOGI_ConnectionService_D365(LOGI_Credencial_D365 oCredential, string CONNECTION_OPE, string CONNECTION_EQUIV, RichTextBox rchConsole = null)
        {
            bConectado = false;
            this.oCreds = oCredential;
            CONST_OPE_CONNECTION = CONNECTION_OPE;
            CONST_EQUIV_CONNECTION = CONNECTION_EQUIV;
            oTransacCtrl = new LOGI_Transacciones_PD(CONST_OPE_CONNECTION);
            oPolizaCtrl = new LOGI_Polizas_PD(CONST_OPE_CONNECTION);
            oDetalleCtrl = new LOGI_Polizas_detalle_PD(CONST_OPE_CONNECTION);
            oDocontrol = new LOGI_Documentos_PD(CONST_OPE_CONNECTION);
            oTools = new LOGI_Tools_PD();
            this.rchConsole = rchConsole;
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
            }
            return bContinuar;
        }


        /// <summary>
        /// Descripción: Metodo encargado de procesar todos los documentos pendientes de enviar hacia D365.
        /// Según el tipo de documento a crear se invoca su referencia. Una vez el proceso es exitoso se almacena el registro recuepardo de 
        /// Dynamics 365. Si el proceso no se pudo realizar se envia a la bitacora de transacciones el log de error devuelto por D365.
        /// Autor: 
        /// Fecha: 28/04/2022
        /// </summary>
        /// <returns></returns>
        public void onExecuteAsientos(bool bSoloCombustible = false)
        {
            bool bCreado = false;
            int PROCESADOS = 0, APLICADOS = 0, FALLIDOS = 0;
            string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
                sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
            LOGI_Polizas_INFO oParam = new LOGI_Polizas_INFO();
            List<LOGI_Polizas_INFO> lstPolizas = new List<LOGI_Polizas_INFO>();
            LOGI_Documentos_INFO oParamdoc = new LOGI_Documentos_INFO();
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
            List<LOGI_Polizas_detalle_INFO> lstDetalles = null;
            LOGI_Polizas_detalle_INFO oDetalle = null;

            try
            {
                //Se recupera la información de los documentos que se pueden interfazar hacia Dynamics D365
                oDocontrol.ListaDocumentos(CONST_USUARIO, ref lstDocumentos, oParamdoc);
                if (lstDocumentos.Count > 0)
                {

                    oParam.estatus = 0;
                    oPolizaCtrl.ListaPolizas(CONST_USUARIO, oParam, ref lstPolizas, Top: 150, bAscendete: true);
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, string.Format("Se han encontrado un total de {0} póliza(s) a procesar", lstPolizas.Count), eType.proceso);

                    oTools.LogProceso(string.Format("Se han encontrado un total de {0} póliza(s) a procesar", lstPolizas.Count), "OnExecuteAsientos", "LOGI_ConnectionService_D365.cs","D365", CONST_USUARIO);

                    foreach (LOGI_Polizas_INFO o in lstPolizas)
                    {

                        if (o.eTypedoc == eDocumentoPolizas.COMBUSTIBLES_PROVEEDOR)
                        {
                            if (!bSoloCombustible)
                                continue;
                        }
                        if (bSoloCombustible)
                        {
                            if (!(o.eTypedoc == eDocumentoPolizas.COMBUSTIBLES_PROVEEDOR))
                                continue;

                        }
                        /*if (bSoloCombustible)
                        {

                            if (!(o.eTypedoc == eDocumentoPolizas.COMBUSTIBLES_PROVEEDOR))
                                continue;
                            oTools.LogProceso(string.Format("Se ejecuta solo el documento", o.FolioAsiento), "OnExecuteAsientos", "LOGI_ConnectionService_D365.cs", "D365", CONST_USUARIO);

                        }*/
                        oTools.LogProceso(string.Format("APLICANDO EL FOLIO {0}", o.FolioAsiento), "Estatus "+bSoloCombustible, "LOGI_ConnectionService_D365.cs", "D365", CONST_USUARIO);

                       // if (!(o.eTypedoc == eDocumentoPolizas.CANCELACION_DE_FACTURAS))
                        //   continue;


                        sresponse = string.Empty;
                        sUUID = string.Empty;
                        sUUIDRef = string.Empty;
                        PROCESADOS++;
                        try
                        {
                            if(rchConsole != null)
                                oTools.m_ConsoleLine(rchConsole, string.Format(string.Format(@"Buscando la información para el folio de documento ""{0}""", o.FolioAsiento), lstPolizas.Count), eType.proceso, false);
                            sURLEndPoint = string.Empty;
                            sERROR = string.Empty;
                            sJSON = string.Empty;
                            oParamdoc = new LOGI_Documentos_INFO();
                            oParamdoc = lstDocumentos.FirstOrDefault(x => x.numerodoc == o.id_tipo_doc);
                            if (oParamdoc != null)
                            {
                                if (oParamdoc.activo == 1)
                                {
                                    sEndPoint = oParamdoc.metodo;
                                    lstDetalles = new List<LOGI_Polizas_detalle_INFO>();
                                    oDetalle = new LOGI_Polizas_detalle_INFO();
                                    oDetalle.FolioAsiento = o.FolioAsiento;
                                    oDetalle.tipo_documento = o.id_tipo_doc;
                                    oDetalleCtrl.ListadetallePoliza(CONST_EQUIV_CONNECTION, CONST_USUARIO, oDetalle, ref lstDetalles);
                                    if (lstDetalles.Count > 0)
                                    {
                                        var oErrores = lstDetalles.FirstOrDefault(x => x.valido == 0);
                                        if (oErrores == null)
                                        {

                                            switch (o.eTypedoc)
                                            {
                                                case eDocumentoPolizas.COMBUSTIBLES_COSTO:
                                                case eDocumentoPolizas.CANCELACION_MANO_DE_OBRA:
                                                case eDocumentoPolizas.MANO_DE_OBRA:
                                                    if (rchConsole != null)
                                                        oTools.m_ConsoleLine(rchConsole, String.Format("Procesando el envío de {0}. Contabilidad general", oParamdoc.nombre), eType.warning, bColor: false);
                                                    bCreado = this.CreaEntradaCostocombus(out sresponse, o, lstDetalles, sEndPoint, out sURLEndPoint, ref sJSON, oParamdoc.diario, o.eTypedoc);
                                                    break;
                                                case INFO.Enums.eDocumentoPolizas.COMBUSTIBLES_PROVEEDOR:
                                                    if (rchConsole != null)
                                                        oTools.m_ConsoleLine(rchConsole, String.Format("Procesando el envío de {0}", oParamdoc.nombre), eType.warning, bColor: false);
                                                    bCreado = this.CreaEntradaProveedorcombus(out sresponse, o, lstDetalles, sEndPoint, out sURLEndPoint, ref sJSON, oParamdoc.diario);
                                                    break;
                                                case eDocumentoPolizas.FACTURACION_DE_PORTES:
                                                case eDocumentoPolizas.FACTURACION_VARIA:
                                                case eDocumentoPolizas.NOTAS_DE_CREDITO:
                                                case eDocumentoPolizas.REFACTURACION_DE_PORTES:
                                                case eDocumentoPolizas.REFACTURACION_VARIA:
                                                    if (rchConsole != null)
                                                        oTools.m_ConsoleLine(rchConsole, String.Format("Procesando el envío de {0}. Ingresos de facturación", oParamdoc.nombre), eType.warning, bColor: false);
                                                    bCreado = this.CreaEntradaPolizaCliente(out sresponse, o, lstDetalles, sEndPoint, out sURLEndPoint, out sUUID, out sUUIDRef, ref sJSON, oParamdoc.diario, o.eTypedoc);
                                                    break;

                                                case eDocumentoPolizas.CANCELACION_DE_FACTURAS:
                                                    if (rchConsole != null)
                                                        oTools.m_ConsoleLine(rchConsole, String.Format("Procesando el envío de {0}", oParamdoc.nombre), eType.warning, bColor: false);
                                                    bCreado = this.CreaCancelacionFactura(out sresponse, o, lstDetalles, sEndPoint, out sURLEndPoint, ref sJSON, oParamdoc.diario);
                                                    break;

                                                case eDocumentoPolizas.COMPROBACIÓN_DE_VIATICOS:
                                                   if (rchConsole != null)
                                                        oTools.m_ConsoleLine(rchConsole, String.Format("Procesando el envío de {0}", oParamdoc.nombre), eType.warning, bColor: false);
                                                    bCreado = this.CreaEntradaComprobanteViatico(out sresponse, o, lstDetalles, sEndPoint, out sURLEndPoint, ref sJSON, oParamdoc.diario);
                                                    break;

                                                default:
                                                    sERROR = string.Format("No se encuentra la clasificación para el tipo de documento");
                                                    if (rchConsole != null)
                                                        oTools.m_ConsoleLine(rchConsole, sERROR, eType.warning);
                                                    break;
                                            }
                                            oTools.LogProceso(sERROR, "Creado "+bCreado, "LOGI_ConnectionService_D365.cs", "D365", CONST_USUARIO);

                                            if (bCreado)
                                            {
                                                APLICADOS++;
                                                oParam = new LOGI_Polizas_INFO();
                                                oParam.FolioAsiento = o.FolioAsiento;
                                                oParam.estatus = 2;
                                                oParam.recIdD365 = GetFolioD365(this.sMensaje);
                                                oParam.eTypedoc = o.eTypedoc;
                                                oParam.uuid = sUUID;
                                                oParam.uuidref = sUUIDRef;
                                                if (rchConsole != null)
                                                    oTools.m_ConsoleLine(rchConsole, string.Format(@"El diario contable con folio ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", o.FolioAsiento, this.sMensaje), eType.success);
                                                oTools.LogProceso(string.Format(@"El diario contable con folio ""{0}"" se ha creado con éxito en D365. Se asignó el siguiente folio REC ID {1}", o.FolioAsiento, this.sMensaje), "Creado " + bCreado, "LOGI_ConnectionService_D365.cs", "D365", CONST_USUARIO);

                                                oPolizaCtrl.Actualizapoliza(CONST_USUARIO, oParam);
                                                sERROR = string.Empty;
                                            }
                                            else
                                            {
                                                oTools.LogProceso(sMensaje, "Creado " + bCreado, "LOGI_ConnectionService_D365.cs", "D365", CONST_USUARIO);

                                                if (!string.IsNullOrEmpty(this.sMensaje))
                                                    sERROR = sresponse;
                                                else sERROR = string.Empty;
                                                
                                                    if (rchConsole != null)
                                                        oTools.m_ConsoleLine(rchConsole, sresponse, eType.error);                                               
                                            }


                                        }
                                        else sERROR = string.Format("Se han detectado una o varias líneas que no tienen configuradas las equivalencias. ERROR {0}", oErrores.mensaje);

                                    }
                                    else sERROR = string.Format("No se ha encontrado información para el documento {0}", o.FolioAsiento);

                                }
                                else sERROR = string.Format(@"El tipo de documento ""{0}"" para el folio {0} se encuentra inactivo", oParamdoc.nombre, o.FolioAsiento);

                            }
                        }
                        catch (Exception ex)
                        {
                            sERROR = string.Format(@"El documento con folio ""{0}"" no se ha podido sincronizar. ERROR {1}", o.FolioAsiento, ex.Message);
                            if(rchConsole != null)
                           oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                        }
                        finally
                        {
                            oTools.LogProceso(sERROR,"", "LOGI_ConnectionService_D365.cs", "D365", CONST_USUARIO);

                            if (!string.IsNullOrEmpty(sERROR))
                            {
                                if (this.sMensaje.Contains("Internal Server"))
                                    intentos++;

                                if (intentos >= 2)
                                    bConectado = false;
                                
                                if (!sMensaje.Contains("Internal Server"))//Se ignora porque se desconectó de D365 debido a que el token venció
                                {
                                    FALLIDOS++;
                                    sJSON = sJSON.Replace("'", String.Empty);
                                    oPolizaCtrl.Creaerror(CONST_USUARIO, sERROR, o.FolioAsiento, sJSON, this.sMensaje, sURLEndPoint);
                                }
                            }
                        }

                    }

                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                sERROR = string.Format(@"El proceso de replica no se ha realizado. ERROR {0}", ex.Message);
                if (rchConsole != null)
                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
            }
            finally
            {
                sresponse = string.Format("Se han procesado un total de {0} documento(s). Creando correctamente {1}, se han detectado {2} fallo(s).",PROCESADOS, APLICADOS, FALLIDOS);
                if (PROCESADOS > 0)
                {
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, sresponse, eType.success);
                    else oTools.LogProceso(sresponse, "", "", "", CONST_USUARIO);
                }

            }
        }
        /// <summary>
        /// Descripción: Proceso que limpia la cadena retornada por D365 para la extracción del folio contable ejemplo: t|11000002
        /// Autor: 
        /// Fecha: 28/04/2022
        /// </summary>
        /// <param name="sCadena">Cadena que contiene el folio RECID devuelto por Dynamics365</param>
        /// <returns></returns>
        string GetFolioD365(String sCadena)
        {
            string sFolio = string.Empty;
            try
            {
                var splT = sCadena.Split('|');
                sFolio = splT[1].ToString();
            }
            catch { }
            return sFolio;
        }


    /// <summary>
    /// Descripción: Proceso encargado de interfazar las provisiones de combustibles para proveedores. La emisión de dicha provisión
    /// es avalada por una factura fiscal
    /// Autor: 
    /// Fecha: 28/04/2022
    /// </summary>
    /// <param name="response">Variable de salida que contiene el mensaje de excepcion o error generado durante la ejecución</param>
    /// <param name="oEncabezado">Contenido del diario proveniente desde OTM</param>
    /// <param name="lstDetalle">Detalle de las líneas contables</param>
    /// <param name="sEndPoint">Metodo hacia el cual se ejecutará la operación en Dynamics 365</param>
    /// <param name="sURLEndPoint">Cadena de salida que se forma con el valor del endpoint y API hacia donde se hace la petición</param>
    /// <param name="sJSON">Contenido JSON generado de los objetos que se envían a D365</param>
    /// <param name="sDiario">Nombre del diario a crear en D365</param>
    /// <returns></returns>
    public bool CreaEntradaProveedorcombus(out string response, LOGI_Polizas_INFO oEncabezado, List<LOGI_Polizas_detalle_INFO> lstDetalle,
            string sEndPoint, out string sURLEndPoint, ref string sJSON, string sDiario)
        {
            bool bContinuar = false;
            response = string.Empty;
            BBICargarFacturaCxPContract oContrato = new BBICargarFacturaCxPContract();
            oPolizaCtrl = new LOGI_Polizas_PD(CONST_OPE_CONNECTION);
            List<BBICargarFacturaCxPContractLin> lstListaContrato = null;
            BBICargarFacturaCxPContractLin oLinea = null;
            sJSON = string.Empty;
            sURLEndPoint = string.Empty;

            try
            {
                if (!this.bConectado)
                    onCreateLogin();

                var oProveedor = lstDetalle.FirstOrDefault(x => x.mayor == 3010);
                if (oProveedor == null)
                    throw new Exception("No se encuentra el proveedor dentro de las líneas del detalle");

                oContrato = new BBICargarFacturaCxPContract();
                oContrato.Encabezado = new BBICargarFacturaCxPContractEnc();
                oContrato.Encabezado.SistemaOrigen = D365API.Helpers.BBISistemaOrigen.OpeAdm;
                oContrato.Encabezado.IdRegistro = oEncabezado.FolioAsiento.ToString();
                oContrato.Encabezado.Descripcion = string.Format("Diario de factura para entrada {0}", oEncabezado.FolioAsiento);
                oContrato.Encabezado.NombreDiario = sDiario;
                oContrato.Encabezado.Compania = oCreds.ciad365;
                oContrato.Encabezado.ImpuestosInc = D365API.Helpers.NoYes.Yes;
                ///Fecha contable 
                oContrato.Encabezado.FechaFactura = DateTime.ParseExact(oEncabezado.fechaContable, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                oContrato.Encabezado.CodProveedor = oProveedor.cuenta_AX;

                LOGI_Polizas_INFO oXML = new LOGI_Polizas_INFO();
                if (oPolizaCtrl.RecuperaXML("", ref oXML, oEncabezado.FolioAsiento).Equals("OK", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(oXML.docxml))
                    {
                        LOGI_XMLS_INFO oCFDI = new LOGI_XMLS_INFO();
                        if (oTools.DevuelveXMLObject(out response, ref oCFDI, sContentXML: oXML.docxml))
                        {
                            oContrato.Encabezado.Serie = string.Empty; //oCFDI.Serie; //se omite el envio de la serie para no afectar a EDOC
                            oContrato.Encabezado.XMLFactura = oCFDI.CFDIContent;
                            oContrato.Encabezado.Folio = Convert.ToInt32(oCFDI.Folio);
                            oContrato.Encabezado.Factura = string.Format("{0}{1}", oCFDI.Serie, oCFDI.Folio);
                            oContrato.Encabezado.Docto = string.Format("{0}{1}", oCFDI.Serie, oCFDI.Folio);
                            oContrato.Encabezado.Texto = string.Format("Factura {0}{1}", oCFDI.Serie, oCFDI.Folio);
                            oContrato.Encabezado.FechaDocto = Convert.ToDateTime(oCFDI.Fecha);
                        }
                        else throw new Exception("El XML no se ha podido recuperar");
                    }
                }
                //SE REQUIERE CALCULAR LA FECHA DE VENCIMIENTO SEGÚN EL PLAZO DE CREDITO SOBRE EL PROVEEDOR
                oContrato.Encabezado.FechaVencimiento = this.FechaVencimientoSocioNegocio(oProveedor.cuenta, oProveedor.scuenta, oContrato.Encabezado.FechaDocto);



                oContrato.Encabezado.SinComprobante = D365API.Helpers.NoYes.No;
                oContrato.Encabezado.UUID = oEncabezado.uuid;
                oContrato.Encabezado.AprobadoPor = oCreds.aprobador;
                oContrato.Encabezado.TipoFact = D365API.Helpers.BBITipoFact.Factura;//.Cancelacion;
                oContrato.Encabezado.IdFactReferencia = 0;
                oContrato.Encabezado.DesctoPago = "";
                oContrato.Encabezado.Subtotal = oEncabezado.subtotal;
                oContrato.Encabezado.Total = oEncabezado.total;
                oContrato.Encabezado.Moneda = "MXN";
                oContrato.Encabezado.TipoCambio = 1.0M;
                oContrato.Encabezado.ImpuestosInc = D365API.Helpers.NoYes.Yes;
                lstListaContrato = new List<BBICargarFacturaCxPContractLin>();


                //RECUPERAMOS LA CUENTA CONTABLE CORRESPONDIENTE AL IVA (IMPUESTO APLICADO A PROVISIÓN)
                List<LOGI_Catalogos_INFO> lstCat = new List<LOGI_Catalogos_INFO>();
                LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                //LA CONVINACIÓN DE MAYOR CUENTA Y SUBCUENTA CORRESPONDE A IVA EN OPEADM
                oParam.iCuentamayor = 1540;
                oParam.iCuenta = 1540;
                oParam.iSubcuenta = 35;
                oParam.iEmpresa = CONST_EMPRESA;
                oParam.iArea = -1;
                oPolizaCtrl.RecuperaCatalogoImpuestos(this.CONST_EQUIV_CONNECTION, CONST_USUARIO, ref lstCat, oParam);
                if (lstCat.Count == 0)
                    throw new Exception("No se ha podido recuperar la información del impuesto excento");
                if (lstCat.Count > 1)
                    throw new Exception("Se han encontrado más de una cuenta configurada para IVA EXCENTO");

                List<LOGI_Catalogos_INFO> lstCatFrontera = new List<LOGI_Catalogos_INFO>();
                oParam = new LOGI_Catalogos_INFO();
                //LA CONVINACIÓN DE MAYOR CUENTA Y SUBCUENTA CORRESPONDE A IVA EN OPEADM
                oParam.iCuentamayor = 1540;
                oParam.iCuenta = 1540;
                oParam.iSubcuenta = 15405;
                oParam.iEmpresa = CONST_EMPRESA;
                oParam.iArea = -1;
                oPolizaCtrl.RecuperaCatalogoImpuestos(this.CONST_EQUIV_CONNECTION, CONST_USUARIO, ref lstCatFrontera, oParam);
                if (lstCatFrontera.Count == 0)
                    throw new Exception("No se ha podido recuperar la información del impuesto excento");
                if (lstCatFrontera.Count > 1)
                    throw new Exception("Se han encontrado más de una cuenta configurada para IVA FRONTERA");


                foreach (LOGI_Polizas_detalle_INFO d in lstDetalle)
                {
                    if (d.mayor == 3010)
                        continue;
                    var oImpuestos = lstDetalle.FirstOrDefault(x => x.descrip.Contains("Parte excenta"));

                    oLinea = new BBICargarFacturaCxPContractLin();
                    oLinea.CuentaContable = d.descrip.Contains("Parte excenta") ? lstDetalle.FirstOrDefault(x => x.mayor == 7016).cuenta_AX : d.cuenta_AX;
                    oLinea.Texto = d.descrip;
                    oLinea.Debito = d.cargo;
                    oLinea.DimArea = d.area_D365 == "NO EXISTE" ? string.Empty : d.area_D365;
                    oLinea.DimCeco = d.centrocosto_D365 == "NO EXISTE" ? string.Empty : d.centrocosto_D365;
                    oLinea.DimSucursal = d.sucursal_D365 == "NO EXISTE" ? string.Empty : d.sucursal_D365;
                    oLinea.DimDepto = d.departamento_D365 == "NO EXISTE" ? string.Empty : d.departamento_D365;
                    if (d.descrip.Contains("Parte excenta"))
                    {
                        oLinea.CodImpuesto = oImpuestos.cuenta_AX;
                        oLinea.GrupoImpuestosArt =  oImpuestos.ImpuestoArt;
                        oLinea.GrupoImpuestos = d.IVA16 == 1 ? oImpuestos.GrupoImpuesto : lstCatFrontera[0].sAxgrupoventa;
                    }
                    else if (!d.descrip.Contains("Parte excenta"))
                    {
                        if (d.IVA16 == 1)
                        {
                            oLinea.CodImpuesto = lstCat[0].sAX365;
                            oLinea.GrupoImpuestosArt = lstCat[0].sAxgrupoarticulo ;
                            oLinea.GrupoImpuestos = lstCat[0].sAxgrupoventa;
                        }
                        else
                        {
                            oLinea.CodImpuesto = lstCatFrontera[0].sAX365;
                            oLinea.GrupoImpuestosArt = lstCatFrontera[0].sAxgrupoarticulo;  
                            oLinea.GrupoImpuestos = lstCatFrontera[0].sAxgrupoventa; 
                        }
                    }
                    if (!string.IsNullOrEmpty(d.vehiculo))
                        oLinea.Vehiculo = d.vehiculo.Split('|')[1].ToString();
                    oLinea.DimFilTer = d.filialtercero_D365;
                    lstListaContrato.Add(oLinea);
                }

                oContrato.Lineas = lstListaContrato.ToArray();

                var total_debito = oContrato.Lineas.Sum(x=>x.Debito);
                var total_credito = oContrato.Lineas.Sum(x => x.Credito);

                var total_debito_excenta = oContrato.Lineas.Where(x=> x.Texto.Contains("Parte excenta")).Sum(x => x.Debito);
                var total_credito_excenta = oContrato.Lineas.Where(x=> !x.Texto.Contains("Parte excenta")).Sum(x => x.Debito);


                sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                
                sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, sEndPoint);
                System.ServiceModel.Channels.Binding oBindig;
                EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);

                oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                //oBindig.SendTimeout = new TimeSpan(1000000);
                //oBindig.ReceiveTimeout = TimeSpan.MaxValue;
                //oBindig.SendTimeout = TimeSpan.MaxValue;
                oBindig.CloseTimeout = System.TimeSpan.Parse("00:10:00");
                oBindig.OpenTimeout = System.TimeSpan.Parse("00:10:00");
                oBindig.ReceiveTimeout = System.TimeSpan.Parse("00:30:00");
                oBindig.SendTimeout = System.TimeSpan.Parse("00:10:00");
                //oBindig.

                BBICargarFacturaCxPServiceClient oClienteD365 = new BBICargarFacturaCxPServiceClient(
                binding: oBindig, endpointAddress);
                Helpers.CallContext oContext = new Helpers.CallContext();
                oContext.Company = oContrato.Encabezado.Compania;

                using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                    Helpers.Infolog oInfoLog = oClienteD365.cargarFactura(oContext, oContrato, out response);
                    bContinuar = this.RecuperaMensaje(ref response);
                }

            }
            catch (Exception ex)
            {
                response = ex.Message;
                this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                if (this.sMensaje.Contains("Internal Server"))
                    intentos++;
            }
            finally
            {
                if (intentos >= 2)
                    bConectado = false;

            }
            return bContinuar;
        }


        /// <summary>
        /// Descripción: Proceso encargado de registrar los comprobantes de viaticos emitidos desde APEX. Los registros de gastos se 
        /// contabilizan con una cuenta de tipo gastos configurada en catalogo "cat_cuentas" de equivalencias. La contrapartida que 
        /// corresponde al operador se contabiliza con una cuenta fija configurada en ambiente lm_config_d365.
        /// Autor: 
        /// Fecha: 28/04/2022
        /// </summary>
        /// <param name="response">Variable de salida que contiene el mensaje de excepcion o error generado durante la ejecución</param>
        /// <param name="oEncabezado">Contenido del diario proveniente desde OTM</param>
        /// <param name="lstDetalle">Detalle de las líneas contables</param>
        /// <param name="sEndPoint">Metodo hacia el cual se ejecutará la operación en Dynamics 365</param>
        /// <param name="sURLEndPoint">Cadena de salida que se forma con el valor del endpoint y API hacia donde se hace la petición</param>
        /// <param name="sJSON">Contenido JSON generado de los objetos que se envían a D365</param>
        /// <param name="sDiario">Nombre del diario a crear en D365</param>
        /// <returns></returns>
        public bool CreaEntradaComprobanteViatico(out string response, LOGI_Polizas_INFO oEncabezado, List<LOGI_Polizas_detalle_INFO> lstDetalle,
                string sEndPoint, out string sURLEndPoint, ref string sJSON, string sDiario)
        {
            bool bContinuar = false;
            response = string.Empty;
            oPolizaCtrl = new LOGI_Polizas_PD(CONST_OPE_CONNECTION);
            List<BBICargarFacturaCxPContractLin> lstListaContrato = null;
            BBICargarFacturaCxPContractLin oLinea = null;
            sJSON = string.Empty;
            sURLEndPoint = string.Empty;

            try
            {
                if (!this.bConectado)
                    onCreateLogin();


                Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContract oContrato = new Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContract();
                Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractFac factura;
                List<Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractFac> listaFac;
                Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractLin linea;
                List<Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractLin> lista;

                oContrato.Encabezado = new Helpers.D365FOBBICGCxPServices.BBICargarFacturaCGCxPContractEnc();
                oContrato.Encabezado.SistemaOrigen = Helpers.D365FOBBICGCxPServices.BBISistemaOrigen.OpeAdm;
                oContrato.Encabezado.IdRegistro = oEncabezado.FolioAsiento;
                oContrato.Encabezado.Compania = oCreds.ciad365;
                oContrato.Encabezado.Descripcion = string.Format("Gastos para viaje {0}", oEncabezado.doctoref);
                oContrato.Encabezado.NombreDiario = sDiario;
                oContrato.Encabezado.Texto = string.Format("Gastos para viaje {0}", oEncabezado.doctoref);
                oContrato.Encabezado.ImpuestosInc = Helpers.D365FOBBICGCxPServices.NoYes.Yes;
                oContrato.Encabezado.FechaFactura = DateTime.ParseExact(oEncabezado.fechaContable, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                oContrato.Encabezado.TipoCuenta = BBITipoCuentaLineaDiario.Contabilidad;
                oContrato.Encabezado.CuentaContable = oCreds.cuentaviaticos;
                oContrato.Encabezado.Factura = oEncabezado.FolioAsiento;
                oContrato.Encabezado.Docto = oEncabezado.FolioAsiento;
                oContrato.Encabezado.AprobadoPor = oCreds.aprobador;
                oContrato.Encabezado.TipoFact = Helpers.D365FOBBICGCxPServices.BBITipoFact.Factura;


                Decimal Total = lstDetalle.Where(x => x.cuenta == 25).ToList().Sum(x => x.abono);
                Decimal IVA = lstDetalle.Where(x => x.mayor == 1540).ToList().Sum(x => x.cargo);

                oContrato.Encabezado.Subtotal = Total - IVA;
                oContrato.Encabezado.Total = Total;
                oContrato.Encabezado.Moneda = "MXN";
                oContrato.Encabezado.TipoCambio = 1.0M;
                var oFiltro = lstDetalle.FirstOrDefault(x => x.cuenta != 25 && x.mayor != 1540);
                if (oFiltro != null)
                {
                    oContrato.Encabezado.DimFilTer = oFiltro.filialtercero_D365;
                    oContrato.Encabezado.DimSucursal = oFiltro.sucursal_D365;
                }

                List<LOGI_Catalogos_INFO> lstCat = new List<LOGI_Catalogos_INFO>();
                List<LOGI_Catalogos_INFO> lstFiliales = new List<LOGI_Catalogos_INFO>();
                LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                LOGI_Catalogos_INFO oNOIVA = new LOGI_Catalogos_INFO();
                LOGI_Catalogos_INFO oNODeducible = new LOGI_Catalogos_INFO();
                //LA CONVINACIÓN DE MAYOR CUENTA Y SUBCUENTA CORRESPONDE A IVA EN OPEADM
                oParam.iCuenta = 1540;
                oParam.iSubcuenta = 15402;
                oParam.iEmpresa = CONST_EMPRESA;
                oParam.iArea = -1;
                oPolizaCtrl.RecuperaCatalogoImpuestos(this.CONST_EQUIV_CONNECTION, CONST_USUARIO, ref lstCat, oParam);
                if (lstCat.Count == 0)
                    throw new Exception("No se ha podido recuperar la información del impuesto excento");
                if (lstCat.Count > 1)
                    throw new Exception("Se han encontrado más de una cuenta configurada para impuestos");
                oNOIVA = lstCat[0];

                lstCat = new List<LOGI_Catalogos_INFO>();
                oParam = new LOGI_Catalogos_INFO();
                oParam.iCuenta = 1540;
                oParam.iSubcuenta = 15404;
                oParam.iEmpresa = CONST_EMPRESA;
                oParam.iArea = -1;
                oPolizaCtrl.RecuperaCatalogoImpuestos(this.CONST_EQUIV_CONNECTION, CONST_USUARIO, ref lstCat, oParam);
                if (lstCat.Count == 0)
                    throw new Exception("No se ha podido recuperar la información del impuesto excento");
                if (lstCat.Count > 1)
                    throw new Exception("Se han encontrado más de una cuenta configurada para impuestos");
                oNODeducible = lstCat[0];


                oParam = new LOGI_Catalogos_INFO();
                oParam.iCuenta = 624;
                oParam.iSubcuenta = 0;
                oParam.iEmpresa = CONST_EMPRESA;
                oParam.iArea = -1;
                oPolizaCtrl.RecuperaCatalogoFiliales(this.CONST_EQUIV_CONNECTION, CONST_USUARIO, ref lstFiliales, oParam);


                listaFac = new List<BBICargarFacturaCGCxPContractFac>();
                //recorremos el detalle para detectar cuales líneas tienen XML y excluimos la cuenta #25 que corresponde al operador
                int conteo = 0;
                foreach (LOGI_Polizas_detalle_INFO d in lstDetalle)
                {
                    if (d.cuenta == 25)
                        continue;
                    if (d.mayor == 1540) //ignoramos los ivas
                        continue;

                    //POR CADA LÍNEA AGREGAMOS EL REGISTRO CONTABLE
                    factura = new BBICargarFacturaCGCxPContractFac();
                    lista = new List<BBICargarFacturaCGCxPContractLin>();
                    linea = new BBICargarFacturaCGCxPContractLin();

                    //todas las líneas por default tienen la fecha del documento, si tienen comprobante heredan el valor de la fecha emitida del XML
                    factura.FechaDocto = DateTime.ParseExact(oEncabezado.fechaContable, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    factura.SinComprobante = Helpers.D365FOBBICGCxPServices.NoYes.Yes;
                    factura.Texto = String.Format("{0} - {1}", oEncabezado.doctoref, d.descrip);
                    factura.Texto = factura.Texto.Length > 60 ? factura.Texto.Substring(0, 59) : factura.Texto;
                    factura.TipoOperacion = VendorOperationType_MX.Blank;
                    factura.TipoProveedor = VendorType_MX.Blank;

                    if (!string.IsNullOrEmpty(d.vehiculo))
                        factura.Vehiculo = d.vehiculo.Split('|')[1].ToString();

                    factura.Docto = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmss"), conteo);
                    factura.Factura = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmss"), conteo);


                    if (!string.IsNullOrEmpty(d.XML))
                    {
                        LOGI_XMLS_INFO oCFDI = new LOGI_XMLS_INFO();
                        if (oTools.DevuelveXMLObject(out response, ref oCFDI, sContentXML: d.XML))
                        {
                            factura.TipoOperacion = VendorOperationType_MX.Other;
                            factura.TipoProveedor = VendorType_MX.DomesticVendor;
                            factura.SinComprobante = Helpers.D365FOBBICGCxPServices.NoYes.No;
                            if (!string.IsNullOrEmpty(oCFDI.Serie) && !string.IsNullOrEmpty(oCFDI.Folio))
                            {
                                factura.Factura = string.Format("{0}{1}", oCFDI.Serie, oCFDI.Folio);
                                factura.Docto = string.Format("{0}{1}", oCFDI.Serie, oCFDI.Folio);
                            }
                            else
                            {
                                factura.Factura = oCFDI.Complemento.TimbreFiscalDigital.UUID.Split('-')[0];
                                factura.Docto = oCFDI.Complemento.TimbreFiscalDigital.UUID.Split('-')[0];
                            }
                            factura.UUID = oCFDI.Complemento.TimbreFiscalDigital.UUID;
                            factura.XMLFactura = oCFDI.CFDIContent;
                            factura.RFC = oCFDI.Emisor.Rfc;
                            factura.NombreProveedor = oCFDI.Emisor.Nombre;
                            factura.FechaDocto = Convert.ToDateTime(oCFDI.Fecha);
                        }
                    }

                    linea.CuentaContable = d.cuenta_AX;
                    linea.Texto = String.Format("{0} - {1}", oEncabezado.doctoref, d.descrip);
                    linea.Texto = linea.Texto.Length > 60 ? linea.Texto.Substring(0, 59) : linea.Texto;
                    linea.Debito = d.cargo;
                    linea.Credito = d.abono;
                    linea.DimArea = d.area_D365 == "NO EXISTE" ? string.Empty : d.area_D365;
                    linea.DimCeco = d.centrocosto_D365 == "NO EXISTE" ? string.Empty : d.centrocosto_D365;
                    linea.DimSucursal = d.sucursal_D365 == "NO EXISTE" ? string.Empty : d.sucursal_D365;
                    linea.DimDepto = d.departamento_D365 == "NO EXISTE" ? string.Empty : d.departamento_D365;
                    linea.DimFilTer = d.filialtercero_D365;
                    if (!string.IsNullOrEmpty(d.vehiculo))
                        linea.Vehiculo = d.vehiculo.Split('|')[1].ToString();


                    var oImpuestoIVA = lstDetalle.FirstOrDefault(x => x.mayor == 1540 && x.refdoc.Equals(d.refdoc, StringComparison.InvariantCultureIgnoreCase));
                    if (oImpuestoIVA != null)
                    {
                        linea.Debito = linea.Debito + oImpuestoIVA.cargo;
                        linea.Credito = linea.Credito + oImpuestoIVA.abono;
                        linea.CodImpuesto = oImpuestoIVA.cuenta_AX;
                        linea.GrupoImpuestosArt = oImpuestoIVA.ImpuestoArt;
                        linea.GrupoImpuestos = oImpuestoIVA.GrupoImpuesto;
                    }
                    else if (d.esdeducible == 0 || (d.esdeducible == 1 && string.IsNullOrEmpty(d.XML)))
                    {
                        linea.CodImpuesto = oNODeducible.sAX365;
                        linea.GrupoImpuestosArt = oNODeducible.sAxgrupoarticulo;
                        linea.GrupoImpuestos = oNODeducible.sAxgrupoventa;
                        if (lstFiliales.Count == 0)
                            throw new Exception("No se ha determinado el filial no deducible");
                        linea.DimFilTer = lstFiliales[0].sAX365;
                    }else 
                    {

                        linea.CodImpuesto = oNOIVA.sAX365;
                        linea.GrupoImpuestosArt = oNOIVA.sAxgrupoarticulo;
                        linea.GrupoImpuestos = oNOIVA.sAxgrupoventa;
                    }
                    //}
                    lista.Add(linea);
                    factura.Lineas = lista.ToArray();
                    listaFac.Add(factura);
                    conteo++;
                }

                oContrato.Facturas = listaFac.ToArray();
                sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, sEndPoint);

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
                    Helpers.D365FOBBICGCxPServices.Infolog oInfoLog = oClienteD365.cargarFactura(oContext, oContrato, out response);
                    bContinuar = this.RecuperaMensaje(ref response);
                }

            }
            catch (Exception ex)
            {
                response = ex.Message;
                this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                if (this.sMensaje.Contains("Internal Server"))
                    intentos++;
            }
            finally
            {
                if (intentos >= 2)
                    bConectado = false;

            }
            return bContinuar;
        }
        /// <summary>
        /// Descripción: Entrada de combustible proveniente de motor de combustibles, esta provision está avalada por los 
        /// tickets de cargas de combustibles generados en el portal previamente mencionado. Este movimiento no tiene un avaluo fiscal.
        /// Autor: 
        /// Fecha: 28/04/2022
        /// </summary>
        /// <param name="response">Variable de salida que contiene el mensaje de excepcion o error generado durante la ejecución</param>
        /// <param name="oEncabezado">Contenido del diario proveniente desde OTM</param>
        /// <param name="lstDetalle">Detalle de las líneas contables</param>
        /// <param name="sEndPoint">Metodo hacia el cual se ejecutará la operación en Dynamics 365</param>
        /// <param name="sURLEndPoint">Cadena de salida que se forma con el valor del endpoint y API hacia donde se hace la petición</param>
        /// <param name="sJSON">Contenido JSON generado de los objetos que se envían a D365</param>
        /// <param name="sDiario">Nombre del diario a crear en D365</param>        
        /// <returns></returns>
        public bool CreaEntradaCostocombus(out string response, LOGI_Polizas_INFO oEncabezado, List<LOGI_Polizas_detalle_INFO> lstDetalle,
            string sEndPoint, out string sURLEndPoint, ref string sJSON, string sDiario, eDocumentoPolizas EdocType)
        {
            bool bContinuar = false;
            response = string.Empty;
            BBICargarPolizaContContract oContrato = new BBICargarPolizaContContract();
            List<BBICargarPolizaContContractLin> lstListaContrato = null;
            BBICargarPolizaContContractLin oLinea = null;
            sJSON = string.Empty;
            sURLEndPoint = string.Empty;

            try
            {
                if (!this.bConectado)
                    onCreateLogin();

                oContrato = new BBICargarPolizaContContract();
                oContrato.Encabezado = new BBICargarPolizaContContractEnc();
                oContrato.Encabezado.SistemaOrigen = D365API.Helpers.D365FOBBIContaServices.BBISistemaOrigen.OpeAdm;
                oContrato.Encabezado.IdRegistro = oEncabezado.FolioAsiento;
                if(EdocType == eDocumentoPolizas.COMBUSTIBLES_COSTO)
                oContrato.Encabezado.Descripcion = string.Format("Combustible costo para entrada {0}", oEncabezado.FolioAsiento);
                else
                    oContrato.Encabezado.Descripcion = string.Format("Mano de obra para entrada {0}", oEncabezado.FolioAsiento);
                oContrato.Encabezado.NombreDiario = sDiario;
                oContrato.Encabezado.Compania = oCreds.ciad365;
                oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBIContaServices.NoYes.No;
                oContrato.Encabezado.FechaTrans = DateTime.ParseExact(oEncabezado.fechaContable, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                oContrato.Encabezado.Moneda = "MXN";
                oContrato.Encabezado.TipoCambio = 1.0M;


                lstListaContrato = new List<BBICargarPolizaContContractLin>();

               
                foreach (LOGI_Polizas_detalle_INFO d in lstDetalle)
                {
                    oLinea = new BBICargarPolizaContContractLin();
                    oLinea.Texto = d.descrip;
                    oLinea.CuentaContable = d.cuenta_AX;
                    oLinea.Texto = d.descrip;
                    oLinea.Debito = d.cargo;
                    oLinea.Credito = d.abono;
                    oLinea.DimArea = d.area_D365 == "NO EXISTE" ? string.Empty : d.area_D365;
                    oLinea.DimCeco = d.centrocosto_D365 == "NO EXISTE" ? string.Empty : d.centrocosto_D365;
                    oLinea.DimSucursal = d.sucursal_D365 == "NO EXISTE" ? string.Empty : d.sucursal_D365;
                    oLinea.DimDepto = d.departamento_D365 == "NO EXISTE" ? string.Empty : d.departamento_D365;
                    oLinea.DimFilTer = d.filialtercero_D365;
                    if (!string.IsNullOrEmpty(d.vehiculo))
                        oLinea.Vehiculo = d.vehiculo.Split('|')[1].ToString();
                    lstListaContrato.Add(oLinea);
                }

                if (EdocType == eDocumentoPolizas.COMBUSTIBLES_COSTO)
                {
                    //Validamos que los importes cuadren cargo vs abono 
                    Decimal dTotalCargo = lstDetalle.Sum(x => x.cargo);
                    Decimal dTotalAbono = lstDetalle.Sum(x => x.abono);
                    Decimal dTotaldiferencia = Math.Abs(dTotalCargo - dTotalAbono);
                    if (dTotaldiferencia > 0)
                    {
                        if (dTotaldiferencia >= 1)
                            throw new Exception("Se ha detectado una diferencia de más de un peso");
                        else if (dTotaldiferencia < 1)
                        {
                            oTools.LogProceso(string.Format("Se ha detectado una diferencia en la poliza {0} vs {1}", dTotalCargo, dTotalAbono), "", "", "", CONST_USUARIO);
                            lstListaContrato.Where(x => x.Credito > 0).ToList().ForEach(j => j.Credito = dTotalCargo);
                        }
                    }
                }

                oContrato.Lineas = lstListaContrato.ToArray();


                sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, sEndPoint);
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
                    bContinuar = this.RecuperaMensaje(ref response);
                }

            }
            catch (Exception ex)
            {
                response = ex.Message;
                this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                if (this.sMensaje.Contains("Internal Server"))
                    intentos++;
            }
            finally
            {
                if (intentos >= 2)
                    bConectado = false;

            }

            return bContinuar; 
        }

        /// <summary>
        /// Descripción: Metodo utilizado para crear movimientos contables de cuentas por pagar. Este método procesa los documentos del motor de facturación
        /// Facturación varia (Facturas sin complemento carta porte). Facturación de porte con complemento de carta porte y Notas de crédito, estas son relacionadas 
        /// a una factura para su descuento. Antes del envío de información se debe considerar que el documento fiscal de cualquier tipo de movimiento exista en tabla lm_historicos_xml.
        /// Cuando es una nota de crédito los importes pasan a descontar el importe hacía el cliente. 
        /// Autor: 
        /// Fecha: 28/04/2022
        /// </summary>
        /// <param name="response">Variable de salida que contiene el mensaje de excepcion o error generado durante la ejecución</param>
        /// <param name="oEncabezado">Contenido del diario proveniente desde OTM</param>
        /// <param name="lstDetalle">Detalle de las líneas contables</param>
        /// <param name="sEndPoint">Metodo hacia el cual se ejecutará la operación en Dynamics 365</param>
        /// <param name="sURLEndPoint">Cadena de salida que se forma con el valor del endpoint y API hacia donde se hace la petición</param>
        /// <param name="sUUUD">Parametro de salida utilizado para retornar el UUID fiscal debido a que en tiempo de creación no se cuenta con este dato</param>
        /// <param name="sJSON">Contenido JSON generado de los objetos que se envían a D365</param>
        /// <param name="sDiario">Nombre del diario a crear en D365</param>        
        /// <param name="EdocType">Enumerador para determinar el tipo de documento a crear</param>
        /// <returns></returns>
        bool CreaEntradaPolizaCliente(out string response, LOGI_Polizas_INFO oEncabezado, List<LOGI_Polizas_detalle_INFO> lstDetalle,
    string sEndPoint, out string sURLEndPoint, out string sUUID, out string sUUUIDRef, ref string sJSON, string sDiario, eDocumentoPolizas EdocType)
        {
            bool bContinuar = false;
            response = string.Empty;
            oPolizaCtrl = new LOGI_Polizas_PD(CONST_OPE_CONNECTION);
            BBICargarFacturaCxCContract oContrato = new BBICargarFacturaCxCContract();
            List<BBICargarFacturaCxCContractLin> lstListaContrato = null;
            BBICargarFacturaCxCContractLin oLinea = null;
            sJSON = string.Empty;
            sURLEndPoint = string.Empty;
            sUUID = string.Empty;
            sUUUIDRef = string.Empty;

            try
            {
                if (!this.bConectado)
                    onCreateLogin();

                var oCliente = lstDetalle.FirstOrDefault(x => x.mayor == 1300);
                if (oCliente == null)
                    throw new Exception("No se encuentra el cliente dentro de las líneas del detalle");


                oContrato = new BBICargarFacturaCxCContract();
                oContrato.Encabezado = new BBICargarFacturaCxCContractEnc();
                oContrato.Encabezado.SistemaOrigen = D365API.Helpers.D365FOBBICxCServices.BBISistemaOrigen.OpeAdm;
                oContrato.Encabezado.IdRegistro = oEncabezado.FolioAsiento;


                oContrato.Encabezado.NombreDiario = sDiario;
                oContrato.Encabezado.Compania = oCreds.ciad365;

                List<LOGI_Factura_INFO> lstFacturas = new List<LOGI_Factura_INFO>();
                LOGI_Factura_INFO oFactura = new LOGI_Factura_INFO();
                oFactura.Folio = Convert.ToInt32(oEncabezado.folio);
                oFactura.serie = oEncabezado.serie;
                response = oPolizaCtrl.ListaDatoAdicionalFactura(CONST_USUARIO, ref lstFacturas, oFactura);
                oContrato.Encabezado.Orden = "NA";
                oContrato.Encabezado.Referencia = "NA";

                ///CORRESPONDE AL NUMERO 1O DEL REGISTRO lm_facadis
                var Orden = lstFacturas.FirstOrDefault(x => x.subcuentaadi == 36 || x.subcuentaadi == 30 || x.subcuentaadi == 10 || x.subcuentaadi == 13);
                if (Orden != null)
                    oContrato.Encabezado.Orden = string.IsNullOrEmpty(Orden.Valor) ? "NA" : Orden.Valor;

                //CORRESPONDE EL NUMERO 33 DEL REGISTRO lm_facadis
                var oRef = lstFacturas.FirstOrDefault(x => x.subcuentaadi == 33 || x.subcuentaadi == 22);
                if (oRef != null)
                    oContrato.Encabezado.Referencia = string.IsNullOrEmpty(oRef.Valor) ? "NA" : oRef.Valor;

                oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                oContrato.Encabezado.FechaFactura = DateTime.ParseExact(oEncabezado.fechaContable, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                oContrato.Encabezado.CodCliente = oCliente.cuenta_AX;



                oContrato.Encabezado.TipoFact = D365API.Helpers.D365FOBBICxCServices.BBITipoFact.Factura;
                if (EdocType == eDocumentoPolizas.NOTAS_DE_CREDITO)
                    oContrato.Encabezado.TipoFact = D365API.Helpers.D365FOBBICxCServices.BBITipoFact.NotaCred;

                oContrato.Encabezado.DesctoPago = "";
                oContrato.Encabezado.Subtotal = oEncabezado.subtotal;
                oContrato.Encabezado.Total = oEncabezado.total;
                oContrato.Encabezado.Moneda = "MXN";
                oContrato.Encabezado.TipoCambio = 1.0M;
                oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                oContrato.Encabezado.TipoRelacionUUID = EInvoiceCFDIReferenceType_MX.Invoice;

                


                //RECUPERAMOS EL DOCUMENTO XML TIMBRADO POR EL PROCESO DE MASTER EDI
                #region "Búsqueda de XML timbrado en la bandeja de facturación de MasterEDI"
                lstFacturas = new List<LOGI_Factura_INFO>();
                oFactura = new LOGI_Factura_INFO();
                oFactura.Folio = Convert.ToInt32(oEncabezado.folio.Trim());
                oFactura.serie = oEncabezado.serie;
                response = oPolizaCtrl.RecuperaMasterEDIXML(CONST_USUARIO, ref lstFacturas, oFactura);
                if (response != "OK")
                {
                    //NO GENERAMOS NINGÚN MENSAJE PARA QUE EL REGISTRO DE POLIZA NO SEA MARADO COMO UN ERROR YA QUE EL 
                    //DOCUMENTO AÚN NO SE TIMBRA A TRAVÉS DEL MOTOR DE MASTER EDI
                    response = "No se ha encontrado el documento fiscal en el directorio de Master Edi";
                    this.sMensaje = string.Empty;
                    return false;
                }
                #endregion "Búsqueda de XML timbrado en la bandeja de facturación de MasterEDI"
                oFactura = new LOGI_Factura_INFO();
                oFactura = lstFacturas[0];
                oContrato.Encabezado.UUID = oFactura.sUUID;

                if (!string.IsNullOrEmpty(oFactura.sXML))
                {
                    LOGI_XMLS_INFO oCFDI = new LOGI_XMLS_INFO();
                    if (oTools.DevuelveXMLObject(out response, ref oCFDI, sContentXML: oFactura.sXML))
                    {
                        sUUID = oCFDI.Complemento.TimbreFiscalDigital.UUID;
                        oContrato.Encabezado.Serie = oCFDI.Serie;
                        oContrato.Encabezado.XMLFactura = oCFDI.CFDIContent;
                        oContrato.Encabezado.Folio = Convert.ToInt32(oCFDI.Folio);
                        oContrato.Encabezado.Factura = string.Format("{0}{1}", oCFDI.Serie, oCFDI.Folio);
                        oContrato.Encabezado.Docto = string.Format("{0}{1}", oCFDI.Serie, oCFDI.Folio);
                        oContrato.Encabezado.Texto = string.Format("{0} {1}{2}", EdocType == eDocumentoPolizas.NOTAS_DE_CREDITO ? "Nota de crédito" : "Factura", oCFDI.Serie, oCFDI.Folio);
                        oContrato.Encabezado.UsoCFDI = oCFDI.Receptor.UsoCFDI;
                        oContrato.Encabezado.MetodoPagoSAT = oCFDI.MetodoPago;
                        oContrato.Encabezado.FechaDocto = Convert.ToDateTime(oCFDI.Fecha);

                        switch (EdocType)
                        {
                            case eDocumentoPolizas.FACTURACION_DE_PORTES:
                                oContrato.Encabezado.Descripcion = string.Format("CXC FACTURA {0}{1}-{2}-{3}", oCFDI.Serie, oCFDI.Folio, oEncabezado.FolioAsiento, oEncabezado.nombrerfc);
                                break;
                            case eDocumentoPolizas.REFACTURACION_DE_PORTES:
                                oContrato.Encabezado.Descripcion = string.Format("CXC FACTURA {0}{1}-{2}-{3}-{4}", oCFDI.Serie, oCFDI.Folio, oEncabezado.doctoref, oEncabezado.FolioAsiento, oEncabezado.nombrerfc);
                                break;

                            case eDocumentoPolizas.NOTAS_DE_CREDITO:
                                oContrato.Encabezado.Descripcion = string.Format("CXC NOTA C. {0}{1}-{2}", oCFDI.Serie, oCFDI.Folio, oEncabezado.nombrerfc);
                                break;
                            case eDocumentoPolizas.REFACTURACION_VARIA:
                                oContrato.Encabezado.Descripcion = string.Format("CXC FACTURA {0}{1}-{2}-{3}", oCFDI.Serie, oCFDI.Folio, oEncabezado.doctoref, oEncabezado.nombrerfc);
                                break;
                            default:
                                oContrato.Encabezado.Descripcion = string.Format("CXC FACTURA {0}{1}-{2}", oCFDI.Serie, oCFDI.Folio, oEncabezado.nombrerfc);
                                break;
                        }

                    }
                    else throw new Exception("El XML no se ha podido recuperar");
                }
                else throw new Exception("El XML no se ha podido recuperar");

                oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.Length > 60 ? oContrato.Encabezado.Descripcion.Substring(0, 59) : oContrato.Encabezado.Descripcion;
                oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.ToUpper();

                //SE REQUIERE CALCULAR LA FECHA DE VENCIMIENTO SEGÚN EL PLAZO DE CREDITO SOBRE EL CLIENTE
                oContrato.Encabezado.FechaVencimiento = this.FechaVencimientoSocioNegocio(oCliente.cuenta, oCliente.scuenta, oContrato.Encabezado.FechaDocto);


                ///CUANDO EL DOCUMENTO CORRESPONDE A UNA NOTA DE CRÉDITO O REFACTURACIÓN SE REALIZA LA BÚSQUEDA DEL DOCUMENTO ASOCIADO (VIAJE O FACTURA VARIA) 
                if (EdocType == eDocumentoPolizas.NOTAS_DE_CREDITO || EdocType == eDocumentoPolizas.REFACTURACION_DE_PORTES ||
                    EdocType == eDocumentoPolizas.REFACTURACION_VARIA)
                {

                    if (!string.IsNullOrEmpty(oEncabezado.doctoref))
                    {
                        //SOLO SI ES DE TIPO NOTAS DE CREDITO SE CAMBIA EL GIRO DEL TIPO DE DOCUMENTO 
                        if (EdocType == eDocumentoPolizas.NOTAS_DE_CREDITO)
                            oContrato.Encabezado.TipoRelacionUUID = EInvoiceCFDIReferenceType_MX.CreditNote;

                        List<LOGI_Polizas_INFO> lstNotas = new List<LOGI_Polizas_INFO>();
                        LOGI_Polizas_INFO oNotaC = new LOGI_Polizas_INFO();
                        oNotaC.estatus = -1;//Solo tomamos los códigos que esten sincronizados
                        oNotaC.folio = Regex.Replace(oEncabezado.doctoref, @"[A-Za-z ]", string.Empty).Trim();
                        oNotaC.serie = Regex.Replace(oEncabezado.doctoref, @"[0-9\-]", string.Empty).Trim();
                        oPolizaCtrl.ListaPolizas(CONST_USUARIO, oNotaC, ref lstNotas);
                        if (lstNotas.Count == 0)
                        {
                            this.sMensaje = string.Empty;
                            return false;
                        }
                        oContrato.Encabezado.IdFactReferencia = Convert.ToInt64(lstNotas[0].recIdD365);
                        lstFacturas = new List<LOGI_Factura_INFO>();
                        oFactura = new LOGI_Factura_INFO();
                        oFactura.Folio = Convert.ToInt32(oNotaC.folio);
                        oFactura.serie = oNotaC.serie;
                        response = oPolizaCtrl.RecuperaMasterEDIXML(CONST_USUARIO, ref lstFacturas, oFactura);
                        if (response != "OK")
                            throw new Exception("No se ha encontrado el documento fiscal relacionado a la nota de crédito");
                        oFactura = new LOGI_Factura_INFO();
                        oFactura = lstFacturas[0];
                        sUUUIDRef = oFactura.sUUID;
                        oContrato.Encabezado.UUIDRelacionado = oFactura.sUUID;
                    }
                    else throw new Exception("La nota de crédito no tiene un documento fiscal asociado");
                }


                var oSucursal = lstDetalle.FirstOrDefault(x=>x.sucursal_D365 != ""); 
                oContrato.Encabezado.DimSucursal = oSucursal.sucursal_D365 == "NO EXISTE" ? string.Empty : oSucursal.sucursal_D365;

                //RECUPERAMOS LA CUENTA CONTABLE CORRESPONDIENTE AL IVA (IMPUESTO APLICADO A PROVISIÓN)
                List<LOGI_Catalogos_INFO> lstCat = new List<LOGI_Catalogos_INFO>();
                LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                oParam.iEmpresa = CONST_EMPRESA;
                lstListaContrato = new List<BBICargarFacturaCxCContractLin>();
                string sDescrip = string.Empty;
                Decimal importetotal = 0;
                foreach (LOGI_Polizas_detalle_INFO o in lstDetalle)
                {
                    if (!(o.mayor == 6073 || o.mayor == 6573 || o.mayor == 6090))
                        continue;

                    oLinea = new BBICargarFacturaCxCContractLin();
                    lstCat = new List<LOGI_Catalogos_INFO>();

                    if (o.descrip.ToUpper().Contains("FLETES"))
                        sDescrip = "FLETE";
                    else if (o.descrip.Contains("MANIOBRAS"))
                        sDescrip = "MANIOBRA";
                    else
                        sDescrip = o.descrip.Trim().ToUpper().Replace("INGRESO POR : ", string.Empty).Trim();

                    oLinea.CuentaContable = o.cuenta_AX;
                    oLinea.Texto = string.Format("{0}-{1}{2}-{3}", sDescrip, oEncabezado.serie, oEncabezado.folio, oEncabezado.nombrerfc); //oAXCuenta.descrip;
                    oLinea.Texto = oLinea.Texto.Length > 60 ? oLinea.Texto.Substring(0, 59) : oLinea.Texto;
                    oLinea.Texto = oLinea.Texto.ToUpper();
                    if (EdocType == eDocumentoPolizas.NOTAS_DE_CREDITO)
                        oLinea.Debito = o.importe;
                    else
                        oLinea.Credito = o.importe;
                    importetotal += o.importe;
                    oLinea.DimArea = o.area_D365 == "NO EXISTE" ? string.Empty : o.area_D365;
                    oLinea.DimCeco = o.centrocosto_D365 == "NO EXISTE" ? string.Empty : o.centrocosto_D365;
                    oLinea.DimSucursal = o.sucursal_D365 == "NO EXISTE" ? string.Empty : o.sucursal_D365;
                    oLinea.DimDepto = o.departamento_D365 == "NO EXISTE" ? string.Empty : o.departamento_D365;

                    #region "recuperamos los grupos de impuestos según aplique retenciones"
                    oParam = new LOGI_Catalogos_INFO();
                    oParam.sAX365MATCH = "IVA16T";
                    if (sDescrip.Equals("FLETE", StringComparison.InvariantCultureIgnoreCase))
                        oParam.sAxgrupoarticulo = "FLETES";
                    else
                    {
                        oParam.sAxgrupoventa = "CLFLETES";
                        oParam.sAxgrupoarticulo = "GRAV16%";
                    }
                    oPolizaCtrl.RecuperaCatalogoImpuestos(this.CONST_EQUIV_CONNECTION, CONST_USUARIO, ref lstCat, oParam);
                    if (lstCat.Count == 0)
                        throw new Exception("No se ha podido recuperar la información del impuesto para fletes");
                    if (lstCat.Count > 1)
                        throw new Exception("Se han encontrado más de una cuenta configurada para el impuesto de fletes");
                    #endregion ""

                    oLinea.CodImpuesto = lstCat[0].sAX365;
                    oLinea.GrupoImpuestosArt = lstCat[0].sAxgrupoarticulo;
                    oLinea.GrupoImpuestos = lstCat[0].sAxgrupoventa;
                    if (!string.IsNullOrEmpty(o.vehiculo))
                        oLinea.Vehiculo = o.vehiculo.Split('|')[1].ToString();
                    oLinea.DimFilTer = o.filialtercero_D365;
                    lstListaContrato.Add(oLinea);
                    oContrato.Lineas = lstListaContrato.ToArray();
                }
                //Se asigna el importe total que se recopiló del recorrido para que no hayan descuadres
                oContrato.Encabezado.Total = importetotal;

                sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                //m_ConsoleLine(rchConsole, string.Format("Contenido de diario enviado: {0}", sJSON), eType.proceso);             

                sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, sEndPoint);
                System.ServiceModel.Channels.Binding oBindig;
                EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
                oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                oBindig.ReceiveTimeout = TimeSpan.MaxValue;
                oBindig.SendTimeout = TimeSpan.MaxValue;
                BBICargarFacturaCxCServiceClient oClienteD365 = new BBICargarFacturaCxCServiceClient(
                binding: oBindig, endpointAddress);
                Helpers.D365FOBBICxCServices.CallContext oContexto = new Helpers.D365FOBBICxCServices.CallContext();
                oContexto.Company = oContrato.Encabezado.Compania;
                using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                    Helpers.D365FOBBICxCServices.Infolog oInfoLog = oClienteD365.cargarFactura(oContexto, oContrato, out response);
                    bContinuar = this.RecuperaMensaje(ref response);
                }


            }
            catch (Exception ex)
            {
                response = ex.Message;
                this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                if (this.sMensaje.Contains("Internal Server"))
                    intentos++;
            }
            finally
            {
                if (intentos >= 2)
                    bConectado = false;
            }

            return bContinuar;
        }

        bool CreaCancelacionFactura(out string response, LOGI_Polizas_INFO oEncabezado, List<LOGI_Polizas_detalle_INFO> lstDetalle, string sEndPoint, out string sURLEndPoint, ref string sJSON, string sDiario)
        {
            bool bContinuar = false;
            response = string.Empty;
            oPolizaCtrl = new LOGI_Polizas_PD(CONST_OPE_CONNECTION);
            BBICargarFacturaCxCContract oContrato = new BBICargarFacturaCxCContract();
            List<BBICargarFacturaCxCContractLin> lstListaContrato = null;
            BBICargarFacturaCxCContractLin oLinea = null;
            sURLEndPoint = string.Empty;

            try
            {
                if (!this.bConectado)
                    onCreateLogin();

                var oCliente = lstDetalle.FirstOrDefault(x => x.mayor == 1300);
                if (oCliente == null)
                    throw new Exception("No se encuentra el cliente dentro de las líneas del detalle");


                oContrato = new BBICargarFacturaCxCContract();
                oContrato.Encabezado = new BBICargarFacturaCxCContractEnc();
                oContrato.Encabezado.SistemaOrigen = D365API.Helpers.D365FOBBICxCServices.BBISistemaOrigen.OpeAdm;
                oContrato.Encabezado.IdRegistro = oEncabezado.FolioAsiento;
                oContrato.Encabezado.NombreDiario = sDiario;
                oContrato.Encabezado.Compania = oCreds.ciad365;
                oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                oContrato.Encabezado.FechaFactura = DateTime.ParseExact(oEncabezado.fechaContable, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                oContrato.Encabezado.CodCliente = oCliente.cuenta_AX;
                oContrato.Encabezado.TipoFact = D365API.Helpers.D365FOBBICxCServices.BBITipoFact.Cancelacion;
                oContrato.Encabezado.DesctoPago = "";
                oContrato.Encabezado.Subtotal = oEncabezado.subtotal;
                oContrato.Encabezado.Total = oEncabezado.total;
                oContrato.Encabezado.Moneda = "MXN";
                oContrato.Encabezado.TipoCambio = 1.0M;
                oContrato.Encabezado.ImpuestosInc = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                oContrato.Encabezado.SinComprobante = D365API.Helpers.D365FOBBICxCServices.NoYes.Yes;
                oContrato.Encabezado.Orden = "NA";
                oContrato.Encabezado.Referencia = "NA";


                oContrato.Encabezado.TipoRelacionUUID = EInvoiceCFDIReferenceType_MX.Invoice;
                oContrato.Encabezado.Descripcion = string.Format("CANCELACIÓN. {0}-{1}", oEncabezado.doctoref, oEncabezado.nombrerfc);
                oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.Length > 60 ? oContrato.Encabezado.Descripcion.Substring(0, 59) : oContrato.Encabezado.Descripcion;
                oContrato.Encabezado.Descripcion = oContrato.Encabezado.Descripcion.ToUpper();

                var oSucursal = lstDetalle.FirstOrDefault(x => x.sucursal_D365 != "");
                oContrato.Encabezado.DimSucursal = oSucursal.sucursal_D365 == "NO EXISTE" ? string.Empty : oSucursal.sucursal_D365;

                if (!string.IsNullOrEmpty(oEncabezado.doctoref))
                {

                    List<LOGI_Polizas_INFO> lstDocumento = new List<LOGI_Polizas_INFO>();
                    LOGI_Polizas_INFO oNotaC = new LOGI_Polizas_INFO();
                    oNotaC.estatus = -1;
                    oNotaC.folio = Regex.Replace(oEncabezado.doctoref, @"[A-Za-z ]", string.Empty).Trim();
                    oNotaC.serie = Regex.Replace(oEncabezado.doctoref, @"[0-9\-]", string.Empty).Trim();
                    oPolizaCtrl.ListaPolizas(CONST_USUARIO, oNotaC, ref lstDocumento);
                    if (lstDocumento.Count == 0)
                        throw new Exception("No se ha encontrado la información del documento a cancelar");
                    oContrato.Encabezado.IdFactReferencia = Convert.ToInt64(lstDocumento[0].recIdD365);                   
                    oContrato.Encabezado.UUIDRelacionado = lstDocumento[0].uuid;
                    oContrato.Encabezado.Folio = Convert.ToInt32(lstDocumento[0].folio);
                    oContrato.Encabezado.Serie = lstDocumento[0].serie;
                    string FolioSerie = string.Format("C{0}{1}", oContrato.Encabezado.Serie, oContrato.Encabezado.Folio);

                    oContrato.Encabezado.Factura = FolioSerie;
                    oContrato.Encabezado.Docto = FolioSerie;
                    oContrato.Encabezado.Texto = string.Format("CANCELACIÓN {0}", FolioSerie);
                }
                else throw new Exception("La nota de crédito no tiene un documento fiscal asociado");

                //RECUPERAMOS LA CUENTA CONTABLE CORRESPONDIENTE AL IVA (IMPUESTO APLICADO A PROVISIÓN)
                List<LOGI_Catalogos_INFO> lstCat = new List<LOGI_Catalogos_INFO>();
                LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                oParam.iEmpresa = CONST_EMPRESA;
                lstListaContrato = new List<BBICargarFacturaCxCContractLin>();
                string sDescrip = string.Empty;
                Decimal importetotal = 0;
                foreach (LOGI_Polizas_detalle_INFO o in lstDetalle)
                {
                    if (!(o.mayor == 6073 || o.mayor == 6573 || o.mayor == 6090))
                        continue;

                    oLinea = new BBICargarFacturaCxCContractLin();
                    lstCat = new List<LOGI_Catalogos_INFO>();

                    if (o.descrip.ToUpper().Contains("FLETES"))
                        sDescrip = "FLETE";
                    else if (o.descrip.Contains("MANIOBRAS"))
                        sDescrip = "MANIOBRA";
                    else
                        sDescrip = o.descrip.Trim().ToUpper().Replace("INGRESO POR : ", string.Empty).Trim();

                    oLinea.CuentaContable = o.cuenta_AX;
                    oLinea.Texto = string.Format("{0}-{1}{2}-{3}", sDescrip, oEncabezado.serie, oEncabezado.folio, oEncabezado.nombrerfc); //oAXCuenta.descrip;
                    oLinea.Texto = oLinea.Texto.Length > 60 ? oLinea.Texto.Substring(0, 59) : oLinea.Texto;
                    oLinea.Texto = oLinea.Texto.ToUpper();
                    oLinea.Debito = o.importe;
                    importetotal += o.importe;
                    oLinea.DimArea = o.area_D365 == "NO EXISTE" ? string.Empty : o.area_D365;
                    oLinea.DimCeco = o.centrocosto_D365 == "NO EXISTE" ? string.Empty : o.centrocosto_D365;
                    oLinea.DimSucursal = o.sucursal_D365 == "NO EXISTE" ? string.Empty : o.sucursal_D365;
                    oLinea.DimDepto = o.departamento_D365 == "NO EXISTE" ? string.Empty : o.departamento_D365;

                    #region "recuperamos los grupos de impuestos según aplique retenciones"
                    oParam = new LOGI_Catalogos_INFO();
                    oParam.sAX365MATCH = "IVA16T";
                    if (sDescrip.Equals("FLETE", StringComparison.InvariantCultureIgnoreCase))
                        oParam.sAxgrupoarticulo = "FLETES";
                    else
                    {
                        oParam.sAxgrupoventa = "CLFLETES";
                        oParam.sAxgrupoarticulo = "GRAV16%";
                    }
                    oPolizaCtrl.RecuperaCatalogoImpuestos(this.CONST_EQUIV_CONNECTION, CONST_USUARIO, ref lstCat, oParam);
                    if (lstCat.Count == 0)
                        throw new Exception("No se ha podido recuperar la información del impuesto para fletes");
                    if (lstCat.Count > 1)
                        throw new Exception("Se han encontrado más de una cuenta configurada para el impuesto de fletes");
                    #endregion ""

                    oLinea.CodImpuesto = lstCat[0].sAX365;
                    oLinea.GrupoImpuestosArt = lstCat[0].sAxgrupoarticulo;
                    oLinea.GrupoImpuestos = lstCat[0].sAxgrupoventa;
                    if (!string.IsNullOrEmpty(o.vehiculo))
                        oLinea.Vehiculo = o.vehiculo.Split('|')[1].ToString();
                    oLinea.DimFilTer = o.filialtercero_D365;
                    lstListaContrato.Add(oLinea);
                    oContrato.Lineas = lstListaContrato.ToArray();
                }
                //Se asigna el importe total que se recopiló del recorrido para que no hayan descuadres
                oContrato.Encabezado.Total = importetotal;

                sJSON = JsonConvert.SerializeObject(oContrato, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                //m_ConsoleLine(rchConsole, string.Format("Contenido de diario enviado: {0}", sJSON), eType.proceso);             

                sURLEndPoint = string.Format("{0}/soap/services/{1}", oCreds.api, sEndPoint);
                System.ServiceModel.Channels.Binding oBindig;
                EndpointAddress endpointAddress = new EndpointAddress(sURLEndPoint);
                oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBICargarFacturaCxPService");
                oBindig.ReceiveTimeout = TimeSpan.MaxValue;
                oBindig.SendTimeout = TimeSpan.MaxValue;
                BBICargarFacturaCxCServiceClient oClienteD365 = new BBICargarFacturaCxCServiceClient(
                binding: oBindig, endpointAddress);
                Helpers.D365FOBBICxCServices.CallContext oContexto = new Helpers.D365FOBBICxCServices.CallContext();
                oContexto.Company = oContrato.Encabezado.Compania;
                using (OperationContextScope operationContextScope = new OperationContextScope(oClienteD365.InnerChannel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                    Helpers.D365FOBBICxCServices.Infolog oInfoLog = oClienteD365.cargarFactura(oContexto, oContrato, out response);
                    bContinuar = this.RecuperaMensaje(ref response);
                }


            }
            catch (Exception ex)
            {
                response = ex.Message;
                this.sMensaje = string.Format("ERROR: {0}", ex.Message);
                if (this.sMensaje.Contains("Internal Server"))
                    intentos++;
            }
            finally
            {
                if (intentos >= 2)
                    bConectado = false;
            }

            return bContinuar;
        }

        /// <summary>
        /// Descripción: Metodo encargado de validar el saldo de crédito disponible para el cliente,
        /// El webservice devolverá Verdadero junto con el importe del saldo del cliente, 
        /// el límite de crédito configurado en el cliente y el límite de crédito disponible(t|saldo|limitecredito|creditodisponible) 
        /// si se pudo obtener el mismo; de lo contrario devolverá falso junto con el mensaje de error(f|mensajeerror).						
        /// Autor: Ing. Abril Tun Salazar
        /// Fecha: 28/04/2022
        /// </summary>
        /// <param name="cuenta">Representa la cuenta del cliente (catalogo OPE) 10 o 50</param>
        /// <param name="scuenta">Represeta la subcuenta del cliente (catalogo OPE)</param>
        /// <param name="MontoOperacion">Determina el monto a evaluar sobre la operación del cliente</param>
        /// <param name="response">Respuesta de retorno sobre la operación</param>
        /// <returns></returns>
        public bool SaldoCliente(Int32 cuenta, Int32 scuenta, Double MontoOperacion, out string response)
        {

            bool bContinuar = false;
            List<LOGI_Catalogos_INFO> lstClientes = new List<LOGI_Catalogos_INFO>();
            LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
            oParam.iCuenta = cuenta;
            oParam.iSubcuenta = scuenta;
            oParam.iActivo = 1;
            oParam.iArea= -1;
            oParam.iEmpresa = CONST_EMPRESA;
            try
            {
                new LOGI_Clientes_PD(this.CONST_EQUIV_CONNECTION).ListaClientes(CONST_USUARIO, oParam, ref lstClientes);
                if (lstClientes.Count == 0)
                    throw new Exception("No se ha podido recuperar la información del cliente en catálogo de equivalencias");
                if (lstClientes.Count > 1)
                    throw new Exception("La configuración del cliente se encuentra duplicada en catálogo de equivalencias");
                oParam = lstClientes[0];


                Helpers.D365FOBBISaldoClienteServices.CallContext callContext;
                Helpers.D365FOBBISaldoClienteServices.BBISaldoClienteServiceClient cliente;
                EndpointAddress endpointAddress;
                System.ServiceModel.Channels.Binding oBindig;
                Uri uri;
                string dirEndpoint;
                dirEndpoint = string.Format("{0}/soap/services/{1}", this.oCreds.api, "BBISaldoClienteServices");
                uri = new Uri(dirEndpoint);
                endpointAddress = new EndpointAddress(dirEndpoint);
                oBindig = oTools.crearBasicHttpBinding("BasicHttpBinding_BBISaldoClienteService");

                cliente = new Helpers.D365FOBBISaldoClienteServices.BBISaldoClienteServiceClient(oBindig, endpointAddress);

                try
                {
                    Helpers.D365FOBBISaldoClienteServices.Infolog infolog;
                    callContext = new Helpers.D365FOBBISaldoClienteServices.CallContext();
                    callContext.Company = oCreds.ciad365;
                    using (OperationContextScope operationContextScope = new OperationContextScope(cliente.InnerChannel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["Authorization"] = string.Format("{0} {1}", oSession.token_type, oSession.access_token);
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                        infolog = cliente.obtenerSaldo(callContext, oParam.sAX365, oCreds.ciad365, out response);
                    }
                    bContinuar = RecuperaMensaje(ref response);
                    if (bContinuar)
                    {
                        bContinuar = false;
                        var spldata = response.Split('|');
                        Double credito = Convert.ToDouble(spldata[3].ToString());
                        if (credito > 0)
                        {
                            if (credito >= MontoOperacion)
                                bContinuar = true;
                            else response = String.Format("El monto de la operación excede el límite del crédito disponible para el cliente. Saldo: {0} Límite {1} Disponible {2}", Convert.ToDecimal( spldata[1]).ToString("n2"), Convert.ToDecimal( spldata[2]).ToString("n2"), Convert.ToDecimal( spldata[3]).ToString("n2"));
                        }
                        else response = String.Format("El cliente no cuenta con crédito disponible. Saldo: {0} Límite {1} Disponible {2}", Convert.ToDecimal(spldata[1]).ToString("n2"), Convert.ToDecimal(spldata[2]).ToString("n2"), Convert.ToDecimal(spldata[3]).ToString("n2"));
                    }
                    if (rchConsole != null)
                        oTools.m_ConsoleLine(rchConsole, string.Format(@"Información del saldo del cliente ""{0}"" {1}", oParam.sAX365, response), bContinuar ? eType.success : eType.error);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (rchConsole != null)
                    oTools.m_ConsoleLine(rchConsole, ex.Message, eType.error);
                response = ex.Message;
            }

            return bContinuar;
        }


        /// <summary>
        /// Proceso utilizado para calcular la fecha de vencimiento de una factura. Según los días de crédito
        /// otorgados al cliente o proveedor
        /// </summary>
        /// <param name="cuenta">Cuenta contable para el proveedor o cliente</param>
        /// <param name="scuenta">Subcuenta para el cliente o proveedor</param>
        /// <param name="Fechadocto">Fecha de emisión del documento fiscal. Sobre este valor se aplican los días de credito para realizar el calculo</param>
        /// <returns></returns>
        DateTime FechaVencimientoSocioNegocio(int cuenta, int scuenta, DateTime Fechadocto)
        {
            List<LOGI_Proveedores_INFO> lstSocios = new List<LOGI_Proveedores_INFO>();
            LOGI_Proveedores_INFO otemp = new LOGI_Proveedores_INFO();
            otemp.cuenta = cuenta;
            otemp.scuenta = scuenta;
            new PD.Tablas.FUEL.LOGI_Proveedores_PD(this.CONST_OPE_CONNECTION).Listaproveedores("", ref lstSocios, otemp);
            /*if (lstSocios.Count == 0)
                throw new Exception("No se ha encontrado la información para los días de crédito para el socio de negocio");*/
            if (lstSocios.Count > 1)
                throw new Exception("Se ha detectado más de una configuración para los días de crédito del socio de negocio");
            if (lstSocios[0].diacredito < 0)
                throw new Exception("La configuración de los días de crédito para el socio de negocio no están establecidos");
            return Fechadocto.AddDays(lstSocios[0].diacredito);
        }
        bool RecuperaMensaje(ref string response)
        {
            response = response.Replace("'",string.Empty);
            var spl = response.Split('|');
            this.sMensaje = response;
            return spl[0].Trim().Equals("t", StringComparison.InvariantCultureIgnoreCase);
        }
         
    }
}
