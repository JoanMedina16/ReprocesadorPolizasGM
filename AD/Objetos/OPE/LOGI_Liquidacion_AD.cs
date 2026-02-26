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
   public class LOGI_Liquidacion_AD
    {
        public string NuevaBitacoraLiquidacion(ref LOGI_ConexionSql_AD oConnection, LOGI_LiquidacionBitacora_INFO oParam, out string sConsultaSql)
        {
            int iCommand = 0;
            Hashtable oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_tms_liquidacion_viaje(folio_tms,folio_viaje,operador_id,tractor_id)
            VALUES(@folio_tms,@folio_viaje,@operador_id,@tractor_id)");
            oHashParam.Add("@folio_tms", oParam.folioTMS);
            oHashParam.Add("@folio_viaje", oParam.folioViaje);
            oHashParam.Add("@operador_id", oParam.operadorID);
            oHashParam.Add("@tractor_id", oParam.tractoID);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }
        public string ListaBitacora(ref LOGI_ConexionSql_AD oConnection, LOGI_LiquidacionBitacora_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Format(@"SELECT * FROM lm_tms_liquidacion_viaje where folio_tms = '{0}'", oParam.folioTMS);
            odataset = oConnection.FillDataSet(sConsultaSql);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaConceptosCombustibles(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Combustibles_INFO> lstConceptos, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT * FROM lm_combus_tms WHERE activo = 1 ");
             
            odataset = oConnection.FillDataSet(sConsultaSql);
            LOGI_Combustibles_INFO otemp = null;
            foreach (DataRow orow in odataset.Tables[0].Rows)
            {
                otemp = new LOGI_Combustibles_INFO();
                otemp.cuentatms = orow["cuenta_tms"] == DBNull.Value ? "" : Convert.ToString(orow["cuenta_tms"]);
                otemp.Concepto = orow["concepto"] == DBNull.Value ? "" : Convert.ToString(orow["concepto"]);
                otemp.cuentad365 = orow["cuenta_365"] == DBNull.Value ? "" : Convert.ToString(orow["cuenta_365"]);
                otemp.almacen365 = orow["almacen_365"] == DBNull.Value ? "" : Convert.ToString(orow["almacen_365"]);
                otemp.sucursal365 = orow["sucursal_365"] == DBNull.Value ? "" : Convert.ToString(orow["sucursal_365"]);
                lstConceptos.Add(otemp);
            }
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }


        public string ListaBitacoraCombustible(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_LiquidacionCombus_INFO> lstCombustibles, LOGI_LiquidacionCombus_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT * FROM lm_combus_cargas_tms  ");

            if (!string.IsNullOrEmpty(oParam.sFolios)  )
            {
                sConsultaSql += string.Format(" {0} folio_id IN({1})", bAnd ? "AND" : "WHERE", oParam.sFolios);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.vehiculo))
            {
                sConsultaSql += string.Format(" {0} corporativo like '%1%'", bAnd ? "AND" : "WHERE", oParam.vehiculo);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.economico))
            {
                sConsultaSql += string.Format(" {0} economico like '%1%'", bAnd ? "AND" : "WHERE", oParam.economico);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.poliza))
            {
                sConsultaSql += string.Format(" {0} poliza like '%1%'", bAnd ? "AND" : "WHERE", oParam.poliza);
                bAnd = true;
            } 

            if (oParam.estatus > 0)
            {
                sConsultaSql += string.Format(" {0} estatus = {1}", bAnd ? "AND" : "WHERE", oParam.estatus);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.estacionclave))
            {
                sConsultaSql += string.Format(" {0} codestacion = '{1}'", bAnd ? "AND" : "WHERE", oParam.estacionclave);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.FechaInicio) && !string.IsNullOrEmpty(oParam.FechaFinal))
            {
                DateTime odate = Convert.ToDateTime(oParam.FechaFinal);
                odate = odate.AddHours(23).AddMinutes(59).AddSeconds(59);
                string sFechaparse = string.Format("{0}", odate.ToString("yyyyMMdd HH:mm:ss"));
                sConsultaSql += string.Format(" {0} fecharegistro  BETWEEN '{1}  00:00:00' AND '{2}' ", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oParam.FechaInicio).ToString("yyyyMMdd"), sFechaparse);
                bAnd = true;
            }

            odataset = oConnection.FillDataSet(sConsultaSql);
            LOGI_LiquidacionCombus_INFO otemp = null;
            foreach (DataRow orow in odataset.Tables[0].Rows)
            {
                otemp = new LOGI_LiquidacionCombus_INFO();
                otemp.bitacoraID = orow["folio_id"] == DBNull.Value ? 1 : Convert.ToInt32(orow["folio_id"]);
                otemp.comentarios = orow["comentarios"] == DBNull.Value ? "" : Convert.ToString(orow["comentarios"]);
                otemp.codigooper = orow["codigooperador"] == DBNull.Value ? "" : Convert.ToString(orow["codigooperador"]);
                otemp.operador = orow["nombreperador"] == DBNull.Value ? "" : Convert.ToString(orow["nombreperador"]);
                otemp.fechaLiquidacion = orow["fechaliquidacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(orow["fechaliquidacion"]);
                otemp.sFechaLiquidacion = otemp.fechaLiquidacion.ToString("dd/MM/yyyy");
                otemp.fecharegistro = orow["fecharegistro"] == DBNull.Value ? DateTime.MinValue  : Convert.ToDateTime(orow["fecharegistro"]);
                otemp.sFecharegistro = otemp.fecharegistro.ToString("dd/MM/yyyy");
                otemp.estacion = orow["estacion"] == DBNull.Value ? "" : Convert.ToString(orow["estacion"]);

                otemp.economico = orow["economico"] == DBNull.Value ? "" : Convert.ToString(orow["economico"]);
                otemp.vehiculo = orow["corporativo"] == DBNull.Value ? "" : Convert.ToString(orow["corporativo"]);

                otemp.folio = orow["folio"] == DBNull.Value ? "" : Convert.ToString(orow["folio"]);
                otemp.folioelectronico = orow["folioelectronico"] == DBNull.Value ? "" : Convert.ToString(orow["folioelectronico"]);

                otemp.litros = orow["litros"] == DBNull.Value ? 1 : Convert.ToDecimal(orow["litros"]);
                otemp.preciolitro = orow["preciolitro"] == DBNull.Value ? 1 : Convert.ToDecimal(orow["preciolitro"]);
                otemp.total = orow["total"] == DBNull.Value ? 1 : Convert.ToDecimal(orow["total"]);
                otemp.estatus = orow["estatus"] == DBNull.Value ? 1 : Convert.ToInt32(orow["estatus"]);

                otemp.poliza = orow["poliza"] == DBNull.Value ? "" : Convert.ToString(orow["poliza"]);
                otemp.foliod365 = orow["foliod365"] == DBNull.Value ? "" : Convert.ToString(orow["foliod365"]);
                otemp.viaje = orow["folioviaje"] == DBNull.Value ? "" : Convert.ToString(orow["folioviaje"]);
                otemp.sucursal = orow["sucursal"] == DBNull.Value ? "" : Convert.ToString(orow["sucursal"]);                
                otemp.centro = orow["centro"] == DBNull.Value ? "" : Convert.ToString(orow["centro"]);
                otemp.area = orow["area"] == DBNull.Value ? "" : Convert.ToString(orow["area"]);
                otemp.depto = orow["depto"] == DBNull.Value ? "" : Convert.ToString(orow["depto"]);
                otemp.filial = orow["filial"] == DBNull.Value ? "" : Convert.ToString(orow["filial"]);
                lstCombustibles.Add(otemp);
            }
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

    }
}
