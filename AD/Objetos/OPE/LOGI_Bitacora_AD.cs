using INFO.Objetos.OPE;
using INFO.Tablas.LOG;
using INFO.Tablas.OPE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos.OPE
{
   public class LOGI_Bitacora_AD
    {
        public string NuevaBitacora(ref LOGI_ConexionSql_AD oConnection, LOGI_Bitacora_INFO oParam, out string sConsultaSql)
        {
            int iCommand = 0;
            Hashtable oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_tms_nomina_mano(folio_d365,sesion_d365,proceso_id_d365)
            VALUES(@folio_d365,@sesion_d365,@proceso_id_d365)");
            oHashParam.Add("@folio_d365", oParam.folioD365);
            oHashParam.Add("@sesion_d365", oParam.sesion_d365);
            oHashParam.Add("@proceso_id_d365", oParam.proceso_id_d365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevaBitacoraCxC(ref LOGI_ConexionSql_AD oConnection, LOGI_Bitacora_INFO oParam, out string sConsultaSql)
        {
            int iCommand = 0;
            Hashtable oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_tms_cxc_factura(folio_d365,sesion_d365,proceso_id_d365,seriefolio)
            VALUES(@folio_d365,@sesion_d365,@proceso_id_d365,@seriefolio)");
            oHashParam.Add("@folio_d365", oParam.folioD365);
            oHashParam.Add("@sesion_d365", oParam.sesion_d365);
            oHashParam.Add("@proceso_id_d365", oParam.proceso_id_d365);
            oHashParam.Add("@seriefolio", oParam.seriefolio);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevaBitacoraCxP(ref LOGI_ConexionSql_AD oConnection, LOGI_Bitacora_INFO oParam, out string sConsultaSql)
        {
            int iCommand = 0;
            Hashtable oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_tms_cxp_factura(folio_d365,sesion_d365,proceso_id_d365,seriefolio)
            VALUES(@folio_d365,@sesion_d365,@proceso_id_d365,@seriefolio)");
            oHashParam.Add("@folio_d365", oParam.folioD365);
            oHashParam.Add("@sesion_d365", oParam.sesion_d365);
            oHashParam.Add("@proceso_id_d365", oParam.proceso_id_d365);
            oHashParam.Add("@seriefolio", oParam.seriefolio);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string InsertaNomina(ref LOGI_ConexionSql_AD oConnection, LOGI_ManoObra_INFO oParam, out string sConsultaSql)
        {
            int iCommand = 0;
            Hashtable oHashParam = new Hashtable();

            sConsultaSql = string.Format(@"INSERT INTO movtos(cia,suc,emp,concepto,fecha,unidades,importe,ajuste,saldo,otros,externo,fechacap,activo,usuario,refer,folio,dbf)
            VALUES(@cia,@suc,@emp,@concepto,@fecha,0,0,@ajuste,1,2,@externo,getdate(),1,1,0,@folio,0)");
            oHashParam.Add("@cia",oParam.cia);
            oHashParam.Add("@suc", 0);
            oHashParam.Add("@emp", oParam.empleado);
            oHashParam.Add("@concepto", oParam.concepto);
            oHashParam.Add("@fecha", oParam.fecha);
            oHashParam.Add("@ajuste", oParam.importe); 
            oHashParam.Add("@externo", 1);
            oHashParam.Add("@folio", oParam.folio);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaBitacora(ref LOGI_ConexionSql_AD oConnection, LOGI_Bitacora_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Format(@"SELECT * FROM lm_tms_nomina_mano where folio_d365 = '{0}'", oParam.folioD365);
            odataset = oConnection.FillDataSet(sConsultaSql);
            //this.LoopDataSet(odataset, ref lstSucursales);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaBitacoraCxC(ref LOGI_ConexionSql_AD oConnection, LOGI_Bitacora_INFO oParam, ref LOGI_Bitacora_INFO oBitacora, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT * FROM lm_tms_cxc_factura ");

            if (!string.IsNullOrEmpty(oParam.seriefolio))
            {
                sConsultaSql += string.Format(" {0} seriefolio = '{1}'", bAnd ? "AND" : "WHERE", oParam.seriefolio);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oParam.folioD365))
            {
                sConsultaSql += string.Format(" {0} folio_d365 = '{1}'", bAnd ? "AND" : "WHERE", oParam.folioD365);
                bAnd = true;
            }


            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow orow = odataset.Tables[0].Rows[0];
                oBitacora.proceso_id_d365 = orow["proceso_id_d365"] == DBNull.Value ? "" : Convert.ToString(orow["proceso_id_d365"]);
                oBitacora.seriefolio = orow["seriefolio"] == DBNull.Value ? "" : Convert.ToString(orow["seriefolio"]);
            }
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaBitacoraCxP(ref LOGI_ConexionSql_AD oConnection, LOGI_Bitacora_INFO oParam, ref LOGI_Bitacora_INFO oBitacora, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT * FROM lm_tms_cxp_factura ");

            if (!string.IsNullOrEmpty(oParam.seriefolio))
            {
                sConsultaSql += string.Format(" {0} seriefolio = '{1}'", bAnd ? "AND" : "WHERE", oParam.seriefolio);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oParam.folioD365))
            {
                sConsultaSql += string.Format(" {0} folio_d365 = '{1}'", bAnd ? "AND" : "WHERE", oParam.folioD365);
                bAnd = true;
            }


            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow orow = odataset.Tables[0].Rows[0];
                oBitacora.proceso_id_d365 = orow["proceso_id_d365"] == DBNull.Value ? "" : Convert.ToString(orow["proceso_id_d365"]);
                oBitacora.seriefolio = orow["seriefolio"] == DBNull.Value ? "" : Convert.ToString(orow["seriefolio"]);
            }
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public DataSet ListaControlProcesos(ref LOGI_ConexionSql_AD oConnection, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();

            sConsultaSql = string.Format(@"SELECT FechaInicial, ProMovimientosBancarios, ProFacturas, ProCancelacionFacturas, ProNotasCredito, ProPasivos, FechaSolicitud, NuevaFechaSolicitud FROM lm_tms_ControlProcesos_vw ORDER BY FechaInicial;");

            return  oConnection.FillDataSet(sConsultaSql);
        }

        public string ActualizaControlProcesos(ref LOGI_ConexionSql_AD oConnection, LOGI_ControlProcesos_INFO cProceso, LOGI_EstadoControlProcesos_INFO eProceso, out string sConsultaSql)
        {
            string result = "OK";

            sConsultaSql = string.Format(@"EXEC lm_tms_ActualizaControlProcesos_usp @FechaInicial = '{0}',@FechaSolicitud = '{1}', @Estado  = {2};", cProceso.FechaInicial.ToString("yyyy-MM-dd HH:mm:ss.fff"), cProceso.FechaSolicitud.ToString("yyyy-MM-dd HH:mm:ss.fff"), cProceso.Estado);

            try
            {
                oConnection.ExecuteCommand(sConsultaSql);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            
            return result;
        }


    }
}
