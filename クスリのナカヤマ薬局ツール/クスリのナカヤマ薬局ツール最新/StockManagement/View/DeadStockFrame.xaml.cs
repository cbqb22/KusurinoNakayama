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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using StockManagement.Const;
using StockManagement.Settings;
using StockManagement.ViewModel;
using StockManagement.View.ProgressBar;
using StockManagement.ViewModel.Excel;
using StockManagement.View.Settings;
using StockManagement.ViewModel.DSException;
using StockManagement.ViewModel.Common.MessageBox;


namespace StockManagement.View
{
    /// <summary>
    /// DeadStockFrame.xaml の相互作用ロジック
    /// </summary>
    public partial class DeadStockFrame : UserControl, INotifyPropertyChanged
    {
        #region フィールド
        private BackgroundWorker _backgroundWorker;
        private BackgroundWorker _backgroundWorkerForExcel;
        private ProgressBarWindow pbw;
        private CompletedCountProgressBarWindow ccpbw;

        private DateTime _folderDate;
        private DateTime _from;
        private DateTime _to;
        private bool _autoPrint;
        private string _baseStoreName = null;
        private ExcelTypeEnum _excelType;
        private ButtonActionTypeEnum pressedAction;

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
        public DeadStockFrame()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(DeadStockFrame_Loaded);

            this.btnCreateOrderSheet.MouseLeftButtonDown += new MouseButtonEventHandler(btnCreateOrderSheet_MouseLeftButtonDown);

            this.DataContext = this;

        }
        #endregion

        #region クラスイベント
        void DeadStockFrame_Loaded(object sender, RoutedEventArgs e)
        {
            SetInit();
        }
        #endregion

        #region メソッド

        /// <summary>
        /// イニシャライザ
        /// </summary>
        private void SetInit()
        {

            int selectedIndex = -1;
            int counter = 0;
            InitialData.AllShopList.ForEach
                (
                    delegate(string x)
                    {
                        cmbOrderShopName.Items.Add(x);
                        if (x == InitialData.DeadMangementSourceStore)
                        {
                            selectedIndex = counter;
                        }
                        counter++;
                    }
                );

            // 使用量期間設定から値を取得
            int daterange = InitialData.UsedAmountDateRange;
            // デフォルトで過去４ヶ月（当月を含めて）
            nudFrom.tbNum.Text = DateTime.Now.AddMonths((daterange - 1) * -1).ToString("yyyy/MM");
            nudTo.tbNum.Text = DateTime.Now.ToString("yyyy/MM");

            cmbOrderShopName.SelectedIndex = selectedIndex;
            cmbOutputType.SelectedIndex = (int)InitialData.OutputExcelType;


            tblVersion.Text = string.Format("Version {0}", StockManagement.Model.DI.VersionName);


        }

        /// <summary>
        /// 開いているプログレスバーをすべて閉じる
        /// </summary>
        public void CloseAllProgressBar()
        {
            if (pbw != null)
            {
                pbw.Close();
            }

            if (ccpbw != null)
            {
                ccpbw.Close();
            }
        }

        /// <summary>
        /// 初期化メソッド　ボタン押下時
        /// </summary>
        /// <param name="actionEnum"></param>
        private bool ResetParameter(ButtonActionTypeEnum actionEnum)
        {

            DateTime fromResult;
            DateTime toResult;

            if (DateTime.TryParse(nudFrom.tbNum.Text, out fromResult) == false)
            {
                MessageBoxTop.Show("開始日付を選択して下さい。");
                return false;
            }

            if (DateTime.TryParse(nudTo.tbNum.Text, out toResult) == false)
            {
                MessageBoxTop.Show("終了日付を選択して下さい。");
                return false;
            }

            if (cmbOrderShopName.SelectedIndex == -1)
            {
                MessageBoxTop.Show("依頼元店舗名を選択して下さい。");
                return false;
            }



            if (cmbAutoPrint.SelectedIndex == 1)
            {
                _autoPrint = true;
            }
            else
            {
                _autoPrint = false;
            }


            if (cmbOutputType.SelectedIndex == 0)
            {
                _excelType = ExcelTypeEnum.Excel2003;
            }
            else if (cmbOutputType.SelectedIndex == 1)
            {
                _excelType = ExcelTypeEnum.Excel2007;
            }
            else
            {
                MessageBoxTop.Show("出力形式を選択して下さい。");
                return false;
            }

            _baseStoreName = cmbOrderShopName.SelectedValue.ToString();
            _folderDate = DateTime.Now;
            _from = fromResult;
            _to = toResult;
            pressedAction = actionEnum;


            return true;

        }


