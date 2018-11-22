using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using OASystem.ViewModel.File;
using OASystem.ViewModel.OrderCenter;
using OASystem.ViewModel.Common.Printer;
using OASystem.Model.Entity;
using OASystem.ViewModel.Common.Program;

namespace OASystem.View.Windows
{
    /// <summary>
    /// Menus.xaml の相互作用ロジック
    /// </summary>
    public partial class Menus : Window
    {
        public Menus()
        {
            InitializeComponent();
            SetInit();
            this.Closed += new EventHandler(Menus_Closed);
            this.Loaded += new RoutedEventHandler(Menus_Loaded);
        }

        void Menus_Loaded(object sender, RoutedEventArgs e)
        {
            //最初から表示しておく。
            var stb = (this.Resources["stbButtonDisplay"] as Storyboard);
            if (stb != null)
            {
                stb.Begin();
            }
        }

        void SetInit()
        {
            tblVersion.Text = string.Format("Version {0}", OASystem.Model.DI.VersionName);
        }

        void Menus_Closed(object sender, EventArgs e)
        {
            //ControlProgram.EndProgram(OASystem.Common.Settings.OASystemProcessName);
            ControlProgram.EndProgram(Process.GetCurrentProcess().ProcessName);
        }

        private void btn発注書作成_Click(object sender, RoutedEventArgs e)
        {
            // すでに開いていたら再利用
            if (SingletonWindows.CheckHasOpenOwnWindow(typeof(OrderCenter)))
            {
                SingletonWindows.OrderCenterWindow.Show();
                SingletonWindows.OrderCenterWindow.Activate();
            }
            else
            {
                var sendList = SEND01DATAnalyzer.DoAnalyze(OASystem.Common.Settings.SENDO1DATFilePath);
                if (sendList.Count == 0)
                {
                    MessageBox.Show("発注する医薬品がありません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }


                List<BalancingAccountsCheckResultEntity> result = null;
                if (MessageBox.Show("帳合先のチェックを行いますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    result = CheckExpAndDead.CheckBalancingAccounts(sendList);
                    BalancingAccountsCheckResult bacr = new BalancingAccountsCheckResult();
                    bacr.lvBACheckResult.ItemsSource = result;
                    bacr.ShowDialog();

                    if (MessageBox.Show("続けて発注書作成画面を開きますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    {
                        return;
                    }

                }

                // 閉じて、新しく作り直す
                SingletonWindows.OrderCenterWindow.Close();

                CheckExpAndDead.SetOrderData(SingletonWindows.OrderCenterWindow, sendList, result);
                SingletonWindows.OrderCenterWindow.Show();
                SingletonWindows.OrderCenterWindow.Activate();
                if (((List<OrderScheduledListEntity>)SingletonWindows.OrderCenterWindow.lvOrderDeadAndExpWithBalancingAccounts.ItemsSource).Count == 0)
                {
                    MessageBox.Show("他店に発注できる医薬品がありませんでした。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        private void btn発注履歴_Click(object sender, RoutedEventArgs e)
        {
            if (SingletonWindows.CheckHasOpenWindow(typeof(OrderLogs)))
            {
                SingletonWindows.OrderLogsWindow.Show();
                SingletonWindows.OrderLogsWindow.Activate();
            }
            else
            {
                // 閉じて、新しく作り直す
                SingletonWindows.OrderLogsWindow.Close();
                SingletonWindows.OrderLogsWindow.Show();
                SingletonWindows.OrderLogsWindow.Activate();
            }
        }

        private void btn帳合先チェック登録_Click(object sender, RoutedEventArgs e)
        {
            if (SingletonWindows.CheckHasOpenWindow(typeof(BalancingAccountsCheckRegister)))
            {
                MessageBox.Show("他のウィンドウが開いております。登録する前に全て閉じて下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SingletonWindows.BalancingAccountsCheckRegisterWindow.IsLoaded)
            {
                SingletonWindows.BalancingAccountsCheckRegisterWindow.Show();
                SingletonWindows.BalancingAccountsCheckRegisterWindow.Activate();
            }
            else
            {

                LoginCheck lc = new LoginCheck();
                lc.ShowDialog();

                if (lc.LoginSuccess)
                {
                    // 閉じて、新しく作り直す
                    SingletonWindows.BalancingAccountsCheckRegisterWindow.Close();
                    SingletonWindows.BalancingAccountsCheckRegisterWindow.Show();
                    SingletonWindows.BalancingAccountsCheckRegisterWindow.Activate();
                }
            }

        }

        private void btnマスタ登録_Click(object sender, RoutedEventArgs e)
        {
            if (SingletonWindows.CheckHasOpenWindow(typeof(MasterDataManagement)))
            {
                MessageBox.Show("他のウィンドウが開いております。登録する前に全て閉じて下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SingletonWindows.MasterDataManagementWindow.IsLoaded)
            {
                SingletonWindows.MasterDataManagementWindow.Show();
                SingletonWindows.MasterDataManagementWindow.Activate();
            }
            else
            {
                LoginCheck lc = new LoginCheck();
                lc.ShowDialog();

                if (lc.LoginSuccess)
                {
                    // 閉じて、新しく作り直す
                    SingletonWindows.MasterDataManagementWindow.Close();
                    SingletonWindows.MasterDataManagementWindow.Show();
                    SingletonWindows.MasterDataManagementWindow.Activate();
                }
            }
        }

        private void btn設定_Click(object sender, RoutedEventArgs e)
        {
            if (SingletonWindows.CheckHasOpenWindow(typeof(Settings)))
            {
                MessageBox.Show("他のウィンドウが開いております。登録する前に全て閉じて下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // 閉じて、新しく作り直す
            SingletonWindows.SettingsWindow.Close();
            SingletonWindows.SettingsWindow.Show();
            SingletonWindows.SettingsWindow.Activate(); ;
        }

        private void btnデータ手動更新_Click(object sender, RoutedEventArgs e)
        {
            if (SingletonWindows.CheckHasOpenWindow())
            {
                MessageBox.Show("データ更新する前に開いている他のウィンドウを閉じてください。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //プログレスバーの表示
            var pbw = new View.Windows.OAStartProgressBarWindow();

            try
            {

                pbw.Show();
                pbw.SetProgressBarValueAndText(10, "データ更新中");


                OASystemCenter.FolderCheck();
                OrderLogsFileManagement.FolderCheck();
                DownloadCenter.FolderCheck();
                pbw.SetProgressBarValueAndText(30);

                try
                {
                    DownloadCenter.FolderCheck();
                    pbw.SetProgressBarValueAndText(40);

                    DownloadCenter.RoutineDownload(true, null);
                    pbw.SetProgressBarValueAndText(80);

                    DownloadCenter.Setメーカー名リスト();
                    pbw.SetProgressBarValueAndText(90);
                }
                catch (Exception ex)
                {
                    throw new Exception("サーバーからファイル取得中にエラーが発生しました。" + ex.Message + ex.StackTrace);
                }

                pbw.SetProgressBarValueAndText(100);


                MessageBox.Show("データ更新が正常に完了しました。\r\n\r\n設定を反映するには、プログラムを再起動してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("データ更新中にエラーが発生しました。処理を中断します。\r\n\r\nエラーメッセージ:" + ex.Message + "\r\n\r\nStackTrace:" + ex.StackTrace, "エラー" , MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                pbw.Topmost = false;
                pbw.Close();
            }
        }

        private void btn発注医薬品管理_Click(object sender, RoutedEventArgs e)
        {
            if (SingletonWindows.CheckHasOpenWindow(typeof(OrderMedicineManagement)))
            {
                MessageBox.Show("他のウィンドウが開いております。登録する前に全て閉じて下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SingletonWindows.OrderMedicineManagementWindow.IsLoaded)
            {
                SingletonWindows.OrderMedicineManagementWindow.Show();
                SingletonWindows.OrderMedicineManagementWindow.Activate();
            }
            else
            {
                // 閉じて、新しく作り直す
                SingletonWindows.OrderMedicineManagementWindow.Close();
                SingletonWindows.OrderMedicineManagementWindow.Show();
                SingletonWindows.OrderMedicineManagementWindow.Activate();
            }

        }


        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            var stb = (this.Resources["stbButtonDisplay"] as Storyboard);
            if (stb != null)
            {
                stb.Begin();
            }

        }

        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            var stb = (this.Resources["stbButtonHide"] as Storyboard);
            if (stb != null)
            {
                stb.Begin();
            }
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMediWebOpen_Click(object sender, RoutedEventArgs e)
        {
            //var str = ControlProgram.StartProgram(OASystem.Common.Settings.MediWebProgramPath, OASystem.Common.Settings.MediWebProcessName);
            if (OASystem.Model.DI.MEDICODEWebSRFIlePath == null)
            {
                MessageBox.Show("MEDICODE-WebSRのパスが設定されてません。設定画面より設定してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var str = ControlProgram.StartProgram(OASystem.Model.DI.MEDICODEWebSRFIlePath, OASystem.Common.Settings.MediWebProcessName);
            if (str != null)
            {
                MessageBox.Show(str, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMEDISUpdate_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.Filter =
                "TXTファイル(*.TXT)|*.TXT|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "MEDIS(日付).TXTを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;


            //ダイアログを表示する
            while (ofd.ShowDialog() == true)
            {

                string filefullname = ofd.FileName;
                var sepa0 = filefullname.Split('\\');
                if (sepa0.Length < 2)
                {
                    return;
                }

                string filename = sepa0[sepa0.Length - 1];

                var pattern = @"MEDIS\d{8}.TXT"; //ex) MEDIS20140513.TXT
                var matches = Regex.Matches(filename, pattern);

                if (matches.Count != 1)
                {
                    MessageBox.Show("MEDIS(日付).TXTを選択して下さい。\r\n例) MEDIS20140531.TXT", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    continue;
                }


                using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding(932)))
                {
                    int counter = 0;
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        var sepa = line.Split(',');
                        counter++;
                        if (counter == 1)
                        {
                            // 2014/05/31の時点で24項目
                            if (sepa.Length < 24)
                            {
                                MessageBox.Show("MEDIS(日付).TXTのヘッダーが24項目以下です。\r\n選択されたファイルのデータが正しいか確認してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                return;
                            }
                            continue;
                        }
                    }
                }

                if (MessageBox.Show(string.Format("{0}をサーバーへアップロードします。\r\n完了までにしばらく時間がかかります。\r\n\r\n宜しいですか？", filename), "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                {
                    return;
                }

                try
                {
                    UploadCenter.UploadMEDIS_HOT13(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("{0}をアップロード出来ませんでした。\r\nErrorMassage:{1},\r\nStackTrace:{2}", filename, ex.Message, ex.StackTrace), "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show(string.Format("{0}を更新しました。", filename), "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
        }
    }
}
