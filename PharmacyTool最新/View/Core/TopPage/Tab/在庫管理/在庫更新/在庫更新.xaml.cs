using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using View.Core.共通.Entity;
using System.Collections.Generic;
using View.Service.File.TreeView;
using System.Collections.ObjectModel;
using View.Service.File.Writer;
using System.Net;
using System.IO;
using System.Text;
using View.Service.DAO.PharmacyTool.店舗;
using View.Core.共通;
using View.Service.File.Reader;

namespace View.Core.TopPage.Tab.在庫管理.在庫更新
{
    public partial class 在庫更新 : UserControl
    {
        public 在庫更新()
        {
            InitializeComponent();
            SingletonInstances.在庫更新Instance = this;


            SetDefaultItemSources();

            SetMEDIS最終更新日時();
        }

        #region フィールド変数
        ObservableCollection<string> _店舗名List = new ObservableCollection<string>();

        public ObservableCollection<string> 店舗名List
        {
            get { return _店舗名List; }
            set { _店舗名List = value; }
        }
        ObservableCollection<PT店舗名リターンデータセット> _dataset = new ObservableCollection<PT店舗名リターンデータセット>();

        public ObservableCollection<PT店舗名リターンデータセット> Dataset
        {
            get { return _dataset; }
            set { _dataset = value; }
        }

        ObservableCollection<string> 更新年List = new ObservableCollection<string>();
        ObservableCollection<string> 更新月List = new ObservableCollection<string>();
        ObservableCollection<string> 更新種別List = new ObservableCollection<string>();
        private FileInfo 参照したFileInfo;
        #endregion


        private void SetMEDIS最終更新日時()
        {
            FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.GetMEDISデータ最終更新日時Completed += new EventHandler<GetMEDISデータ最終更新日時CompletedEventArgs>(client_GetMEDISデータ最終更新日時Completed);
            client.GetMEDISデータ最終更新日時Async();
        }

