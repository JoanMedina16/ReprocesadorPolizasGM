using INFO.Objetos.D365;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos.D365
{
   public class LOGI_Dispersiones_AD
    {
        internal Hashtable oHashParam = null;
        internal string docto = string.Empty;
        void GetObjeto(DataRow orow, ref LOGI_Dispersion_INFO oParam)
        {
            oParam.ano = orow["ano"] == DBNull.Value ? -1 : Convert.ToInt32(orow["ano"]);
            oParam.mes = orow["mes"] == DBNull.Value ? -1 : Convert.ToInt32(orow["mes"]);
            oParam.poliza = orow["poliza"] == DBNull.Value ? -1 : Convert.ToInt32(orow["poliza"]);
            oParam.linea = orow["linea"] == DBNull.Value ? -1 : Convert.ToInt32(orow["linea"]);
            oParam.formato = orow["formato"] == DBNull.Value ? -1 : Convert.ToInt32(orow["formato"]);
            oParam.fecha = orow["fecha"] == DBNull.Value ? DateTime.MinValue.ToString("dd/MM/yyyy") : Convert.ToDateTime(orow["fecha"]).ToString("dd/MM/yyyy");
            
            docto =  orow["docto"] == DBNull.Value ? "" : Convert.ToString(orow["docto"]).Trim();
            oParam.descrip = string.Format("{0}-{1}/{2}/{3}", docto, oParam.formato, oParam.mes, oParam.poliza);
            //oParam.descrip = orow["descrip"] == DBNull.Value ? "" : Convert.ToString(orow["descrip"]).Trim();
            oParam.cargo = orow["cargo"] == DBNull.Value ? -1 : Convert.ToDouble(orow["cargo"]);
            oParam.operador = orow["nombre"] == DBNull.Value ? "" : Convert.ToString(orow["nombre"]).Trim();
            oParam.rfc = orow["rfc"] == DBNull.Value ? "" : Convert.ToString(orow["rfc"]).Trim();
            oParam.cuenta = orow["cuentaContable"] == DBNull.Value ? "" : Convert.ToString(orow["cuentaContable"]).Trim();
            oParam.refbanco = orow["referenciaBancaria"] == DBNull.Value ? "" : Convert.ToString(orow["referenciaBancaria"]).Trim();
            oParam.cia = orow["cia"] == DBNull.Value ? -1 : Convert.ToInt32(orow["cia"]);
            oParam.suc = orow["suc"] == DBNull.Value ? -1 : Convert.ToInt32(orow["suc"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Dispersion_INFO> lstDispersiones)
        {
            LOGI_Dispersion_INFO objTemp = new LOGI_Dispersion_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Dispersion_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstDispersiones.Add(objTemp);
            }
        }

        public string ActualizaPoliza(ref LOGI_ConexionSql_AD oConnection, LOGI_Dispersion_INFO oParam, out string sConsultaSql)
        {
            oHashParam = new Hashtable();            
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_polizas_d365");

            if (oParam.usado > 0)
            {
                sConsultaSql += string.Format(" {0} usado = {1}", bSET ? "," : "SET", oParam.usado);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.FolioAsistente))
            {
                sConsultaSql += string.Format(" {0} FolioAsistente = '{1}'", bSET ? "," : "SET", oParam.FolioAsistente);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.refbanco))
            {
                sConsultaSql += string.Format(" {0} referenciaBancaria = '{1}'", bSET ? "," : "SET", oParam.refbanco);
                bSET = true;
            }

            sConsultaSql += string.Format(" WHERE ano = {0}	AND mes = {1}	AND poliza = {2} AND linea = {3} ", oParam.ano, oParam.mes, oParam.poliza, oParam.linea);
            int icommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ReactivaPoliza(ref LOGI_ConexionSql_AD oConnection, LOGI_Dispersion_INFO oParam, out string sConsultaSql)
        {
            oHashParam = new Hashtable();
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_polizas_d365");

            if (oParam.usado >= 0)
            {
                sConsultaSql += string.Format(" {0} usado = {1}", bSET ? "," : "SET", oParam.usado);
                bSET = true;
            }  

            sConsultaSql += string.Format(" WHERE FolioAsistente = '{0}' ", oParam.FolioAsistente);
            int icommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaDispersiones(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Dispersion_INFO> lstDispersiones, LOGI_Dispersion_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = true;
            sConsultaSql = string.Format(@"SELECT T2.formato, T2.docto, T2.cia, T2.suc,T1.ano, T1.mes, T1.poliza, T1.linea, T2.fecha, T2.descrip, T2.cargo, T3.nombre, T4.rfc, T1.cuentaContable, T1.referenciaBancaria 
                                            FROM lm_polizas_d365 T1 INNER JOIN
                                            oa_polizas T2 ON (T1.ano = T2.ano AND T1.mes = T2.mes AND T1.poliza = T2.poliza
                                            and T1.formato = T2.formato) LEFT JOIN oa_scuentas T3 on (T3.cuenta = T2.cuenta AND 
                                            T3.scuenta = T2.scuenta) LEFT JOIN rh T4 on (T4.emp = T3.scuenta)
                                            where T2.cuenta = 25");

            if (!string.IsNullOrEmpty(oParam.operador))
            {
                sConsultaSql += string.Format(" {0} T3.nombre LIKE '%{1}%' ", bAnd ? "AND" : "WHERE", oParam.operador.ToUpper());
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.rfc))
            {
                sConsultaSql += string.Format(" {0} T4.rfc LIKE '%{1}%' ", bAnd ? "AND" : "WHERE", oParam.rfc.ToUpper());
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.viaje))
            {
                sConsultaSql += string.Format(" {0} T2.docto LIKE '%{1}%' ", bAnd ? "AND" : "WHERE", oParam.viaje.ToUpper());
                bAnd = true;
            }


            if (!string.IsNullOrEmpty(oParam.sucursal))
            {
                sConsultaSql += string.Format(" {0} T2.suc = {1} ", bAnd ? "AND" : "WHERE", oParam.sucursal);
                bAnd = true;
            }

            if (oParam.usado >= 0)
            {
                sConsultaSql += string.Format(" {0} T1.usado = {1} ", bAnd ? "AND" : "WHERE", oParam.usado);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.fechainicio) && !string.IsNullOrEmpty(oParam.fechafin))
            {
                DateTime odate = Convert.ToDateTime(oParam.fechafin);
                odate = odate.AddHours(23).AddMinutes(59).AddSeconds(59);
                string sFechaparse = string.Format("{0}", odate.ToString("yyyyMMdd HH:mm:ss"));
                sConsultaSql += string.Format(" {0} T2.fecha  BETWEEN '{1}  00:00:00' AND '{2}' ", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oParam.fechainicio).ToString("yyyyMMdd"), sFechaparse);
                bAnd = true;
            }

            sConsultaSql += " order by T2.fecha desc";
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstDispersiones);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
