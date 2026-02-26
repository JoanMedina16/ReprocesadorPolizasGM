using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFO.Tablas.D365;
using System.Collections;
using System.Data;

namespace AD.Tablas.D365
{
    public class LOGI_CargaInicial_AD
    {
        /// <summary>
        /// Descripción: Query utilizado para insertar un nuevo registro en lm_asientos_d365 de poliza contable
        /// Autor: Ing. Abril Tun Salazar
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="oPoliza">Referencia de objeto para retornar el valor del EXCEL</param>
        /// <param name="sUsuarioID">Referencia al Id del Usuario </param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string InsertaCarga(ref LOGI_ConexionSql_AD oConnection, LOGI_Polizas_INFO oPoliza, out string sConsultaSql, string sUsuarioID, int contador)
        {
            string Respuesta = "ERROR";
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"IF NOT EXISTS(SELECT * FROM lm_asientos_d365 WHERE recIdD365 = '{3}' ) 
                                    BEGIN DECLARE @TABLE_TEMP table(recIdD365 VARCHAR(30) );
                 INSERT lm_asientos_d365 (FolioAsiento,id_tipo_doc,mes,ano, fechaContable, fechaCreacion, fechaInterfaz,idUsuario,recIdD365, estatus,uuid,serie, folio, impuesto )
                                OUTPUT INSERTED.recIdD365 INTO @TABLE_TEMP
                                VALUES ('{0}',{1},{9},{10},GETDATE(),GETDATE(),GETDATE(),{2},'{3}',{4},'{5}','{6}',{7},{8})
                                SELECT * FROM @TABLE_TEMP ;
                            END
           ELSE 
               BEGIN 
                   UPDATE lm_asientos_d365 SET
                   fechaEdicion= GETDATE(), idUsuarioEdita={2}, uuid='{5}', serie='{6}', folio={7}
                    OUTPUT INSERTED.recIdD365 INTO @TABLE_TEMP
                   WHERE recIdD365  = '{3}'  
                   SELECT * FROM @TABLE_TEMP
                END ", oPoliza.FolioAsiento, oPoliza.id_tipo_doc, sUsuarioID, oPoliza.recIdD365, oPoliza.estatus, oPoliza.uuid, oPoliza.serie, oPoliza.folio, oPoliza.impuesto, oPoliza.mes, oPoliza.ano);
            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow orow = odataset.Tables[0].Rows[0];

                if (oPoliza.recIdD365 == Convert.ToString(orow["recIdD365"]))
                    Respuesta = "OK";

            }
            else Respuesta = "SIN RESULTADOS";

            return Respuesta;

        }

        /// <summary>
        /// Descripción: Query utilizado para recupear el total de documento en lm_asientos_d365 
        /// Autor: Ing. Abril Tun Salazar
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="tipodoc">Tipo de documento a filtrar</param>
        /// <param name="Totales">Referencia del total de datos </param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ObtenerConteoPolizas(ref LOGI_ConexionSql_AD oConnection, out string sConsultaSql, ref Int32 Totales, int tipodoc, int mes, int ano)
        {
            string Respuesta = "ERROR";
            DataSet odataset = new DataSet();
            sConsultaSql = string.Format(@"SELECT COUNT(FolioAsiento)+1 as total FROM lm_asientos_d365 WHERE id_tipo_doc={0} AND mes = {1} AND ano = {2} ", tipodoc, mes, ano);
            odataset = oConnection.FillDataSet(sConsultaSql);
            if (odataset.Tables[0].Rows.Count > 0)
            {
                DataRow orow = odataset.Tables[0].Rows[0];
                Totales = Convert.ToInt32(orow["total"]);
                Respuesta = "OK";
            }
            else Respuesta = "SIN RESULTADOS";

            return Respuesta;

        }
    }
}
