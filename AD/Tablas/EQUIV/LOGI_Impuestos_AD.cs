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
   public class LOGI_Impuestos_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oImpuesto)
        {
            oImpuesto.sAX365 = objRow["IdImpuesto"] == DBNull.Value ? "" : Convert.ToString(objRow["IdImpuesto"]);
            oImpuesto.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oImpuesto.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oImpuesto.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oImpuesto.sAxgrupoventa = objRow["sAXVenta"] == DBNull.Value ? "" : Convert.ToString(objRow["sAXVenta"]);
            oImpuesto.sAxgrupoarticulo = objRow["sAXArticulo"] == DBNull.Value ? "" : Convert.ToString(objRow["sAXArticulo"]);
            oImpuesto.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oImpuesto.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oImpuesto.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            //oImpuesto.sGrupoimpuesto = objRow["FK_1_IdGrupo"] == DBNull.Value ? "" : Convert.ToString(objRow["FK_1_IdGrupo"]);
            oImpuesto.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oImpuesto.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oImpuesto.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            if (oImpuesto.dtFechAlta != DateTime.MinValue)
                oImpuesto.sFechaalta = oImpuesto.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstImpuestos)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstImpuestos.Add(objTemp);
            }
        }


        public string EditaImpuesto(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oImpuesto, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_impuesto_logi ";
            if (!string.IsNullOrEmpty(oImpuesto.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oImpuesto.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oImpuesto.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oImpuesto.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oImpuesto.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oImpuesto.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oImpuesto.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oImpuesto.sPlanning);
                bSET = true;
            }

            if (oImpuesto.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oImpuesto.iActivo);
                bSET = true;
            }

            if (oImpuesto.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oImpuesto.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oImpuesto.iDUsuario);
            }

            if (oImpuesto.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oImpuesto.iCuenta);
                bSET = true;
            }
            if (oImpuesto.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oImpuesto.iSubcuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdImpuesto = '{1}'", oImpuesto.iDUsuario, oImpuesto.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoImpuesto(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oImpuesto, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_impuesto_logi(IdImpuesto,sDescripcion,sAX2009,sAX2012,sAXVenta,sAXArticulo,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@IdImpuesto, @sDescripcion,@sAX2009,@sAX2012,@sAXVenta,@sAXArticulo,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");
            oHashParam.Add("@IdImpuesto", oImpuesto.sAX365);
            oHashParam.Add("@sDescripcion", oImpuesto.sDescripcion);
            oHashParam.Add("@sAX2009", oImpuesto.sAX2009);
            oHashParam.Add("@sAX2012", oImpuesto.sAX2012);
            oHashParam.Add("@sAXVenta", oImpuesto.sAxgrupoventa);
            oHashParam.Add("@sAXArticulo", oImpuesto.sAxgrupoarticulo);
            oHashParam.Add("@iCuenta_OPE", oImpuesto.iCuenta);
            oHashParam.Add("@iSubcuenta_OPE", oImpuesto.iSubcuenta);
            oHashParam.Add("@sPlanning", oImpuesto.sPlanning);
            oHashParam.Add("@iIdUsuarioCreacion", oImpuesto.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaImpuestos(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstImpuestos, LOGI_Catalogos_INFO oImpuesto, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT * FROM cat_impuesto_logi ";

            if (!string.IsNullOrEmpty(oImpuesto.sAX365))
            {
                sConsultaSql += string.Format(" {0} IdImpuesto LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oImpuesto.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oImpuesto.sAX365MATCH))
            {
                sConsultaSql += string.Format(" {0} IdImpuesto = '{1}'", bAnd ? "AND" : "WHERE", oImpuesto.sAX365MATCH);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oImpuesto.sAxgrupoarticulo))
            {
                sConsultaSql += string.Format(" {0} sAXArticulo = '{1}'", bAnd ? "AND" : "WHERE", oImpuesto.sAxgrupoarticulo);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oImpuesto.sAxgrupoventa))
            {
                sConsultaSql += string.Format(" {0} sAXVenta = '{1}'", bAnd ? "AND" : "WHERE", oImpuesto.sAxgrupoventa);
                bAnd = true;
            }


            if (!string.IsNullOrEmpty(oImpuesto.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oImpuesto.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oImpuesto.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oImpuesto.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oImpuesto.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oImpuesto.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oImpuesto.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oImpuesto.sPlanning);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oImpuesto.sFechareginicio) && !string.IsNullOrEmpty(oImpuesto.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oImpuesto.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oImpuesto.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }
            if (oImpuesto.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oImpuesto.iCuenta);
                bAnd = true;
            }
            if (oImpuesto.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oImpuesto.iSubcuenta);
                bAnd = true;
            }

            if (oImpuesto.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bAnd ? "AND" : "WHERE", oImpuesto.iEliminado);
                bAnd = true;
            }
            if (oImpuesto.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bAnd ? "AND" : "WHERE", oImpuesto.iActivo);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstImpuestos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

    }
}
