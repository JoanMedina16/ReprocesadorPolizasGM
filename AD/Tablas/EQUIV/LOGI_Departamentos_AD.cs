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
   public class LOGI_Departamentos_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oDepto)
        {
            oDepto.sAX365 = objRow["IdDepartamento"] == DBNull.Value ? "" : Convert.ToString(objRow["IdDepartamento"]);
            oDepto.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oDepto.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oDepto.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oDepto.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oDepto.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oDepto.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oDepto.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oDepto.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oDepto.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oDepto.iArea = objRow["FK_1_area"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["FK_1_area"]);
            string sNombreare = objRow["sNomarea"] == DBNull.Value ? "No asignado" : Convert.ToString(objRow["sNomarea"]);
            oDepto.sNombrearea = string.Format("{0} - {1}", oDepto.iArea, sNombreare);
            if (oDepto.dtFechAlta != DateTime.MinValue)
                oDepto.sFechaalta = oDepto.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstDeptos)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstDeptos.Add(objTemp);
            }
        }
        public string EditaDepartamento(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oDepto, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_departamento ";

            if (!string.IsNullOrEmpty(oDepto.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oDepto.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oDepto.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oDepto.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oDepto.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oDepto.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oDepto.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oDepto.sPlanning);
                bSET = true;
            }
            if (oDepto.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oDepto.iActivo);
                bSET = true;
            }
            if (oDepto.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oDepto.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oDepto.iDUsuario);
            }
            if (oDepto.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oDepto.iCuenta);
                bSET = true;
            }
            if (oDepto.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oDepto.iSubcuenta);
                bSET = true;
            }
            if (oDepto.iArea > 0)
            {
                sConsultaSql += string.Format(" {0} FK_1_area = {1}", bSET ? "," : "SET", oDepto.iArea);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdDepartamento = '{1}'", oDepto.iDUsuario, oDepto.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevaDepartamento(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oDepto, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_departamento(IdDepartamento,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja,FK_1_area) VALUES(@IdDepartamento, @sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL, @FK_1_area)");
            oHashParam.Add("@IdDepartamento", oDepto.sAX365);
            oHashParam.Add("@sDescripcion", oDepto.sDescripcion);
            oHashParam.Add("@sAX2009", oDepto.sAX2009);
            oHashParam.Add("@sAX2012", oDepto.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oDepto.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oDepto.iSubcuenta);
            oHashParam.Add("@sPlanning", oDepto.sPlanning);
            oHashParam.Add("@iIdUsuarioCreacion", oDepto.iDUsuario);
            oHashParam.Add("@FK_1_area", oDepto.iArea);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaDepartamentos(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstDeptos, LOGI_Catalogos_INFO oDepto, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "select T1.*, T2.sDescripcion as sNomarea from cat_departamento T1 LEFT JOIN cat_area  T2 ON T1.FK_1_area = T2.IdArea";

            if (!string.IsNullOrEmpty(oDepto.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.IdDepartamento LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oDepto.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oDepto.sFiltroIN))
            {
                sConsultaSql += string.Format(" {0} T1.IdDepartamento IN ({1}) ", bAnd ? "AND" : "WHERE", oDepto.sFiltroIN);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oDepto.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oDepto.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oDepto.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oDepto.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oDepto.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oDepto.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oDepto.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oDepto.sPlanning);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oDepto.sFechareginicio) && !string.IsNullOrEmpty(oDepto.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oDepto.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oDepto.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }
            if (oDepto.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oDepto.iCuenta);
                bAnd = true;
            }
            if (oDepto.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oDepto.iSubcuenta);
                bAnd = true;
            }

            if (oDepto.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oDepto.iEliminado);
                bAnd = true;
            }
            if (oDepto.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oDepto.iActivo);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstDeptos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
