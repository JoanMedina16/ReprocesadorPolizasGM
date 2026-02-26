using INFO.Objetos;
using INFO.Tablas;
using INFO.Tablas.CAT;
using PD.Herramientas;
using PD.Tablas;
using PD.Tablas.CAT;
using PD.Tablas.D365;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos
{
    public partial class Usuarios_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Siempre que se accede a este archivo por url se redirige al login
            Response.Redirect("../login.aspx");
        }
        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaUsuarios(LOGI_Usuarios_INFO usuario)
        {
            List<LOGI_Usuarios_INFO> lstUsuarios = new List<LOGI_Usuarios_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    if (usuario.iUsuario == -1)
                        usuario.sUsuarioExcep = oUser.iUsuario;
                    oResponse.estatus = new LOGI_Usuarios_PD(oTool.CONST_CONNECTION).ListaUsuarios(ref lstUsuarios, usuario);
                    oResponse.data = lstUsuarios;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Nuevousuario(LOGI_Usuarios_INFO usuario)
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
                    List<LOGI_Usuarios_INFO> lstUsuarios = new List<LOGI_Usuarios_INFO>();
                    new LOGI_Usuarios_PD(oTool.CONST_CONNECTION).ListaUsuarios(ref lstUsuarios, usuario);
                    if (lstUsuarios.Count == 0)
                    {

                        usuario.iUsuariocrea = oUser.iUsuario;
                        oResponse.estatus = new LOGI_Usuarios_PD(oTool.CONST_CONNECTION).GuardaUsuario(usuario);
                        oResponse.mensaje = oResponse.estatus;
                    }
                    else {
                        oResponse.estatus = "ERROR";
                        oResponse.mensaje = "El usuario OPEADM que intenta registrar ya ha sido dado de alta anteriormente, favor de validar.";
                    }
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Editausaurio(LOGI_Usuarios_INFO usuario)
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
                    usuario.iUsuariocrea = oUser.iUsuario;
                    oResponse.estatus = new LOGI_Usuarios_PD(oTool.CONST_CONNECTION).EditaUsuario(usuario);
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }
         


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaUsuariosOPE(LOGI_Usuarios_INFO usuario)
        {
            List<LOGI_Usuarios_INFO> lstUsuarios = new List<LOGI_Usuarios_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    if (string.IsNullOrEmpty(usuario.sUsuario))
                        usuario.sUsuarioExcep = oUser.iUsuario;
                    oResponse.estatus = new LOGI_Loginusuario_PD(oTool.CONST_CONNECTION).ListausuariosOPE(ref lstUsuarios, usuario);
                    oResponse.data = lstUsuarios;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listaperfiles()
        {
            List<LOGI_Roles_INFO> lstRoles = new List<LOGI_Roles_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    LOGI_Roles_INFO tmp = new LOGI_Roles_INFO();
                    oResponse.estatus = new LOGI_Roles_PD(oTool.CONST_CONNECTION).ListaRoles(ref lstRoles, tmp);
                    oResponse.data = lstRoles;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listapermisosperfil(int perfil)
        {
            List<LOGI_Permisos_INFO> lstpermisos = new List<LOGI_Permisos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = (perfil == 1 || perfil == 2) ? new LOGI_Permisos_PD(oTool.CONST_CONNECTION).ListaPermisos(ref lstpermisos) : new LOGI_Permisos_PD(oTool.CONST_CONNECTION).ListaPermisosByPerfil(ref lstpermisos, perfil);
                    oResponse.data = lstpermisos;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }


        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listamodulos(string usuario)
        {
            List<LOGI_Modulos_INFO> lstModulos = new List<LOGI_Modulos_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Modulos_PD(oTool.CONST_CONNECTION).Listamodulos(ref lstModulos);
                    if (!string.IsNullOrEmpty(usuario))
                    {
                        List<LOGI_Usuarios_INFO> lstUsuarios = new List<LOGI_Usuarios_INFO>();
                        LOGI_Usuarios_INFO oUsuario = new LOGI_Usuarios_INFO();
                        oUsuario.sUsuario = usuario;
                        new LOGI_Usuarios_PD(oTool.CONST_CONNECTION).ListaUsuarios(ref lstUsuarios, oUsuario);
                        if (lstUsuarios.Count == 1)
                        {
                            oUsuario = new LOGI_Usuarios_INFO();
                            oUsuario = lstUsuarios[0];
                            if (oUsuario.iPerfil == 1)
                                lstModulos.ForEach(x => x.bCheck = true);
                            else
                            {
                                List<LOGI_Modulos_INFO> lstpermisos = new List<LOGI_Modulos_INFO>();
                                List<LOGI_Modulos_INFO> lsttemporales = new List<LOGI_Modulos_INFO>();

                                new LOGI_Modulos_PD(oTool.CONST_CONNECTION).Listapermisosusuario(ref lstpermisos, oUsuario.iSucursal);
                                foreach (LOGI_Modulos_INFO o in lstModulos)
                                {
                                    var key = lstpermisos.FirstOrDefault(x => x.clave == o.clave);
                                    if (key != null)
                                    {
                                        key.bCheck = true;
                                        lsttemporales.Add(key);
                                    }
                                    else lsttemporales.Add(o);
                                }
                                lstModulos = lsttemporales;
                            }
                        }
                        else oResponse.estatus = "No se  ha podido encontrar los datos del usuario";
                    }

                    if (lstModulos.Count > 0)
                        lstModulos.ForEach(x => x.pagina = "");
                    oResponse.mensaje = oResponse.estatus;
                    oResponse.estatus = lstModulos.Count > 0 ? "OK" : "ERROR";
                    oResponse.data = lstModulos;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        #endregion "WEBMETHODS"
    }
}