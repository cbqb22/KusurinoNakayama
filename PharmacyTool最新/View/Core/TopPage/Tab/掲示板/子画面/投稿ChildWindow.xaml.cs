using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using View.Service.File.Writer;
using View.Core.共通;
using View.Core.TopPage.Tab.掲示板.メイン;
using System.IO;
using View.Core.共通.FileOperation;
using System.Collections.ObjectModel;
using View.Util.Common;

namespace View.Core.TopPage.Tab.掲示板.子画面
{
    public partial class 投稿ChildWindow : ChildWindow
    {
        // listのインデックスとcmbboxのインデックスが一致
        private List<int> _cmbbox_fileinfo_matching = new List<int>();
        ObservableCollection<string> 添付画像パスリスト = new ObservableCollection<string>();

        private List<Tuple<添付ファイルステート, FileInfo, string>> _添付情報管理 = new List<Tuple<添付ファイルステート, FileInfo, string>>();
        internal List<Tuple<添付ファイルステート, FileInfo, string>> 添付情報管理
        {
            get { return _添付情報管理; }
            set { _添付情報管理 = value; }
        }

        public long ChunkSize = 4194304;



        public List<int> Cmbbox_fileinfo_matching
        {
            get { return _cmbbox_fileinfo_matching; }
            set { _cmbbox_fileinfo_matching = value; }
        }

        private 掲示板書込タイプ _書込タイプ;

        public 掲示板書込タイプ 書込タイプ
        {
            get { return _書込タイプ; }
            set { _書込タイプ = value; }
        }


        public 投稿ChildWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(投稿ChildWindow_Loaded);
        }

        private void Set添付Visibilly()
        {
            if (1 <= Cmbbox_fileinfo_matching.Count)
            {
                this.cmb添付Files.Visibility = Visibility.Visible;
                this.bt添付削除.Visibility = Visibility.Visible;

                if (1 <= this.cmb添付Files.Items.Count)
                {
                    this.cmb添付Files.SelectedIndex = 0;
                }
            }
            else
            {
                this.cmb添付Files.Visibility = Visibility.Collapsed;
                this.bt添付削除.Visibility = Visibility.Collapsed;
            }
        }

        private void Set添付情報管理()
        {
            // 一旦削除してから再構築する
            this.cmb添付Files.Items.Clear();
            Cmbbox_fileinfo_matching.Clear();

            int counter = 0;
            foreach (var 添付情報 in 添付情報管理)
            {
                if (添付情報.Value1 == 添付ファイルステート.添付済)
                {
                    this.cmb添付Files.Items.Add(添付情報.Value3);
                    Cmbbox_fileinfo_matching.Add(counter);
                }
                else if (添付情報.Value1 == 添付ファイルステート.未添付)
                {
                    this.cmb添付Files.Items.Add(添付情報.Value2.Name);
                    Cmbbox_fileinfo_matching.Add(counter);
                }
                counter++;
            }
        }


        void 投稿ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetLabelVisibilly();

