using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AD;
using AD.Tablas.D365;
using INFO.Tablas.D365;
using PD.Herramientas;

namespace PD.Tablas.D365
{
    public class LOGI_CargaInicial_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_CargaInicial_AD oCargaInicial = null;
        const string CONST_CLASE = "LOGI_CargaInicial_PD.cs";
        const string CONST_MODULO = " Carga Inicial";

        public LOGI_CargaInicial_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oCargaInicial = new LOGI_CargaInicial_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ProcesaCargaInicial(string sUsuarioID,  List<LOGI_Polizas_INFO> lstPolizas)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                Int32 contador = 0;
                oConnection.OpenConnection();
                oConnection.StarTransacction();
                string prefix = DateTime.Now.ToString("yyyyMMdd");

                //se listan las polizas por el estatus número cuatro para poder realizar los incrementos de datos
                oCargaInicial.ObtenerConteoPolizas(ref oConnection, out sConsultaSql, ref contador, 99, DateTime.Now.Month, DateTime.Now.Year);
                if (contador <= 0)
                    throw new Exception("No se ha podido determinar el siguiente folio consecutivo");
                foreach (LOGI_Polizas_INFO data in lstPolizas)
                {
                    contador++;
                    data.FolioAsiento = string.Format("LM.CI{0}{1}", prefix, contador);
                    data.id_tipo_doc = 99;
                    data.estatus = 4;
                    data.impuesto = 0.00m;
                    data.mes = DateTime.Now.Month;
                    data.ano = DateTime.Now.Year;
                    sReponse = oCargaInicial.InsertaCarga(ref oConnection, data, out sConsultaSql, sUsuarioID, contador);
                    if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception("Ocurrio un error al intentar crear el documento con RECID " + data.recIdD365);                    
                }
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnection.RollBackTransacction();
                oTool.LogError(ex, "ProcesaCargaInicial", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido crear la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

    }
}
