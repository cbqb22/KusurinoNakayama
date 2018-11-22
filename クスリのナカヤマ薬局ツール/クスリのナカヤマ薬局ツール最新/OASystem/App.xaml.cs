using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using OASystem.Properties;
using OASystem.ViewModel.File;
using OASystem.ViewModel.Common.Program;
using OASystem.View.Windows;
using OASystem.Model.Entity;

namespace OASystem
{

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {

        private readonly string ErrorLogPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "OASystemError.log");

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            //プログレスバーの表示
            OAStartProgressBarWindow pbw = new View.Windows.OAStartProgressBarWindow();
            pbw.Show();

            pbw.SetProgressBarValueAndText(10, "発注支援システムのバージョンチェック中");


            // 二重起動チェック
            string s = "";
            if (!ControlProgram.CheckMutexOperation(s = Process.GetCurrentProcess().ProcessName))
            {
                MessageBox.Show("すでに起動しています。", "確認", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
                return;
            }


            DateTime startDate = DateTime.Now;
            string savePath = Path.Combine(OASystem.Common.Settings.UpdateLocalFolder, startDate.ToString("yyyy.MM.dd.HHmmss"));

            try
            {

                if (OASystem.ViewModel.Updater.UpdateCenter.StartUpdateProcess(savePath))
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
                MessageBox.Show(string.Format("発注支援システムのアップデート中にエラーが発生しました。\r\n以前のバージョンでプログラムを開始します。\r\n\r\n時間をおいて、再起動してください。\r\nErrorMessage:{0}\r\nStackTrace:{1}", ex.Message, ex.StackTrace), "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
            //int i = 0;
            //do
            //{
            //    i++;
            //    SetProgressBarValueAndText(pbw, i, "ループ中");
            //}
            //while (i < 100);




            pbw.SetProgressBarValueAndText( 20, "発注支援システムのフォルダをチェック中");


            OASystemCenter.FolderCheck();
            pbw.SetProgressBarValueAndText( 25, null);

            OrderLogsFileManagement.FolderCheck();
            pbw.SetProgressBarValueAndText(30, null);

            DownloadCenter.FolderCheck();
            pbw.SetProgressBarValueAndText(40, null);

            // RoutineDownloadの前にやらないと、ファイアーウォール解除しますかの文字が出たときに応答前にセットされずにメニューにいってしまう。
            SettingsIniController.SetVersionNameToDI();

            try
            {

                pbw.SetProgressBarValueAndText(50, "発注支援システムのデータをダウンロード中");

                //DownloadCenter.ProgressEvent += CallBackProgress;

                DownloadCenter.RoutineDownload(false,pbw);

                DownloadCenter.Setメーカー名リスト();

                pbw.SetProgressBarValueAndText(90, null);



            }
            catch (Exception ex)
            {
                MessageBox.Show("サーバーからファイル取得中にエラーが発生しました。\n\rマスタデータが最新に更新されておりません。\r\n時間をおいて、再起動してください。" + ex.Message + ex.StackTrace);
            }

            pbw.SetProgressBarValueAndText(95, "発注支援システムプログラムを起動中");
            SettingsIniController.DoLoad();


            pbw.SetProgressBarValueAndText(100, "完了");

            pbw.Topmost = false;

            //先にshowしないと、初めのwindowをcloseした瞬間に終了してしまう
            App.Current.MainWindow = new Menus();
            App.Current.MainWindow.Show();

            //プログレスバー非表示
            pbw.Close();


        }

        /// <summary>
        /// 進捗のコールバック
        /// </summary>
        /// <param name="percentage"></param>
        public void CallBackProgress(int percentage)
        {
            System.Diagnostics.Debug.WriteLine(percentage);
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                var dispMessage = string.Format("プログラムを実行中にエラーが発生しました。プログラムを終了します。\r\n\r\nエラーメッセージ:{0}", e.Exception.Message);
                var outputMessage = string.Format("プログラムを実行中にエラーが発生しました。プログラムを終了します。\r\n\r\nエラーメッセージ:{0} \r\n StackTrace:{1}", e.Exception.Message, e.Exception.StackTrace);
                MessageBox.Show(dispMessage, "エラー",MessageBoxButton.OK, MessageBoxImage.Error);
                OutputErrorLog(outputMessage);

                Environment.Exit(1);
            }
        }

        private void OutputErrorLog(string errorMessage)
        {
            using (var stream = new StreamWriter(ErrorLogPath, true, System.Text.Encoding.GetEncoding(932)))
            {
                stream.WriteLine(errorMessage);
            }
        }

        ///// <summary>
        ///// 起動前のプログレスバーをUIを更新する
        ///// </summary>
        ///// <param name="pbw"></param>
        ///// <param name="value"></param>
        ///// <param name="operationtext"></param>
        //private void SetProgressBarValueAndText(OAStartProgressBarWindow pbw,double value,string operationtext)
        //{

        //    if (string.IsNullOrEmpty(operationtext) == false)
        //    {
        //        pbw.oaspb.tbOperationIndicator.Text = operationtext;
        //    }

        //    pbw.oaspb.tbProgressPercentage.Text = value.ToString();
        //    pbw.oaspb.pbOAStarting.Value = value;
        //    pbw.oaspb.pbOAStarting.Refresh(); //UI更新
        //    Thread.Sleep(100);

        //}


    }

    ///// <summary>
    ///// 拡張メソッド
    ///// </summary>
    //public static class ExtensionMethods
    //{

    //    /// <summary>
    //    /// UIを更新する
    //    /// </summary>
    //    private static Action EmptyDelegate = delegate() { };
    //    public static void Refresh(this UIElement uiElement)
    //    {
    //        //uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
    //        uiElement.Dispatcher.Invoke(DispatcherPriority.Input, EmptyDelegate);
    //    }
    //}

}
