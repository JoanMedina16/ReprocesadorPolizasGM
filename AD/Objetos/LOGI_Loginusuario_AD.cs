using INFO.Tablas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos
{
   public class LOGI_Loginusuario_AD
    {
        internal Hashtable oHashParam = null;

        public string BuscaUsuarioSystem(ref LOGI_ConexionSql_AD oConnection, ref LOGI_Usuarios_INFO oUsuario, LOGI_Usuarios_INFO oParam, out string sConsultaSql,string sDatabase)
        {
            string sresponse = "SIN RESULTADOS";
            DataSet odataset = new DataSet();
            bool bAnd = false;
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"select al.cia, al.alias, al.password1, us.usuario,us.nomuser, cu.perfil from master..sysalias al inner join 
                             {0}..oa_usuarios us on us.cia = al.cia and  us.alias = al.alias 
                             inner join {0}..lm_cat_usuarios cu on cu.alias = us.alias", sDatabase);

            if (!string.IsNullOrEmpty(oParam.sUsuario))
            {
                sConsultaSql += string.Format(" {0} ltrim(rtrim(upper(al.alias))) = '{1}'", bAnd ? "AND" : "WHERE", oParam.sUsuario.ToUpper());
                bAnd = true;
            }

            sConsultaSql += string.Format(" {0} al.cia = 67 and al.bloqueo = 0 and intentos < 4 AND cu.activo = 1 ", bAnd ? "AND" : "WHERE");
            odataset = oConnection.FillDataSet(sConsultaSql);

            if (odataset.Tables[0].Rows.Count > 0)
            {
                oUsuario = new LOGI_Usuarios_INFO();
                DataRow oorow = odataset.Tables[0].Rows[0];
                //oUsuario.iCia = oorow["cia"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["cia"]);
                oUsuario.sUsuario= oorow["alias"] == DBNull.Value ? "" : Convert.ToString(oorow["alias"]);
                oUsuario.sContrasenia = oorow["password1"] == DBNull.Value ? "" : Convert.ToString(oorow["password1"]);
                oUsuario.iUsuario = oorow["usuario"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["usuario"]);
                oUsuario.sNombre = oorow["nomuser"] == DBNull.Value ? "" : Convert.ToString(oorow["nomuser"]);
                oUsuario.iPerfil = oorow["perfil"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["perfil"]);
                sresponse = "OK";
            }
            return sresponse;
        }


        public string ListausuariosOPE(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Usuarios_INFO> lstUsuarios, LOGI_Usuarios_INFO oParam, out string sConsultaSql)
        {
            string sresponse = "SIN RESULTADOS";
            DataSet odataset = new DataSet();
            bool bAnd = false;
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"select cia,suc,usuario,emp,nomuser,alias from oa_usuarios");

            if (!string.IsNullOrEmpty(oParam.sUsuario))
            {
                sConsultaSql += string.Format(" {0} ltrim(rtrim(upper(alias))) = '{1}'", bAnd ? "AND" : "WHERE", oParam.sUsuario.ToUpper());
                bAnd = true;
            }

            if (oParam.sUsuarioExcep > 0)
            {
                sConsultaSql += string.Format(" {0} usuario <> {1}", bAnd ? "AND" : "WHERE", oParam.sUsuarioExcep);
                bAnd = true;
            }

            sConsultaSql += string.Format(" {0} bloqueo = 0 ", bAnd ? "AND" : "WHERE");
            odataset = oConnection.FillDataSet(sConsultaSql);

            if (odataset.Tables[0].Rows.Count > 0)
            {
                LOGI_Usuarios_INFO tmp = null;
                foreach (DataRow row in odataset.Tables[0].Rows)
                {
                    tmp = new LOGI_Usuarios_INFO();
                    tmp.sUsuario = row["alias"] == DBNull.Value ? "" : Convert.ToString(row["alias"]);
                    tmp.iUsuario = row["usuario"] == DBNull.Value ? -1 : Convert.ToInt32(row["usuario"]);
                    tmp.sNombre = row["nomuser"] == DBNull.Value ? "" : Convert.ToString(row["nomuser"]);
                    lstUsuarios.Add(tmp);
                }
                sresponse = "OK";
            }
            return sresponse;
        }

        public string ValidaSucursal(ref LOGI_ConexionSql_AD oConnection, out string sConsultaSql, ref LOGI_Usuarios_INFO oUsuario)
        {
            string sresponse = "SIN RESULTADOS";
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            if (oUsuario.iAdministrador == 0)
            {
                sConsultaSql = string.Format(@"select    cia = (case when v.ciav is null then us.cia else v.ciav end),                                 
								suc = (case when v.sucv is null then us.suc else v.sucv end),
								us.usuario, 
                                cambio = (case when v.ciav is null then '0' else '1' end), T3.nomciaab
                        from oa_usuarios us                     
                        left outer join siat.sucvirtual v on v.ciav = us.cia and v.usuario = us.usuario
						INNER JOIN cias T3 on (T3.cia = us.cia AND (case when v.sucv is null then us.suc else v.sucv end) = T3.suc )
                        where lower(rtrim(ltrim(us.alias))) = lower('{0}')  and us.cia = 67", oUsuario.sUsuario);
            }
            else
            {
                sConsultaSql = @"select T1.cia,T1.suc, 0 as usuario , T3.nomciaab from oa_config T1
                                 INNER JOIN cias T3 on (T3.cia = T1.cia AND T1.suc = T3.suc ) ";
            }
            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow oorow = odataset.Tables[0].Rows[0];                              
                oUsuario.iSucursal = oorow["suc"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["usuario"]);
                oUsuario.sSucursal = oorow["nomciaab"] == DBNull.Value ? "" : Convert.ToString(oorow["nomciaab"]);
                sresponse = "OK";
            }
            sresponse = odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
            return sresponse;
        }

    }
}
