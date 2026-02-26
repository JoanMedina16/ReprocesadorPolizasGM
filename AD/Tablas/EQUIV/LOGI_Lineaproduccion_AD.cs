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
   public class LOGI_Lineaproduccion_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oLinea)
        {

            oLinea.sAX365 = objRow["IdLinea"] == DBNull.Value ? "" : Convert.ToString(objRow["IdLinea"]);
            oLinea.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oLinea.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oLinea.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oLinea.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oLinea.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oLinea.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oLinea.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oLinea.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oLinea.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oLinea.iEmpresa = objRow["IdEmpresa"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdEmpresa"]);
            oLinea.sNombreempresa = objRow["sNombrerazon"] == DBNull.Value ? "" : Convert.ToString(objRow["sNombrerazon"]);
            if (oLinea.dtFechAlta != DateTime.MinValue)
                oLinea.sFechaalta = oLinea.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
            oLinea.sNombreCodEmpresa = string.Format("{0} - {1}", oLinea.iEmpresa, oLinea.sNombreempresa);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstLineas)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstLineas.Add(objTemp);
            }
        }

        public string EditaLinea(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oLinea, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_lineaproduccion ";

            if (!string.IsNullOrEmpty(oLinea.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oLinea.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oLinea.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oLinea.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oLinea.sPlanning);
                bSET = true;
            }

            if (oLinea.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oLinea.iActivo);
                bSET = true;
            }

            if (oLinea.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oLinea.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oLinea.iDUsuario);
            }

            if (oLinea.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oLinea.iCuenta);
                bSET = true;
            }
            if (oLinea.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oLinea.iSubcuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdLinea = '{1}'", oLinea.iDUsuario, oLinea.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevaLinea(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oLinea, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_lineaproduccion(IdLinea,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,FK_1_IdEmpresa,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@IdLinea, @sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning, @iEmpresa,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");

            oHashParam.Add("@IdLinea", oLinea.sAX365);
            oHashParam.Add("@sDescripcion", oLinea.sDescripcion);
            oHashParam.Add("@sAX2009", oLinea.sAX2009);
            oHashParam.Add("@sAX2012", oLinea.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oLinea.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oLinea.iSubcuenta);
            oHashParam.Add("@sPlanning", oLinea.sPlanning);
            oHashParam.Add("@iEmpresa", oLinea.iEmpresa);
            oHashParam.Add("@iIdUsuarioCreacion", oLinea.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaLienas(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstLineas, LOGI_Catalogos_INFO oLinea, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = @"SELECT T1.*, T2.IdEmpresa, T2.sNombrerazon FROM cat_lineaproduccion T1 INNER JOIN cnf_empresa T2 ON 
                             T1.FK_1_IdEmpresa = T2.IdEmpresa";

            if (!string.IsNullOrEmpty(oLinea.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.IdLinea LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oLinea.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oLinea.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oLinea.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oLinea.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oLinea.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oLinea.sPlanning);
                bAnd = true;
            }
            if (oLinea.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oLinea.iCuenta);
                bAnd = true;
            }
            if (oLinea.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oLinea.iSubcuenta);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sFechareginicio) && !string.IsNullOrEmpty(oLinea.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oLinea.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oLinea.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }

            if (oLinea.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oLinea.iEliminado);
                bAnd = true;
            }
            if (oLinea.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oLinea.iActivo);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oLinea.sEmpresas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa IN ({1})", bAnd ? "AND" : "WHERE", oLinea.sEmpresas);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstLineas);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}

