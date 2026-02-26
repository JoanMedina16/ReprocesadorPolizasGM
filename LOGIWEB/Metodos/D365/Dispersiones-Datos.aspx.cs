using INFO.Objetos;
using INFO.Objetos.D365;
using INFO.Tablas;
using INFO.Tablas.CAT;
using INFO.Tablas.EQUIV;
using PD.Herramientas;
using PD.Objetos.D365;
using PD.Tablas.EQUIV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.D365
{
    public partial class Dispersiones_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listadispersiones(LOGI_Dispersion_INFO oDispersion)
        {
            List<LOGI_Dispersion_INFO> lstDispersiones = new List<LOGI_Dispersion_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Dispersiones_PD(oTool.CONST_CONNECTION).Listadispersiones(oUser.iUsuario.ToString(), oTool.CONST_EQUIV_CONNECTION, oDispersion, ref lstDispersiones);
                    oResponse.data = lstDispersiones;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaBancos()
        {
            List<LOGI_Catalogos_INFO> lstBancos = new List<LOGI_Catalogos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                    oParam.iActivo = 1;
                    oParam.iEmpresa = oTool.CONST_EMPRESA;
                    oResponse.estatus = new LOGI_Bancos_PD(oTool.CONST_EQUIV_CONNECTION).ListaBancos(ref lstBancos, oParam);
                    oResponse.data = lstBancos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaFondofijo()
        {
            List<LOGI_Catalogos_INFO> lstFondofijo = new List<LOGI_Catalogos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    LOGI_Catalogos_INFO oParam = new LOGI_Catalogos_INFO();
                    oParam.iActivo = 1;
                    oParam.iEmpresa = oTool.CONST_EMPRESA;
                    oResponse.estatus = new LOGI_Fondofijo_PD(oTool.CONST_EQUIV_CONNECTION).ListaFondofijo(ref lstFondofijo, oParam);
                    oResponse.data = lstFondofijo;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listsucursales()
        {
            List<LOGI_Sucursales_INFO> lstSucursales = new List<LOGI_Sucursales_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new PD.Tablas.CAT.LOGI_Sucursales_PD(oTool.CONST_CONNECTION).Listasucursales("", ref lstSucursales);
                    oResponse.data = lstSucursales;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO CreaPlantilla(string Asistente, int tipo, List<LOGI_Dispersion_INFO> lstPolizas)
        {
            string FolioAsistente = string.Empty;
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Dispersiones_PD(oTool.CONST_CONNECTION).CreaAsistente(oUser.iUsuario.ToString(), Asistente, tipo, lstPolizas, ref FolioAsistente);
                    oResponse.data = FolioAsistente;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }
        

        #endregion "WEBMETHODS"
    }
}