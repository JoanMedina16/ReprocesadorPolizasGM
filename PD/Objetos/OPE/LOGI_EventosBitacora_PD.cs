using AD;
using AD.Objetos.OPE;
using AD.Tablas.D365;
using INFO.Enums;
using INFO.Tablas.D365;
using OfficeOpenXml;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace PD.Objetos.OPE
{
  public  class LOGI_EventosBitacora_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnectionLOG = null;
        internal LOGI_ConexionSql_AD oConnectionADM = null;
        internal LOGI_EventosBitacora_AD oBitacorapoliza = null;
        const string CONST_CLASE = "LOGI_EventosBitacora_PD.cs";
        const string CONST_MODULO = "Bitacora polizas D365";
        const int CONST_EMPRESA = 67;///Siempre corresponde a 67 - Logística del Mayab

        public LOGI_EventosBitacora_PD(string sConnectionLOG, string sConnectionADM)
        {
            oConnectionLOG = new LOGI_ConexionSql_AD(sConnectionLOG);
            oConnectionADM = new LOGI_ConexionSql_AD(sConnectionADM);
            oBitacorapoliza = new  LOGI_EventosBitacora_AD();
            oTool = new LOGI_Tools_PD();
        }   
         
        public string CrearegistroBitacora(string sUsuarioID, LOGI_PolizasD365_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnectionLOG.OpenConnection();
                oConnectionADM.OpenConnection();
                List<LOGI_PolizasD365_INFO> lstTransacciones = new List<LOGI_PolizasD365_INFO>();
                LOGI_ConfiguracionD365_INFO oConfig = new LOGI_ConfiguracionD365_INFO();
                LOGI_PolizasD365_INFO otemp = new LOGI_PolizasD365_INFO();
                otemp.folio = oParam.folio;
                otemp.tipo = oParam.tipo;
                otemp.estatus = -1;
                otemp.enviado = -1;
                sReponse = oBitacorapoliza.ListaBitacoraPolizas(ref oConnectionLOG, ref lstTransacciones, otemp, out sConsultaSql);

                if (lstTransacciones.Count == 1)
                {
                    //Si el registro existe se incrementa el contador y se actualiza mensaje de errores
                    otemp = new LOGI_PolizasD365_INFO();
                    otemp = lstTransacciones[0];
                    otemp.errores++;
                    otemp.mensaje = oParam.mensaje.Length > 500 ? oParam.mensaje.Substring(0, 498) : oParam.mensaje;
                    otemp.JSONPATH = oParam.JSONPATH;
                    otemp.folio = oParam.folio;
                    otemp.tipo = oParam.tipo;
                    if (otemp.estatus == 1)
                    {
                        sReponse = oBitacorapoliza.ActualizaBitacoraPoliza(ref oConnectionLOG, sUsuarioID, otemp, out sConsultaSql);
                        if (sReponse != "OK")
                            throw new Exception("No se ha podido actualizar el documento");
                        sReponse = new LOGI_ConfiguracionD365_AD().ListaConfiguracion(ref oConnectionADM, ref oConfig, out sConsultaSql);
                        if (sReponse != "OK")
                            throw new Exception("No se ha determinado la configuración para límite de errores");
                        if (otemp.errores >= oConfig.intentos)
                        {
                            otemp.estatus = 2;
                            otemp.enviado = 0;
                            sReponse = oBitacorapoliza.ActualizaBitacoraPoliza(ref oConnectionLOG, sUsuarioID, otemp, out sConsultaSql);
                            if (sReponse != "OK")
                                throw new Exception("No se ha podido actualizar el documento");
                        }
                    }
                }
                else
                {
                    ///Crea un nuevo registro de bitacora
                    oParam.mensaje = oParam.mensaje.Length > 500 ? oParam.mensaje.Substring(0, 498) : oParam.mensaje;
                    sReponse = oBitacorapoliza.NuevaBitacoraERROR(ref oConnectionLOG, oParam, out sConsultaSql);
                    if (sReponse != "OK")
                        throw new Exception("No se ha podido crear el registro de error");
                }
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Creaerror", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();

                if (oConnectionADM != null)
                    oConnectionADM.CloseConnection();
            }
            return sReponse;
        }


        public string CreaprocesoNotificacionERRORES(string sUsuarioID)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionLOG.OpenConnection();
                oConnectionADM.OpenConnection();

                List<LOGI_PolizasD365_INFO> lstTransacciones = new List<LOGI_PolizasD365_INFO>();
                LOGI_ConfiguracionD365_INFO oConfig = new LOGI_ConfiguracionD365_INFO();
                LOGI_PolizasD365_INFO otemp = new LOGI_PolizasD365_INFO();
                otemp.estatus = 2;
                otemp.enviado = 0;
                DateTime primerdiames = new DateTime();

                if (DateTime.Now.Day==1)
                    primerdiames= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                else
                    primerdiames = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                otemp.FechaInicio = primerdiames.ToShortDateString();

                otemp.FechaFinal = DateTime.Now.AddDays(-1).ToShortDateString();

                sReponse = oBitacorapoliza.ListaBitacoraPolizas(ref oConnectionLOG, ref lstTransacciones, otemp, out sConsultaSql);
                

                if (lstTransacciones.Count > 0)
                {
                    sReponse = new LOGI_ConfiguracionD365_AD().ListaConfiguracion(ref oConnectionADM, ref oConfig, out sConsultaSql);
                    if (sReponse != "OK")
                        throw new Exception("No se ha determinado la configuración para límite de errores");
                    oConfig.cuentas_soport_mail = LOGI_Rijndael_PD.DecryptRijndael(oConfig.cuentas_soport_mail);
                    oConfig.password_mail = LOGI_Rijndael_PD.DecryptRijndael(oConfig.password_mail);
                    oConfig.api_tms = LOGI_Rijndael_PD.DecryptRijndael(oConfig.api_tms);
                    oConfig.url_tms = LOGI_Rijndael_PD.DecryptRijndael(oConfig.url_tms);
                    oConfig.host_tms = LOGI_Rijndael_PD.DecryptRijndael(oConfig.host_tms);

                    //Si el registro existe se incrementa el contador y se actualiza mensaje de errores
                    var lstDocumentos = lstTransacciones.GroupBy(x => x.tipo).ToList();
                    List<LOGI_PolizasD365_INFO> lstFacturas = new List<LOGI_PolizasD365_INFO>();
                    List<LOGI_PolizasD365_INFO> lstNotasCredito = new List<LOGI_PolizasD365_INFO>();
                    List<LOGI_PolizasD365_INFO> lstCancelaciones = new List<LOGI_PolizasD365_INFO>();
                    List<LOGI_PolizasD365_INFO> lstPasivos = new List<LOGI_PolizasD365_INFO>();
                    List<LOGI_PolizasD365_INFO> lstDispersiones = new List<LOGI_PolizasD365_INFO>();
                    List<LOGI_PolizasD365_INFO> lstComprobaciones = new List<LOGI_PolizasD365_INFO>();
                    List<LOGI_PolizasD365_INFO> lstManodeObra = new List<LOGI_PolizasD365_INFO>();
                    List<LOGI_PolizasD365_INFO> lstFacturaspago = new List<LOGI_PolizasD365_INFO>();
                    lstFacturas = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.FACTURACION_DE_VIAJES).ToList();
                    this.CreaBodyCorreo(oConfig, lstFacturas, oConfig.cuentas_cxc_mail, "PROGRAMADO- FACTURAS NO SINCRONIZADAS", sTipoSocio: "Cliente");

                    lstNotasCredito = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.NOTAS_DE_CREDITO).ToList();
                    this.CreaBodyCorreo(oConfig, lstNotasCredito, oConfig.cuentas_cxc_mail, "PROGRAMADO- NOTAS DE CREDITO NO SINCRONIZADAS", sTipoSocio: "Cliente");

                    lstCancelaciones = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.CANCELACION_DE_INGRESOS).ToList();
                    this.CreaBodyCorreo(oConfig, lstCancelaciones, oConfig.cuentas_cxc_mail, "PROGRAMADO- CANCELACIONES NO SINCRONIZADAS", sTipoSocio: "Cliente");

                    lstDispersiones = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.DISPERSION_ANTICIPOS).ToList();
                    this.CreaBodyCorreo(oConfig, lstDispersiones, oConfig.cuentas_gsto_mail, "PROGRAMADO- DISPERSIONES NO SINCRONIZADAS", sTipoSocio: "Operador");

                    lstComprobaciones = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.COMPROBACION_DE_GASTOS).ToList();
                    this.CreaBodyCorreo(oConfig, lstComprobaciones, oConfig.cuentas_gsto_mail, "PROGRAMADO- GASTOS NO SINCRONIZADAS", sTipoSocio: "Operador");

                    lstManodeObra = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.REGISTRO_MANO_DE_OBRA).ToList();
                    this.CreaBodyCorreo(oConfig, lstManodeObra, oConfig.cuentas_mtto_mail, "PROGRAMADO- MANO OBRA NO SINCRONIZADAS", sTipoSocio: string.Empty);

                    lstFacturaspago = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.REGISTRO_DE_PASIVOS).ToList();
                    this.CreaBodyCorreo(oConfig, lstFacturaspago, oConfig.cuentas_cxp_mail, "PROGRAMADO- PASIVOS NO SINCRONIZADOS", sTipoSocio: string.Empty);
                    //lstManodeObra = lstTransacciones.Where(x => x.tipo == eDocumentoTMS.CONSUMO_DE_COMBUSTIBLES).ToList();
                    //this.CreaBodyCorreo(oConfig, lstManodeObra, oConfig.cue, "MANO OBRA NO SINCRONIZADAS", sTipoSocio: string.Empty);
                    //Se actualizan los intentos de envío 
                    foreach (LOGI_PolizasD365_INFO oTransac in lstTransacciones)
                    {
                        otemp = new LOGI_PolizasD365_INFO();
                        otemp.enviado = 0;
                        otemp.errores = -1;
                        oTransac.enviados++;
                        otemp.enviados = oTransac.enviados;
                        otemp.IdRegistro = oTransac.IdRegistro;
                        otemp.estatus = -1;
                        if (otemp.enviados >= oConfig.intento_mail)
                            otemp.enviado = 1;
                        sReponse = oBitacorapoliza.ActualizaBitacoraPoliza(ref oConnectionLOG, "", otemp, out sConsultaSql);
                        if (sReponse != "OK")
                            throw new Exception("No se ha podido actualizar las transacciones de bitacora");
                    }
                    sReponse = "OK";
                }
            }

            catch (Exception ex)
            {
                oTool.LogError(ex, "Creaerror", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();

                if (oConnectionADM != null)
                    oConnectionADM.CloseConnection();
            }
            return sReponse;
        }

        void CreaBodyCorreo(LOGI_ConfiguracionD365_INFO oConfig, List<LOGI_PolizasD365_INFO> lstRegistros, string sCuentas, string sAsunto, string sTipoSocio = "")
        {
            String HTMLBody = string.Empty, sPathAttach = string.Empty, sTablaEventos = string.Empty ;
            int contador = 0;
            if (lstRegistros.Count == 0)
                return;


            sTablaEventos = @"<table ><tr><th colspan='5' ><center><h1>DOCUMENTOS NO SINCRONIZADOS<h1></center> </th> </tr>&nbsp;";
            foreach (LOGI_PolizasD365_INFO x in lstRegistros)
            {
                if (contador > 5)
                    break;

                sTablaEventos += "<tr> <td> <h3>Folio documento: </h3> </td> <td>" + x.folio + " </td></tr>";
                sTablaEventos += "<tr> <td> <h3>Factura: </h3> </td> <td>" + x.factura + " </td></tr>";
                sTablaEventos += "<tr> <td> <h3>Fecha: </h3> </td> <td>" + x.fechaconta.ToString("dd/MM/yyyy") + " </td></tr>";
                if (!string.IsNullOrEmpty(sTipoSocio))
                {
                    sTablaEventos += "<tr> <td> <h3>" + sTipoSocio + ": </h3> </td> <td>" + x.nombresocio + " </td></tr>";
                }
                sTablaEventos += "<tr> <td> <h3>Mensaje: </h3> </td> <td>" + (x.mensaje.Length > 180 ? x.mensaje.Substring(0, 180) : x.mensaje) + " </td></tr>";
                contador++;
            }
            sTablaEventos += @"</table>";
            string logoLM = " Sincronización de documentos <B>DYNAMICS 365</B> ";
            HTMLBody = @"<html><head><meta charset=""UTF-8""></head><title> </title> <style type=""text/css""> 
table {font-size:12px;color:#333333;border-width: 1px;border-color: #9cab93;border-collapse: collapse; width:100%; cursor:default;} table th 
{font-size:12px;background-color:#9cab93;border-width: 1px;padding: 3px;border-style: solid;border-color: #9cab93;text-align:center;}
                                              table tr {background-color:#ffffff;}
                                              table td {font-size:12px;border-width: 1px;padding: 5px;border-style: solid;border-color: #9cab93;} </style> </head><body>";
            HTMLBody += string.Format(@"
                                        <h2>67 - LOGISTICA DEL MAYAB</h2>
                                        <p> Relación de documentos pendientes por sincronizar, se envia el adjunto con la relación de registros.  </p>");
            HTMLBody += sTablaEventos;
            HTMLBody += string.Format(@"
                                        <p><u>*Este es un correo enviado a través de un servicio automatico, favor de no responderlo.</u>*</p>
                                        <BR><BR>
                                        {0}
                                        </body></html>", logoLM);


            if (lstRegistros.Count > 5) //si son más de cinco registros se crea un archivo excel de lo contrario solo se contempla el HTML
            {
                string sPath = string.Format(@"{0}\LOGS_SERVICIO", Environment.CurrentDirectory);
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                sPathAttach = string.Format(@"{0}\{1}_{2}.xlsx", sPath, "REPORTE_TMS", DateTime.Now.ToString("yyyyMMddHHmmss"));
                ExcelPackage package = new ExcelPackage(new FileInfo(sPathAttach));
                ExcelWorksheet hojaproceso = package.Workbook.Worksheets.Add("REPORTE_TMS");
                hojaproceso.Cells["A1"].Value = "ID INTERNO";
                hojaproceso.Cells["B1"].Value = "DOCUMENTO";
                hojaproceso.Cells["C1"].Value = "FOLIO";
                hojaproceso.Cells["D1"].Value = "MENSAJE";

                int linea = 1;
                foreach (LOGI_PolizasD365_INFO item in lstRegistros)
                {
                    linea++;
                    hojaproceso.Cells["A" + linea].Value = item.IdRegistro;
                    hojaproceso.Cells["B" + linea].Value = oTool.GetEnumDefaultValue((eDocumentoTMS)item.tipo); 
                    hojaproceso.Cells["C" + linea].Value = item.folio;
                    hojaproceso.Cells["D" + linea].Value = item.mensaje;
                }
                hojaproceso.Cells["A:I"].AutoFitColumns();
                package.Save();
            }
            this.NotificaXCorreo(oConfig, sCuentas, sAsunto, HTMLBody, sPathAttach);

        }

        void NotificaXCorreo(LOGI_ConfiguracionD365_INFO oConfig, string sCuentas, string sAsunto, String HTMLBody, string sPathAttach)
        {
            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress(oConfig.user_mail);


            if (!string.IsNullOrEmpty(sPathAttach))
            {
                System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(sPathAttach);
                correo.Attachments.Add(at);
            }
            try
            {
                foreach (string em in sCuentas.Split(';'))
                {
                    if (!string.IsNullOrEmpty(em))
                        correo.To.Add(em);
                }
            }
            catch { correo.To.Add(sCuentas); }

            try
            {
                foreach (string em in oConfig.cuentas_soport_mail.Split(';'))
                {
                    if (!string.IsNullOrEmpty(em))
                        correo.CC.Add(em);
                }
            }
            catch { correo.CC.Add(oConfig.cuentas_soport_mail); }

            correo.Subject = "TMS -" + sAsunto;
            correo.Body = HTMLBody;

            correo.IsBodyHtml = true;
            correo.Priority = System.Net.Mail.MailPriority.High;
            
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            
            smtp.Host = oConfig.host_tms;//servidor de correos
            smtp.Port = Convert.ToInt32( oConfig.puerto_mail); //puerto de salida
            smtp.Credentials = new System.Net.NetworkCredential(
                oConfig.user_mail, //cuenta de correo
                oConfig.password_mail //contraseña de cuenta
                );

            if (oConfig.ssl_mail == 1)
                smtp.EnableSsl = true;
            smtp.Send(correo);
            
        }
    }
}
