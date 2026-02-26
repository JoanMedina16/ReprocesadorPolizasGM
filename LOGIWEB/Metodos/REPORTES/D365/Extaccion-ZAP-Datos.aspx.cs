using INFO.Objetos;
using INFO.Objetos.D365;
using INFO.Tablas;
using PD.Herramientas;
using PD.Objetos.D365;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB.Metodos.REPORTES.D365
{
    public partial class Extaccion_ZAP_Datos : System.Web.UI.Page
    {
        public static int CONST_MODULO = 130;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Siempre que se accede a este archivo por url se redirige al login
            Response.Redirect("../../../login.aspx");
        }

        #region "WEBMETHODS"
        [WebMethod(EnableSession = true)]
        public static LOGI_Response_INFO Extraecuentaszap(LOGI_Extraccion_ZAP_INFO Periodo)
        {
            List<LOGI_Extraccion_ZAP_INFO> lstCuentas = new List<LOGI_Extraccion_ZAP_INFO>();
            LOGI_Response_INFO oResponse = new LOGI_Response_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();
            if (oUser == null)
                oResponse.estatus = "-1";
            else
            {
                if (oTool.ValidaAcceso(CONST_MODULO))
                {
                    oResponse.estatus = new LOGI_Extraccion_ZAP_PD(oTool.CONST_ZAP_CONNECTION).Listacuentas(ref lstCuentas, Periodo);
                    oResponse.data = lstCuentas;
                    oResponse.mensaje = oResponse.estatus;
                }
                else oResponse.estatus = "-2";

            }
            return oResponse;
        }
        #endregion "WEBMETHODS"

    }
}