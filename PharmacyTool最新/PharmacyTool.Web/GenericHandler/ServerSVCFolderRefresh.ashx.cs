using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace PharmacyTool.Web.GenericHandler
{
    /// <summary>
    /// ServerSVCFolderRefresh の概要の説明
    /// </summary>
    public class ServerSVCFolderRefresh : IHttpHandler
    {

        private HttpContext ctx;
        public void ProcessRequest(HttpContext context)
        {
            ctx = context;


            try
            {
                string caQuery = context.Request.QueryString["CheckAccess"];

                if (string.IsNullOrEmpty(caQuery))
                {
                    context.Response.Write("Illegal Access");
                    context.Response.Flush();
                    return;
                }

                if (!CheckType(context.Request.QueryString["CheckAccess"]))
                {
                    context.Response.Write("Illegal Access");
                    context.Response.Flush();
                    return;
                }


#if DEBUG
                string SVCFolderPath = "";

                context.Response.Write("Can't Access");
                context.Response.Flush();
                return;
#elif NAKAYAMA
                string SVCFolderPath = PharmacyTool.Web.Properties.Settings.Default.SVCFolderPathNAKAYAMA;
#else
                string SVCFolderPath = "";

                context.Response.Write("Can't Access");
                context.Response.Flush();
                return;
#endif
                string SVCBackUpFolderPath = SVCFolderPath + "_BackUp";

                if (!Directory.Exists(SVCFolderPath))
                {
                    context.Response.Write("Don't find SVC Folder,Please call to Administrator");
                    context.Response.Flush();
                    return;
                }

                // 前にバックアップフォルダーが残っている場合があるので、一旦削除して作成する
                if (Directory.Exists(SVCBackUpFolderPath))
                {

                    Directory.Delete(SVCBackUpFolderPath,true);
                    Directory.CreateDirectory(SVCBackUpFolderPath);
                    //File.SetAttributes(SVCBackUpFolderPath,File.GetAttributes(SVCFolderPath));
                }

                // バックアップ先にフォルダーを作成
                var SVCFoldersPath = System.IO.Directory.GetDirectories(SVCFolderPath, "*", SearchOption.AllDirectories);
                foreach (var FolderPath in SVCFoldersPath)
                {
                    var CopyFolderPath = FolderPath.Replace(SVCFolderPath, SVCBackUpFolderPath);

                    if (!Directory.Exists(CopyFolderPath))
                    {
                        Directory.CreateDirectory(CopyFolderPath);
                        //File.SetAttributes(CopyFolderPath, File.GetAttributes(FolderPath));

                    }

                }

                // バックアップ先にファイルをコピー
                var SVCFilesPath = System.IO.Directory.GetFiles(SVCFolderPath, "*", SearchOption.AllDirectories);
                foreach (var FilePath in SVCFilesPath)
                {
                    var CopyFilePath = FilePath.Replace(SVCFolderPath, SVCBackUpFolderPath);

                    File.Copy(FilePath, CopyFilePath, true);
                }

                // 元のSVC Folder内のファイルを全削除
                foreach (var FilePath in SVCFilesPath)
                {
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                }

                // バックアップ先のフォルダーからSVC Folderへコピー(先程と逆のことを行う)
                var SVCBackUpFilesPath = System.IO.Directory.GetFiles(SVCBackUpFolderPath, "*.*", SearchOption.AllDirectories);
                foreach (var FilePath in SVCBackUpFilesPath)
                {
                    var CopyFilePath = FilePath.Replace(SVCBackUpFolderPath, SVCFolderPath);

                    File.Copy(FilePath, CopyFilePath, true);
                }
                
            }
            catch (Exception ex)
            {
                context.Response.Write(string.Format("Raise Error!! ErrorMessage:{0} StackTrace:{1}",ex.Message,ex.StackTrace));
                context.Response.Flush();
                return;
            }

            
            context.Response.Write(string.Format("Refreshing Server File is SuccessFul!! Please try to Access your HomePage."));
            context.Response.Flush();
            return;



        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        private bool CheckType(string Type)
        {

            if (Type.Equals("SvcReplaceWorking"))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}