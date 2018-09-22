using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using View.Core.TopPage.Tab.掲示板.メイン;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using View.Service.File.Writer;
using View.Util.Common;
using View.Service.File.Reader;


namespace View
{
    public partial class 投稿フォーム : UserControl
    {
        private List<FileInfo> _fileinfo = new List<FileInfo>();

        public List<FileInfo> Fileinfo
        {
            get { return _fileinfo; }
            set { _fileinfo = value; }
        }

        public 投稿フォーム()
        {
            // 変数を初期化する必要があります
            InitializeComponent();
            SetDefaltToおなまえTextBox();
            SetDefault暗証キー();
        }

        void SetDefault暗証キー()
        {
            this.pb暗証キー.Password = "1234";
        }

        private 掲示板書込タイプ _書込タイプ;

        public 掲示板書込タイプ 書込タイプ
        {
            get { return _書込タイプ; }
            set { _書込タイプ = value; }
        }


        private void 投稿Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbおなまえ.Text))
            {
                MessageBox.Show("投稿者を入力して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrEmpty(this.tbコメント.Text))
            {
                MessageBox.Show("記事を入力して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            TreeViewItemData tvid = Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;
            if (tvid == null)
            {
                MessageBox.Show("投稿するスレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(tvid.Name))
            {
                return;
            }

            string 投稿タイトル = "";

            if (string.IsNullOrEmpty(this.tbタイトル.Text))
            {
                投稿タイトル = "無題";
                // タイトルが入っていなかったら「無題」とする。
                //MessageBox.Show("タイトルを入力して下さい。", "確認", MessageBoxButton.OK);
                //return;
            }
            else
            {
                投稿タイトル = this.tbタイトル.Text;
            }

            string convertEmail = "";
            if (!string.IsNullOrEmpty(this.tbEメール.Text))
            {
                convertEmail = "mailto:" + this.tbEメール.Text;
            }




            ObservableCollection<string> 添付画像パスリスト = new ObservableCollection<string>();
            foreach (var fi in Fileinfo)
            {
                添付画像パスリスト.Add(fi.Name);
            }




            View.Service.File.Writer.文字色 文字色 = Check文字色();


            Service.File.Writer.FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
            client.掲示板データ書込Completed += new EventHandler<Service.File.Writer.掲示板データ書込CompletedEventArgs>(client_掲示板データ書込Completed);
            client.掲示板データ書込Async(tvid.Name, 投稿タイトル, tbおなまえ.Text, tbコメント.Text, 書込タイプ, "", tbURL.Text, convertEmail, 添付画像パスリスト, pb暗証キー.Password, 文字色);





        }

        View.Service.File.Writer.文字色 Check文字色()
        {
            if ((bool)this.rbtBrown.IsChecked)
            {
                return Service.File.Writer.文字色.茶色;
            }
            else if ((bool)this.rbtBlue.IsChecked)
            {
                return Service.File.Writer.文字色.青;
            }
            else if ((bool)this.rbtBlack.IsChecked)
            {
                return Service.File.Writer.文字色.黒;
            }
            else if ((bool)this.rbtGreen.IsChecked)
            {
                return Service.File.Writer.文字色.緑;
            }
            else if ((bool)this.rbtOrange.IsChecked)
            {
                return Service.File.Writer.文字色.オレンジ;
            }
            else if ((bool)this.rbtPink.IsChecked)
            {
                return Service.File.Writer.文字色.ピンク;
            }
            else if ((bool)this.rbtPurple.IsChecked)
            {
                return Service.File.Writer.文字色.紫;
            }
            else if ((bool)this.rbtRed.IsChecked)
            {
                return Service.File.Writer.文字色.赤;
            }
            else
            {
                return Service.File.Writer.文字色.茶色;
            }

        }


        private void クリアButton_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            tbタイトル.Text = "";
            tbコメント.Text = "";
            Fileinfo = new List<System.IO.FileInfo>();
            stp添付File.Children.Clear();

        }

        void client_掲示板データ書込Completed(object sender, Service.File.Writer.掲示板データ書込CompletedEventArgs e)
        {
            try
            {

                if (e.Error == null)
                {
                    掲示板結果リターンEntity ent = e.Result as 掲示板結果リターンEntity;

                    if (ent.書込成功か)
                    {

                        // 添付ファイルのアップロードi
                        foreach (var fi in Fileinfo)
                        {
                            Core.共通.FileOperation.FileUploader ful = new Core.共通.FileOperation.FileUploader(this.Dispatcher, fi, true, ent.書込み記事No, ent.書込みカテゴリ);
                            ful.CheckFileOnServer(Core.共通.Settings.Upload掲示板FileRootPath);
                        }



                        TreeViewItemData tvid = Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;

                        if (tvid == null || string.IsNullOrEmpty(tvid.Name))
                        {
                            return;
                        }

                        Core.共通.SingletonInstances.掲示板FrameInstance.Get掲示板データ(tvid.Name, 1);
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

        public long ChunkSize = 4194304;
        private void bt添付File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "All Files (*.*)|*.*";
            if ((bool)ofd.ShowDialog())
            {
                if (ofd.File.Name.Contains("@"))
                {
                    MessageBox.Show("ファイル名に「@」がつくファイルは選択出来ません。", "エラー", MessageBoxButton.OK);
                    return;
                }
            }
            else
            {
                return;
            }

            if (ChunkSize <= ofd.File.Length)
            {
                MessageBox.Show("ファイルサイズが４ＭＢを超えている為、添付出来ません。\r\n資料タブを利用して、保存して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            int childCount = VisualTreeHelper.GetChildrenCount(stp添付File);
            for (int i = 0; i < childCount; i++)
            {
                FrameworkElement fe = VisualTreeHelper.GetChild(stp添付File, i) as FrameworkElement;
                if (fe == null)
                {
                    continue;
                }
                else if (fe is TextBlock)
                {
                    if (((TextBlock)fe).Text.Equals(ofd.File.Name))
                    {
                        MessageBox.Show("同じファイル名が存在します。\r\nファイル名を変更して下さい。", "確認", MessageBoxButton.OK);
                        return;
                    }
                }
            }


            Fileinfo.Add(ofd.File);



            Button bt = sender as Button;
            TextBlock tbl = new TextBlock();
            tbl.Text = ofd.File.Name;

            stp添付File.Children.Add(tbl);

        }

        private void bt送信_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItemData tvid = Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;
            bool 管理者か = (Core.共通.PageScope.アクティブアクセス権限 == 0 || Core.共通.PageScope.アクティブアクセス権限 == 1) ? true : false;

            if (tvid == null || string.IsNullOrEmpty(tvid.Name))
            {
                MessageBox.Show("操作するスレッドを選択して下さい。", "エラー", MessageBoxButton.OK);
                return;
            }

            if (cmb処理 == null && cmb処理.SelectedIndex != 0 && cmb処理.SelectedIndex != 1)
            {
                MessageBox.Show("処理を選択して下さい。", "エラー", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrEmpty(tb記事No.Text))
            {
                MessageBox.Show("記事Noを入力して下さい。", "エラー", MessageBoxButton.OK);
                return;
            }

            if (!管理者か && string.IsNullOrEmpty(pbVerify暗証キー.Password))
            {
                MessageBox.Show("暗証キーを入力して下さい。", "エラー", MessageBoxButton.OK);
                return;
            }


            // 記事番号は半角にする
            string 半角記事番号 = StringConverter.数字全角to半角変換(tb記事No.Text);
            int result;
            if (!int.TryParse(半角記事番号, out result))
            {
                MessageBox.Show("記事Noに数値を入力して下さい。", "エラー", MessageBoxButton.OK);
                return;
            }

            // 記事修正
            if (cmb処理.SelectedIndex == 0)
            {
                // 修正記事のパスワードがあっているか認証を行う。
                Service.File.Reader.FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
                client.掲示板記事修正確認チェックCompleted += new EventHandler<Service.File.Reader.掲示板記事修正確認チェックCompletedEventArgs>(client_掲示板記事修正確認チェックCompleted);

                client.掲示板記事修正確認チェックAsync(tvid.Name, 半角記事番号, pbVerify暗証キー.Password, 管理者か);


            }
            // 記事削除
            else
            {
                Service.File.Writer.FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
                client.掲示板データ削除Completed += new EventHandler<Service.File.Writer.掲示板データ削除CompletedEventArgs>(client_掲示板データ削除Completed);
                client.掲示板データ削除Async(tvid.Name, 半角記事番号, pbVerify暗証キー.Password, 管理者か);

            }



        }


        void client_掲示板記事修正確認チェックCompleted(object sender, Service.File.Reader.掲示板記事修正確認チェックCompletedEventArgs e)
        {
            try
            {


                if (e.Error == null)
                {
                    Service.File.Reader.掲示板記事修正確認Entity ent = e.Result as Service.File.Reader.掲示板記事修正確認Entity;
                    if (!ent.暗証キーチェック成功)
                    {
                        MessageBox.Show(ent.エラーメッセージ);
                    }
                    else
                    {
                        Core.共通.SingletonInstances.掲示板FrameInstance.記事修正入力画面(ent.記事データ);
                    }
                }
                else
                {
                    MessageBox.Show("修正を開始できませんでした。\r\n原因:" + e.Error.Message + e.Error.StackTrace);
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

        void client_掲示板データ削除Completed(object sender, Service.File.Writer.掲示板データ削除CompletedEventArgs e)
        {

            try
            {


                if (e.Error == null)
                {
                    MessageBox.Show(e.Result, "確認", MessageBoxButton.OK);

                    if (!e.Result.Equals("削除しました。"))
                    {
                        return;
                    }

                    TreeViewItemData tvid = Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;

                    if (tvid == null || string.IsNullOrEmpty(tvid.Name))
                    {
                        return;
                    }

                    Core.共通.SingletonInstances.掲示板FrameInstance.Get掲示板データ(tvid.Name, 1);

                }
                else
                {
                    MessageBox.Show(e.Error.Message + e.Error.StackTrace + "削除できませんでした。", "エラー", MessageBoxButton.OK);
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

        /// <summary>
        /// ログインユーザー名に合わせて「おなまえ」をセット
        /// </summary>
        public void SetDefaltToおなまえTextBox()
        {
            // 管理ユーザーは管理者
            if (Core.共通.PageScope.アクティブアクセス権限 == 0)
            {
                this.tbおなまえ.Text = "管理者";
                return;
            }

            // 本部ユーザーは本部
            if (Core.共通.PageScope.アクティブアクセス権限 == 1)
            {
                this.tbおなまえ.Text = "本部";
                return;
            }


            if (string.IsNullOrEmpty(Core.共通.PageScope.表示名称))
            {
                this.tbおなまえ.Text = "";
                return;
            }
            else
            {
                this.tbおなまえ.Text = Core.共通.PageScope.表示名称;
                return;
            }
        }

        private void bt検索ボタン_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.tbWordSearch.Text) || this.tbWordSearch.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {
                MessageBox.Show("キーワードを入力して下さい。");
                return;
            }

            TreeViewItemData tvid = Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;

            if (tvid == null || string.IsNullOrEmpty(tvid.Name))
            {
                MessageBox.Show("検索するスレッド名を選択して下さい。");
                return;
            }


            FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.Open掲示板データキーワード検索Completed += new EventHandler<Open掲示板データキーワード検索CompletedEventArgs>(client_Open掲示板データキーワード検索Completed);
            client.Open掲示板データキーワード検索Async(this.tbWordSearch.Text, tvid.Name);


        }

        void client_Open掲示板データキーワード検索Completed(object sender, Open掲示板データキーワード検索CompletedEventArgs e)
        {
            try
            {

                if (e.Error == null)
                {
                    var dataset = e.Result;
                    if (dataset.記事取得成功か)
                    {
                        Core.共通.SingletonInstances.掲示板FrameInstance.DoSet掲示板データ(dataset);
                    }
                    else
                    {
                        MessageBox.Show(dataset.エラーメッセージ);
                    }
                }
                else
                {
                    MessageBox.Show("キーワード検索時にエラーになりました。\r\n再度、検索を行って下さい。");
                    return;
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
    }
}