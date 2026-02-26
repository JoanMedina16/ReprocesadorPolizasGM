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
   public class LOGI_Sucursales_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oSucursal)
        {
            oSucursal.sAX365 = objRow["IdSucursal"] == DBNull.Value ? "" : Convert.ToString(objRow["IdSucursal"]);
            oSucursal.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oSucursal.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oSucursal.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oSucursal.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oSucursal.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oSucursal.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oSucursal.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oSucursal.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oSucursal.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oSucursal.iEmpresa = objRow["IdEmpresa"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdEmpresa"]);
            oSucursal.sNombreempresa = objRow["sNombrerazon"] == DBNull.Value ? "" : Convert.ToString(objRow["sNombrerazon"]);
            if (oSucursal.dtFechAlta != DateTime.MinValue)
                oSucursal.sFechaalta = oSucursal.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
            oSucursal.sNombreCodEmpresa = string.Format("{0} - {1}", oSucursal.iEmpresa, oSucursal.sNombreempresa);
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
        public string EditaSucursal(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oSucursal, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_sucursal ";

            if (!string.IsNullOrEmpty(oSucursal.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oSucursal.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oSucursal.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oSucursal.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oSucursal.sPlanning);
                bSET = true;
            }

            if (oSucursal.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oSucursal.iActivo);
                bSET = true;
            }

            if (oSucursal.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oSucursal.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oSucursal.iDUsuario);
            }

            if (oSucursal.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oSucursal.iCuenta);
                bSET = true;
            }
            if (oSucursal.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oSucursal.iSubcuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdSucursal = '{1}'", oSucursal.iDUsuario, oSucursal.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoSucursal(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oSucursal, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_sucursal(IdSucursal,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,FK_1_IdEmpresa,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@IdSucursal, @sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning, @iEmpresa,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");

            oHashParam.Add("@IdSucursal", oSucursal.sAX365);
            oHashParam.Add("@sDescripcion", oSucursal.sDescripcion);
            oHashParam.Add("@sAX2009", oSucursal.sAX2009);
            oHashParam.Add("@sAX2012", oSucursal.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oSucursal.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oSucursal.iSubcuenta);
            oHashParam.Add("@sPlanning", oSucursal.sPlanning);
            oHashParam.Add("@iEmpresa", oSucursal.iEmpresa);
            oHashParam.Add("@iIdUsuarioCreacion", oSucursal.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaSucursales(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstSucursales, LOGI_Catalogos_INFO oSucursal, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = @"SELECT T1.*, T2.IdEmpresa, T2.sNombrerazon FROM cat_sucursal T1 INNER JOIN cnf_empresa T2 ON 
                             T1.FK_1_IdEmpresa = T2.IdEmpresa";

            if (!string.IsNullOrEmpty(oSucursal.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.IdSucursal = '{1}'", bAnd ? "AND" : "WHERE", oSucursal.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oSucursal.sFiltroIN))
            {
                sConsultaSql += string.Format(" {0} T1.IdSucursal IN ({1}) ", bAnd ? "AND" : "WHERE", oSucursal.sFiltroIN);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oSucursal.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oSucursal.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 = '{1}'", bAnd ? "AND" : "WHERE", oSucursal.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 = '{1}'", bAnd ? "AND" : "WHERE", oSucursal.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning = '{1}'", bAnd ? "AND" : "WHERE", oSucursal.sPlanning);
                bAnd = true;
            }
            if (oSucursal.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oSucursal.iCuenta);
                bAnd = true;
            }
            if (oSucursal.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oSucursal.iSubcuenta);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sFechareginicio) && !string.IsNullOrEmpty(oSucursal.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oSucursal.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oSucursal.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }

            if (oSucursal.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oSucursal.iEliminado);
                bAnd = true;
            }
            if (oSucursal.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oSucursal.iActivo);
                bAnd = true;
            }
            if (oSucursal.iEmpresa >= 0)
            {
                sConsultaSql += string.Format(" {0} FK_1_IdEmpresa = {1}", bAnd ? "AND" : "WHERE", oSucursal.iEmpresa);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oSucursal.sEmpresas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa IN ({1})", bAnd ? "AND" : "WHERE", oSucursal.sEmpresas);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstSucursales);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
