using INFO.Tablas.D365;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.D365
{
    public class LOGI_Documentos_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_Documentos_INFO otemp)
        {
            otemp.numerodoc = orow["id"] == DBNull.Value ? -1 : Convert.ToInt32(orow["id"]);
            otemp.nombre = orow["nombre"] == DBNull.Value ? "" : Convert.ToString(orow["nombre"]);
            otemp.diario = orow["diario"] == DBNull.Value ? "" : Convert.ToString(orow["diario"]);
            otemp.metodo = orow["metodo"] == DBNull.Value ? "" : Convert.ToString(orow["metodo"]);
            otemp.activo = orow["activo"] == DBNull.Value ? 0 : Convert.ToInt32(orow["activo"]); 
        }

        void GetObjetoProcesa(DataRow orow, ref LOGI_DocumentosProcesa_INFO otemp)
        {
            otemp.Id = Convert.ToInt32(orow["Id"]);
            otemp.Proceso = Convert.ToString(orow["Proceso"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_Documentos_INFO> lstDocumentos)
        {
            LOGI_Documentos_INFO objTemp = new LOGI_Documentos_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_Documentos_INFO();
                this.GetObjeto(oorow, ref objTemp);
                lstDocumentos.Add(objTemp);
            }
        }

        void LoopDataSetProcesa(DataSet objDataSet, ref List<LOGI_DocumentosProcesa_INFO> lstDocumentos)
        {
            LOGI_DocumentosProcesa_INFO objTemp = new LOGI_DocumentosProcesa_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_DocumentosProcesa_INFO();
                this.GetObjetoProcesa(oorow, ref objTemp);
                lstDocumentos.Add(objTemp);
            }
        }
        /// <summary>
        /// Descripción: Query utilizado para recuperarlos documentos validos de D365
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="lstPolizas">Lista de objetos referenciada</param>
        /// <param name="oParam">Objeto de tipo poliaza para filtro de datos</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string lstDocumentos(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Documentos_INFO> lstDocumentos, LOGI_Documentos_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = true;
            sConsultaSql = string.Format(@"SELECT * FROM lm_documentos_d365 WHERE activo = 1");
            if (oParam.numerodoc >0)
            {
                sConsultaSql += string.Format(" {0} id = {1}", bAnd ? "AND" : "WHERE", oParam.numerodoc);
                bAnd = true;
            }

            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstDocumentos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string lstDocumentosProcesa(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_DocumentosProcesa_INFO> lstDocumentos, LOGI_DocumentosProcesa_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = true;
            sConsultaSql = string.Format(@"SELECT Id, Proceso FROM lm_tms_Procesos;");

            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSetProcesa(odataset, ref lstDocumentos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }


        public string lstDocumentosXusuario(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_Documentos_INFO> lstDocumentos, string sUsuario, int iUsuario, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"select T3.* from lm_cat_usuarios T1 
            INNER JOIN lm_usuariodiarios_d365 T2 ON 
            T1.usuario = T2.usuarioid INNER JOIN 
            lm_documentos_d365 T3 ON T2.clavediario = T3.id  ");

            if (!string.IsNullOrEmpty(sUsuario))
            {
                sConsultaSql += string.Format(" {0} T1.alias = '{1}'", bAnd ? "AND" : "WHERE", sUsuario);
                bAnd = true;
            }

            if (iUsuario >0)
            {
                sConsultaSql += string.Format(" {0} T1.usuario = {1}", bAnd ? "AND" : "WHERE", iUsuario);
                bAnd = true;
            }


            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstDocumentos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

        public string lstDocumentosProcesaXusuario(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_DocumentosProcesa_INFO> lstDocumentos, string sUsuario, int iUsuario, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = false;
            sConsultaSql = string.Format(@"SELECT Id, Proceso FROM lm_tms_Procesos;");

            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSetProcesa(odataset, ref lstDocumentos);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }


        /// <summary>
        /// Descripción: Query utilizado para la actualización de los datos de la tabla lm_documentos_d365, se actualiza según los datos 
        /// enviados en objeto oParam
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades a actualizar</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string Actualizadocumentos(ref LOGI_ConexionSql_AD oConnection, string sUsuarioID, LOGI_Documentos_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_documentos_d365");

            if (!string.IsNullOrEmpty(oParam.nombre))
            {
                sConsultaSql += string.Format(" {0} nombre = '{1}'", bSET ? "," : "SET", oParam.nombre);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.metodo))
            {
                sConsultaSql += string.Format(" {0} metodo = '{1}'", bSET ? "," : "SET", oParam.metodo);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.diario))
            {
                sConsultaSql += string.Format(" {0} diario = '{1}'", bSET ? "," : "SET", oParam.diario);
                bSET = true;
            }

            sConsultaSql += string.Format(" {0} activo = {1}", bSET ? "," : "SET", oParam.activo);
            bSET = true;

            sConsultaSql += string.Format(" WHERE id = {0}", oParam.numerodoc);
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Metodo utilizado para agregar un nuevo registro en lm_documentos_d365
        /// Autor: Ing. Grupo consultor GB
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string Creadocumento(ref LOGI_ConexionSql_AD oConnection, string sUsuarioID, LOGI_Documentos_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;            
            sConsultaSql = string.Format(@"INSERT lm_documentos_d365 (id,nombre,diario,metodo)
                                   VALUES (@id,@nombre,@diario,@metodo)");
            oHashParam.Add("@id", oParam.numerodoc);
            oHashParam.Add("@nombre", oParam.nombre);
            oHashParam.Add("@diario", oParam.diario);
            oHashParam.Add("@metodo", oParam.metodo);             
            int icommand = oConnection.ExecuteCommand(sConsultaSql, oHashParam);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
