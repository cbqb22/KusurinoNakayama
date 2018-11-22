using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIO = System.IO;
using System.Diagnostics;
using OASystem.ViewModel.File;
using OASystem.Properties;
using OASystem.Common;


namespace OASystem.ViewModel.Updater
{
    public static class UpdateCenter
    {
        public static bool StartUpdateProcess(string savePath)
        {
            if (!SIO.Directory.Exists(savePath))
            {
                SIO.Directory.CreateDirectory(savePath);
            }

            string downloadVersionDatLocalPath = SIO.Path.Combine(savePath, "Version.dat");
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
                if (!ApplyDownloadFileToLocal(savePath, downloadVersionDatLocalPath))
                {
                    // 適用後なので、バックアップから元にもどす。
                    // とりあえず、ユーザーへメッセージにする？
                    // Updateに保存したフォルダを削除しておく。
                    SIO.Directory.Delete(savePath);
                    return false;
                }

                // StartNewModule
                if (!StartNewModule())
                {
                    return false;
                }

                // ClearOldProcess
                ClearOldProcess();
            }
            catch(Exception ex)
            {
                // とりあえずfalseで返しておく.
                throw ex;
            }

            return true;
        }


        public static bool ApplyDownloadFileToLocal(string savePath, string downloadVersiondat)
        {
            try
            {
                // ダウンロードしたファイルをコピー
                var dirs = SIO.Directory.GetDirectories(savePath);

                foreach (var dir in dirs)
                {
                    var files = SIO.Directory.GetFiles(dir);

                    foreach (var file in files)
                    {
                        var sepa = file.Split('\\');
                        var filename = sepa[sepa.Count() - 1];
                        System.IO.File.Copy(file, SIO.Path.Combine(Settings.OASystemRootPath, filename), true);
                    }
                }

                // Version.datを適用
                System.IO.File.Copy(downloadVersiondat, SIO.Path.Combine(Settings.OASystemRootPath, "Version.dat"), true);

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
                DownloadCenter.DownloadFile(OASystem.Common.Settings.VersionDatServerPath, downloadVersionDatLocalPath);

                using (SIO.StreamReader sr = new SIO.StreamReader(downloadVersionDatLocalPath, Encoding.GetEncoding(932)))
                using (SIO.StreamReader sr2 = new SIO.StreamReader(OASystem.Common.Settings.VersionDatLocalPath, Encoding.GetEncoding(932)))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        serverVersions.Add(line);
                    }


                    string line2 = "";
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
                if (!System.IO.Directory.Exists(OASystem.Common.Settings.OASystemBackUpPath))
                {
                    System.IO.Directory.CreateDirectory(OASystem.Common.Settings.OASystemBackUpPath);
                }
                bkPath = System.IO.Path.Combine(OASystem.Common.Settings.OASystemBackUpPath, DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss"));
                File.GeneralMethods.CopyDirectory(OASystem.Common.Settings.OASystemRootPath, bkPath);

                // 一旦、別アプリへ名前変更
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                System.IO.File.Delete(System.IO.Path.Combine(baseDir, "OASystem.old"));
                //System.IO.File.Copy(System.IO.Path.Combine(baseDir, "OASystem.exe"), System.IO.Path.Combine(baseDir, "OASystem2.exe"));
                System.IO.File.Move(System.IO.Path.Combine(baseDir, "OASystem.exe"), System.IO.Path.Combine(baseDir, "OASystem.old"));
                //System.IO.File.Move(System.IO.Path.Combine(baseDir, "OASystem2.exe"), System.IO.Path.Combine(baseDir, "OASystem.exe"));
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
            System.IO.File.Delete(System.IO.Path.Combine(baseDir, "OASystem.exe"));
            System.IO.File.Move(System.IO.Path.Combine(baseDir, "OASystem.old"), System.IO.Path.Combine(baseDir, "OASystem.exe"));
        }

        public static bool DownloadUpdateFile(List<string> needApplyVersionList, string savePath)
        {
            try
            {
                List<string> fileListInfo = DownloadCenter.DownloadFileList(OASystem.Common.Settings.UpdateFolderServerPath);
                if (fileListInfo.Count == 0)
                {
                    throw new Exception("サーバーOASystemのUpdateフォルダー内が空です。");
                }

                foreach (var folder in fileListInfo)
                {
                    // datファイルは飛ばす
                    if (folder == "Version.dat")
                    {
                        continue;
                    }

                    // 警告ファイルも飛ばす
                    if (folder == "ここはdatとこのファイル以外は全てフォルダ.txt")
                    {
                        continue;
                    }

                    // それ以外はフォルダしかないことが前提

                    string downloadFolder = OASystem.Common.Settings.UpdateFolderServerPath + "/" + folder;
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
                var proc = Process.Start(SIO.Path.Combine(OASystem.Common.Settings.OASystemRootPath, "OASystem.exe"), "/up " + Process.GetCurrentProcess().Id);
                if (proc == null)
                {
                    return false;
                }
                //Process.Start("OASystem.exe", "/up " + Process.GetCurrentProcess().Id); //DEBUG用
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
                        System.IO.File.Delete(System.IO.Path.Combine(baseDir, "OASystem.old"));
                    }
                    catch
                    {
                    }
                }
            }
        }

    }
}
