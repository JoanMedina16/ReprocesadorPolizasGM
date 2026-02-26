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
  public class LOGI_Filialesterceros_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oFilial)
        {
            oFilial.sAX365 = objRow["IdFilial"] == DBNull.Value ? "" : Convert.ToString(objRow["IdFilial"]);
            oFilial.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oFilial.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oFilial.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oFilial.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oFilial.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oFilial.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oFilial.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oFilial.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oFilial.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oFilial.iEmpresa = objRow["IdEmpresa"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdEmpresa"]);
            oFilial.sNombreempresa = objRow["sNombrerazon"] == DBNull.Value ? "" : Convert.ToString(objRow["sNombrerazon"]);
            if (oFilial.dtFechAlta != DateTime.MinValue)
                oFilial.sFechaalta = oFilial.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");

            oFilial.sNombreCodEmpresa = string.Format("{0} - {1}", oFilial.iEmpresa, oFilial.sNombreempresa);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstFileales)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstFileales.Add(objTemp);
            }
        }
        public string EditaFileal(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oFilial, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_filialesterceros ";

            if (!string.IsNullOrEmpty(oFilial.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oFilial.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oFilial.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oFilial.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oFilial.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oFilial.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oFilial.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oFilial.sPlanning);
                bSET = true;
            }

            if (oFilial.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oFilial.iActivo);
                bSET = true;
            }

            if (oFilial.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oFilial.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oFilial.iDUsuario);
            }

            if (oFilial.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oFilial.iCuenta);
                bSET = true;
            }
            if (oFilial.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oFilial.iSubcuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdFilial = '{1}'", oFilial.iDUsuario, oFilial.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoFileal(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oFilial, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_filialesterceros(IdFilial,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,FK_1_IdEmpresa,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@IdFilial, @sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning, @iEmpresa,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");
            oHashParam.Add("@IdFilial", oFilial.sAX365);
            oHashParam.Add("@sDescripcion", oFilial.sDescripcion);
            oHashParam.Add("@sAX2009", oFilial.sAX2009);
            oHashParam.Add("@sAX2012", oFilial.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oFilial.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oFilial.iSubcuenta);
            oHashParam.Add("@sPlanning", oFilial.sPlanning);
            oHashParam.Add("@iEmpresa", oFilial.iEmpresa);
            oHashParam.Add("@iIdUsuarioCreacion", oFilial.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaFilieales(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstFiliales, LOGI_Catalogos_INFO oFilial, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = @"SELECT T1.*, T2.IdEmpresa, T2.sNombrerazon FROM cat_filialesterceros T1 INNER JOIN cnf_empresa T2 ON 
                             T1.FK_1_IdEmpresa = T2.IdEmpresa";

            if (!string.IsNullOrEmpty(oFilial.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.IdFilial LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oFilial.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oFilial.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oFilial.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oFilial.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oFilial.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oFilial.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oFilial.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oFilial.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oFilial.sPlanning);
                bAnd = true;
            }
            if (oFilial.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oFilial.iCuenta);
                bAnd = true;
            }
            if (oFilial.iSubcuenta >= 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oFilial.iSubcuenta);
                bAnd = true;
            }

            if (oFilial.iEmpresa > 0)
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa = {1}", bAnd ? "AND" : "WHERE", oFilial.iEmpresa);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oFilial.sFechareginicio) && !string.IsNullOrEmpty(oFilial.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oFilial.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oFilial.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }

            if (oFilial.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oFilial.iEliminado);
                bAnd = true;
            }
            if (oFilial.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oFilial.iActivo);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oFilial.sEmpresas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa IN ({1})", bAnd ? "AND" : "WHERE", oFilial.sEmpresas);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstFiliales);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}

