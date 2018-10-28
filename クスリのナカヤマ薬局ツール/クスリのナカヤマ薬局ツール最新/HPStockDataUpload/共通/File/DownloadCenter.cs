using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using IO = System.IO;
using クスリのナカヤマ薬局ツール.Properties;
using System.Windows;

namespace クスリのナカヤマ薬局ツール.共通.File
{
    public static class DownloadCenter
    {

        public static List<string> DownloadFileList(string UrlFolderPath)
        {
            List<string> fileListInfo = new List<string>();
            try
            {

                //ファイル一覧を取得するディレクトリのURI
                Uri u = new Uri(UrlFolderPath);

                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential(Settings.FtpId, Settings.FtpCredential);
                //MethodにWebRequestMethods.Ftp.ListDirectoryDetails("LIST")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = Settings.UsePassive;

                //FtpWebResponseを取得


                //FTPサーバーから送信されたデータを取得
                using(System.Net.FtpWebResponse ftpRes =(System.Net.FtpWebResponse)ftpReq.GetResponse())
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ftpRes.GetResponseStream()))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        fileListInfo.Add(line);
                    }
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            return fileListInfo;

        }

        public static void DownloadFile(string downUrlPath,string savePath)
        {

            System.Net.FtpWebResponse ftpRes = null;
            try
            {
                ////ダウンロードするファイルのURI
                Uri u = new Uri(downUrlPath);

                //ダウンロードしたファイルの保存先
                string saveFile = savePath;


                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential(Settings.FtpId, Settings.FtpCredential);
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = Settings.UsePassive;
                // タイムアウトは６０秒とする
                ftpReq.Timeout = 60000;
                //FtpWebResponseを取得
                ftpRes =
                    (System.Net.FtpWebResponse)ftpReq.GetResponse();

                using (System.IO.Stream resStrm = ftpRes.GetResponseStream()) //ファイルをダウンロードするためのStreamを取得
                using (System.IO.FileStream fs = new System.IO.FileStream(saveFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))   //ダウンロードしたファイルを書き込むためのFileStreamを作成
                {
                    //ダウンロードしたデータを書き込む
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int readSize = resStrm.Read(buffer, 0, buffer.Length);
                        if (readSize == 0)
                            break;
                        fs.Write(buffer, 0, readSize);
                    }
                }

                //FTPサーバーから送信されたステータスを表示
                System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription));
                //System.Diagnostics.Debug.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription);


            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }




    }
}
