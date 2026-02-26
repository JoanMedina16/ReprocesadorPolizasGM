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
    public class LOGI_Modulos_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow objRow, ref LOGI_Modulos_INFO oModulo)
        {
            oModulo.clave = objRow["clave"] == DBNull.Value ? "" : Convert.ToString(objRow["clave"]).Trim();
            oModulo.nombre = objRow["nombre"] == DBNull.Value ? "" : Convert.ToString(objRow["nombre"]);
            oModulo.pagina = objRow["pagina"] == DBNull.Value ? "" : Convert.ToString(objRow["pagina"]);
            oModulo.icono = objRow["icono"] == DBNull.Value ? "" : Convert.ToString(objRow["icono"]);
            oModulo.nivel = objRow["nivel"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["nivel"]);
            oModulo.padre = objRow["padre"] == DBNull.Value ? "" : Convert.ToString(objRow["padre"]).Trim();
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Modulos_INFO> lstAreas)
        {
            LOGI_Modulos_INFO objTemp = new LOGI_Modulos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Modulos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstAreas.Add(objTemp);
            }
        } 

        public string Listamodulos(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Modulos_INFO> lstModulos, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT * FROM lm_cat_modulos ";
            sConsultaSql += string.Format(" {0} activo = 1", bAnd ? "AND" : "WHERE");
            sConsultaSql += " order by nivel, nombre  asc ";
            odataset = oConnection.FillDataSet(sConsultaSql);
            LoopDataSet(odataset, ref lstModulos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string Listapermisosusuario(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Modulos_INFO> lstModulos, int UsuarioID, out string sConsultaSql, string sClavemodulo = "")
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "select T2.* from lm_usuariosacceso_d365 T1 INNER JOIN lm_cat_modulos T2 ON T1.clavemod = T2.clave";
            if (UsuarioID >0)
            {
                sConsultaSql += string.Format(" {0} T1.usuarioid = {1}", bAnd ? "AND" : "WHERE", UsuarioID);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(sClavemodulo))
            {
                sConsultaSql += string.Format(" {0} T2.clave = '{1}'", bAnd ? "AND" : "WHERE", sClavemodulo);
                bAnd = true;
            }
            sConsultaSql += string.Format(" {0} T2.activo = 1", bAnd ? "AND" : "WHERE");
            odataset = oConnection.FillDataSet(sConsultaSql);
            LoopDataSet(odataset, ref lstModulos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
