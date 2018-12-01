using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Configuration;
using System.Net.Configuration;
using StockManagement.Const;
using StockManagement.Settings;
using StockManagement.ViewModel.DSException;

namespace StockManagement.ViewModel
{
    class FileDownloader
    {
        private List<string> _downloadList;
        public List<string> DownloadList
        {
            get { return _downloadList; }
            set { _downloadList = value; }
        }

        private DateTime _folderDate;

        public DateTime FolderDate
        {
            get { return _folderDate; }
            set { _folderDate = value; }
        }

        private int CompletedCount = 0;
        private int TotalCount = 0;
        private string _username;
        private string _confidencial;
        private string _savePath;
        private string _baseStoreName = null;
        private BackgroundWorker _controlledBackgroundWorker;

        public FileDownloader(BackgroundWorker controlledBackgroundWorker, DateTime folderDate, string baseStoreName)
        {
            FolderDate = folderDate;
            FolderCheck();
            _baseStoreName = baseStoreName;
            SetDownloadUriList();
            _controlledBackgroundWorker = controlledBackgroundWorker;

        }

        public bool CheckBackgroundWorkerCanncelation()
        {
            // バックグラウンドワーカーのキャンセル処理をチェック
            if (_controlledBackgroundWorker != null)
            {
                if (_controlledBackgroundWorker.CancellationPending)
                {
                    return false;
                }
            }

            return true;

        }


        /// <summary>
        /// ダウンロードする店舗名を設定
        /// </summary>
        private void SetDownloadUriList()
        {

            // 自店舗とデッド品管理対象店舗のみダウンロードする
            var ret = new List<string>();
            ret.Add(_baseStoreName); // 自店舗
            InitialData.DeadStockManagementStoresList.ForEach
                (
                     delegate(string x)
                     {
                         // 自店舗と重複しないようにする。
                         if (!ret.Contains(x))
                         {
                             ret.Add(x);
                         }
                     }
                    );
            _downloadList = ret; // 自店舗 + デッド品管理対象店舗
        }

        /// <summary>
        /// 
        /// </summary>
        public void FolderCheck()
        {
            if (!Directory.Exists(SMConst.rootFolder))
            {
                Directory.CreateDirectory(SMConst.rootFolder);
            }

            if (!Directory.Exists(SMConst.downloadFolder))
            {
                Directory.CreateDirectory(SMConst.downloadFolder);
            }

        }


        /// <summary>
        /// プログレスバーに表示する為の総ダウンロード数を計算
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public void TotalDownloadCountCalc(DateTime from, DateTime to)
        {

            int returnvalue = 0;

            foreach (var shop in _downloadList)
            {

                returnvalue++;

                for (DateTime d = from; d <= to; d = d.AddMonths(1))
                {
                    returnvalue++;
                }
            }

            TotalCount = returnvalue;

        }


