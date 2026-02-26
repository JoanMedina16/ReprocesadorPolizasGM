using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIWEB
{
    public partial class login : System.Web.UI.Page
    {
        internal static DataSet BaseServers = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadConnection();
                lstServer_SelectedIndexChanged(sender, e);
            }
        }


        void LoadConnection()
        {
            try
            {
                BaseServers = new DataSet();
                BaseServers.ReadXml(string.Format("{0}/Serversources/ServerList.xml", AppDomain.CurrentDomain.BaseDirectory));
                BaseServers.Tables["servers"].Columns.Add(new DataColumn("ID"));
                for (int ID = 0; ID < BaseServers.Tables["servers"].Rows.Count; ID++)
                {
                    BaseServers.Tables["servers"].Rows[ID]["ID"] = ID;
                }

                this.lstServer.Items.Clear();
                this.lstServer.DataSource = BaseServers.Tables["servers"];
                this.lstServer.DataTextField = "dsn";
                this.lstServer.DataValueField = "ID";
                this.lstServer.DataBind();
                this.lstServer.Items[0].Selected = true;


            }
            catch
            {
                this.lblError.Text = "ERROR: El archivo de conexiones no está disponible";
            }
        }

        protected void lstServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                this.lblTitulo.Text = "Descripcion de la base de datos seleccionada.";
                for (int i = 0; i < BaseServers.Tables["servers"].Rows.Count; i++)
                {
                    if (BaseServers.Tables["servers"].Rows[i]["ID"].ToString().Trim() == this.lstServer.SelectedItem.Value.Trim())
                    {
                        this.lblLeyenda.Text = BaseServers.Tables["servers"].Rows[i]["notas"].ToString().Trim();
                        this.hdnServer.Value = LOGI_Rijndael_PD.EncryptRijndael(BaseServers.Tables["servers"].Rows[i]["server"].ToString().Trim());
                        this.hdnDataBase.Value = LOGI_Rijndael_PD.EncryptRijndael(BaseServers.Tables["servers"].Rows[i]["database"].ToString().Trim());
                    }
                }
                this.lblError.Text = "Servidor: " + this.lstServer.SelectedItem.Text.Trim();
            }
            catch (Exception ex)
            {

                this.lblError.Text = string.Format("No se ha podido realizar la acción: {0}", ex.Message);
            }
        }
    }
}