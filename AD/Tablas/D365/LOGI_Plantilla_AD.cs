using INFO.Tablas.D365;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.D365
{
    public class LOGI_Plantilla_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_Plantilla_INFO oParam)
        {
            oParam.FolioAsistente = orow["FolioAsistente"] == DBNull.Value ? "" : Convert.ToString(orow["FolioAsistente"]);
            oParam.ano = orow["ano"] == DBNull.Value ? -1 : Convert.ToInt32(orow["ano"]);
            oParam.mes = orow["mes"] == DBNull.Value ? -1 : Convert.ToInt32(orow["mes"]);
            oParam.plantillanom = orow["plantillanombre"] == DBNull.Value ? "" : Convert.ToString(orow["plantillanombre"]);
            oParam.pathcabecera = orow["rutacabecera"] == DBNull.Value ? "" : Convert.ToString(orow["rutacabecera"]);
            oParam.pathdetalle = orow["rutadetalle"] == DBNull.Value ? "" : Convert.ToString(orow["rutadetalle"]);
            oParam.fechacreado = orow["fechacreado"] == DBNull.Value ? DateTime.MinValue.ToString() : Convert.ToDateTime(orow["fechacreado"]).ToString();
            oParam.activo = orow["activo"] == DBNull.Value ? -1 : Convert.ToInt32(orow["activo"]);
            oParam.fechaeliminado = orow["fechaelimiando"] == DBNull.Value ? "": Convert.ToDateTime(orow["fechaelimiando"]).ToString();
            oParam.tipo = orow["tipo"] == DBNull.Value ? -1 : Convert.ToInt32(orow["tipo"]);

        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Plantilla_INFO> lstPlantillas)
        {
            LOGI_Plantilla_INFO objTemp = new LOGI_Plantilla_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Plantilla_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstPlantillas.Add(objTemp);
            }
        }
        public string CuentaExistencias(ref LOGI_ConexionSql_AD oConnection, ref Int32 Totales, int anio, int mes, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            string response = string.Empty;
            sConsultaSql = string.Format(@"SELECT COUNT(FolioAsistente)+1 as total FROM lm_asientos_plantilla WHERE ano={0} and mes = {1}", anio, mes);
            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                Totales = Convert.ToInt32(odataset.Tables[0].Rows[0]["total"]);
                response = "OK";
            }
            else response = "SIN RESULTADOS";

            return response;
        }
        public string Creaplantilla(ref LOGI_ConexionSql_AD oConnection, LOGI_Plantilla_INFO oParam, out string sConsultaSql)
        {
            oHashParam = new Hashtable();            
            sConsultaSql = string.Format(@"INSERT lm_asientos_plantilla (FolioAsistente,ano,mes,plantillanombre,
                                   rutacabecera,rutadetalle,usuariocreo,tipo)
                                   VALUES (@FolioAsistente,@ano,@mes,@plantillanombre,
                                    @rutacabecera,@rutadetalle,@usuariocreo,@tipo)");
            oHashParam.Add("@FolioAsistente", oParam.FolioAsistente);
            oHashParam.Add("@ano", oParam.ano);
            oHashParam.Add("@mes", oParam.mes);
            oHashParam.Add("@plantillanombre", oParam.plantillanom);
            oHashParam.Add("@rutacabecera", oParam.pathcabecera);
            oHashParam.Add("@rutadetalle", oParam.pathdetalle);
            oHashParam.Add("@usuariocreo", oParam.usuariocreo);
            oHashParam.Add("@tipo", oParam.tipo);
            int icommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ActualizaPlantilla(ref LOGI_ConexionSql_AD oConnection, string UsuarioID, LOGI_Plantilla_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_asientos_plantilla");             

            if (oParam.activo >=0)
            {
                sConsultaSql += string.Format(" {0} activo = {1}", bSET ? "," : "SET", oParam.activo);
                bSET = true;
            }

            sConsultaSql += string.Format(",fechaelimiando = GETDATE(),usuarioelimino = {0} WHERE FolioAsistente = '{1}'", UsuarioID, oParam.FolioAsistente);
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }
        public string ListaPlantillaAsistentes(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Plantilla_INFO> lstDispersiones, LOGI_Plantilla_INFO oParam, out string sConsultaSql, int iTopresultados = -1)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT {0} * FROM lm_asientos_plantilla", iTopresultados > 0 ? string.Format("TOP {0}", iTopresultados) : "");

            if (!string.IsNullOrEmpty(oParam.FolioAsistente))
            {
                sConsultaSql += string.Format(" {0} FolioAsistente LIKE '%{1}%' ", bAnd ? "AND" : "WHERE", oParam.FolioAsistente.ToUpper());
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.FolioAsistentemath))
            {
                sConsultaSql += string.Format(" {0} FolioAsistente = '{1}' ", bAnd ? "AND" : "WHERE", oParam.FolioAsistentemath);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.plantillanom))
            {
                sConsultaSql += string.Format(" {0} UPPER(plantillanombre) LIKE '%{1}%' ", bAnd ? "AND" : "WHERE", oParam.plantillanom.ToUpper());
                bAnd = true;
            } 

            if (!string.IsNullOrEmpty(oParam.fechainicio) && !string.IsNullOrEmpty(oParam.fechafin))
            {
                DateTime odate = Convert.ToDateTime(oParam.fechafin);
                odate = odate.AddHours(23).AddMinutes(59).AddSeconds(59);
                string sFechaparse = string.Format("{0}", odate.ToString("yyyyMMdd HH:mm:ss"));
                sConsultaSql += string.Format(" {0} fechacreado  BETWEEN '{1}  00:00:00' AND '{2}' ", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oParam.fechainicio).ToString("yyyyMMdd"), sFechaparse);
                bAnd = true;
            }

            if (oParam.activo >= 0)
            {
                sConsultaSql += string.Format(" {0} activo = {1} ", bAnd ? "AND" : "WHERE", oParam.activo);
                bAnd = true;
            }

            if (oParam.tipo > 0)
            {
                sConsultaSql += string.Format(" {0} tipo = {1} ", bAnd ? "AND" : "WHERE", oParam.tipo);
                bAnd = true;
            }

            sConsultaSql += " order by fechacreado desc";
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstDispersiones);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
