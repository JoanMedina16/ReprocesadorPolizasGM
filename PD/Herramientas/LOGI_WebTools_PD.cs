using INFO.Tablas;
using INFO.Tablas.D365;
using OfficeOpenXml;
using PD.Tablas.D365;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Herramientas
{
   public class LOGI_WebTools_PD : System.Web.UI.Page
    {
        public string CONST_CONNECTION = "";
        public string CONST_EQUIV_CONNECTION = "";
        public string CONST_ZAP_CONNECTION = "";
        public string CONST_CONNECTIONLM = ConfigurationManager.ConnectionStrings["cnnLISTAM"].ConnectionString;
        public string CONST_CONNLOG = ConfigurationManager.ConnectionStrings["cnnLOG"].ConnectionString;
        const string CONST_CLASE = "LOGI_WebTools_PD.cs";
        const string CONST_MODULO = "Herramientas Web";
        public int CONST_EMPRESA = 67;


        public LOGI_WebTools_PD()
        {

            if (Session["CONNECTION"] != null)
            {
                LOGI_ConfiguracionD365_INFO oConfig = new LOGI_ConfiguracionD365_INFO();
                CONST_CONNECTION = LOGI_Rijndael_PD.DecryptRijndael(Convert.ToString(Session["CONNECTION"]));
                new LOGI_ConfiguracionD365_PD(CONST_CONNECTION).ListaConfiguracion("", ref oConfig);
                CONST_EQUIV_CONNECTION = oConfig.Conexion_eqv;
                CONST_ZAP_CONNECTION = oConfig.conexionzap;
            }

        }
        public LOGI_WebTools_PD(LOGI_Usuarios_INFO oUsuario)
        {
            try
            {
                string sPassCrypt = LOGI_EncryptaOTM_PD.Encripta(oUsuario.sContrasenia);
                string sServidor = LOGI_Rijndael_PD.DecryptRijndael(oUsuario.sServer);
                string sDataBase = LOGI_Rijndael_PD.DecryptRijndael(oUsuario.sDatabase);
                CONST_CONNECTION = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password='{3}'; Connection Timeout = 120", sServidor, sDataBase, oUsuario.sUsuario.ToLower(), sPassCrypt);
                Session["CONNECTION"] = LOGI_Rijndael_PD.EncryptRijndael(CONST_CONNECTION);
            }
            catch { }
        }

        public Object GetUserinSession()
        {
            return true;
        }
        public LOGI_Usuarios_INFO UsuarioSession()
        {
            LOGI_Usuarios_INFO oUsuario = null;

            if (Session["user"] != null)
            {
                oUsuario = new LOGI_Usuarios_INFO();
                oUsuario.sNombre = Convert.ToString(Session["nombre"]);
                oUsuario.sUsuario = Convert.ToString(Session["user"]);
                oUsuario.iUsuario = Convert.ToInt32(Session["clave"]);
                oUsuario.iAdministrador =  Convert.ToInt32(Session["admin"]);
                oUsuario.sSucursal = Convert.ToString(Session["Sucursal"]);
                oUsuario.iSucursal = Convert.ToInt32(Session["SucursalID"]);
                oUsuario.ctcentro = Convert.ToInt32(Session["ctacentrouser"]);
                oUsuario.subctcentro = Convert.ToInt32(Session["sctacentrouser"]);
                oUsuario.centrocosto = Convert.ToString(Session["nomcentro"]);
                oUsuario.iPerfil = Convert.ToInt32(Session["PerfilID"]);

            }
            return oUsuario;
        }
        public void StarSession(LOGI_Usuarios_INFO oUsuario)
        {
            Session["clave"] = oUsuario.iUsuario;
            Session["user"] = oUsuario.sUsuario;
            Session["nombre"] = oUsuario.sNombre;
            Session["admin"] = oUsuario.iAdministrador;
            Session["Sucursal"] = oUsuario.sSucursal;
            Session["SucursalID"] = oUsuario.iSucursal;
            Session["PerfilID"] = oUsuario.iPerfil;
            Session["ctacentrouser"] = "243";
            Session["sctacentrouser"] = "80";
            Session["centroab"] = "CG";
            Session["nomcentro"] = "Carga General";

        }
        public bool DestroySession()
        {
            try
            {
                Session.Contents.RemoveAll();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidaAcceso(int modulo)
        {
            bool bContinuar = false;
            string sClaveuser = string.Empty;
            try
            {
                //CFDI_Loginusuario_PD oLogin = new CFDI_Loginusuario_PD(this.CONST_CONNECTION_OPEADM);
                //CFDI_Usuarios_INFO ouser = this.UsuarioSession();
                //bContinuar = oLogin.ValidaPermisomodulo(modulo, ouser.iUsuario).Equals("OK", StringComparison.InvariantCultureIgnoreCase);
                bContinuar = true;
            }
            catch (Exception ex)
            {
                new LOGI_Tools_PD().LogError(ex, "ValidaAcceso", sClaveuser, CONST_CLASE, CONST_MODULO);
            }
            return bContinuar;
        }
        public string RepoTemporal(int iDias = 10)
        {
            string sPath = string.Empty;
            try
            {
                sPath = string.Format(@"{0}\tmp", AppDomain.CurrentDomain.BaseDirectory);
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);

                try
                {
                    DateTime oTime = DateTime.Now.AddDays(-iDias);
                    DirectoryInfo oDir = new DirectoryInfo(sPath);
                    FileInfo[] oFiles = oDir.GetFiles().OrderBy(x => x.CreationTime).ToArray();
                    foreach (FileInfo oFile in oFiles)
                    {
                        if (oFile.CreationTime <= oTime)
                        {
                            try
                            {
                                //eliminamos el archivo 
                                oFile.Delete();
                            }
                            catch { }
                        }
                        else continue;
                    }
                }
                catch { }
            }
            catch
            {

            }

            return sPath;
        }


        public bool RecuperaDataExcelV2(string sFullpath, out string sResponse, ref DataTable dsDataExcel, string sNombrehoja = "Hoja1")
        {
            bool bContinuar = false;
            sResponse = string.Empty;
            try
            {
                ExcelPackage package = new ExcelPackage(new FileInfo(sFullpath));
                ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                    dsDataExcel.Columns.Add(firstRowCell.Text.Replace("$", ""));
                for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                    var newRow = dsDataExcel.NewRow();
                    foreach (var cell in row)
                    {
                        try
                        {
                            if (cell.Text != null && cell.Text.Length > 0)
                                newRow[cell.Start.Column - 1] = cell.Text.Replace("$", "");

                            else
                                newRow[cell.Start.Column - 1] = "";
                        }
                        catch { }
                    }
                    dsDataExcel.Rows.Add(newRow);
                }

                bContinuar = true;
            }
            catch (Exception ex)
            {
                sResponse = ex.Message;
            }

            return bContinuar;
        }

        public bool RecuperaDataFromExcel(string sFullpath, out string sResponse, ref DataSet dsDataExcel)
        {
            bool bContinuar = false;
            string sConnectionOLEDB = string.Empty;
            OleDbConnection oConnExcel = null;
            sResponse = "ERROR";
            try
            {

                FileInfo file = new FileInfo(sFullpath);
                if (!file.Exists)
                {
                    sResponse = "El archivo adjunto no se ha encontrado. asegurese de haberlo cargado";
                    return false;
                }
                String extension = file.Extension;
                switch (extension)
                {
                    case ".xls":
                        sConnectionOLEDB = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", sFullpath);

                        break;
                    case ".xlsx":
                        sConnectionOLEDB = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", sFullpath);
                        break;
                    default:
                        sConnectionOLEDB = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", sFullpath);
                        break;
                }
                OleDbCommand oCmdExcel = new OleDbCommand();
                OleDbDataAdapter oDataAdapExcel = new OleDbDataAdapter();
                dsDataExcel = new DataSet();
                oConnExcel = new OleDbConnection(sConnectionOLEDB);
                oConnExcel.Open();
                oCmdExcel = new OleDbCommand("SELECT * FROM [Hoja1$]", oConnExcel);
                oDataAdapExcel = new OleDbDataAdapter(oCmdExcel);
                oDataAdapExcel.Fill(dsDataExcel, "dsDataUpload");
                oConnExcel.Close();
                bContinuar = true;
            }
            catch (Exception ex)
            {
                sResponse = ex.Message;
            }
            finally
            {
                if (oConnExcel != null)
                {
                    if (oConnExcel.State == ConnectionState.Open)
                        oConnExcel.Close();
                }
            }
            return bContinuar;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
