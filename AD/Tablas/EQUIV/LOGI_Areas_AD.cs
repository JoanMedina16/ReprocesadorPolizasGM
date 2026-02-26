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
   public class LOGI_Areas_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oArea)
        {
            oArea.iSubcuenta = objRow["IdArea"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdArea"]);
            oArea.sAX365 = objRow["sAX365"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX365"]);
            oArea.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oArea.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oArea.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oArea.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oArea.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oArea.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oArea.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oArea.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);

            if (oArea.dtFechAlta != DateTime.MinValue)
                oArea.sFechaalta = oArea.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstAreas)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstAreas.Add(objTemp);
            }
        }
        public string EditaArea(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oArea, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_area ";
            if (!string.IsNullOrEmpty(oArea.sAX365))
            {
                sConsultaSql += string.Format(" {0} sAX365 = '{1}'", bSET ? "," : "SET", oArea.sAX365);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oArea.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oArea.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oArea.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oArea.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oArea.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oArea.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oArea.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oArea.sPlanning);
                bSET = true;
            }

            if (oArea.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oArea.iActivo);
                bSET = true;
            }

            if (oArea.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oArea.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oArea.iDUsuario);
            }

            if (oArea.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oArea.iCuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdArea = {1}", oArea.iDUsuario, oArea.iSubcuenta);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevaArea(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oArea, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_area(IdArea,sAX365,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,sPlanning,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@IdArea,@sAX365, @sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@sPlanning,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");
            oHashParam.Add("@IdArea", oArea.iSubcuenta);
            oHashParam.Add("@sAX365", oArea.sAX365);
            oHashParam.Add("@sDescripcion", oArea.sDescripcion);
            oHashParam.Add("@sAX2009", oArea.sAX2009);
            oHashParam.Add("@sAX2012", oArea.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oArea.iCuenta);
            oHashParam.Add("@sPlanning", oArea.sPlanning);
            oHashParam.Add("@iIdUsuarioCreacion", oArea.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaAreas(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstAreas, LOGI_Catalogos_INFO oArea, out string sConsultaSql)
        {
            DataSet odataSet = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT * FROM cat_area ";

            if (!string.IsNullOrEmpty(oArea.sAX365))
            {
                sConsultaSql += string.Format(" {0} sAX365 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oArea.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oArea.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oArea.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oArea.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oArea.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oArea.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oArea.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oArea.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oArea.sPlanning);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oArea.sFechareginicio) && !string.IsNullOrEmpty(oArea.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oArea.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oArea.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }
            if (oArea.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oArea.iCuenta);
                bAnd = true;
            }
            if (oArea.iSubcuenta >= 0)
            {
                sConsultaSql += string.Format(" {0} IdArea = {1}", bAnd ? "AND" : "WHERE", oArea.iSubcuenta);
                bAnd = true;
            }

            if (oArea.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bAnd ? "AND" : "WHERE", oArea.iEliminado);
                bAnd = true;
            }
            if (oArea.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bAnd ? "AND" : "WHERE", oArea.iActivo);
                bAnd = true;
            }
            odataSet = oConnection.FillDataSet(sConsultaSql);
            LoopDataSet(odataSet, ref lstAreas);
            return odataSet.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}

