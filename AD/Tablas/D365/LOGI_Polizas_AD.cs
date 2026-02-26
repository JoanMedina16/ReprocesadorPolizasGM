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
    public class LOGI_Polizas_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_Polizas_INFO otemp)
        {
            otemp.estatus =orow["estatus"] == DBNull.Value ? -1 : Convert.ToInt32(orow["estatus"]);
            otemp.FolioAsiento = orow["FolioAsiento"] == DBNull.Value ? "" : Convert.ToString(orow["FolioAsiento"]);
            otemp.id_tipo_doc = orow["id_tipo_doc"] == DBNull.Value ? -1 : Convert.ToInt32(orow["id_tipo_doc"]);
            otemp.Nombredocumento = orow["nombre"] == DBNull.Value ? "" : Convert.ToString(orow["nombre"]);
            otemp.eTypedoc = (INFO.Enums.eDocumentoPolizas)otemp.id_tipo_doc;
            otemp.fechaContable = orow["fechaContable"] == DBNull.Value ? "" : Convert.ToDateTime(orow["fechaContable"]).ToString("dd/MM/yyyy");
            otemp.fechaCreacion = orow["fechaCreacion"] == DBNull.Value ? "" : Convert.ToDateTime(orow["fechaCreacion"]).ToString("dd/MM/yyyy");
            otemp.recIdD365 = orow["recIdD365"] == DBNull.Value ? "" : Convert.ToString(orow["recIdD365"]);
            otemp.total = orow["total"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["total"]);
            otemp.subtotal = orow["subtotal"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["subtotal"]);
            otemp.impuesto = orow["impuesto"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["impuesto"]);
            otemp.rfc = orow["rfc"] == DBNull.Value ? "" : Convert.ToString(orow["rfc"]);
            otemp.nombrerfc = orow["nombrerfc"] == DBNull.Value ? "" : Convert.ToString(orow["nombrerfc"]);
            otemp.uuid = orow["uuid"] == DBNull.Value ? "" : Convert.ToString(orow["uuid"]);
            otemp.folio = orow["folio"] == DBNull.Value ? "" : Convert.ToString(orow["folio"]);
            otemp.serie = orow["serie"] == DBNull.Value ? "" : Convert.ToString(orow["serie"]);
            otemp.doctoref = orow["doctoref"] == DBNull.Value ? "" : Convert.ToString(orow["doctoref"]);
            otemp.uuidref = orow["uuidref"] == DBNull.Value ? "" : Convert.ToString(orow["uuidref"]);
            otemp.folioserie = string.Format("{0}{1}", otemp.serie, otemp.folio);
            otemp.errortimbrado = orow["errortimbrado"] == DBNull.Value ? 0 : Convert.ToInt32(orow["errortimbrado"]);

        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Polizas_INFO> lstPolizas)
        {
            LOGI_Polizas_INFO objTemp = new LOGI_Polizas_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Polizas_INFO();
                this.GetObjeto(oorow, ref objTemp);
                lstPolizas.Add(objTemp);
            }
        }
        /// <summary>
        /// Descripción: Query utilizado para recuperar las polizas contables creadas a través de las interfaces de logistica,
        /// se listan todos los estatus 0 = pendiente de sincronizar. 1 = errores al sincronizarse. 2 = replicada con éxito en D365
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="lstPolizas">Lista de objetos referenciada</param>
        /// <param name="oParam">Objeto de tipo poliaza para filtro de datos</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ListaPolizas(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Polizas_INFO> lstPolizas, LOGI_Polizas_INFO oParam, out string sConsultaSql, int Top = 0, bool bAscendete = false)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT {0} * FROM (SELECT uuidref = '', doctoref = FacturaRef, FolioAsiento = Folio365, id_tipo_doc = IdModulo, nombre = Modulo, fechaContable = FechaContable, fechaCreacion = Fecha, recIdD365 = ID365, estatus = Estado, total = Total, subtotal = SubTotal, impuesto = 0.00, rfc = RFCSocio, nombrerfc = NombreSocio, uuid = UUID, folio = '', serie = Factura, errortimbrado = 0 FROM OTMLog..lm_tms_bitacora_vw ", Top > 0 ? string.Format("TOP {0}", Top) : "");
            //sConsultaSql += string.Format(@"SELECT {0} uuidref, doctoref, FolioAsiento, id_tipo_doc, T2.nombre, fechaContable, fechaCreacion, recIdD365, estatus, total, subtotal, impuesto, rfc, 
            //                               nombrerfc,uuid,folio,serie,errortimbrado FROM lm_asientos_d365 T1 INNER JOIN lm_documentos_d365 T2 ON T1.id_tipo_doc = T2.id", Top > 0 ? string.Format("TOP {0}", Top) : "" );
            sConsultaSql += ") Asientos365";

            if (oParam.estatus >=0)
            {
                sConsultaSql += string.Format(" {0} estatus = {1}", bAnd ? "AND" : "WHERE", oParam.estatus);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oParam.FolioAsiento))
            {
                sConsultaSql += string.Format(" {0} FolioAsiento LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParam.FolioAsiento);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.uuid))
            {
                sConsultaSql += string.Format(" {0} uuid = '{1}'", bAnd ? "AND" : "WHERE", oParam.uuid);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.folioserie))
            {
                sConsultaSql += string.Format(" {0} (serie+''+folio) LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParam.folioserie);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.folio))
            {
                sConsultaSql += string.Format(" {0} folio = '{1}'", bAnd ? "AND" : "WHERE", oParam.folio);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.serie))
            {
                sConsultaSql += string.Format(" {0} serie = '{1}'", bAnd ? "AND" : "WHERE", oParam.serie);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.sDocumentosIN))
            {
                sConsultaSql += string.Format(" {0} id_tipo_doc IN({1})", bAnd ? "AND" : "WHERE", oParam.sDocumentosIN.TrimEnd(','));
                bAnd = true;
            }

            if (oParam.id_tipo_doc >0)
            {
                sConsultaSql += string.Format(" {0} id_tipo_doc = {1} ", bAnd ? "AND" : "WHERE", oParam.id_tipo_doc);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.rfc))
            {
                sConsultaSql += string.Format(" {0} rfc = '{1}'", bAnd ? "AND" : "WHERE", oParam.rfc);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.recIdD365))
            {
                sConsultaSql += string.Format(" {0} recIdD365 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParam.recIdD365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.FechaConInicio) && !string.IsNullOrEmpty(oParam.FechaConFin))
            {
                DateTime odate = Convert.ToDateTime(oParam.FechaConFin);
                odate = odate.AddHours(23).AddMinutes(59).AddSeconds(59);
                string sFechaparse = string.Format("{0}", odate.ToString("yyyyMMdd HH:mm:ss"));
                sConsultaSql += string.Format(" {0} fechaContable  BETWEEN '{1}  00:00:00' AND '{2}' ", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oParam.FechaConInicio).ToString("yyyyMMdd"), sFechaparse);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.FechaCreInicio) && !string.IsNullOrEmpty(oParam.FechaCreFin))
            {
                DateTime odate = Convert.ToDateTime(oParam.FechaCreFin);
                odate = odate.AddHours(23).AddMinutes(59).AddSeconds(59);
                string sFechaparse = string.Format("{0}", odate.ToString("yyyyMMdd HH:mm:ss"));
                sConsultaSql += string.Format(" {0} fechaCreacion  BETWEEN '{1}  00:00:00' AND '{2}' ", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oParam.FechaCreInicio).ToString("yyyyMMdd"), sFechaparse);
                bAnd = true;
            }

            sConsultaSql += string.Format(" order by fechaCreacion {0}", bAscendete ? "ASC" : "DESC");
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstPolizas);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string InsertaPeticion(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Peticion_INFO> lstPolizas, LOGI_Peticion_INFO oParam, out string sConsultaSql, int Top = 0, bool bAscendete = false)
        {

            sConsultaSql = string.Empty;
            
            sConsultaSql = string.Format(@"EXEC lm_tms_ControlProcesos_EnviaPeticion_usp @FechaInicial = '{0}', @Proceso = {1}, @Usuario = '{2}';", oParam.FechaInicial, oParam.Proceso, oParam.Usuario);

            try
            {
                oConnection.ExecuteCommand(sConsultaSql);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }


        /// <summary>
        /// Descripción: Metodo utilizado para retornar el error grabado en la poliza por motivos de descarte
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="oPoliza">Objecto con comentarios devuelto</param>
        /// <param name="oParam">Objeto de tipo poliaza para filtro de datos</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string Listacomentario(ref LOGI_ConexionSql_AD oConnection, ref LOGI_Polizas_INFO oPoliza, LOGI_Polizas_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"select comentario from lm_asientos_d365");
            if (!string.IsNullOrEmpty(oParam.FolioAsientoMatch))
            {
                sConsultaSql += string.Format(" {0} FolioAsiento = '{1}'", bAnd ? "AND" : "WHERE", oParam.FolioAsientoMatch);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            DataRow row = odataset.Tables[0].Rows[0];
            oPoliza.comments = row["comentario"] == DBNull.Value ? "Sin comentarios del usuario. El registro fue descartado en apartado errores, por duplicidad en sincronización o factura duplicada." : Convert.ToString(row["comentario"]);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Query utilizado para la actualización de los datos de la tabla lm_asientos_d365, se actualiza según los datos 
        /// enviados en objeto oParam
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades a actualizar</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ActualizaPoliza(ref LOGI_ConexionSql_AD oConnection, string sUsuarioID, LOGI_Polizas_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_asientos_d365");

            if (!string.IsNullOrEmpty(oParam.recIdD365))
            {
                sConsultaSql += string.Format(" {0} recIdD365 = '{1}'", bSET ? "," : "SET", oParam.recIdD365);
                bSET = true;
            }

            if (oParam.estatus >= 0)
            {
                sConsultaSql += string.Format(" {0} estatus = {1}", bSET ? "," : "SET", oParam.estatus);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.docxml))
            {
                sConsultaSql += string.Format(" {0} docxml = @docxml", bSET ? "," : "SET", oParam.docxml);
                oHashParam.Add("@docxml", oParam.docxml);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.folio))
            {
                sConsultaSql += string.Format(" {0} folio = '{1}'", bSET ? "," : "SET", oParam.folio);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.serie))
            {
                sConsultaSql += string.Format(" {0} serie = '{1}'", bSET ? "," : "SET", oParam.serie);
                bSET = true;
            }

            if (oParam.total > 0)
            {
                sConsultaSql += string.Format(" {0} total = {1}", bSET ? "," : "SET", oParam.total);
                bSET = true;
            }

            if (oParam.subtotal > 0)
            {
                sConsultaSql += string.Format(" {0} subtotal = {1}", bSET ? "," : "SET", oParam.subtotal);
                bSET = true;
            }

            if (oParam.impuesto > 0)
            {
                sConsultaSql += string.Format(" {0} impuesto = {1}", bSET ? "," : "SET", oParam.impuesto);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.rfc))
            {
                sConsultaSql += string.Format(" {0} rfc = '{1}'", bSET ? "," : "SET", oParam.rfc);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.nombrerfc))
            {
                sConsultaSql += string.Format(" {0} nombrerfc = '{1}'", bSET ? "," : "SET", oParam.nombrerfc);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.uuid))
            {
                sConsultaSql += string.Format(" {0} uuid = '{1}'", bSET ? "," : "SET", oParam.uuid);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.uuidref))
            {
                sConsultaSql += string.Format(" {0} uuidref = '{1}'", bSET ? "," : "SET", oParam.uuidref);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.comments))
            {
                sConsultaSql += string.Format(" {0} comentario = '{1}'", bSET ? "," : "SET", oParam.comments);
                bSET = true;
            }


            sConsultaSql += string.Format(" {0} idUsuarioEdita = {1}", bSET ? "," : "SET", sUsuarioID);
            bSET = true;
            sConsultaSql += string.Format(" {0} fechaEdicion = GETDATE()", bSET ? "," : "SET");
            bSET = true;

            sConsultaSql += string.Format(" WHERE FolioAsiento = '{0}'", oParam.FolioAsiento);
            int icommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Query utilizado para recuperar el XML de un registro de poliza contable
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="oPoliza">Referencia de objeto para retornar el valor del XML</param>
        /// <param name="id_transaccion">Valor a filtrar para recuperar el XML</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string RecuperaXML(ref LOGI_ConexionSql_AD oConnection, ref LOGI_Polizas_INFO oPoliza, String id_transaccion, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"SELECT docxml FROM lm_asientos_d365 WHERE FolioAsiento = '{0}'", id_transaccion);
            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow orow = odataset.Tables[0].Rows[0];
                oPoliza = new LOGI_Polizas_INFO();
                oPoliza.docxml = orow["docxml"] == DBNull.Value ? "" : Convert.ToString(orow["docxml"]);
                return "OK";
            }
            else return "SIN RESULTADOS";
        }
    }
}
