using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.FUEL
{
    public class LOGI_Tickets_AD
    {

        /// <summary>
        /// Descripción: Query utilizado para la actualización de los datos de la tabla lm_asientos_d365, se actualiza según los datos 
        /// enviados en objeto oParam
        /// 
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades a actualizar</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ActualizaTickets(ref LOGI_ConexionSql_AD oConnection, String FolioAsiento, String DocumentoD365 , out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            int icommand = -1; 
            sConsultaSql = string.Empty;
            sConsultaSql += string.Format(@"update opesat.movcombus set polizad365 = '{0}' where  FolioAsiento = '{1}'", DocumentoD365, FolioAsiento);
            sConsultaSql += string.Format(@"update lm_valescomb set polizad365 = '{0}' where  FolioAsiento = '{1}'", DocumentoD365, FolioAsiento);
            sConsultaSql += string.Format(@"update siat.emisioncombus set polizad365 = '{0}' where  FolioAsiento = '{1}'", DocumentoD365, FolioAsiento);
            icommand = oConnection.ExecuteCommand(sConsultaSql);            
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
