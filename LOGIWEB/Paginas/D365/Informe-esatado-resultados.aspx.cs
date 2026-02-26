using INFO.Objetos.D365;
using LOGIWEB.Paginas.REPORTES.DATASETS;
using Microsoft.Reporting.WebForms;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using PD.Herramientas;
using PD.Objetos.D365;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Paginas.D365
{
    public partial class Informe_esatado_resultados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnInforme_Click(object sender, EventArgs e)
        {
            if (txtDesde.Text != "" && txtHasta.Text != "")
            {
                GeneraInforme();
            }
            else Creamensaje("Favor de seleccionar el período contable para generar el informe");

        }

        void Creamensaje(string sCadena)
        {
            try
            {
                string sCodeScript = "setTimeout(function(){  Onwarning('" + sCadena + "'); }, 900);";
                ScriptManager.RegisterStartupScript(Scrptmager, Scrptmager.GetType(), "script", sCodeScript, true);
            }
            catch { }
        }

        void GeneraInforme()
        {
            try
            {
                string sPath = string.Format(@"{0}\Paginas\REPORTES\RDLCS\EDR.rdlc", AppDomain.CurrentDomain.BaseDirectory);
                while (rptInforme.LocalReport.DataSources.Count > 0)
                    rptInforme.LocalReport.DataSources.RemoveAt(0);
                rptInforme.LocalReport.ReportPath = sPath;
                //rptInforme.Visible = false;
                ReportDataSource oParams = new ReportDataSource();
                oParams.Name = "ds_EDR";
                ds_EDR dsInforme = new ds_EDR();
                oParams.Value = dsInforme._ds_EDR;

                List<LOGI_Estadocuentas_INFO> lstEstados = new List<LOGI_Estadocuentas_INFO>();
                #region "Fletes"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Fletes",
                    cuenta_inicio = "DHB001001",
                    cuenta_fin = "DHB001098"

                });
                #endregion "Fletes"

                #region "Servicios Logísticos"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Servicios Logísticos",
                    cuenta_inicio = "DHA001001",
                    cuenta_fin = "DHA001002"

                });
                #endregion "Servicios Logísticos"

                #region "Otros"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Otros",
                    cuenta_inicio = "DJA001001",
                    cuenta_fin = "DJA001036"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Otros",
                    cuenta_inicio = "DJA001020",
                    cuenta_fin = "DJA001020",
                    negativo = true

                });
                #endregion "Otros"

                #region "Salarios"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios",
                    cuenta_inicio = "ECA121100",
                    cuenta_fin = "ECA122175",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios",
                    cuenta_inicio = "ECA124105",
                    cuenta_fin = "ECA126151",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios",
                    cuenta_inicio = "ECA126319",
                    cuenta_fin = "ECA126351",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios",
                    cuenta_inicio = "ECA126380",
                    cuenta_fin = "ECA126380",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios",
                    cuenta_inicio = "EKC002001",
                    cuenta_fin = "EKC002001",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios",
                    cuenta_inicio = "ECA123301",
                    cuenta_fin = "ECA123301",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios",
                    cuenta_inicio = "ECA126309",
                    cuenta_fin = "ECA126309",

                });
                #endregion "Salarios"

                #region "Prevision Social"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECA123308",
                    cuenta_fin = "ECA123308",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECA126310",
                    cuenta_fin = "ECA126313",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECA126361",
                    cuenta_fin = "ECA126361",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECA126382",
                    cuenta_fin = "ECA324003",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECA324005",
                    cuenta_fin = "ECB320246",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECC201001",
                    cuenta_fin = "ECC224001",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECC283001",
                    cuenta_fin = "ECC283001",

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Prevision Social",
                    cuenta_inicio = "ECC901001",
                    cuenta_fin = "ECC901003",

                });
                #endregion "Prevision Social"

                

                #region "Diversos admon"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos admon",
                    cuenta_inicio = "FAA324004",
                    cuenta_fin = "FAA324004",
                    area = "AD"


                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos admon",
                    cuenta_inicio = "FAC323002",
                    cuenta_fin = "FAC323005",
                    area = "AD"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos admon",
                    cuenta_inicio = "FAB331009",
                    cuenta_fin = "FAB331417",
                    area = "AD"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos admon",
                    cuenta_inicio = "FAB302009",
                    cuenta_fin = "FAB302009",
                    area = "AD"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos admon",
                    cuenta_inicio = "FAC237009",
                    cuenta_fin = "FAC245421",
                    area = "AD"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos admon",
                    cuenta_inicio = "FBA018001",
                    cuenta_fin = "FBA573001",
                    area = "AD"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos admon",
                    cuenta_inicio = "FAC323426",
                    cuenta_fin = "FAC327402",
                    area = "AD"

                });
                #endregion "Diversos admon"

                /*
                */

                #region "Depreciaciones"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Depreciaciones",
                    cuenta_inicio = "EMA108001",
                    cuenta_fin = "EMA108005",
                    DepFiscal = true

                });
                #endregion "Depreciaciones"

                #region "Combustible"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Combustible",
                    cuenta_inicio = "ELA226001",
                    cuenta_fin = "ELA226007"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Combustible",
                    cuenta_inicio = "DJA001020",
                    cuenta_fin = "DJA001020"

                });
                #endregion "Combustible"


                #region "Arrendamiento"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Arrendamiento",
                    cuenta_inicio = "ELA525004",
                    cuenta_fin = "ELA525004"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Arrendamiento",
                    cuenta_inicio = "ELA525008",
                    cuenta_fin = "ELA525008"

                });
                #endregion "Arrendamiento"


                #region "Mantenimiento Automotriz"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA018001",
                    cuenta_fin = "ELA018001"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA216001",
                    cuenta_fin = "ELA216001"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA328001",
                    cuenta_fin = "ELA328003"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA334899",
                    cuenta_fin = "ELA334899"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA466001",
                    cuenta_fin = "ELA466001"

                });
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA145001",
                    cuenta_fin = "ELA145001"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA185001",
                    cuenta_fin = "ELA185001"

                });
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA204006",
                    cuenta_fin = "ELA204006"

                });
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA216002",
                    cuenta_fin = "ELA216002"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA328004",
                    cuenta_fin = "ELA328004"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Mantenimiento Automotriz",
                    cuenta_inicio = "ELA185003",
                    cuenta_fin = "ELA185003"

                });
                #endregion "Mantenimiento Automotriz"

                #region "Siniestros"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Siniestros",
                    cuenta_inicio = "ELA328009",
                    cuenta_fin = "ELA328009"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Siniestros",
                    cuenta_inicio = "ELA328341",
                    cuenta_fin = "ELA328341"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Siniestros",
                    cuenta_inicio = "ELA332021",
                    cuenta_fin = "ELA332021"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Siniestros",
                    cuenta_inicio = "ELA328006",
                    cuenta_fin = "ELA328006"

                });
                #endregion "Siniestros"


                #region "Siniestros"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ECA324004",
                    cuenta_fin = "ECA324004"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ECB331247",
                    cuenta_fin = "ECB331405"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ECC237009",
                    cuenta_fin = "ECC245421"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA240001",
                    cuenta_fin = "ELA327447"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA328005",
                    cuenta_fin = "ELA328005"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA328010",
                    cuenta_fin = "ELA328015"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA330018",
                    cuenta_fin = "ELA332020"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA333341",
                    cuenta_fin = "ELA334423"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA334901",
                    cuenta_fin = "ELA420001"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA466002",
                    cuenta_fin = "ELA466003"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA500001",
                    cuenta_fin = "ELA525003"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA525005",
                    cuenta_fin = "ELA525006"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA203001",
                    cuenta_fin = "ELA204001"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA525128",
                    cuenta_fin = "ELA573001"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "FGA096001",
                    cuenta_fin = "FGA096001"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ECC318001",
                    cuenta_fin = "ECC323426"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA207081",
                    cuenta_fin = "ELA207081"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA332022",
                    cuenta_fin = "ELA332022"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diversos",
                    cuenta_inicio = "ELA332023",
                    cuenta_fin = "ELA332023"

                });
                #endregion "Siniestros" 

                #region "Peajes y Transbordos"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Peajes y Transbordos",
                    cuenta_inicio = "ELA213150",
                    cuenta_fin = "ELA213150"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Peajes y Transbordos",
                    cuenta_inicio = "ELA468001",
                    cuenta_fin = "ELA468001"

                });
                #endregion "Peajes y Transbordos" 

                #region "Productos Financieros"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Productos Financieros",
                    cuenta_inicio = "HAA001001",
                    cuenta_fin = "HAA001008"

                });
                #endregion "Productos Financieros"

                #region "Gastos financieros"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Gastos financieros",
                    cuenta_inicio = "HAB001001",
                    cuenta_fin = "HAB001012"

                });
                #endregion "Gastos financieros"

                #region "Diferencia cambiaria a favor"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diferencia cambiaria a favor",
                    cuenta_inicio = "HBA001001",
                    cuenta_fin = "HBA001003"

                });
                #endregion "Diferencia cambiaria a favor"

                #region "Diferiencia cambiaria cargo"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Diferiencia cambiaria cargo",
                    cuenta_inicio = "HBB002001",
                    cuenta_fin = "HBB002003"

                });
                #endregion "Diferiencia cambiaria cargo"


                #region "ISR"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "ISR",
                    cuenta_inicio = "JAA001001",
                    cuenta_fin = "JAA001001"

                });
                #endregion "ISR"

                #region "ISR Diferido"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "ISR Diferido",
                    cuenta_inicio = "JAA001002",
                    cuenta_fin = "JAA001002"

                });
                #endregion "ISR Diferido"
               
                #region "Salarios de adminsitracion"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios Admon",
                    cuenta_inicio = "FAA121100",
                    cuenta_fin = "FAA126151",
                    area = "AD"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios Admon",
                    cuenta_inicio = "FAA126319",
                    cuenta_fin = "FAA126351",
                    area = "AD"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios Admon",
                    cuenta_inicio = "FAA126380",
                    cuenta_fin = "FAA126380",
                    area = "AD"


                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Salarios Admon",
                    cuenta_inicio = "FAA126309",
                    cuenta_fin = "FAA126309",
                    area = "AD"

                });
                #endregion "Salarios de adminsitracion"

                
                #region "Gastos de Venta"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Gastos de Venta",
                    cuenta_inicio = "FAA121100",
                    cuenta_fin = "FGE999001",
                    departamento = "CO02"

                });

                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Gastos de Venta",
                    cuenta_inicio = "FGA096001",
                    cuenta_fin = "FGA096001",
                    departamento = "CO02",
                    negativo = true

                });
                #endregion "Gastos de Venta"

                
                #region "Depreciaciones gastos"
                lstEstados.Add(new LOGI_Estadocuentas_INFO
                {
                    descripcion = "Depreciaciones gastos",
                    cuenta_inicio = "FDA108001",
                    cuenta_fin = "FDC108001",
                    area = "AD",
                    DepFiscal = true


                });  
                #endregion "Depreciaciones gastos"
                

                /**/
                DataTable dtConcentrado = dsInforme._ds_EDR;
                LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
                LOGI_Extraccion_ZAP_INFO Periodo = new LOGI_Extraccion_ZAP_INFO();
                List<LOGI_Extraccion_ZAP_INFO> lstCuentas = new List<LOGI_Extraccion_ZAP_INFO>();
                List<LOGI_Extraccion_Vehiculo_INFO> lstVehiculos = new List<LOGI_Extraccion_Vehiculo_INFO>();
                string Vouchers = string.Empty, Descripciones = string.Empty;

                foreach (LOGI_Estadocuentas_INFO o in lstEstados)
                {
                    DataRow dtRow = dtConcentrado.NewRow();
                    dtRow["Informe"] = o.descripcion;
                    dtConcentrado.Rows.Add(dtRow);
                    dtConcentrado.AcceptChanges();
                    //busqueda de contenido
                    lstCuentas = new List<LOGI_Extraccion_ZAP_INFO>();
                    Periodo = new LOGI_Extraccion_ZAP_INFO();
                    Periodo.fecha_inicio = txtDesde.Text; //"01/10/2022";
                    Periodo.fecha_final = txtHasta.Text; //"30/10/2022";
                    Periodo.cuenta_inicio = o.cuenta_inicio;
                    Periodo.cuenta_fin = o.cuenta_fin;
                    Periodo.depto = o.departamento;
                    Periodo.depto_distinto = o.descarte_deptos;
                    Periodo.area = o.area;
                    Periodo.DepFiscal = o.DepFiscal;
                    new LOGI_Extraccion_ZAP_PD(oTool.CONST_ZAP_CONNECTION).Listacuentas(ref lstCuentas, Periodo);


                    var lstVouchers = lstCuentas.GroupBy(x => x.Voucher).ToList();
                    var lstDescrip = lstCuentas.GroupBy(x => x.text);
                    foreach (var vouc in lstVouchers)
                    {
                        if (!string.IsNullOrEmpty(vouc.Key))
                            Vouchers += string.Format("'{0}',", vouc.Key);
                    }

                    foreach (var desc in lstDescrip)
                    {
                        if (!string.IsNullOrEmpty(desc.Key))
                            Descripciones += string.Format("'{0}',", desc.Key);
                    }

                    Vouchers = Vouchers.TrimEnd(',');
                    Descripciones = Descripciones.TrimEnd(',');
                    lstVehiculos = new List<LOGI_Extraccion_Vehiculo_INFO>();
                    new LOGI_Extraccion_ZAP_PD(oTool.CONST_ZAP_CONNECTION).ListaDimensionVehiculo(ref lstVehiculos, Descripciones, Vouchers);

                    foreach (LOGI_Extraccion_ZAP_INFO data in lstCuentas)
                    {
                        var oVehiculo = lstVehiculos.Count == 0 ? null : lstVehiculos.FirstOrDefault(x => x.Voucher == data.Voucher && x.Descripcion == data.text);
                        dtRow = dtConcentrado.NewRow();
                        dtRow["Informe"] = o.descripcion;
                        dtRow["Fecha_registro"] = data.Fecha;
                        dtRow["Dimensiones"] = data.Display;
                        dtRow["Cuenta_contable"] = data.cuenta;
                        dtRow["Sucursal"] = data.sucursal;
                        dtRow["Filial"] = data.filial;
                        dtRow["Centro_de_costo"] = data.centro;
                        dtRow["Departamento"] = data.depto;
                        dtRow["Descripcion"] = data.text;
                        dtRow["Vehiculo"] = oVehiculo == null ? "NO REGISTRADO" : oVehiculo.Vehiculo;
                        dtRow["Total"] = o.negativo ? -Convert.ToDecimal(data.debito) : Convert.ToDecimal(data.debito);
                        dtConcentrado.Rows.Add(dtRow);
                        dtConcentrado.AcceptChanges();
                    }
                }
                rptInforme.LocalReport.DataSources.Add(oParams);
                rptInforme.Visible = true;
            }
            catch (Exception ex)
            {
                Creamensaje(String.Format("Informe no generado. ERROR {0}", ex.Message));
            }
        }
    }

    
}