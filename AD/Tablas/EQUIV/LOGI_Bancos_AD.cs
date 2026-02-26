using INFO.Tablas.EQUIV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.EQUIV
{
    public class LOGI_Bancos_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oBanco)
        {

            oBanco.sAX365 = objRow["idBanco"] == DBNull.Value ? "" : Convert.ToString(objRow["idBanco"]).Trim();
            oBanco.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oBanco.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oBanco.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oBanco.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oBanco.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oBanco.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oBanco.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oBanco.iEmpresa = objRow["IdEmpresa"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdEmpresa"]);
            oBanco.sNombreempresa = objRow["sNombrerazon"] == DBNull.Value ? "" : Convert.ToString(objRow["sNombrerazon"]);
            if (oBanco.dtFechAlta != DateTime.MinValue)
                oBanco.sFechaalta = oBanco.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
            oBanco.sNombreCodEmpresa = string.Format("{0} - {1}", oBanco.iEmpresa, oBanco.sNombreempresa);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstArticulos)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstArticulos.Add(objTemp);
            }
        }
        public string RecuperaBancos(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstBancos, LOGI_Catalogos_INFO oBanco, out string sConsultaSql, bool bTop)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            string sTop = bTop ? " TOP 150 " : "";
            sConsultaSql = string.Format(@"SELECT {0} T1.*, T2.IdEmpresa, T2.sNombrerazon FROM cat_bancos T1 INNER JOIN cnf_empresa T2 ON 
                             T1.FK_1_IdEmpresa = T2.IdEmpresa", sTop);

            if (!string.IsNullOrEmpty(oBanco.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.IdBanco LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oBanco.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oBanco.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oBanco.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oBanco.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oBanco.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oBanco.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oBanco.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oBanco.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oBanco.sPlanning);
                bAnd = true;
            }
            if (oBanco.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oBanco.iCuenta);
                bAnd = true;
            }
            if (oBanco.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oBanco.iSubcuenta);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oBanco.sFechareginicio) && !string.IsNullOrEmpty(oBanco.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oBanco.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oBanco.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }

            if (oBanco.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oBanco.iEliminado);
                bAnd = true;
            }
            if (oBanco.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oBanco.iActivo);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oBanco.sEmpresas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa IN ({1})", bAnd ? "AND" : "WHERE", oBanco.sEmpresas);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstBancos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
