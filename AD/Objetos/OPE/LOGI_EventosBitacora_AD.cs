using INFO.Tablas.D365;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos.OPE
{
   public class LOGI_EventosBitacora_AD
    {
        public string NuevaBitacoraERROR(ref LOGI_ConexionSql_AD oConnection, LOGI_PolizasD365_INFO oParam, out string sConsultaSql)
        {
            int iCommand = 0;
            Hashtable oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_tms_bitacora(servicio,documento_tms,mensaje,errores,socio,rfcsocio,fechadocto,subtotal,total,factura,pathenviado,facturaref,uuid,url,estatus)
            VALUES(@servicio,@documento_tms,@mensaje,1,@socio,@rfcsocio,@fechadocto,@subtotal,@total,@factura,@pathenviado,@facturaref,@uuid,@url,1)");
            oHashParam.Add("@servicio", Convert.ToInt32(oParam.tipo));
            oHashParam.Add("@documento_tms", oParam.folio);
            oHashParam.Add("@mensaje", oParam.mensaje);
            oHashParam.Add("@socio", oParam.socio); 
            oHashParam.Add("@rfcsocio", oParam.rfc);
            oHashParam.Add("@fechadocto", oParam.fechaconta);
            oHashParam.Add("@subtotal", oParam.subtotal);
            oHashParam.Add("@total", oParam.total);
            oHashParam.Add("@factura", oParam.factura);
            oHashParam.Add("@pathenviado", oParam.JSONPATH);
            oHashParam.Add("@facturaref", oParam.facturaref);
            oHashParam.Add("@uuid", oParam.uuid);
            oHashParam.Add("@url", oParam.URL); 
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        void GetObjeto(DataRow orow, ref LOGI_PolizasD365_INFO otemp)
        {
            otemp.estatus = orow["estatus"] == DBNull.Value ? -1 : Convert.ToInt32(orow["estatus"]);
            otemp.errores = orow["errores"] == DBNull.Value ? -1 : Convert.ToInt32(orow["errores"]);
            otemp.enviado = orow["enviado"] == DBNull.Value ? -1 : Convert.ToInt32(orow["enviado"]);
            otemp.enviados = orow["envios"] == DBNull.Value ? -1 : Convert.ToInt32(orow["envios"]);
            otemp.IdRegistro = orow["bitacora_id"] == DBNull.Value ? -1 : Convert.ToInt32(orow["bitacora_id"]);
            otemp.folio = orow["documento_tms"] == DBNull.Value ? "" : Convert.ToString(orow["documento_tms"]);
            otemp.tipo = (INFO.Enums.eDocumentoTMS)(orow["servicio"] == DBNull.Value ? -1 : Convert.ToInt32(orow["servicio"]));
            otemp.fecharegistro = orow["fecha"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(orow["fecha"]);

            otemp.socio = orow["socio"] == DBNull.Value ? "" : Convert.ToString(orow["socio"]);
            otemp.rfc = orow["rfcsocio"] == DBNull.Value ? "" : Convert.ToString(orow["rfcsocio"]);
            otemp.fechaconta = orow["fechadocto"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(orow["fechadocto"]);
            otemp.subtotal = orow["subtotal"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["subtotal"]);
            otemp.total = orow["total"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["total"]);

            otemp.factura = orow["factura"] == DBNull.Value ? "" : Convert.ToString(orow["factura"]);
            otemp.JSONPATH = orow["pathenviado"] == DBNull.Value ? "" : Convert.ToString(orow["pathenviado"]);
            otemp.facturaref = orow["facturaref"] == DBNull.Value ? "" : Convert.ToString(orow["facturaref"]);
            otemp.uuid = orow["uuid"] == DBNull.Value ? "" : Convert.ToString(orow["uuid"]);
            otemp.URL = orow["url"] == DBNull.Value ? "" : Convert.ToString(orow["url"]);
            otemp.mensaje = orow["mensaje"] == DBNull.Value ? "" : Convert.ToString(orow["mensaje"]);
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
        /// <summary>
        /// Descripción: Query utilizado para recuperar las polizas contables creadas a través de las interfaces de logistica,
        /// se listan todos los estatus 0 = pendiente de sincronizar. 1 = errores al sincronizarse. 2 = replicada con éxito en D365
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="lstPolizas">Lista de objetos referenciada</param>
        /// <param name="oParam">Objeto de tipo poliaza para filtro de datos</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ListaBitacoraPolizas(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_PolizasD365_INFO> lstPolizas, LOGI_PolizasD365_INFO oParam, out string sConsultaSql, int Top = 0, bool bAscendete = false, bool bCamposcorreo = false)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT {0} * from lm_tms_bitacora", Top > 0 ? string.Format("TOP {0}", Top) : "");



            if (!string.IsNullOrEmpty(oParam.factura))
            {
                sConsultaSql += string.Format(" {0} factura LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParam.factura);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.folio))
            {
                sConsultaSql += string.Format(" {0} documento_tms = '{1}'", bAnd ? "AND" : "WHERE", oParam.folio);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.rfc))
            {
                sConsultaSql += string.Format(" {0} rfcsocio LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParam.rfc);
                bAnd = true;
            }

            if (oParam.enviado >= 0)
            {
                sConsultaSql += string.Format(" {0} enviado = {1} ", bAnd ? "AND" : "WHERE", oParam.enviado);
                bAnd = true;
            }

             if (oParam.estatus >= 0)
            {
                sConsultaSql += string.Format(" {0} estatus = {1} ", bAnd ? "AND" : "WHERE", oParam.estatus);
                bAnd = true;
            }

            if (oParam.tipo > 0)
            {
                sConsultaSql += string.Format(" {0} servicio = {1} ", bAnd ? "AND" : "WHERE", Convert.ToInt32(oParam.tipo));
                bAnd = true;
            }



            if (!string.IsNullOrEmpty(oParam.FechaInicio) && !string.IsNullOrEmpty(oParam.FechaFinal))
            {
                DateTime odate = Convert.ToDateTime(oParam.FechaFinal);
                odate = odate.AddHours(23).AddMinutes(59).AddSeconds(59);
                string sFechaparse = string.Format("{0}", odate.ToString("yyyyMMdd HH:mm:ss"));
                sConsultaSql += string.Format(" {0} fechadocto  BETWEEN '{1}  00:00:00' AND '{2}' ", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oParam.FechaInicio).ToString("yyyyMMdd"), sFechaparse);
                bAnd = true;
            }


            sConsultaSql += string.Format(" order by fecha {0}", bAscendete ? "ASC" : "DESC");
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstPolizas);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ActualizaBitacoraPoliza(ref LOGI_ConexionSql_AD oConnection, string sUsuarioID, LOGI_PolizasD365_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
             sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_tms_bitacora ");
             

            if (!string.IsNullOrEmpty(oParam.mensaje))
            {
                sConsultaSql += string.Format(" {0} mensaje = '{1}'", bSET ? "," : "SET", oParam.mensaje);
                bSET = true;
            }

            if (oParam.errores >= 0)
            {
                sConsultaSql += string.Format(" {0} errores = {1}", bSET ? "," : "SET", oParam.errores);
                bSET = true;
            }

            if (oParam.enviados >= 0)
            {
                sConsultaSql += string.Format(" {0} envios = {1}", bSET ? "," : "SET", oParam.enviados);
                bSET = true;
            }

            if (oParam.enviado >= 0)
            {
                sConsultaSql += string.Format(" {0} enviado = {1}", bSET ? "," : "SET", oParam.enviado);
                bSET = true;
            }

            if (oParam.estatus >= 0)
            {
                sConsultaSql += string.Format(" {0} estatus = {1}", bSET ? "," : "SET", oParam.estatus);
                bSET = true;
            }


            sConsultaSql += string.Format(" WHERE bitacora_id = {0}  ", oParam.IdRegistro );
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
