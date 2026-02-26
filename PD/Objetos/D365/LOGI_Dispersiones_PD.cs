using AD;
using AD.Objetos.D365;
using AD.Tablas.D365;
using AD.Tablas.EQUIV;
using INFO.Objetos.D365;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using OfficeOpenXml;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Objetos.D365
{
    public class LOGI_Dispersiones_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Dispersiones_AD oDispersiones = null;
        const string CONST_CLASE = "LOGI_Dispersiones_PD.cs";
        const string CONST_MODULO = "Dispersiones";

        public LOGI_Dispersiones_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oDispersiones = new  LOGI_Dispersiones_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string Listadispersiones(string sUsuarioID, string sConnectionEquiv, LOGI_Dispersion_INFO oParam, ref List<LOGI_Dispersion_INFO> lstDispersiones)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                LOGI_ConexionSql_AD oConnEquiv = new LOGI_ConexionSql_AD(sConnectionEquiv);
                List<LOGI_Catalogos_INFO> lstCat = new List<LOGI_Catalogos_INFO>();
                LOGI_Catalogos_INFO otemp = new LOGI_Catalogos_INFO();
                LOGI_ConfiguracionD365_INFO oConfig = new LOGI_ConfiguracionD365_INFO();
                sReponse = new LOGI_ConfiguracionD365_AD().ListaConfiguracion(ref oConnection, ref oConfig, out sConsultaSql);
                if (sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(oConfig.cuenta_uno) || !string.IsNullOrEmpty(oConfig.cuenta_dos))
                    {
                        sReponse = oDispersiones.ListaDispersiones(ref oConnection, ref lstDispersiones, oParam, out sConsultaSql);
                        if (sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var lstSucursales = lstDispersiones.GroupBy(x => new { x.cia, x.suc }).ToList();
                            string ACCUNT_DISPLAY = oParam.tipo == 1 ? oConfig.cuenta_uno : oConfig.cuenta_dos;
                            string DEFAULT_DIM = string.Empty;
                            string FILIA_DIM = string.Empty;
                            otemp = new LOGI_Catalogos_INFO();
                            otemp.iCuenta = 624;
                            otemp.iSubcuenta = 67;
                            otemp.iEmpresa = 67;
                            new LOGI_Filialesterceros_AD().RecuperaFilieales(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                            if (lstCat.Count == 1)
                                FILIA_DIM = lstCat[0].sAX365;

                            foreach (var o in lstSucursales)
                            {
                                if (o.Key == null)
                                    continue;
                                lstCat = new List<LOGI_Catalogos_INFO>();
                                otemp = new LOGI_Catalogos_INFO();
                                otemp.iCuenta = o.Key.cia;
                                otemp.iSubcuenta = o.Key.suc;
                                otemp.iEmpresa = 67;
                                otemp.iActivo = 1;
                                new LOGI_Sucursales_AD().RecuperaSucursales(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                                if (lstCat.Count == 1)
                                {
                                    lstDispersiones.Where(x => x.cia == o.Key.cia && x.suc == o.Key.suc).ToList().ForEach(x =>
                                    {
                                        x.AccDisplay = string.Format("{0}-{1}-----{2}-----", ACCUNT_DISPLAY, lstCat[0].sAX365, FILIA_DIM);
                                        x.Defaultdim = string.Format("-{0}-----{1}-----", lstCat[0].sAX365, FILIA_DIM);
                                    });
                                }
                            }
                            lstDispersiones.ForEach(x =>
                            {
                                x.valido = 1;
                                string cadena = string.Empty;
                                if (string.IsNullOrEmpty(x.AccDisplay))
                                    cadena = "Sin cuenta contable. ";
                                if (string.IsNullOrEmpty(x.Defaultdim))
                                    cadena = string.Format("{0}Sin dimensiones. ", cadena);
                                if (string.IsNullOrEmpty(x.rfc))
                                    cadena = string.Format("{0}Sin RFC de operador. ", cadena);
                                if (string.IsNullOrEmpty(x.cuenta))
                                    cadena = string.Format("{0}Sin cuenta bancaria. ", cadena);
                                if (!string.IsNullOrEmpty(cadena))
                                {
                                    x.valido = 0;
                                    x.mensaje = string.Format("Error en equivalencias. {0}", cadena);
                                    if (!cadena.Contains("dimensiones") && !cadena.Contains("RFC"))
                                    {
                                        x.valido = 2;
                                        x.mensaje = cadena;
                                    }
                                }

                            });
                        }

                    }
                    else throw new Exception("No se encontró la configuración para las cuentas contables de dispersión y fondo fijo");
                }
                else throw new Exception("No se encontró la configuración para las cuentas contables de dispersión y fondo fijo");
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Listadispersiones", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string CreaAsistente(string sUsuarioID, string Asistente, int tipo, List<LOGI_Dispersion_INFO> lstPolizas, ref string FolioAsistente)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty, sFullpathLine = string.Empty, sFullpathHead = string.Empty;
            Int32 Totales = -1;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                oConnection.StarTransacction();
                LOGI_ConfiguracionD365_INFO oCnfparam = new LOGI_ConfiguracionD365_INFO();
                LOGI_ConfiguracionD365_AD oConfiguracion = new LOGI_ConfiguracionD365_AD();
                LOGI_Plantilla_AD oPlantilla = new LOGI_Plantilla_AD();
                sReponse = oConfiguracion.ListaConfiguracion(ref oConnection, ref oCnfparam, out sConsultaSql);
                if (sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (Directory.Exists(oCnfparam.plantilla))
                    {
                        if (string.IsNullOrEmpty(oCnfparam.ciad365))
                            throw new Exception("No se ha configurado el nombre de la empresa D365");
                        if (string.IsNullOrEmpty(oCnfparam.aprobadordisper))
                            throw new Exception("No se ha configurado el aprobador de dispersiones D365");

                        oCnfparam.ciad365 = LOGI_Rijndael_PD.DecryptRijndael(oCnfparam.ciad365);
                        oCnfparam.aprobadordisper = LOGI_Rijndael_PD.DecryptRijndael(oCnfparam.aprobadordisper);
                        #region "GENERA ARCHIVO EXCEL CONTENIDO DETALLE"
                        string sFoldermes = DateTime.Now.ToString("MMyyyy");
                        string sPath = string.Format(@"{0}\{1}", oCnfparam.plantilla, sFoldermes);
                        string sNameLine = string.Format(@"{0}{1}.xls", tipo==1 ? "LINPAGO":"LINFA", DateTime.Now.ToString("ddMMyyyyHHmm"));
                        string sNameHeader = string.Format(@"{0}{1}.xls",tipo == 1 ? "CABPAGO" : "CABFA", DateTime.Now.ToString("ddMMyyyyHHmm"));
                        sFullpathLine = string.Format(@"{0}\{1}", sPath, sNameLine);
                        sFullpathHead = string.Format(@"{0}\{1}", sPath, sNameHeader);
                        if (!Directory.Exists(sPath))
                            Directory.CreateDirectory(sPath);
                        if (File.Exists(sFullpathLine))
                            throw new Exception("Ya se ha generado una plantilla anteriormente con los mismos datos favor de reintentarlo");
                        if (File.Exists(sFullpathHead))
                            throw new Exception("Ya se ha generado una plantilla anteriormente con los mismos datos favor de reintentarlo");

                        ExcelPackage oPackHead = new ExcelPackage(new FileInfo(sFullpathHead));
                        ExcelWorksheet oWorkHead = null;

                        ExcelPackage oPackLine = new ExcelPackage(new FileInfo(sFullpathLine));
                        ExcelWorksheet oWork = null;

                        if (tipo == 1)
                        {
                            oWork  = oPackLine.Workbook.Worksheets.Add("Vendor_payment_journal_line");
                            oWorkHead = oPackHead.Workbook.Worksheets.Add("Vendor_payment_journal_header");
                            this.ArmaExcelDispersion(Asistente, ref oWork, ref oPackLine, ref oWorkHead, ref oPackHead, lstPolizas, oCnfparam);
                        }
                        else
                        {
                            oWork = oPackLine.Workbook.Worksheets.Add("Vendor_invoice_journal_line");
                            oWorkHead = oPackHead.Workbook.Worksheets.Add("Vendor_invoice_journal_header");
                            this.ArmaExcelFondoFijo(Asistente, ref oWork, ref oPackLine, ref oWorkHead, ref oPackHead, lstPolizas, oCnfparam);
                        }


                        #endregion "GENERA ARCHIVO EXCEL CONTENIDO DETALLE"

                        sReponse = oPlantilla.CuentaExistencias(ref oConnection, ref Totales, DateTime.Now.Year, DateTime.Now.Month, out sConsultaSql);
                        if (!sReponse.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            throw new Exception("el folio para el asistente no se pudo generar");
                        FolioAsistente= string.Format("LM.PLA{0}{1}", DateTime.Now.ToString("yyyyMMdd"), Totales);
                        LOGI_Plantilla_INFO odata = new LOGI_Plantilla_INFO();
                        odata.FolioAsistente = FolioAsistente;
                        odata.plantillanom = Asistente;
                        odata.ano = DateTime.Now.Year;
                        odata.mes = DateTime.Now.Month;
                        odata.pathcabecera = string.Format(@"{0}/{1}", sFoldermes, sNameHeader);
                        odata.pathdetalle = string.Format(@"{0}/{1}", sFoldermes, sNameLine);
                        odata.usuariocreo = Convert.ToInt32(sUsuarioID);
                        odata.tipo = tipo;
                        sReponse = oPlantilla.Creaplantilla(ref oConnection, odata, out sConsultaSql);
                        if (!sReponse.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            throw new Exception("el asistente no pudo ser generado");

                        foreach (LOGI_Dispersion_INFO d in lstPolizas)
                        {
                            d.usado = 1;
                            d.FolioAsistente = FolioAsistente;
                            sReponse = oDispersiones.ActualizaPoliza(ref oConnection, d, out sConsultaSql);
                            if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                                throw new Exception("los registros de la poliza no pudieron ser actualizados");
                        }
                        sReponse = "OK";
                    }
                    else throw new Exception("El directorio para guardar la plantilla no existe");
                }
                else throw new Exception("No se ha podido validar la configuración del repositorio");
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "CreaAsistente", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido crear la plantilla {0}", ex.Message);
                if (!string.IsNullOrEmpty(sFullpathHead))
                {
                    if (File.Exists(sFullpathHead))
                        File.Delete(sFullpathHead);
                }
                if (!string.IsNullOrEmpty(sFullpathLine))
                {
                    if (File.Exists(sFullpathLine))
                        File.Delete(sFullpathLine);
                }
                oConnection.RollBackTransacction();
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        void ArmaExcelDispersion(string Asistente, ref ExcelWorksheet oWork, ref ExcelPackage oPack, ref ExcelWorksheet oWorkHead, ref ExcelPackage oPackHead, List<LOGI_Dispersion_INFO> lstPolizas, LOGI_ConfiguracionD365_INFO oCnfparam)
        {
            if (string.IsNullOrEmpty(oCnfparam.diariodisp))
                throw new Exception("No se ha establecido el diario D365 para la dispersión");
            string DateCellFormat = "dd/MM/yyyy";
            

            #region "DOCUMENTO DE CABECERA"                        
            oWorkHead.Cells["A1"].Value = "JOURNALBATCHNUMBER";
            oWorkHead.Cells["B1"].Value = "DESCRIPTION";
            oWorkHead.Cells["C1"].Value = "JOURNALNAME";
            oWorkHead.Cells["A2"].Value = "1";
            oWorkHead.Cells["B2"].Value = Asistente;
            oWorkHead.Cells["C2"].Value = oCnfparam.diariodisp;// "368DISOPER";
            oWorkHead.Cells[string.Format("A:C")].AutoFitColumns();
            oPackHead.Save();
            #endregion "DOCUMENTO DE CABECERA"                     

            #region "CABECERA PARA PLANTILLA"                        
            oWork.Cells["A1"].Value = "TRANSACTIONDATE";
            oWork.Cells["B1"].Value = "VOUCHER";
            oWork.Cells["C1"].Value = "COMPANY";
            oWork.Cells["D1"].Value = "ACCOUNTTYPE";
            oWork.Cells["E1"].Value = "ACCOUNTDISPLAYVALUE";
            oWork.Cells["F1"].Value = "DEFAULTDIMENSIONSFORACCOUNTDISPLAYVALUE";
            oWork.Cells["G1"].Value = "BANKTRANSACTIONTYPE";
            oWork.Cells["H1"].Value = "MARKEDINVOICECOMPANY";
            oWork.Cells["I1"].Value = "MARKEDINVOICE";
            oWork.Cells["J1"].Value = "TRANSACTIONTEXT";
            oWork.Cells["K1"].Value = "CURRENCYCODE";
            oWork.Cells["L1"].Value = "EXCHANGERATE";
            oWork.Cells["M1"].Value = "GRWEXCHRATE";
            oWork.Cells["N1"].Value = "GRWMULTIEXCHRATE";
            oWork.Cells["O1"].Value = "DEBITAMOUNT";
            oWork.Cells["P1"].Value = "CREDITAMOUNT";
            oWork.Cells["Q1"].Value = "PAYMENTREFERENCE";
            oWork.Cells["R1"].Value = "OFFSETACCOUNTTYPE";
            oWork.Cells["S1"].Value = "OFFSETACCOUNTDISPLAYVALUE";
            oWork.Cells["T1"].Value = "DEFAULTDIMENSIONSFOROFFSETACCOUNTDISPLAYVALUE";
            oWork.Cells["U1"].Value = "OFFSETCOMPANY";
            oWork.Cells["V1"].Value = "PAYMENTMETHODNAME";
            oWork.Cells["W1"].Value = "PAYMENTSPECIFICATION";
            oWork.Cells["X1"].Value = "THIRDPARTYBANKACCOUNTID";
            oWork.Cells["Y1"].Value = "ISPREPAYMENT";
            oWork.Cells["Z1"].Value = "JOURNALBATCHNUMBER";
            oWork.Cells["AA1"].Value = "LINENUMBER";
            oWork.Cells["AB1"].Value = "POSTINGPROFILE";
            oWork.Cells["AC1"].Value = "TAXGROUP";
            oWork.Cells["AD1"].Value = "TAXITEMGROUP";
            oWork.Cells["AE1"].Value = "GRWINVOICEIDS";
            oWork.Cells["AF1"].Value = "SETTLEVOUCHER";
            oWork.Cells["AG1"].Value = "RFC";
            oWork.Cells["AH1"].Value = "BENEFICIARIO";
            oWork.Cells["AI1"].Value = "BancoDestinoNac";
            oWork.Cells["AJ1"].Value = "CuentaDestinoNac";
            #endregion "CABECERA PARA PLANTILLA"
            #region "DETALLE DE PLANTILLA"                
            int colum = 2;
            int contador = 1;
            foreach (LOGI_Dispersion_INFO data in lstPolizas)
            {

                //oWork.Cells[string.Format("A{0}", colum)].Value = data.fecha;
                //oWork.Cells[string.Format("A{0}", colum)].Value = data.fecha;
                using (ExcelRange Rng = oWork.Cells[string.Format("A{0}", colum)])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                    Rng.Value = Convert.ToDateTime(data.fecha);
                }

                
                //oWork.Cells[string.Format("A{0}", colum)].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                oWork.Cells[string.Format("B{0}", colum)].Value = contador.ToString();
                oWork.Cells[string.Format("C{0}", colum)].Value = oCnfparam.ciad365;
                oWork.Cells[string.Format("D{0}", colum)].Value = "ledger";
                oWork.Cells[string.Format("E{0}", colum)].Value = data.AccDisplay;
                oWork.Cells[string.Format("H{0}", colum)].Value = oCnfparam.ciad365;
                oWork.Cells[string.Format("J{0}", colum)].Value = data.descrip;
                oWork.Cells[string.Format("K{0}", colum)].Value = "MXN";
                oWork.Cells[string.Format("L{0}", colum)].Value = "1";
                oWork.Cells[string.Format("O{0}", colum)].Value = data.cargo.ToString();

                oWork.Cells[string.Format("P{0}", colum)].Value = "0";

                oWork.Cells[string.Format("Q{0}", colum)].Value = data.refbanco.Trim();
                oWork.Cells[string.Format("R{0}", colum)].Value = "Banco";

                oWork.Cells[string.Format("S{0}", colum)].Value = data.nambanco;

                oWork.Cells[string.Format("T{0}", colum)].Value = data.Defaultdim;
                oWork.Cells[string.Format("U{0}", colum)].Value = oCnfparam.ciad365;
                oWork.Cells[string.Format("V{0}", colum)].Value = "03";
                oWork.Cells[string.Format("Y{0}", colum)].Value = "No";
                oWork.Cells[string.Format("Z{0}", colum)].Value = "1";

                oWork.Cells[string.Format("AA{0}", colum)].Value = contador.ToString();
                oWork.Cells[string.Format("AB{0}", colum)].Value = "GEN";

                oWork.Cells[string.Format("AG{0}", colum)].Value = data.rfc;
                oWork.Cells[string.Format("AH{0}", colum)].Value = data.operador;
                oWork.Cells[string.Format("AI{0}", colum)].Value = string.IsNullOrEmpty(data.cuenta) ? "" : data.cuenta.Substring(0, 3);
                oWork.Cells[string.Format("AJ{0}", colum)].Value = data.cuenta;
                contador++;
                colum++;
            }
            oWork.Cells[string.Format("A:AZ")].AutoFitColumns();
            oPack.Save();
            #endregion "DETALLE DE PLANTILLA"
        }

        void ArmaExcelFondoFijo(string Asistente, ref ExcelWorksheet oWork, ref ExcelPackage oPack, ref ExcelWorksheet oWorkHead, ref ExcelPackage oPackHead, List<LOGI_Dispersion_INFO> lstPolizas, LOGI_ConfiguracionD365_INFO oCnfparam)
        {
            if (string.IsNullOrEmpty(oCnfparam.diariofondo))
                throw new Exception("No se ha establecido el diario D365 para el fondo fijo");
            string DateCellFormat = "dd/MM/yyyy";


            #region "DOCUMENTO DE CABECERA"                        
            oWorkHead.Cells["A1"].Value = "JOURNALBATCHNUMBER";
            oWorkHead.Cells["B1"].Value = "DESCRIPTION";
            oWorkHead.Cells["C1"].Value = "JOURNALNAME";
            oWorkHead.Cells["D1"].Value = "SALESTAXINCLUDED";
            oWorkHead.Cells["A2"].Value = "1";
            oWorkHead.Cells["B2"].Value = Asistente;
            oWorkHead.Cells["C2"].Value = oCnfparam.diariofondo;//"350FFOPER";
            oWorkHead.Cells["D2"].Value = "Yes";
            oWorkHead.Cells[string.Format("A:D")].AutoFitColumns();
            oPackHead.Save();
            #endregion "DOCUMENTO DE CABECERA"  


            #region "CABECERA PARA PLANTILLA"                        
            oWork.Cells["A1"].Value = "JOURNALBATCHNUMBER";
            oWork.Cells["B1"].Value = "LINENUMBER";
            oWork.Cells["C1"].Value = "ACCOUNTDISPLAYVALUE";
            oWork.Cells["D1"].Value = "ACCOUNTTYPE";
            oWork.Cells["E1"].Value = "APPROVED";
            oWork.Cells["F1"].Value = "APPROVERNUMBER";
            oWork.Cells["G1"].Value = "ASSETID";
            oWork.Cells["H1"].Value = "ASSETTRANSTYPE";
            oWork.Cells["I1"].Value = "BANKACCOUNTID";
            oWork.Cells["J1"].Value = "BOOKID";
            oWork.Cells["K1"].Value = "CASHDISCOUNT";
            oWork.Cells["L1"].Value = "CASHDISCOUNTAMOUNT";
            oWork.Cells["M1"].Value = "CASHDISCOUNTDATE";
            oWork.Cells["N1"].Value = "COMPANY";
            oWork.Cells["O1"].Value = "CREDIT";
            oWork.Cells["P1"].Value = "CURRENCY";
            oWork.Cells["Q1"].Value = "CUSTVENDBANKACCOUNTID";
            oWork.Cells["R1"].Value = "DATE";
            oWork.Cells["S1"].Value = "DEBIT";
            oWork.Cells["T1"].Value = "DEFAULTDIMENSIONDISPLAYVALUE";
            oWork.Cells["U1"].Value = "DESCRIPTION";
            oWork.Cells["V1"].Value = "DOCUMENT";
            oWork.Cells["W1"].Value = "DUEDATE";
            oWork.Cells["X1"].Value = "EXCHRATE";
            oWork.Cells["Y1"].Value = "EXCHRATESECOND";
            oWork.Cells["Z1"].Value = "INVOICE";
            oWork.Cells["AA1"].Value = "INVOICEDATE";
            oWork.Cells["AB1"].Value = "ITEMSALESTAXGROUP";
            oWork.Cells["AC1"].Value = "METHODOFPAYMENT";
            oWork.Cells["AD1"].Value = "OFFSETACCOUNTDISPLAYVALUE";
            oWork.Cells["AE1"].Value = "OFFSETACCOUNTTYPE";
            oWork.Cells["AF1"].Value = "OFFSETCOMPANY";
            oWork.Cells["AG1"].Value = "OFFSETTRANSACTIONTEXT";
            oWork.Cells["AH1"].Value = "PAYMENTSPECIFICATION";
            oWork.Cells["AI1"].Value = "PAYMID";
            oWork.Cells["AJ1"].Value = "POSTINGPROFILE";
            oWork.Cells["AK1"].Value = "REMITTANCEADDRESSCITY";
            oWork.Cells["AL1"].Value = "REMITTANCEADDRESSCOUNTRY";
            oWork.Cells["AM1"].Value = "REMITTANCEADDRESSCOUNTRYISOCODE";
            oWork.Cells["AN1"].Value = "REMITTANCEADDRESSCOUNTY";
            oWork.Cells["AO1"].Value = "REMITTANCEADDRESSDESCRIPTION";
            oWork.Cells["AP1"].Value = "REMITTANCEADDRESSDISTRICTNAME";
            oWork.Cells["AQ1"].Value = "REMITTANCEADDRESSLATITUDE";
            oWork.Cells["AR1"].Value = "REMITTANCEADDRESSLOCATIONID";
            oWork.Cells["AS1"].Value = "REMITTANCEADDRESSLONGITUDE";
            oWork.Cells["AT1"].Value = "REMITTANCEADDRESSSTATE";
            oWork.Cells["AU1"].Value = "REMITTANCEADDRESSSTREET";
            oWork.Cells["AV1"].Value = "REMITTANCEADDRESSTIMEZONE";
            oWork.Cells["AW1"].Value = "REMITTANCEADDRESSVALIDFROM";
            oWork.Cells["AX1"].Value = "REMITTANCEADDRESSVALIDTO";
            oWork.Cells["AY1"].Value = "REMITTANCEADDRESSZIPCODE";
            oWork.Cells["AZ1"].Value = "REPORTINGCURRENCYEXCHRATE";
            oWork.Cells["BA1"].Value = "SALESTAXGROUP";
            oWork.Cells["BB1"].Value = "TAXEXEMPTNUMBER";
            oWork.Cells["BC1"].Value = "TERMSOFPAYMENT";
            oWork.Cells["BD1"].Value = "TRANSACTIONTYPE";
            oWork.Cells["BE1"].Value = "TYPEOFOPERATION";
            oWork.Cells["BF1"].Value = "UUID";
            oWork.Cells["BG1"].Value = "VOUCHER";
            oWork.Cells["BH1"].Value = "TTSINCOMPROBANTE";
            #endregion "CABECERA PARA PLANTILLA"
            #region "DETALLE DE PLANTILLA"                
            int colum = 2;
            int contador = 1;
            int vaucher = 1;
            foreach (LOGI_Dispersion_INFO data in lstPolizas)
            {
                #region "LINEAS DE DEBITO"
                oWork.Cells[string.Format("A{0}", colum)].Value = "1";
                oWork.Cells[string.Format("B{0}", colum)].Value = contador.ToString();
                oWork.Cells[string.Format("C{0}", colum)].Value = data.AccDisplay.Trim();
                oWork.Cells[string.Format("D{0}", colum)].Value = "ledger";
                oWork.Cells[string.Format("E{0}", colum)].Value = "Yes";
                oWork.Cells[string.Format("F{0}", colum)].Value = oCnfparam.aprobadordisper;
                oWork.Cells[string.Format("N{0}", colum)].Value = oCnfparam.ciad365;
                oWork.Cells[string.Format("P{0}", colum)].Value = "MXN";
                using (ExcelRange Rng = oWork.Cells[string.Format("R{0}", colum)])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                    Rng.Value = Convert.ToDateTime(data.fecha);
                }
                //oWork.Cells[string.Format("R{0}", colum)].Value = data.fecha;
                //oWork.Cells[string.Format("R{0}", colum)].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;;
                oWork.Cells[string.Format("S{0}", colum)].Value = data.cargo.ToString();
                //oWork.Cells[string.Format("T{0}", colum)].Value = data.Defaultdim;
                oWork.Cells[string.Format("U{0}", colum)].Value = data.descrip;
                oWork.Cells[string.Format("X{0}", colum)].Value = "1";
                oWork.Cells[string.Format("Z{0}", colum)].Value = string.Format("{0}{1}{2}", data.ano, data.mes, data.poliza);
                //oWork.Cells[string.Format("AA{0}", colum)].Value = data.fecha;
                //oWork.Cells[string.Format("AA{0}", colum)].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;;
                using (ExcelRange Rng = oWork.Cells[string.Format("AA{0}", colum)])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                    Rng.Value = Convert.ToDateTime(data.fecha);
                }
                oWork.Cells[string.Format("AC{0}", colum)].Value = "01";
                oWork.Cells[string.Format("AE{0}", colum)].Value = "Ledger";
                oWork.Cells[string.Format("AF{0}", colum)].Value = oCnfparam.ciad365;
                oWork.Cells[string.Format("AJ{0}", colum)].Value = "GEN";
                oWork.Cells[string.Format("BD{0}", colum)].Value = "Vend";
                oWork.Cells[string.Format("BG{0}", colum)].Value = vaucher.ToString();
                oWork.Cells[string.Format("BH{0}", colum)].Value = "Yes";
                contador++;
                colum++;
                #endregion "LINEAS DE DEBITO"

                #region "LINEAS DE CREDITO"
                oWork.Cells[string.Format("A{0}", colum)].Value = "1";
                oWork.Cells[string.Format("B{0}", colum)].Value = contador.ToString();
                oWork.Cells[string.Format("C{0}", colum)].Value = data.FondoFijo.Trim();
                oWork.Cells[string.Format("D{0}", colum)].Value = "Vend";
                oWork.Cells[string.Format("E{0}", colum)].Value = "Yes";
                oWork.Cells[string.Format("F{0}", colum)].Value = oCnfparam.aprobadordisper;
                oWork.Cells[string.Format("N{0}", colum)].Value = oCnfparam.ciad365;
                oWork.Cells[string.Format("O{0}", colum)].Value = data.cargo.ToString();
                oWork.Cells[string.Format("P{0}", colum)].Value = "MXN";
                //oWork.Cells[string.Format("R{0}", colum)].Value = data.fecha;
                //oWork.Cells[string.Format("R{0}", colum)].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;;
                using (ExcelRange Rng = oWork.Cells[string.Format("R{0}", colum)])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                    Rng.Value = Convert.ToDateTime(data.fecha);
                }
                oWork.Cells[string.Format("T{0}", colum)].Value = data.Defaultdim;
                oWork.Cells[string.Format("U{0}", colum)].Value = data.descrip;
                oWork.Cells[string.Format("X{0}", colum)].Value = "1";
                oWork.Cells[string.Format("Z{0}", colum)].Value = string.Format("{0}{1}{2}", data.ano, data.mes, data.poliza);
                //oWork.Cells[string.Format("AA{0}", colum)].Value = data.fecha;
                //oWork.Cells[string.Format("AA{0}", colum)].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;;
                using (ExcelRange Rng = oWork.Cells[string.Format("AA{0}", colum)])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                    Rng.Value = Convert.ToDateTime(data.fecha);
                }
                oWork.Cells[string.Format("AC{0}", colum)].Value = "01";
                oWork.Cells[string.Format("AE{0}", colum)].Value = "Ledger";
                oWork.Cells[string.Format("AF{0}", colum)].Value = oCnfparam.ciad365;
                oWork.Cells[string.Format("AJ{0}", colum)].Value = "GEN";
                oWork.Cells[string.Format("BD{0}", colum)].Value = "Vend";
                oWork.Cells[string.Format("BG{0}", colum)].Value = vaucher.ToString();
                oWork.Cells[string.Format("BH{0}", colum)].Value = "Yes";
                contador++;
                colum++;
                #endregion "LINEAS DE CREDITO"
                vaucher++;
            }
            oWork.Cells[string.Format("A:AZ")].AutoFitColumns();
            oPack.Save();
            #endregion "DETALLE DE PLANTILLA"
        }
    }
}
