using INFO.Objetos;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace LOGIWEB.Metodos
{
    /// <summary>
    /// Descripción breve de UploadFiles
    /// </summary>
    public class UploadFiles : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["delete"]))
                    this.m_OnEventDelete(context);
                else this.m_OnEventUpload(context);
            }
            catch { }

        }
        void m_OnEventDelete(HttpContext context)
        {
            try
            {
                LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
                String sPath = oTool.RepoTemporal();
                String sFileDataName = Convert.ToString(context.Request.QueryString["delete"]);
                String sPathFileDelete = string.Format(@"{0}\{1}", sPath, sFileDataName);
                if (File.Exists(sPathFileDelete))
                    File.Delete(sPathFileDelete);
            }
            catch
            {

            }
        }
        void m_OnEventUpload(HttpContext context)
        {
            LOGI_WebTools_PD oTool = new LOGI_WebTools_PD();
            List<LOGI_UploadFile_INFO> lstresponse = new List<LOGI_UploadFile_INFO>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    bool bContinua = true;
                    HttpPostedFile hpf = context.Request.Files[i] as HttpPostedFile;
                    string FileName = string.Empty;
                    if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE" || HttpContext.Current.Request.Browser.Browser.Equals("InternetExplorer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string[] files = hpf.FileName.Split(new char[] { '\\' });
                        FileName = files[files.Length - 1];
                    }
                    else
                        FileName = hpf.FileName;
                    if (hpf.ContentLength == 0)
                        bContinua = false;


                    if (bContinua)
                    {
                        FileName = FileName.Replace("/", "");
                        String sPath = oTool.RepoTemporal();
                        string sFileName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Convert.ToBase64String(Guid.NewGuid().ToByteArray()), FileName);

                        string savedFileName = string.Format(@"{0}\{1}", sPath, sFileName);
                        hpf.SaveAs(savedFileName);
                        lstresponse.Add(new LOGI_UploadFile_INFO()
                        {
                            Thumbnail_url = sFileName,
                            Name = sFileName,
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            delete_type = "POST",
                            delete_url = "../../Metodos/UploadFiles.ashx?delete=" + sFileName
                        });
                    }
                    else
                    {
                        lstresponse.Add(new LOGI_UploadFile_INFO()
                        {
                            Thumbnail_url = "",
                            Name = FileName + " el archivo no se ha encontrado",
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            delete_type = "",
                            delete_url = ""
                        });
                    }
                    var uploadedFiles = new
                    {
                        files = lstresponse.ToArray()
                    };
                    var jsonObj = js.Serialize(uploadedFiles);
                    context.Response.Write(jsonObj.ToString());
                }
            }
            catch (Exception ex)
            {

                string datos = ex.Message;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}