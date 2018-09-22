using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;
using View.Core.TopPage.Tab.掲示板.メイン;

namespace View
{
    public partial class 返信セット : UserControl
    {
        public 返信セット()
        {
            // 変数を初期化する必要があります
            InitializeComponent();
            Set削除ボタン表示設定();

            this.記事TextBlock.GotFocus += new RoutedEventHandler(記事TextBlock_GotFocus);
            this.記事TextBlock.LostFocus += new RoutedEventHandler(記事TextBlock_LostFocus);
        }


        public 返信セット(ObservableCollection<string> 添付ファイルパス)
        {
            InitializeComponent();

            Set削除ボタン表示設定();

            this.記事TextBlock.GotFocus += new RoutedEventHandler(記事TextBlock_GotFocus);
            this.記事TextBlock.LostFocus += new RoutedEventHandler(記事TextBlock_LostFocus);


            foreach (var filepath in 添付ファイルパス)
            {
                string[] spl = filepath.Split('/');
                if (spl.Length < 1)
                {
                    continue;
                }

                string ファイル名 = spl[spl.Length - 1];

                string[] ext = spl[spl.Length - 1].Split('.');
                if (ext.Length < 1)
                {
                    continue;
                }

                string 拡張子 = ext[ext.Length - 1];

                StackPanel stp = new StackPanel();
                stp.Orientation = Orientation.Horizontal;
                stp.Background = new SolidColorBrush(Colors.Transparent);
                stp.MouseEnter += new MouseEventHandler(stp_MouseEnter);
                stp.MouseLeave += new MouseEventHandler(stp_MouseLeave);
                stp.MouseLeftButtonUp += new MouseButtonEventHandler(stp_MouseLeftButtonUp);

                Uri uriSource = new Uri("/etc/Icon/txt.png", UriKind.Relative);

                if (拡張子.Equals("xls") || 拡張子.Equals("xlsx"))
                {
                    uriSource = new Uri("/etc/Icon/xls.png", UriKind.Relative);
                }
                else if (拡張子.Equals("doc") || 拡張子.Equals("docx"))
                {
                    uriSource = new Uri("/etc/Icon/doc.png", UriKind.Relative);
                }
                else if (拡張子.Equals("ppt") || 拡張子.Equals("pptx"))
                {
                    uriSource = new Uri("/etc/Icon/ppt.png", UriKind.Relative);
                }
                else if (拡張子.Equals("pdf"))
                {
                    uriSource = new Uri("/etc/Icon/pdf.png", UriKind.Relative);
                }
                else if (拡張子.Equals("txt"))
                {
                    // そのまま
                }

                Image img = new Image();
                img.Source = new BitmapImage(uriSource);
                img.Width = 20.0;
                img.Height = 20.0;

                TextBox tbl = new TextBox();
                tbl.Background = null;
                tbl.BorderBrush = null;
                tbl.CaretBrush = null;
                tbl.Text = ファイル名;
                tbl.Foreground = new SolidColorBrush(Colors.Blue);

                Label lb = new Label();
                lb.Content = filepath;
                lb.Visibility = Visibility.Collapsed;

                stp.Children.Add(img);
                stp.Children.Add(tbl);
                stp.Children.Add(lb);

                stp添付画像.Children.Add(stp);



            }
        }


        private void 返信Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;

            while (true)
            {
                if (fe == null)
                {
                    return;
                }

                if (fe is StackPanel)
                {
                    break;
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
            }

            StackPanel sp = fe as StackPanel;

            var childs = sp.Children;

            string no = "";
            foreach (var child in childs)
            {
                if (child is TextBlock)
                {
                    if (((TextBlock)child).Name.Equals("NoTextBlock"))
                    {
                        no = ((TextBlock)child).Text.Replace("No:", "");
                        break;
                    }
                }
            }

        }
        void stp_MouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel stp = sender as StackPanel;

            FrameworkElement fe = VisualTreeHelper.GetChild(stp, 1) as FrameworkElement;

            if (fe == null)
            {
                return;
            }
            if (fe is TextBox)
            {
                ((TextBox)fe).Foreground = new SolidColorBrush(Colors.Blue);

                return;
            }
        }

        void stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StackPanel stp = sender as StackPanel;

            // ファイル名
            FrameworkElement fe = VisualTreeHelper.GetChild(stp, 1) as FrameworkElement;

            // ファイルパス
            FrameworkElement fe2 = VisualTreeHelper.GetChild(stp, 2) as FrameworkElement;

