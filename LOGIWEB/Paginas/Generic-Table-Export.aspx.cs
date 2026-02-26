using INFO.Objetos;
using INFO.Objetos.D365;
using INFO.Objetos.SAT;
using INFO.Tablas.D365;
using Ionic.Zip;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using PD.Herramientas;
using PD.Objetos.D365;
using PD.Tablas.D365;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Paginas
{
    public partial class Generic_Table_Export : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String sMethod = Request.QueryString["modo"].ToString();
                String sAsistente = Request.QueryString["identificador"].ToString();
                String sLinea = String.Empty, fecha_inicio = string.Empty, fecha_final = string.Empty,
                   diario = string.Empty, documento = string.Empty, cuenta = string.Empty,
                   sucursal = string.Empty, centro = string.Empty, depto = string.Empty;
                try
                {
                    sLinea = Request.QueryString["linea"].ToString();
                }
                catch { }
                try {
                    fecha_inicio = Request.QueryString["fecha_inicio"].ToString();
                    fecha_final = Request.QueryString["fecha_final"].ToString();
                    diario = Request.QueryString["diario"].ToString();
                    documento = Request.QueryString["documento"].ToString();
                    cuenta = Request.QueryString["cuenta"].ToString();
                    sucursal = Request.QueryString["sucursal"].ToString();
                    centro = Request.QueryString["centro"].ToString();
                    depto = Request.QueryString["depto"].ToString();
                }
                catch { }

                switch (sMethod)
                {
                    case "reportecstm":
                        LOGI_Extraccion_ZAP_INFO otemp = new LOGI_Extraccion_ZAP_INFO();
                        otemp.fecha_inicio = fecha_inicio.Replace("'",String.Empty).Trim();
                        otemp.fecha_final = fecha_final.Replace("'", String.Empty).Trim();
                        otemp.diario = diario.Replace("'", String.Empty).Trim();
                        otemp.documento = documento.Replace("'", String.Empty).Trim();
                        otemp.cuenta = cuenta.Replace("'", String.Empty).Trim();
                        otemp.sucursal = sucursal.Replace("'", String.Empty).Trim();
                        otemp.centro = centro.Replace("'", String.Empty).Trim();
                        otemp.depto = depto.Replace("'", String.Empty).Trim();
                        GenerareporteZAPDimensiones("reporte_cuentas", otemp);
                        break;

                    case "archivo":
                        this.DescargArchivo(sAsistente);
                        break;

                    case "comprobanteviatico":
                        this.m_ConsultaXMLViatico(sAsistente, sLinea);
                        break;

                    case "datos":
                        this.m_ConstruyeDataExcel(sAsistente);
                        break;

                    case "datos_remesa":
                        try
                        {
                            this.m_DescargaZIPRemesas(sAsistente);
                        }
                        catch { }
                        break;

                    case "diferencia":
                        this.m_MessageInterfaz("La cabecera difiere de la longitud de valores, favor de configurar la misma longitud", "warning");
                        break;
                    case "serverror":
                        this.m_MessageInterfaz("Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente", "error");
                        break;

                    case "plantilla":
                        m_ConsultaAsistentPlantilla(sAsistente);
                        break;

                    default:
                        this.m_MessageInterfaz("No se ha obtenido la instrucción para exportar datos o la información enviada se encuentra vacia", "warning");
                        break;
                }

            }
            catch (Exception ex)
            {
                this.m_MessageInterfaz(string.Format("No se ha podido procesar la instrucción. ERROR: {0}", ex.Message), "error");
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GeneraArchivoSalida(LOGI_Tablainformacion_INFO contenido, string sNombre)
        {
            try
            {

                Generic_Table_Export oThis = new Generic_Table_Export();
                LOGI_WebTools_PD oTools = new LOGI_WebTools_PD();
                if (oTools.UsuarioSession() == null)
                    return "-1;";
                else return oThis.m_GuardaDataSession(contenido, sNombre);
            }
            catch {
                return "OK";
            }
        }

        void m_ConstruyeDataExcel(string Asistente)
        {

            TableRow fila = new TableRow();
            TableCell celda = new TableCell();

            try
            {
                int iColumn = 0;
                string sFullpath = string.Empty, sPath = string.Empty, sNameFile = string.Empty;
                char cAlpha = 'A';

                String TituloAsistente = (String)Session["titleAsistente_" + Asistente];
                sPath = string.Format(@"{0}\tmp\output", AppDomain.CurrentDomain.BaseDirectory);
                try
                {
                    DateTime oTime = DateTime.Now.AddDays(-8);
                    DirectoryInfo oDir = new DirectoryInfo(sPath);
                    FileInfo[] oFiles = oDir.GetFiles().OrderBy(x => x.CreationTime).ToArray();
                    foreach (FileInfo oFile in oFiles)
                    {
                        if (oFile.CreationTime <= oTime)
                        {
                            try
                            {
                                //eliminamos el archivo 
                                oFile.Delete();
                            }
                            catch { }
                        }
                        else continue;
                    }
                }
                catch { }
                sNameFile = string.Format(@"{0}{1}.xls", TituloAsistente, DateTime.Now.ToString("ddMMyyyyHHmm"));
                sFullpath = string.Format(@"{0}\{1}", sPath, sNameFile);
                LOGI_Tablainformacion_INFO oTabla = (LOGI_Tablainformacion_INFO)Session["dataExportGrid_" + Asistente];
                ExcelPackage oPack = new ExcelPackage(new FileInfo(sFullpath));
                ExcelWorksheet oWork = oPack.Workbook.Worksheets.Add(TituloAsistente);
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                iColumn++;
                foreach (LOGI_Columnas_INFO header in oTabla.lstHeader)
                {
                    string sColum = string.Format("{0}{1}", cAlpha, iColumn);
                    oWork.Cells[sColum].Value = header.sColum;
                    oWork.Cells[sColum].Style.Font.Bold = true;
                    cAlpha++;

                }
                iColumn = 2;
                foreach (List<LOGI_Filas_INFO> data in oTabla.lstValues)
                {
                    cAlpha = 'A';
                    for (int i = 0; i < data.Count; i++)
                    {
                        string sColum = string.Format("{0}{1}", cAlpha, iColumn);
                        string sValue = string.Empty;
                        String sDato = data[i].sValor == null ? "" : data[i].sValor.ToString();
                        if (oTabla.lstHeader[i].sType.ToString().Trim().Equals("moneda", StringComparison.InvariantCultureIgnoreCase))
                            sValue = string.Format("{0:#.0000}", Convert.ToDecimal(sDato));
                        else sValue = sDato;
                        if (oTabla.lstHeader[i].sType.ToString().Trim().Equals("moneda", StringComparison.InvariantCultureIgnoreCase))
                            oWork.Cells[sColum].Style.Numberformat.Format = "0.00";

                        oWork.Cells[sColum].Value = sValue;
                        cAlpha++;
                    }
                    iColumn++;
                }
                oWork.Cells[string.Format("A:{0}", cAlpha)].AutoFitColumns();
                oPack.Save();
                m_OutputExcel(sNameFile, sFullpath);
                try
                {
                    Session["dataExportGrid_" + Asistente] = null;
                    Session["titleAsistente_" + Asistente] = null;
                    Session.Remove("dataExportGrid_" + Asistente);
                    Session.Remove("titleAsistente_" + Asistente);
                }
                catch { }

                try
                {
                    File.Delete(sFullpath);
                }
                catch { }
            }
            catch (Exception ex)
            {
                this.m_MessageInterfaz(string.Format("No se ha podido generar la información. ERROR: {0}", ex.Message), "error");
            }
        }

        void m_ConsultaXMLViatico(string FolioViatico, string Linea)
        {
            try
            {
                LOGI_WebTools_PD oTools = new LOGI_WebTools_PD();
                string response = string.Empty;
                LOGI_Polizas_detalle_INFO oParam = new LOGI_Polizas_detalle_INFO();
                LOGI_Polizas_detalle_INFO oLinea = new LOGI_Polizas_detalle_INFO();
                oParam.linea = Convert.ToInt32(Linea);
                oParam.tipo_documento = 9;
                oParam.FolioAsiento = FolioViatico;


                response = new LOGI_Polizas_detalle_PD(oTools.CONST_CONNECTION).RecuperaXMLLinea("", oParam, ref oLinea);
                if (!response.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("El comprobante del viatico no existe");
                if (string.IsNullOrEmpty(oLinea.XML))
                    throw new Exception("La línea no cuenta con un comprbante XML");

                string sPath = string.Format(@"{0}\tmp\output", AppDomain.CurrentDomain.BaseDirectory);
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                string sFullPath = string.Format(@"{0}\{1}_{2}_{3}.xml", sPath, FolioViatico, Linea, DateTime.Now.ToString("ddMMyyyyHHmm"));

                File.Create(sFullPath).Dispose();
                using (TextWriter tw = new StreamWriter(sFullPath))
                {
                    tw.WriteLine(oLinea.XML);
                }

                m_OutputTXT(new FileInfo(sFullPath).Name, sFullPath);

            }
            catch (Exception ex)
            {
                this.m_MessageInterfaz(string.Format("No se ha podido generar la información. ERROR: {0}", ex.Message), "error");
            }
        }


        void m_DescargaZIPRemesas(string Asistente)
        {
            try
            {
                Asistente = "reporte_de_remesas";
                string response = string.Empty;
                LOGI_WebTools_PD oTools = new LOGI_WebTools_PD();
                List<LOGI_XMLS_INFO> lstPolizas = new List<LOGI_XMLS_INFO>();


                LOGI_XMLS_INFO oTemp = (LOGI_XMLS_INFO)Session["FACTURAS_BEBIDAS"];
                oTemp.bContent = true; 
                response = new LOGI_Historico_XMLS_PD(oTools.CONST_CONNECTION).ListaCFDIS(ref lstPolizas, oTemp);
                if (response.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    string sPath = string.Format(@"{0}\tmp\output\CFDIS", AppDomain.CurrentDomain.BaseDirectory);
                    string sFullPath = string.Format(@"{0}\{1}_{2}.zip", sPath, Asistente, DateTime.Now.ToString("ddMMyyyyHHmm"));
                    if (!Directory.Exists(sPath))
                        Directory.CreateDirectory(sPath);
                    string sPATHCFDI = string.Empty;
                    using (ZipFile zip = new ZipFile())
                    {
                        foreach (LOGI_XMLS_INFO oLinea in lstPolizas)
                        {
                            sPATHCFDI = string.Format(@"{0}\{1}_{2}.xml", sPath, oLinea.Serie, oLinea.Folio);
                            if (!File.Exists(sPATHCFDI))
                                File.WriteAllText(sPATHCFDI, oLinea.CFDIContent);
                            zip.AddFile(sPATHCFDI, DateTime.Now.ToString("ddMMyyyyHHmm"));
                        }

                        if (zip.Entries.Count > 0)
                        {
                            zip.Comment = DateTime.Now.ToString("ddMMyyyyHHmm");
                            zip.Save(sFullPath);
                        }
                    }                    
                    m_OutputZIP(string.Format("{0}_{1}.zip", Asistente, DateTime.Now.ToString("ddMMyyyyHHmm")), sFullPath);

                }
                else throw new Exception(response);

            }
            catch (Exception ex)
            {
                this.m_MessageInterfaz(string.Format("No se ha podido generar la información. ERROR: {0}", ex.Message), "error");
            }
        }

        void m_ConsultaAsistentPlantilla(string Asistente)
        {
            try
            {
                LOGI_WebTools_PD oTools = new LOGI_WebTools_PD();
                string response = string.Empty;
                LOGI_Plantilla_INFO oParam = new LOGI_Plantilla_INFO();
                LOGI_ConfiguracionD365_INFO oConfig = new LOGI_ConfiguracionD365_INFO();
                List<LOGI_Plantilla_INFO> lstAsistentes = new List<LOGI_Plantilla_INFO>();
                oParam.FolioAsistentemath = Asistente;
                oParam.activo = -1;
                response = new LOGI_Plantilla_PD(oTools.CONST_CONNECTION).ListaPlantillas("", oParam, ref lstAsistentes);
                if (response.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    oParam = lstAsistentes[0];
                    response = new LOGI_ConfiguracionD365_PD(oTools.CONST_CONNECTION).ListaConfiguracion("", ref oConfig);
                    if (response.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string sPath = string.Format(@"{0}\tmp\output", AppDomain.CurrentDomain.BaseDirectory);
                        if (!Directory.Exists(sPath))
                            Directory.CreateDirectory(sPath);
                        string sFullPath = string.Format(@"{0}\{1}_{2}.zip",sPath, oParam.FolioAsistente, DateTime.Now.ToString("ddMMyyyyHHmm"));
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddFile(string.Format(@"{0}\{1}", oConfig.plantilla, oParam.pathcabecera), Asistente);
                            zip.AddFile(string.Format(@"{0}\{1}", oConfig.plantilla, oParam.pathdetalle), Asistente);

                            if (zip.Entries.Count > 0)
                            {
                                zip.Comment = oParam.plantillanom;
                                zip.Save(sFullPath);
                            }

                        }
                        m_OutputZIP(string.Format("{0}_{1}.zip", oParam.FolioAsistente, DateTime.Now.ToString("ddMMyyyyHHmm")), sFullPath);
                    }
                    else throw new Exception("La información del directorio no se encuentra disponible");

                }
                else throw new Exception("No se ha podido obtener la información del asistente");

            }
            catch (Exception ex)
            {
                this.m_MessageInterfaz(string.Format("No se ha podido generar la información. ERROR: {0}", ex.Message), "error");
            }
        }


        public string m_GuardaDataSession(LOGI_Tablainformacion_INFO contenidoArchivo, string Asistente)
        {
            LOGI_WebTools_PD oTools = new LOGI_WebTools_PD();
            string response = string.Empty;
            int iheaders = contenidoArchivo.lstHeader.Count;
            int ivalues = contenidoArchivo.lstValues[0].Count;
            string asistente_usuario = string.Format("{0}-{1}", oTools.UsuarioSession().iUsuario, DateTime.Now.ToString("ddMMyyyyHHmm"));

            if (iheaders == ivalues)
            {
                response = string.Format("OK;{0}", asistente_usuario);
                Session["dataExportGrid_" + asistente_usuario] = contenidoArchivo;
                Session["titleAsistente_" + asistente_usuario] = Asistente;
            }
            else response = "ERROR;";

            return response;
        }

        void GenerareporteZAPDimensiones(string Archivo, LOGI_Extraccion_ZAP_INFO oCuenta)
        {
            try
            {

                LOGI_Tools_PD otollog = new LOGI_Tools_PD();

                //otollog.LogProceso(String.Format("FEchas: {0} - {1}", oCuenta.fecha_inicio,oCuenta.fecha_final), "GenerareporteZAPDimensiones", "", "", "");


                LOGI_WebTools_PD oTools = new LOGI_WebTools_PD();
                List<LOGI_Extraccion_ZAP_INFO> lstcuentas = new List<LOGI_Extraccion_ZAP_INFO>();
                //otollog.LogProceso("INICIANDO", "GenerareporteZAPDimensiones", "", "", "");
                new LOGI_Extraccion_ZAP_PD(oTools.CONST_ZAP_CONNECTION).Listacuentas(ref lstcuentas, oCuenta);
                if (lstcuentas.Count > 0)
                {
                    //otollog.LogProceso("REGISTROS", "GenerareporteZAPDimensiones", "", "", "");

                    string sPath = string.Format(@"{0}\tmp\output", AppDomain.CurrentDomain.BaseDirectory);
                    try
                    {
                        DateTime oTime = DateTime.Now.AddDays(-8);
                        DirectoryInfo oDir = new DirectoryInfo(sPath);
                        FileInfo[] oFiles = oDir.GetFiles().OrderBy(x => x.CreationTime).ToArray();
                        foreach (FileInfo oFile in oFiles)
                        {
                            if (oFile.CreationTime <= oTime)
                            {
                                try
                                {
                                    //eliminamos el archivo 
                                    oFile.Delete();
                                }
                                catch { }
                            }
                            else continue;
                        }
                    }
                    catch { }
                    //otollog.LogProceso("ARMANDO FILE", "GenerareporteZAPDimensiones", "", "", "");

                    string sNameFile = string.Format(@"{0}{1}.xls", Archivo, DateTime.Now.ToString("ddMMyyyyHHmm"));
                    string sFullpath = string.Format(@"{0}\{1}", sPath, sNameFile);
                    ExcelPackage oPack = new ExcelPackage(new FileInfo(sFullpath));
                    ExcelWorksheet oWork = oPack.Workbook.Worksheets.Add(Archivo);
                    if (!Directory.Exists(sPath))
                        Directory.CreateDirectory(sPath);
                    int iColumn = 1;
                    char cAlpha = 'A';
                    oWork.Cells["A1"].Value = "FECHA DE REGISTRO";
                    oWork.Cells["B1"].Value = "NÚMERO DE DIARIO";
                    oWork.Cells["C1"].Value = "DIMENSIONES";
                    oWork.Cells["D1"].Value = "CUENTA CONTABLE";
                    oWork.Cells["E1"].Value = "SUCURSAL";
                    oWork.Cells["F1"].Value = "FILIAL";
                    oWork.Cells["G1"].Value = "CENTRO DE COSTO";
                    oWork.Cells["H1"].Value = "DEPARTAMENTO";
                    oWork.Cells["I1"].Value = "DESCRIPCIÓN";
                    oWork.Cells["J1"].Value = "VEHÍCULO";
                    oWork.Cells["K1"].Value = "TOTAL";
                    iColumn = 2;
                    cAlpha = 'A';
                    //otollog.LogProceso("CABECERA", "GenerareporteZAPDimensiones", "", "", "");


                    foreach (LOGI_Extraccion_ZAP_INFO data in lstcuentas)
                    {
                        //otollog.LogProceso("CICLO", "GenerareporteZAPDimensiones", "", "", "");

                        oWork.Cells[string.Format("A{0}", iColumn)].Value = data.Fecha;
                        oWork.Cells[string.Format("B{0}", iColumn)].Value = data.documento;
                        oWork.Cells[string.Format("C{0}", iColumn)].Value = data.Display;
                        oWork.Cells[string.Format("D{0}", iColumn)].Value = data.cuenta;
                        oWork.Cells[string.Format("E{0}", iColumn)].Value = data.sucursal;
                        oWork.Cells[string.Format("F{0}", iColumn)].Value = data.filial;
                        oWork.Cells[string.Format("G{0}", iColumn)].Value = data.centro;
                        oWork.Cells[string.Format("H{0}", iColumn)].Value = data.depto;
                        oWork.Cells[string.Format("I{0}", iColumn)].Value = data.text;
                        oWork.Cells[string.Format("J{0}", iColumn)].Value = data.vehiculo;
                        oWork.Cells[string.Format("K{0}", iColumn)].Value = data.debito.ToString();
                        //otollog.LogProceso("FIN DE CICLO", "GenerareporteZAPDimensiones", "", "", "");

                        cAlpha++;
                        iColumn++;
                    }
                    //otollog.LogProceso("GRABANDO", "GenerareporteZAPDimensiones", "", "", "");

                    oWork.Cells["A:K"].AutoFitColumns();
                    oPack.Save();
                    m_OutputExcel(sNameFile, sFullpath);

                    try
                    {
                        File.Delete(sFullpath);
                    }
                    catch { }

                }
                else
                {
                    //otollog.LogProceso("SIN REGISTROS", "GenerareporteZAPDimensiones", "", "", "");

                    this.m_MessageInterfaz(string.Format("No se ha podido generar la información"), "error");
                }
            }
            catch (Exception ex)
            {

                this.m_MessageInterfaz(string.Format("No se ha podido generar la información. ERROR: {0}", ex.Message), "error");

            }
        }

        void DescargArchivo(string Archivo)
        {
            try
            {
                Archivo = LOGI_WebTools_PD.Base64Decode(Archivo);
                Archivo = LOGI_Rijndael_PD.DecryptRijndael(Archivo);
                   FileInfo oFile = new FileInfo(@Archivo);
                m_OutputExcel(oFile.Name, oFile.FullName);
            }
            catch (Exception ex)
            {
                this.m_MessageInterfaz(string.Format("No se ha podido generar la información. ERROR: {0}", ex.Message), "error");

            }
        }

        void m_MessageInterfaz(string sMessage, string sIcon)
        {
            string sIconType = string.Empty;
            if (sIcon.Equals("error", StringComparison.InvariantCultureIgnoreCase))
                sIconType = "<i class=\"fa fa-exclamation-triangle fa-2x\" style=\"cursor:pointer; color:red;\"></i>";
            else if (sIcon.Equals("warning", StringComparison.InvariantCultureIgnoreCase))
                sIconType = "<i class=\"fa fa-exclamation-triangle fa-2x\" style=\"cursor:pointer; color:orange;\" ></i>";
            else sIconType = "<i class=\"icon-ok-sign fa-2x\" style=\"cursor:pointer; color:green;\" ></i>";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { $('#div-icon-loader').html('" + sIconType + "'); $('#lbl-mensaje').html('" + sMessage + "'); });", true);

        }
        private void m_OutputTXT(String sNombre, String sFullpath)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/xml";// "application/ms-word";
            Response.Expires = 0;
            Response.AddHeader("Program", "no-cache");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + sNombre);
            Response.Write(sw.ToString());
            Response.WriteFile(sFullpath);
            Response.End();
        }
        private void m_OutputExcel(String sNombre, String sFullpath)
        {
            try
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.Expires = 0;
                Response.AddHeader("Program", "no-cache");
                Response.AddHeader("Content-Disposition", "filename=" + sNombre);
                Response.Write(sw.ToString());
                Response.WriteFile(sFullpath);
                Response.End();
            }
            catch { }
        }

        private void m_OutputZIP(String nombre, string sRutaArchivo)
        {
            Response.Clear();
            Response.ClearContent();
            Response.Buffer = true;
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Expires = 0;
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Content-Disposition", "filename=" + nombre);
            Response.TransmitFile(sRutaArchivo);
            Response.Flush();
            Response.End();
        }

    }

}