        void client_GetMEDISデータ最終更新日時Completed(object sender, GetMEDISデータ最終更新日時CompletedEventArgs e)
        {

            try
            {

                if (e.Error == null)
                {
                    最終更新日時リターンデータセット rdataset = e.Result;
                    if (rdataset.データ取得成功か)
                    {
                        this.tblMEDIS最終更新日時.Text = rdataset.MEDIS最終更新日時;
                    }
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }

        }

        private void SetDefaultItemSources()
        {
            Create店舗名List();

            Create更新年List();
            int NowYear = System.DateTime.Now.Year;
            this.更新年ComboBox.SelectedIndex = NowYear - 2010 + 1;

            Create更新月List();
            int Month = System.DateTime.Now.Month;
            this.更新月ComboBox.SelectedIndex = Month - 1;

            Create更新種別List();
            this.更新種別ComboBox.SelectedIndex = 0;
        }

        public void Create店舗名List()
        {
            if (this.店舗名ComboBox.ItemsSource != null)
            {
                this.店舗名ComboBox.ItemsSource = null;
            }

            StoreDataClient client = Util.ServiceUtil.ReferenceCreater.GetStoreDataClient();
            client.店舗名取得Completed += new EventHandler<店舗名取得CompletedEventArgs>(client_店舗名取得Completed);
            client.店舗名取得Async();
        }


        void client_店舗名取得Completed(object sender, 店舗名取得CompletedEventArgs e)
        {
#if DEBUG 

            try
            {
                if (e != null)
                {

                    this.店舗名List.Clear();

                    店舗名List.Add("横浜駅南口店");
                    店舗名List.Add("川崎ラゾーナ店");
                    店舗名List.Add("小田原店");
                    店舗名List.Add("目黒店");
                    店舗名List.Add("六本木ヒルズ店");
                    店舗名List.Add("新宿都庁駅前店");
                    店舗名List.Add("本郷３丁目店");
                    店舗名List.Add("カレッタ汐留店");
                    店舗名List.Add("秋葉原メインストリート店");

                    this.店舗名ComboBox.ItemsSource = 店舗名List;
                    if (1 <= 店舗名List.Count)
                    {
                        this.店舗名ComboBox.SelectedIndex = 0;
                    }

                    // 新規ユーザー作成タブで、アクセスレベルのコンボボックスに最新の店舗名を更新する
                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.uc新規ユーザー作成.Setアクセス権限ComboBoxItem(this.Dataset);

                    // 既存ユーザー更新タブのアクセスレベルのコンボボックスに最新のアクセスレベル（店舗名）を更新する
                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.uc既存ユーザー更新.SetComboBoxItemsource2(this.Dataset);

                    // 店舗削除タブのドロップダウンリストに、最新の店舗名を配置する
                    Core.共通.SingletonInstances.店舗管理TabItemInstance.uc店舗名削除.Setcmb店舗名(this.Dataset);

                    // 店舗名の表示順序をセット
                    Dictionary<int, string> dic = new Dictionary<int, string>();
                    int compareValue = 1;
                    foreach (var l in 店舗名List)
                    {
                        dic.Add(compareValue, l);
                        compareValue++;
                    }

                    Core.共通.PageScope.表示順序 = dic;

                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }


#elif NAKAYAMA

            try
            {

                if (e != null)
                {
                    Dataset = e.Result;

                    this.店舗名List.Clear();
                    foreach (var set in Dataset)
                    {
                        店舗名List.Add(set.店舗名);
                    }

                    this.店舗名ComboBox.ItemsSource = 店舗名List;
                    if (1 <= 店舗名List.Count)
                    {
                        this.店舗名ComboBox.SelectedIndex = 0;
                    }

                    // 新規ユーザー作成タブで、アクセスレベルのコンボボックスに最新の店舗名を更新する
                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.uc新規ユーザー作成.Setアクセス権限ComboBoxItem(this.Dataset);

                    // 既存ユーザー更新タブのアクセスレベルのコンボボックスに最新のアクセスレベル（店舗名）を更新する
                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.uc既存ユーザー更新.SetComboBoxItemsource2(this.Dataset);

                    // 店舗削除タブのドロップダウンリストに、最新の店舗名を配置する
                    Core.共通.SingletonInstances.店舗管理TabItemInstance.uc店舗名削除.Setcmb店舗名(this.Dataset);

                    // 店舗名の表示順序をセット
                    Dictionary<int, string> dic = new Dictionary<int, string>();
                    int compareValue = 1;
                    foreach (var l in 店舗名List)
                    {
                        dic.Add(compareValue, l);
                        compareValue++;
                    }

                    Core.共通.PageScope.表示順序 = dic;

                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }

#else
            try
            {
                if (e != null)
                {

                    this.店舗名List.Clear();

                    店舗名List.Add("横浜駅南口店");
                    店舗名List.Add("川崎ラゾーナ店");
                    店舗名List.Add("小田原店");
                    店舗名List.Add("目黒店");
                    店舗名List.Add("六本木ヒルズ店");
                    店舗名List.Add("新宿都庁駅前店");
                    店舗名List.Add("本郷３丁目店");
                    店舗名List.Add("カレッタ汐留店");
                    店舗名List.Add("秋葉原メインストリート店");

                    this.店舗名ComboBox.ItemsSource = 店舗名List;
                    if (1 <= 店舗名List.Count)
                    {
                        this.店舗名ComboBox.SelectedIndex = 0;
                    }

                    // 新規ユーザー作成タブで、アクセスレベルのコンボボックスに最新の店舗名を更新する
                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.uc新規ユーザー作成.Setアクセス権限ComboBoxItem(this.Dataset);

                    // 既存ユーザー更新タブのアクセスレベルのコンボボックスに最新のアクセスレベル（店舗名）を更新する
                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.uc既存ユーザー更新.SetComboBoxItemsource2(this.Dataset);

                    // 店舗削除タブのドロップダウンリストに、最新の店舗名を配置する
                    Core.共通.SingletonInstances.店舗管理TabItemInstance.uc店舗名削除.Setcmb店舗名(this.Dataset);

                    // 店舗名の表示順序をセット
                    Dictionary<int, string> dic = new Dictionary<int, string>();
                    int compareValue = 1;
                    foreach (var l in 店舗名List)
                    {
                        dic.Add(compareValue, l);
                        compareValue++;
                    }

                    Core.共通.PageScope.表示順序 = dic;

                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }

#endif



        }

        /// <summary>
        /// 管理ユーザーまたは本部ユーザー以外でログインした場合は、ItemSourceは一般ユーザーの店舗名に設定
        /// </summary>
        public void Set一般ユーザーSelectIndex()
        {
            if (Core.共通.PageScope.表示名称 != null &&
                101 <= Core.共通.PageScope.アクティブアクセス権限)
            {
                // 一旦空にする
                this.店舗名ComboBox.ItemsSource = null;

                ObservableCollection<string> 一般ユーザー用ItemSource = new ObservableCollection<string>();
                // 店舗名が含まれていたら
                if (this.店舗名List.Contains(Core.共通.PageScope.表示名称))
                {
                    string s = this.店舗名List[this.店舗名List.IndexOf(Core.共通.PageScope.表示名称)];
                    一般ユーザー用ItemSource.Add(s);
                    this.店舗名ComboBox.ItemsSource = 一般ユーザー用ItemSource;

                    if (this.店舗名ComboBox.Items.Count == 1)
                    {
                        this.店舗名ComboBox.SelectedIndex = 0;
                    }
                }
                // 含まれていなかったら
                else
                {
                    // 全店のリストは表示するが、選択はしない。
                    this.店舗名ComboBox.ItemsSource = this.店舗名List;
                    //if (1 <= this.店舗名ComboBox.Items.Count)
                    //{
                    //    this.店舗名ComboBox.SelectedIndex = 0;
                    //}
                }
            }


        }

        private void Create更新年List()
        {
            for (int counter = 1; counter < 53; counter++)
            {

                this.更新年List.Add((2008 + counter).ToString().ToUpper() + "年");
            }

            this.更新年ComboBox.ItemsSource = this.更新年List;
        }

        private void Create更新月List()
        {
            for (int counter = 1; counter <= 12; counter++)
            {

                this.更新月List.Add(counter.ToString().ToUpper() + "月");
            }

            this.更新月ComboBox.ItemsSource = this.更新月List;
        }

        private void Create更新種別List()
        {
            this.更新種別List.Add("現在庫データ");
            this.更新種別List.Add("使用量データ");
            this.更新種別List.Add("不動品データ");


            // 管理ユーザーまたは本部ユーザーの場合は、MEDISデータの更新を可能とする。
            if (Core.共通.PageScope.アクティブアクセス権限 == 0 ||
                Core.共通.PageScope.アクティブアクセス権限 == 1)
            {
                this.更新種別List.Add("MEDISデータ");
            }


            this.更新種別ComboBox.ItemsSource = this.更新種別List;
        }


        /// <summary>
        /// 管理ユーザーまたは本部ユーザーの場合は、MEDISデータの更新を可能とする。
        /// Login後に呼び出される
        /// </summary>
        public void Create更新種別ListAfterLogin()
        {
            this.更新種別ComboBox.ItemsSource = null;

            // 管理ユーザーまたは本部ユーザーの場合は、MEDISデータの更新を可能とする。
            if (Core.共通.PageScope.アクティブアクセス権限 == 0 ||
                Core.共通.PageScope.アクティブアクセス権限 == 1)
            {
                this.更新種別List.Add("MEDISデータ");
            }

            this.更新種別ComboBox.ItemsSource = this.更新種別List;

            this.更新種別ComboBox.SelectedIndex = 0;
        }

        private void 参照Button_Click(object sender, RoutedEventArgs e)
        {

            if (this.更新種別ComboBox.SelectedItem == null)
            {
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            if (this.更新種別ComboBox.SelectedItem.ToString().Equals("MEDISデータ"))
            {
                ofd.Multiselect = false;
                ofd.Filter = "テキストファイル (*.txt)|*.txt";
            }
            else
            {
                ofd.Multiselect = false;
                ofd.Filter = "CSVファイル (*.csv)|*.csv";
            }

            if ((bool)ofd.ShowDialog())
            {
                if (ofd.File.Name.Contains("@"))
                {
                    MessageBox.Show("ファイル名に「@」がつくファイルは選択出来ません。", "エラー", MessageBoxButton.OK);
                }

                this.更新ファイル名TextBlock.Text = ofd.File.Name;
                File = ofd.File;

            }


        }


        private void UploadFile(string fileName, Stream data)
        {
            //Util.Common.UriBuilderBase ub = new View.Util.Common.UriBuilderBase(Core.共通.Settings.在庫更新GenericHandlerPath);

            //ub.Query = string.Format(fileName);

            //List<byte[]> listbyte = CreatePushData(data);

            //foreach (var b in listbyte)
            //{
            WebClient c = new WebClient();

            try
            {
                c.OpenWriteCompleted += (sender, e) =>
                {
                    PushData(data, e.Result);
                    e.Result.Close();
                    data.Close();
                };
                //c.OpenWriteAsync(ub.Uri);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //}
        }

        private List<byte[]> CreatePushData(Stream input)
        {
            List<byte[]> listbyte = new List<byte[]>();

            byte[] buffer = new byte[409600];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                listbyte.Add(buffer);
            }

            listbyte.Add(buffer);

            return listbyte;
        }


        private void PushData(byte[] buffer, Stream output)
        {
            output.Write(buffer, 0, buffer.Length);
        }




        private void PushData(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {

#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif

            if (this.店舗名ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("店舗名を選択して下さい", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.更新種別ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("更新種別を選択して下さい", "確認", MessageBoxButton.OK);
                return;
            }

            if (File == null)
            {
                MessageBox.Show("更新ファイルを選択して下さい", "確認", MessageBoxButton.OK);
                return;
            }

            // ファイル名のチェック
            if (this.更新種別ComboBox.SelectedIndex == 0)
            {
                if (!File.Name.Equals("現在庫データ.csv"))
                {
                    MessageBox.Show("選択したファイル名は、現在庫データ.csvではありません。\r\n現在庫データ.csvを選択して下さい。", "エラー", MessageBoxButton.OKCancel);
                    return;
                }
            }

            if (this.更新種別ComboBox.SelectedIndex == 1)
            {

                int 年;
                int 月;
                if (int.TryParse(this.更新年ComboBox.SelectedItem.ToString().Replace("年", ""), out 年) == false ||
                int.TryParse(this.更新月ComboBox.SelectedItem.ToString().Replace("月", ""), out 月) == false)
                {
                    MessageBox.Show("更新年、更新月が正しく選択されていません。");
                    return;
                }


                if (!File.Name.Equals(string.Format("使用量データ_{0}{1}.csv", 年.ToString(), 月.ToString())) &&
                    !File.Name.Equals(string.Format("使用量データ_{0}{1}.CSV", 年.ToString(), 月.ToString())))
                {
                    MessageBox.Show(string.Format("選択したファイル名は、使用量データ_{0}{1}.CSVではありません。\r\n使用量データ_{0}{1}.CSVを選択して下さい。", 年.ToString(), 月.ToString()), "エラー", MessageBoxButton.OKCancel);
                    return;
                }
            }



            //string name = File.Name.Replace("\\", "@");

            string s = this.更新種別ComboBox.SelectedItem.ToString();

            if (MessageBox.Show(string.Format("{0}{1}を更新します。\r\n宜しいですか？", s.Equals("MEDISデータ") ? "" : this.店舗名ComboBox.SelectedItem.ToString() + "の", this.更新種別ComboBox.SelectedItem.ToString()), "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (s.Equals("現在庫データ"))
                {
                    this.実行中タイプ = アップロードタイプ.現在庫データ;
                }
                else if (s.Equals("使用量データ"))
                {
                    this.実行中タイプ = アップロードタイプ.使用量データ;
                }
                else if (s.Equals("不動品データ"))
                {
                    this.実行中タイプ = アップロードタイプ.不動品データ;
                }
                else if (s.Equals("MEDISデータ"))
                {
                    this.実行中タイプ = アップロードタイプ.MEDISデータ;
                }
                else
                {
                    this.実行中タイプ = アップロードタイプ.中止;
                }

                CheckFileOnServer();

            }
            else
            {
                return;
            }

        }


        private void 店舗名ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void 更新種別ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox sb = sender as ComboBox;
            if (sb == null)
            {
                return;
            }

            if (sb.SelectedItem == null)
            {
                return;
            }


            if (sb.SelectedItem.ToString().Equals("MEDISデータ"))
            {
                this.店舗名ComboBox.IsEnabled = false;
                this.更新年ComboBox.IsEnabled = false;
                this.更新月ComboBox.IsEnabled = false;
            }
            else if (sb.SelectedItem.ToString().Equals("現在庫データ")
                || sb.SelectedItem.ToString().Equals("不動品データ"))
            {
                this.店舗名ComboBox.IsEnabled = true;
                this.更新年ComboBox.IsEnabled = false;
                this.更新月ComboBox.IsEnabled = false;
            }
            else if (sb.SelectedItem.ToString().Equals("使用量データ"))
            {
                this.店舗名ComboBox.IsEnabled = true;
                this.更新年ComboBox.IsEnabled = true;
                this.更新月ComboBox.IsEnabled = true;
            }


            // MEDISデータを選択時は、MEDISデータ最終更新日時を表示する
            if (this.更新種別ComboBox.SelectedItem.ToString().Equals("MEDISデータ"))
            {
                this.tblMEDIS最終更新日時.Visibility = Visibility.Visible;
            }
            else
            {
                this.tblMEDIS最終更新日時.Visibility = Visibility.Collapsed;
            }

        }



        #region UploadLogic


        enum アップロードタイプ
        {
            現在庫データ = 0,
            使用量データ = 1,
            不動品データ = 2,
            MEDISデータ = 3,
            中止 = 4

        }


        private アップロードタイプ _実行中タイプ = アップロードタイプ.中止;

        private アップロードタイプ 実行中タイプ
        {
            get { return _実行中タイプ; }
            set { _実行中タイプ = value; }
        }


        private long fileLength;
        public long FileLength
        {
            get { return fileLength; }
            set
            {
                fileLength = value;
            }
        }


        private long bytesUploaded;
        public long BytesUploaded
        {
            get { return bytesUploaded; }
            set
            {
                bytesUploaded = value;
            }
        }

        public enum FileUploadStatus
        {
            Pending,
            Uploading,
            Complete,
            Error,
            Canceled,
            Removed,
            Resizing
        }

        private FileUploadStatus status;
        public FileUploadStatus Status
        {
            get { return status; }
            set
            {
                status = value;

            }
        }

        public Uri UploadUrl { get; set; }
        private FileInfo file;
        public FileInfo File
        {
            get { return file; }
            set
            {
                file = value;
                Stream temp = file.OpenRead();
                FileLength = temp.Length;
                temp.Close();
            }
        }


        public long ChunkSize = 4194304;
        private MemoryStream resizeStream;


        private string _UploadingFileName;

        public string UploadingFileName
        {
            get { return _UploadingFileName; }
            set { _UploadingFileName = value; }
        }

        private void CheckFileOnServer()
        {
            SetUploadingFileName();
            string uri = Core.共通.Settings.GenericHandlerPath;
            UriBuilder ub = new UriBuilder(uri);
            ub.Query = string.Format("{1}filename={0}&GetBytes=true&Type={2}", UploadingFileName, string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&", "在庫関連");
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(ub.Uri);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // 同じファイル名を毎日更新する場合がほとんどだから、同名ファイルの確認メッセージは行わない。

            //long lengthtemp = 0;
            //if (!string.IsNullOrEmpty(e.Result))
            //{
            //    lengthtemp = long.Parse(e.Result);
            //}

            //if (lengthtemp > 0)
            //{
            //    MessageBoxResult result;
            //    if (lengthtemp == FileLength)
            //    {
            //        result = MessageBox.Show("同名ファイルが存在します。上書きしますか？", "上書き確認？", MessageBoxButton.OKCancel);
            //        if (result == MessageBoxResult.OK)
            //            lengthtemp = 0;
            //        else
            //        {

            //            BytesUploaded = FileLength;
            //            Status = FileUploadStatus.Complete;
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        result = MessageBox.Show("同名ファイルが存在します。アップロードを続けますか？", "アップロード継続確認", MessageBoxButton.OKCancel);
            //        if (result == MessageBoxResult.Cancel)
            //            lengthtemp = 0;
            //    }
            //}

            if (e.Error == null)
            {
                UploadFileEx();
            }
            else
            {
                this.実行中タイプ = アップロードタイプ.中止;
                MessageBox.Show("ファイルの更新に失敗しました。再度、更新操作を行って下さい。");
                return;
            }


        }


        public void UploadFileEx()
        {
            Status = FileUploadStatus.Uploading;
            long temp = FileLength - BytesUploaded;

            UriBuilder ub = new UriBuilder(Core.共通.Settings.GenericHandlerPath);
            bool complete = temp <= ChunkSize;
            ub.Query = string.Format("{3}filename={0}&StartByte={1}&Complete={2}&Type={4}", UploadingFileName, BytesUploaded, complete, string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&", "在庫関連");

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(ub.Uri);
            webrequest.Method = "POST";
            webrequest.BeginGetRequestStream(new AsyncCallback(WriteCallback), webrequest);
        }



        private void WriteCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the operation.
            Stream requestStream = webrequest.EndGetRequestStream(asynchronousResult);

            byte[] buffer = new Byte[4096];
            int bytesRead = 0;
            int tempTotal = 0;

            Stream fileStream = resizeStream != null ? (Stream)resizeStream : File.OpenRead();

            fileStream.Position = BytesUploaded;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0 && tempTotal + bytesRead < ChunkSize && !cancel)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();
                BytesUploaded += bytesRead;
                tempTotal += bytesRead;



                this.Dispatcher.BeginInvoke(delegate()
                {
                    int percent = (int)(((double)BytesUploaded / (double)FileLength) * 100);
                    //this.pgUpload.Value = percent;
                    //this.tbPercent.Text = percent.ToString() + "%";
                });



            }

            if (resizeStream == null)
                fileStream.Close();
            requestStream.Close();
            webrequest.BeginGetResponse(new AsyncCallback(ReadCallback), webrequest);

        }



        private bool cancel;
        private bool remove;


        public void CancelUpload()
        {
            cancel = true;
        }

        public void RemoveUpload()
        {
            cancel = true;
            remove = true;
            if (Status != FileUploadStatus.Uploading)
                Status = FileUploadStatus.Removed;
        }



        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)webrequest.EndGetResponse(asynchronousResult);
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string responsestring = reader.ReadToEnd();
            reader.Close();

            if (cancel)
            {
                // DoMerge用のフラグを中止にしておく
                this.実行中タイプ = アップロードタイプ.中止;

                if (resizeStream != null)
                    resizeStream.Close();
                if (remove)
                    Status = FileUploadStatus.Removed;
                else
                    Status = FileUploadStatus.Canceled;
            }
            else if (BytesUploaded < FileLength)
                UploadFileEx();
            else
            {
                if (resizeStream != null)
                    resizeStream.Close();

                Status = FileUploadStatus.Complete;

                this.Dispatcher.BeginInvoke(delegate()
                {
                    BytesUploaded = 0L;


                    // MEDISデータの時は最終更新日時を更新する
                    if (this.実行中タイプ == アップロードタイプ.MEDISデータ)
                    {
                        SetMEDIS最終更新日時();
                    }

                    // 成功した時だけマージする
                    PreMerge();

                    // フラグも戻す
                    this.実行中タイプ = アップロードタイプ.中止;


                });

            }

        }

        #endregion


        private void PreMerge()
        {
            if (this.実行中タイプ == アップロードタイプ.現在庫データ)
            {
                DoMerge(Service.File.Reader.MergeType.現在庫);
            }
            else if (this.実行中タイプ == アップロードタイプ.使用量データ)
            {
                DoMerge(Service.File.Reader.MergeType.使用量);
            }
            else if (this.実行中タイプ == アップロードタイプ.不動品データ)
            {
                DoMerge(Service.File.Reader.MergeType.不動品);
            }
        }


        private void DoMerge(Service.File.Reader.MergeType マージタイプ)
        {
            Service.File.Reader.FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.DoMergeCompleted += new EventHandler<Service.File.Reader.DoMergeCompletedEventArgs>(client_DoMergeCompleted);
            client.DoMergeAsync(マージタイプ);

            //string uri = Settings.ZaikoGenericHandlerPath;
            //UriBuilder ub = new UriBuilder(uri);
            //ub.Query = string.Format("Type={0}&Operation={1}", "Merge", マージタイプ);
            //WebClient client = new WebClient();
            ////client.Credentials = new System.Net.NetworkCredential(Settings.BasicID, Settings.BasicCredent);
            //client.DownloadStringCompleted +=new DownloadStringCompletedEventHandler(client_DownloadStringCompleted_Merge);
            //client.DownloadStringAsync(ub.Uri);

        }

        void client_DoMergeCompleted(object sender, Service.File.Reader.DoMergeCompletedEventArgs e)
        {

            try
            {

                if (e.Error == null)
                {
                    if (e.Result.Merge成功か)
                    {
                        MessageBox.Show("アップロードしたデータをマージしました。");
                    }
                    else
                    {
                        MessageBox.Show(e.Result.エラーメッセージ);
                    }
                }
                else
                {
                    MessageBox.Show("データをマージできませんでした。\r\n原因：" + e.Error.Message + e.Error.StackTrace);
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }

        }


        //void client_DownloadStringCompleted_Merge(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        MessageBox.Show("アップロードしたデータをマージしました。");
        //    }
        //    else
        //    {
        //        MessageBox.Show("データをマージできませんでした。\r\n原因：" + e.Error.Message + e.Error.StackTrace);
        //    }

        //}

        private void SetUploadingFileName()
        {
            // \は@に置換

            if (this.更新種別ComboBox.SelectedItem.ToString().Equals("MEDISデータ"))
            {
                UploadingFileName = string.Format("MEDIS\\MEDIS.TXT").Replace("\\", "@");
                return;
            }
            else if (this.更新種別ComboBox.SelectedItem.ToString().Equals("現在庫データ"))
            {
                string 選択中店舗名 = this.店舗名ComboBox.SelectedItem.ToString();
                UploadingFileName = string.Format("現在庫\\{0}\\現在庫データ.csv", 選択中店舗名).Replace("\\", "@");
                return;
            }
            else if (this.更新種別ComboBox.SelectedItem.ToString().Equals("不動品データ"))
            {
                string 選択中店舗名 = this.店舗名ComboBox.SelectedItem.ToString();
                UploadingFileName = string.Format("不動品\\{0}\\不動品データ.csv", 選択中店舗名).Replace("\\", "@");
                return;
            }
            else if (this.更新種別ComboBox.SelectedItem.ToString().Equals("使用量データ"))
            {
                string 選択中店舗名 = this.店舗名ComboBox.SelectedItem.ToString();
                string 選択中更新月 = this.更新月ComboBox.SelectedItem.ToString();
                string 選択中更新年 = this.更新年ComboBox.SelectedItem.ToString();
                UploadingFileName = string.Format("使用量\\{0}\\{1}\\{2}.csv", 選択中店舗名, 選択中更新年, 選択中更新月).Replace("\\", "@");
                return;
            }
            else
            {
                MessageBox.Show("更新種別が正しく選択されていません");
                return;
            }


        }





    }
}
