using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using IO = System.IO;
using OASystem.Properties;
using System.Windows;
using OASystem.View.UserControls;

namespace OASystem.ViewModel.File
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
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.ListDirectoryDetails("LIST")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;

                //FtpWebResponseを取得


                //FTPサーバーから送信されたデータを取得
                using (System.Net.FtpWebResponse ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse())
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ftpRes.GetResponseStream()))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        fileListInfo.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileListInfo;

        }

        public static void DownloadFile(string downUrlPath, string savePath)
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
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static event DownloadProgressDelegate ProgressEvent;
        public delegate void DownloadProgressDelegate(int totalpercentage);

        public static void DownloadTotal不動品CSV()
        {
            System.Net.FtpWebResponse ftpRes = null;
            System.Net.FtpWebResponse ftpRes2 = null;
            try
            {
                // バックグラウンドワーカーのキャンセル処理をチェック
                //if (!CheckBackgroundWorkerCanncelation())
                //    return new DeadStockException(true, "処理を中断しました。");


                ////ダウンロードするファイルのURI
                Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/不動品/total.csv");

                //ダウンロードしたファイルの保存先
                string saveFile = OASystem.Properties.Settings.Default.Download不動品TotalFilePath;




                //ファイルダウンロード
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                //ファイルサイズを取得
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
                // タイムアウトは６０秒とする
                ftpReq.Timeout = 60000;
                ftpRes =
                        (System.Net.FtpWebResponse)ftpReq.GetResponse();


                //ファイルサイズ取得
                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq2 = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq2.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                //ファイルサイズを取得
                ftpReq2.Method = System.Net.WebRequestMethods.Ftp.GetFileSize;
                //要求の完了後に接続を閉じる
                ftpReq2.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq2.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq2.UsePassive = false;
                // タイムアウトは６０秒とする
                ftpReq2.Timeout = 60000;
                ftpRes2 =
                        (System.Net.FtpWebResponse)ftpReq2.GetResponse();
                var filesize = ftpRes2.ContentLength;


                using (System.IO.Stream resStrm = ftpRes.GetResponseStream()) //ファイルをダウンロードするためのStreamを取得
                using (System.IO.FileStream fs = new System.IO.FileStream(saveFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))   //ダウンロードしたファイルを書き込むためのFileStreamを作成
                {
                    //ダウンロードしたデータを書き込む
                    byte[] buffer = new byte[1024];
                    int totalReadSize = 0;
                    while (true)
                    {
                        int readSize = resStrm.Read(buffer, 0, buffer.Length);
                        if (readSize == 0)
                            break;
                        totalReadSize += readSize;
                        fs.Write(buffer, 0, readSize);

                        //int d = ((int)(totalReadSize * 100 / filesize));
                        //ProgressEvent(d);
                    }
                }

                //FTPサーバーから送信されたステータスを表示
                System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription));
                //System.Diagnostics.Debug.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DownloadTotal現在庫CSV()
        {
            System.Net.FtpWebResponse ftpRes = null;
            try
            {

                ////ダウンロードするファイルのURI
                Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/現在庫/total.csv");

                //ダウンロードしたファイルの保存先
                string saveFile = OASystem.Properties.Settings.Default.Download現在庫TotalFilePath;


                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void DownloadTotalMEDIS_HOT13TXT()
        {
            System.Net.FtpWebResponse ftpRes = null;
            try
            {

                ////ダウンロードするファイルのURI
                Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/MEDIS/MEDIS_HOT13.TXT");

                //ダウンロードしたファイルの保存先
                string saveFile = OASystem.Properties.Settings.Default.DownloadMEDIS_HOT13lFilePath;


                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void DownloadTotal帳合先マスタCSV()
        {
            System.Net.FtpWebResponse ftpRes = null;
            try
            {

                ////ダウンロードするファイルのURI
                Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/発注支援システムマスタ/帳合先マスタ.csv");

                //ダウンロードしたファイルの保存先
                string saveFile = OASystem.Properties.Settings.Default.Download帳合先マスタFilePath;


                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void DownloadTotal帳合先チェックマスタ医薬品別CSV()
        {
            System.Net.FtpWebResponse ftpRes = null;
            try
            {

                ////ダウンロードするファイルのURI
                Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/発注支援システムマスタ/帳合先チェックマスタ医薬品別.csv");

                //ダウンロードしたファイルの保存先
                string saveFile = OASystem.Properties.Settings.Default.Download帳合先チェックマスタ医薬品別FilePath;


                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void DownloadTotal帳合先チェックマスタメーカー別CSV()
        {
            System.Net.FtpWebResponse ftpRes = null;
            try
            {

                ////ダウンロードするファイルのURI
                Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/発注支援システムマスタ/帳合先チェックマスタメーカー別.csv");

                //ダウンロードしたファイルの保存先
                string saveFile = OASystem.Properties.Settings.Default.Download帳合先チェックマスタメーカー別FilePath;


                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void DownloadTotal優先移動リスト()
        {


            List<string> fli = DownloadCenter.DownloadFileList(Settings.Default.UploadFolderPath優先移動リスト);
            string sfolderpath = Settings.Default.Download優先移動リストFolderPath;

            foreach (var f in fli)
            {
                string downloadFile = Settings.Default.UploadFolderPath優先移動リスト + "/" + f;
                if (!System.IO.Directory.Exists(sfolderpath))
                {
                    System.IO.Directory.CreateDirectory(sfolderpath);
                }
                string sfilepath = System.IO.Path.Combine(sfolderpath, f);
                DownloadCenter.DownloadFile(downloadFile, sfilepath);
            }

        }

        public static void DownloadTotal保護リスト()
        {
            List<string> fli = DownloadCenter.DownloadFileList(Settings.Default.UploadFolderPath保護リスト);
            string sfolderpath = Settings.Default.Download保護リストFolderPath;

            foreach (var f in fli)
            {
                string downloadFile = Settings.Default.UploadFolderPath保護リスト + "/" + f;
                if (!System.IO.Directory.Exists(sfolderpath))
                {
                    System.IO.Directory.CreateDirectory(sfolderpath);
                }
                string sfilepath = System.IO.Path.Combine(sfolderpath, f);
                DownloadCenter.DownloadFile(downloadFile, sfilepath);
            }

        }


        public static void DownloadTotal個別管理医薬品マスタCSV()
        {
            System.Net.FtpWebResponse ftpRes = null;
            try
            {

                ////ダウンロードするファイルのURI
                Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/発注支援システムマスタ/個別管理医薬品マスタ.csv");

                //ダウンロードしたファイルの保存先
                string saveFile = OASystem.Properties.Settings.Default.Download個別管理医薬品マスタFilePath;


                //FtpWebRequestの作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //ログインユーザー名とパスワードを設定
                ftpReq.Credentials = new System.Net.NetworkCredential("a10254880", "hxzn9jXQ");
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求の完了後に接続を閉じる
                ftpReq.KeepAlive = false;
                //ASCIIモードで転送する
                ftpReq.UseBinary = false;
                //PASSIVEモードを無効にする
                ftpReq.UsePassive = false;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void Setメーカー名リスト()
        {
            List<string> makerlist = new List<string>();
            using (IO.StreamReader sr = new IO.StreamReader(OASystem.Properties.Settings.Default.DownloadMEDIS_HOT13lFilePath, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    var sepa = line.Split(',');

                    //  6.JANコード
                    //  8.個別医薬品コード
                    //  9.レセプト電算コード
                    // 12.医薬品名
                    // 15.包装形態
                    // 17.包装単位
                    // 18.包装総量
                    // 21.製薬会社
                    // 22.販売会社

                    // 今後増える可能性もあるので現状の24以下とする
                    if (sepa.Length < 24)
                    {
                        throw new Exception("MEDIS_HOT13にインデックスが２４ではない不正なデータが含まれて降ります。\r\nCounter: " + counter);
                    }

                    string str = sepa[21].Replace("\"", "");
                    if (makerlist.Contains(str) == false)
                    {
                        makerlist.Add(str);
                    }
                }
            }


            var ivmaster = Model.DI.個別管理医薬品マスタ;
            foreach (var data in ivmaster)
            {
                if (makerlist.Contains(data.販売会社) == false)
                {
                    makerlist.Add(data.販売会社);
                }
            }


            makerlist = makerlist.Distinct().OrderBy(x => x).ToList();

            OASystem.Model.DI.メーカー名リスト = makerlist;
        }


        public static void FolderCheck()
        {
            if (!IO.Directory.Exists(Settings.Default.DownloadFilesPath))
            {
                IO.Directory.CreateDirectory(Settings.Default.DownloadFilesPath);
            }

            if (!IO.Directory.Exists(Settings.Default.Download保護リストFolderPath))
            {
                IO.Directory.CreateDirectory(Settings.Default.Download保護リストFolderPath);
                // 初回フォルダ作成時に、保護リストをダウンロードしてくれるように、LastWriteTimeをMinimumにしておく。
                IO.Directory.SetLastWriteTime(Settings.Default.Download保護リストFolderPath, new DateTime(1900, 01, 01));
            }

            if (!IO.Directory.Exists(Settings.Default.Download優先移動リストFolderPath))
            {
                IO.Directory.CreateDirectory(Settings.Default.Download優先移動リストFolderPath);
                // 初回フォルダ作成時に、保護リストをダウンロードしてくれるように、LastWriteTimeをMinimumにしておく。
                IO.Directory.SetLastWriteTime(Settings.Default.Download優先移動リストFolderPath, new DateTime(1900, 01, 01));
            }

        }

        /// <summary>
        /// サーバーデータダウンロード
        /// </summary>
        /// <param name="DoForce">強制ダウンロード</param>
        public static void RoutineDownload(bool DoForce, OASystem.View.Windows.OAStartProgressBarWindow pbw)
        {
            try
            {
                // 強制ダウンロード
                if (DoForce)
                {
                    DownloadCenter.DownloadTotal不動品CSV();
                    DownloadCenter.DownloadTotal現在庫CSV();
                    DownloadCenter.DownloadTotalMEDIS_HOT13TXT();
                    DownloadCenter.DownloadTotal帳合先マスタCSV();
                    DownloadCenter.DownloadTotal帳合先チェックマスタメーカー別CSV();
                    DownloadCenter.DownloadTotal帳合先チェックマスタ医薬品別CSV();
                    DownloadCenter.DownloadTotal個別管理医薬品マスタCSV();
                    DownloadCenter.DownloadTotal保護リスト();
                    DownloadCenter.DownloadTotal優先移動リスト();
                }
                // 起動時のダウンロード
                else
                {


                    // 不動品Total
                    if (IO.File.Exists(Settings.Default.Download不動品TotalFilePath))
                    {
                        DateTime lastdownloadday = IO.File.GetLastWriteTime(Settings.Default.Download不動品TotalFilePath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal不動品CSV();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal不動品CSV();
                    }

                    pbw.SetProgressBarValueAndText(55, null);


                    // 現在庫Total
                    if (IO.File.Exists(Settings.Default.Download現在庫TotalFilePath))
                    {
                        DateTime lastdownloadday = IO.File.GetLastWriteTime(Settings.Default.Download現在庫TotalFilePath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal現在庫CSV();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal現在庫CSV();
                    }


                    pbw.SetProgressBarValueAndText(60, null);



                    // MEDIS_HOT13
                    // MEDISは月１回のダウンロード
                    if (IO.File.Exists(Settings.Default.DownloadMEDIS_HOT13lFilePath))
                    {
                        DateTime lastdownloadday = IO.File.GetLastWriteTime(Settings.Default.DownloadMEDIS_HOT13lFilePath);

                        DateTime add1month = lastdownloadday.AddMonths(1);
                        // １ヶ月以上経っていたらダウンロードする
                        if (new DateTime(add1month.Year, add1month.Month, add1month.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotalMEDIS_HOT13TXT();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotalMEDIS_HOT13TXT();
                    }

                    pbw.SetProgressBarValueAndText(70, null);



                    // 帳合先マスタ
                    if (IO.File.Exists(Settings.Default.Download帳合先マスタFilePath))
                    {
                        DateTime lastdownloadday = IO.File.GetLastWriteTime(Settings.Default.Download帳合先マスタFilePath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal帳合先マスタCSV();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal帳合先マスタCSV();
                    }

                    pbw.SetProgressBarValueAndText(75, null);


                    // 帳合先チェックマスタ医薬品別
                    if (IO.File.Exists(Settings.Default.Download帳合先チェックマスタ医薬品別FilePath))
                    {
                        DateTime lastdownloadday = IO.File.GetLastWriteTime(Settings.Default.Download帳合先チェックマスタ医薬品別FilePath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal帳合先チェックマスタ医薬品別CSV();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal帳合先チェックマスタ医薬品別CSV();
                    }

                    pbw.SetProgressBarValueAndText(77, null);




                    // 帳合先チェックマスタメーカー別
                    if (IO.File.Exists(Settings.Default.Download帳合先チェックマスタメーカー別FilePath))
                    {
                        DateTime lastdownloadday = IO.File.GetLastWriteTime(Settings.Default.Download帳合先チェックマスタメーカー別FilePath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal帳合先チェックマスタメーカー別CSV();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal帳合先チェックマスタメーカー別CSV();
                    }


                    pbw.SetProgressBarValueAndText(80, null);



                    // 個別管理医薬品マスタ
                    if (IO.File.Exists(Settings.Default.Download個別管理医薬品マスタFilePath))
                    {
                        DateTime lastdownloadday = IO.File.GetLastWriteTime(Settings.Default.Download個別管理医薬品マスタFilePath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal個別管理医薬品マスタCSV();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal個別管理医薬品マスタCSV();
                    }


                    pbw.SetProgressBarValueAndText(82, null);



                    // 保護リストはLastWriteTimeはディレクトリを調べる
                    if (IO.Directory.Exists(Settings.Default.Download保護リストFolderPath))
                    {
                        DateTime lastdownloadday = IO.Directory.GetLastWriteTime(Settings.Default.Download保護リストFolderPath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal保護リスト();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal保護リスト();
                    }

                    pbw.SetProgressBarValueAndText(84, null);


                    // 優先移動リストはLastWriteTimeはディレクトリを調べる
                    if (IO.Directory.Exists(Settings.Default.Download優先移動リストFolderPath))
                    {
                        DateTime lastdownloadday = IO.Directory.GetLastWriteTime(Settings.Default.Download優先移動リストFolderPath);

                        // 最終ダウンロードが今日より前のデータならば
                        if (new DateTime(lastdownloadday.Year, lastdownloadday.Month, lastdownloadday.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            DownloadCenter.DownloadTotal優先移動リスト();
                        }
                    }
                    else
                    {
                        DownloadCenter.DownloadTotal優先移動リスト();
                    }

                    pbw.SetProgressBarValueAndText(85, null);


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




    }
}
