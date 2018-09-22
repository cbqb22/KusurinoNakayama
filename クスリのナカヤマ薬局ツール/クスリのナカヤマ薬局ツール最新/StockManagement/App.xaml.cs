using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using System.Text;
using System.Diagnostics;
using StockManagement.Settings;
using StockManagement.ViewModel.IO;
using StockManagement.ViewModel.Common.Program;
using StockManagement.ViewModel.File;
using StockManagement.ViewModel.Common.MessageBox;


namespace StockManagement
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// アプリケーションが開始した際に呼び出される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // MessageBoxTopがエラーにならないように先にインスタンスをいれておく。
            // staticイニシャライザは非同期だとWindowにアクセスするとエラーになるので、やらない。
            this.MainWindow = new MainWindow();

            // 二重起動チェック
            if (!ControlProgram.CheckMutexOperation(Process.GetCurrentProcess().ProcessName))
            {
                MessageBoxTop.Show("すでに起動しています。", "確認", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
                return;
            }


            FolderController.FolderCheck();
            if (!FileController.FileCheck())
            {
                this.Shutdown();
                return;
            }


            ////ローカルコンピュータ上で実行されているすべてのプロセスを取得
            //System.Diagnostics.Process[] ps =
            //    System.Diagnostics.Process.GetProcesses();
            ////"machinename"という名前のコンピュータで実行されている
            ////すべてのプロセスを取得するには次のようにする。
            ////System.Diagnostics.Process[] ps =
            ////    System.Diagnostics.Process.GetProcesses("machinename");

            ////配列から1つずつ取り出す
            //foreach (System.Diagnostics.Process p in ps)
            //{
            //    try
            //    {
            //        //プロセス名を出力する
            //        Debug.WriteLine(string.Format("プロセス名: {0}", p.ProcessName));
            //        //ID
            //        Debug.WriteLine(string.Format("ID: {0}", p.Id));
            //        //メインモジュールのパス
            //        Debug.WriteLine(string.Format("ファイル名: {0}", p.MainModule.FileName));
            //        //合計プロセッサ時間
            //        Debug.WriteLine(string.Format("合計プロセッサ時間: {0}", p.TotalProcessorTime));
            //        //物理メモリ使用量
            //        Debug.WriteLine(string.Format("物理メモリ使用量: {0}", p.WorkingSet64));
            //        //.NET Framework 1.1以前では次のようにする
            //        //Console.WriteLine("物理メモリ使用量: {0}", p.WorkingSet);

            //        //Debug.WriteLine();
            //    }
            //    catch (Exception ex)
            //    {
            //       Debug.WriteLine(string.Format("エラー: {0}", ex.Message));
            //    }
            //}


            DateTime startDate = DateTime.Now;
            string savePath = Path.Combine(StockManagement.Const.SMConst.UpdateLocalFolder, startDate.ToString("yyyy.MM.dd.HHmmss"));

            try
            {
                if (StockManagement.ViewModel.Updater.UpdateCenter.StartUpdateProcess(savePath))
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
                MessageBoxTop.Show(string.Format("アップデート中にエラーが発生しました。\r\n以前のバージョンでプログラムを開始します。\r\nErrorMessage:{0}\r\nStackTrace:{1}", ex.Message, ex.StackTrace), "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            try
            {

                InitialData.LoadInitData();
                InitialData.SetFTPAppConfig();

                SettingsIniController.SetVersionNameToDI();


                this.MainWindow = new MainWindow();
                this.MainWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBoxTop.Show(ex.Message + ex.StackTrace);
                this.Shutdown();
                return;
            }


        }

    }
}