            if (fe == null)
            {
                return;
            }
            if (fe is TextBox)
            {
                string ファイル名 = ((TextBox)fe).Text;

                if (fe2 == null)
                {
                    return;
                }

                if (fe2 is Label)
                {

                    var ent = this.DataContext as Service.File.Reader.返信Entity;
                    if (ent == null)
                    {
                        return;
                    }


                    string 記事No = ent.No.Replace("No:", "");
                    string prepath = System.IO.Path.Combine(ent.カテゴリ名, 記事No);
                    string FileName = ((Label)fe2).Content.ToString();
                    string 相対ファイルパス = System.IO.Path.Combine(prepath, FileName);

                    string filter = "";
                    string defaultext = "";
                    FileStream fs;

                    if (((TextBox)fe).Text.Contains("."))
                    {
                        string[] sepa = ファイル名.Split('.');
                        string 拡張子 = sepa[sepa.GetLength(0) - 1];
                        filter = string.Format("拡張子(*.{0})|*.{0}", 拡張子);
                        defaultext = 拡張子;
                    }
                    else
                    {
                        filter = "All files(*.*)|*.*";
                    }

                    try
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog()
                        {
                            Filter = filter,
                            DefaultExt = defaultext
                        };
                        if (saveFileDialog.ShowDialog() != true)
                        {
                            return;
                        }

                        fs = (FileStream)saveFileDialog.OpenFile();

                        Core.共通.FileOperation.FileDownloader fdl = new Core.共通.FileOperation.FileDownloader(filter, defaultext, 相対ファイルパス, "掲示板資料", fs, FileName);
                        fdl.StartDownload();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }



                    //System.Diagnostics.Debug.WriteLine(((Label)fe2).Content.ToString());
                    //Uri uri = new Uri(System.IO.Path.Combine(Core.共通.Settings.Download掲示板FileRootPath,((Label)fe2).Content.ToString()),UriKind.Absolute);
                    //// JavaScriptのwindowオブジェクト(実際はHtmlWindowクラス)
                    //var windowObj = HtmlPage.Window;
                    //if (windowObj.Confirm(((TextBlock)fe).Text + "をダウンロードしますか？"))
                    //    windowObj.Navigate(uri, "_self");

                }
            }
        }



        void stp_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel stp = sender as StackPanel;

            FrameworkElement fe = VisualTreeHelper.GetChild(stp, 1) as FrameworkElement;

            if (fe == null)
            {
                return;
            }
            if (fe is TextBox)
            {
                ((TextBox)fe).Foreground = new SolidColorBrush(Colors.Green);

                return;
            }

        }

        private void bt削除_Click(object sender, RoutedEventArgs e)
        {

            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            if (MessageBox.Show("この記事を削除しますか？", "確認", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }


            // カテゴリ名を取得
            TreeViewItemData tvid = Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;
            if (tvid == null)
            {
                MessageBox.Show("スレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(tvid.Name))
            {
                MessageBox.Show("スレッドを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }


            // 記事Noを取得
            string 記事No = Get記事NoFromPressButton(bt);

            if (string.IsNullOrEmpty(記事No))
            {
                MessageBox.Show("記事Noがない為、削除できませんでした。", "エラー", MessageBoxButton.OK);
                return;
            }



            Service.File.Writer.FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
            client.掲示板データ削除Completed += new EventHandler<Service.File.Writer.掲示板データ削除CompletedEventArgs>(client_掲示板データ削除Completed);
            client.掲示板データ削除Async(tvid.Name, 記事No, "", true);

        }

        void client_掲示板データ削除Completed(object sender, Service.File.Writer.掲示板データ削除CompletedEventArgs e)
        {
            try
            {

                if (e.Error == null)
                {
                    MessageBox.Show(e.Result, "確認", MessageBoxButton.OK);

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
        /// 記事内の押された返信、削除ボタンからその記事Noを取する。
        /// </summary>
        /// <param name="bt"></param>
        private string Get記事NoFromPressButton(Button bt)
        {
            FrameworkElement fe = bt as FrameworkElement;

            while (true)
            {
                if (fe == null)
                {
                    return "";
                }

                if (fe is StackPanel)
                {
                    break;
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
            }

            StackPanel sp = fe as StackPanel;

            var childs = sp.Children;

            string no = "";
            foreach (var child in childs)
            {
                if (child is TextBlock)
                {
                    if (((TextBlock)child).Name.Equals("NoTextBlock"))
                    {
                        no = ((TextBlock)child).Text.Replace("No:", "");
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(no))
            {
                return "";
            }

            return no;

        }

        public void Set削除ボタン表示設定()
        {
            if (Core.共通.PageScope.アクティブアクセス権限 == -1)
            {
                this.bt削除.Visibility = Visibility.Collapsed;
                return;
            }

            // 管理ユーザーもしくは本部ユーザーは削除ボタンを設定
            if (Core.共通.PageScope.アクティブアクセス権限 == 0 ||
                Core.共通.PageScope.アクティブアクセス権限 == 1)
            {
                this.bt削除.Visibility = Visibility.Visible;
                return;
            }

            this.bt削除.Visibility = Visibility.Collapsed;
            return;

        }

        void 記事TextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb == null)
            {
                return;
            }

            this.記事TextBlock.IsReadOnly = true;
        }

        void 記事TextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb == null)
            {
                return;
            }

            this.記事TextBlock.IsReadOnly = false;

        }


    }
}