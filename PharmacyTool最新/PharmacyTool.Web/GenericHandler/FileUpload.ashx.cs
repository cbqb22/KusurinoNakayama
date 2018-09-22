using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading;
using System.Text;
using PharmacyTool.Web.Code.SilverlightFileUpload;

namespace PharmacyTool.Web.GenericHandler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FileUpload : IHttpHandler
    {
        private HttpContext ctx;
        public void ProcessRequest(HttpContext context)
        {

            string rootPath = CheckType(context.Request.QueryString["Type"]);

            ctx = context;
            //string uploadPath = context.Server.MapPath("~/Upload");
            FileUploadProcess fileUpload = new FileUploadProcess();
            fileUpload.FileUploadCompleted += new FileUploadCompletedEvent(fileUpload_FileUploadCompleted);
            fileUpload.ProcessRequest(context, rootPath);
        }

        void fileUpload_FileUploadCompleted(object sender, FileUploadCompletedEventArgs args)
        {
            ctx.Response.Write("UploadCompleted");
            //string id = ctx.Request.QueryString["id"];
            //FileInfo fi = new FileInfo(args.FilePath);
            //string targetFile = Path.Combine(fi.Directory.FullName, args.FileName);
            //if (File.Exists(targetFile))
            //    File.Delete(targetFile);
            //fi.MoveTo(targetFile);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        private string CheckType(string Type)
        {

            if (Type.Equals("在庫関連"))
            {
#if DEBUG
                return PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPathDEBUG;
#elif NAKAYAMA
                return PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPathNAKAYAMA;
#else
                return PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPath;
#endif
            }
            else if (Type.Equals("資料"))
            {

#if DEBUG
                return PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPathDEBUG;
#elif NAKAYAMA
                return PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPathNAKAYAMA;
#else
                return PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPath;
#endif
            }
            else if (Type.Equals("掲示板資料"))
            {

#if DEBUG
                return PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathDEBUG;
#elif NAKAYAMA
                return PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathNAKAYAMA;
#else
                return PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPath;
#endif
            }

            throw new Exception("更新するTypeが指定されていません");


        }

    }
}
