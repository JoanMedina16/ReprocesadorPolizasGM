using INFO.Tablas;
using INFO.Tablas.CAT;
using PD.Herramientas;
using PD.Tablas.CAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB
{
    public partial class _default : System.Web.UI.Page
    {
        public string sNombre { get; set; }
        public string sCentro { get; set; }
        public string sSucursal { get; set; }
        public string sLeyenda { get; set; }
        public string sVersion = "0.00000000014";
        protected void Page_Load(object sender, EventArgs e)
         {
            LOGI_Usuarios_INFO oUser = new LOGI_Usuarios_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            oUser = oTool.UsuarioSession();
            if (oUser == null)
                Response.Redirect("login.aspx");
            else
            {
                if (!IsPostBack)
                {
                    sNombre = string.Format("{0} - {1}", oUser.sUsuario, oUser.sNombre).ToUpper();
                    sCentro = string.Format("{0} - {1}", oUser.subctcentro, oUser.centrocosto).ToUpper();
                    sSucursal = string.Format("{0} - {1}", oUser.iSucursal, oUser.sSucursal).ToUpper();
                    sLeyenda = string.Format("usuario: {0} | sucursal: {1} | centro de costo: {2}", oUser.sUsuario,oUser.sSucursal, oUser.centrocosto).ToUpper();
                    CargaDatosmenu();
                    CargaPermisos();
                }
            }
        }

        void CargaDatosmenu()
        {
            List<LOGI_Modulos_INFO> lstModulos = new List<LOGI_Modulos_INFO>();
            List<LOGI_Modulos_INFO> lstPermisos = new List<LOGI_Modulos_INFO>();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            string response = string.Empty;
            LOGI_Usuarios_INFO ouser = oTool.UsuarioSession();

            //Si el usuario tiene asignado el perfil de "SA" super usuario/sistemas se listan todos los modulos 
            //Caso contrario listamos los modulos según el permiso al que se le ha asignado 
            if (ouser.iPerfil == 1)
                response = new LOGI_Modulos_PD(oTool.CONST_CONNECTION).Listamodulos(ref lstModulos);
            else
            {
                //recuperamos todos los modulos 
                List<LOGI_Modulos_INFO> lsttemporales = new List<LOGI_Modulos_INFO>();
                lstModulos = new List<LOGI_Modulos_INFO>();
                response = new LOGI_Modulos_PD(oTool.CONST_CONNECTION).Listamodulos(ref lstModulos);
                response = new LOGI_Modulos_PD(oTool.CONST_CONNECTION).Listapermisosusuario(ref lstPermisos, ouser.iUsuario);
                var lstGrupos = lstPermisos.GroupBy(x => x.padre);
                foreach (var o in lstGrupos)
                {
                    if (!string.IsNullOrEmpty(o.Key))
                    {
                        var oMathc = lstModulos.FirstOrDefault(x => x.clave == o.Key);
                        if (oMathc != null)
                            lsttemporales.Add(oMathc);
                    }
                }
                lstModulos = lsttemporales;
            }

            if (response.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
            {
                lstModulos = lstModulos.Where(x=>x.nivel == 0).ToList();
                rptItems.DataSource = lstModulos.Count > 0 ? lstModulos : null;
                rptItems.DataBind();
            }
        }

        void CargaPermisos()
        {
            LOGI_Usuarios_INFO oUser = new LOGI_Usuarios_INFO();
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            oUser = oTool.UsuarioSession();
            LOGI_Permisos_PD oPermisoControl = new LOGI_Permisos_PD(oTool.CONST_CONNECTION);
            //valida si es super usuario o administrador le damos permisos a todo
            if (oUser.iPerfil == 1 || oUser.iPerfil == 2)
            {
                hdAgregar.Value = "1";
                hdEditar.Value = "1";
                hdEliminar.Value = "1";
                hdDescargar.Value = "1";
                hdBuscar.Value = "1";
                hdUploadcsv.Value = "1";
                hdEntradaAX.Value = "1";
                hdRsterror.Value = "1";
                hdAdjuntaxml.Value = "1";

            }
            else
            {
                List<LOGI_Permisos_INFO> lstPermisos = new List<LOGI_Permisos_INFO>();
                string sreponse = oPermisoControl.ListaPermisosByPerfil(ref lstPermisos, oUser.iPerfil);
                if (sreponse.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                {
                    //validamos uno a uno los valores que trae la lista
                    var oAgrega = lstPermisos.FirstOrDefault(x => x.permiso == 1);
                    hdAgregar.Value = oAgrega == null ? "0" : "1";
                    var oEdita = lstPermisos.FirstOrDefault(x => x.permiso == 2);
                    hdEditar.Value = oEdita == null ? "0" : "1";
                    var oElimina = lstPermisos.FirstOrDefault(x => x.permiso == 3);
                    hdEliminar.Value = oElimina == null ? "0" : "1";
                    var oDescarga = lstPermisos.FirstOrDefault(x => x.permiso == 4);
                    hdDescargar.Value = oDescarga == null ? "0" : "1";
                    var oBusca = lstPermisos.FirstOrDefault(x => x.permiso == 5);
                    hdBuscar.Value = oBusca == null ? "0" : "1";
                    var oUploadcsv = lstPermisos.FirstOrDefault(x => x.permiso == 6);
                    hdUploadcsv.Value = oUploadcsv == null ? "0" : "1";
                    var oEntradaAX = lstPermisos.FirstOrDefault(x => x.permiso == 7);
                    hdEntradaAX.Value = oEntradaAX == null ? "0" : "1";
                    var oResetError = lstPermisos.FirstOrDefault(x => x.permiso == 8);
                    hdRsterror.Value = oResetError == null ? "0" : "1";
                    var oAdjuntaxml = lstPermisos.FirstOrDefault(x => x.permiso == 9);
                    hdAdjuntaxml.Value = oAdjuntaxml == null ? "0" : "1";
                }
                //cargamos los permisos en la sesion para utilizarlos posteriormente
                Session["permisos"] = lstPermisos;
            }
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
                    List<LOGI_Modulos_INFO> lstModulos = new List<LOGI_Modulos_INFO>();
                    LOGI_Usuarios_INFO oUser = oTool.UsuarioSession();

                    if (oUser.iPerfil == 1)
                        new LOGI_Modulos_PD(oTool.CONST_CONNECTION).Listamodulos(ref lstModulos);
                    else
                        new LOGI_Modulos_PD(oTool.CONST_CONNECTION).Listapermisosusuario(ref lstModulos, oUser.iUsuario);

                    LOGI_Modulos_INFO oItem = (LOGI_Modulos_INFO)e.Item.DataItem;
                    List<LOGI_Modulos_INFO> ltChilds = lstModulos.Where(x => x.nivel == 1 && x.padre.Equals(oItem.clave, StringComparison.InvariantCultureIgnoreCase)).ToList();

                    ltChilds.ForEach(
                        x =>
                        {
                            if (x.nombre.Trim().Equals("Informe de cuentas", StringComparison.InvariantCultureIgnoreCase))
                            {
                                x.ref_nav = @"Target=""_blank""";
                                x.navega = x.pagina;
                            }
                            else x.navega = "#";
                        }
                        );

                    var childRepeater = (Repeater)e.Item.FindControl("rptDetalle");
                    childRepeater.DataSource = ltChilds;
                    childRepeater.DataBind();
                }
            }
            catch { }
        }
    }
}