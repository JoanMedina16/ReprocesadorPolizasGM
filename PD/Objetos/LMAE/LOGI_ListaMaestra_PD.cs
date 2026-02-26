using AD.Tablas.EQUIV;
using AD;
using INFO.Objetos.LMAE;
using INFO.Tablas.EQUIV;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AD.Objetos.LMAE;

namespace PD.Objetos.LMAE
{
    public class LOGI_ListaMaestra_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        internal LOGI_ListaMaestra_AD oListaM = null;
        const string CONST_CLASE = "LOGI_ListaMaestra_PD.cs";
        const string CONST_MODULO = "Listamaestra";

        public LOGI_ListaMaestra_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oListaM = new LOGI_ListaMaestra_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaUnidadesmaestra(string sUsuarioID, string sConnectionEquiv, LOGI_ListaMaestra_INFO oParam, ref List<LOGI_ListaMaestra_INFO> lstListamaestra)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty, responseLine = string.Empty;
            LOGI_Catalogos_INFO otemp = null;
            List<LOGI_Catalogos_INFO> lstCat = null;
            LOGI_ConexionSql_AD oConnEquiv = new LOGI_ConexionSql_AD(sConnectionEquiv);
            sReponse = "ERROR";
            try
            {

                oConnection.OpenConnection();
                oConnEquiv.OpenConnection();
                sReponse = oListaM.ListaUnidadesmaestra(ref oConnection, ref lstListamaestra, oParam, out sConsultaSql);


                #region Validaciones_por_grupo_de_sucursales
                responseLine = string.Empty;
                var lstSucursales = lstListamaestra.GroupBy(x => x.suc).ToList();
                foreach (var o in lstSucursales)
                {
                    if (o.Key == null)
                        continue;
                    if (string.IsNullOrEmpty(o.Key.ToString()))
                        continue;

                    otemp = new LOGI_Catalogos_INFO();
                    otemp.iCuenta = 67;
                    otemp.iSubcuenta = o.Key;
                    otemp.iEmpresa = 67;
                    otemp.iActivo = 1;
                    lstCat = new List<LOGI_Catalogos_INFO>();
                    new LOGI_Sucursales_AD().RecuperaSucursales(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                    if (lstCat.Count == 1)
                    {
                        lstListamaestra.Where(x => x.suc == o.Key).ToList().ForEach(x =>
                        {
                            x.sucursal = string.Format("{0}-{1}-{2}", o.Key, lstCat[0].sAX365, lstCat[0].sDescripcion);
                        });
                    }
                }
                #endregion Validaciones_por_grupo_de_sucursales

                #region Validaciones_por_grupo_de_centros_de_costo
                responseLine = string.Empty;
                var lstCentros = lstListamaestra.GroupBy(x => x.ccosto).ToList();
                foreach (var o in lstCentros)
                {
                    if (o.Key == null)
                        continue;
                    if (string.IsNullOrEmpty(o.Key.ToString()))
                        continue;

                    otemp = new LOGI_Catalogos_INFO();
                    otemp.iCuenta = 243;
                    otemp.iSubcuenta = o.Key;
                    otemp.iEmpresa = 67;
                    otemp.iActivo = 1;
                    lstCat = new List<LOGI_Catalogos_INFO>();
                    new LOGI_Centroscosto_AD().RecuperaCentroscosto(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                    //oTool.LogProceso("","RES: "+lstCat.Count, "", CONST_MODULO, "", sDatosAdicionales: sConsultaSql);
                    if (lstCat.Count == 1)
                    {
                        lstListamaestra.Where(x => x.ccosto == o.Key).ToList().ForEach(x =>
                        {
                            x.centrocosto = string.Format("{0}-{1}-{2}", o.Key, lstCat[0].sAX365, lstCat[0].sDescripcion);
                        });
                    }

                }
                #endregion Validaciones_por_grupo_de_centros_de_costo

                #region Validaciones_por_grupo_de_departamentos
                responseLine = string.Empty;
                var lstDeptos = lstListamaestra.GroupBy(x => x.depto).ToList();
                foreach (var o in lstDeptos)
                {
                    if (o.Key == null)
                        continue;

                    if (string.IsNullOrEmpty(o.Key.ToString()))
                        continue;

                    otemp = new LOGI_Catalogos_INFO();
                    otemp.iCuenta = 299;
                    otemp.iSubcuenta = o.Key;
                    otemp.iEmpresa = 67;
                    otemp.iActivo = 1;
                    lstCat = new List<LOGI_Catalogos_INFO>();
                    new LOGI_Departamentos_AD().RecuperaDepartamentos(ref oConnEquiv, ref lstCat, otemp, out sConsultaSql);
                    if (lstCat.Count == 1)
                    {
                        lstListamaestra.Where(x => x.depto == o.Key).ToList().ForEach(x =>
                        {
                            x.departamento = string.Format("{0}-{1}-{2}", o.Key, lstCat[0].sAX365, lstCat[0].sDescripcion);

                        });
                    }
                }
                #endregion Validaciones_por_grupo_de_departamentos

            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaUnidadesmaestra", sUsuarioID, CONST_CLASE, CONST_MODULO, sDatosAdicionales: sConsultaSql);
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


