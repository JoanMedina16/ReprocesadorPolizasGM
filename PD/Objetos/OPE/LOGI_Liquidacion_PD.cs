using AD;
using AD.Objetos.OPE;
using INFO.Tablas.OPE;
using PD.Herramientas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Objetos.OPE
{
   public class LOGI_Liquidacion_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnectionLOG = null;
        internal LOGI_ConexionSql_AD oConnectionOPE = null;
        internal LOGI_Liquidacion_AD oLiquidacionAD = null;
        const string CONST_CLASE = "LOGI_Liquidacion_PD.cs";
        const string CONST_MODULO = "Liquidaciones";

        public LOGI_Liquidacion_PD(string sConnbitacora, string sConnopeadm)
        {
            oConnectionLOG = new LOGI_ConexionSql_AD(sConnbitacora);
            oConnectionOPE = new LOGI_ConexionSql_AD(sConnopeadm);
            oLiquidacionAD = new LOGI_Liquidacion_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string InsertaMovimientoCombustible(string sUsuarioID, List<LOGI_LiquidacionCombus_INFO> lstLiquidaciones, LOGI_LiquidacionBitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty, SqueryAux;
            sReponse = "ERROR";
            int iCommand = 0;
            string CodigoOperador = string.Empty, NombreOperador = string.Empty;
            Decimal Litros = 0, PrecioLitro = 0;
            try
            {
                oConnectionLOG.OpenConnection();
                oConnectionOPE.OpenConnection();
                oConnectionLOG.StarTransacction();
                oConnectionOPE.StarTransacction();

                sReponse = oLiquidacionAD.ListaBitacora(ref oConnectionLOG, oBitacora, out sConsultaSql);///Crea la bitacora

                if (sReponse.Equals("SIN RESULTADOS", StringComparison.InvariantCultureIgnoreCase))
                {
                    //se recuperan los conceptos que aplican para combustibles dentro de la liquidación 
                    List<LOGI_Combustibles_INFO> lstConceptos = new List<LOGI_Combustibles_INFO>();
                    oLiquidacionAD.ListaConceptosCombustibles(ref oConnectionOPE, ref lstConceptos, out sConsultaSql);
                    if (lstConceptos.Count == 0)
                        throw new Exception("No se han encontrado resultados de conceptos de combustibles");

                    //Sobre el listado de liquidación se hace un barrido para tomar solo las cuentas de combustibles de consumo interno (UREA,DIESEL,ETC)
                    List<LOGI_LiquidacionCombus_INFO> lstConsumos = this.ListaConceptoscombustibles(lstLiquidaciones, lstConceptos);
                    if (lstConsumos.Count > 0)
                    {
                        //La liquidación contiene movimientos de consumo en estaciones, los movimientos se insertan en BD para que puedan ser 
                        Hashtable oHashParam = new Hashtable();
                        DataSet dsUnidad = null;
                        string NoEconomico = string.Empty;
                        foreach (LOGI_LiquidacionCombus_INFO oConsumo in lstConsumos)
                        {
                            sConsultaSql = string.Format(@"INSERT INTO lm_combus_cargas_tms(usuario,nombreusuario,fechaliquidacion,fecharegistro,fechacarga,estacion,economico,corporativo,folio,folioelectronico,estatus,litros,
                                                                                        total,poliza,foliod365,folioviaje,sucursal,centro,area,depto,filial,comentarios,nombreperador,codigooperador,preciolitro,codestacion)
            VALUES(NULL,NULL,@fechaliquidacion,getdate(),@fechacarga,@estacion,@economico,@corporativo,@folio,NULL,1,@litros,
                                                                                        @total,NULL,NULL,@folioviaje,@sucursal,@centro,@area,@depto,@filial,@comentarios,@nombreperador,@codigooperador,@preciolitro,@codestacion)");

                            SqueryAux =  "select descrip from oa_catalogo where cuenta = 423 and clave = '" + oConsumo.vehiculo + "'";
                            dsUnidad = oConnectionOPE.FillDataSet(SqueryAux, oHashParam);
                            if (dsUnidad.Tables[0].Rows.Count > 0)
                                NoEconomico = dsUnidad.Tables[0].Rows[0]["descrip"].ToString().Trim();

                            this.DevuelveConsumo(oConsumo.texto, ref CodigoOperador, ref NombreOperador, ref Litros, ref PrecioLitro);
                            oConsumo.comentarios = string.Format("OS-{0} ESTACION {1} {2}L", oConsumo.liquidacionID, oConsumo.estacion, Litros);
                            oBitacora.operadorID = Convert.ToInt32(CodigoOperador);
                            oBitacora.folioViaje = oConsumo.viaje;
                            oHashParam = new Hashtable();
                            oHashParam.Add("@fechaliquidacion", oConsumo.fechaLiquidacion);
                            oHashParam.Add("@fechacarga", oConsumo.fechaLiquidacion);
                            oHashParam.Add("@estacion", oConsumo.estacion);
                            oHashParam.Add("@codestacion", oConsumo.estacionclave);
                            oHashParam.Add("@corporativo", oConsumo.vehiculo);
                            oHashParam.Add("@economico", NoEconomico);
                            oHashParam.Add("@folio", oConsumo.liquidacionID);
                            oHashParam.Add("@litros", Litros);
                            oHashParam.Add("@preciolitro", PrecioLitro);
                            oHashParam.Add("@total", oConsumo.total); //----
                            oHashParam.Add("@folioviaje", oConsumo.viaje);
                            oHashParam.Add("@sucursal", oConsumo.sucursal);
                            oHashParam.Add("@centro", oConsumo.centro);
                            oHashParam.Add("@area", oConsumo.area);
                            oHashParam.Add("@depto", oConsumo.depto);
                            oHashParam.Add("@filial", oConsumo.filial);
                            oHashParam.Add("@comentarios", oConsumo.comentarios);
                            oHashParam.Add("@codigooperador", CodigoOperador);
                            oHashParam.Add("@nombreperador", NombreOperador);
                            iCommand = oConnectionOPE.ExecuteCommand(sConsultaSql, oHashParam);
                            if (iCommand == 0)
                                throw new Exception("No se ha podido insertar el desglose de consumo de combustible");
                        }

                        //Valida que la bitacora no exista para no duplicar nómina
                        sReponse = oLiquidacionAD.NuevaBitacoraLiquidacion(ref oConnectionLOG, oBitacora, out sConsultaSql);///Crea la bitacora
                        if (!sReponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                            throw new Exception("No se pudo crear el registro de bitacora en nómina");
                    }
                    else sReponse = "OK";
                }
                oConnectionLOG.CommitTransacction();
                oConnectionOPE.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnectionLOG.RollBackTransacction();
                oConnectionOPE.RollBackTransacction();
                oTool.LogError(ex, "InsertaMovimientoCombustible", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();

                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();
            }
            return sReponse;
        }


        public string InsertaMovimientoCombustiblePoliza(string sUsuarioID, List<LOGI_LiquidacionCombus_INFO> lstLiquidaciones, LOGI_LiquidacionBitacora_INFO oBitacora, string sUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty, SqueryAux;
            sReponse = "ERROR";
            int iCommand = 0; 
            try
            {
                oConnectionLOG.OpenConnection();
                oConnectionOPE.OpenConnection();
                oConnectionLOG.StarTransacction();
                oConnectionOPE.StarTransacction();

                foreach (LOGI_LiquidacionCombus_INFO oPoliza in lstLiquidaciones)
                {
                    sConsultaSql = string.Format(@"UPDATE lm_combus_cargas_tms SET estatus =2 , poliza = '{0}', foliod365 = '{1}',
                                                   usregistra = '{2}', fecharegistra = getdate()
                                                   WHERE folio_id = {3}", oBitacora.folioTMS, oBitacora.recID,sUsuario, oPoliza.bitacoraID);
                    iCommand =  oConnectionOPE.ExecuteCommand(sConsultaSql);
                    if (iCommand == 0)
                        throw new Exception("No se ha podido grabar las cargas afectadas en la poliza");
                }

                Hashtable oHashParam = new Hashtable();
                sConsultaSql = string.Format(@"INSERT INTO lm_tms_almacencombus(folio_tms,sesion_d365,ano,mes,cargas)
            VALUES(@folio_tms,@sesion_d365,@ano,@mes,@cargas)");
                oHashParam.Add("@folio_tms", oBitacora.folioTMS);
                oHashParam.Add("@sesion_d365", oBitacora.SessionID);
                oHashParam.Add("@ano", DateTime.Now.Year);
                oHashParam.Add("@mes", DateTime.Now.Month);
                oHashParam.Add("@cargas", lstLiquidaciones.Count);
                iCommand = oConnectionLOG.ExecuteCommand(sConsultaSql, oHashParam);

                if (iCommand == 0)
                    throw new Exception("No se ha podido grabar la bitacora de las cargas");

                oConnectionLOG.CommitTransacction();
                oConnectionOPE.CommitTransacction();
            }
            catch (Exception ex)
            {
                oConnectionLOG.RollBackTransacction();
                oConnectionOPE.RollBackTransacction();
                oTool.LogError(ex, "InsertaMovimientoCombustible", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();

                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();
            }
            return sReponse;
        }


        public string ListadodeCombustible(string sUsuarioID, ref List<LOGI_LiquidacionCombus_INFO> lstLiquidaciones, LOGI_LiquidacionCombus_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR"; 
            try
            {
                oConnectionOPE.OpenConnection();
                sReponse = oLiquidacionAD.ListaBitacoraCombustible(ref oConnectionOPE, ref lstLiquidaciones, oParam, out sConsultaSql);///Crea la bitacora

            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListadodeCombustible", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();
            }
            return sReponse;
        }

        public string ListadodeAlmacenes(string sUsuarioID, ref List<LOGI_Combustibles_INFO> lstConceptos)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionOPE.OpenConnection();
                sReponse = oLiquidacionAD.ListaConceptosCombustibles(ref oConnectionOPE, ref lstConceptos, out sConsultaSql);///Crea la bitacora

            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListadodeAlmacenes", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();
            }
            return sReponse;
        }

        public string ListaBitacora(string sUsuarioID, LOGI_LiquidacionBitacora_INFO oBitacora)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionOPE.OpenConnection();
                sReponse =  oLiquidacionAD.ListaBitacora(ref oConnectionLOG, oBitacora, out sConsultaSql);///Crea la bitacora

            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaBitacora", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();
            }
            return sReponse;
        }


        public string CreaPolizaContableD365(string sUsuarioID,  List<LOGI_LiquidacionCombus_INFO> lstSeleccion,  Decimal dPrecioCombus, ref List<LOGI_LiquidacionCombus_INFO> lstCargaD365, out string FolioPoliza)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            List<LOGI_LiquidacionCombus_INFO> lstLiquidaciones = new List<LOGI_LiquidacionCombus_INFO>();
            string cargasSeleccionada = string.Empty;
            LOGI_LiquidacionCombus_INFO oParam = new LOGI_LiquidacionCombus_INFO();
            FolioPoliza = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnectionOPE.OpenConnection();
                foreach (LOGI_LiquidacionCombus_INFO o in lstSeleccion)
                    cargasSeleccionada += string.Format("{0},", o.bitacoraID);
                cargasSeleccionada = cargasSeleccionada.TrimEnd(',');
                oParam.sFolios = cargasSeleccionada;
                oParam.estatus = 1;
                sReponse = oLiquidacionAD.ListaBitacoraCombustible(ref oConnectionOPE, ref lstLiquidaciones, oParam, out sConsultaSql);///Crea la bitacora
                if (lstSeleccion.Count != lstLiquidaciones.Count)
                    throw new Exception("La seleccion de cargas difiere al registro el bitacora");

                foreach (LOGI_LiquidacionCombus_INFO oCarga in lstLiquidaciones)
                {
                    oCarga.preciolitro = dPrecioCombus;
                    oCarga.total = oCarga.litros * dPrecioCombus;
                    lstCargaD365.Add(oCarga);
                }

                //Genera poliza para consumo de combustible / DIARIO
                Int64 total = 0;
                sConsultaSql = string.Format("SELECT COUNT(folio_tms) as total FROM lm_tms_almacencombus WHERE  mes = {0} AND ano = {1}", DateTime.Now.Month, DateTime.Now.Year);
                oConnectionLOG.OpenConnection();
                DataSet dsBitacora = oConnectionLOG.FillDataSet(sConsultaSql);
                if (dsBitacora.Tables[0].Rows.Count > 0)
                {
                    total = Convert.ToInt64(dsBitacora.Tables[0].Rows[0]["total"]);
                    total++;
                }
                FolioPoliza = string.Format("LM/COMBUS-{0}{1}", DateTime.Now.ToString("yyyyMMdd"), total);

                sReponse = "OK";
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListadodeCombustible", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnectionOPE != null)
                    oConnectionOPE.CloseConnection();

                if (oConnectionLOG != null)
                    oConnectionLOG.CloseConnection();
            }
            return sReponse;
        }


        void DevuelveConsumo(string sCadena, ref string CodigoOperador, ref string NombreOperador, ref Decimal Litros, ref Decimal PrecioLitro)
        {
            var CadenaOperador = sCadena.Split('|')[1];
            var DatoConsumo = CadenaOperador.Split('-');
            CodigoOperador = DatoConsumo[0];
            NombreOperador = DatoConsumo[1];
            Litros = Convert.ToDecimal(DatoConsumo[3].ToString().Replace("Litros:", string.Empty).Trim());
            PrecioLitro = Convert.ToDecimal(DatoConsumo[4].ToString().Replace("Precio por litro:", string.Empty).Trim());
        }

        List<LOGI_LiquidacionCombus_INFO> ListaConceptoscombustibles(List<LOGI_LiquidacionCombus_INFO> lstLiquidaciones, List<LOGI_Combustibles_INFO> lstConceptos)
        {
            List<LOGI_LiquidacionCombus_INFO> lstdepurados = new List<LOGI_LiquidacionCombus_INFO>();
            foreach (LOGI_LiquidacionCombus_INFO oLiquidacion in lstLiquidaciones)
            {
                var oConsumo = lstConceptos.FirstOrDefault(x => x.cuentatms.Equals(oLiquidacion.cuenta));
                if (oConsumo == null)
                    continue;
                oLiquidacion.estacion = oConsumo.Concepto;
                oLiquidacion.estacionclave = oConsumo.cuentatms;
                lstdepurados.Add(oLiquidacion);
            }
            return lstdepurados;
        }
    }
}