            Set添付情報管理();
            Set添付Visibilly();

        }

        void SetLabelVisibilly()
        {

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(this.tbInputPerson.Text))
            {
                MessageBox.Show("投稿者を入力して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrEmpty(this.wtbInputArticle.Text))
            {
                MessageBox.Show("記事を入力して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            string convertEmail = "";
            if (!string.IsNullOrEmpty(this.tbInputEmail.Text))
            {
                convertEmail = "mailto:" + this.tbInputEmail.Text;
            }





            string 投稿タイトル = "";

            if (string.IsNullOrEmpty(this.tbInputTitle.Text))
            {
                投稿タイトル = "無題";
                // タイトルが入っていなかったら「無題」とする。
                //MessageBox.Show("タイトルを入力して下さい。", "確認", MessageBoxButton.OK);
                //return;
            }
            else
            {
                投稿タイトル = this.tbInputTitle.Text;
            }




            // 添付画像パスリスト取得
            foreach (var 添付情報 in 添付情報管理)
            {
                if (添付情報.Value1 == 添付ファイルステート.添付済)
                {
                    添付画像パスリスト.Add(添付情報.Value3);
                }
                else if (添付情報.Value1 == 添付ファイルステート.未添付)
                {
                    添付画像パスリスト.Add(添付情報.Value2.Name);
                }
                else if (添付情報.Value1 == 添付ファイルステート.添付削除)
                {
                    // 何もしない
                }
            }

            Do提示板データ書込(投稿タイトル, this.tbInputPerson.Text, this.wtbInputArticle.Text, this.tbInputHomepageUrl.Text, convertEmail, 添付画像パスリスト);



            this.DialogResult = true;
        }

        void client_FileWriter実行Completed(object sender, FileWriter実行CompletedEventArgs e)
        {
            try
            {




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

        private void Do提示板データ書込(string タイトル, string 投稿者, string 記事, string HomepageUrl, string Email, ObservableCollection<string> 添付画像パスリスト)
        {
            View.Service.File.Writer.文字色 文字色 = Check文字色();

            FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
            client.掲示板データ書込Completed += new EventHandler<掲示板データ書込CompletedEventArgs>(client_掲示板データ書込Completed);
            client.掲示板データ書込Async(this.カテゴリ名, タイトル, 投稿者, 記事, 書込タイプ, this.投稿記事No, HomepageUrl, Email, 添付画像パスリスト, pb暗証キー.Password, 文字色);
        }

        void client_掲示板データ書込Completed(object sender, 掲示板データ書込CompletedEventArgs e)
        {
            try
            {


                if (e.Error == null)
                {
                    掲示板結果リターンEntity ent = e.Result as 掲示板結果リターンEntity;

                    if (ent.書込成功か)
                    {

                        // 添付削除を先に処理する
                        foreach (var 添付情報 in 添付情報管理)
                        {
                            if (添付情報.Value1 == 添付ファイルステート.添付削除)
                            {
                                // 削除処理
                                string prepath = System.IO.Path.Combine(Core.共通.Settings.Upload掲示板FileRootPath, ent.書込みカテゴリ);
                                string pre = System.IO.Path.Combine(prepath, ent.書込み記事No);
                                string deleteFilePath = System.IO.Path.Combine(pre, 添付情報.Value3);

                                FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
                                client.FileWriter実行Completed += new EventHandler<FileWriter実行CompletedEventArgs>(client_FileWriter実行Completed);
                                client.FileWriter実行Async(deleteFilePath, ディレクトリ操作モード.Delete, タイプ.ファイル, null);
                            }
                        }

                        // 未添付を添付する
                        foreach (var 添付情報 in 添付情報管理)
                        {
                            if (添付情報.Value1 == 添付ファイルステート.未添付)
                            {
                                FileUploader ful = new FileUploader(this.Dispatcher, 添付情報.Value2, true, ent.書込み記事No, ent.書込みカテゴリ);

                                ful.CheckFileOnServer(Core.共通.Settings.Upload掲示板FileRootPath);
                            }
                        }


                        TreeViewItemData tvid = SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;

                        if (tvid == null || string.IsNullOrEmpty(tvid.Name))
                        {
                            return;
                        }

                        SingletonInstances.掲示板FrameInstance.Get掲示板データ(tvid.Name, 1);
                    }
                    else
                    {
                        MessageBox.Show(ent.エラーメッセージ);
                    }
                }
                else
                {
                    MessageBox.Show("エラーが発生しました。\r\n原因：" + e.Error.Message + e.Error.StackTrace);
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        View.Service.File.Writer.文字色 Check文字色()
        {
            if (this.cmbSelectColors.SelectedIndex == 0)
            {
                return Service.File.Writer.文字色.茶色;
            }
            else if (this.cmbSelectColors.SelectedIndex == 1)
            {
                return Service.File.Writer.文字色.赤;
            }
            else if (this.cmbSelectColors.SelectedIndex == 2)
            {
                return Service.File.Writer.文字色.緑;
            }
            else if (this.cmbSelectColors.SelectedIndex == 3)
            {
                return Service.File.Writer.文字色.青;
            }
            else if (this.cmbSelectColors.SelectedIndex == 4)
            {
                return Service.File.Writer.文字色.紫;
            }
            else if (this.cmbSelectColors.SelectedIndex == 5)
            {
                return Service.File.Writer.文字色.ピンク;
            }
            else if (this.cmbSelectColors.SelectedIndex == 6)
            {
                return Service.File.Writer.文字色.オレンジ;
            }
            else if (this.cmbSelectColors.SelectedIndex == 7)
            {
                return Service.File.Writer.文字色.黒;
            }
            else
            {
                return Service.File.Writer.文字色.茶色;
            }


        }


        /// <summary>
        /// レスの入力の時の番号
        /// </summary>
        private string _投稿記事No;

        public string 投稿記事No
        {
            get { return _投稿記事No; }
            set { _投稿記事No = value; }
        }


        private string _カテゴリ名;

        public string カテゴリ名
        {
            get { return _カテゴリ名; }
            set { _カテゴリ名 = value; }
        }

        private void cmbSelectColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null)
            {
                return;
            }

            if (this.wtbInputArticle == null)
            {
                return;
            }

            switch (cmb.SelectedIndex)
            {
                case 0: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Brown);
                    break;
                case 1: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case 2: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case 3: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Blue);
                    break;
                case 4: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Purple);
                    break;
                case 5: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Magenta);
                    break;
                case 6: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case 7: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                default: this.wtbInputArticle.Foreground = new SolidColorBrush(Colors.Brown);
                    break;
            }
        }


        private void bt添付File_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "All Files (*.*)|*.*";
            if ((bool)ofd.ShowDialog())
            {
                if (ofd.File.Name.Contains("@"))
                {
                    MessageBox.Show("ファイル名に「@」がつくファイルは選択出来ません。", "エラー", MessageBoxButton.OK);
                }
            }
            else
            {
                return;
            }

            if (ChunkSize <= ofd.File.Length)
            {
                MessageBox.Show("ファイルサイズが４ＭＢを超えている為、添付出来ません。\r\n資料タブをご利用し、保存して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            foreach (var checkname in 添付情報管理)
            {
                // 添付済もしくは、未添付ファイルと同じ名称があれば削除する
                if (checkname.Value1 == 添付ファイルステート.添付済 || checkname.Value1 == 添付ファイルステート.未添付)
                {
                    if (checkname.Value3.Equals(ofd.File.Name))
                    {
                        MessageBox.Show("同じファイル名が存在します。\r\nファイル名を変更して下さい。", "確認", MessageBoxButton.OK);
                        return;
                    }
                }
            }

            Tuple<添付ファイルステート, FileInfo, string> tp = new Tuple<添付ファイルステート, FileInfo, string>();
            tp.Value1 = 添付ファイルステート.未添付;
            tp.Value2 = ofd.File;
            tp.Value3 = ofd.File.Name;
            添付情報管理.Add(tp);

            Set添付情報管理();

            // 念のため
            Set添付Visibilly();

        }

        private void bt添付削除_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            if (cmb添付Files.SelectedIndex < 0)
            {
                return;
            }
            else
            {
                if (cmb添付Files.SelectedIndex <= Cmbbox_fileinfo_matching.Count - 1)
                {
                    int index = Cmbbox_fileinfo_matching[cmb添付Files.SelectedIndex];

                    添付情報管理[index].Value1 = 添付ファイルステート.添付削除;

                    Set添付情報管理();
                    Set添付Visibilly();

                }
            }


        }
    }

    enum 文字色タイプ
    {
        茶色 = 0,
        赤 = 1,
        緑 = 2,
        青 = 3,
        紫 = 4,
        ピンク = 5,
        オレンジ = 6,
        黒 = 7
    }

    enum 添付ファイルステート
    {
        添付済 = 0,
        未添付 = 1,
        添付削除 = 2
    }

}

