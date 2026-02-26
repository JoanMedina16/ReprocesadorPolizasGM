using D365;
using D365.INFOD;
using INFO.Enums;
using INFO.Objetos.D365;
using INFO.Objetos.OPE;
using INFO.Tablas.D365;
using Newtonsoft.Json;
using PD.Herramientas;
using PD.Objetos.OPE;
using PD.Tablas.D365;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOGISERVICES
{
    public partial class Service1 //: ServiceBase
    {
        #region "PROPIEDADES"        
        LOGI_Tools_PD oTools = null;
        internal LOGI_ConnectionService_D365 oConnection = null;
        internal LOGI_TMSConnection_D365 oConectionIngresoPago =null;
        internal LOGI_TMSConnection_D365 oConectionPasivoUno =null;
        internal LOGI_TMSConnection_D365 oConectionPasivodos = null;
        internal LOGI_TMSConnection_D365 oConectionPasivoTres = null;
        internal LOGI_TMSConnection_D365 oConectionPasivoCuatro = null;
        internal LOGI_TMSConnection_D365 oConectionAntiguos = null;
        internal const string CONST_USUARIO = "620";
        internal string CONST_CONNECTION = ConfigurationManager.AppSettings["CONEXION_LOGI"].ToString();
        internal string CONST_CONNECTION_LOG = ConfigurationManager.AppSettings["CONEXION_LOG_BITACORA"].ToString();
        internal string PATH_JSON = ConfigurationManager.AppSettings["RUTA_ARCHIVOS_LOGS"].ToString(); 
        internal string CONST_ENVIO_COMBUS = ConfigurationManager.AppSettings["PASIVOS_COMBUSTIBLE"].ToString();
        internal string CONST_ENVIO_PASIVOS = ConfigurationManager.AppSettings["PASIVOS_TALLER"].ToString();
        internal string CONST_ENVIO_INGRESOS = ConfigurationManager.AppSettings["INGRESOS"].ToString();
        internal string CONST_INICIO_CONTROL_PROCESOS = ConfigurationManager.AppSettings["INICIO_PROCESOS"].ToString();
        internal string CONST_INTERVALO_CONTROL_PROCESOS = ConfigurationManager.AppSettings["INTERVALO_PROCESOS"].ToString();
        internal string CONST_PATH_ANTIGUOS = ConfigurationManager.AppSettings["ANTIGUOS"].ToString();

        //internal Thread Timer_SincronizaD365 = null;
        internal Timer Timer_EnviaIngresosDepositos;
        internal Timer Timer_EnviaPasivosUno;
        internal Timer Timer_EnviaPasivosdos;
        internal Timer Timer_EnviaPasivosTres;
        internal Timer Timer_EnviaPasivosCuatro;
        internal bool _isRunning = true;
        private static bool _enEjecucion = false;
        const string CONST_CLASE = "Service1.cs";
        const string CONST_MODULO = "Operaciones Logística";
        const string CONST_USER = "WINSERVICES";
        #endregion "PROPIEDADES"

        //public Service1()
        //{
        //    InitializeComponent();
        //}

/*
        public void DebugStart()
        {
            OnStart(new string[] { });
            Thread.Sleep(999999999);
        }

        public void DebugStop()
        {
            OnStop();
        }
*/

        protected override void OnStart(string[] args)
        {
            _isRunning = true;
            oTools = new LOGI_Tools_PD();
            oTools.LogProceso("Servicio de replicas inicializado", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);

            //Este hilo ejecuta todos los procesos de tipo ingres (cancelación y notas de credito) hacia D365. Procesa la reposición de gastos de operadores hacia OPEADM (FyEmpleados)
            /*
                TimeSpan oIntervalo = new TimeSpan(0, Convert.ToInt32(CONST_ENVIO_INGRESOS), 0);
                Timer_EnviaIngresosDepositos = new Timer(new TimerCallback(TimerSincronizaIngresoReposicionD365_Object), null, oIntervalo, oIntervalo);
            */
            
            TimeSpan oInicioIntervalo = new TimeSpan(0, Convert.ToInt32(CONST_INICIO_CONTROL_PROCESOS), 0);
            TimeSpan oIntervalo = new TimeSpan(0, Convert.ToInt32(CONST_INTERVALO_CONTROL_PROCESOS), 0);

            Timer_EnviaIngresosDepositos = new Timer(new TimerCallback(TimerSincronizaIngresoReposicionD365_Object), null, oInicioIntervalo, oIntervalo);
            
            oIntervalo = new TimeSpan(0, Convert.ToInt32(CONST_ENVIO_PASIVOS), 0);
            Timer_EnviaPasivosUno = new Timer(new TimerCallback(TimerSincronizaPasivoSemanaUnoD365_Object), null, oIntervalo, oIntervalo);
            oIntervalo = new TimeSpan(0, Convert.ToInt32(CONST_ENVIO_PASIVOS) + 5, 0);
            Timer_EnviaPasivosdos = new Timer(new TimerCallback(TimerSincronizaPasivoSemanaDosD365_Object), null, oIntervalo, oIntervalo);
            oIntervalo = new TimeSpan(0, Convert.ToInt32(CONST_ENVIO_PASIVOS) + 10, 0);
            Timer_EnviaPasivosTres = new Timer(new TimerCallback(TimerSincronizaPasivoSemanaTresD365_Object), null, oIntervalo, oIntervalo);
            oIntervalo = new TimeSpan(0, Convert.ToInt32(CONST_ENVIO_PASIVOS) + 15, 0);
            Timer_EnviaPasivosCuatro = new Timer(new TimerCallback(TimerSincronizaPasivoSemanaCuatroD365_Object), null, oIntervalo, oIntervalo);
            ListenerPathPeriodos();
        }

        protected override void OnStop()
        {
            _isRunning = false;
            oTools.LogProceso("Servicio de replicas detenido", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
            /*if (Timer_SincronizaD365 != null)
            {
                if (Timer_SincronizaD365.IsAlive)
                    Timer_SincronizaD365.Abort();
            }*/
            Timer_EnviaIngresosDepositos.Change(Timeout.Infinite, Timeout.Infinite);
            Timer_EnviaPasivosUno.Change(Timeout.Infinite, Timeout.Infinite);
            Timer_EnviaPasivosdos.Change(Timeout.Infinite, Timeout.Infinite);
            Timer_EnviaPasivosTres.Change(Timeout.Infinite, Timeout.Infinite);
            Timer_EnviaPasivosCuatro.Change(Timeout.Infinite, Timeout.Infinite);

        }

        #region "METODOS GENERALES"        

        void ListenerPathPeriodos()
        {
            try
            {

                FileSystemWatcher oView = new FileSystemWatcher();
                oView.Path = CONST_PATH_ANTIGUOS;
                oView.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
                oView.Filter = "*.*";
                oView.EnableRaisingEvents = true;
                oView.Created += new FileSystemEventHandler(onLoadJSONFolder);

            }
            catch (Exception ex)
            {
                oTools.LogError(ex, "ListenerPathPeriodos", CONST_USER, CONST_CLASE, CONST_MODULO);
            }
        }


        private void onLoadJSONFolder(object source, FileSystemEventArgs e)
        {
            try
            {
                string sresponseXML = string.Empty;
                FileInfo oInfo = new FileInfo(e.FullPath);
                LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
                LOGI_Credencial_D365 oCredencial = new LOGI_Credencial_D365();
                if (oInfo.Extension.Equals(".json", StringComparison.InvariantCultureIgnoreCase))
                {
                    string sFallos = string.Format(@"{0}\FALLOS", CONST_PATH_ANTIGUOS);
                    string sCorrectos = string.Format(@"{0}\APLICADOS", CONST_PATH_ANTIGUOS);
                    if (!Directory.Exists(sFallos))
                        Directory.CreateDirectory(sFallos);
                    if (!Directory.Exists(sCorrectos))
                        Directory.CreateDirectory(sCorrectos);

                    DirectoryInfo oDirectory = new DirectoryInfo(CONST_PATH_ANTIGUOS);
                    FileSystemInfo[] lstFiles = oDirectory.GetFileSystemInfos();
                    var lstCfdis = lstFiles.Where(x => x.Extension.Equals(".json", StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x.CreationTime.Date).ToList();
                    //es la cantidad total de xmls en el directorio 
                    if (lstCfdis.Count > 0)
                    {
                        Thread.Sleep(7500);
                        int iProcesados = 0, iAplicados = 0, iFallidos = 0;
                        foreach (FileInfo oFile in lstCfdis)
                        {
                            iProcesados++;
                            LOGI_Tiempos_INFO oTiempos = new LOGI_Tiempos_INFO();
                            if (File.Exists(oFile.FullName))
                            {
                                string jsonContent = File.ReadAllText(oFile.FullName);
                                oTiempos = JsonConvert.DeserializeObject<LOGI_Tiempos_INFO>(jsonContent);

                                if (this.GetConfig(ref oCredencial, ref oConfiguracion))
                                {
                                    //solo si el envío de polizas está habilitado se sincronizan los datos 
                                    if (oConfiguracion.sincd365 == 1)
                                    {
                                        if (oConectionAntiguos == null)
                                            oConectionAntiguos = new LOGI_TMSConnection_D365(oCredencial, CONST_CONNECTION, oConfiguracion.Conexion_eqv, CONST_CONNECTION_LOG, PATH_JSON, oConfiguracion);
                                        if (oConectionAntiguos.onCreateLogin())
                                        {
                                            oConectionAntiguos.DescargaEjecutaMovimientosPasivos(oTiempos.fechainicio.ToString("dd/MM/yyyy"), oTiempos.fechafinal.ToString("dd/MM/yyyy"), bIgnoraFechas:true);

                                            oTools.LogProceso("onLoadJSONFolder", "TimerSincronizaIngresoReposicionD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                                            oConectionAntiguos.DescargaEjecutaMovimientosIngreso(oTiempos.fechainicio.ToString("dd/MM/yyyy"), oTiempos.fechafinal.ToString("dd/MM/yyyy"), bCreaArchivoJSON:false);

                                            string sFiletoMove = string.Format(@"{0}\{1}", sCorrectos, oFile.Name);
                                            //antes de mover validar si existe; en caso de existir anteponer copia mas datetime
                                            if (File.Exists(sFiletoMove))
                                                sFiletoMove = string.Format(@"{0}\copy_{1}_{2}", sCorrectos, DateTime.Now.ToString("ddMMyyyyhhmmsstt"), oFile.Name);
                                            File.Move(oFile.FullName, sFiletoMove);

                                        }
                                    }
                                    //CONST_PAUSA = oTools.GetTiempoSleepSegmento(0, oConfiguracion.enviohorad365, Convert.ToInt32(CONST_ENVIO_COMBUS), oConfiguracion.enviosegd365);
                                }
                                else oTools.LogProceso("No se ha podido leer la configuración para el servicio", "onLoadJSONFolder", CONST_CLASE, CONST_MODULO, CONST_USUARIO);


                            }
                            else iFallidos++;
                        }
                        //Finalizado
                        sresponseXML = string.Format("Se ha finalizado el proceso de replica, se han procesado un total de {0}, aplicando {1} y encontrado {2} fallos", iProcesados, iAplicados, iFallidos);
                        oTools.LogProceso(sresponseXML, "onLoadJSONFolder", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                    }

                }
            }
            catch (Exception ex)
            {
                oTools.LogError(ex, "ListenerPathPeriodos", CONST_USER, CONST_CLASE, CONST_MODULO);
            }
        }

        #region "METODOS UTILIZADO PARA REPLICA DE DATOS DEL COORPORATIVO"
        /// <summary>
        /// Descripción: Metodo de ejecución automatico, el proceso se mantiene en vida mientras el servicio windows se encuentra activo. 
        /// El proceso invoca la replica de polizas contables hacia D365. Toda operación invocada se procesa en liberería "D365". Los tiempos 
        /// de ejecución corresponden a la información configurada en tabla lm_config_d365.
        /// 
        /// Fecha: 28/04/2022
        /// </summary>
        /// <returns></returns>
        private void TimerSincronizaContabilidadD365_Object()
        {
            Int32 CONST_PAUSA = 3000;
            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_Credencial_D365 oCredencial = new LOGI_Credencial_D365();
            oTools.LogProceso("Ejecución de envio de contabilidad", "TimerSincronizaContabilidadD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);

            while (_isRunning)
            {
                try
                {
                    
                    if (this.GetConfig(ref oCredencial, ref oConfiguracion))
                    {
                        //solo si el envío de polizas está habilitado se sincronizan los datos 
                        if (oConfiguracion.sincd365 == 1)
                        {
                            if (oConnection == null)
                                oConnection = new LOGI_ConnectionService_D365(oCredencial, CONST_CONNECTION, oConfiguracion.Conexion_eqv);
                            if (oConnection.onCreateLogin())
                            {
                                oTools.LogProceso("onExecuteAsientos(false)", "TimerSincronizaContabilidadD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                                oConnection.onExecuteAsientos(false);
                            }
                            else
                            {
                                oTools.LogProceso("No se pudo establecer la conexión con 365", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                            }
                        }
                        else
                        {
                            oTools.LogProceso("El envío de pólizas a 365 no está habilitado", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                        }
                        
                        CONST_PAUSA = oTools.GetTiempoSleepSegmento(0, oConfiguracion.enviohorad365, oConfiguracion.enviomind365, oConfiguracion.enviosegd365);
                    }
                    else oTools.LogProceso("No se ha podido leer la configuración para el servicio", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                }
                catch (Exception ex)
                {
                    oTools.LogError(ex, "TimerSincronizaContabilidadD365_Object=> CATCH", CONST_USER, CONST_CLASE, CONST_MODULO);
                }
                finally
                {
                    Thread.Sleep(CONST_PAUSA);
                    System.GC.Collect();
                }
            }
        }


        private void TimerSincronizaIngresoReposicionD365_Object(object state)
        {
            if (_enEjecucion) return;
            
            _enEjecucion = true;

            try
            {
                //Int32 CONST_PAUSA = 3000;
                LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
                LOGI_Credencial_D365 oCredencial = new LOGI_Credencial_D365();
                oTools.LogProceso("Ejecución de envio de ingreso/pagos", "TimerSincronizaIngresoReposicionD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);

                if (this.GetConfig(ref oCredencial, ref oConfiguracion))
                {
                    //solo si el envío de polizas está habilitado se sincronizan los datos 
                    if (oConfiguracion.sincd365 == 1)
                    {
                        if (oConectionIngresoPago == null)
                            oConectionIngresoPago = new LOGI_TMSConnection_D365(oCredencial, CONST_CONNECTION, oConfiguracion.Conexion_eqv, CONST_CONNECTION_LOG, PATH_JSON, oConfiguracion);
                        if (oConectionIngresoPago.onCreateLogin())
                        {
                            LOGI_NominaMO_PD resultcProcesos = new LOGI_NominaMO_PD(CONST_CONNECTION_LOG, CONST_CONNECTION);
                            List<LOGI_ControlProcesos_INFO> cProcesos = resultcProcesos.ConsultaControlProcesos(CONST_USUARIO);

                            foreach (LOGI_ControlProcesos_INFO cProceso in cProcesos)
                            {
                                LOGI_EstadoControlProcesos_INFO eProceso = new LOGI_EstadoControlProcesos_INFO {
                                    ProFacturas = true,
                                    ProCancelacionFacturas = true,
                                    ProNotasCredito = true,
                                    ProMovimientosBancarios = true,
                                    ProPasivos = true
                                };

                                cProceso.Estado = 1;
                                resultcProcesos.ActualizaControlProcesos(cProceso, eProceso);
                                oTools.LogProceso("DescargaMovimientoTMS", "TimerSincronizaIngresoReposicionD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                                
                                eProceso = oConectionIngresoPago.DescargaEjecutaMovimientosIngreso(cProceso.FechaInicial.ToString("dd/MM/yyyy"), cProceso.FechaInicial.ToString("dd/MM/yyyy"), false, true, cProceso.ProFacturas, cProceso.ProCancelacionFacturas, cProceso.ProNotasCredito, cProceso.ProMovimientosBancarios, cProceso.ProPasivos);
                                
                                cProceso.Estado = 2;
                                resultcProcesos.ActualizaControlProcesos(cProceso, eProceso);
                            }
                        }
                    }
                    //CONST_PAUSA = oTools.GetTiempoSleepSegmento(0, oConfiguracion.enviohorad365, Convert.ToInt32(CONST_ENVIO_COMBUS), oConfiguracion.enviosegd365);
                }
                else oTools.LogProceso("No se ha podido leer la configuración para el servicio", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
            }
            catch (Exception ex)
            {
                oTools.LogError(ex, "TimerSincronizaIngresoReposicionD365_Object=> CATCH", CONST_USER, CONST_CLASE, CONST_MODULO);
            }
            finally
            {
                _enEjecucion = false;
            }

        }


        private void TimerSincronizaPasivoSemanaUnoD365_Object(object state)
        {

            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_Credencial_D365 oCredencial = new LOGI_Credencial_D365();
            oTools.LogProceso("Ejecución de pasivos semana uno", "TimerSincronizaPasivoSemanaUnoD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);


            try
            {

                if (this.GetConfig(ref oCredencial, ref oConfiguracion))
                {
                    //solo si el envío de polizas está habilitado se sincronizan los datos 
                    if (oConfiguracion.sincd365 == 1)
                    {
                        if (oConectionPasivoUno == null)
                            oConectionPasivoUno = new LOGI_TMSConnection_D365(oCredencial, CONST_CONNECTION, oConfiguracion.Conexion_eqv, CONST_CONNECTION_LOG, PATH_JSON, oConfiguracion);
                        if (oConectionPasivoUno.onCreateLogin())
                        {

                            DateTime fechaActual = DateTime.Now;
                            int totalDiasDelMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                            int diaDelMes = fechaActual.Day;
                            List<LOGI_Tiempos_INFO> lstBloques = DefinirBloques(fechaActual.Year, fechaActual.Month, totalDiasDelMes);
                            LOGI_Tiempos_INFO oBloque = lstBloques[0];
                            oTools.LogProceso(string.Format("Ejecución del día {0}. Para el rango de fechas fecha inicio {1} al rango de fecha final {2}", diaDelMes, oBloque.fechainicio, oBloque.fechafinal), "TimerSincronizaPasivoSemanaUnoD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                            //if (diaDelMes >= oBloque.fechainicio.Day && diaDelMes <= oBloque.fechafinal.Day)
                            //{
                                oTools.LogProceso("DescargaMovimientoTMS", "TimerSincronizaPasivoSemanaUnoD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                                oConectionPasivoUno.DescargaEjecutaMovimientosPasivos(oBloque.fechainicio.ToString("dd/MM/yyyy"), oBloque.fechafinal.ToString("dd/MM/yyyy"));
                            //}
                        }
                    }
                }
                else oTools.LogProceso("No se ha podido leer la configuración para el servicio", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
            }
            catch (Exception ex)
            {
                oTools.LogError(ex, "TimerSincronizaPasivoSemanaUnoD365_Object=> CATCH", CONST_USER, CONST_CLASE, CONST_MODULO);
            }
        }


        private void TimerSincronizaPasivoSemanaDosD365_Object(object state)
        {

            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_Credencial_D365 oCredencial = new LOGI_Credencial_D365();
            oTools.LogProceso("Ejecución de pasivos semana dos", "TimerSincronizaPasivoSemanaDosD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);


            try
            {

                if (this.GetConfig(ref oCredencial, ref oConfiguracion))
                {
                    //solo si el envío de polizas está habilitado se sincronizan los datos 
                    if (oConfiguracion.sincd365 == 1)
                    {
                        if (oConectionPasivodos == null)
                            oConectionPasivodos = new LOGI_TMSConnection_D365(oCredencial, CONST_CONNECTION, oConfiguracion.Conexion_eqv, CONST_CONNECTION_LOG, PATH_JSON, oConfiguracion);
                        if (oConectionPasivodos.onCreateLogin())
                        {

                            DateTime fechaActual = DateTime.Now;
                            int totalDiasDelMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                            int diaDelMes = fechaActual.Day;
                            List<LOGI_Tiempos_INFO> lstBloques = DefinirBloques(fechaActual.Year, fechaActual.Month, totalDiasDelMes);
                            LOGI_Tiempos_INFO oBloque = lstBloques[1];
                            oTools.LogProceso(string.Format("Ejecución del día {0}. Para el rango de fechas fecha inicio {1} al rango de fecha final {2}", diaDelMes, oBloque.fechainicio, oBloque.fechafinal), "TimerSincronizaPasivoSemanaUnoD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                            //if (diaDelMes >= oBloque.fechainicio.Day && diaDelMes <= oBloque.fechafinal.Day)
                            //{
                                oTools.LogProceso("DescargaMovimientoTMS", "TimerSincronizaPasivoSemanaDosD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                                oConectionPasivodos.DescargaEjecutaMovimientosPasivos(oBloque.fechainicio.ToString("dd/MM/yyyy"), oBloque.fechafinal.ToString("dd/MM/yyyy"));
                            //}
                        }
                    }
                }
                else oTools.LogProceso("No se ha podido leer la configuración para el servicio", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
            }
            catch (Exception ex)
            {
                oTools.LogError(ex, "TimerSincronizaPasivoSemanaUnoD365_Object=> CATCH", CONST_USER, CONST_CLASE, CONST_MODULO);
            }
        }



        private void TimerSincronizaPasivoSemanaTresD365_Object(object state)
        {

            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_Credencial_D365 oCredencial = new LOGI_Credencial_D365();
            oTools.LogProceso("Ejecución de pasivos semana tres", "TimerSincronizaPasivoSemanaTresD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);


            try
            {

                if (this.GetConfig(ref oCredencial, ref oConfiguracion))
                {
                    //solo si el envío de polizas está habilitado se sincronizan los datos 
                    if (oConfiguracion.sincd365 == 1)
                    {
                        if (oConectionPasivoTres == null)
                            oConectionPasivoTres = new LOGI_TMSConnection_D365(oCredencial, CONST_CONNECTION, oConfiguracion.Conexion_eqv, CONST_CONNECTION_LOG, PATH_JSON, oConfiguracion);
                        if (oConectionPasivoTres.onCreateLogin())
                        {

                            DateTime fechaActual = DateTime.Now;
                            int totalDiasDelMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                            int diaDelMes = fechaActual.Day;
                            List<LOGI_Tiempos_INFO> lstBloques = DefinirBloques(fechaActual.Year, fechaActual.Month, totalDiasDelMes);
                            LOGI_Tiempos_INFO oBloque = lstBloques[2];
                            oTools.LogProceso(string.Format("Ejecución del día {0}. Para el rango de fechas fecha inicio {1} al rango de fecha final {2}", diaDelMes, oBloque.fechainicio, oBloque.fechafinal), "TimerSincronizaPasivoSemanaUnoD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                            //if (diaDelMes >= oBloque.fechainicio.Day && diaDelMes <= oBloque.fechafinal.Day)
                            //{
                                oTools.LogProceso("DescargaMovimientoTMS", "TimerSincronizaPasivoSemanaTresD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                                oConectionPasivoTres.DescargaEjecutaMovimientosPasivos(oBloque.fechainicio.ToString("dd/MM/yyyy"), oBloque.fechafinal.ToString("dd/MM/yyyy"));
                            //}
                        }
                    }
                }
                else oTools.LogProceso("No se ha podido leer la configuración para el servicio", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
            }
            catch (Exception ex)
            {
                oTools.LogError(ex, "TimerSincronizaPasivoSemanaTresD365_Object=> CATCH", CONST_USER, CONST_CLASE, CONST_MODULO);
            }
        }

        private void TimerSincronizaPasivoSemanaCuatroD365_Object(object state)
        {

            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_Credencial_D365 oCredencial = new LOGI_Credencial_D365();
            oTools.LogProceso("Ejecución de pasivos semana cuatro", "TimerSincronizaPasivoSemanaCuatroD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);


            try
            {

                if (this.GetConfig(ref oCredencial, ref oConfiguracion))
                {
                    //solo si el envío de polizas está habilitado se sincronizan los datos 
                    if (oConfiguracion.sincd365 == 1)
                    {
                        if (oConectionPasivoCuatro == null)
                            oConectionPasivoCuatro = new LOGI_TMSConnection_D365(oCredencial, CONST_CONNECTION, oConfiguracion.Conexion_eqv, CONST_CONNECTION_LOG, PATH_JSON, oConfiguracion);
                        if (oConectionPasivoCuatro.onCreateLogin())
                        {

                            DateTime fechaActual = DateTime.Now;
                            int totalDiasDelMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                            int diaDelMes = fechaActual.Day;
                            List<LOGI_Tiempos_INFO> lstBloques = DefinirBloques(fechaActual.Year, fechaActual.Month, totalDiasDelMes);
                            LOGI_Tiempos_INFO oBloque = lstBloques[3];
                            oTools.LogProceso(string.Format("Ejecución del día {0}. Para el rango de fechas fecha inicio {1} al rango de fecha final {2}", diaDelMes, oBloque.fechainicio, oBloque.fechafinal), "TimerSincronizaPasivoSemanaUnoD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                            //if (diaDelMes >= oBloque.fechainicio.Day && diaDelMes <= oBloque.fechafinal.Day)
                            //{
                                oTools.LogProceso("DescargaMovimientoTMS", "TimerSincronizaPasivoSemanaCuatroD365_Object", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
                                oConectionPasivoCuatro.DescargaEjecutaMovimientosPasivos(oBloque.fechainicio.ToString("dd/MM/yyyy"), oBloque.fechafinal.ToString("dd/MM/yyyy"));
                            //}
                        }
                    }
                }
                else oTools.LogProceso("No se ha podido leer la configuración para el servicio", "OnStart", CONST_CLASE, CONST_MODULO, CONST_USUARIO);
            }
            catch (Exception ex)
            {
                oTools.LogError(ex, "TimerSincronizaPasivoSemanaCuatroD365_Object=> CATCH", CONST_USER, CONST_CLASE, CONST_MODULO);
            }
        }

        #endregion "METODOS UTILIZADO PARA REPLICA DE DATOS DEL COORPORATIVO"

        #region "METODOS AUXILIARES"
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

        static List<LOGI_Tiempos_INFO> DefinirBloques(int anio, int mes, int totalDias)
        {
            int bloque1Fin = totalDias >= 31 ? 8 : 7;
            int bloque2Fin = bloque1Fin + (totalDias >= 31 ? 7 : 6);
            int bloque3Fin = bloque2Fin + (totalDias >= 31 ? 7 : 7);
            int bloque4Fin = totalDias;

            return new List<LOGI_Tiempos_INFO>
        {
            new LOGI_Tiempos_INFO { fechainicio = new DateTime(anio, mes, 1), fechafinal = new DateTime(anio, mes, bloque1Fin) },
            new LOGI_Tiempos_INFO { fechainicio = new DateTime(anio, mes, bloque1Fin + 1), fechafinal = new DateTime(anio, mes, bloque2Fin) },
            new LOGI_Tiempos_INFO { fechainicio = new DateTime(anio, mes, bloque2Fin + 1), fechafinal = new DateTime(anio, mes, bloque3Fin) },
            new LOGI_Tiempos_INFO { fechainicio = new DateTime(anio, mes, bloque3Fin + 1), fechafinal = new DateTime(anio, mes, bloque4Fin) }
        };
        }

        #endregion "METODOS AUXILIARES"

        #endregion "METODOS GENERALES"
    }
}