        /// <summary>
        /// 使用量2.CSVのダウンロード (=表画面の使用量.CSV)
        /// </summary>
        /// <param name="downloadShopList"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public Exception StartDownloadUsedData2(List<string> downloadShopList, DateTime from, DateTime to)
        {
            downloadShopList = _downloadList;

            foreach (var shop in downloadShopList)
            {

                for (DateTime d = from; d <= to; d = d.AddMonths(1))
                {

                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");


                    System.IO.FileStream fs = null;
                    System.IO.Stream resStrm = null;
                    System.Net.FtpWebResponse ftpRes = null;

                    try
                    {
                        // ダウンロードするファイルのURI
                        Uri u = new Uri(StockManagement.Common.Settings.Ftp使用量2Path + "/" + shop + string.Format("/{0}年/{1}月.csv", d.Year, d.Month));
                        //Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/使用量2/" + shop + string.Format("/{0}年/{1}月.csv", d.Year, d.Month));


                        // ダウンロードしたファイルの保存先
                        string makeFoler = Path.Combine(SMConst.downloadFolder, FolderDate.ToString("yyyyMMddHHmmss"));
                        if (!Directory.Exists(makeFoler))
                            Directory.CreateDirectory(makeFoler);

                        string deadFoler = Path.Combine(makeFoler, StockManagement.Common.Settings.LocalUsageFolderName);

                        string shopFolder = Path.Combine(deadFoler, shop);
                        if (!Directory.Exists(shopFolder))
                            Directory.CreateDirectory(shopFolder);

                        string yearFolder = Path.Combine(shopFolder, string.Format("{0}年", d.Year));
                        if (!Directory.Exists(yearFolder))
                            Directory.CreateDirectory(yearFolder);

                        string downFile = Path.Combine(yearFolder, string.Format("{0}月.csv", d.Month));

                        System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(u);

                        ftpReq.Credentials = new System.Net.NetworkCredential(StockManagement.Common.Settings.FtpId, StockManagement.Common.Settings.FtpCredential);
                        ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                        ftpReq.KeepAlive = false;
                        ftpReq.UseBinary = false;
                        ftpReq.UsePassive = StockManagement.Common.Settings.UsePassive;
                        ftpReq.Timeout = 60000;

                        ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();

                        using (resStrm = ftpRes.GetResponseStream())
                        using (fs = new System.IO.FileStream(downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        {
                            //ダウンロードしたデータを書き込む
                            byte[] buffer = new byte[1024];
                            while (true)
                            {
                                // バックグラウンドワーカーのキャンセル処理をチェック
                                if (!CheckBackgroundWorkerCanncelation())
                                    return new DeadStockException(true, "処理を中断しました。");


                                int readSize = resStrm.Read(buffer, 0, buffer.Length);
                                if (readSize == 0)
                                    break;
                                fs.Write(buffer, 0, readSize);
                            }
                        }

                        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription));

                        CompletedCount++;
                        _controlledBackgroundWorker.ReportProgress(CompletedCount * 100 / TotalCount);


                    }
                    catch (Exception ex)
                    {
                        //エラー発生より先に中断ボタンが押されたら、中断として処理。
                        // バックグラウンドワーカーのキャンセル処理をチェック
                        if (!CheckBackgroundWorkerCanncelation())
                            return new DeadStockException(true, "処理を中断しました。");


                        // ファイルがない場合でも使用量は継続する
                        if (ex.Message.Contains("リモート サーバーがエラーを返しました: (550) ファイルが使用できません (例: ファイルが見つからない、ファイルへのアクセスがない)"))
                        {
                            continue;
                        }


                        return new Exception(string.Format("使用量データをダウンロード中にエラーが発生した為、処理を中断しました。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));
                    }
                    finally
                    {
                        try
                        {
                            if (ftpRes != null)
                                ftpRes.Close();
                        }
                        catch (Exception ex)
                        {
                            //エラー発生より先に中断ボタンが押されたら、中断として処理。
                            // バックグラウンドワーカーのキャンセル処理をチェック
                            if (!CheckBackgroundWorkerCanncelation())
                                throw new DeadStockException(true, "処理を中断しました。");

                            throw new Exception(string.Format("使用量データをダウンロード中にエラーが発生した為、処理を中断しました。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));
                        }
                    }
                }

            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Exception StartDownloadDeadStockData()
        {
            System.Net.FtpWebResponse ftpRes = null;

            try
            {

                foreach (var shop in _downloadList)
                {

                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");

                    // ダウンロードするファイルのURI
                    Uri u = new Uri(StockManagement.Common.Settings.Ftp不動品Path + "/" + shop + "/" + StockManagement.Common.Settings.LocalDeadStockCsvFileName);
                    //Uri u = new Uri("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/不動品/" + shop + "/不動品データ.csv");

                    // ダウンロードしたファイルの保存先
                    string makeFoler = Path.Combine(SMConst.downloadFolder, FolderDate.ToString("yyyyMMddHHmmss"));
                    if (!Directory.Exists(makeFoler))
                    {
                        Directory.CreateDirectory(makeFoler);
                    }

                    string deadFoler = Path.Combine(makeFoler, StockManagement.Common.Settings.LocalDeadStockFolderName);
                    string shopFolder = Path.Combine(deadFoler, shop);
                    if (!Directory.Exists(shopFolder))
                    {
                        Directory.CreateDirectory(shopFolder);
                    }

                    string downFile = Path.Combine(shopFolder, StockManagement.Common.Settings.LocalDeadStockCsvFileName);

                    System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(u);
                    ftpReq.Credentials = new System.Net.NetworkCredential(StockManagement.Common.Settings.FtpId, StockManagement.Common.Settings.FtpCredential);
                    ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                    ftpReq.KeepAlive = false;
                    ftpReq.UseBinary = false;
                    ftpReq.UsePassive = StockManagement.Common.Settings.UsePassive;
                    ftpReq.Timeout = 60000;
                    ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();

                    using (System.IO.Stream resStrm = ftpRes.GetResponseStream()) //ファイルをダウンロードするためのStreamを取得
                    using (System.IO.FileStream fs = new System.IO.FileStream(downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))   //ダウンロードしたファイルを書き込むためのFileStreamを作成
                    {
                        //ダウンロードしたデータを書き込む
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            // バックグラウンドワーカーのキャンセル処理をチェック
                            if (!CheckBackgroundWorkerCanncelation())
                                return new DeadStockException(true, "処理を中断しました。");


                            int readSize = resStrm.Read(buffer, 0, buffer.Length);
                            if (readSize == 0)
                                break;
                            fs.Write(buffer, 0, readSize);
                        }
                    }

                    //FTPサーバーから送信されたステータスを表示
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription));

                    CompletedCount++;
                    _controlledBackgroundWorker.ReportProgress(CompletedCount * 100 / TotalCount);

                }

            }
            catch (Exception ex)
            {
                //エラー発生より先に中断ボタンが押されたら、中断として処理。
                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return new DeadStockException(true, "処理を中断しました。");

                // 不動品データの場合はデータがない場合は処理を中断する。
                if (ex.Message.Contains("リモート サーバーがエラーを返しました: (550) ファイルが使用できません (例: ファイルが見つからない、ファイルへのアクセスがない)"))
                    return new Exception(string.Format("不動品データが存在しない為、ダウンロードに失敗しました。処理を中断します。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));

                return new Exception(string.Format("不動品データをダウンロード中にエラーが発生した為、処理を中断しました。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));

            }
            finally
            {
                try
                {
                    //閉じる
                    if (ftpRes != null)
                        ftpRes.Close();
                }
                catch (Exception ex)
                {
                    //エラー発生より先に中断ボタンが押されたら、中断として処理。
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        throw new DeadStockException(true, "処理を中断しました。");


                    throw new Exception(string.Format("不動品データをダウンロード中にエラーが発生した為、処理を中断しました。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));

                }
            }


            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseshopname"></param>
        /// <returns></returns>
        public Exception StartDownloadStockAllData(string baseshopname)
        {
            System.Net.FtpWebResponse ftpRes = null;
            try
            {

                // ダウンロードするファイルのURI
                Uri u = new Uri(string.Format(StockManagement.Common.Settings.Ftp現在庫Path + "/{0}/" + StockManagement.Common.Settings.LocalStockCsvFileName, baseshopname));
                //Uri u = new Uri(string.Format("ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/現在庫/{0}/現在庫データ.csv", baseshopname));

                //ダウンロードしたファイルの保存先
                string makeFolder = Path.Combine(SMConst.downloadFolder, FolderDate.ToString("yyyyMMddHHmmss"));

                if (Directory.Exists(makeFolder) == false)
                    Directory.CreateDirectory(makeFolder);

                string makeFolder2 = Path.Combine(makeFolder, StockManagement.Common.Settings.LocalStockFolderName);
                if (Directory.Exists(makeFolder2) == false)
                    Directory.CreateDirectory(makeFolder2);

                //ダウンロードしたファイルの保存先
                string saveFile = Path.Combine(makeFolder2, StockManagement.Common.Settings.LocalStockCsvFileName);

                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(u);
                ftpReq.Credentials = new System.Net.NetworkCredential(StockManagement.Common.Settings.FtpId, StockManagement.Common.Settings.FtpCredential);
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                ftpReq.KeepAlive = false;
                ftpReq.UseBinary = false;
                ftpReq.UsePassive = StockManagement.Common.Settings.UsePassive;
                ftpReq.Timeout = 60000;
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();

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
            }
            catch (Exception ex)
            {
                //エラー発生より先に中断ボタンが押されたら、中断として処理。
                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return new DeadStockException(true, "処理を中断しました。");

                // 不動品データの場合はデータがない場合は処理を中断する。
                if (ex.Message.Contains("リモート サーバーがエラーを返しました: (550) ファイルが使用できません (例: ファイルが見つからない、ファイルへのアクセスがない)"))
                    return new Exception(string.Format("現在庫データが存在しない為、ダウンロードに失敗しました。処理を中断します。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));

                return new Exception(string.Format("現在庫データをダウンロード中にエラーが発生した為、処理を中断しました。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));

            }
            finally
            {
                try
                {
                    //閉じる
                    if (ftpRes != null)
                        ftpRes.Close();
                }
                catch (Exception ex)
                {
                    //エラー発生より先に中断ボタンが押されたら、中断として処理。
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        throw new DeadStockException(true, "処理を中断しました。");

                    throw new Exception(string.Format("現在庫データをダウンロード中にエラーが発生した為、処理を中断しました。\r\nエラーメッセージ:{0}\r\nスタックトレース:{1}", ex.Message, ex.StackTrace));
                }
            }

            return null;
        }

    }
}
