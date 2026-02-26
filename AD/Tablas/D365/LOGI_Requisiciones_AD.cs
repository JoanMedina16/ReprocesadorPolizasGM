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
    public class LOGI_Requisiciones_AD
    {

        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_Requisicion_INFO oParam)
        {
            oParam.PURCHREQTYPE = orow["PURCHREQTYPE"] == DBNull.Value ? -1 : Convert.ToInt32(orow["PURCHREQTYPE"]);
            oParam.ORIGINATOR = orow["ORIGINATOR"] == DBNull.Value ? "" : Convert.ToString(orow["ORIGINATOR"]);
            oParam.RECID = orow["RECID"] == DBNull.Value ? "" : Convert.ToString(orow["RECID"]);
            oParam.PURCHREQID = orow["PURCHREQID"] == DBNull.Value ? "" : Convert.ToString(orow["PURCHREQID"]);
            oParam.PURCHREQNAME = orow["PURCHREQNAME"] == DBNull.Value ? "" : Convert.ToString(orow["PURCHREQNAME"]);
            oParam.REQUIREDDATE = orow["REQUIREDDATE"] == DBNull.Value ? "" : Convert.ToDateTime(orow["REQUIREDDATE"]).ToString("dd/MM/yyyy");
            oParam.REQUISITIONSTATUS = orow["REQUISITIONSTATUS"] == DBNull.Value ? -1 : Convert.ToInt32(orow["REQUISITIONSTATUS"]);
            oParam.TOTAL = orow["TOTAL"] == DBNull.Value ? -1 : Convert.ToDouble(orow["TOTAL"]);

            switch (oParam.REQUISITIONSTATUS)
            {
                case 0:
                    oParam.ESTATUS = "BORRADOR";
                    break;
                case 10:
                    oParam.ESTATUS = "EN REVISIÓN";
                    break;
                case 20:
                    oParam.ESTATUS = "RECHAZADO";
                    break;
                case 30:
                    oParam.ESTATUS = "APROBADO";
                    break;
                case 50:
                    oParam.ESTATUS = "CERRADO";
                    break;


                default:
                    oParam.ESTATUS = "NO DEFINIDO";
                    break;
            }
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Requisicion_INFO> lstRequisiciones)
        {
            LOGI_Requisicion_INFO objTemp = new LOGI_Requisicion_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Requisicion_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstRequisiciones.Add(objTemp);
            }
        }
        public string Listarequisiciones(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Requisicion_INFO> lstRequisiciones, LOGI_Requisicion_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT TOP 200  T1.PURCHREQTYPE,T1.ORIGINATOR,T1.RECID, T1.PURCHREQID, PURCHREQNAME,   
                                            T1.REQUIREDDATE, T1.ORIGINATOR, T1.REQUISITIONSTATUS, SUM(T2.LINEAMOUNT) AS TOTAL
                                            FROM [Warehouse].[PURCHREQTABLE] T1
                                            INNER JOIN [Warehouse].[PURCHREQLINE] T2
                                            ON T1.RECID = T2.PURCHREQTABLE 
											INNER JOIN Warehouse.DEFAULTDIMENSIONVIEW  T3
											ON T3.DEFAULTDIMENSION = T2.DEFAULTDIMENSION
                                            where T2.INVENTDIMIDDATAAREA = 'LOG' ");

            if (oParam.MINIMO_TOTAL > 0 && oParam.MAXIMO_TOTAL > 0)
                sConsultaSql += String.Format(" and T2.LINEAMOUNT >= {0} AND  T2.LINEAMOUNT <= {1} ", oParam.MINIMO_TOTAL, oParam.MAXIMO_TOTAL);
            else sConsultaSql += String.Format(" and T2.LINEAMOUNT >0 ");

            if (!string.IsNullOrEmpty(oParam.MAIACCOUNT))
            {
                sConsultaSql += string.Format(" AND T3.NAME = 'MainAccount' AND  T3.DISPLAYVALUE = '{0}'", oParam.MAIACCOUNT);
            }

            if (!string.IsNullOrEmpty(oParam.SUCURSAL))
            {
                sConsultaSql += string.Format(" AND T3.NAME = 'SUCURSAL' AND  T3.DISPLAYVALUE = '{0}'", oParam.SUCURSAL);
            }

            if (!string.IsNullOrEmpty(oParam.CENTRO_DE_COSTO))
            {
                sConsultaSql += string.Format(" AND T3.NAME = 'CENTRO_DE_COSTOS' AND  T3.DISPLAYVALUE = '{0}'", oParam.CENTRO_DE_COSTO);
            }

            if (oParam.REQUISITIONSTATUS >= 0)
            {
                sConsultaSql += string.Format(" AND T1.REQUISITIONSTATUS = {0}", oParam.REQUISITIONSTATUS);

            }

            if (!string.IsNullOrEmpty(oParam.DEPARTAMENTO))
            {
                sConsultaSql += string.Format(" AND T3.NAME = 'DEPARTAMENTO' AND  T3.DISPLAYVALUE = '{0}'", oParam.DEPARTAMENTO);
            }

            if (!string.IsNullOrEmpty(oParam.FECHA_INICIO) && !string.IsNullOrEmpty(oParam.FECHA_FINAL))
            {
                sConsultaSql += string.Format(" AND T1.REQUIREDDATE BETWEEN '{0}' AND '{1}'", oParam.FECHA_INICIO, oParam.FECHA_FINAL);
            }

            sConsultaSql += String.Format(@" GROUP BY T1.PURCHREQTYPE,T1.ORIGINATOR,T1.RECID, T1.PURCHREQID, PURCHREQNAME,
             T1.REQUIREDDATE, T1.ORIGINATOR, T1.REQUISITIONSTATUS
             order by REQUIREDDATE desc");

            

            //sConsultaSql += " order by fechacreado desc";
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstRequisiciones);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }


        public string Listarequisiciondetalle(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Requisicion_Line_INFO> lstLineas, string Foliorequi, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT * FROM [Warehouse].[PURCHREQLINE] WHERE PURCHREQTABLE  = {0}", Foliorequi);

            odataset = oConnection.FillDataSet(sConsultaSql);

            LOGI_Requisicion_Line_INFO otemp = new LOGI_Requisicion_Line_INFO();
            foreach (DataRow orow in odataset.Tables[0].Rows)
            {
                otemp = new LOGI_Requisicion_Line_INFO();
                otemp.DELIVERYNAME = orow["DELIVERYNAME"] == DBNull.Value ? "" : Convert.ToString(orow["DELIVERYNAME"]);
                otemp.ITEMID = orow["ITEMID"] == DBNull.Value ? "" : Convert.ToString(orow["ITEMID"]);
                otemp.TAXGROUP = orow["TAXGROUP"] == DBNull.Value ? "" : Convert.ToString(orow["TAXGROUP"]);
                otemp.PURCHPRICE = orow["PURCHPRICE"] == DBNull.Value ? -1 : Convert.ToDouble(orow["PURCHPRICE"]);
                otemp.PURCHQTY = orow["PURCHQTY"] == DBNull.Value ? -1 : Convert.ToDouble(orow["PURCHQTY"]);
                otemp.LINEAMOUNT = orow["LINEAMOUNT"] == DBNull.Value ? -1 : Convert.ToDouble(orow["LINEAMOUNT"]);

                otemp.NAME = orow["NAME"] == DBNull.Value ? "" : Convert.ToString(orow["NAME"]);

                lstLineas.Add(otemp);
            }

            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
