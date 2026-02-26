using INFO.Tablas.CAT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.CAT
{
    public class LOGI_Roles_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow objRow, ref LOGI_Roles_INFO oRol)
        {
            oRol.rol = objRow["rol"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["rol"]);
            oRol.nombre = objRow["nombre"] == DBNull.Value ? "" : Convert.ToString(objRow["nombre"]);
            oRol.activo = objRow["activo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["activo"]);
            oRol.eliminado = objRow["baja"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["baja"]);
            oRol.fechacreacion = objRow["fechacreacion"] == DBNull.Value ? "" : Convert.ToDateTime(objRow["fechacreacion"]).ToString("dd/MM/yyyy h:mm tt");

        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Roles_INFO> lstRoles)
        {
            LOGI_Roles_INFO objTemp = new LOGI_Roles_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Roles_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstRoles.Add(objTemp);
            }
        }
        public string EditaRol(ref LOGI_ConexionSql_AD oConnection, LOGI_Roles_INFO oRol, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE lm_cat_roles ";
            if (!string.IsNullOrEmpty(oRol.nombre))
            {
                sConsultaSql += string.Format(" {0} nombre = '{1}'", bSET ? "," : "SET", oRol.nombre);
                bSET = true;
            }
            if (oRol.activo >= 0)
            {
                sConsultaSql += string.Format(" {0} activo = {1}", bSET ? "," : "SET", oRol.activo);
                bSET = true;
            }

            if (oRol.eliminado > 0)
            {
                sConsultaSql += string.Format(" {0} baja = {1}", bSET ? "," : "SET", oRol.eliminado);
                bSET = true;
                sConsultaSql += string.Format(" {0} fechabaja = GETDATE()", bSET ? "," : "SET");
                sConsultaSql += string.Format(" {0} usuarioelimina = {1}", bSET ? "," : "SET", oRol.usuarioID);
            }

            sConsultaSql += string.Format(", fechaedicion = GETDATE(),usuarioedita = {0} WHERE rol = {1}", oRol.usuarioID, oRol.rol);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoRol(ref LOGI_ConexionSql_AD oConnection, LOGI_Roles_INFO oRol, ref Int32 iNuevoPerfil, out string sConsultaSql)
        {
            string sResponse = "ERROR";
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = @"BEGIN
               DECLARE @lm_table_temp TABLE(rol smallint NOT NULL);
               BEGIN
               INSERT INTO lm_cat_roles (nombre,fechacreacion,fechaedicion,fechabaja,activo,baja,usuariocrea,usuarioedita,usuarioelimina) 
               OUTPUT INSERTED.rol INTO @lm_table_temp     VALUES(@nombre,GETDATE(),NULL,NULL,1,0,@usuariocrea,NULL,NULL)   END SELECT * FROM @lm_table_temp END";
            oHashParam.Add("@nombre", oRol.nombre);
            oHashParam.Add("@usuariocrea", oRol.usuarioID);
            odataset = oConnection.FillDataSet(sConsultaSql, oHashParam);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow row = odataset.Tables[0].Rows[0];
                iNuevoPerfil = Convert.ToInt32(row["rol"]);
                sResponse = "OK";
            }
            return sResponse;
        }

        public string ListaRoles(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Roles_INFO> lstRoles, LOGI_Roles_INFO perfil, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;

            sConsultaSql = "SELECT * FROM lm_cat_roles";

            if (!string.IsNullOrEmpty(perfil.nombre))
            {
                sConsultaSql += string.Format(" {0} nombre = '{1}'", bAnd ? "AND" : "WHERE", perfil.nombre);
                bAnd = true;
            }

            if (perfil.rol > 0)
            {
                sConsultaSql += string.Format(" {0} rol = {1}", bAnd ? "AND" : "WHERE", perfil.rol);
                bAnd = true;
            }

            if (perfil.bIgnorarol)
            {
                sConsultaSql += string.Format(" {0} rol NOT IN(1,2)", bAnd ? "AND" : "WHERE");
                bAnd = true;
            }

            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstRoles);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevopermisoRol(ref LOGI_ConexionSql_AD oConexion, int iRolID, int iPermisoID, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_cat_permisosrol(rol,permiso) 
                                        VALUES(@rol,@permiso)");
            oHashParam.Add("@rol", iRolID);
            oHashParam.Add("@permiso", iPermisoID);
            iCommand = oConexion.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string Nuevamatrizrol(ref LOGI_ConexionSql_AD oConexion, int iRolID, int Linea, LOGI_Matrizrol_INFO oMatriz, out string sConsultaSql)
        {
            int iCommand = 0;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO cat_matriz_rol(rolid,descripcion,cuenta_inicial,cuenta_final,rango_inicial,rango_final,linea) 
                                        VALUES(@rolid,@descripcion,@cuenta_inicial,@cuenta_final,@rango_inicial,@rango_final,@linea)");
            oHashParam.Add("@rolid", iRolID);
            oHashParam.Add("@descripcion", oMatriz.descripcion);
            oHashParam.Add("@cuenta_inicial", oMatriz.cuenta_inicial);
            oHashParam.Add("@cuenta_final", oMatriz.cuenta_final);
            oHashParam.Add("@rango_inicial", oMatriz.rango_inicial);
            oHashParam.Add("@rango_final", oMatriz.rango_final);
            oHashParam.Add("@linea", Linea);
            iCommand = oConexion.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string Eliminapermisosrol(ref LOGI_ConexionSql_AD oConnection, int iRolID, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = "DELETE FROM lm_cat_permisosrol WHERE rol = " + iRolID;
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string Eliminamatrizporol(ref LOGI_ConexionSql_AD oConnection, int iRolID, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = "DELETE FROM cat_matriz_rol WHERE rolid = " + iRolID;
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

    }
    public class LOGI_Permisos_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow row, ref LOGI_Permisos_INFO otemp)
        {
            otemp.permiso = row["permiso"] == DBNull.Value ? -1 : Convert.ToInt32(row["permiso"]);
            otemp.descripcion = row["descripcion"] == DBNull.Value ? "" : Convert.ToString(row["descripcion"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Permisos_INFO> lstPermisos)
        {
            LOGI_Permisos_INFO objTemp = new LOGI_Permisos_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Permisos_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstPermisos.Add(objTemp);
            }
        }
        public string RecuperaPermisosByRol(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Permisos_INFO> lstPermisos, int iPerfilID, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            oConnection.OpenConnection();
            sConsultaSql = @"select T2.* from lm_cat_permisosrol T1 INNER JOIN 
                            lm_cat_permisos T2 ON T1.permiso = T2.permiso ";
            sConsultaSql += string.Format(" {0} T1.rol = '{1}'", bAnd ? "AND" : "WHERE", iPerfilID);
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstPermisos);
            return lstPermisos.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaMatrizByPerfil(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Matrizrol_INFO> lstMatriz, int iPerfilID, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            string response = "SIN RESULTADOS";
            sConsultaSql = string.Empty;
            bool bAnd = false;
            oConnection.OpenConnection();
            sConsultaSql = @"select * from cat_matriz_rol";
            sConsultaSql += string.Format(" {0} rolid = '{1}'", bAnd ? "AND" : "WHERE", iPerfilID);
            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in odataset.Tables[0].Rows)
                {
                    LOGI_Matrizrol_INFO otemp = new LOGI_Matrizrol_INFO();
                    otemp.descripcion = row["descripcion"] == DBNull.Value ? "" : Convert.ToString(row["descripcion"]);
                    otemp.cuenta_inicial = row["cuenta_inicial"] == DBNull.Value ? "" : Convert.ToString(row["cuenta_inicial"]);
                    otemp.cuenta_final = row["cuenta_final"] == DBNull.Value ? "" : Convert.ToString(row["cuenta_final"]);
                    otemp.rango_inicial = row["rango_inicial"] == DBNull.Value ? 0 : Convert.ToDecimal(row["rango_inicial"]);
                    otemp.rango_final = row["rango_final"] == DBNull.Value ? 0 : Convert.ToDecimal(row["rango_final"]);
                    lstMatriz.Add(otemp);
                }
                response = "OK";
            }
            return response;
        }


        public string RecuperaPermisos(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Permisos_INFO> lstPermisos, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            oConnection.OpenConnection();
            sConsultaSql = @"SELECT * FROM lm_cat_permisos ";
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstPermisos);
            return lstPermisos.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
