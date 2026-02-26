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
   public class LOGI_Clientes_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oCliente)
        {
            oCliente.sAX365 = objRow["IdCliente"] == DBNull.Value ? "" : Convert.ToString(objRow["IdCliente"]);
            oCliente.sAX365_II = objRow["sAX365II"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX365II"]);
            oCliente.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oCliente.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oCliente.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oCliente.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oCliente.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oCliente.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oCliente.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oCliente.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oCliente.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            oCliente.iEmpresa = objRow["IdEmpresa"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["IdEmpresa"]);
            oCliente.sNombreempresa = objRow["sNombrerazon"] == DBNull.Value ? "" : Convert.ToString(objRow["sNombrerazon"]);
            if (oCliente.dtFechAlta != DateTime.MinValue)
                oCliente.sFechaalta = oCliente.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
            oCliente.sNombreCodEmpresa = string.Format("{0} - {1}", oCliente.iEmpresa, oCliente.sNombreempresa);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstClientes)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstClientes.Add(objTemp);
            }
        }
        public string EditaCliente(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oCliente, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_clientes ";

            if (!string.IsNullOrEmpty(oCliente.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oCliente.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oCliente.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oCliente.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oCliente.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oCliente.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oCliente.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oCliente.sPlanning);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oCliente.sAX365_II))
            {
                sConsultaSql += string.Format(" {0} sAX365II = '{1}'", bSET ? "," : "SET", oCliente.sAX365_II);
                bSET = true;
            }

            if (oCliente.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oCliente.iActivo);
                bSET = true;
            }

            if (oCliente.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oCliente.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oCliente.iDUsuario);
            }

            if (oCliente.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oCliente.iCuenta);
                bSET = true;
            }
            if (oCliente.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oCliente.iSubcuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdCliente = '{1}'", oCliente.iDUsuario, oCliente.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoCliente(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oCliente, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_clientes(IdCliente,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,FK_1_IdEmpresa,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja,sAX365II) VALUES(@IdCliente, @sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning, @iEmpresa,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL, @sAX365II)");
            oHashParam.Add("@IdCliente", oCliente.sAX365);
            oHashParam.Add("@sDescripcion", oCliente.sDescripcion);
            oHashParam.Add("@sAX2009", oCliente.sAX2009);
            oHashParam.Add("@sAX2012", oCliente.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oCliente.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oCliente.iSubcuenta);
            oHashParam.Add("@sPlanning", oCliente.sPlanning);
            oHashParam.Add("@iEmpresa", oCliente.iEmpresa);
            oHashParam.Add("@iIdUsuarioCreacion", oCliente.iDUsuario);
            oHashParam.Add("@sAX365II", oCliente.sAX365_II);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaClientes(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstClientes, LOGI_Catalogos_INFO oCliente, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false; sConsultaSql = @"SELECT T1.*, T2.IdEmpresa, T2.sNombrerazon FROM cat_clientes T1 INNER JOIN cnf_empresa T2 ON 
                             T1.FK_1_IdEmpresa = T2.IdEmpresa";

            if (!string.IsNullOrEmpty(oCliente.sAX365))
            {
                sConsultaSql += string.Format(" {0} T1.IdCliente LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCliente.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCliente.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} T1.sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCliente.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCliente.sAX2009))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCliente.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCliente.sAX2012))
            {
                sConsultaSql += string.Format(" {0} T1.sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCliente.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCliente.sPlanning))
            {
                sConsultaSql += string.Format(" {0} T1.sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCliente.sPlanning);
                bAnd = true;
            }
            if (oCliente.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oCliente.iCuenta);
                bAnd = true;
            }
            if (oCliente.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} T1.iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oCliente.iSubcuenta);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCliente.sFechareginicio) && !string.IsNullOrEmpty(oCliente.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} T1.dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oCliente.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oCliente.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }

            if (oCliente.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bBaja = {1}", bAnd ? "AND" : "WHERE", oCliente.iEliminado);
                bAnd = true;
            }
            if (oCliente.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} T1.bActivo = {1}", bAnd ? "AND" : "WHERE", oCliente.iActivo);
                bAnd = true;
            }

            if (oCliente.iEmpresa > 0)
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa = {1}", bAnd ? "AND" : "WHERE", oCliente.iEmpresa);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCliente.sEmpresas))
            {
                sConsultaSql += string.Format(" {0} T1.FK_1_IdEmpresa IN ({1})", bAnd ? "AND" : "WHERE", oCliente.sEmpresas);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstClientes);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

    }
}

