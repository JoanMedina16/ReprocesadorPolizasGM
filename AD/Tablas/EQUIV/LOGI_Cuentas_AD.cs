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
  public  class LOGI_Cuentas_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oCuenta)
        {
            oCuenta.sAX365 = objRow["sAX365"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX365"]);
            oCuenta.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oCuenta.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oCuenta.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oCuenta.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oCuenta.iCuentamayor = objRow["iMayor_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iMayor_OPE"]);
            oCuenta.iArea = objRow["FK_1_IdArea"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["FK_1_IdArea"]);
            oCuenta.sNombrearea = objRow["sNomarea"] == DBNull.Value ? "" : Convert.ToString(objRow["sNomarea"]);
            oCuenta.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oCuenta.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oCuenta.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oCuenta.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oCuenta.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oCuenta.iEmpresa = objRow["IdEmpresa"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdEmpresa"]);
            oCuenta.sNombreempresa = objRow["sNombrerazon"] == DBNull.Value ? "" : Convert.ToString(objRow["sNombrerazon"]);
            oCuenta.identificador = objRow["cat_id"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["cat_id"]);
            if (oCuenta.dtFechAlta != DateTime.MinValue)
                oCuenta.sFechaalta = oCuenta.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
            oCuenta.sCodArea = string.Format("{0} - {1}", oCuenta.iArea, oCuenta.sNombrearea);
            oCuenta.sNombreCodEmpresa = string.Format("{0} - {1}", oCuenta.iEmpresa, oCuenta.sNombreempresa);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstCuentas)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstCuentas.Add(objTemp);
            }
        }
        public string EditaCuenta(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oCuenta, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_cuentas ";

            if (!string.IsNullOrEmpty(oCuenta.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oCuenta.sDescripcion);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sAX365))
            {
                sConsultaSql += string.Format(" {0} sAX365 = '{1}'", bSET ? "," : "SET", oCuenta.sAX365);
                bSET = true;
            }



            if (!string.IsNullOrEmpty(oCuenta.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oCuenta.sAX2012);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oCuenta.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oCuenta.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oCuenta.sPlanning);
                bSET = true;
            }

            if (oCuenta.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oCuenta.iActivo);
                bSET = true;
            }

            if (oCuenta.iArea >= 0)
            {
                sConsultaSql += string.Format(" {0} FK_1_IdArea = {1}", bSET ? "," : "SET", oCuenta.iArea);
                bSET = true;
            }

            if (oCuenta.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oCuenta.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oCuenta.iDUsuario);
            }

            if (oCuenta.iCuentamayor >= 0)
            {
                sConsultaSql += string.Format(" {0} iMayor_OPE = {1}", bSET ? "," : "SET", oCuenta.iCuentamayor);
                bSET = true;
            }

            if (oCuenta.iCuenta >= 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oCuenta.iCuenta);
                bSET = true;
            }

            if (oCuenta.iSubcuenta >= 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oCuenta.iSubcuenta);
                bSET = true;
            }

            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE cat_id = {1}", oCuenta.iDUsuario, oCuenta.identificador);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand == 1 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevaCuenta(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oCuenta, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_cuentas(sAX365,sDescripcion,sAX2009,sAX2012,iMayor_OPE,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,FK_1_IdArea,FK_2_IdEmpresa,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@sAX365, @sDescripcion,@sAX2009,@sAX2012,@iMayor_OPE,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning,@iArea, @iEmpresa,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");
            oHashParam.Add("@sAX365", oCuenta.sAX365);
            oHashParam.Add("@sDescripcion", oCuenta.sDescripcion);
            oHashParam.Add("@sAX2009", oCuenta.sAX2009);
            oHashParam.Add("@sAX2012", oCuenta.sAX2012);
            oHashParam.Add("@iMayor_OPE", oCuenta.iCuentamayor);
            oHashParam.Add("@iCuenta_OPE", oCuenta.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oCuenta.iSubcuenta);
            oHashParam.Add("@sPlanning", oCuenta.sPlanning);
            oHashParam.Add("@iArea", oCuenta.iArea);
            oHashParam.Add("@iEmpresa", oCuenta.iEmpresa);
            oHashParam.Add("@iIdUsuarioCreacion", oCuenta.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaCuentas(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstCuentas, LOGI_Catalogos_INFO oCuenta, out string sConsultaSql)
        {

            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = @"SELECT T1.*, T2.IdEmpresa, T2.sNombrerazon, T3.sDescripcion as sNomarea FROM cat_cuentas T1 INNER JOIN cnf_empresa T2 ON 
                             T1.FK_2_IdEmpresa = T2.IdEmpresa INNER JOIN cat_area T3 ON T3.IdArea = T1.FK_1_IdArea";

            if (!string.IsNullOrEmpty(oCuenta.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.sAX365 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCuenta.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sFiltroIN))
            {
                sConsultaSql += string.Format(" {0} T1.sAX365 IN ({1}) ", bAnd ? "AND" : "WHERE", oCuenta.sFiltroIN);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sAX365MATCH))
            {
                sConsultaSql += string.Format(" {0} T1.sAX365 = '{1}'", bAnd ? "AND" : "WHERE", oCuenta.sAX365MATCH);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCuenta.sDescripcion);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCuenta.sAX2009);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sAX2009MATCH))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 = '{1}'", bAnd ? "AND" : "WHERE", oCuenta.sAX2009MATCH);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCuenta.sAX2012);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sAX2012MATCH))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 = '{1}'", bAnd ? "AND" : "WHERE", oCuenta.sAX2012MATCH);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCuenta.sPlanning);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sPlanningMATCH))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning = '{1}'", bAnd ? "AND" : "WHERE", oCuenta.sPlanningMATCH);
                bAnd = true;
            }

            if (oCuenta.iCuentamayor >= 0)
            {
                sConsultaSql += string.Format(" {0} T1.iMayor_OPE = {1}", bAnd ? "AND" : "WHERE", oCuenta.iCuentamayor);
                bAnd = true;
            }

            if (oCuenta.iArea >= 0)
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdArea = {1}", bAnd ? "AND" : "WHERE", oCuenta.iArea);
                bAnd = true;
            }

            if (oCuenta.iCuenta >= 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oCuenta.iCuenta);
                bAnd = true;
            }
            if (oCuenta.iSubcuenta >= 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oCuenta.iSubcuenta);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCuenta.sFechareginicio) && !string.IsNullOrEmpty(oCuenta.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oCuenta.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oCuenta.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }

            if (oCuenta.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oCuenta.iEliminado);
                bAnd = true;
            }
            if (oCuenta.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oCuenta.iActivo);
                bAnd = true;
            }

            if (oCuenta.iEmpresa >= 0)
            {
                sConsultaSql += string.Format(" {0} FK_2_IdEmpresa = {1}", bAnd ? "AND" : "WHERE", oCuenta.iEmpresa);
                bAnd = true;
            }

            if (oCuenta.identificadorMATCH > 0)
            {
                sConsultaSql += string.Format(" {0} cat_id <> {1}", bAnd ? "AND" : "WHERE", oCuenta.identificadorMATCH);
                bAnd = true;
            }

            if (oCuenta.identificador > 0)
            {
                sConsultaSql += string.Format(" {0} cat_id =  {1}", bAnd ? "AND" : "WHERE", oCuenta.identificador);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCuenta.sEmpresas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_2_IdEmpresa IN ({1})", bAnd ? "AND" : "WHERE", oCuenta.sEmpresas);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCuenta.sAreas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdArea IN ({1})", bAnd ? "AND" : "WHERE", oCuenta.sAreas);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstCuentas);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}

