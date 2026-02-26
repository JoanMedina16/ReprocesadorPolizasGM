using AD;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Objetos.OPE
{
   public class LOGI_Gstoviaje_PD
    {

        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnectionLOG = null;
        internal LOGI_ConexionSql_AD oConnectionOPE = null;
        const string CONST_CLASE = "LOGI_Gstoviaje_PD.cs";
        const string CONST_MODULO = "Gastos de viaje";

        public LOGI_Gstoviaje_PD(string sConnbitacora, string sConnopeadm)
        {
            oConnectionLOG = new LOGI_ConexionSql_AD(sConnbitacora);
            oConnectionOPE = new LOGI_ConexionSql_AD(sConnopeadm);
            oTool = new LOGI_Tools_PD();
        }

        /*
        public string GenerarPolizasReposicionFondoFijo(LOGI_FondoFijo_INFO oFondofijo)
        {
            string response = "ERROR", tipo = string.Empty, mensaje = string.Empty, sConsultaSql = string.Empty;
            try
            {

                oConnectionLOG.OpenConnection();
                oConnectionOPE.OpenConnection();
                oConnectionLOG.StarTransacction();
                oConnectionOPE.StarTransacction();

                Hashtable oParams = new Hashtable();
                DataTable tblViajes = this.CreateViajeTableType();
                // Datos de viajes.
                var row = tblViajes.NewRow();
                row[ViajeTableType.FolioOTM] = oFondofijo.Folio;
                row[ViajeTableType.OperadorOTM] = oFondofijo.OperadorId;
                row[ViajeTableType.TractorOTM] = oFondofijo.TractorId;
                row[ViajeTableType.Importe] = oFondofijo.Importe;
                row[ViajeTableType.Cuenta] = oFondofijo.Cuenta;
                tblViajes.Rows.Add(row);
                oParams.Add("@viajes", tblViajes);
                oParams.Add("@usuario", oFondofijo.UsuarioId);
                oParams.Add("@proceso_id", new Guid());
                DataSet dsRespuesta = oConnectionOPE.ExecteProcedure("usp_reposicion_fondo_fijo_tms", oParams);
                if (dsRespuesta.Tables[0].Rows.Count > 0)
                {
                    DataRow orow = dsRespuesta.Tables[0].Rows[0];
                    tipo = Convert.ToString(orow["tipo"]);
                    mensaje = Convert.ToString(orow["mensaje"]);

                    if (tipo == "I" && mensaje.Contains("correctamente."))
                    {
                        //Insertar bitacora 
                        oParams = new Hashtable();
                        sConsultaSql = string.Format(@"INSERT INTO lm_tms_gasto_viaje(folio_tms,folio_viaje,operador_id,tractor_id)
            VALUES(@folio_tms,@folio_viaje,@operador_id,@tractor_id)");
                        oParams.Add("@folio_tms", oFondofijo.FolioTMS);
                        oParams.Add("@folio_viaje", oFondofijo.Folio);
                        oParams.Add("@operador_id", oFondofijo.OperadorId);
                        oParams.Add("@tractor_id", oFondofijo.TractorId);
                        int icommand = oConnectionLOG.ExecuteCommand(sConsultaSql, oParams);
                        if (icommand == 0)
                            throw new Exception("No se ha podido insertar el movimiento en bitacora TMS");
                        response = "OK";
                    }

                }
                else response = "SIN RESULTADOS";

                oConnectionLOG.CommitTransacction();
                oConnectionOPE.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnectionLOG.RollBackTransacction();
                oConnectionOPE.RollBackTransacction();
                oTool.LogError(ex, "GenerarPolizasReposicionFondoFijo", "", CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                response = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();

                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();
            }
            return response;
        }

        public string ConsultaBitacoraGasto(string sUsuarioID, LOGI_Bitacora_INFO oParam, ref LOGI_Bitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionLOG.OpenConnection();

                DataSet odataset = new DataSet();
                bool bAnd = false;
                sConsultaSql = string.Format(@"SELECT folio_tms FROM lm_tms_gasto_viaje ");

                if (!string.IsNullOrEmpty(oParam.folioD365))
                {
                    sConsultaSql += string.Format(" {0} folio_tms = '{1}'", bAnd ? "AND" : "WHERE", oParam.folioD365);
                    bAnd = true;
                }

                odataset = oConnectionLOG.FillDataSet(sConsultaSql);
                if (odataset.Tables[0].Rows.Count > 0)
                {
                    DataRow orow = odataset.Tables[0].Rows[0];
                    oBitacora.proceso_id_d365 = orow["folio_tms"] == DBNull.Value ? "" : Convert.ToString(orow["folio_tms"]);
                }
                return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
            }
            catch (Exception ex)
            {

                oTool.LogError(ex, "ConsultaBitacoraGasto", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return sReponse;
        }

        public static class ViajeTableType
        {
            public static string FolioOTM = "folio_otm";
            public static string OperadorOTM = "operador_otm";
            public static string TractorOTM = "tractor_otm";
            public static string Importe = "importe";
            public static string Cuenta = "cuenta_banco";
        }
        private DataTable CreateViajeTableType()
        {
            try
            {
                DataTable tblViajeTableType = new DataTable();

                tblViajeTableType.Columns.Add(ViajeTableType.FolioOTM, typeof(String));
                tblViajeTableType.Columns.Add(ViajeTableType.OperadorOTM, typeof(String));
                tblViajeTableType.Columns.Add(ViajeTableType.TractorOTM, typeof(String));
                tblViajeTableType.Columns.Add(ViajeTableType.Importe, typeof(Decimal));
                tblViajeTableType.Columns.Add(ViajeTableType.Cuenta, typeof(String));
                return tblViajeTableType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        */
    }
}
