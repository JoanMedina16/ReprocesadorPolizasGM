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
   public class LOGI_Transacciones_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_Transacciones_INFO otemp)
        {
            if (orow.Table.Columns.Contains("FolioAsiento"))
                otemp.FolioAsiento = orow["FolioAsiento"] == DBNull.Value ? "" : Convert.ToString(orow["FolioAsiento"]);            
            if (orow.Table.Columns.Contains("intento"))
                otemp.intento = orow["intento"] == DBNull.Value ? -1 : Convert.ToInt32(orow["intento"]);
            if (orow.Table.Columns.Contains("peticion"))
                otemp.peticion = orow["peticion"] == DBNull.Value ? "" : Convert.ToString(orow["peticion"]);
            if (orow.Table.Columns.Contains("mensaje"))
                otemp.mensaje = orow["mensaje"] == DBNull.Value ? "" : Convert.ToString(orow["mensaje"]);
            if (orow.Table.Columns.Contains("urlweb"))
                otemp.urlweb = orow["urlweb"] == DBNull.Value ? "" : Convert.ToString(orow["urlweb"]);
            if (orow.Table.Columns.Contains("respuesta"))
                otemp.respuesta = orow["respuesta"] == DBNull.Value ? "" : Convert.ToString(orow["respuesta"]);
            if (orow.Table.Columns.Contains("FechaModificacion"))
                otemp.FechaModificacion = orow["FechaModificacion"] == DBNull.Value ? "" : Convert.ToDateTime(orow["FechaModificacion"]).ToString();
            
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Transacciones_INFO> lstDetalle)
        {
            LOGI_Transacciones_INFO objTemp = new LOGI_Transacciones_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Transacciones_INFO();
                this.GetObjeto(oorow, ref objTemp);
                lstDetalle.Add(objTemp);
            }
        }
        /// <summary>
        /// Descripción: Query utilizado para recuperar las transacciones (movimientos de polizas a enviar a D365)
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Objeto de tipo conexión hacia base de datos</param>
        /// <param name="lstTransacciones">Listado referenciado utilizado para retornar todos los registros encontrados</param>
        /// <param name="oParam">Objeto utilizado para filtros de búsqueda</param>
        /// <param name="sConsultaSql">Consulta retornada</param>
        /// <returns></returns>
        public string ListaTransacciones(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Transacciones_INFO> lstTransacciones, LOGI_Transacciones_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT FolioAsiento,intento,mensaje FROM lm_transacc");

            if (!string.IsNullOrEmpty( oParam.FolioAsiento))
            {
                sConsultaSql += string.Format(" {0} FolioAsiento = '{1}'", bAnd ? "AND" : "WHERE", oParam.FolioAsiento);
                bAnd = true;
            }
            if (oParam.intento_mayor > 0)
            {
                sConsultaSql += string.Format(" {0} intento >= {1}", bAnd ? "AND" : "WHERE", oParam.intento_mayor);
                bAnd = true;
            }
            if (oParam.intento_menor > 0)
            {
                sConsultaSql += string.Format(" {0} intento <= {1}", bAnd ? "AND" : "WHERE", oParam.intento_menor);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstTransacciones);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
        /// <summary>
        /// Descripción: Query utilizado para recuperar el json enviado hacia D365
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Objeto de tipo conexión hacia base de datos</param>
        /// <param name="oParam">Objeto referenciado utilizado para retornar el json enviado a D365</param>
        /// <param name="asiento_id">Identificador de la transacción a recuperar</param>
        /// <param name="sConsultaSql">Consulta retornada</param>
        /// <returns></returns>
        public string RecuperaResponse(ref LOGI_ConexionSql_AD oConnection, ref  LOGI_Transacciones_INFO oParam, String asiento_id, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"WITH Bitacora (FolioAsiento, FechaModificacion,peticion, mensaje, urlweb, respuesta) AS ( SELECT documento_tms, fecha_bit, '', mensaje, url, mensaje FROM OTMLog..lm_tms_bitacora ) SELECT * FROM Bitacora");

            if (!string.IsNullOrEmpty(asiento_id))
            {
                sConsultaSql += string.Format(" {0} FolioAsiento = '{1}'", bAnd ? "AND" : "WHERE", asiento_id);
                bAnd = true;
            }
            List<LOGI_Transacciones_INFO> lstTransacciones = new List<LOGI_Transacciones_INFO>();
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstTransacciones);
            if (lstTransacciones.Count > 0)
            {
                oParam = lstTransacciones[0];
                return "OK";
            }
            else return "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Query que permite realizar una actualización de los registros basados por el parametro oParam
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Objeto de tipo conexión hacia base de datos</param>
        /// <param name="oParam">Objeto utilizado para filtros de actualización</param>
        /// <param name="sConsultaSql">Consulta retornada</param>
        /// <returns></returns>
        public string ActualizaTransaccion(ref LOGI_ConexionSql_AD oConnection, LOGI_Transacciones_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_transacc");

            if (oParam.intento >= 0)
            {
                sConsultaSql += string.Format(" {0} intento = {1}", bSET ? "," : "SET", oParam.intento);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.urlweb))
            {
                sConsultaSql += string.Format(" {0} urlweb = '{1}'", bSET ? "," : "SET", oParam.urlweb);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.peticion))
            {
                sConsultaSql += string.Format(" {0} peticion = '{1}'", bSET ? "," : "SET", oParam.peticion);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.respuesta))
            {
                sConsultaSql += string.Format(" {0} respuesta = '{1}'", bSET ? "," : "SET", oParam.respuesta);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.mensaje))
            {
                sConsultaSql += string.Format(" {0} mensaje = '{1}'", bSET ? "," : "SET", oParam.mensaje);
                bSET = true;
            }

            sConsultaSql += string.Format(" {0} FechaModificacion = GETDATE()", bSET ? "," : "SET");
            bSET = true;


            sConsultaSql += string.Format(@" WHERE FolioAsiento = '{0}'", oParam.FolioAsiento);
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Query que elimina la línea de transacción. Estos registros se eliminan una vez que el documento contable ha sido creado en D365
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Objeto de tipo conexión hacia base de datos</param>
        /// <param name="oParam">Objeto utilizado para filtros de actualización</param>
        /// <param name="sConsultaSql">Consulta retornada</param>
        /// <returns></returns>
        public string EliminaTransaccion(ref LOGI_ConexionSql_AD oConnection, String FolioAsiento, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;            
            sConsultaSql = string.Format(@"DELETE lm_transacc WHERE FolioAsiento = '{0}'", FolioAsiento);
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
