using AD;
using AD.Objetos.OPE;
using INFO.Objetos.OPE;
using INFO.Tablas.LOG;
using INFO.Tablas.OPE;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Objetos.OPE
{
    public  class LOGI_NominaMO_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnectionLOG = null;
        internal LOGI_ConexionSql_AD oConnectionOPE = null;
        const string CONST_CLASE = "LOGI_NominaMO_PD.cs";
        const string CONST_MODULO = "Nominamano";

        public LOGI_NominaMO_PD(string sConnbitacora, string sConnopeadm)
        {
            oConnectionLOG = new LOGI_ConexionSql_AD(sConnbitacora);
            oConnectionOPE = new LOGI_ConexionSql_AD(sConnopeadm);
            oTool = new LOGI_Tools_PD();
        }

        public string InsertaNomina(string sUsuarioID, LOGI_ManoObra_INFO oNomina, LOGI_Bitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionLOG.OpenConnection();
                oConnectionOPE.OpenConnection();
                oConnectionLOG.StarTransacction();
                oConnectionOPE.StarTransacction();

                //Valida que la bitacora no exista para no duplicar nómina
                sReponse = new LOGI_Bitacora_AD().ListaBitacora(ref oConnectionLOG, oBitacora, out sConsultaSql);
                if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    sReponse = new LOGI_Bitacora_AD().NuevaBitacora(ref oConnectionLOG, oBitacora, out sConsultaSql);///Crea la bitacora
                    if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception("No se pudo crear el registro de bitacora en nómina");

                    sReponse = new LOGI_Bitacora_AD().InsertaNomina(ref oConnectionOPE, oNomina, out sConsultaSql);///Crea la bitacora
                    if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception("No se pudo crear el registro de bitacora en nómina");

                    sReponse = "OK";
                }
                else if (sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    sReponse = "OK";
                else throw new Exception("No se pudo crear el registro de bitacora en nómina");

                oConnectionLOG.CommitTransacction();
                oConnectionOPE.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnectionLOG.RollBackTransacction();
                oConnectionOPE.RollBackTransacction();
                oTool.LogError(ex, "InsertaNomina", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();

                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();
            }
            return sReponse;
        }

        public string InsertaBitacoraCxC(string sUsuarioID, LOGI_Bitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionLOG.OpenConnection();
                oConnectionLOG.StarTransacction();

                sReponse = new LOGI_Bitacora_AD().NuevaBitacoraCxC(ref oConnectionLOG, oBitacora, out sConsultaSql);///Crea la bitacora

                oConnectionLOG.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnectionLOG.RollBackTransacction();
                oTool.LogError(ex, "InsertaBitacoraCxC", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection(); 
            }
            return sReponse;
        }

        public string ConsultaBitacoraCxC(string sUsuarioID, LOGI_Bitacora_INFO oParam, ref LOGI_Bitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                sReponse = new LOGI_Bitacora_AD().ListaBitacoraCxC(ref oConnectionLOG, oParam, ref oBitacora, out sConsultaSql);///Crea la bitacora
            }
            catch (Exception ex)
            {
                
                oTool.LogError(ex, "ConsultaBitacoraCxC", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return sReponse;
        }

        public string ConsultaBitacoraCxP(string sUsuarioID, LOGI_Bitacora_INFO oParam, ref LOGI_Bitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                sReponse = new LOGI_Bitacora_AD().ListaBitacoraCxP(ref oConnectionLOG, oParam, ref oBitacora, out sConsultaSql);///Crea la bitacora
            }
            catch (Exception ex)
            {

                oTool.LogError(ex, "ConsultaBitacoraCxP", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return sReponse;
        }

        public string InsertaBitacoraCxP(string sUsuarioID, LOGI_Bitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionLOG.OpenConnection();
                oConnectionLOG.StarTransacction();

                sReponse = new LOGI_Bitacora_AD().NuevaBitacoraCxP(ref oConnectionLOG, oBitacora, out sConsultaSql);///Crea la bitacora

                oConnectionLOG.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnectionLOG.RollBackTransacction();
                oTool.LogError(ex, "InsertaBitacoraCxP", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return sReponse;
        }


        public string ValidaManoRegistrada(LOGI_Bitacora_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionLOG.OpenConnection();
                sReponse =  new LOGI_Bitacora_AD().ListaBitacora(ref oConnectionLOG, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ValidaManoRegistrada", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return sReponse;
        }


        public List<LOGI_ControlProcesos_INFO> ConsultaControlProcesos(string sUsuarioID)
        {
            List<LOGI_ControlProcesos_INFO> listaProcesos = new List<LOGI_ControlProcesos_INFO>();
            string sConsultaSql = string.Empty;
            DataSet sReponse = null;

            try
            {
                sReponse = new LOGI_Bitacora_AD().ListaControlProcesos(ref oConnectionLOG, out sConsultaSql);

                if (sReponse != null)
                {
                    DataTable procesos = sReponse.Tables[0];

                    foreach (DataRow proceso in procesos.Rows)
                    {
                        LOGI_ControlProcesos_INFO item = new LOGI_ControlProcesos_INFO
                        {
                            FechaInicial = Convert.ToDateTime(proceso["FechaInicial"]),
                            ProMovimientosBancarios = Convert.ToInt32(proceso["ProMovimientosBancarios"]),
                            ProFacturas = Convert.ToInt32(proceso["ProFacturas"]),
                            ProCancelacionFacturas = Convert.ToInt32(proceso["ProCancelacionFacturas"]),
                            ProNotasCredito = Convert.ToInt32(proceso["ProNotasCredito"]),
                            ProPasivos = Convert.ToInt32(proceso["ProPasivos"]),
                            FechaSolicitud = Convert.ToDateTime(proceso["FechaSolicitud"]),
                            NuevaFechaSolicitud = Convert.ToDateTime(proceso["NuevaFechaSolicitud"])
                        };

                        listaProcesos.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {

                oTool.LogError(ex, "ConsultaControlProcesos", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return listaProcesos;
        }

        public string ActualizaControlProcesos(LOGI_ControlProcesos_INFO cProceso, LOGI_EstadoControlProcesos_INFO eProceso)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionLOG.OpenConnection();
                sReponse = new LOGI_Bitacora_AD().ActualizaControlProcesos(ref oConnectionLOG, cProceso, eProceso, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ActualizaControlProcesos", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return sReponse;
        }

    }
}
