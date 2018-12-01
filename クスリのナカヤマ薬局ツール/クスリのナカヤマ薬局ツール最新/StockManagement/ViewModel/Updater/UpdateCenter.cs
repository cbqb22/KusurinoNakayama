using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIO = System.IO;
using System.Diagnostics;
using StockManagement.ViewModel.File;
using StockManagement.Properties;


namespace StockManagement.ViewModel.Updater
{
    public static class UpdateCenter
    {
        public static bool StartUpdateProcess(string savePath)
        {
            //WPFToolKit.dllをアップデート中に上書きすると、使用中のプロセスでエラーになる。
            //原因は不明だが、WPFToolKit.dllを更新することはない。

            //Version.datのコピーを取る
            string tempversiondatpath = SIO.Path.Combine(StockManagement.Const.SMConst.rootFolder, StockManagement.Common.Settings.TempVersionFileName);
            System.IO.File.Copy(StockManagement.Const.SMConst.VersionDatLocalPath,tempversiondatpath, true);

            if (!SIO.Directory.Exists(savePath))
            {
                SIO.Directory.CreateDirectory(savePath);
            }

            string downloadVersionDatLocalPath = SIO.Path.Combine(savePath, StockManagement.Common.Settings.VersionFileName);
            List<string> needApplyVersionList = new List<string>();

            try
            {
                // UpdateCheck
                needApplyVersionList = UpdateCheck(savePath, downloadVersionDatLocalPath);

                if (needApplyVersionList.Count == 0)
                {
                    return false;
                }

                // PrepairUpdate
                string backUpFolderPath = PrepairUpdate();


                // DownloadUpdateFile
                if (!DownloadUpdateFile(needApplyVersionList, savePath))
                {
                    // ダウンロードファイル適用前は別アプリ名を戻す形でok
                    RollBackPrepair();
                    return false;
                }


                // ApplyDownloadFileToLocal
                if (!ApplyDownloadFileToLocal(savePath, downloadVersionDatLocalPath, needApplyVersionList))
                {
                    // 適用後なので、バックアップから元にもどす。
                    // とりあえず、ユーザーへメッセージにする？
                    // Updateに保存したフォルダを削除しておく。
                    SIO.Directory.Delete(savePath);
                    RollBackPrepair();
                    return false;
                }


                // StartNewModule
                if (!StartNewModule())
                {
                    RollBackPrepair();
                    return false;
                }



                // ClearOldProcess
                ClearOldProcess();


            }
            catch(Exception ex)
            {
                // とりあえずfalseで返しておく.
                RollBackPrepair();
                throw ex;
            }

            return true;
        }


