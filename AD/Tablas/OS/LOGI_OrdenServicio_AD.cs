using INFO.Tablas.OS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.OS
{
    public class LOGI_OrdenServicio_AD
    {
        void GetObjeto(DataRow objRow, ref LOGI_OrdenServicio_INFO Orden)
        { 
            Orden.suc = objRow["suc"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["suc"]);
            Orden.orden = objRow["orden"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["orden"]);
            Orden.cuentavehiculo = objRow["cuenta"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["cuenta"]);
            Orden.sctavehiculo = objRow["clave"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["clave"]);
            Orden.cancelado = objRow["cancelado"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["cancelado"]);
            Orden.FechaSalida = objRow["fechasal"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["fechasal"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_OrdenServicio_INFO> lstCentros)
        {
            LOGI_OrdenServicio_INFO objTemp = new LOGI_OrdenServicio_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_OrdenServicio_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstCentros.Add(objTemp);
            }
        } 

        public string Listaordenes(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_OrdenServicio_INFO> lstCentros, LOGI_OrdenServicio_INFO Orden, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "select suc, orden,cuenta,clave,cancelado,fechasal from ordenes ";
            
            if (Orden.orden >0)
            {
                sConsultaSql += string.Format(" {0} orden = {1}", bAnd ? "AND" : "WHERE", Orden.orden);
                bAnd = true;
            }


            if (Orden.suc > 0)
            {
                sConsultaSql += string.Format(" {0} suc = {1}", bAnd ? "AND" : "WHERE", Orden.suc);
                bAnd = true;
            }

            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstCentros);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
