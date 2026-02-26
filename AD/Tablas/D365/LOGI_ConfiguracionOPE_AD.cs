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
    public class LOGI_ConfiguracionOPE_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_ConfiguracionOPE_INFO otemp)
        {
            otemp.cia = orow["cia"] == DBNull.Value ? -1 : Convert.ToInt32(orow["cia"]);
            otemp.anocont = orow["anocont"] == DBNull.Value ? -1 : Convert.ToInt32(orow["anocont"]);
            otemp.mescont = orow["mescont"] == DBNull.Value ? -1 : Convert.ToInt32(orow["mescont"]); 
        } 

        public string ListaConfiguracion(ref LOGI_ConexionSql_AD oConnection, ref LOGI_ConfiguracionOPE_INFO oConfiguracion, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"SELECT anocont,mescont,cia FROM oa_config");
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.GetObjeto(odataset.Tables[0].Rows[0], ref oConfiguracion);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        } 
        public string ActualizaConfiguracion(ref LOGI_ConexionSql_AD oConnection, LOGI_ConfiguracionOPE_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE oa_config");

            if (oParam.anocont >0)
            {
                sConsultaSql += string.Format(" {0} anocont = {1}", bSET ? "," : "SET", oParam.anocont);
                bSET = true;
            }

            if (oParam.mescont > 0)
            {
                sConsultaSql += string.Format(" {0} mescont = {1}", bSET ? "," : "SET", oParam.mescont);
                bSET = true;
            }

            sConsultaSql += string.Format(" WHERE cia = {0}", oParam.cia);
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        } 
    }
}
