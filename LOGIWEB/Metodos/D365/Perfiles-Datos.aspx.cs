using INFO.Objetos;
using INFO.Tablas;
using INFO.Tablas.CAT;
using PD.Herramientas;
using PD.Tablas.CAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.D365
{
    public partial class Perfiles_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Siempre que se accede a este archivo por url se redirige al login
            Response.Redirect("../../login.aspx");
        }

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listaperfiles(LOGI_Roles_INFO perfil)
        {
            List<LOGI_Roles_INFO> lstPerfiles = new List<LOGI_Roles_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    perfil.bIgnorarol = true;
                    oResponse.estatus = new LOGI_Roles_PD(oTool.CONST_CONNECTION).ListaRoles(ref lstPerfiles, perfil);
                    oResponse.data = lstPerfiles;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listapermisos()
        {
            List<LOGI_Permisos_INFO> lstPermisos = new List<LOGI_Permisos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Permisos_PD(oTool.CONST_CONNECTION).ListaPermisos(ref lstPermisos);
                    oResponse.data = lstPermisos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listaperfilpermisos(int perfilID)
        {
            List<LOGI_Permisos_INFO> lstPermisos = new List<LOGI_Permisos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Permisos_PD(oTool.CONST_CONNECTION).ListaPermisosByPerfil(ref lstPermisos, perfilID);
                    oResponse.data = lstPermisos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listamatrizperfil(int perfilID)
        {
            List<LOGI_Matrizrol_INFO> lstMatriz = new List<LOGI_Matrizrol_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Permisos_PD(oTool.CONST_CONNECTION).ListaMatrizByperfil(ref lstMatriz, perfilID);
                    oResponse.data = lstMatriz;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Nuevoperfil(LOGI_Roles_INFO perfil)
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
                    perfil.usuarioID = oUser.iUsuario;
                    oResponse.estatus = new LOGI_Roles_PD(oTool.CONST_CONNECTION).NuevoRol(perfil);
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Editaperfil(LOGI_Roles_INFO perfil)
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
                    oResponse.estatus = new LOGI_Roles_PD(oTool.CONST_CONNECTION).EditaRol(perfil);
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        #endregion "WEBMETHODS"

    }
}