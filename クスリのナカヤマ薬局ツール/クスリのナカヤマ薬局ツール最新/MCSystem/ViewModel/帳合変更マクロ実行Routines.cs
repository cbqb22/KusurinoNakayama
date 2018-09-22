using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;
using MCSystem.Model;
using MCSystem.View.Windows;
using MCSystem.ViewModel.Common.MessageBox;


namespace MCSystem.ViewModel
{
    public class 帳合変更マクロ実行Routines : INotifyPropertyChanged
    {

        #region フィールド
        private BackgroundWorker _backgroundWorker;
        //private ProgressBarWindow pbw;
        //private CompletedCountProgressBarWindow ccpbw;

        private DateTime _Starttime;
        private Cancel _CancelWindow;

        private List<新帳合変更データ表Entity> _OperationData;
        private List<新帳合変更データ表Entity> _ErrorData;
        private List<新帳合変更データ表Entity> _SucceedData;
        private BCSettingsEntity _Settings;
        private byte[] _Bitmap受付MD5;
        private byte[] _BitmapMD5;

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                this.FirePropertyChanged("IsBusy");

            }
        }
        #endregion


        #region コンストラクタ

        public 帳合変更マクロ実行Routines(DateTime Starttime, List<新帳合変更データ表Entity> OperationData, BCSettingsEntity Settings, List<新帳合変更データ表Entity> ErrorData, byte[] Bitmap受付MD5, byte[] BitmapMD5)
        {
            this._Starttime = Starttime;
            this._OperationData = OperationData;
            this._Settings = Settings;
            this._ErrorData = ErrorData;
            this._Bitmap受付MD5 = Bitmap受付MD5;
            this._BitmapMD5 = BitmapMD5;
        }

        #endregion

        /// <summary>
        /// ルーチン開始
        /// </summary>
        public void DoRoutine()
        {
            _CancelWindow = new Cancel();
            _CancelWindow.Show();

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_backgroundWorker_ProgressChanged);
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.RunWorkerAsync();


        }

        public void CloseCancelWindow()
        {
            if (this._CancelWindow != null)
            {
                this._CancelWindow.Close();
            }
        }

        void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            var folder = System.IO.Directory.GetCurrentDirectory();
            var sepa = DI.新帳合変更データ表パス.Split('\\');
            if (2 <= sepa.Count())
            {
                var str = "";
                for (int i = 0; i < sepa.Count() - 1; i++)
                {
                    if (str == "")
                    {
                        str = sepa[i];
                    }
                    else
                    {
                        str += string.Format("\\{0}", sepa[i]);
                    }
                }

                folder = str;
            }

            var filename = "帳合変更結果.txt";

            //var desktoppath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //var filename = string.Format("帳合変更結果_{0}.txt", _Starttime.ToString("yyyyMMddHHmmss"));
            var resultfilepath = System.IO.Path.Combine(folder, filename);


            try
            {
                if (e.Cancelled)
                {
                    MessageBoxTop.Show("処理変更中にエラーが発生した為、中止しました。");

                    using (StreamWriter sw = new StreamWriter(resultfilepath, false, Encoding.GetEncoding(932)))
                    {
                        sw.WriteLine("帳合変更処理の途中でエラーが発生した為、処理を中止しました。");
                        sw.WriteLine(string.Format("変更開始日時:{0}", _Starttime.ToString("yyyy/MM/dd HH:mm:ss")));
                        sw.WriteLine(e.Result.ToString());
                    }

                }
                else if (_CancelWindow.PushedCancel)
                {
                    // 完了メッセージ
                    MessageBoxTop.Show("中断ボタンが押された為、\r\n処理を中断しました。", "処理中断", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    using (StreamWriter sw = new StreamWriter(resultfilepath, false, Encoding.GetEncoding(932)))
                    {
                        sw.WriteLine("帳合変更処理が途中で中断されました。");
                        sw.WriteLine(string.Format("変更開始日時:{0}", _Starttime.ToString("yyyy/MM/dd HH:mm:ss")));
                    }

                }
                else
                {

                    // 処理した内容を出力
                    string str = "処理結果,商品コード,医薬品名,通常仕入先名,新帳合先名,新帳合先コード";

                    foreach (var sd in _SucceedData)
                    {
                        str += string.Format("\r\n{0},{1},{2},{3},{4},{5}","成功",sd.商品コード,sd.医薬品名,sd.通常仕入先名,sd.新帳合先名,sd.新帳合先コード);
                    }

                    // エラーではじいたものを出力
                    foreach (var sd in _ErrorData)
                    {
                        str += string.Format("\r\n{0},{1},{2},{3},{4},{5}", "失敗", sd.商品コード, sd.医薬品名, sd.通常仕入先名, sd.新帳合先名, sd.新帳合先コード);
                    }

                    using (StreamWriter sw = new StreamWriter(resultfilepath, false, Encoding.GetEncoding(932)))
                    {
                        sw.WriteLine("帳合変更を正常に完了しました。");
                        sw.WriteLine(string.Format("変更開始日時:{0}", _Starttime.ToString("yyyy/MM/dd HH:mm:ss")));
                        sw.WriteLine(str);
                    }

                    // 完了メッセージ
                    MessageBoxTop.Show(string.Format("新帳合先への一括変更処理が正常に完了しました。\r\n成功:{0}件  失敗:{1}件\r\n\r\n処理した内容は以下に出力しております。\r\n{2}",_SucceedData.Count,_ErrorData.Count, resultfilepath), "正常完了", MessageBoxButton.OK, MessageBoxImage.Information);


                    this.CloseCancelWindow();
                }

            }
            catch(Exception ex)
            {
                MessageBoxTop.Show("エラーが発生しました。\r\nMessage:" + ex.Message + "\r\nStackTrace:" + ex.StackTrace);
            }
            finally
            {
                SingletonWindows.BalanceChangeMenuWindow.IsRun帳合変更マクロ = false;
                this.CloseCancelWindow();

                SingletonWindows.BalanceChangeMenuWindow.Show();
                SingletonWindows.BalanceChangeMenuWindow.Activate();

            }

        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //在庫メンテナンス画面(受付)になっているかチェック
                double CenterX = _Settings.在庫メンテナンス受付範囲.X + _Settings.在庫メンテナンス受付範囲.Width / 2;
                double CenterY = _Settings.在庫メンテナンス受付範囲.Y + _Settings.在庫メンテナンス受付範囲.Height / 2;
                BCMacroRoutines.ClickPoint(new Rect(CenterX, CenterY, 0d, 0d)); // 在庫メンテナンス(受付)範囲の中央をクリック

                if (BCMacroRoutines.在庫メンテナンス受付画面かチェック(_Settings.在庫メンテナンス受付範囲, _Bitmap受付MD5)== false)
                {
                    e.Result = "在庫メンテナンス受付画面が表示されなかった為、処理を中断しました。\r\n開始時のエラー。" ;
                    e.Cancel = true;
                    return;
                }

                _SucceedData = new List<新帳合変更データ表Entity>();

                // ルーチンスタート
                foreach (var data in _OperationData)
                {

                    if (_CancelWindow.PushedCancel)
                    {
                        break;
                    }

                    var succeed = BCMacroRoutines.Routines(_Settings.検索名称XY座標, data.商品コード, _Settings.検索名称完了ボタンXY座標, _Settings.通常仕入先XY座標, data.新帳合先コード, _Settings.個別入力完了ボタンXY座標, _Settings.在庫メンテナンス受付範囲, _Settings.在庫メンテナンス範囲, _Bitmap受付MD5, _BitmapMD5);

                    if (!succeed)
                    {
                        // エラー時の補足
                        _ErrorData.Add(data);
                    }
                    else
                    {
                        _SucceedData.Add(data);
                    }

                    // dwWaitTimeミリ秒間待機する
                    // (キーの取りこぼしを防ぐため)
                    Thread.Sleep(300);

                }

            }
            catch (Exception ex)
            {
                e.Result = ex.Message + ex.StackTrace;
                e.Cancel = true;
            }


        }

        #region INotifyPropertyChanged メンバ

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    }
}