        public static bool ApplyDownloadFileToLocal(string savePath, string downloadVersiondat, List<string> needApplyVersionList)
        {
            try
            {


                // ダウンロードしたファイルをコピー
                var dirs = SIO.Directory.GetDirectories(savePath);

                foreach (var dir in dirs)
                {
                    var files = SIO.Directory.GetFiles(dir);
                    var sepa0 = dir.Split('\\');
                    var folderVersionName = sepa0[sepa0.Count() - 1];

                    //適用が必要なバージョン名でない場合はスルー
                    if (!needApplyVersionList.Contains(folderVersionName))
                    {
                        continue;
                    }

                    foreach (var file in files)
                    {

                        var sepa = file.Split('\\');
                        var filename = sepa[sepa.Count() - 1];

                        System.IO.File.Copy(file, SIO.Path.Combine(StockManagement.Const.SMConst.rootFolder, filename), true);
                    }
                }

                // Version.datを適用
                System.IO.File.Copy(downloadVersiondat, SIO.Path.Combine(StockManagement.Const.SMConst.rootFolder, StockManagement.Common.Settings.VersionFileName), true);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public static List<string> UpdateCheck(string savePath, string downloadVersionDatLocalPath)
        {
            List<string> applyVersions = new List<string>();
            List<string> serverVersions = new List<string>();
            List<string> needApply = new List<string>();

            try
            {
                DownloadCenter.DownloadFile(StockManagement.Common.Settings.VersionDatServerPath, downloadVersionDatLocalPath);

                using (SIO.StreamReader sr = new SIO.StreamReader(downloadVersionDatLocalPath, Encoding.GetEncoding(932)))
                using (SIO.StreamReader sr2 = new SIO.StreamReader(StockManagement.Const.SMConst.VersionDatLocalPath, Encoding.GetEncoding(932)))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        serverVersions.Add(line);
                    }


                    string line2 = string.Empty;
                    while ((line2 = sr2.ReadLine()) != null)
                    {
                        applyVersions.Add(line2);
                    }
                }

                foreach (var serverVersion in serverVersions)
                {
                    if (applyVersions.Contains(serverVersion))
                    {
                        continue;
                    }

                    needApply.Add(serverVersion);
                }

                return needApply;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string PrepairUpdate()
        {
            string bkPath = null;
            try
            {
                // バックアップの作成
                if (!System.IO.Directory.Exists(StockManagement.Const.SMConst.StockManagementBackUpPath))
                {
                    System.IO.Directory.CreateDirectory(StockManagement.Const.SMConst.StockManagementBackUpPath);
                }
                bkPath = System.IO.Path.Combine(StockManagement.Const.SMConst.StockManagementBackUpPath, DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss"));
                File.GeneralMethods.CopyDirectory(StockManagement.Const.SMConst.rootFolder, bkPath);

                // 一旦、別アプリへ名前変更
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                System.IO.File.Delete(System.IO.Path.Combine(baseDir, StockManagement.Common.Settings.OldExeName));
                System.IO.File.Move(System.IO.Path.Combine(baseDir, StockManagement.Common.Settings.ExeName), System.IO.Path.Combine(baseDir, StockManagement.Common.Settings.OldExeName));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bkPath;
        }

        public static void RollBackPrepair()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            System.IO.File.Delete(System.IO.Path.Combine(baseDir, StockManagement.Common.Settings.ExeName));
            System.IO.File.Move(System.IO.Path.Combine(baseDir, StockManagement.Common.Settings.OldExeName), System.IO.Path.Combine(baseDir, StockManagement.Common.Settings.ExeName));

            //もとのVersion.datへもどす
            string tempversiondatpath = SIO.Path.Combine(StockManagement.Const.SMConst.rootFolder, StockManagement.Common.Settings.TempVersionFileName);
            System.IO.File.Copy(tempversiondatpath, StockManagement.Const.SMConst.VersionDatLocalPath, true);



        }

        public static bool DownloadUpdateFile(List<string> needApplyVersionList, string savePath)
        {
            try
            {
                List<string> fileListInfo = DownloadCenter.DownloadFileList(StockManagement.Common.Settings.UpdateFolderServerPath);
                if (fileListInfo.Count == 0)
                {
                    throw new Exception("サーバーStockManagementのUpdateフォルダー内が空です。");
                }

                foreach (var folder in fileListInfo)
                {
                    // datファイルは飛ばす
                    if (folder == StockManagement.Common.Settings.VersionFileName)
                        continue;

                    // 警告ファイルも飛ばす
                    if (folder == StockManagement.Common.Settings.SuggestFileName)
                        continue;

                    // それ以外はフォルダしかないことが前提
                    string downloadFolder = StockManagement.Common.Settings.UpdateFolderServerPath + "/" + folder;
                    List<string> fli = DownloadCenter.DownloadFileList(downloadFolder);

                    foreach (var f in fli)
                    {
                        string downloadFile = downloadFolder + "/" + f;
                        string sfolderpath = System.IO.Path.Combine(savePath, folder);
                        if (!SIO.Directory.Exists(sfolderpath))
                        {
                            SIO.Directory.CreateDirectory(sfolderpath);
                        }
                        string sfilepath = System.IO.Path.Combine(sfolderpath, f);
                        DownloadCenter.DownloadFile(downloadFile, sfilepath);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }


            return true;
        }


        /// <summary>
        /// ダウンロードした新しいModuleを起動
        /// </summary>
        /// <returns>Processを起動できなかったらfalse</returns>
        public static bool StartNewModule()
        {
            try
            {
                var proc = Process.Start(SIO.Path.Combine(StockManagement.Const.SMConst.rootFolder, StockManagement.Common.Settings.ExeName), "/up " + Process.GetCurrentProcess().Id);
                if (proc == null)
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public static void ClearOldProcess()
        {

            if (Environment.CommandLine.IndexOf("/up", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                try
                {
                    string[] args = Environment.GetCommandLineArgs();
                    int pid = Convert.ToInt32(args[2]);
                    Process.GetProcessById(pid).Kill();    // 終了待ち
                }
                catch (Exception)
                {
                }
                finally
                {
                    try
                    {
                        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        System.IO.File.Delete(System.IO.Path.Combine(baseDir, StockManagement.Common.Settings.OldExeName));
                    }
                    catch
                    {
                    }
                }
            }
        }

    }
}
