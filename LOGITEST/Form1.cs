using D365;
using D365.Helpers;
using D365.Helpers.D365FOBBIContaServices;
using D365.Helpers.D365FOBBICxCServices;
using D365.INFOD;
using INFO.Enums;
using INFO.Objetos.SAT;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using INFO.Tablas.FUEL;
using Newtonsoft.Json;
using PD.Herramientas;
using PD.Objetos.OPE;
using PD.Tablas.D365;
using PD.Tablas.FUEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOGITEST
{
    public partial class Form1 : Form
    {
        internal LOGI_Tools_PD oTools = new LOGI_Tools_PD();
        internal LOGI_ConnectionService_D365 oConnection = null;
        internal LOGI_TMSConnection_D365 oConnectionTMS = null;
        internal const string CONST_USUARIO = "620";
        internal string CONST_CONNECTION = @"Data Source=10.20.128.149;Initial Catalog=admlog04;Persist Security Info=True;User ID=usr_pbasbil;Password=desarrollo;Connection Timeout=120";
        internal string CONST_CONNECTION_EQUIV = @"Data Source=10.20.128.149;Initial Catalog=BIL;Persist Security Info=True;User ID=usr_pbasbil;Password=desarrollo;Connection Timeout=120";
        internal string CONST_CONNECTION_LOG = @"Data Source=10.20.128.149;Initial Catalog=OTMLog;Persist Security Info=True;User ID=usr_pbasbil;Password=desarrollo;Connection Timeout=120";
        internal string CONST_CONNECTION_PATH = @"C:\OPERACIONES\DESCARGA_TMS_LOGS";
        public Form1()
        {
            InitializeComponent();

            Dictionary<string, string> lstOptions = new Dictionary<string, string>();
            lstOptions.Add("0", "Selecciona la operacion a ejecutar");
            lstOptions.Add("1", "Enviar información hacia D365");
            //lstOptions.Add("2", "Extrae información XML");
            //lstOptions.Add("3", "Saldo del cliente");
            //lstOptions.Add("4", "Comprobación de viaticos");
            lstOptions.Add("5", "Notificación de errores");
            cmbOpciones.DataSource = new BindingSource(lstOptions, null);
            cmbOpciones.DisplayMember = "value";
            cmbOpciones.ValueMember = "key";
           oTools.m_ConsoleLine(rchConsole, "Aplicacación de pruebas inicializada", eType.success);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string response = string.Empty;
            LOGI_XMLS_INFO oXML = new LOGI_XMLS_INFO();
            int ejecutado = 0, aplicado = 0, fallo = 0;
             try
            {
                switch (cmbOpciones.SelectedValue.ToString())
                {
                    case "1":
                        ProcesaInformacion();
                        break;

                    case "2":
                        oTools.DevuelveXMLObject(out response, ref oXML, @"C:\Users\Usuario\Downloads\EDENRED\EDENRED\TCGE000002290131 MERIDA.xml");
                        break;
                    case "3":
                        SaldoCliente();
                        break;
                    case "4":
                        ValidadiariomultipleLineas();
                        break;

                    case "5":
                        LOGI_EventosBitacora_PD oEventosBitacora = new LOGI_EventosBitacora_PD(CONST_CONNECTION_LOG, CONST_CONNECTION);
                        response = oEventosBitacora.CreaprocesoNotificacionERRORES("");
                        if (response.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                            oTools.m_ConsoleLine(rchConsole, "", eType.success);
                        else oTools.m_ConsoleLine(rchConsole, string.Format("No se pudo enviar las notificaciones de proceso. ERROR " + response), eType.warning);
                        break;

                    default:
                        MessageBox.Show("Favor de seleccionar una opción para ejecutar el proceso", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        void ValidadiariomultipleLineas()
        {
            string response = string.Empty;
            LOGI_Credencial_D365 oCred = new LOGI_Credencial_D365();
            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();

            this.GetConfig(ref oCred, ref oConfiguracion);

            if (oConnection == null)
                oConnection = new LOGI_ConnectionService_D365(oCred, CONST_CONNECTION, CONST_CONNECTION_EQUIV, rchConsole);
            if (oConnection.onCreateLogin())
            {
                oTools.m_ConsoleLine(rchConsole, "La conexión hacía D365 se ha establecido con éxito", eType.proceso);
                oTools.m_ConsoleLine(rchConsole, oConnection.sMensaje, eType.success);
                //oConnection.CreaEntradaComprobacion(out response);
            }
            else oTools.m_ConsoleLine(rchConsole, string.Format("No se ha podido establecer la conexión con D365. ERROR {0}", oConnection.sMensaje), eType.proceso);
        }

        void SaldoCliente()
        {
            string response = string.Empty;
            LOGI_Credencial_D365 oCred = new LOGI_Credencial_D365();
            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();

            this.GetConfig(ref oCred, ref oConfiguracion);

            if (oConnection == null)
                oConnection = new LOGI_ConnectionService_D365(oCred, CONST_CONNECTION, CONST_CONNECTION_EQUIV, rchConsole);
            if (oConnection.onCreateLogin())
            {
                oTools.m_ConsoleLine(rchConsole, "La conexión hacía D365 se ha establecido con éxito", eType.proceso);
                oTools.m_ConsoleLine(rchConsole, oConnection.sMensaje, eType.success);
                oConnection.SaldoCliente(10, 1322, 15000000, out response);
            }
            else oTools.m_ConsoleLine(rchConsole, string.Format("No se ha podido establecer la conexión con D365. ERROR {0}", oConnection.sMensaje), eType.proceso);
        }

        void ProcesaInformacion()
        {
            LOGI_Credencial_D365 oCred = new LOGI_Credencial_D365();
            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            this.GetConfig(ref oCred, ref oConfiguracion);

            //MessageBox.Show("Usuario "+oCred.client_id + " URL "+oCred.api+ " D "+oConfiguracion.URLApilogin);

            if (oConnectionTMS == null)
                oConnectionTMS = new LOGI_TMSConnection_D365(oCred, CONST_CONNECTION, CONST_CONNECTION_EQUIV, CONST_CONNECTION_LOG, CONST_CONNECTION_PATH, oConfiguracion, rchConsole);
            
            if (oConnectionTMS.onCreateLogin())
            {

                oTools.m_ConsoleLine(rchConsole, "La conexión hacía D365 se ha establecido con éxito", eType.proceso);
                oTools.m_ConsoleLine(rchConsole, oConnectionTMS.sMensaje, eType.success);
                oConnectionTMS.DescargaMovimientoTMS(dtfechainicio.Text, dtfechafin.Text, bLeeTXT: chkFile.Checked);
            }
            else oTools.m_ConsoleLine(rchConsole, string.Format("No se ha podido establecer la conexión con D365. ERROR {0}", oConnectionTMS.sMensaje), eType.proceso);
        }

        bool GetConfig(ref LOGI_Credencial_D365 oCred, ref LOGI_ConfiguracionD365_INFO oConfiguracion)
        {
            bool bContinuar = false;
            oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_ConfiguracionD365_PD oCnfcontrol = new LOGI_ConfiguracionD365_PD(this.CONST_CONNECTION);            
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
       
    }
  
}
