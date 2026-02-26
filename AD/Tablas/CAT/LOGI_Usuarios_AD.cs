using INFO.Tablas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.CAT
{
    public class LOGI_Usuarios_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow objRow, ref LOGI_Usuarios_INFO oUsuario)
        {
            oUsuario.iUsuario = objRow["usuario"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["usuario"]);
            oUsuario.sUsuario = objRow["alias"] == DBNull.Value ? "" : Convert.ToString(objRow["alias"]);
            oUsuario.sNombre = objRow["nombre"] == DBNull.Value ? "" : Convert.ToString(objRow["nombre"]);
            oUsuario.iPerfil = objRow["perfil"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["perfil"]);
            oUsuario.sCorreo = objRow["correo"] == DBNull.Value ? "" : Convert.ToString(objRow["correo"]);
            oUsuario.iActivo = objRow["activo"] == DBNull.Value ? -1 : Convert.ToInt32(objRow["activo"]);
            oUsuario.dtFechAlta = objRow["fechaalta"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objRow["fechaalta"]);
            if (oUsuario.dtFechAlta != DateTime.MinValue)
                oUsuario.sFechaalta = oUsuario.dtFechAlta.ToString("dd/MM/yyyy h:mm tt");
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Usuarios_INFO> lstUsuarios)
        {
            LOGI_Usuarios_INFO objTemp = new LOGI_Usuarios_INFO();
            foreach (DataRow orow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Usuarios_INFO();
                this.GetObjeto(orow, ref objTemp);
                lstUsuarios.Add(objTemp);
            }
        }
        public string EditaUsuario(ref LOGI_ConexionSql_AD oConnection, LOGI_Usuarios_INFO oUsuario, out string sConsultaSql)
        {
            bool bSET = false;
            int iCommand = 0;
            sConsultaSql = "UPDATE lm_cat_usuarios ";

            if (!string.IsNullOrEmpty(oUsuario.sNombre))
            {
                sConsultaSql += string.Format(" {0} nombre = '{1}'", bSET ? "," : "SET", oUsuario.sNombre);
                bSET = true;
            }

            if (oUsuario.iPerfil > 0)
            {
                sConsultaSql += string.Format(" {0} perfil = {1}", bSET ? "," : "SET", oUsuario.iPerfil);
                bSET = true;
            }

            if (oUsuario.iActivo >= 0)
            {
                sConsultaSql += string.Format(" {0} activo = {1}", bSET ? "," : "SET", oUsuario.iActivo);
                bSET = true;
            }
              
            if (!string.IsNullOrEmpty(oUsuario.sCorreo))
            {
                sConsultaSql += string.Format(" {0} correo = '{1}'", bSET ? "," : "SET", oUsuario.sCorreo);
                bSET = true;
            }
            sConsultaSql += string.Format(", fechaedicion = GETDATE(),usuarioedita = {0} WHERE usuario = {1}", oUsuario.iUsuariocrea, oUsuario.iUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string NuevoUsuario(ref LOGI_ConexionSql_AD oConnection, LOGI_Usuarios_INFO oUsuario, out string sConsultaSql)
        {
            int iCommand = -1;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_cat_usuarios(usuario,alias,nombre,fechaalta,usuariocrea,activo,perfil,correo) 
            VALUES(@usuario,@alias,@nombre,getdate(),@usuariocrea,1,@perfil,@correo)");
            oHashParam.Add("@usuario", oUsuario.iUsuario);
            oHashParam.Add("@alias", oUsuario.sUsuario);
            oHashParam.Add("@nombre", oUsuario.sNombre);
            oHashParam.Add("@usuariocrea", oUsuario.iUsuariocrea);
            oHashParam.Add("@perfil", oUsuario.iPerfil);
            oHashParam.Add("@correo", oUsuario.sCorreo);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string RecuperaUsuario(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Usuarios_INFO> lstUsuarios, LOGI_Usuarios_INFO oUsuario, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            oConnection.OpenConnection();
            sConsultaSql = "SELECT * FROM lm_cat_usuarios ";

            if (oUsuario.iUsuario > 0)
            {
                sConsultaSql += string.Format(" {0} usuario = {1}", bAnd ? "AND" : "WHERE", oUsuario.iUsuario);
                bAnd = true;
            }
             
            if (!string.IsNullOrEmpty(oUsuario.sUsuario))
            {
                sConsultaSql += string.Format(" {0} alias LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oUsuario.sUsuario);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oUsuario.sNombre))
            {
                sConsultaSql += string.Format(" {0} nombre LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oUsuario.sNombre);
                bAnd = true;
            }

            if (oUsuario.iActivo > 0)
            {
                sConsultaSql += string.Format(" {0} activo = {1}", bAnd ? "AND" : "WHERE", oUsuario.iActivo);
                bAnd = true;
            }

            if (oUsuario.sUsuarioExcep >0)
            {
                sConsultaSql += string.Format(" {0} usuario <> '{1}'", bAnd ? "AND" : "WHERE", oUsuario.sUsuarioExcep);
                bAnd = true;
            }

            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstUsuarios);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string EliminaPermisos(ref LOGI_ConexionSql_AD oConnection, int sUsuario, out string sConsultaSql)
        {
            int iCommand = -1;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format("DELETE FROM lm_usuariosacceso_d365 where usuarioid = {0}", sUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string Creapermiso(ref LOGI_ConexionSql_AD oConnection, string Clavemod, int iUsuario, out string sConsultaSql)
        {
            int iCommand = -1;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_usuariosacceso_d365(clavemod,usuarioid) 
            VALUES(@clavemod,@usuarioid)");
            oHashParam.Add("@clavemod", Clavemod);
            oHashParam.Add("@usuarioid", iUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }


        public string EliminaPermisosDiario(ref LOGI_ConexionSql_AD oConnection, int sUsuario, out string sConsultaSql)
        {
            int iCommand = -1;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format("DELETE FROM lm_usuariodiarios_d365 where usuarioid = {0}", sUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string CreapermisoDiario(ref LOGI_ConexionSql_AD oConnection, string Clavediario, int iUsuario, out string sConsultaSql)
        {
            int iCommand = -1;
            oHashParam = new Hashtable();
            sConsultaSql = string.Format(@"INSERT INTO lm_usuariodiarios_d365(clavediario,usuarioid) 
            VALUES(@clavediario,@usuarioid)");
            oHashParam.Add("@clavediario", Clavediario);
            oHashParam.Add("@usuarioid", iUsuario);
            iCommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return iCommand > 0 ? "OK" : "SIN RESULTADOS";
        }


    }
}
