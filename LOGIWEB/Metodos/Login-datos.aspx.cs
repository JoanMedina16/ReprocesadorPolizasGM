using INFO.Objetos;
using INFO.Tablas;
using PD.Herramientas;
using PD.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos
{
    public partial class Login_datos : System.Web.UI.Page
    {
        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Login(LOGI_Usuarios_INFO user)
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD(user);
            LOGI_Usuarios_INFO ouser = new LOGI_Usuarios_INFO();
            ouser.sUsuario = user.sUsuario;
            LOGI_Usuarios_INFO oUsuario = new LOGI_Usuarios_INFO();
            oResponse.estatus = new LOGI_Loginusuario_PD(oTool.CONST_CONNECTION).ValidaLoginUsuario(ref oUsuario, user);
            if (oResponse.estatus == "SIN RESULTADOS")
                oResponse.mensaje = "Usuario o contraseña no válidos no se ha podido verficiar la existencia del usuario";
            else if (oResponse.estatus == "DERECHOS")
                oResponse.mensaje = "El usuario proporcionado no cuenta con los permisos para el acceso al sistema.";
            else if (oResponse.estatus.Equals("PASS", StringComparison.InvariantCultureIgnoreCase))
            {
                oResponse.estatus = "ERROR";
                oResponse.mensaje = "Proporciona una contraseña válida para el usuario";
            }
            else if (oResponse.estatus.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
            {
                oUsuario.iAdministrador = oUsuario.sUsuario.Equals("usrsiat", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
                oResponse.estatus = new LOGI_Loginusuario_PD(oTool.CONST_CONNECTION).RecuperaSucursal(ref oUsuario);
                if (oResponse.estatus != "OK")
                {
                    oResponse.estatus = "ERROR";
                    oResponse.mensaje = "No se ha podido recuperar la información de la sucursal y centro de costo";
                }
                else oTool.StarSession(oUsuario);
            }
            else oResponse.mensaje = oResponse.estatus;
            return oResponse;

        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Logout()
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();

            if (oTool.DestroySession())
                oResponse.estatus = "OK";
            else
            {
                oResponse.estatus = "ERROR";
                oResponse.mensaje = "No se ha podido cerrar la sesión. Favor de reintentar";
            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ValidaSesion()
        {
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();

            if (oTool.UsuarioSession() != null)
                oResponse.estatus = "OK";
            else
                oResponse.estatus = "-1";
            return oResponse;
        }
        #endregion "WEBMETHODS"
    }
}