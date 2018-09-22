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
using View.Service.File.Reader;
using View.Service.File.Writer;
using System.Collections.ObjectModel;
using View.Core.TopPage.Tab.掲示板.子画面;
using View.Util.Common;

namespace View.Core.TopPage.Tab.掲示板.メイン
{
    public partial class 掲示板Frame : UserControl
    {
        public 掲示板Frame()
        {
            InitializeComponent();
            View.Core.共通.SingletonInstances.掲示板FrameInstance = this;

            Create調剤部門連絡掲示板TextBlock();

            this.LayoutUpdated += new EventHandler(掲示板Frame_LayoutUpdated);
            this.svThread.MouseWheel += new MouseWheelEventHandler(svThread_MouseWheel);
        }

        // マウスホイールのスクロールアクション
        void svThread_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;

            if (sv == null)
            {
                return;
            }

            svThread.ScrollToVerticalOffset(sv.VerticalOffset - (e.Delta));
        }

        private void Create調剤部門連絡掲示板TextBlock()
        {
            Label lb = new Label();
            lb.Content = "調 剤 部 門 連 絡 掲 示 板";
            lb.FontSize = 24d;

            Color col = new Color();
            col.R = (byte)88;
            col.G = (byte)143;
            col.B = (byte)175;
            col.A = (byte)255;

            SolidColorBrush scb = new SolidColorBrush(col);

            lb.Foreground = scb;
            lb.HorizontalAlignment = HorizontalAlignment.Center;
            lb.VerticalAlignment = VerticalAlignment.Center;

            this.調剤部門連絡掲示板Label = lb;

        }

        // カラム０（スレッド名称表示列）の幅をキャッシュ
        // 幅０の時はキャッシュしない
        private double _ColumnDefaultWidth;

        public double ColumnDefaultWidth
        {
            get { return _ColumnDefaultWidth; }
            set { _ColumnDefaultWidth = value; }
        }

        void 掲示板Frame_LayoutUpdated(object sender, EventArgs e)
        {
            // 投稿返信セットの幅を調整(-30は微調整値)
            Set投稿返信セットWidth(this.gdTop.ColumnDefinitions[1].ActualWidth - 30);
            SetColumnDefaultWidth();


        }

        void SetColumnDefaultWidth()
        {
            if (0 < this.gdTop.ColumnDefinitions[0].ActualWidth)
            {
                this.ColumnDefaultWidth = this.gdTop.ColumnDefinitions[0].ActualWidth;
            }
        }

        void Set投稿返信セットWidth(double aw)
        {

            // 投稿返信セットのMinWidth=410
            if (aw < 410)
            {
                return;
            }

            int count = VisualTreeHelper.GetChildrenCount(this.StackP1);
            for (int i = 0; i < count; i++)
            {
                FrameworkElement fe = VisualTreeHelper.GetChild(this.StackP1, i) as FrameworkElement;
                if (fe == null)
                {
                    return;
                }

                if (fe is 投稿返信セット)
                {
                    投稿返信セット set = (投稿返信セット)fe;
                    // 131はLeftMargin分
                    set.stpContents.MaxWidth = aw - 131;
                    if (set.MinWidthセット済か)
                    {
                        set.stpHeader.Width = aw;
                    }
                }

            }
        }



        public void Get掲示板データ(string カテゴリ名, int グループNo)
        {
            FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();

            client.Open掲示板データCompleted += new EventHandler<Open掲示板データCompletedEventArgs>(client_Open掲示板データCompleted);
            client.Open掲示板データAsync(カテゴリ名, グループNo);
        }

