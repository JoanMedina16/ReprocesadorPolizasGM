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
    public class LOGI_PolizasD365_AD
    {
        internal Hashtable oHashParam = null;


        public string Creadocumento(ref LOGI_ConexionSql_AD oConnection, string sTabla, string sUsuarioID, LOGI_PolizasD365_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"INSERT {0} (folio,socio,nombre,rfc,factura,uuid,facturaref,uuidref,fechaconta,fecharegistro,impuesto,subtotal,total,tipo,archivo,recid)
                                   VALUES (@folio,@socio,@nombre,@rfc,@factura,@uuid,@facturaref,@uuidref,@fechaconta,@fecharegistro,@impuesto,@subtotal,@total,@tipo,@archivo,@recid)", sTabla);
            oHashParam.Add("@folio", oParam.folio);
            oHashParam.Add("@socio", oParam.socio);
            oHashParam.Add("@nombre", oParam.nombresocio);
            oHashParam.Add("@rfc", oParam.rfc);
            oHashParam.Add("@factura", oParam.factura);
            oHashParam.Add("@uuid", oParam.uuid);
            oHashParam.Add("@facturaref", oParam.facturaref);
            oHashParam.Add("@uuidref", oParam.uuidref); 
            oHashParam.Add("@fechaconta", oParam.fechaconta);
            oHashParam.Add("@fecharegistro", oParam.fecharegistro);
            oHashParam.Add("@impuesto", oParam.impuesto);
            oHashParam.Add("@subtotal", oParam.subtotal);
            oHashParam.Add("@total", oParam.total);
            oHashParam.Add("@tipo", oParam.tipo);
            oHashParam.Add("@recid", oParam.recid);
            oHashParam.Add("@archivo", oParam.JSONPATH);
            int icommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaPolizas(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_PolizasD365_INFO> lstPolizas, LOGI_PolizasD365_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT  * FROM lm_cxc_polizas T1 INNER JOIN lm_documentos_d365 T2 ON T1.tipo = T2.id");


            if (!string.IsNullOrEmpty(oParam.folio))
            {
                sConsultaSql += string.Format(" {0} folio LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParam.folio);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.uuid))
            {
                sConsultaSql += string.Format(" {0} uuid = '{1}'", bAnd ? "AND" : "WHERE", oParam.uuid);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.factura))
            {
                sConsultaSql += string.Format(" {0} factura LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParam.factura);
                bAnd = true;
            }
         
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstPolizas);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        void GetObjeto(DataRow orow, ref LOGI_PolizasD365_INFO otemp)
        {
            otemp.folio = orow["folio"] == DBNull.Value ? "" : Convert.ToString(orow["folio"]);
            otemp.factura = orow["factura"] == DBNull.Value ? "" : Convert.ToString(orow["factura"]);
            //otemp.tipo = orow["tipo"] == DBNull.Value ? -1 : Convert.ToInt32(orow["id_tipo_doc"]);
            otemp.recid = orow["recid"] == DBNull.Value ? 0 : Convert.ToInt32(orow["recid"]);
            otemp.JSONPATH = orow["archivo"] == DBNull.Value ? "" : Convert.ToString(orow["archivo"]);

        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_PolizasD365_INFO> lstPolizas)
        {
            LOGI_PolizasD365_INFO objTemp = new LOGI_PolizasD365_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_PolizasD365_INFO();
                this.GetObjeto(oorow, ref objTemp);
                lstPolizas.Add(objTemp);
            }
        }
    }
}
