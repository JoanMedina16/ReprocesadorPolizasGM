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
    public class LOGI_Polizas_detalle_AD
    {
        
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow orow, ref LOGI_Polizas_detalle_INFO otemp)
        {
            otemp.FolioAsiento = orow["FolioAsiento"] == DBNull.Value ? "" : Convert.ToString(orow["FolioAsiento"]);
            otemp.linea = orow["linea"] == DBNull.Value ? -1 : Convert.ToInt32(orow["linea"]);
            otemp.mayor = orow["mayor"] == DBNull.Value ? -1 : Convert.ToInt32(orow["mayor"]);
            otemp.cuenta = orow["cuenta"] == DBNull.Value ? -1 : Convert.ToInt32(orow["cuenta"]);
            otemp.scuenta = orow["scuenta"] == DBNull.Value ? 0 : Convert.ToInt32(orow["scuenta"]);
            otemp.cargo = orow["cargo"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["cargo"]);
            otemp.abono = orow["abono"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["abono"]);
            otemp.importe = orow["importe"] == DBNull.Value ? 0 : Convert.ToDecimal(orow["importe"]);
            otemp.sucursal = orow["sucursal"] == DBNull.Value ? "" : Convert.ToString(orow["sucursal"]);
            otemp.centrocosto = orow["centrocosto"] == DBNull.Value ? "" : Convert.ToString(orow["centrocosto"]);
            otemp.departamento = orow["departamento"] == DBNull.Value ? "" : Convert.ToString(orow["departamento"]);
            otemp.area = orow["area"] == DBNull.Value ? "" : Convert.ToString(orow["area"]);
            otemp.vehiculo = orow["vehiculo"] == DBNull.Value ? "" : Convert.ToString(orow["vehiculo"]);
            otemp.descrip = orow["descrip"] == DBNull.Value ? "" : Convert.ToString(orow["descrip"]);
            if (orow.Table.Columns.Contains("usaxml"))
                otemp.usaxml = orow["usaxml"] == DBNull.Value ? 0 : Convert.ToInt32(orow["usaxml"]);
            if (orow.Table.Columns.Contains("uuid"))
                otemp.uuid = orow["uuid"] == DBNull.Value ? "" : Convert.ToString(orow["uuid"]);

            if (orow.Table.Columns.Contains("esfrontera"))
                otemp.esfrontera = orow["esfrontera"] == DBNull.Value ? 0 : Convert.ToInt32(orow["esfrontera"]);

            if (orow.Table.Columns.Contains("esdeducible"))
                otemp.esdeducible = orow["esdeducible"] == DBNull.Value ? 0 : Convert.ToInt32(orow["esdeducible"]);

            if (orow.Table.Columns.Contains("usaiva"))
                otemp.usaiva = orow["usaiva"] == DBNull.Value ? 0 : Convert.ToInt32(orow["usaiva"]);

            if (orow.Table.Columns.Contains("docxml"))
                otemp.XML = orow["docxml"] == DBNull.Value ? "" : Convert.ToString(orow["docxml"]);

            if (orow.Table.Columns.Contains("IVA16"))
                otemp.IVA16 = orow["IVA16"] == DBNull.Value ? 0 : Convert.ToInt32(orow["IVA16"]);

            if (orow.Table.Columns.Contains("refdoc"))
                otemp.refdoc = orow["refdoc"] == DBNull.Value ? "" : Convert.ToString(orow["refdoc"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Polizas_detalle_INFO> lstDetalle)
        {
            LOGI_Polizas_detalle_INFO objTemp = new LOGI_Polizas_detalle_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Polizas_detalle_INFO();
                this.GetObjeto(oorow, ref objTemp);
                lstDetalle.Add(objTemp);
            }
        }
        /// <summary>
        /// Descripción: Query utilizado para recuperar el detalle del asiento contable a grabar en D365, la información
        /// está vinculada a la tabla lm_asientos_d365 ó lm_asientos_cxc_detalle por el campo id_asiento.
        /// La tabla a consumir está determinada por el Enumerador eDocumentoPolizas
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="lstDetalle">Lista de objetos referenciada</param>
        /// <param name="oParam">Objeto de tipo poliaza para filtro de datos</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ListaDetallePoliza(string CONST_TABLE, ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Polizas_detalle_INFO> lstDetalle, LOGI_Polizas_detalle_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT * FROM {0}", CONST_TABLE);
            if (!string.IsNullOrEmpty( oParam.FolioAsiento))
            {
                sConsultaSql += string.Format(" {0} FolioAsiento = '{1}'", bAnd ? "AND" : "WHERE", oParam.FolioAsiento);
                bAnd = true;
            }
            if (oParam.linea > 0)
            {
                sConsultaSql += string.Format(" {0} linea = {1}", bAnd ? "AND" : "WHERE", oParam.linea);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oParam.uuid))
            {
                sConsultaSql += string.Format(" {0} uuid = '{1}'", bAnd ? "AND" : "WHERE", oParam.uuid);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oParam.refdoc))
            {
                sConsultaSql += string.Format(" {0} refdoc = '{1}'", bAnd ? "AND" : "WHERE", oParam.refdoc);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstDetalle);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        } 

        /// <summary>
        /// Descripción: Query utilizado para devolver el XML de la tabla gastos <<lm_asientos_gto_detalle>>
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="oDetalle">Lista de objetos referenciada</param>
        /// <param name="oParam">Objeto de tipo poliaza para filtro de datos</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string RecuperaXMLLinea(string CONST_TABLE, ref LOGI_ConexionSql_AD oConnection, ref LOGI_Polizas_detalle_INFO oDetalle, LOGI_Polizas_detalle_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT docxml FROM {0}", CONST_TABLE);
            if (!string.IsNullOrEmpty(oParam.FolioAsiento))
            {
                sConsultaSql += string.Format(" {0} FolioAsiento = '{1}'", bAnd ? "AND" : "WHERE", oParam.FolioAsiento);
                bAnd = true;
            }
            if (oParam.linea > 0)
            {
                sConsultaSql += string.Format(" {0} linea = {1}", bAnd ? "AND" : "WHERE", oParam.linea);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oParam.uuid))
            {
                sConsultaSql += string.Format(" {0} uuid = '{1}'", bAnd ? "AND" : "WHERE", oParam.uuid);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            DataRow row = odataset.Tables[0].Rows[0];
            oDetalle.XML = row["docxml"] == DBNull.Value ? "" : Convert.ToString(row["docxml"]);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Query utilizado para la actualización de los datos de la tabla detalle ( La tabla a consumir está determinada por el Enumerador eDocumentoPolizas), se actualiza según los datos 
        /// enviados en objeto oParam
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades a actualizar</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ActualizaLineapoliza(string CONST_TABLE, ref LOGI_ConexionSql_AD oConnection, string sUsuarioID, LOGI_Polizas_detalle_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE {0}", CONST_TABLE);

            if (!string.IsNullOrEmpty(oParam.uuid))
            {
                sConsultaSql += string.Format(" {0} uuid = '{1}'", bSET ? "," : "SET", oParam.uuid);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.XML))
            {
                sConsultaSql += string.Format(" {0} docxml = '{1}'", bSET ? "," : "SET", oParam.XML);
                bSET = true;
            }

            sConsultaSql += string.Format(" WHERE FolioAsiento = '{0}' AND linea = {1}", oParam.FolioAsiento, oParam.linea);
            int icommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

    }
}
