using INFO.Objetos.D365;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos.D365
{
    public class LOGI_Extraccion_ZAP_AD
    {
        internal Hashtable oHashParam = null;
        internal string docto = string.Empty;
        void GetObjeto(DataRow orow, ref LOGI_Extraccion_ZAP_INFO oParam)
        {
            //oParam.documento = orow["JOURNALNUM"] == DBNull.Value ? "" : Convert.ToString(orow["JOURNALNUM"]);
            //oParam.RecID = orow["RECID"] == DBNull.Value ? "" : Convert.ToString(orow["RECID"]);
            oParam.Fecha = orow["ACCOUNTINGDATE"] == DBNull.Value ? DateTime.MinValue.ToString("dd/MM/yyyy") : Convert.ToDateTime(orow["ACCOUNTINGDATE"]).ToString("dd/MM/yyyy");
            oParam.Display = orow["LEDGERACCOUNT"] == DBNull.Value ? "" : Convert.ToString(orow["LEDGERACCOUNT"]);
            oParam.cuenta = orow["CUENTA"] == DBNull.Value ? "" : Convert.ToString(orow["CUENTA"]);
            oParam.sucursal = orow["SUCURSAL"] == DBNull.Value ? "" : Convert.ToString(orow["SUCURSAL"]);
            oParam.filial = orow["FILIAL"] == DBNull.Value ? "" : Convert.ToString(orow["FILIAL"]);
            oParam.centro = orow["CC"] == DBNull.Value ? "" : Convert.ToString(orow["CC"]);
            oParam.depto = orow["DEPTO"] == DBNull.Value ? "" : Convert.ToString(orow["DEPTO"]);
            oParam.text = orow["TEXT"] == DBNull.Value ? "" : Convert.ToString(orow["TEXT"]);
            //oParam.vehiculo = orow["VEHICULO"] == DBNull.Value ? "" : Convert.ToString(orow["VEHICULO"]);
            oParam.debito = orow["SALDO"] == DBNull.Value ? -1 : Convert.ToDouble(orow["SALDO"]);
            oParam.Voucher = orow["SUBLEDGERVOUCHER"] == DBNull.Value ? "" : Convert.ToString(orow["SUBLEDGERVOUCHER"]);            
            //oParam.credito = orow["AMOUNTCURCREDIT"] == DBNull.Value ? -1 : Convert.ToDouble(orow["AMOUNTCURCREDIT"]);

        }

        void LoopDataSet(LOGI_Extraccion_ZAP_INFO oParam,DataSet objDataSet, ref List<LOGI_Extraccion_ZAP_INFO> lstDispersiones)
        {
            LOGI_Extraccion_ZAP_INFO objTemp = new LOGI_Extraccion_ZAP_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                

                objTemp = new LOGI_Extraccion_ZAP_INFO();
                this.GetObjeto(orow, ref objTemp);
                if (oParam.DepFiscal)
                {
                    if (objTemp.text.Contains("DPFIS00"))
                        continue;
                }

                lstDispersiones.Add(objTemp);
            }
        }

        public string ListaVehiculo(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Extraccion_Vehiculo_INFO> lstVehiculos, string Descripciones, string Vouchers, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = String.Format(@"SELECT  OFFSETTXT, VOUCHER,TXT FROM 
                Warehouse.LEDGERJOURNALTRANS  VH WITH(nolock)

                WHERE COMPANY = 'LOG' AND VOUCHER IN ({0})
                AND TXT IN ({1})
                GROUP BY OFFSETTXT, VOUCHER,TXT", Vouchers, Descripciones);

            LOGI_Extraccion_Vehiculo_INFO otemp = null;
            odataset = oConnection.FillDataSet(sConsultaSql, iTimeOut:999999999);
            foreach (DataRow orow in odataset.Tables[0].Rows)
            {
                otemp = new  LOGI_Extraccion_Vehiculo_INFO();
                otemp.Voucher = orow["VOUCHER"] == DBNull.Value ? "" : Convert.ToString(orow["VOUCHER"]);
                otemp.Descripcion = orow["TXT"] == DBNull.Value ? "" : Convert.ToString(orow["TXT"]);
                otemp.Vehiculo = orow["OFFSETTXT"] == DBNull.Value ? "" : Convert.ToString(orow["OFFSETTXT"]);
                lstVehiculos.Add(otemp);
            }
            return lstVehiculos.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string Listacuentasdiario(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Extraccion_ZAP_INFO> lstcuentas, LOGI_Extraccion_ZAP_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = String.Format(@"select GJE.SUBLEDGERVOUCHER, GJE.ACCOUNTINGDATE,--VEH.JOURNALNUM,
                                    GJAE.LEDGERACCOUNT,GJAE.TEXT,  MA.MAINACCOUNTID AS CUENTA,
                                    ISNULL(SUC.DISPLAYVALUE, 'NINGUNO') AS SUCURSAL, ISNULL(FIL.DISPLAYVALUE, 'NINGUNO') AS FILIAL,
                                    ISNULL(CC.DISPLAYVALUE, 'NINGUNO') AS CC, ISNULL(DEP.DISPLAYVALUE, 'NINGUNO') AS DEPTO,
                                    ISNULL(AREA.DISPLAYVALUE, 'NINGUNO') AS AREA,
                                    SUM(GJAE.ACCOUNTINGCURRENCYAMOUNT) AS SALDO 

									--,ISNULL(VEH.OFFSETTXT, 'NINGUNO') AS VEHICULO
									from Warehouse.GENERALJOURNALACCOUNTENTRY GJAE WITH(nolock)
                                    INNER JOIN Warehouse.GENERALJOURNALENTRY GJE ON GJE.RECID = GJAE.GENERALJOURNALENTRY  AND
                                    GJE.PARTITION = '5637144576' INNER JOIN Warehouse.LEDGER L ON L.RECID = GJE.LEDGER AND
                                    L.PARTITION = '5637144576' INNER JOIN Warehouse.DimensionAttributeValueCombination DAVC WITH(nolock) ON
                                    DAVC.RECID = GJAE.LEDGERDIMENSION AND DAVC.PARTITION = '5637144576' INNER JOIN Warehouse.MAINACCOUNT
                                    MA WITH(nolock) ON MA.RECID = DAVC.MAINACCOUNT AND MA.PARTITION = '5637144576' INNER JOIN
                                    Warehouse.DimensionAttributeLevelValueView SUC WITH(nolock) ON SUC.VALUECOMBINATIONRECID = DAVC.RECID
                                    AND SUC.DIMENSIONATTRIBUTE = '5637145326' AND SUC.PARTITION = '5637144576' INNER JOIN
                                    Warehouse.DimensionAttributeLevelValueView FIL WITH(nolock) ON
                                    FIL.VALUECOMBINATIONRECID = DAVC.RECID AND FIL.DIMENSIONATTRIBUTE = '5637145331' AND FIL.PARTITION = '5637144576'
                                    LEFT JOIN Warehouse.DimensionAttributeLevelValueView CC WITH(nolock) ON
                                    CC.VALUECOMBINATIONRECID = DAVC.RECID AND CC.DIMENSIONATTRIBUTE = '5637145329' AND
                                    CC.PARTITION = '5637144576' LEFT JOIN Warehouse.DimensionAttributeLevelValueView DEP WITH(nolock) ON
                                    DEP.VALUECOMBINATIONRECID = DAVC.RECID AND DEP.DIMENSIONATTRIBUTE = '5637145328' AND
                                    DEP.PARTITION = '5637144576'  

LEFT JOIN Warehouse.DimensionAttributeLevelValueView AREA WITH(nolock) ON
                                    AREA.VALUECOMBINATIONRECID = DAVC.RECID AND AREA.DIMENSIONATTRIBUTE = '5637145327' AND
                                    DEP.PARTITION = '5637144576'

									--LEFT JOIN Warehouse.LEDGERJOURNALTRANS VEH WITH(nolock)
									--ON (VEH.VOUCHER = GJE.SUBLEDGERVOUCHER
									--and VEH.TXT = GJAE.TEXT)
									WHERE L.NAME = 'LOG'  and ");

            if (!string.IsNullOrEmpty(oParam.fecha_inicio) && !string.IsNullOrEmpty(oParam.fecha_final))
            {
                DateTime INICIO = Convert.ToDateTime(oParam.fecha_inicio);
                DateTime FINAL = Convert.ToDateTime(oParam.fecha_final);
                sConsultaSql += string.Format("year(gje.ACCOUNTINGDATE) = {0}  and month(GJE.ACCOUNTINGDATE) between  {1} and  {2}", INICIO.Year, INICIO.Month, FINAL.Month);
            }

            if (!string.IsNullOrEmpty(oParam.cuenta))
            {
                sConsultaSql += string.Format(@" and(                                        
                                        MA.MAINACCOUNTID = '{0}'
                                        ) ", oParam.cuenta);

            }

            if (!string.IsNullOrEmpty(oParam.cuenta_inicio) && !string.IsNullOrEmpty(oParam.cuenta_inicio))
            {
                sConsultaSql += string.Format(@" and(                                        
                                        MA.MAINACCOUNTID  BETWEEN '{0}' AND '{1}') ", oParam.cuenta_inicio, oParam.cuenta_fin);

            }
            /*else {
                sConsultaSql += string.Format(@"and(                                        
                                        MA.MAINACCOUNTID like 'ELA%' 
                                        OR MA.MAINACCOUNTID like 'ECA%'
                                        )");
            }*/

            if (!string.IsNullOrEmpty(oParam.sucursal))
            {
                sConsultaSql += string.Format(" AND  SUC.DISPLAYVALUE = '{0}' ", oParam.sucursal);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.centro))
            {
                sConsultaSql += string.Format(" AND  CC.DISPLAYVALUE = '{0}' ", oParam.centro);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.depto))
            {
                sConsultaSql += string.Format(" AND  DEP.DISPLAYVALUE = '{0}' ", oParam.depto);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParam.area))
            {
                sConsultaSql += string.Format(" AND  AREA.DISPLAYVALUE = '{0}' ", oParam.area);
                bAnd = true;
            }


            if (!string.IsNullOrEmpty(oParam.depto_distinto))
            {
                sConsultaSql += string.Format(" AND  DEP.DISPLAYVALUE <> '{0}' ", oParam.depto_distinto);
                bAnd = true;
            }


            if (!string.IsNullOrEmpty(oParam.documento))
            {
                sConsultaSql += string.Format(" AND  VEH.JOURNALNUM = '{0}' ", oParam.documento);
                bAnd = true;
            }
            sConsultaSql += string.Format(@"                     GROUP BY GJE.SUBLEDGERVOUCHER, GJE.ACCOUNTINGDATE,--VEH.JOURNALNUM,
                                    GJAE.LEDGERACCOUNT
                                    , GJAE.TEXT, MA.MAINACCOUNTID,
                                    ISNULL(SUC.DISPLAYVALUE, 'NINGUNO'),
                                    ISNULL(FIL.DISPLAYVALUE, 'NINGUNO'),
                                    ISNULL(CC.DISPLAYVALUE, 'NINGUNO'), 
                                    ISNULL(DEP.DISPLAYVALUE, 'NINGUNO'),
                                    ISNULL(AREA.DISPLAYVALUE, 'NINGUNO') 
									--ISNULL(VEH.OFFSETTXT, 'NINGUNO')
                                    ");


            odataset = oConnection.FillDataSet(sConsultaSql, iTimeOut: 999999999);
            this.LoopDataSet(oParam,odataset, ref lstcuentas);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
