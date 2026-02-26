using INFO.Objetos;
using INFO.Tablas;
using INFO.Tablas.D365;
using PD.Herramientas;
using PD.Tablas.D365;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.REPORTES.D365
{
    public partial class Requisiciones_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Siempre que se accede a este archivo por url se redirige al login
            Response.Redirect("../../../login.aspx");
        }

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO ListarequisicionesZAP(LOGI_Requisicion_INFO Periodo)
        {
            List<LOGI_Requisicion_INFO> lstRequisiciones = new List<LOGI_Requisicion_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Requisiciones_PD(oTool.CONST_ZAP_CONNECTION, oTool.CONST_CONNECTION ).Listarequisiones(oUser.iPerfil,ref lstRequisiciones, Periodo);
                    oResponse.data = lstRequisiciones;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }

        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Listadetallerequi(LOGI_Requisicion_Line_INFO Detalle)
        {
            List<LOGI_Requisicion_Line_INFO> lstLineas = new List<LOGI_Requisicion_Line_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Requisiciones_PD(oTool.CONST_ZAP_CONNECTION, oTool.CONST_CONNECTION).Listarequisiondetalle(oUser.sUsuario, ref lstLineas, Detalle.RECID);
                    oResponse.data = lstLineas;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }
        #endregion "WEBMETHODS"
    }
}