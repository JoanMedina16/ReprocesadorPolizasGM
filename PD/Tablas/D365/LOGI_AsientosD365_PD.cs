using AD;
using AD.Tablas.D365;
using AD.Tablas.EQUIV;
using AD.Tablas.FUEL;
using INFO.Enums;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.D365
{
    public class LOGI_AsientosD365_PD
    {

        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Polizas_detalle_AD oPolizadetalle = null;
        internal LOGI_Polizas_AD oPolizas = null;
        const string CONST_CLASE = "LOGI_AsientosD365_PD.cs";
        const string CONST_MODULO = "Polizas D365";
        const int CONST_EMPRESA = 67;///Siempre corresponde a 67 - Logística del Mayab

        public LOGI_AsientosD365_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oPolizadetalle = new LOGI_Polizas_detalle_AD();
            oPolizas = new LOGI_Polizas_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaPolizas(string sUsuarioID, LOGI_Polizas_INFO oParam, ref List<LOGI_Polizas_INFO> lstPolizas, int Top = 0, bool bAscendete = false)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oPolizas.ListaPolizas(ref oConnection, ref lstPolizas, oParam, out sConsultaSql, Top, bAscendete);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaPolizas", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaComentario(string sUsuarioID, LOGI_Polizas_INFO oParam, ref LOGI_Polizas_INFO oPoliza)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oPolizas.Listacomentario(ref oConnection, ref oPoliza, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaComentario", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaMasterEDIXML(string sUsuarioID, ref List<LOGI_Factura_INFO> lstFacturas, LOGI_Factura_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = new LOGI_FacturasCFDI_AD().RecuperaXML(ref oConnection, ref lstFacturas, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaMasterEDIXML", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaBandejaLM(string sUsuarioID, ref List<LOGI_Factura_INFO> lstFacturas, LOGI_Factura_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = new LOGI_FacturasCFDI_AD().RecuperaFacturaBandjeaLM(ref oConnection, ref lstFacturas, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaBandejaLM", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaDatoAdicionalFactura(string sUsuarioID, ref List<LOGI_Factura_INFO> lstFacturas, LOGI_Factura_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = new LOGI_FacturasCFDI_AD().ListadatosAdiciolFactura(ref oConnection, ref lstFacturas, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaDatoAdicionalFactura", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaXML(string sUsuarioID, ref LOGI_Polizas_INFO oParam, String TransaccID)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                sReponse = oPolizas.RecuperaXML(ref oConnection, ref oParam, TransaccID, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaXML", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaCatalogoImpuestos(string sConnectionEquiv,string sUsuarioID, ref List<LOGI_Catalogos_INFO> lstCatalogo, LOGI_Catalogos_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            LOGI_ConexionSql_AD oConnEquiv = null;
            try
            {

                oConnEquiv = new LOGI_ConexionSql_AD(sConnectionEquiv);
                sReponse = new LOGI_Impuestos_AD().RecuperaImpuestos(ref oConnEquiv, ref lstCatalogo, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaXML", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnEquiv != null)
                    oConnEquiv.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaCatalogoFiliales(string sConnectionEquiv, string sUsuarioID, ref List<LOGI_Catalogos_INFO> lstCatalogo, LOGI_Catalogos_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            LOGI_ConexionSql_AD oConnEquiv = null;
            try
            {

                oConnEquiv = new LOGI_ConexionSql_AD(sConnectionEquiv);
                sReponse = new LOGI_Filialesterceros_AD().RecuperaFilieales(ref oConnEquiv, ref lstCatalogo, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaXML", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnEquiv != null)
                    oConnEquiv.CloseConnection();
            }
            return sReponse;
        }


        public string Creaerror(string sUsuarioID, String mensaje, String FolioAsiento, string sJSON, string sResponse, string sURLEndPoint)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                oConnection.StarTransacction();
                List<LOGI_Transacciones_INFO> lstTransacciones = new List<LOGI_Transacciones_INFO>();
                LOGI_ConfiguracionD365_INFO oConfig = new LOGI_ConfiguracionD365_INFO();
                LOGI_Transacciones_INFO otemp = new LOGI_Transacciones_INFO();
                otemp.FolioAsiento = FolioAsiento;
                sReponse = new LOGI_Transacciones_AD().ListaTransacciones(ref oConnection, ref lstTransacciones, otemp, out sConsultaSql);
                
                if (lstTransacciones.Count == 1)
                {
                    otemp = lstTransacciones[0];
                    otemp.intento++;
                    otemp.mensaje = mensaje.Length > 1000 ? mensaje.Substring(0, 999) : mensaje;
                    otemp.respuesta = sResponse;
                    otemp.peticion = sJSON;
                    otemp.urlweb = sURLEndPoint;
                    sReponse = new LOGI_Transacciones_AD().ActualizaTransaccion(ref oConnection, otemp, out sConsultaSql);
                    if (sReponse != "OK")
                        throw new Exception("No se ha podido actualizar el documento");
                    sReponse = new LOGI_ConfiguracionD365_AD().ListaConfiguracion(ref oConnection, ref oConfig, out sConsultaSql);
                    if (sReponse != "OK")
                        throw new Exception("No se ha determinado la configuración para límite de errores");
                    if (otemp.intento >= oConfig.intentos)
                    {
                        //actualizamos como error el registro
                        LOGI_Polizas_INFO oParam = new LOGI_Polizas_INFO();
                        oParam.FolioAsiento = FolioAsiento;
                        oParam.estatus = 1;
                        sReponse = oPolizas.ActualizaPoliza(ref oConnection, sUsuarioID, oParam, out sConsultaSql);
                        if (sReponse != "OK")
                            throw new Exception("No se ha podido actualizar el documento");
                    }

                }                
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnection.RollBackTransacction();
                oTool.LogError(ex, "Creaerror", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }
        public string Actualizapoliza(string sUsuarioID, LOGI_Polizas_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                oConnection.StarTransacction();
                sReponse = oPolizas.ActualizaPoliza(ref oConnection, sUsuarioID, oParam, out sConsultaSql);                
                if (!string.IsNullOrEmpty(oParam.recIdD365) && oParam.estatus == 2)
                {
                    ///SI EL ESTATUS DE LA PÓLIZA ES IGUAL A 1= SINCRONIZADA CON D365 SE ACTUALIZAN TODOS LOS DOCUMENTOS INVOLUCRADOS 
                    switch (oParam.eTypedoc)
                    {
                        case eDocumentoPolizas.COMBUSTIBLES_COSTO:
                        case eDocumentoPolizas.COMBUSTIBLES_PROVEEDOR:
                            new LOGI_Tickets_AD().ActualizaTickets(ref oConnection, oParam.FolioAsiento, oParam.recIdD365, out sConsultaSql);
                            break;

                        case eDocumentoPolizas.FACTURACION_DE_PORTES:
                        case eDocumentoPolizas.FACTURACION_VARIA:
                        case eDocumentoPolizas.NOTAS_DE_CREDITO:
                            sReponse = new LOGI_FacturasCFDI_AD().ActualizaLMFactura(ref oConnection, oParam.FolioAsiento, oParam.recIdD365, out sConsultaSql);
                            break;
                    }
                    ///SI EL ESTATUS DE LA PÓLIZA ES IGUAL A 1= SINCRONIZADA CON D365 SE ACTUALIZAN TODOS LOS DOCUMENTOS INVOLUCRADOS 
                    new LOGI_Transacciones_AD().EliminaTransaccion(ref oConnection, oParam.FolioAsiento, out sConsultaSql);
                }
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnection.RollBackTransacction();
                oTool.LogError(ex, "Actualizapoliza", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ActualizaPolizasTransaccion(string sUsuarioID, List<LOGI_Polizas_INFO> lstPolizas, int intentos = 0)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                oConnection.StarTransacction();
                LOGI_Polizas_INFO otemp = null;
                LOGI_Transacciones_INFO oTransacc = null;
                foreach (LOGI_Polizas_INFO o in lstPolizas)
                {
                    oTransacc = new LOGI_Transacciones_INFO();
                    otemp = new LOGI_Polizas_INFO();
                    otemp.estatus = o.estatus;
                    otemp.FolioAsiento = o.FolioAsiento;
                    otemp.comments = o.comments;
                    if (intentos >= 0)
                        oTransacc.intento = intentos;
                    oTransacc.FolioAsiento = o.FolioAsiento;
                    sReponse = oPolizas.ActualizaPoliza(ref oConnection, sUsuarioID, otemp, out sConsultaSql);
                    if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception("No se ha podido actualizar el registro de la poliza");
                    sReponse = new LOGI_Transacciones_AD().ActualizaTransaccion(ref oConnection, oTransacc, out sConsultaSql);
                    //if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                    //throw new Exception("No se ha podido actualizar el registro de la poliza");
                    sReponse = "OK";
                }
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnection.RollBackTransacction();
                oTool.LogError(ex, "ReiniciIntentoPolizas", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
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
