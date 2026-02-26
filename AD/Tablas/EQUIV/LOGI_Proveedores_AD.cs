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
    public class LOGI_Proveedores_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oProveedor)
        {

            oProveedor.sAX365 = objRow["IdProveedor"] == DBNull.Value ? "" : Convert.ToString(objRow["IdProveedor"]);
            oProveedor.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oProveedor.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oProveedor.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oProveedor.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oProveedor.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oProveedor.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oProveedor.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oProveedor.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oProveedor.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oProveedor.iEmpresa = objRow["IdEmpresa"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdEmpresa"]);
            oProveedor.sNombreempresa = objRow["sNombrerazon"] == DBNull.Value ? "" : Convert.ToString(objRow["sNombrerazon"]);
            if (oProveedor.dtFechAlta != DateTime.MinValue)
                oProveedor.sFechaalta = oProveedor.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
            oProveedor.sNombreCodEmpresa = string.Format("{0} - {1}", oProveedor.iEmpresa, oProveedor.sNombreempresa);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstProveedores)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstProveedores.Add(objTemp);
            }
        }
        public string EditaProveedor(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oProveedor, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_proveedores ";

            if (!string.IsNullOrEmpty(oProveedor.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oProveedor.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oProveedor.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oProveedor.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oProveedor.sPlanning);
                bSET = true;
            }

            if (oProveedor.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oProveedor.iActivo);
                bSET = true;
            }

            if (oProveedor.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oProveedor.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oProveedor.iDUsuario);
            }

            if (oProveedor.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oProveedor.iCuenta);
                bSET = true;
            }
            if (oProveedor.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oProveedor.iSubcuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdProveedor = '{1}'", oProveedor.iDUsuario, oProveedor.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoProveedor(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oProveedor, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_proveedores(IdProveedor,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,FK_1_IdEmpresa,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@IdProveedor, @sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning, @iEmpresa,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");
            oHashParam.Add("@IdProveedor", oProveedor.sAX365);
            oHashParam.Add("@sDescripcion", oProveedor.sDescripcion);
            oHashParam.Add("@sAX2009", oProveedor.sAX2009);
            oHashParam.Add("@sAX2012", oProveedor.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oProveedor.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oProveedor.iSubcuenta);
            oHashParam.Add("@sPlanning", oProveedor.sPlanning);
            oHashParam.Add("@iEmpresa", oProveedor.iEmpresa);
            oHashParam.Add("@iIdUsuarioCreacion", oProveedor.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaProveedores(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstProveedor, LOGI_Catalogos_INFO oProveedor, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = @"SELECT T1.*, T2.IdEmpresa, T2.sNombrerazon FROM cat_proveedores T1 INNER JOIN cnf_empresa T2 ON 
                             T1.FK_1_IdEmpresa = T2.IdEmpresa";

            if (!string.IsNullOrEmpty(oProveedor.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.IdProveedor = '{1}'", bAnd ? "AND" : "WHERE", oProveedor.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oProveedor.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oProveedor.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 = '{1}'", bAnd ? "AND" : "WHERE", oProveedor.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 = '{1}'", bAnd ? "AND" : "WHERE", oProveedor.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning = '{1}'", bAnd ? "AND" : "WHERE", oProveedor.sPlanning);
                bAnd = true;
            }
            if (oProveedor.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oProveedor.iCuenta);
                bAnd = true;
            }
            if (oProveedor.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oProveedor.iSubcuenta);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sFechareginicio) && !string.IsNullOrEmpty(oProveedor.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oProveedor.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oProveedor.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }

            if (oProveedor.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oProveedor.iEliminado);
                bAnd = true;
            }
            if (oProveedor.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oProveedor.iActivo);
                bAnd = true;
            }

            if (oProveedor.iEmpresa > 0)
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa = {1}", bAnd ? "AND" : "WHERE", oProveedor.iEmpresa);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oProveedor.sEmpresas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa IN ({1})", bAnd ? "AND" : "WHERE", oProveedor.sEmpresas);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstProveedor);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