        void client_Open掲示板データCompleted(object sender, Open掲示板データCompletedEventArgs e)
        {
            try
            {

                var dataset = e.Result;
                DoSet掲示板データ(dataset);


                if (!this.ThreadSelectorName.初回ロード済か)
                {
                    // 初回ロード時は一番上のスレッド名を選択させる。
                    Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.SetTreeViewItemSelect();

                    // 掲示板リロード用にボタンも初期にもどしておく
                    this.btスレッド非表示.Visibility = Visibility.Collapsed;
                    this.btスレッド表示.Visibility = Visibility.Visible;


                    // Initスレッド名幅ゼロ()はNAKAYAMAの場合だけにする
                    
#if DEBUG
                    this.ThreadSelectorName.初回ロード済か = true;

#elif NAKAYAMA
                    Core.共通.SingletonInstances.掲示板FrameInstance.Initスレッド名幅ゼロ();
                    this.ThreadSelectorName.初回ロード済か = true;
#else
                    this.ThreadSelectorName.初回ロード済か = true;
                    
#endif

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

        private Label _調剤部門連絡掲示板Label;

        public Label 調剤部門連絡掲示板Label
        {
            get { return _調剤部門連絡掲示板Label; }
            set { _調剤部門連絡掲示板Label = value; }
        }


        public void DoSet掲示板データ(掲示板リターンデータセット dataset)
        {
            ObservableCollection<Dictionary<投稿Entity, ObservableCollection<返信Entity>>> data = dataset.掲示板データ;

            // 投稿フォームはそのままキャッシュしておく
            int count = VisualTreeHelper.GetChildrenCount(this.StackP1);
            投稿フォーム set = null;
            for (int i = 0; i < count; i++)
            {
                FrameworkElement fe = VisualTreeHelper.GetChild(this.StackP1, i) as FrameworkElement;
                if (fe == null)
                {
                    return;
                }

                if (fe is 投稿フォーム)
                {
                    set = (投稿フォーム)fe;

                    // タイトルとコメントは空白
                    // ファイル情報はクリア
                    // 表示用ファイル名もクリア
                    set.tbタイトル.Text = "";
                    set.tbコメント.Text = "";
                    set.Fileinfo = new List<System.IO.FileInfo>();
                    set.stp添付File.Children.Clear();


                }
            }


            // 一度StackPanelにある前のデータを空にする
            this.StackP1.Children.Clear();

            this.StackP1.Children.Add(this.調剤部門連絡掲示板Label);

            if (data.Count == 0)
            {
                // キーワード検索の時のメッセージ
                if (dataset.キーワード検索結果か)
                {
                    TextBlock tbl = new TextBlock();
                    tbl.Text = string.Format("キーワード【{0}】 で検索した結果、該当する記事がありません。", dataset.検索したキーワード);
                    tbl.FontSize = 16.0;

                    this.StackP1.Children.Add(tbl);
                }
                // 通常のメッセージ
                else
                {
                    TextBlock tbl = new TextBlock();
                    tbl.Text = "まだ投稿記事がありません。";
                    tbl.FontSize = 16.0;

                    this.StackP1.Children.Add(tbl);
                }
            }
            else
            {

                int counter = 1;

                if (dataset.キーワード検索結果か)
                {

                    // キーワード検索で１件以上ある場合は、検索した記ワードをいれておく。
                    TextBlock tbl = new TextBlock();
                    tbl.Text = string.Format("キーワード【{0}】 で検索した結果、{1}件の該当する記事がありました。", dataset.検索したキーワード, data.Count.ToString());
                    tbl.FontSize = 16.0;
                    this.StackP1.Children.Add(tbl);
                }

                foreach (var d in data)
                {
                    foreach (var ind in d)
                    {

                        投稿返信セット 投返セット = new 投稿返信セット(ind.Key.添付ファイルlist);
                        // 同じUserControl内、NameがつけられていないUserControlを配置するとエラーになる為、それを回避する
                        // 投稿返信セットはエラーにならないが、念のために。
                        投返セット.Name = "uc投返セット" + 投返セット.GetHashCode();
                        投返セット.DataContext = ind.Key;


                        foreach (var hset in ind.Value)
                        {
                            返信セット 返セット = new 返信セット(hset.添付ファイルlist);

                            // 同じUserControl内、NameがつけられていないUserControlを配置するとエラーになる為、それを回避する
                            返セット.Name = "uc返セット" + 返セット.GetHashCode();
                            返セット.DataContext = hset;
                            投返セット.stp返信セット.Children.Add(返セット);

                        }

                        if (counter != 1)
                        {
                            // 空の行用
                            TextBlock tb = new TextBlock();
                            tb.Height = 20d;

                            this.StackP1.Children.Add(tb);
                        }
                        this.StackP1.Children.Add(投返セット);
                    }
                    counter++;

                }
            }

            // 記事がセットし終わった後に、各ボタンをオンにする。
            SetButtonsVisible();


            // ページ選択ボタンを追加
            List<string> pages = new List<string>();
            Stack<string> backBages = new Stack<string>();
            if (dataset.メイン記事数 <= 0)
            {
                //ページセレクターは作らずスルーする キーワード検索時用
            }
            else
            {
                double d = (double)dataset.メイン記事数 / 10;
                int maxpage = ((int)Math.Ceiling(d) < dataset.作成グループNo + 9) ? (int)Math.Ceiling(d) : dataset.作成グループNo + 9;

                if (dataset.作成グループNo != 1)
                {
                    // (int)Math.Ceiling(d) < dataset.作成グループNo + 9 がtrueの時
                    if ((int)Math.Ceiling(d) < dataset.作成グループNo + 9)
                    {
                        // 差分を前に追加する
                        int 前に追加する差分数 = dataset.作成グループNo + 9 - (int)Math.Ceiling(d);
                        for (int i = 1; ; i++)
                        {
                            // 1になったら最後
                            if ((dataset.作成グループNo - i) == 1)
                            {
                                backBages.Push((dataset.作成グループNo - i).ToString());
                                break;
                            }
                            // 追加する差分数になったら最後
                            else if (i == 前に追加する差分数)
                            {
                                backBages.Push((dataset.作成グループNo - i).ToString());
                                break;
                            }

                            backBages.Push((dataset.作成グループNo - i).ToString());
                        }
                    }
                }

                // スタックから順番に取り出す
                foreach (var stack in backBages)
                {
                    pages.Add(stack);
                }

                for (int i = dataset.作成グループNo; i <= maxpage; i++)
                {
                    pages.Add(i.ToString());
                }

                View.Core.共通.UserControls.PageSelector ps = new View.Core.共通.UserControls.PageSelector(pages, dataset.作成グループNo <= 1 ? false : true, dataset.作成グループNo * 10 < dataset.メイン記事数 ? true : false, dataset.作成グループNo, dataset.メイン記事数);
                this.StackP1.Children.Add(ps);

            }

            // 最下の投稿フォームを追加
            if (set == null)
            {
                set = new 投稿フォーム();
            }

            this.StackP1.Children.Add(set);


            // スクロールを一番上にセットする
            this.svThread.ScrollToTop();


        }


        /// <summary>
        /// スレッドボタンがCollapasedならば、Visibieへ変化させる。
        /// スレッド追加とスレッド修正ボタンは管理者権限が必要
        /// </summary>
        private void SetButtonsVisible()
        {
            int アクティブ権限 = Core.共通.PageScope.アクティブアクセス権限;

            if (this.btスレット追加.Visibility == Visibility.Collapsed && アクティブ権限 == 0)
            {
                this.btスレット追加.Visibility = Visibility.Visible;
            }
            if (this.bt修正.Visibility == Visibility.Collapsed && アクティブ権限 == 0)
            {
                this.bt修正.Visibility = Visibility.Visible;
            }
            if (this.bt新規投稿.Visibility == Visibility.Collapsed)
            {
                this.bt新規投稿.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// 投稿入力画面を開く
        /// </summary>
        /// <param name="新規投稿か"></param>
        /// <param name="投稿記事No"></param>
        public void 投稿入力画面(掲示板書込タイプ 書込タイプ, string 投稿記事No)
        {
            TreeViewItemData tvid = this.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;
            if (tvid == null)
            {
                MessageBox.Show("投稿するスレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(tvid.Name))
            {
                return;
            }

            View.Core.TopPage.Tab.掲示板.子画面.投稿ChildWindow cw = new View.Core.TopPage.Tab.掲示板.子画面.投稿ChildWindow();
            cw.書込タイプ = 書込タイプ;
            cw.投稿記事No = 投稿記事No;
            cw.カテゴリ名 = tvid.Name;
            cw.pb暗証キー.Password = "1234";  //パスワードのデフォルト

            // 管理ユーザーは管理者
            if (Core.共通.PageScope.アクティブアクセス権限 == 0)
            {
                cw.tbInputPerson.Text = "管理者";
            }
            // 本部ユーザーは本部
            else if (Core.共通.PageScope.アクティブアクセス権限 == 1)
            {
                cw.tbInputPerson.Text = "本部";
            }
            else if (!string.IsNullOrEmpty(Core.共通.PageScope.表示名称))
            {
                cw.tbInputPerson.Text = Core.共通.PageScope.表示名称;
            }
            else
            {
                cw.tbInputPerson.Text = "";
            }

            cw.Show();
        }

        /// <summary>
        /// 記事修正入力画面を開く
        /// </summary>
        /// <param name="新規投稿か"></param>
        /// <param name="投稿記事No"></param>
        public void 記事修正入力画面(Service.File.Reader.投稿Entity 投稿Ent)
        {
            TreeViewItemData tvid = this.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;
            if (tvid == null)
            {
                MessageBox.Show("投稿するスレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(tvid.Name))
            {
                MessageBox.Show("投稿するスレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            View.Core.TopPage.Tab.掲示板.子画面.投稿ChildWindow cw = new View.Core.TopPage.Tab.掲示板.子画面.投稿ChildWindow();
            cw.書込タイプ = 掲示板書込タイプ.記事修正;
            cw.投稿記事No = 投稿Ent.No;
            cw.tbInputTitle.Text = 投稿Ent.Title;
            cw.tbInputEmail.Text = 投稿Ent.Email;
            cw.tbInputHomepageUrl.Text = 投稿Ent.HomepageUrl;
            cw.tbInputPerson.Text = 投稿Ent.投稿者名;
            cw.pb暗証キー.Password = 投稿Ent.暗証キー;
            cw.カテゴリ名 = tvid.Name;
            cw.wtbInputArticle.Text = 投稿Ent.記事;

            foreach (var list in 投稿Ent.添付ファイルlist)
            {
                Tuple<添付ファイルステート, System.IO.FileInfo, string> tp = new Tuple<添付ファイルステート, System.IO.FileInfo, string>();
                tp.Value1 = 添付ファイルステート.添付済;
                tp.Value2 = null;
                tp.Value3 = list;
                cw.添付情報管理.Add(tp);
            }

            cw.cmbSelectColors.SelectedIndex = GetColorIndex(投稿Ent.文字色);


            cw.Show();
        }

        private int GetColorIndex(string 文字色)
        {
            if (Service.File.Writer.文字色.茶色.ToString() == 文字色)
            {
                return 0;
            }
            else if (Service.File.Writer.文字色.赤.ToString() == 文字色)
            {
                return 1;
            }
            else if (Service.File.Writer.文字色.緑.ToString() == 文字色)
            {
                return 2;
            }
            else if (Service.File.Writer.文字色.青.ToString() == 文字色)
            {
                return 3;
            }
            else if (Service.File.Writer.文字色.紫.ToString() == 文字色)
            {
                return 4;
            }
            else if (Service.File.Writer.文字色.ピンク.ToString() == 文字色)
            {
                return 5;
            }
            else if (Service.File.Writer.文字色.オレンジ.ToString() == 文字色)
            {
                return 6;
            }
            else if (Service.File.Writer.文字色.黒.ToString() == 文字色)
            {
                return 7;
            }
            else
            {
                return 0;
            }

        }

        private void bt新規投稿_Click(object sender, RoutedEventArgs e)
        {
            this.svThread.ScrollToBottom();
            //this.投稿入力画面(true, "");
        }

        private void bt修正_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            TreeViewItemData tvid = this.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;
            if (tvid == null)
            {
                MessageBox.Show("修正するスレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(tvid.Name))
            {
                MessageBox.Show("修正するスレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            スレッド追加ChildWindow cw = new スレッド追加ChildWindow();
            cw.スレッド修正削除か = true;
            cw.Title = "ス レ ッ ド 修 正 削 除 画 面";
            cw.修正前スレッド名 = tvid.Name;
            cw.tbInputTitle.Text = tvid.Name;
            cw.SetImageSelector(tvid.Image);
            cw.OKButton.Content = "修 正";

            cw.Show();
        }

        private void btスレット追加_Click(object sender, RoutedEventArgs e)
        {
            スレッド追加ChildWindow cw = new スレッド追加ChildWindow();
            cw.btDeleteThread.Visibility = Visibility.Collapsed;
            cw.Show();

        }

        private void btスレッド表示_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }


            if (this.gdTop.ColumnDefinitions[0].ActualWidth == 0)
            {
                this.gdTop.ColumnDefinitions[0].Width = new GridLength(this.ColumnDefaultWidth);
                this.btスレッド表示.Visibility = Visibility.Collapsed;
                this.btスレッド非表示.Visibility = Visibility.Visible;
            }

        }

        private void btスレッド非表示_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }


            if (0 < this.gdTop.ColumnDefinitions[0].ActualWidth)
            {
                this.gdTop.ColumnDefinitions[0].Width = new GridLength(0d);
                this.btスレッド表示.Visibility = Visibility.Visible;
                this.btスレッド非表示.Visibility = Visibility.Collapsed;

            }

        }


        public void Initスレッド名幅ゼロ()
        {
            if (0 < this.gdTop.ColumnDefinitions[0].ActualWidth)
            {
                this.gdTop.ColumnDefinitions[0].Width = new GridLength(0d);
            }
            //else if (this.gdTop.ColumnDefinitions[0].ActualWidth == 0)
            //{
            //    this.gdTop.ColumnDefinitions[0].Width = new GridLength(this.ColumnDefaultWidth);
            //}
        }

    }

}