        private bool ParameterCheck()
        {

            DateTime fromResult;
            if (DateTime.TryParse(nudFrom.tbNum.Text, out fromResult) == false)
            {
                MessageBoxTop.Show("開始日付を選択して下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            DateTime toResult;
            if (DateTime.TryParse(nudTo.tbNum.Text, out toResult) == false)
            {
                MessageBoxTop.Show("終了日付を選択して下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (fromResult < SMConst.deadStockMinDate || fromResult < SMConst.deadStockMinDate)
            {
                MessageBoxTop.Show("使用期間を2012年1月より以前を指定できません。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            if (toResult <= fromResult)
            {
                MessageBoxTop.Show("終了日付は開始日付より未来を設定してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            var nowYearMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // 当月の１日とする
            if (nowYearMonth < toResult)
            {
                MessageBoxTop.Show("終了日付は本日(システム日付)以前の日付を設定してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            if (fromResult.AddMonths(15) <= toResult)
            {
                MessageBoxTop.Show("使用期間は最大１５ヶ月間以内で設定してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;

        }

        /// <summary>
        /// ダウンロードルーチン
        /// </summary>
        private void DownloadRoutine()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork); ;
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_backgroundWorker_ProgressChanged);
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.RunWorkerAsync(_folderDate);

            pbw = new ProgressBarWindow();
            pbw.Show();

        }

        /// <summary>
        /// ダウンロード＆Excel出力ルーチン
        ///  貰い受依頼書or貰い受可能リストor全店使用無しデッド品リスト
        /// </summary>
        private void DownloadAndExcelRoutine()
        {

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted2);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_backgroundWorker_ProgressChanged);
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.RunWorkerAsync(cmbOperationType.SelectedIndex);

            pbw = new ProgressBarWindow();
            pbw.Owner = Application.Current.MainWindow;
            pbw.Show();

        }




        #region Excelルーチン呼び出し
        /// <summary>
        /// 貰受依頼書作成
        /// </summary>
        private Exception DeadStockRoutine()
        {
            DeadStockRoutines dsr = new DeadStockRoutines(_from, _to, _folderDate.ToString("yyyyMMddHHmmss"), _backgroundWorkerForExcel, _excelType, _autoPrint, _folderDate, _baseStoreName);
            return dsr.DoWork();
        }



        /// <summary>
        /// 貰受可能リスト作成
        /// </summary>
        private bool DeadStockRoutinesForReceiving()
        {
            DeadStockRoutinesForReceiving dsrr = new DeadStockRoutinesForReceiving(_from, _to, _folderDate.ToString("yyyyMMddHHmmss"), _backgroundWorkerForExcel, _excelType, _autoPrint, _folderDate, _baseStoreName);
            return dsrr.DoWork();
        }

        /// <summary>
        /// 全店使用無しデッド品リスト作成
        /// </summary>
        private bool DeadStockRoutinesForAllStoreNoUse()
        {
            DeadStockRoutinesForAllStoreNoUse dsrfasn = new DeadStockRoutinesForAllStoreNoUse(_from, _to, _folderDate.ToString("yyyyMMddHHmmss"), _backgroundWorkerForExcel, _excelType, _autoPrint, _folderDate, _baseStoreName);
            return dsrfasn.DoWork();
        }


        /// <summary>
        /// 期限切迫品依頼書作成
        /// </summary>
        private bool ExpOrderRoutines()
        {
            ExpStockOrderRoutines esor = new ExpStockOrderRoutines(_from, _to, _folderDate.ToString("yyyyMMddHHmmss"), _backgroundWorkerForExcel, _excelType, _autoPrint, _folderDate, _baseStoreName);
            return esor.DoWork();
        }




        #endregion


        #endregion

        #region クリックイベント
        /// <summary>
        /// Click イベント download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {

            IsBusy = true;

            if (!ParameterCheck())
            {
                IsBusy = false;
                return;
            }


            if (!ResetParameter(ButtonActionTypeEnum.Downloading))
            {
                IsBusy = false;
                return;
            }

            //ExcelController ec = new ExcelController();
            //ec.CreatExcel();
            //string ExcelBookFileName = Path.Combine(SMConst.outputOrderFolder, string.Format("貰受依頼書{0}", _folderDate));
            //ec.CloseExcel(true, ExcelBookFileName, ExcelTypeEnum.Excel2007);

            DownloadRoutine();
        }

        /// <summary>
        /// Click イベント 依頼書または可能リスト作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateOrderOrReceivingSheet_Click(object sender, RoutedEventArgs e)
        {

            IsBusy = true;

            if (!ParameterCheck())
            {
                IsBusy = false;
                return;
            }

            if (cmbOperationType.SelectedIndex < 0)
            {
                MessageBoxTop.Show("操作種別が選択されていない為、処理を中断しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                IsBusy = false;
                return;
            }

            if (!ResetParameter((ButtonActionTypeEnum)cmbOperationType.SelectedIndex))
            {
                IsBusy = false;
                return;
            }

            //if (!ResetParameter(ButtonActionTypeEnum.OrderOrReceiving))
            //{
            //    IsBusy = false;
            //    return;
            //}

            if (cmbOperationType.SelectedIndex == 0 ||
                cmbOperationType.SelectedIndex == 1 ||
                cmbOperationType.SelectedIndex == 2 ||
                cmbOperationType.SelectedIndex == 3)
            {
                DownloadAndExcelRoutine();
            }
            else
            {
                MessageBoxTop.Show("操作種別が選択されていない為、処理を中断しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                IsBusy = false;
                return;
            }

        }

        /// <summary>
        /// 設定ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }

        /// <summary>
        /// 中断ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {

            string StopIndicator = "中断しております....";

            // 中断処理を複数回クリック防止
            if (pbw != null)
            {
                if (pbw.dp.tbOperationIndicator.Text == StopIndicator)
                {
                    return;
                }
            }
            if (ccpbw != null)
            {
                if (ccpbw.ccp.tbOperationIndicator.Text == StopIndicator)
                {
                    return;
                }
            }


            // 中断処理実行ダウンロード側
            if (_backgroundWorker != null)
            {
                _backgroundWorker.CancelAsync();

                if (pbw != null)
                {
                    pbw.dp.tbOperationIndicator.Text = StopIndicator;
                }
            }
            // 中断処理実行Excel処理側
            if (_backgroundWorkerForExcel != null)
            {
                _backgroundWorkerForExcel.CancelAsync();

                if (ccpbw != null)
                {
                    ccpbw.ccp.tbOperationIndicator.Text = StopIndicator;
                }

            }
        }


        #endregion

        #region バックグランドワーカー

        /// <summary>
        /// Progress Chnaged ダウンロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // プログレスバーの現在値を更新する
            pbw.dp.pbDownloading.Value = e.ProgressPercentage;
            pbw.dp.tbProgressPercentage.Text = e.ProgressPercentage.ToString();
            //pbw.dp.tbUpper.Text = e.ProgressPercentage.ToString();

        }

        /// <summary>
        /// Progress Chnaged Excel処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backgroundWorkerForExcel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //int result;

            //if (int.TryParse(ccpbw.ccp.tbLower.Text, out result) == false)
            //{
            //    return;
            //}

            //// パーセンテージと分母から分子を計算
            //int upper = result * 100 / e.ProgressPercentage;

            // プログレスバーの現在値を更新する
            ccpbw.ccp.pbCompletedCount.Value = e.ProgressPercentage;
            ccpbw.ccp.tbUpper.Text = e.ProgressPercentage.ToString();

        }


        /// <summary>
        /// Completed Downloadのみ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (pbw != null)
            {
                pbw.Close();
            }

            if (e.Cancelled)
            {
            }
            else if (e.Error != null)
            {
                MessageBoxTop.Show("エラーが発生した為、処理を中断しました。\r\nErrorMessage:\r\n" + e.Error.Message + "\r\nStackTrace:\r\n" + e.Error.StackTrace, "エラー",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                MessageBoxTop.Show("ファイルのダウンロードが完了しました。", "完了",MessageBoxButton.OK,MessageBoxImage.Information);
            }


            IsBusy = false;

        }

        /// <summary>
        /// Completed Downloadの後にExcel処理
        /// 貰い受依頼書or貰い受可能リスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backgroundWorker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {

            if (pbw != null)
            {
                pbw.Close();
            }

            if (e.Cancelled)
            {
                IsBusy = false;
                return;
            }
            else if (e.Error != null)
            {
                MessageBoxTop.Show("エラーが発生した為、処理を中断しました。\r\nErrorMessage:\r\n" + e.Error.Message + "\r\nStackTrace:\r\n" + e.Error.StackTrace, "エラー",MessageBoxButton.OK,MessageBoxImage.Error);
                IsBusy = false;
                return;
            }

            try
            {
                ccpbw = new CompletedCountProgressBarWindow();
                ccpbw.Owner = Application.Current.MainWindow;
                ccpbw.ccp.SetInit("");
                ccpbw.Show();

                _backgroundWorkerForExcel = new BackgroundWorker();
                _backgroundWorkerForExcel.WorkerSupportsCancellation = true;
                _backgroundWorkerForExcel.DoWork += new DoWorkEventHandler(_backgroundWorkerForExcel_DoWork);
                _backgroundWorkerForExcel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorkerForExcel_RunWorkerCompleted);
                _backgroundWorkerForExcel.ProgressChanged += new ProgressChangedEventHandler(_backgroundWorkerForExcel_ProgressChanged);
                _backgroundWorkerForExcel.WorkerReportsProgress = true;
                _backgroundWorkerForExcel.RunWorkerAsync(cmbOperationType.SelectedIndex);


            }
            catch (Exception ex)
            {

                MessageBoxTop.Show(ex.Message + ex.StackTrace, "エラー",MessageBoxButton.OK,MessageBoxImage.Error);

                if (ccpbw != null)
                {
                    ccpbw.Close();
                }

                IsBusy = false;

            }
        }

        /// <summary>
        /// Completed Excel処理後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backgroundWorkerForExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (ccpbw != null)
            {
                ccpbw.Close();
            }


            if (e.Cancelled)
            {
            }
            else
                if (e.Error != null)
                {
                    MessageBoxTop.Show("エラーが発生した為、処理を中断しました。\r\nErrorMessage:\r\n" + e.Error.Message + "\r\nStackTrace:\r\n" + e.Error.StackTrace);
                }
                else
                {
                    MessageBoxTop.Show("作成処理が完了しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            IsBusy = false;
        }



        /// <summary>
        /// Background Downloading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int OperationType = (int)e.Argument;

            //DateTime folderDate = (DateTime)e.Argument;
            DateTime folderDate = _folderDate;

            // ダウンロード処理
            FileDownloader fd = new FileDownloader(_backgroundWorker, folderDate, _baseStoreName);
            fd.TotalDownloadCountCalc(_from, _to);

            if (OperationType == 0 || OperationType == 1 || OperationType == 2)
            {
                // デッド品ダウンロード
                var ex = fd.StartDownloadDeadStockData();
                if (ex != null)
                {
                    if (ex is DeadStockException)
                    {
                        var dsex = ex as DeadStockException;
                        if (dsex.IsCancel)
                        {
                            e.Cancel = true;
                            MessageBoxTop.Show(dsex.DisplayMessage, "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;

                        }

                        MessageBoxTop.Show(dsex.DisplayMessage, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    throw ex;
                }

            }

            if (OperationType == 3)
            {
                // 現在庫をダウンロード
                fd.StartDownloadStockAllData(_baseStoreName);
            }


            // 使用量はすべての操作種別で必要
            var ex2 = fd.StartDownloadUsedData2(new List<string>(), _from, _to);
            //var ex2 = fd.StartDownloadUsedData(new List<string>(), _from, _to);
            if (ex2 != null)
            {
                if (ex2 is DeadStockException)
                {
                    var dsex2 = ex2 as DeadStockException;
                    if (dsex2.IsCancel)
                    {
                        e.Cancel = true;
                        MessageBoxTop.Show(dsex2.DisplayMessage, "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;

                    }

                    MessageBoxTop.Show(dsex2.DisplayMessage, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                throw ex2;

            }

        }


        /// <summary>
        /// Background Excel処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backgroundWorkerForExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            int OperationType = (int)e.Argument;

            if (OperationType == 0)
            {
                // 例外がなければnull
                var ex = DeadStockRoutine();
                if (ex != null)
                {

                    if (ex is DeadStockException)
                    {
                        var dsex = ex as DeadStockException;
                        if (dsex.IsCancel)
                        {
                            e.Cancel = true;
                            MessageBoxTop.Show(dsex.DisplayMessage, "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;

                        }

                        MessageBoxTop.Show(dsex.DisplayMessage, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }



                }
            }
            else if (OperationType == 1)
            {

                if (!DeadStockRoutinesForReceiving())
                {
                    e.Cancel = true;
                    return;
                }

            }
            else if (OperationType == 2)
            {
                if (!DeadStockRoutinesForAllStoreNoUse())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (OperationType == 3)
            {
                if (!ExpOrderRoutines())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                MessageBoxTop.Show("指定された操作種別は存在しない為、処理を中断します。");
                return;
            }


        }


        #endregion

        #region ボタンのフォーカス時のアニメーション
        private void btnCreateOrderSheet_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(btnCreateOrderSheet, "MouseEnter", true);


        }

        private void btnCreateOrderSheet_MouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(btnCreateOrderSheet, "MouseLeave", true);

        }

        private void btnCreateOrderSheet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(btnCreateOrderSheet, "MouseLeftButtonDown", true);

        }
        #endregion

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

        private void cmbOperationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                if (cmbOperationType.SelectedIndex == 2)
                {
                    cmbAutoPrint.IsEnabled = false;
                }
                else
                {
                    cmbAutoPrint.IsEnabled = true;
                }
            }

        }




    }
}
