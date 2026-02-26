using INFO.Objetos;
using INFO.Tablas;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using PD.Herramientas;
using PD.Tablas.D365;
using PD.Tablas.EQUIV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using D365API;

namespace LOGIWEB.Metodos.D365
{
    public partial class Configuracion_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaConfiguracion()
        {
            LOGI_ConfiguracionD365_INFO oConfiguracion = new LOGI_ConfiguracionD365_INFO();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_ConfiguracionD365_PD(oTool.CONST_CONNECTION).ListaConfiguracion(oUser.iUsuario.ToString(), ref oConfiguracion);
                    oResponse.data = oConfiguracion;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaPeriodoActual()
        {
            LOGI_ConfiguracionOPE_INFO oConfiguracion = new LOGI_ConfiguracionOPE_INFO();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_ConfiguracionOPE_PD(oTool.CONST_CONNECTION).ListaConfiguracion(oUser.iUsuario.ToString(), ref oConfiguracion);
                    if(oResponse.estatus == "OK")
                    {
                        oResponse.mensaje = String.Format("El perído actual corresponde al mes {0} y año {1}", oConfiguracion.mescont, oConfiguracion.anocont);
                    }
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO CierraPeriodo(LOGI_ConfiguracionOPE_INFO Periodo)
        {
            LOGI_ConfiguracionOPE_INFO oConfiguracion = new LOGI_ConfiguracionOPE_INFO();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = "ERROR";
                    new LOGI_ConfiguracionOPE_PD(oTool.CONST_CONNECTION).ListaConfiguracion(oUser.iUsuario.ToString(), ref oConfiguracion);
                    DateTime PeriodoActual = Convert.ToDateTime(String.Format("01/{0}/{1}", oConfiguracion.mescont, oConfiguracion.anocont));
                    DateTime PeriodoAbrir = Convert.ToDateTime(Periodo.fecha);
                    DateTime CierreSistema = PeriodoActual.AddMonths(1);
                    if (CierreSistema.Month == PeriodoAbrir.Month && CierreSistema.Year == PeriodoAbrir.Year)
                    {
                        Periodo.anocont = PeriodoAbrir.Year;
                        Periodo.mescont = PeriodoAbrir.Month;
                       oResponse.estatus =  new LOGI_ConfiguracionOPE_PD(oTool.CONST_CONNECTION).ActualizaConfiguracion(oUser.iUsuario.ToString(), Periodo);

                    }
                    else oResponse.mensaje = String.Format("El cirrer del periodo no es el correcto. Este debe ser subsecuente al período que se ecuentra abierto actualmente");
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        
        public static LOGI_Response_INFO ActualizaConfiguracion(LOGI_ConfiguracionD365_INFO oConfiguracion)
        {             
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_ConfiguracionD365_PD(oTool.CONST_CONNECTION).ActualizaConfiguracion(oUser.iUsuario.ToString(), oConfiguracion);                    
                    oResponse.mensaje = oResponse.estatus;

                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO DisponibilidaSaldoCliente(LOGI_ConfiguracionD365_INFO oConfiguracion)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_ConfiguracionD365_PD(oTool.CONST_CONNECTION).ActualizaDisponibilid(oUser.iUsuario.ToString(), oConfiguracion);
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaDocumentos(LOGI_Documentos_INFO Documento)
        {
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    if (oUser.iPerfil == 1)
                        oResponse.estatus = new LOGI_Documentos_PD(oTool.CONST_CONNECTION).ListaDocumentos(oUser.iUsuario.ToString(), ref lstDocumentos, Documento);
                    else
                        oResponse.estatus = new LOGI_Documentos_PD(oTool.CONST_CONNECTION).ListaDocumentosXUsuario(oUser.iUsuario.ToString(), ref lstDocumentos, "", oUser.iUsuario);
                    oResponse.data = lstDocumentos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaDocumentosProcesa(LOGI_DocumentosProcesa_INFO Documento)
        {
            List<LOGI_DocumentosProcesa_INFO> lstDocumentos = new List<LOGI_DocumentosProcesa_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    if (oUser.iPerfil == 1)
                        oResponse.estatus = new LOGI_Documentos_PD(oTool.CONST_CONNLOG).ListaDocumentosProcesa(oUser.iUsuario.ToString(), ref lstDocumentos, Documento);
                    else
                        oResponse.estatus = new LOGI_Documentos_PD(oTool.CONST_CONNLOG).ListaDocumentosProcesaXUsuario(oUser.iUsuario.ToString(), ref lstDocumentos, "", oUser.iUsuario);
                    oResponse.data = lstDocumentos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaDocumentosUsuario(string Usuario)
        {
            List<LOGI_Documentos_INFO> lstDocumentos = new List<LOGI_Documentos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Documentos_PD(oTool.CONST_CONNECTION).ListaDocumentosXUsuario(oUser.iUsuario.ToString(), ref lstDocumentos, Usuario, -1);
                    oResponse.data = lstDocumentos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Creadocumento(LOGI_Documentos_INFO Documento)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Documentos_PD(oTool.CONST_CONNECTION).Creadocumento(oUser.iUsuario.ToString(), Documento);                    
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ActualizaDocumento(LOGI_Documentos_INFO Documento)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Documentos_PD(oTool.CONST_CONNECTION).Actualizadocumento(oUser.iUsuario.ToString(), Documento);
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listsucursalesactivas()
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    List<LOGI_Catalogos_INFO> lstSucursales = new List<LOGI_Catalogos_INFO>();
                    LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                    oParam.iCuenta = 67;
                    oParam.iActivo = 1;
                    oParam.iEmpresa = 67;
                    oParam.iSubcuenta = -1;
                    oResponse.estatus = new LOGI_Sucursales_PD(oTool.CONST_EQUIV_CONNECTION).ListaSucursales(ref lstSucursales, oParam);
                    oResponse.data = lstSucursales;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listadepartamentosactivos()
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    List<LOGI_Catalogos_INFO> lstlDepartamentos = new List<LOGI_Catalogos_INFO>();
                    LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                    oParam.iCuenta = 299;
                    oParam.iActivo = 1;
                    oParam.iEmpresa =67;
                    oParam.iSubcuenta = -1;
                    oResponse.estatus = new LOGI_Departamentos_PD(oTool.CONST_EQUIV_CONNECTION).ListaDepartamentos(ref lstlDepartamentos, oParam);
                    oResponse.data = lstlDepartamentos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listacentroscostoactivos()
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    List<LOGI_Catalogos_INFO> lstCentros = new List<LOGI_Catalogos_INFO>();
                    LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                    oParam.iCuenta = 243;
                    oParam.iActivo = 1;
                    oParam.iEmpresa = 67;
                    oParam.iSubcuenta = -1;
                    oResponse.estatus = new LOGI_Centroscosto_PD(oTool.CONST_EQUIV_CONNECTION).ListaCentroscosto(ref lstCentros, oParam);
                    oResponse.data = lstCentros;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        #endregion "WEBMETHODS"
    }
}