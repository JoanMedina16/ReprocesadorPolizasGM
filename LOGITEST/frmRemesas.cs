using AD;
using D365.Helpers.D365FOBBICxCServices;
using D365.INFOD;
using INFO.Tablas.D365;
using Newtonsoft.Json;
using OfficeOpenXml;
using PD.Herramientas;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOGITEST
{
    public partial class frmRemesas : Form
    {
        internal string CONST_CONNECTION = @"Data Source=10.20.128.149;Initial Catalog=admlog04;Persist Security Info=True;User ID=usr_pbasbil;Password=desarrollo;Connection Timeout=120";
        internal RestClient oCliente = null;
        internal RestRequest oRequest = null;
        internal IRestResponse oResponse = null;
        internal List<LOGI_REMESA_INFO> lstSeleccionados;
        Dictionary<string, string> lstBodegas = null;
        internal string sMensaje { get; set; }
        public frmRemesas()
        {
            InitializeComponent();
        }

        private void frmRemesas_Load(object sender, EventArgs e)
        {
            dtinicio.Value = DateTime.Now.AddDays(-7);
             lstBodegas = new Dictionary<string, string>();
            lstBodegas.Add("0", "Selecciona la bodega");
            LOGI_ConexionSql_AD oConexion = new LOGI_ConexionSql_AD(CONST_CONNECTION);

            DataSet dsTabla = oConexion.FillDataSet("SELECT * FROM lm_bodegas_bb_tms");
            if (dsTabla.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow orow in dsTabla.Tables[0].Rows)
                {
                    lstBodegas.Add(Convert.ToString(orow["idbodega"]), string.Format("{0}-{1}", Convert.ToString(orow["codigo365"]), Convert.ToString(orow["nombre"])));
                }
                cmborigen.DataSource = new BindingSource(lstBodegas, null);
                cmborigen.DisplayMember = "value";
                cmborigen.ValueMember = "key";

                /*cmbdestino.DataSource = new BindingSource(lstBodegas, null);
                cmbdestino.DisplayMember = "value";
                cmbdestino.ValueMember = "key";*/
            }
            oConexion.CloseConnection();
            dataGridView1.AutoGenerateColumns = false;
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            List<LOGI_REMESA_INFO> lstREMESAS = ListaMovimientoREMESAS();
            Cursor.Current = Cursors.WaitCursor;
            if (lstREMESAS.Count > 0)
            {
                dataGridView1.DataSource = lstREMESAS;
            }
            else MessageBox.Show("No existe información de remesas con los filtros proporionados");
            Cursor.Current = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                lstSeleccionados = new List<LOGI_REMESA_INFO>();
                var checkedRows = from DataGridViewRow r in dataGridView1.Rows
                                  where Convert.ToBoolean(r.Cells[0].Value) == true
                                  select r;
                if (checkedRows.Count() > 0)
                {
                    foreach (var row in checkedRows)
                    {
                        LOGI_REMESA_INFO otemp = new LOGI_REMESA_INFO();
                        otemp.Folio = Convert.ToInt32(row.Cells["Folio"].Value.ToString());
                        otemp.Serie = Convert.ToString(row.Cells["Serie"].Value.ToString());
                        otemp.UUIDFiscal = Convert.ToString(row.Cells["UUIDFiscal"].Value.ToString());                        
                        otemp.Total = Convert.ToDecimal(row.Cells["Total"].Value.ToString());
                        otemp.SubTotal = Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
                        otemp.NombreDestinatario = Convert.ToString(row.Cells["NombreDestinatario"].Value.ToString());
                        otemp.FechaFactura = Convert.ToString(row.Cells["FechaFactura"].Value.ToString());
                        lstSeleccionados.Add(otemp);
                    }
                    string cadena = string.Format("Se han detectado un total de {0} facturas seleccionadas, teniendo un total de {1} a remesar. ¿Deseaas generar la remesa en Excel?", lstSeleccionados.Count, lstSeleccionados.Sum(x => x.Total).ToString("N2"));
                    if (MessageBox.Show(cadena, "Generación de remesa",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //TODO: Stuff
                        string sFoldermes = DateTime.Now.ToString("MMyyyy");
                        string sPath = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, sFoldermes);
                        string sNameLine = string.Format(@"{0}{1}.xls", "REMESA_", DateTime.Now.ToString("ddMMyyyyHHmm"));
                        if (!Directory.Exists(sPath))
                            Directory.CreateDirectory(sPath);
                        string sFullpathLine = string.Format(@"{0}\{1}", sPath, sNameLine);
                        ExcelPackage oPackHead = new ExcelPackage(new FileInfo(sFullpathLine));
                        ExcelWorksheet oWorkHead = null;
                        oWorkHead = oPackHead.Workbook.Worksheets.Add("Vendor_invoice_journal_line");
                        this.ArmaExcelcontenidoRemesa(lstSeleccionados, ref oWorkHead, ref oPackHead);
                        Process.Start(sFullpathLine);
                    }
                }
                else MessageBox.Show("No se han encontrado elementos seleccionados para remesar");
            }
            catch (Exception ex)
            { 
            
            }
        }

        List<LOGI_REMESA_INFO> ListaMovimientoREMESAS()
        {
            List<LOGI_REMESA_INFO> lstRemesas = new List<LOGI_REMESA_INFO>();

            try
            {

                // Iterar sobre el rango de fechas
                for (DateTime date = dtinicio.Value; date <= dtfinal.Value; date = date.AddDays(1))
                {
                    //Se recupera la información de los documentos que se pueden interfazar hacia Dynamics D365
                    oCliente = new RestClient("https://procesos_especiales8-spprint.gmtransport.co/GMTERPV8_PROCESOSESPECIALES_WEB/ES/API.awp");
                    oRequest = new RestRequest(Method.POST);
                    oRequest.AddHeader("Content-Type", "application/json");
                    oRequest.AddParameter("OutputFormat", "JSON", ParameterType.GetOrPost);
                    oRequest.AddParameter("RFCEmpresa", "LMA0402275Q6", ParameterType.GetOrPost);
                    oRequest.AddParameter("ApiKey", "jPk1F2ElT9fxM8w", ParameterType.GetOrPost);
                    oRequest.AddParameter("Parametros", string.Format(@"{{ ""Clase"":""ClsProViajes"", ""Metodo"":""GetEntregasSeguimiento"", ""Parametros"":{{ ""dFecha"":""{0}""  }}}}", date.ToString("yyyyMMdd")), ParameterType.GetOrPost);

                    ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
                    oResponse = oCliente.Execute(oRequest);
                    this.sMensaje = oResponse.Content;
                    if (oResponse.StatusCode == HttpStatusCode.OK)
                    {
                        this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);

                        LOGI_RESPOMSEREMESA_INFO oResponse = JsonConvert.DeserializeObject<LOGI_RESPOMSEREMESA_INFO>(this.sMensaje,
                         new JsonSerializerSettings
                         {
                             NullValueHandling = NullValueHandling.Ignore
                         });

                        List<LOGI_REMESA_INFO> lstRemesasFiltro_uno = oResponse.Result.Viajes.Where(x => x.NombreFiscal.Trim().Equals("EMBOTELLADORAS BEPENSA") && x.PendienteFacturar == false).ToList();
                        List<LOGI_REMESA_INFO> lstRemesasFiltro_dos = lstRemesasFiltro_uno.Where(x => x.Remitente == Convert.ToInt32(cmborigen.SelectedValue)).ToList();
                        //List<LOGI_REMESA_INFO> lstRemesasFiltro_tres = lstRemesasFiltro_dos.Where(x => x.Destinatario == Convert.ToInt32(cmbdestino.SelectedValue)).ToList();

 
                        LOGI_ConexionSql_AD oConexion = new LOGI_ConexionSql_AD(CONST_CONNECTION);
                        string sConsultaSql = string.Format(@"select * from lm_facturas_tms ");
                        DataSet dtTabla = oConexion.FillDataSet(sConsultaSql);
                        List<LOGI_REMESA_INFO> lstFoliosUUID = new List<LOGI_REMESA_INFO>();
                        MapeaFoliosUUID(ref lstFoliosUUID, dtTabla);

                        foreach (LOGI_REMESA_INFO o in lstRemesasFiltro_dos)
                        {
                            var oFiltro = lstRemesas.FirstOrDefault(x => x.Serie == o.Serie && x.Folio == o.Folio);
                            if (oFiltro == null)
                            {
                                var oUUID = lstFoliosUUID.FirstOrDefault(x => x.Serie.Trim().Equals(o.Serie) && Convert.ToInt32(x.Folio) == o.Folio);
                                if (oUUID != null)
                                    o.UUIDFiscal = oUUID.UUIDFiscal;
                                try
                                {
                                    o.NombreDestinatario = lstBodegas.FirstOrDefault(x => x.Key.Equals(o.Destinatario.ToString())).Value;
                                }
                                catch { o.NombreDestinatario = "NAAAA001-BODEGA NO CONFIGURADA"; }
                                o.NombreRemitente = cmborigen.Text;
                                lstRemesas.Add(o);
                            }
                        }
                        if (chkuuid.Checked)
                            this.MovimientosdeUUDI(date.ToString("dd/MM/yyyy"), date.ToString("dd/MM/yyyy"));

                    }
                }
                /**/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return lstRemesas;
        }

        public void MovimientosdeUUDI(string FechaInicio, string FechaFinal)
        {
            bool bCreado = false;
            int PROCESADOS = 0, APLICADOS = 0, FALLIDOS = 0;
            string sEndPoint = string.Empty, sERROR = string.Empty, sresponse = string.Empty,
                sJSON = string.Empty, sURLEndPoint = string.Empty, sUUID = string.Empty, sUUIDRef = string.Empty;
            List<BBICargarFacturaCxCContractEnc> lstLotes = null;
             List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
            LOGI_Documentos_INFO oDocumento = null;


            try
            { 

                //Se recupera la información de los documentos que se pueden interfazar hacia Dynamics D365
                oCliente = new RestClient("https://procesos_especiales8-spprint.gmtransport.co/GMTERPV8_PROCESOSESPECIALES_WEB/ES/API.awp");
                oRequest = new RestRequest(Method.POST);
                oRequest.AddHeader("Content-Type", "application/json");
                oRequest.AddParameter("OutputFormat", "JSON", ParameterType.GetOrPost);
                oRequest.AddParameter("RFCEmpresa", "LMA0402275Q6", ParameterType.GetOrPost);
                oRequest.AddParameter("ApiKey", "jPk1F2ElT9fxM8w", ParameterType.GetOrPost);
                oRequest.AddParameter("Parametros", string.Format(@"{{ ""Clase"":""ClsProPolizas"", ""Metodo"":""GetPolizasDetalle"", ""Parametros"":{{ ""dFechaInicial"":""{0}"", ""dFechaFinal"":""{1}""  }}}}", Convert.ToDateTime(FechaInicio).ToString("yyyyMMdd"), Convert.ToDateTime(FechaFinal).ToString("yyyyMMdd")), ParameterType.GetOrPost);
 
                ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
                oResponse = oCliente.Execute(oRequest);
                this.sMensaje = oResponse.Content;
                if (oResponse.StatusCode == HttpStatusCode.OK)
                {
                    this.sMensaje = this.sMensaje.Replace("SIN FOLIO ", string.Empty);//Replace(@"LM\/MEX-", string.Empty).Replace(@"LM\/ARR", string.Empty).Replace(@"LM\/MID", string.Empty).Replace(@"LM\/QRO", string.Empty);


                    LOGI_ResponseTMS_INFO oResponse = JsonConvert.DeserializeObject<LOGI_ResponseTMS_INFO>(this.sMensaje,
                     new JsonSerializerSettings
                     {
                         NullValueHandling = NullValueHandling.Ignore
                     });

                    lstLotes = new List<BBICargarFacturaCxCContractEnc>();


                    lstLotes = oResponse.Result[0].Encabezado.Where(x => x.ProcesoLigado.Equals("ProFacturas", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(x.UUID) && x.UUID.Length > 30 && string.IsNullOrEmpty(x.UUIDRelacionado) ).ToList();
                    if (lstLotes.Count > 0)
                    {
                        LOGI_ConexionSql_AD oConexion = new LOGI_ConexionSql_AD(CONST_CONNECTION);
                        Hashtable oHashParam = new Hashtable();
                        string sConsultaSql = string.Empty;
                        DataSet dtTabla = null;
                        foreach (BBICargarFacturaCxCContractEnc oAsiento in lstLotes)
                        {
                            if (!oAsiento.Descripcion.Contains("EMBOTELLADORAS BEPENSA"))
                                continue;
                            sConsultaSql = string.Format(@"select serie from lm_facturas_tms where folio = {0} and serie = '{1}'", oAsiento.Folio, oAsiento.Serie);
                            dtTabla = oConexion.FillDataSet(sConsultaSql);
                            if (dtTabla.Tables[0].Rows.Count == 0)
                            {
                                oHashParam = new Hashtable();
                                sConsultaSql = string.Format(@"INSERT INTO lm_facturas_tms(folio,serie,uuid)
            VALUES(@folio,@serie,@uuid)");
                                oHashParam.Add("@folio", oAsiento.Folio);
                                oHashParam.Add("@serie", oAsiento.Serie);
                                oHashParam.Add("@uuid", oAsiento.UUID);
                                oConexion.ExecuteCommand(sConsultaSql, oHashParam);
                            }
                        }
                        oConexion.CloseConnection();
                    }
                }
                else
                {
                }

            }
            catch (Exception ex)
            { 
            }
            finally
            {


            }
        }
       void MapeaFoliosUUID(ref List<LOGI_REMESA_INFO> lstFolios, DataSet dsTabla)
        {
            lstFolios = new List<LOGI_REMESA_INFO>();
            LOGI_REMESA_INFO otemp = new LOGI_REMESA_INFO();
            foreach (DataRow row in dsTabla.Tables[0].Rows)
            {
                otemp = new LOGI_REMESA_INFO();
                otemp.Folio = Convert.ToInt32(row["folio"]);
                otemp.Serie = Convert.ToString(row["serie"]);
                otemp.UUIDFiscal = Convert.ToString(row["uuid"]);
                lstFolios.Add(otemp);
            } 
        }
        void ArmaExcelcontenidoRemesa(List<LOGI_REMESA_INFO> lstRemesas, ref ExcelWorksheet oWork, ref ExcelPackage oPack)
        {            

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
            int linea = 1;
            foreach (LOGI_REMESA_INFO data in lstRemesas)
            {
                LienaRemesa(data, ref oWork, ref linea, ref colum);
                contador++;
                colum++;
                linea++;
            }
            oWork.Cells[string.Format("A:BZ")].AutoFitColumns();
            oPack.Save();
            #endregion "DETALLE DE PLANTILLA"

        }
        void LienaRemesa(LOGI_REMESA_INFO data, ref ExcelWorksheet oWork, ref int Linea, ref int colum)
        {
            string Dimension = data.NombreDestinatario.Split('-')[0].ToString();

            #region "PARTIDA DE BEBIDAS"
            oWork.Cells[string.Format("A{0}", colum)].Value = "1";
            oWork.Cells[string.Format("B{0}", colum)].Value = Linea.ToString();
            oWork.Cells[string.Format("C{0}", colum)].Value = "0000516";
            oWork.Cells[string.Format("D{0}", colum)].Value = "Vend";
            oWork.Cells[string.Format("E{0}", colum)].Value = "Yes";
            oWork.Cells[string.Format("F{0}", colum)].Value = "000754";
            oWork.Cells[string.Format("G{0}", colum)].Value = "";
            oWork.Cells[string.Format("H{0}", colum)].Value = "";
            oWork.Cells[string.Format("I{0}", colum)].Value = "";
            oWork.Cells[string.Format("J{0}", colum)].Value = "";
            oWork.Cells[string.Format("K{0}", colum)].Value = "";
            oWork.Cells[string.Format("L{0}", colum)].Value = "";
            oWork.Cells[string.Format("M{0}", colum)].Value = "";
            oWork.Cells[string.Format("N{0}", colum)].Value = "EBE";
            oWork.Cells[string.Format("O{0}", colum)].Value = data.Total.ToString();
            oWork.Cells[string.Format("P{0}", colum)].Value = "MXN";
            oWork.Cells[string.Format("Q{0}", colum)].Value = "";
            oWork.Cells[string.Format("R{0}", colum)].Value = DateTime.Now.ToString("dd/MM/yyyy");
            oWork.Cells[string.Format("S{0}", colum)].Value = "";
            oWork.Cells[string.Format("T{0}", colum)].Value = string.Format("-{0}-----T001----", Dimension);
            oWork.Cells[string.Format("U{0}", colum)].Value = string.Format("{0} Remesa 1. Diciembre Logimayab",DateTime.Now.ToString("ddMMyyyy"));
            oWork.Cells[string.Format("V{0}", colum)].Value = "";
            oWork.Cells[string.Format("W{0}", colum)].Value = "";
            oWork.Cells[string.Format("X{0}", colum)].Value = "";
            oWork.Cells[string.Format("Y{0}", colum)].Value = "1";
            oWork.Cells[string.Format("Z{0}", colum)].Value = string.Format("{0}{1}", data.Serie, data.Folio);
            oWork.Cells[string.Format("AA{0}", colum)].Value = Convert.ToDateTime(data.FechaFactura).ToString("dd/MM/yyyy");
            oWork.Cells[string.Format("AB{0}", colum)].Value = "Todos";
            oWork.Cells[string.Format("AC{0}", colum)].Value = "";
            oWork.Cells[string.Format("AD{0}", colum)].Value = "";
            oWork.Cells[string.Format("AE{0}", colum)].Value = "Ledger";
            oWork.Cells[string.Format("AF{0}", colum)].Value = "EBE";
            oWork.Cells[string.Format("AG{0}", colum)].Value = "";
            oWork.Cells[string.Format("AH{0}", colum)].Value = "";
            oWork.Cells[string.Format("AI{0}", colum)].Value = "";
            oWork.Cells[string.Format("AJ{0}", colum)].Value = "GEN";
            oWork.Cells[string.Format("AK{0}", colum)].Value = "";
            oWork.Cells[string.Format("AL{0}", colum)].Value = "";
            oWork.Cells[string.Format("AM{0}", colum)].Value = "";
            oWork.Cells[string.Format("AN{0}", colum)].Value = "";
            oWork.Cells[string.Format("AO{0}", colum)].Value = "";
            oWork.Cells[string.Format("AP{0}", colum)].Value = "";
            oWork.Cells[string.Format("AQ{0}", colum)].Value = "";
            oWork.Cells[string.Format("AR{0}", colum)].Value = "";
            oWork.Cells[string.Format("AS{0}", colum)].Value = "";
            oWork.Cells[string.Format("AT{0}", colum)].Value = "";
            oWork.Cells[string.Format("AU{0}", colum)].Value = "";
            oWork.Cells[string.Format("AV{0}", colum)].Value = "";
            oWork.Cells[string.Format("AW{0}", colum)].Value = "";
            oWork.Cells[string.Format("AX{0}", colum)].Value = "";
            oWork.Cells[string.Format("AY{0}", colum)].Value = "";
            oWork.Cells[string.Format("AZ{0}", colum)].Value = "";
            oWork.Cells[string.Format("BA{0}", colum)].Value = "PRPMFLETES";
            oWork.Cells[string.Format("BB{0}", colum)].Value = "";
            oWork.Cells[string.Format("BC{0}", colum)].Value = "";
            oWork.Cells[string.Format("BD{0}", colum)].Value = "Vend";
            oWork.Cells[string.Format("BE{0}", colum)].Value = "";
            oWork.Cells[string.Format("BF{0}", colum)].Value = data.UUIDFiscal;
            oWork.Cells[string.Format("BG{0}", colum)].Value = "1";
            oWork.Cells[string.Format("BH{0}", colum)].Value = "No";
            #endregion "PARTIDA DE BEBIDAS"
            Linea++;
            colum++;
            #region "PARTIDA DE LOGISTICA DEL MAYAB"
            oWork.Cells[string.Format("A{0}", colum)].Value = "1";
            oWork.Cells[string.Format("B{0}", colum)].Value = Linea.ToString();
            oWork.Cells[string.Format("C{0}", colum)].Value = string.Format("FBA332001-{0}-DS-DS01-B001-NOAPLICA-T001----",Dimension);
            oWork.Cells[string.Format("D{0}", colum)].Value = "ledger";
            oWork.Cells[string.Format("E{0}", colum)].Value = "Yes";
            oWork.Cells[string.Format("F{0}", colum)].Value = "000754";
            oWork.Cells[string.Format("G{0}", colum)].Value = "";
            oWork.Cells[string.Format("H{0}", colum)].Value = "";
            oWork.Cells[string.Format("I{0}", colum)].Value = "";
            oWork.Cells[string.Format("J{0}", colum)].Value = "";
            oWork.Cells[string.Format("K{0}", colum)].Value = "";
            oWork.Cells[string.Format("L{0}", colum)].Value = "";
            oWork.Cells[string.Format("M{0}", colum)].Value = "";
            oWork.Cells[string.Format("N{0}", colum)].Value = "EBE";
            oWork.Cells[string.Format("O{0}", colum)].Value = "";
            oWork.Cells[string.Format("P{0}", colum)].Value = "MXN";
            oWork.Cells[string.Format("Q{0}", colum)].Value = "";
            oWork.Cells[string.Format("R{0}", colum)].Value = DateTime.Now.ToString("dd/MM/yyyy");
            oWork.Cells[string.Format("S{0}", colum)].Value = data.Total.ToString();
            oWork.Cells[string.Format("T{0}", colum)].Value = "";
            oWork.Cells[string.Format("U{0}", colum)].Value = string.Format("{0} Remesa 1. Diciembre Logimayab", DateTime.Now.ToString("ddMMyyyy"));
            oWork.Cells[string.Format("V{0}", colum)].Value = "";
            oWork.Cells[string.Format("W{0}", colum)].Value = "";
            oWork.Cells[string.Format("X{0}", colum)].Value = "";
            oWork.Cells[string.Format("Y{0}", colum)].Value = "1";
            oWork.Cells[string.Format("Z{0}", colum)].Value = string.Format("{0}{1}", data.Serie, data.Folio);
            oWork.Cells[string.Format("AA{0}", colum)].Value = Convert.ToDateTime(data.FechaFactura).ToString("dd/MM/yyyy");
            oWork.Cells[string.Format("AB{0}", colum)].Value = "FLETES";
            oWork.Cells[string.Format("AC{0}", colum)].Value = "";
            oWork.Cells[string.Format("AD{0}", colum)].Value = "";
            oWork.Cells[string.Format("AE{0}", colum)].Value = "Ledger";
            oWork.Cells[string.Format("AF{0}", colum)].Value = "EBE";
            oWork.Cells[string.Format("AG{0}", colum)].Value = "";
            oWork.Cells[string.Format("AH{0}", colum)].Value = "";
            oWork.Cells[string.Format("AI{0}", colum)].Value = "";
            oWork.Cells[string.Format("AJ{0}", colum)].Value = "GEN";
            oWork.Cells[string.Format("AK{0}", colum)].Value = "";
            oWork.Cells[string.Format("AL{0}", colum)].Value = "";
            oWork.Cells[string.Format("AM{0}", colum)].Value = "";
            oWork.Cells[string.Format("AN{0}", colum)].Value = "";
            oWork.Cells[string.Format("AO{0}", colum)].Value = "";
            oWork.Cells[string.Format("AP{0}", colum)].Value = "";
            oWork.Cells[string.Format("AQ{0}", colum)].Value = "";
            oWork.Cells[string.Format("AR{0}", colum)].Value = "";
            oWork.Cells[string.Format("AS{0}", colum)].Value = "";
            oWork.Cells[string.Format("AT{0}", colum)].Value = "";
            oWork.Cells[string.Format("AU{0}", colum)].Value = "";
            oWork.Cells[string.Format("AV{0}", colum)].Value = "";
            oWork.Cells[string.Format("AW{0}", colum)].Value = "";
            oWork.Cells[string.Format("AX{0}", colum)].Value = "";
            oWork.Cells[string.Format("AY{0}", colum)].Value = "";
            oWork.Cells[string.Format("AZ{0}", colum)].Value = "";
            oWork.Cells[string.Format("BA{0}", colum)].Value = "PRPMFLETES";
            oWork.Cells[string.Format("BB{0}", colum)].Value = "";
            oWork.Cells[string.Format("BC{0}", colum)].Value = "";
            oWork.Cells[string.Format("BD{0}", colum)].Value = "Vend";
            oWork.Cells[string.Format("BE{0}", colum)].Value = "";
            oWork.Cells[string.Format("BF{0}", colum)].Value = data.UUIDFiscal;
            oWork.Cells[string.Format("BG{0}", colum)].Value = "1";
            oWork.Cells[string.Format("BH{0}", colum)].Value = "No";
            #endregion "PARTIDA DE LOGISTICA DEL MAYAB"
        }
        public class LOGI_REMESA_INFO
        {
            public string NombreFiscal { get; set; }
            public int Folio { get; set; }
            public string Serie { get; set; }
            public String FechaHora { get; set; }
            public int Remitente { get; set; }
            public string NombreRemitente { get; set; }
            public int Destinatario { get; set; }
            public string NombreDestinatario { get; set; }
            public Decimal SubTotal { get; set; }
            public Decimal Total { get; set; }
            public String FechaFactura { get; set; } 
            public bool PendienteFacturar { get; set; }
            public string DatosRemitente { get; set; }
            public string DatosDestinatario { get; set; }
            public int IdViaje { get; set; }
            public string UUIDFiscal { get; set; }
        }

        public class LOGI_RESPOMSEREMESA_INFO
        {
            public bool Success { get; set; }
            public string VersionAPI { get; set; }
            public string IdRequest { get; set; }
            public LOGI_RequestREMESA_INFO Result { get; set; }

        }
        public class LOGI_RequestREMESA_INFO
        {
            public string Mensaje { get; set; }
            public List<LOGI_REMESA_INFO> Viajes { get; set; }

        }

        private void txtfiltro_KeyDown(object sender, KeyEventArgs e)
        {
             
        }
    }
}
