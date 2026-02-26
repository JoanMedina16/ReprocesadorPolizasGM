using INFO.Objetos;
using INFO.Objetos.MOTF;
using INFO.Tablas;
using PD.Herramientas;
using PD.Objetos.MOTF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.MOTF
{
    public partial class ViajePortes_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListaViajesporte(LOGI_PorteViaje_INFO Viaje)
        {
            List<LOGI_PorteViaje_INFO> lstViajes = new List<LOGI_PorteViaje_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_PortesViaje_PD(oTool.CONST_CONNECTION).ListaBandejaportesViaje(oUser.sUsuario, Viaje, ref lstViajes);
                    oResponse.data = lstViajes;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }
        #endregion "WEBMETHODS"
    }
}