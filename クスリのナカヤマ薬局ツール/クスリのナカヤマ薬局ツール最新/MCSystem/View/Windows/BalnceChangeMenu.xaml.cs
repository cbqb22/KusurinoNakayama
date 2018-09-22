using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using MCSystem.View.Windows;
using MCSystem.ViewModel;
using MCSystem.Model;
using MCSystem.ViewModel.Common.Program;
using MCSystem.ViewModel.Common.MessageBox;


namespace MCSystem.View.Windows
{
    /// <summary>
    /// BalnceChangeMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class BalanceChangeMenu : Window
    {

        private bool _IsRun帳合変更マクロ;

        public bool IsRun帳合変更マクロ
        {
            get { return _IsRun帳合変更マクロ; }
            set { _IsRun帳合変更マクロ = value; }
        }

        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        public BalanceChangeMenu()
        {
            InitializeComponent();
            this.Closed += new EventHandler(BalanceChangeMenu_Closed);

        }

        public void Init()
        {
        }


        void BalanceChangeMenu_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
            ControlProgram.EndProgram(Process.GetCurrentProcess().ProcessName);
        }

        private void btnMacroStart_Click(object sender, RoutedEventArgs e)
        {
            if (_IsRun帳合変更マクロ)
            {
                MessageBoxTop.Show("帳合変更マクロを実行中です。\r\n\r\n中断ボタンより作業を中止するか、\r\n終了するのをお待ちください。", "確認", MessageBoxButton.OK);
                return;
            }

            DateTime starttime = DateTime.Now;

            // 設定データを読み込む
            var settings = ControlBCSettings.LoadSettingsFromFile();

            if (DI.検索名称XY座標 == null)
            {
                MessageBoxTop.Show("検索名称XY座標が入力されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (DI.検索名称完了ボタンXY座標 == null)
            {
                MessageBoxTop.Show("検索名称F12完了ボタンXY座標が入力されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (DI.通常仕入先XY座標 == null)
            {
                MessageBoxTop.Show("通常仕入先XY座標が入力されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (DI.個別入力完了ボタンXY座標 == null)
            {
                MessageBoxTop.Show("通常仕入先F12完了ボタンXY座標が入力されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (DI.在庫メンテナンス受付範囲 == null)
            {
                MessageBoxTop.Show("在庫メンテナンス受付範囲が設定されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (DI.在庫メンテナンス範囲 == null)
            {
                MessageBoxTop.Show("在庫メンテナンス範囲が設定されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrEmpty(DI.在庫テーブルCSVパス))
            {
                MessageBoxTop.Show("在庫テーブルCSVパスが入力されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(DI.新帳合変更データ表パス))
            {
                MessageBoxTop.Show("新帳合変更データ表パスが入力されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            this.Hide();

            帳合変更マクロ実行Routines route = null;

            try
            {

                // 設定データにエラーが含まれていたら処理を中断する。
                // 新帳合先データがない場合などは処理を中断する。 DIをみる？　ファイルパスなど
                // 画像一致処理はする？　ウィンドウ名確認処理のほうがいい？

                // 新帳合先データを読み込む
                var 新帳合先データ = 新帳合変更データ表Loader.Load新帳合変更データ表();

                var OperationData = (from x in 新帳合先データ
                                     where
                                         x.IsErrorData == false
                                     select x).ToList();


                var ErrorData = (from x in 新帳合先データ
                                 where
                                     x.IsErrorData == true
                                 select x).ToList();

                if (!System.IO.File.Exists(DI.在庫テーブルCSVパス))
                {
                    MessageBoxTop.Show("在庫テーブル.CSVが存在しません。\r\nリアル在庫メニュー7.保守→1.メンテナンス→4.在庫→F5テキスト より出力処理を行ってください。", "ファイル読込エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Show();
                    this.Activate();
                    return;
                }


                byte[] bitmap受付MD5 = null;
                byte[] bitmapMD5 = null;

                if (MessageBoxTop.Show("初めにマクロのテストを開始します。\r\n在庫メンテナンス(受付)の検索名称を入力する画面を起動し、\r\n[ＯＫ]ボタンを押して開始してください。\r\nテストは３回行います。\r\n\r\n処理を中断する場合はキャンセルを押してください。", "マクロのテスト", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    //MessageBoxが閉じるのを待つ
                    Thread.Sleep(2000);

                    var table1row = 在庫テーブルLoader.Load在庫テーブル複数行取得(3);

                    if (table1row.Count == 0)
                    {
                        MessageBoxTop.Show("在庫テーブル.CSVから読み込んだデータ数が0件でした。\r\n処理を中断します。同様のエラーが出る場合はシステム管理者に確認してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Show();
                        this.Activate();
                        return;
                    }


                    // マクロのテスト
                    int counter = 0;
                    foreach (var row in table1row)
                    {
                        counter++;
                        if (counter == 1)
                        {
                            if (!BCMacroRoutines.MacroTestRoutines(settings.検索名称XY座標, row.商品コード, settings.検索名称完了ボタンXY座標, settings.通常仕入先XY座標, row.通常仕入先コード, settings.個別入力完了ボタンXY座標, settings.在庫メンテナンス受付範囲, settings.在庫メンテナンス範囲, out bitmap受付MD5, out bitmapMD5))
                            {
                                MessageBoxTop.Show("マクロのテスト中にエラーが発生しました。処理を中断します。\r\n再度実行し、同様のエラーが出る場合はシステム管理者に確認してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                                this.Show();
                                this.Activate();
                                return;
                            }
                        }
                        else
                        {
                            if (!BCMacroRoutines.MacroTestRoutines(settings.検索名称XY座標, row.商品コード, settings.検索名称完了ボタンXY座標, settings.通常仕入先XY座標, row.通常仕入先コード, settings.個別入力完了ボタンXY座標, settings.在庫メンテナンス受付範囲, settings.在庫メンテナンス範囲, bitmap受付MD5, bitmapMD5))
                            {
                                MessageBoxTop.Show("マクロのテスト中にエラーが発生しました。処理を中断します。\r\n再度実行し、同様のエラーが出る場合はシステム管理者に確認してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                                this.Show();
                                this.Activate();
                                return;
                            }
                        }
                    }

                    if (bitmap受付MD5 == null || bitmapMD5 == null)
                    {
                        MessageBoxTop.Show("マクロのテスト中の画像取得にエラーが発生しました。処理を中断します。\r\n再度実行し、同様のエラーが出る場合はシステム管理者に確認してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Show();
                        this.Activate();
                        return;
                    }

                }
                else
                {
                    this.Show();
                    this.Activate();
                    return;
                }


                // 開始準備完了メッセージ
                if (MessageBoxTop.Show("マクロのテストが正常な動作の場合は、続けて本処理を行います。  \r\n\r\n入力モードが半角英数になっていることを確認してください。\r\n在庫メンテナンス(受付)を起動し、検索名称ボックスが表示されている状態で、このウィンドウの『はい』を選択してください。\r\n\r\nここで処理を中断する場合は[いいえ]をクリックしてください。\r\n\r\n処理実行中に途中で中断する場合はESCボタンを10回押して処理が一時止まった後に、右上に表示されている処理を中止ボタンをクリックしてください。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Thread.Sleep(1000);

                    //BackgroundWorkerで処理し、いつでもキャンセルできるようにする
                    IsRun帳合変更マクロ = true;
                    route = new 帳合変更マクロ実行Routines(starttime, OperationData, settings, ErrorData, bitmap受付MD5, bitmapMD5);
                    route.DoRoutine();

                }
                else
                {
                    this.Show();
                    this.Activate();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxTop.Show("エラーが発生しました。処理を中断します。\r\n" + "Message:\r\n" + ex.Message + "Stacktrace:\r\n" + ex.StackTrace);

                if (route != null)
                {
                    route.CloseCancelWindow();
                }

                this.Show();
                this.Activate();

            }

        }

        private void btnValifyStart_Click(object sender, RoutedEventArgs e)
        {
            var folder = System.IO.Directory.GetCurrentDirectory();
            var sepa = DI.新帳合変更データ表パス.Split('\\');
            if (2 <= sepa.Count())
            {
                var str = "";
                for (int i = 0; i < sepa.Count() -1; i++)
                {
                    if (str == "")
                    {
                        str = sepa[i];
                    }
                    else
                    {
                        str += string.Format("\\{0}",sepa[i]);
                    }
                }

                folder = str;
            }

            var filename = "帳合変更結果.txt";
            var 帳合変更結果Filepath = System.IO.Path.Combine(folder,filename);

            if (!System.IO.File.Exists(帳合変更結果Filepath))
            {
                MessageBoxTop.Show("帳合変更結果.txtが存在しません。\r\nマクロ開始より変更処理が完了していることを確認してください。");
                return;
            }

            if (string.IsNullOrEmpty(DI.在庫テーブルCSVパス))
            {
                MessageBoxTop.Show("在庫テーブルCSVパスが入力されておりません。設定画面より設定を行って下さい。", "確認", MessageBoxButton.OK);
                return;
            }



            if (MessageBoxTop.Show("以下の手順で最新の在庫テーブル.CSVを出力してください。\r\n【手順】\r\nリアル在庫メニュー 7.保守→1.メンテナンス→4.在庫→F5テキスト\r\n\r\n出力完了後、[ＯＫ]で照合処理が開始します。", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                if (!System.IO.File.Exists(DI.在庫テーブルCSVパス))
                {
                    MessageBoxTop.Show("在庫テーブル.CSVが存在しません。設定および出力操作を再度確認してください。", "エラー", MessageBoxButton.OK);
                    return;
                }


                var 帳合変更結果list = 帳合変更結果Loader.Load帳合変更結果(帳合変更結果Filepath);
                var 在庫テーブルlist = 在庫テーブルLoader.Load在庫テーブル();

                int ErrorCount = 0;
                int SucceedCount = 0;
                foreach (var d in 帳合変更結果list)
                {
                    if (!d.IsSucceeded)
                    {
                        ErrorCount++;
                        continue;
                    }

                    var selected = (from x in 在庫テーブルlist
                                    where
                                         x.商品コード == d.商品コード
                                         &&
                                         x.通常仕入先コード == d.新帳合先コード
                                    select x).ToList();

                    if (selected.Count == 0)
                    {
                        ErrorCount++;
                    }
                    else
                    {
                        SucceedCount++;
                    }
                }

                MessageBoxTop.Show(string.Format("【帳合変更結果】\r\n 成功:{0}件  失敗:{1}件 \r\n\r\n帳合変更処理結果は次のファイルを参照してください。\r\n{2}", SucceedCount, ErrorCount, 帳合変更結果Filepath),"照合結果",MessageBoxButton.OK,MessageBoxImage.Information);


            }
            else
            {
                return;
            }




        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SingletonWindows.BalanceChangeSettingsWindow.Show();
            SingletonWindows.XYLocationWindow.ControlWindow = SingletonWindows.BalanceChangeSettingsWindow;
            SingletonWindows.XYLocationWindow.Show();
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnCreateMacro_Click(object sender, RoutedEventArgs e)
        {

            //OriginalMacroMaker omm = new OriginalMacroMaker();
            //omm.Show();

            SingletonWindows.OriginalMacroMakerWindow.Show();
            SingletonWindows.XYLocationWindow.ControlWindow = SingletonWindows.BalanceChangeSettingsWindow;
            SingletonWindows.XYLocationWindow.Show();
            this.WindowState = System.Windows.WindowState.Minimized;

        }

        private void btnOriginalMacroStart_Click(object sender, RoutedEventArgs e)
        {
            OriginalMacroExecuter omExe = new OriginalMacroExecuter();
            omExe.Show();
            SingletonWindows.BalanceChangeMenuWindow.Hide();
        }
    }
}
