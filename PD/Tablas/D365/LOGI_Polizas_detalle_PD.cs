using AD.Tablas.EQUIV;
using AD;
using INFO.Enums;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AD.Tablas.D365;
using PD.Herramientas;

namespace PD.Tablas.D365
{
    public class LOGI_Polizas_detalle_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_Polizas_detalle_AD oPolizadetalle = null;
        const string CONST_CLASE = "LOGI_Polizas_detalle_PD.cs";
        const string CONST_MODULO = "Polizas detalle D365";
        const int CONST_EMPRESA = 67;///Siempre corresponde a 67 - Logística del Mayab

        public LOGI_Polizas_detalle_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oPolizadetalle = new LOGI_Polizas_detalle_AD();
            oTool = new LOGI_Tools_PD();
        }
        public string ListadetallePoliza(string sConnectionEquiv, string sUsuarioID, LOGI_Polizas_detalle_INFO oParam, ref List<LOGI_Polizas_detalle_INFO> lstDetalle)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty, sNumeroIVA = string.Empty;
            sReponse = "ERROR";
            LOGI_ConexionSql_AD oConnEquiv = new LOGI_ConexionSql_AD(sConnectionEquiv);
            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            try
            {

                oConnEquiv.OpenConnection();
                oConnection.OpenConnection();
                eDocumentoPolizas eDocType = (eDocumentoPolizas)oParam.tipo_documento;
                string CONST_TABLE = oTool.GetEnumDefaultValue((eDocumentoPolizas)oParam.tipo_documento);
                sReponse = oPolizadetalle.ListaDetallePoliza(CONST_TABLE, ref oConnection, ref lstDetalle, oParam, out sConsultaSql);
                #region "RECUPERAMOS LAS EQUIVALENCIAS UBICADAS EN LAS EQUIVALENCIAS (BIL)"
                int cuenta = -1, scuenta = -1;
                LOGI_Catalogos_INFO otemp = null;
                LOGI_Catalogos_INFO oFilial = null;
                List<LOGI_Catalogos_INFO> lstCat = new List<LOGI_Catalogos_INFO>();
                List<LOGI_Catalogos_INFO> lstCatImpue = new List<LOGI_Catalogos_INFO>();
                string responseLine = string.Empty;
                otemp = new LOGI_Catalogos_INFO();
                otemp.iCuenta = 624;
                otemp.iSubcuenta = 67;
                otemp.iEmpresa = CONST_EMPRESA;
                new LOGI_Filialesterceros_AD().RecuperaFilieales(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                oFilial = new LOGI_Catalogos_INFO();
                if (lstCat.Count == 1)
                    oFilial.sAX365 = lstCat[0].sAX365;

                sReponse =  new LOGI_ConfiguracionD365_AD().ListaConfiguracion(ref oConnection, ref oConfiguracion, out sConsultaSql);
                if (sReponse != "OK")
                    throw new Exception("No se pudo validar la configuración de las cuentas");

                #region Validaciones_por_grupo_de_cuentas_contables
                responseLine = string.Empty;
                var lstCuentas = lstDetalle.GroupBy(x => new { x.mayor, x.cuenta, x.scuenta, x.refdoc }).ToList();
                foreach (var o in lstCuentas)
                {
                    if (o.Key == null)
                        continue;

                    responseLine = string.Empty;
                    //o.valido = 1;
                    ///--BUSQUEDA DE CUENTA CONTABLE 
                    //o.cuenta_AX = "NO EXISTE";
                    if (o.Key.mayor > 0 && o.Key.scuenta >= 0 && o.Key.cuenta >= 0)
                    {
                        otemp = new LOGI_Catalogos_INFO();
                        otemp.iCuentamayor = o.Key.mayor;
                        otemp.iCuenta = o.Key.cuenta;
                        otemp.iSubcuenta = o.Key.scuenta;
                        otemp.iEmpresa = CONST_EMPRESA;
                        var oKeyLinea = lstDetalle.FirstOrDefault(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta);
                        if (!this.CuentaOPE(oKeyLinea.area, ref cuenta, ref scuenta))
                            otemp.iArea = -1;
                        else
                            otemp.iArea = scuenta;
                        otemp.iEmpresa = CONST_EMPRESA;
                        otemp.iActivo = 1;
                        lstCat = new List<LOGI_Catalogos_INFO>();
                        lstCatImpue = new List<LOGI_Catalogos_INFO>();
                        if (o.Key.cuenta == 10 || o.Key.cuenta == 50)
                            //CUANDO LA CUENTA CORRESPONDE A 10 O 50 RECAE SOBRE CLIENTES
                            new AD.Tablas.EQUIV.LOGI_Clientes_AD().RecuperaClientes(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                        else
                        {
                            if (o.Key.mayor == 1540)//se busca en IVAS (cat_impuestos)
                            {
                                #region "Busqueda y equivalencias de impuestos"
                                //Cuando el tipo de documento es de viaticos se condiciona ya que se pueden manejar cuatro tipos de impuestos
                                if (eDocType == eDocumentoPolizas.COMPROBACIÓN_DE_VIATICOS)
                                {
                                    oKeyLinea = lstDetalle.FirstOrDefault(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta && x.refdoc == o.Key.refdoc);

                                    //el impuesto se busca según los criterios de iva, frontera y deducción
                                    if (oKeyLinea.esdeducible == 1 && oKeyLinea.usaiva == 1 && oKeyLinea.esfrontera == 0)//EL IMPUESTO ES IVA 16%
                                        sNumeroIVA = string.Format("{0}1", otemp.iCuenta);
                                    else if (oKeyLinea.esdeducible == 1 && oKeyLinea.usaiva == 0)//EL IMPUESTO ES IVA EXCENTO
                                        sNumeroIVA = string.Format("{0}2", otemp.iCuenta);
                                    else if (oKeyLinea.esdeducible == 1 && oKeyLinea.usaiva == 1 && oKeyLinea.esfrontera == 1)//EL IMPUESTO ES IVA FRONTERA //11% U 8% SEGÚN oa_config
                                        sNumeroIVA = string.Format("{0}3", otemp.iCuenta);
                                    else if (oKeyLinea.esdeducible == 0)//ES UN IMPUESTO NO DEDUCIBLE
                                        sNumeroIVA = string.Format("{0}3", otemp.iCuenta);
                                    otemp.iSubcuenta = Convert.ToInt32(sNumeroIVA);

                                    new LOGI_Impuestos_AD().RecuperaImpuestos(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                                    lstDetalle.Where(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta && x.refdoc.Equals(oKeyLinea.refdoc, StringComparison.InvariantCultureIgnoreCase)).ToList().ForEach(x =>
                                    {
                                        x.valido = 1;
                                        x.GrupoImpuesto = lstCat[0].sAxgrupoventa;
                                        x.ImpuestoArt = lstCat[0].sAxgrupoarticulo;
                                        x.cuenta_AX = lstCat[0].sAX365;
                                    });
                                }else new LOGI_Impuestos_AD().RecuperaImpuestos(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);                                
                                #endregion "Busqueda y equivalencias de impuestos"
                            }
                            else if (o.Key.mayor == 3010)//Se busca en proveedores (cat_proveedores)
                                new AD.Tablas.EQUIV.LOGI_Proveedores_AD().RecuperaProveedores(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                            else if(o.Key.mayor == 1330) ///Se asigana la cuenta de contrapartida para operadores                           
                            {
                                //asiganmos la cuenta de contrapartida fija ubicada en cuenta contable para viaticos "ACA002003" - configurada en lm_config_d365
                                lstCat = new List<LOGI_Catalogos_INFO>();
                                lstCat.Add(new LOGI_Catalogos_INFO
                                {
                                    sAX365 = oConfiguracion.cuentaviatico
                                });
                            }
                            else ///Todo lo demás se busca en catalogo de cuentas (cat_cuenta)
                                new LOGI_Cuentas_AD().RecuperaCuentas(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                        }
                        var oEXCENTA = lstDetalle.FirstOrDefault(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta && x.descrip.Contains("Parte excenta"));
                        if (oEXCENTA != null)
                        {
                            lstCatImpue = new List<LOGI_Catalogos_INFO>();
                            otemp = new LOGI_Catalogos_INFO();
                            otemp.iEmpresa = CONST_EMPRESA;
                            otemp.iActivo = 1;
                            otemp.sAX365MATCH = "NOAFECTOA";
                            new LOGI_Impuestos_AD().RecuperaImpuestos(ref oConnEquiv, ref lstCatImpue, otemp, out sConsultaSql);
                            lstDetalle.Where(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta && x.descrip.Contains("Parte excenta")).ToList().ForEach(x =>
                            {
                                x.valido = 1;
                                x.GrupoImpuesto = lstCatImpue[0].sAxgrupoventa;
                                x.ImpuestoArt = lstCatImpue[0].sAxgrupoarticulo;
                                x.cuenta_AX = lstCatImpue[0].sAX365;
                            });

                        }
                        var oRENCCION = lstDetalle.FirstOrDefault(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta && x.descrip.Contains("Retención"));
                        if (oRENCCION != null)
                        {
                            lstCatImpue = new List<LOGI_Catalogos_INFO>();
                            otemp = new LOGI_Catalogos_INFO();
                            otemp.iEmpresa = CONST_EMPRESA;
                            otemp.iActivo = 1;
                            otemp.sAX365MATCH = "CLFLETES";
                            new LOGI_Impuestos_AD().RecuperaImpuestos(ref oConnEquiv, ref lstCatImpue, otemp, out sConsultaSql);
                            lstDetalle.Where(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta && x.descrip.Contains("Retención")).ToList().ForEach(x =>
                            {
                                x.valido = 1;
                                x.GrupoImpuesto = lstCatImpue[0].sAxgrupoventa;
                                x.ImpuestoArt = lstCatImpue[0].sAxgrupoarticulo;
                                x.cuenta_AX = lstCatImpue[0].sAX365;
                            });
                        }

                        if (lstCat.Count == 1)
                        {
                            lstDetalle.Where(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta && !x.descrip.Contains("Retención") && !x.descrip.Contains("Parte excenta")).ToList().ForEach(x =>
                            {
                                x.valido = 1;
                                x.cuenta_AX = lstCat[0].sAX365;
                            });

                        }
                        else if (lstCat.Count > 1)
                            responseLine = string.Format("{0} Más de una cuenta contable", responseLine);
                        else responseLine = string.Format("{0} Cuenta contable", responseLine);

                        if (responseLine.Trim().Length > 2)
                        {
                            lstDetalle.Where(x => x.mayor == o.Key.mayor && x.cuenta == o.Key.cuenta && x.scuenta == o.Key.scuenta).ToList().ForEach(x =>
                            {
                                x.cuenta_AX = "NO EXISTE";
                                x.mensaje = string.Format("Error en equivalencias: {0}", responseLine.Trim());
                            });
                        }

                    }
                }
                #endregion Validaciones_por_grupo_de_cuentas_contables

                #region Validaciones_por_grupo_de_sucursales
                responseLine = string.Empty;
                var lstSucursales = lstDetalle.GroupBy(x => x.sucursal).ToList();
                foreach (var o in lstSucursales)
                {
                    if (string.IsNullOrEmpty(o.Key))
                        continue;
                    if (this.CuentaOPE(o.Key.ToString(), ref cuenta, ref scuenta))
                    {
                        otemp = new LOGI_Catalogos_INFO();
                        otemp.iCuenta = cuenta;
                        otemp.iSubcuenta = scuenta;
                        otemp.iEmpresa = CONST_EMPRESA;
                        otemp.iActivo = 1;
                        lstCat = new List<LOGI_Catalogos_INFO>();
                        new LOGI_Sucursales_AD().RecuperaSucursales(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                        if (lstCat.Count == 1)
                        {
                            lstDetalle.Where(x => x.sucursal == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.valido = 1;
                                x.sucursal_D365 = lstCat[0].sAX365;
                            });
                        }
                        else if (lstCat.Count > 1)
                            responseLine = string.Format("{0} Más de una sucursal", responseLine);
                        else responseLine = string.Format("{0} Sucursales", responseLine);

                        if (responseLine.Trim().Length > 2)
                        {
                            lstDetalle.Where(x => x.sucursal == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.sucursal_D365 = "NO EXISTE";
                                x.mensaje = string.Format("Error en equivalencias: {0}", responseLine.Trim());
                            });
                        }
                    }
                }
                #endregion Validaciones_por_grupo_de_sucursales

                #region Validaciones_por_grupo_de_centros_de_costo
                responseLine = string.Empty;
                var lstCentros = lstDetalle.GroupBy(x => x.centrocosto).ToList();
                foreach (var o in lstCentros)
                {
                    if (string.IsNullOrEmpty(o.Key))
                        continue;

                    if (this.CuentaOPE(o.Key.ToString(), ref cuenta, ref scuenta))
                    {
                        otemp = new LOGI_Catalogos_INFO();
                        otemp.iCuenta = cuenta;
                        otemp.iSubcuenta = scuenta;
                        otemp.iEmpresa = CONST_EMPRESA;
                        otemp.iActivo = 1;
                        lstCat = new List<LOGI_Catalogos_INFO>();
                        new LOGI_Centroscosto_AD().RecuperaCentroscosto(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                        //oTool.LogProceso("","RES: "+lstCat.Count, "", CONST_MODULO, "", sDatosAdicionales: sConsultaSql);
                        if (lstCat.Count == 1)
                        {
                            lstDetalle.Where(x => x.centrocosto == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.valido = 1;
                                x.centrocosto_D365 = lstCat[0].sAX365;
                            });
                        }
                        else if (lstCat.Count > 1)
                            responseLine = string.Format("{0} Más de un centro de costo", responseLine);
                        else responseLine = string.Format("{0} Centro de costo", responseLine);

                        if (responseLine.Trim().Length > 2)
                        {
                            lstDetalle.Where(x => x.centrocosto == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.centrocosto_D365 = "NO EXISTE";
                                x.mensaje = string.Format("Error en equivalencias: {0}", responseLine.Trim());
                            });
                        }
                    }
                }
                #endregion Validaciones_por_grupo_de_centros_de_costo

                #region Validaciones_por_grupo_de_departamentos
                responseLine = string.Empty;
                var lstDeptos = lstDetalle.GroupBy(x => x.departamento).ToList();
                foreach (var o in lstDeptos)
                {
                    if (string.IsNullOrEmpty(o.Key))
                        continue;

                    if (this.CuentaOPE(o.Key.ToString(), ref cuenta, ref scuenta))
                    {
                        otemp = new LOGI_Catalogos_INFO();
                        otemp.iCuenta = cuenta;
                        otemp.iSubcuenta = scuenta;
                        otemp.iEmpresa = CONST_EMPRESA;
                        otemp.iActivo = 1;
                        lstCat = new List<LOGI_Catalogos_INFO>();
                        new LOGI_Departamentos_AD().RecuperaDepartamentos(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                        if (lstCat.Count == 1)
                        {
                            lstDetalle.Where(x => x.departamento == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.valido = 1;
                                x.departamento_D365 = lstCat[0].sAX365;

                            });
                        }
                        else if (lstCat.Count > 1)
                            responseLine = string.Format("{0} Más de un centro de costo", responseLine);
                        else responseLine = string.Format("{0} Departamento", responseLine);

                        if (responseLine.Trim().Length > 2)
                        {
                            lstDetalle.Where(x => x.departamento == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.departamento_D365 = "NO EXISTE";
                                x.mensaje = string.Format("Error en equivalencias: {0}", responseLine.Trim());
                            });
                        }
                    }
                }
                #endregion Validaciones_por_grupo_de_departamentos

                #region Validaciones_por_grupo_de_areas 
                responseLine = string.Empty;
                var lstAreas = lstDetalle.GroupBy(x => x.area).ToList();
                foreach (var o in lstAreas)
                {
                    if (string.IsNullOrEmpty(o.Key))
                        continue;

                    if (this.CuentaOPE(o.Key.ToString(), ref cuenta, ref scuenta))
                    {
                        otemp = new LOGI_Catalogos_INFO();
                        otemp.iCuenta = cuenta;
                        otemp.iSubcuenta = scuenta;
                        otemp.iEmpresa = CONST_EMPRESA;
                        otemp.iActivo = 1;
                        lstCat = new List<LOGI_Catalogos_INFO>();
                        new LOGI_Areas_AD().RecuperaAreas(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                        if (lstCat.Count == 1)
                        {
                            lstDetalle.Where(x => x.area == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.valido = 1;
                                x.area_D365 = lstCat[0].sAX365;
                            });
                        }
                        else if (lstCat.Count > 1)
                            responseLine = string.Format("{0} Más de un área", responseLine);
                        else responseLine = string.Format("{0} Área", responseLine);

                        if (responseLine.Trim().Length > 2)
                        {
                            lstDetalle.Where(x => x.area == o.Key.ToString()).ToList().ForEach(x =>
                            {
                                x.area_D365 = "NO EXISTE";
                                x.mensaje = string.Format("Error en equivalencias: {0}", responseLine.Trim());
                            });
                        }
                    }
                }
                #endregion Validaciones_por_grupo_de_areas 

                lstDetalle.Where(x => x.mensaje != null && x.mensaje.Trim().Length > 3).ToList().ForEach(x =>
                {
                    x.valido = 0;
                });

                lstDetalle.ForEach(x =>
                {

                    x.filialtercero_D365 = string.IsNullOrEmpty(oFilial.sAX365) ? "NO EXISTE" : oFilial.sAX365;
                });
                #endregion "RECUPERAMOS LAS EQUIVALENCIAS UBICADAS EN LAS EQUIVALENCIAS (BIL)"
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListadetallePoliza", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
                oConnEquiv.CloseConnection();
            }
            return sReponse;
        }

        public string ActualizaLineaDetalle(string sUsuarioID, LOGI_Polizas_detalle_INFO oParam)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";

            try
            {
                oConnection.OpenConnection();
                string CONST_TABLE = oTool.GetEnumDefaultValue((eDocumentoPolizas)oParam.tipo_documento);
                sReponse = oPolizadetalle.ActualizaLineapoliza(CONST_TABLE, ref oConnection, sUsuarioID, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ActualizaDetalle", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string RecuperaXMLLinea(string sUsuarioID, LOGI_Polizas_detalle_INFO oParam, ref LOGI_Polizas_detalle_INFO oLinea)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";

            try
            {
                oConnection.OpenConnection();
                string CONST_TABLE = oTool.GetEnumDefaultValue((eDocumentoPolizas)oParam.tipo_documento);
                sReponse = oPolizadetalle.RecuperaXMLLinea(CONST_TABLE, ref oConnection,ref oLinea, oParam, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "RecuperaXMLLinea", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        bool CuentaOPE(string dimension, ref int cuenta, ref int scuenta)
        {
            bool bContinua = false;
            cuenta = 0;
            scuenta = 0;
            try
            {
                if (!string.IsNullOrEmpty(dimension))
                {
                    var spl = dimension.Split('|');
                    cuenta = Convert.ToInt32(spl[0]);
                    scuenta = Convert.ToInt32(spl[1]);
                    if (cuenta > 0 && scuenta >= 0)
                        bContinua = true;
                }
            }
            catch { }

            return bContinua;
        }
    }
}
