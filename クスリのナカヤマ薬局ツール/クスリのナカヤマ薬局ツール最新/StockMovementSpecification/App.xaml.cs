using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using クスリのナカヤマ薬局ツール.共通.Program;
using System.Diagnostics;
using System.IO;
using クスリのナカヤマ薬局ツール.共通.File;

namespace クスリのナカヤマ薬局ツール
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            // 二重起動チェック
            if (!ControlProgram.CheckMutexOperation(Process.GetCurrentProcess().ProcessName))
            {
                MessageBox.Show("すでに起動しています。", "確認", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
                return;
            }

            在庫HP更新ツールCenter.FolderCheck();
            if (!在庫HP更新ツールCenter.FileCheck())
            {
                this.Shutdown();
                return;
            }

            DateTime startDate = DateTime.Now;
            string savePath = Path.Combine(クスリのナカヤマ薬局ツール.Properties.Settings.Default.UpdateLocalFolder, startDate.ToString("yyyy.MM.dd.HHmmss"));

            try
            {
                if (クスリのナカヤマ薬局ツール.共通.Updater.UpdateCenter.StartUpdateProcess(savePath))
                {
                    this.Shutdown();
                    return;
                }
                else
                {
                    // Version.datだけのフォルダは消しておく。
                    if (Directory.Exists(savePath))
                    {
                        Directory.Delete(savePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("アップデート中にエラーが発生しました。\r\n以前のバージョンでプログラムを開始します。\r\nErrorMessage:{0}\r\nStackTrace:{1}", ex.Message, ex.StackTrace), "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            SettingsIniController.SetVersionNameToDI();


            App.Current.MainWindow = new MainWindow();
            App.Current.MainWindow.ShowDialog();


        }
    }
}
