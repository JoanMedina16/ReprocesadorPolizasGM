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
  public  class LOGI_Centroscosto_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Catalogos_INFO oCentro)
        {
            oCentro.sAX365 = objRow["IdCentrocosto"] == DBNull.Value ? "" : Convert.ToString(objRow["IdCentrocosto"]);
            oCentro.sDescripcion = objRow["sDescripcion"] == DBNull.Value ? "" : Convert.ToString(objRow["sDescripcion"]);
            oCentro.sAX2009 = objRow["sAX2009"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2009"]);
            oCentro.sAX2012 = objRow["sAX2012"] == DBNull.Value ? "" : Convert.ToString(objRow["sAX2012"]);
            oCentro.iCuenta = objRow["iCuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iCuenta_OPE"]);
            oCentro.iSubcuenta = objRow["iSubcuenta_OPE"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["iSubcuenta_OPE"]);
            oCentro.sPlanning = objRow["sPlanning"] == DBNull.Value ? "" : Convert.ToString(objRow["sPlanning"]);
            oCentro.iActivo = objRow["bActivo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bActivo"]);
            oCentro.iEliminado = objRow["bBaja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["bBaja"]);
            oCentro.dtFechAlta = objRow["dtFechaCreacion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["dtFechaCreacion"]);
            if (oCentro.dtFechAlta != DateTime.MinValue)
                oCentro.sFechaalta = oCentro.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Catalogos_INFO> lstCentros)
        {
            LOGI_Catalogos_INFO objTemp = new LOGI_Catalogos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Catalogos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstCentros.Add(objTemp);
            }
        }
        public string EditaCentrocosto(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oCentro, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE cat_centrocosto ";

            if (!string.IsNullOrEmpty(oCentro.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion = '{1}'", bSET ? "," : "SET", oCentro.sDescripcion);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oCentro.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 = '{1}'", bSET ? "," : "SET", oCentro.sAX2009);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oCentro.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 = '{1}'", bSET ? "," : "SET", oCentro.sAX2012);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oCentro.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning = '{1}'", bSET ? "," : "SET", oCentro.sPlanning);
                bSET = true;
            }

            if (oCentro.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bSET ? "," : "SET", oCentro.iActivo);
                bSET = true;
            }

            if (oCentro.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bSET ? "," : "SET", oCentro.iEliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} dtFechaBaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} iIdUsuarioBaja = {1}", bSET ? "," : "SET", oCentro.iDUsuario);
            }

            if (oCentro.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bSET ? "," : "SET", oCentro.iCuenta);
                bSET = true;
            }
            if (oCentro.iSubcuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bSET ? "," : "SET", oCentro.iSubcuenta);
                bSET = true;
            }
            sConsultaSql += string.Format(", dtFechaUltMod = GETDATE(),iIdUsuarioUltMod = {0} WHERE IdCentrocosto = '{1}'", oCentro.iDUsuario, oCentro.sAX365);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoCentrocosto(ref LOGI_ConexionSql_AD oConnection, LOGI_Catalogos_INFO oCentro, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_centrocosto(IdCentrocosto,sDescripcion,sAX2009,sAX2012,
            iCuenta_OPE,iSubcuenta_OPE,sPlanning,dtFechaUltMod,dtFechaBaja,bActivo,bBaja,iIdUsuarioCreacion,iIdUsuarioUltMod,
            iIdUsuarioBaja) VALUES(@IdCentrocosto,@sDescripcion,@sAX2009,@sAX2012,@iCuenta_OPE,@iSubcuenta_OPE,@sPlanning,NULL,NULL,1,0,
            @iIdUsuarioCreacion,NULL,NULL)");
            oHashParam.Add("@IdCentrocosto", oCentro.sAX365);
            oHashParam.Add("@sDescripcion", oCentro.sDescripcion);
            oHashParam.Add("@sAX2009", oCentro.sAX2009);
            oHashParam.Add("@sAX2012", oCentro.sAX2012);
            oHashParam.Add("@iCuenta_OPE", oCentro.iCuenta);
            oHashParam.Add("@sPlanning", oCentro.sPlanning);
            oHashParam.Add("@iSubcuenta_OPE", oCentro.iSubcuenta);
            oHashParam.Add("@iIdUsuarioCreacion", oCentro.iDUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaCentroscosto(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Catalogos_INFO> lstCentros, LOGI_Catalogos_INFO oCentro, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = "SELECT * FROM cat_centrocosto ";
            if (!string.IsNullOrEmpty(oCentro.sAX365))
            {
                sConsultaSql += string.Format(" {0} IdCentrocosto LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCentro.sAX365);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCentro.sFiltroIN))
            {
                sConsultaSql += string.Format(" {0} IdCentrocosto IN ({1}) ", bAnd ? "AND" : "WHERE", oCentro.sFiltroIN);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oCentro.sDescripcion))
            {
                sConsultaSql += string.Format(" {0} sDescripcion LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCentro.sDescripcion);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCentro.sAX2009))
            {
                sConsultaSql += string.Format(" {0} sAX2009 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCentro.sAX2009);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCentro.sAX2012))
            {
                sConsultaSql += string.Format(" {0} sAX2012 LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCentro.sAX2012);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCentro.sPlanning))
            {
                sConsultaSql += string.Format(" {0} sPlanning LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oCentro.sPlanning);
                bAnd = true;
            }
            if (!string.IsNullOrEmpty(oCentro.sFechareginicio) && !string.IsNullOrEmpty(oCentro.sFecharegfin))
            {
                sConsultaSql += string.Format(" {0} dtFechaCreacion BETWEEN '{1}' AND '{2}'", bAnd ? "AND" : "WHERE", Convert.ToDateTime(oCentro.sFechareginicio).ToString("yyyyMMdd"), Convert.ToDateTime(oCentro.sFecharegfin).ToString("yyyyMMdd"));
                bAnd = true;
            }
            if (oCentro.iCuenta > 0)
            {
                sConsultaSql += string.Format(" {0} iCuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oCentro.iCuenta);
                bAnd = true;
            }
            if (oCentro.iSubcuenta >= 0)
            {
                sConsultaSql += string.Format(" {0} iSubcuenta_OPE = {1}", bAnd ? "AND" : "WHERE", oCentro.iSubcuenta);
                bAnd = true;
            }

            if (oCentro.iEliminado > 0)
            {
                sConsultaSql += string.Format(" {0} bBaja = {1}", bAnd ? "AND" : "WHERE", oCentro.iEliminado);
                bAnd = true;
            }
            if (oCentro.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} bActivo = {1}", bAnd ? "AND" : "WHERE", oCentro.iActivo);
                bAnd = true;
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstCentros);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}

