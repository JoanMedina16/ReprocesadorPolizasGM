using INFO.Tablas.FUEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.FUEL
{
    public class LOGI_Proveedores_AD
    {

        void GetObjeto(DataRow objRow, ref LOGI_Proveedores_INFO oProve)
        {
            oProve.cuenta = objRow["cuenta"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["cuenta"]);
            oProve.scuenta = objRow["scuenta"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["scuenta"]);
            oProve.diacredito = objRow["credmat"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["credmat"]);
            oProve.nombre = objRow["nomsuj"] == DBNull.Value ? "" : Convert.ToString(objRow["nomsuj"]);
            oProve.RFC = objRow["rfc"] == DBNull.Value ? "" : Convert.ToString(objRow["rfc"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Proveedores_INFO> lstProveedores)
        {
            LOGI_Proveedores_INFO objTemp = new LOGI_Proveedores_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Proveedores_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstProveedores.Add(objTemp);
            }
        } 
        public string Listaproveedores(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Proveedores_INFO> lstProveedores, LOGI_Proveedores_INFO oProve, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT * FROM oa_sujetos ";
            if (!string.IsNullOrEmpty(oProve.RFC))
            {
                sConsultaSql += string.Format(" {0} rfc LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oProve.RFC);
                bAnd = true;
            } 

            if (!string.IsNullOrEmpty(oProve.nombre))
            {
                sConsultaSql += string.Format(" {0} nomsuj LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oProve.nombre);
                bAnd = true;
            } 
            if (oProve.cuenta> 0)
            {
                sConsultaSql += string.Format(" {0} cuenta = {1}", bAnd ? "AND" : "WHERE", oProve.cuenta);
                bAnd = true;
            }
            if (oProve.scuenta > 0)
            {
                sConsultaSql += string.Format(" {0} scuenta = {1}", bAnd ? "AND" : "WHERE", oProve.scuenta);
                bAnd = true;
            } 
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstProveedores);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string ListaProveedorDias(ref LOGI_ConexionSql_AD oConnection, ref  string diasPAGO, string sCodigo, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"select PAYMTERMID from Warehouse.GRWVENDTABLEMASTER where ACCOUNTNUM = '{0}'", sCodigo); 
            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow row = odataset.Tables[0].Rows[0];
                diasPAGO = row["PAYMTERMID"] == DBNull.Value ? "" : Convert.ToString(row["PAYMTERMID"]);
            }
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
