using INFO.Tablas.CAT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.CAT
{
   public class LOGI_Sucursales_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_Sucursales_INFO oParam)
        {            
            oParam.suc = orow["suc"] == DBNull.Value ? -1 : Convert.ToInt32(orow["suc"]);
            oParam.nomcia = orow["nomcia"] == DBNull.Value ? "" : Convert.ToString(orow["nomcia"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Sucursales_INFO> lstSucursales)
        {
            LOGI_Sucursales_INFO objTemp = new LOGI_Sucursales_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Sucursales_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstSucursales.Add(objTemp);
            }
        } 
        public string ListaSucursales(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Sucursales_INFO> lstSucursales, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Format(@"SELECT cia,suc,nomcia FROM cias where cia = 67");
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstSucursales);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
