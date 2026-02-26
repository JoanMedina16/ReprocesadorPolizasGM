using INFO.Objetos.SAT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos.OPE
{
  public  class LOGI_Historico_XMLS_AD
    {
        internal Hashtable oHashParam = null;
        public string EditaFoliocontrol(ref LOGI_ConexionSql_AD oConnection, int anio, int mes, DateTime? dtFechaCFDI, out string sConsultaSql, int finalizado = -1)
        {
            bool bSET = false;
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = "UPDATE lm_control_XML ";
            if (dtFechaCFDI != null)
            {
                sConsultaSql += string.Format(" {0} ultfechaCFDI = @ultfechaCFDI", bSET ? "," : "SET", dtFechaCFDI);
                oHashParam.Add("@ultfechaCFDI", dtFechaCFDI);
                bSET = true;
            }

            if (finalizado > 0)
            {
                sConsultaSql += string.Format(" {0} finalizado = {1}", bSET ? "," : "SET", finalizado);
                bSET = true;
            }
            sConsultaSql += string.Format(" WHERE anio = {0} and mes = {1}", anio, mes);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string FolioControl(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_ControlXMLS_INFO> lstFolios, out string sConsultaSql, int anio = 0, int mes = 0, int finalizado = -1)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT * FROM lm_control_XML ";

            if (anio > 0)
            {
                sConsultaSql += string.Format(" {0} anio = {1}", bAnd ? "AND" : "WHERE", anio);
                bAnd = true;
            }
            if (mes > 0)
            {
                sConsultaSql += string.Format(" {0} mes = {1}", bAnd ? "AND" : "WHERE", mes);
                bAnd = true;
            }
            if (finalizado > 0)
            {
                sConsultaSql += string.Format(" {0} finalizado = {1}", bAnd ? "AND" : "WHERE", finalizado);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            LOGI_ControlXMLS_INFO otemp = null;
            foreach (DataRow row in odataset.Tables[0].Rows)
            {
                otemp = new LOGI_ControlXMLS_INFO();
                otemp.anio = row["anio"] == DBNull.Value ? 0 : Convert.ToInt32(row["anio"]);
                otemp.mes = row["mes"] == DBNull.Value ? 0 : Convert.ToInt32(row["mes"]);
                otemp.finalizado = row["finalizado"] == DBNull.Value ? 0 : Convert.ToInt32(row["finalizado"]);
                otemp.ultfechaCFDI = row["ultfechaCFDI"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["ultfechaCFDI"]);
                lstFolios.Add(otemp);
            }
            return lstFolios.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoDocumento(ref LOGI_ConexionSql_AD oConnection, LOGI_XMLS_INFO objFormato, int anio, int mes, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();

            sConsultaSql = string.Format(@"INSERT INTO lm_historico_xml(ifolio,serie,sUUID,sXML,sFecha,sTipoComprobante,sFormaPago,sCondicionesdePago,sMetodoPago,srfcemisor,srfcreceptor,sUsoCFDI,subtotal,total,anio,mes) 
                                                                  VALUES(@ifolio,@serie,@sUUID,@sXML,@sFecha,@TipoDeComprobante,@sFormaPago,@sCondicionesdePago,@sMetodoPago,@srfcemisor,@srfcreceptor,@sUsoCFDI,@subtotal,@total,@anio,@mes)");
            oHashParam.Add("@ifolio", objFormato.Folio);
            oHashParam.Add("@serie", objFormato.Serie == null ? "NA" : objFormato.Serie);
            oHashParam.Add("@sUUID", objFormato.Complemento.TimbreFiscalDigital.UUID);
            oHashParam.Add("@sXML", objFormato.CFDIContent);
            oHashParam.Add("@sFecha", objFormato.Fecha);
            oHashParam.Add("@TipoDeComprobante", objFormato.TipoDeComprobante ?? string.Empty);
            oHashParam.Add("@sFormaPago", objFormato.FormaPago ?? string.Empty);
            oHashParam.Add("@sCondicionesdePago", objFormato.CondicionesdePago ?? string.Empty);
            oHashParam.Add("@sMetodoPago", objFormato.MetodoPago ?? string.Empty);
            oHashParam.Add("@srfcemisor", objFormato.Emisor.Rfc);
            oHashParam.Add("@srfcreceptor", objFormato.Receptor.Rfc);
            oHashParam.Add("@sUsoCFDI", objFormato.Receptor.UsoCFDI ?? string.Empty);
            oHashParam.Add("@subtotal", objFormato.SubTotal);
            oHashParam.Add("@total", objFormato.Total);
            oHashParam.Add("@anio", anio);
            oHashParam.Add("@mes", mes);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }


        public string ActualizaDocumento(ref LOGI_ConexionSql_AD oConnection, LOGI_XMLS_INFO objFormato, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            bool bSet = false;
            sConsultaSql = string.Format("UPDATE lm_historico_xml ");
            if (!string.IsNullOrEmpty(objFormato.Complemento.TimbreFiscalDigital.UUID))
            {
                sConsultaSql += string.Format(" {0} sUUID =  '{1}'", bSet ? "," : "SET", objFormato.Complemento.TimbreFiscalDigital.UUID);
                bSet = true;
            }

            if (!string.IsNullOrEmpty(objFormato.CFDIContent))
            {
                sConsultaSql += string.Format(" {0} sXML = '{1}'", bSet ? "," : "SET", objFormato.CFDIContent);
                bSet = true;
            }

            sConsultaSql += string.Format("WHERE ifolio = {0} AND serie = '{1}'", objFormato.Folio, objFormato.Serie == null ? "NA" : objFormato.Serie);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }



        public string NuevoFoliocontrol(ref LOGI_ConexionSql_AD oConnection, int anio, int mes, DateTime dtFechaCFDI, out string sConsultaSql, int finalizado = 0)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_control_XML(anio,mes,ultfechaCFDI,finalizado) VALUES(
                                          @anio,@mes,@ultfechaCFDI,@finalizado)");
            oHashParam.Add("@anio", anio);
            oHashParam.Add("@mes", mes);
            oHashParam.Add("@ultfechaCFDI", dtFechaCFDI);
            oHashParam.Add("@finalizado", finalizado);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaCFDIs(ref LOGI_ConexionSql_AD oConnection, LOGI_XMLS_INFO objFormato, out string sConsultaSql, ref Int32 iRegistros, int anio = 0, int mes = 0)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT  ifolio,serie,sUUID FROM lm_historico_xml ";

            if (!string.IsNullOrEmpty(objFormato.Folio))
            {
                sConsultaSql += string.Format(" {0} ifolio =  '{1}'", bAnd ? "AND" : "WHERE", objFormato.Folio);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(objFormato.Serie))
            {
                sConsultaSql += string.Format(" {0} serie = '{1}'", bAnd ? "AND" : "WHERE", objFormato.Serie);
                bAnd = true;
            }
            if (anio > 0)
            {
                sConsultaSql += string.Format(" {0} anio = {1}", bAnd ? "AND" : "WHERE", anio);
                bAnd = true;
            }
            if (mes > 0)
            {
                sConsultaSql += string.Format(" {0} mes = {1}", bAnd ? "AND" : "WHERE", mes);
                bAnd = true;
            }
            if (objFormato.Complemento != null)
            {
                if (!string.IsNullOrEmpty(objFormato.Complemento.TimbreFiscalDigital.UUID))
                {
                    sConsultaSql += string.Format(" {0} sUUID = '{1}'", bAnd ? "AND" : "WHERE", objFormato.Complemento.TimbreFiscalDigital.UUID);
                    bAnd = true;
                }
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            iRegistros = odataset.Tables[0].Rows.Count;
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaArchivosXML(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_XMLS_INFO> lstsFolios , LOGI_XMLS_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT  * FROM lm_historico_xml ";

            if (!string.IsNullOrEmpty(oParam.sFoliosIN))
            {
                sConsultaSql += string.Format(" {0} ifolio IN ({1})", bAnd ? "AND" : "WHERE", oParam.sFoliosIN);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.sSeriesIN))
            {
                sConsultaSql += string.Format(" {0} serie IN({1})", bAnd ? "AND" : "WHERE", oParam.sSeriesIN);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            LOGI_XMLS_INFO oXML = null;
            if (odataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in odataset.Tables[0].Rows)
                {
                    oXML = new LOGI_XMLS_INFO();
                    oXML.Emisor = new PersonaCFDI();
                    oXML.Receptor = new PersonaCFDI();
                    oXML.Folio = row["ifolio"] == DBNull.Value ? "" : Convert.ToString(row["ifolio"]);
                    oXML.Serie = row["serie"] == DBNull.Value ? "" : Convert.ToString(row["serie"]);
                    oXML.UUID = row["sUUID"] == DBNull.Value ? "" : Convert.ToString(row["sUUID"]);
                    oXML.Fecha = row["sFecha"] == DBNull.Value ? "" : Convert.ToString(row["sFecha"]);
                    oXML.TipoDeComprobante = row["sTipoComprobante"] == DBNull.Value ? "" : Convert.ToString(row["sTipoComprobante"]);
                    oXML.FormaPago = row["sFormaPago"] == DBNull.Value ? "" : Convert.ToString(row["sFormaPago"]);
                    oXML.CondicionesdePago = row["sCondicionesdePago"] == DBNull.Value ? "" : Convert.ToString(row["sCondicionesdePago"]);
                    oXML.MetodoPago = row["sMetodoPago"] == DBNull.Value ? "" : Convert.ToString(row["sMetodoPago"]);
                    oXML.Emisor.Rfc = row["srfcemisor"] == DBNull.Value ? "" : Convert.ToString(row["srfcemisor"]);
                    oXML.Receptor.Rfc = row["srfcreceptor"] == DBNull.Value ? "" : Convert.ToString(row["srfcreceptor"]);
                    oXML.Total = row["total"] == DBNull.Value ? 0 : Convert.ToDecimal(row["total"]);
                    oXML.SubTotal = row["subtotal"] == DBNull.Value ? 0 : Convert.ToDecimal(row["subtotal"]); 
                    if (oParam.bContent)
                        oXML.CFDIContent = row["sXML"] == DBNull.Value ? "" : Convert.ToString(row["sXML"]); 
                    lstsFolios.Add(oXML);                
                }
            }
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
