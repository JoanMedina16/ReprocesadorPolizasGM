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
    public class LOGI_FacturasCFDI_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Factura_INFO oFactura)
        {
            oFactura.Folio = objRow["ifolio"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["ifolio"]);
            oFactura.serie = objRow["serie"] == DBNull.Value ? "" : Convert.ToString(objRow["serie"]);
            oFactura.sUUID = objRow["sUUID"] == DBNull.Value ? "" : Convert.ToString(objRow["sUUID"]);
            oFactura.sXML = objRow["sXML"] == DBNull.Value ? "" : Convert.ToString(objRow["sXML"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Factura_INFO> lstFacturas)
        {
            LOGI_Factura_INFO objTemp = new LOGI_Factura_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Factura_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstFacturas.Add(objTemp);
            }
        } 
        public string RecuperaXML(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Factura_INFO> lstFacturas, LOGI_Factura_INFO oFactura, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false; sConsultaSql = @"SELECT ifolio,serie,sUUID,sXML FROM lm_historico_xml ";

            if (!string.IsNullOrEmpty(oFactura.serie))
            {
                sConsultaSql += string.Format(" {0}  serie = '{1}'", bAnd ? "AND" : "WHERE", oFactura.serie);
                bAnd = true;
            }             
            if (oFactura.Folio > 0)
            {
                sConsultaSql += string.Format(" {0} ifolio = {1}", bAnd ? "AND" : "WHERE", oFactura.Folio);
                bAnd = true;
            } 
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstFacturas);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }


        public string RecuperaFacturaBandjeaLM(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Factura_INFO> lstFacturas, LOGI_Factura_INFO oFactura, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false; sConsultaSql = @"SELECT serie,folio,folioviaje FROM lm_facturas ";

            if (!string.IsNullOrEmpty(oFactura.FolioAsiento))
            {
                sConsultaSql += string.Format(" {0}  FolioAsiento = '{1}'", bAnd ? "AND" : "WHERE", oFactura.FolioAsiento);
                bAnd = true;
            }

            odataset = oConnection.FillDataSet(sConsultaSql);
            LOGI_Factura_INFO otemp = new LOGI_Factura_INFO();
            foreach (DataRow orow in odataset.Tables[0].Rows)
            {
                otemp = new LOGI_Factura_INFO();
                otemp.Docto = orow["folioviaje"] == DBNull.Value ? "" : Convert.ToString(orow["folioviaje"]);               
                lstFacturas.Add(otemp);
            }            
            return lstFacturas.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListadatosAdiciolFactura(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Factura_INFO> lstFacturas, LOGI_Factura_INFO oFactura, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false; sConsultaSql = @"SELECT sctaadi,valor FROM lm_facadis ";

            if (!string.IsNullOrEmpty(oFactura.serie))
            {
                sConsultaSql += string.Format(" {0}  serie = '{1}'", bAnd ? "AND" : "WHERE", oFactura.serie);
                bAnd = true;
            }
            if (oFactura.Folio > 0)
            {
                sConsultaSql += string.Format(" {0} folio = {1}", bAnd ? "AND" : "WHERE", oFactura.Folio);
                bAnd = true;
            }

            odataset = oConnection.FillDataSet(sConsultaSql);
            LOGI_Factura_INFO otemp = new LOGI_Factura_INFO();
            foreach (DataRow orow in odataset.Tables[0].Rows)
            {
                otemp = new LOGI_Factura_INFO();
                otemp.Valor = orow["valor"] == DBNull.Value ? "" : Convert.ToString(orow["valor"]);
                otemp.subcuentaadi = orow["sctaadi"] == DBNull.Value ? -1 : Convert.ToInt32(orow["sctaadi"]);
                lstFacturas.Add(otemp);
            }
            return lstFacturas.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Query utilizado para la actualización de los datos de la tabla lm_facturas, se actualiza según los datos 
        /// enviados en objeto oParam
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades a actualizar</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ActualizaLMFactura(ref LOGI_ConexionSql_AD oConnection, String FolioAsiento, String DocumentoD365, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            int icommand = -1;
            sConsultaSql = string.Empty;
            sConsultaSql += string.Format(@"update lm_facturas set polizad365 = '{0}' where  FolioAsiento = '{1}'", DocumentoD365, FolioAsiento);
            icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
