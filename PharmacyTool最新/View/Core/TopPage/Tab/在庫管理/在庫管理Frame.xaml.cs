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
using View.Core.共通;
using View.Service.File.Reader;
using System.Collections.ObjectModel;
using View.Util.Common;
using System.IO;
using System.Text;


namespace View.Core.TopPage.Tab.在庫管理
{
    public partial class 在庫管理Frame : UserControl
    {
        private Dictionary<Button, bool> activeButton = new Dictionary<Button, bool>();

        public 在庫管理Frame()
        {
            InitializeComponent();

            SingletonInstances.在庫管理FrameInstance = this;

            SetDefalutactiveButton();


            this.Loaded += new RoutedEventHandler(在庫管理Frame_Loaded);

        }

        void 在庫管理Frame_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetSearchTextBoxFocus();
        }

        public void SetSearchTextBoxFocus()
        {
            System.Windows.Browser.HtmlPage.Plugin.Focus();
            this.SearchTextBox1.Focus();

        }


        private void SearchTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                foreach (var content in SearchButtonStack.Children)
                {
                    Button sButton = content as Button;

                    if (sButton == null)
                    {
                        return;
                    }

                    var actBtn = activeButton.Where(a => a.Value == true);

                    if (actBtn.Count() != 1)
                    {
                        return;
                    }

                    if (sButton == actBtn.First().Key)
                    {
                        検索Button_Click(sButton, null);
                    }
                }
            }


        }

        private void SetDefalutactiveButton()
        {
            activeButton[this.現在庫Button] = true;
            activeButton[this.後発品Button] = false;
            activeButton[this.使用量Button] = false;
            activeButton[this.不動品Button] = false;
        }


        private void SetactiveButtonClear()
        {
            activeButton[this.現在庫Button] = false;
            activeButton[this.後発品Button] = false;
            activeButton[this.使用量Button] = false;
            activeButton[this.不動品Button] = false;
        }

        private void SetactiveButton(Button sender)
        {
            Button searchAct = null;
            foreach (var b in activeButton)
            {
                if (b.Key.GetHashCode() == sender.GetHashCode())
                {
                    searchAct = b.Key;
                }
                else
                {
                }
            }

            if (searchAct == null)
            {
                throw new Exception("ブラウザを再起動して下さい。 \r\n エラー詳細：Activeなボタンがありません。");
            }

            SetactiveButtonClear();

            activeButton[searchAct] = true;


        }


        private void 現在庫検索()
        {

            // 空文字ならば、検索しない。
            if (this.SearchTextBox1.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {
                // カーソルをもとの状態にする
                this.SetCursorDefault();

                return;
            }


            bool 全期限 = false;
            bool 期限内 = false;
            bool 期限切 = false;
            bool 期限指定 = false;
            bool 以内指定か = false;
            int 期限加算月 = 0;


            在庫管理Frame instance = SingletonInstances.在庫管理FrameInstance;
            if (instance.cmb使用期限日.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else if (instance.cmb使用期限日.SelectedIndex == 1)
            {
                期限内 = true;
            }
            else if (instance.cmb使用期限日.SelectedIndex == 2)
            {
                期限切 = true;
            }
            else if (3 <= instance.cmb使用期限日.SelectedIndex)
            {
                期限指定 = true;
                Tuple<int, bool> tp = Set期限加算月();
                期限加算月 = tp.Value1;
                以内指定か = tp.Value2;

            }



            Service.File.Reader.FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.Get現在庫検索データCompleted += new EventHandler<Get現在庫検索データCompletedEventArgs>(client_Get現在庫検索データCompleted);
            client.Get現在庫検索データAsync(SearchTextBox1.Text, 全期限, 期限内, 期限切, 期限指定, 以内指定か, 期限加算月);



        }

        Tuple<int, bool> Set期限加算月()
        {
            Tuple<int, bool> result = new Tuple<int, bool>();
            if (this.cmb使用期限日.SelectedIndex == 3)
            {
                result.Value1 = 1;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 4)
            {
                result.Value1 = 1;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 5)
            {
                result.Value1 = 2;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 6)
            {
                result.Value1 = 2;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 7)
            {
                result.Value1 = 3;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 8)
            {
                result.Value1 = 3;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 9)
            {
                result.Value1 = 4;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 10)
            {
                result.Value1 = 4;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 11)
            {
                result.Value1 = 5;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 12)
            {
                result.Value1 = 5;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 13)
            {
                result.Value1 = 6;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 14)
            {
                result.Value1 = 6;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 15)
            {
                result.Value1 = 7;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 16)
            {
                result.Value1 = 7;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 17)
            {
                result.Value1 = 8;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 18)
            {
                result.Value1 = 8;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 19)
            {
                result.Value1 = 9;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 20)
            {
                result.Value1 = 9;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 21)
            {
                result.Value1 = 10;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 22)
            {
                result.Value1 = 10;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 23)
            {
                result.Value1 = 11;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 24)
            {
                result.Value1 = 11;
                result.Value2 = false;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 25)
            {
                result.Value1 = 12;
                result.Value2 = true;
                return result;
            }
            else if (this.cmb使用期限日.SelectedIndex == 26)
            {
                result.Value1 = 12;
                result.Value2 = false;
                return result;
            }
            else
            {
                throw new Exception("使用期限日が不正です。");
            }
        }

        void client_Get現在庫検索データCompleted(object sender, Get現在庫検索データCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    

                    //WCFでサーバー側のサービスのGenericsクラスを使うと以下の様な英数字がつくみたい
                    在庫リターンデータセットOf現在庫データZYtYGqK_P rdataset = e.Result as 在庫リターンデータセットOf現在庫データZYtYGqK_P;
                    if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
                    {

                        var list = rdataset.検索結果データlist.ToList();
                        var 表示順序dic = Core.共通.PageScope.表示順序;

                        // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                        var CopyDic = View.Util.Common.GenericUtil.Copy(表示順序dic);
                        int cnt = CopyDic.Count + 1;

                        list.Sort(
                            delegate(現在庫データ x, 現在庫データ y)
                            {
                                int xValue = 0;
                                int yValue = 0;
                                foreach (var d in CopyDic)
                                {
                                    if (d.Value.Equals(x.店名))
                                    {
                                        xValue = d.Key;
                                    }

                                    if (d.Value.Equals(y.店名))
                                    {
                                        yValue = d.Key;
                                    }
                                }

                                // 一致するものがなかったら、臨時のcntの番号にする（表示順序が後ろ）
                                bool xy店名が等しい = false;
                                if (xValue == 0)
                                {
                                    if (x.店名.Equals(y.店名))
                                    {
                                        xy店名が等しい = true;
                                    }

                                    xValue = cnt;
                                    CopyDic.Add(cnt, x.店名);
                                    cnt++;
                                }

                                if (yValue == 0)
                                {
                                    if (xy店名が等しい)
                                    {
                                        yValue = xValue;
                                    }
                                    else
                                    {
                                        yValue = cnt;
                                        CopyDic.Add(cnt, y.店名);
                                        cnt++;
                                    }
                                }

                                // 店名が等しい場合は、医薬品名のアイウエオ順
                                if (xValue == yValue)
                                {
                                    return x.医薬品名.CompareTo(y.医薬品名);

                                }


                                return xValue - yValue;
                            }
                            );



                        this.現在庫DataGrid1.name現在庫DataGrid.ItemsSource = list;
                        this.Set在庫検索結果Result(list.Count, rdataset.検索キーワード);



                    }
                    else
                    {
                        // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                        //MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                        this.tbl検索結果.Text = rdataset.エラーメッセージ;
                    }
                }
                else
                {
                    MessageBox.Show(e.Error.Message + e.Error.StackTrace);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // カーソルをもとの状態にする
                this.SetCursorDefault();

                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync",Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender,paramlist);
            }



        }


        private void 不動品検索()
        {

            // 空文字ならば、検索しない。
            if (this.SearchTextBox1.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {
                // カーソルをもとの状態にする
                this.SetCursorDefault();


                return;
            }


            bool 全期限 = false;
            bool 期限内 = false;
            bool 期限切 = false;
            bool 期限指定 = false;
            bool 以内指定か = false;
            int 期限加算月 = 0;


            在庫管理Frame instance = SingletonInstances.在庫管理FrameInstance;
            if (instance.cmb使用期限日.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else if (instance.cmb使用期限日.SelectedIndex == 1)
            {
                期限内 = true;
            }
            else if (instance.cmb使用期限日.SelectedIndex == 2)
            {
                期限切 = true;
            }
            else if (3 <= instance.cmb使用期限日.SelectedIndex)
            {
                期限指定 = true;
                Tuple<int, bool> tp = Set期限加算月();
                期限加算月 = tp.Value1;
                以内指定か = tp.Value2;

            }



            Service.File.Reader.FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.Open不動品CSVCompleted += new EventHandler<Open不動品CSVCompletedEventArgs>(client_Open不動品CSVCompleted);
            client.Open不動品CSVAsync(this.SearchTextBox1.Text, 全期限, 期限内, 期限切, 期限指定, 以内指定か, 期限加算月);



        }

        void client_Open不動品CSVCompleted(object sender, Open不動品CSVCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    在庫リターンデータセットOf不動品データZYtYGqK_P rdataset = e.Result;

                    if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
                    {

                        ObservableCollection<不動品データ> data = rdataset.検索結果データlist;

                        var list = data.ToList();
                        var 表示順序dic = Core.共通.PageScope.表示順序;

                        // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                        var CopyDic = View.Util.Common.GenericUtil.Copy(表示順序dic);
                        int cnt = CopyDic.Count + 1;

                        list.Sort(
                            delegate(不動品データ x, 不動品データ y)
                            {
                                int xValue = 0;
                                int yValue = 0;
                                foreach (var d in CopyDic)
                                {
                                    if (d.Value.Equals(x.店名))
                                    {
                                        xValue = d.Key;
                                    }

                                    if (d.Value.Equals(y.店名))
                                    {
                                        yValue = d.Key;
                                    }
                                }

                                // 一致するものがなかったら、臨時のcntの番号にする（表示順序が後ろ）
                                bool xy店名が等しい = false;
                                if (xValue == 0)
                                {
                                    if (x.店名.Equals(y.店名))
                                    {
                                        xy店名が等しい = true;
                                    }

                                    xValue = cnt;
                                    CopyDic.Add(cnt, x.店名);
                                    cnt++;
                                }

                                if (yValue == 0)
                                {
                                    if (xy店名が等しい)
                                    {
                                        yValue = xValue;
                                    }
                                    else
                                    {
                                        yValue = cnt;
                                        CopyDic.Add(cnt, y.店名);
                                        cnt++;
                                    }
                                }

                                // 店名が等しい場合は、医薬品名のアイウエオ順
                                if (xValue == yValue)
                                {
                                    return x.医薬品名.CompareTo(y.医薬品名);

                                }



                                return xValue - yValue;
                                //return yValue - xValue;
                            }
                            );


                        this.不動品DataGrid1.name不動品DataGrid.ItemsSource = list;
                        this.Set在庫検索結果Result(list.Count, rdataset.検索キーワード);
                    }
                    else
                    {
                        // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                        //MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                        this.tbl検索結果.Text = rdataset.エラーメッセージ;
                    }

                }
                else
                {
                    MessageBox.Show(e.Error.Message + e.Error.StackTrace);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // カーソルをもとの状態にする
                this.SetCursorDefault();

                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);


            }


        }

        int Set使用量検索期限加算月()
        {
            if (this.cmb使用量検索.SelectedIndex == -1)
            {
                // デフォルトで３ヶ月に設定
                return 3;
            }
            else if (1 <= this.cmb使用量検索.SelectedIndex)
            {
                return this.cmb使用量検索.SelectedIndex;
            }
            else
            {
                throw new Exception("使用量検索の選択が正しくなりません。");
            }
        }

        private void 使用量検索()
        {

            // 空文字ならば、検索しない。
            if (this.SearchTextBox1.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {

                // カーソルをもとの状態にする
                this.SetCursorDefault();

                return;
            }



            bool 全期限 = false;
            int 期限加算月 = 3;

            if (this.cmb使用量検索.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else
            {
                期限加算月 = Set使用量検索期限加算月();
            }

            this.使用量DataGrid1.SetDataGridItemSource(this.SearchTextBox1.Text, 全期限, 期限加算月);

        }

        private void 検索Button_Click(object sender, RoutedEventArgs e)
        {
            Button sButton = sender as Button;

            if (sButton == null)
            {
                return;
            }

            // 各検索ボタンの色を変える。
            ButtonColorChange(sButton);

            // 押されたボタンのActiveを変える。
            SetactiveButton(sButton);

            // 不要な検索DataGridを見えなくする。
            foreach (var col in this.gdDataGrids.Children)
            {
                col.Visibility = Visibility.Collapsed;
            }

            try
            {
                switch (sButton.Name)
                {
                    case ("現在庫Button"): 現在庫Click処理(); break;
                    case ("使用量Button"): 使用量Click処理(); break;
                    case ("不動品Button"): 不動品Click処理(); break;
                    case ("後発品Button"): 後発品Click処理(); break;
                    default: break;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + exp.StackTrace);

                // カーソルをもとの状態にする
                // ここはFinallyにしてしまうと、非同期呼び出しが戻ってくる前にすぐにArrowになってしまうので、エラー発生時のみ
                this.SetCursorDefault();
            }

        }


        public void SetCursor(Cursor cursor)
        {
            if (cursor != null)
            {
                // 大タブのユーザーコントロールレベルでCursorを変更してしまうと、もとに戻すのが大変なのでやらない
                //SingletonInstances.TabItemControlInstance.uc在庫管理TabItem.Cursor = cursor;

                this.Cursor = cursor;
                SingletonInstances.TabItemControlInstance.在庫管理TabItem.Cursor = cursor;
                SingletonInstances.TabItemControlInstance.uc在庫管理TabItem.在庫検索TabItem.Cursor = cursor;
                SingletonInstances.TabItemControlInstance.uc在庫管理TabItem.在庫更新TabItem.Cursor = cursor;
                this.SearchTextBox1.Cursor = cursor;


            }
        }

        /// <summary>
        /// SetCursor(Cursor cursor)で変更したCursorを元に戻す
        /// </summary>
        public void SetCursorDefault()
        {
            this.Cursor = Cursors.Arrow;
            SingletonInstances.TabItemControlInstance.在庫管理TabItem.Cursor = Cursors.Hand;
            SingletonInstances.TabItemControlInstance.uc在庫管理TabItem.在庫検索TabItem.Cursor = Cursors.Hand;
            SingletonInstances.TabItemControlInstance.uc在庫管理TabItem.在庫更新TabItem.Cursor = Cursors.Hand;
            this.SearchTextBox1.Cursor = Cursors.IBeam;
        }

        private void 現在庫Click処理()
        {
            // カーソルをWait状態にする
            SetCursor(Cursors.Wait);

            // ItemoSourceを空にしてから、表示し、その後検索。
            this.現在庫DataGrid1.name現在庫DataGrid.ItemsSource = new ObservableCollection<現在庫データ>();
            this.現在庫DataGrid1.Visibility = Visibility.Visible;

            現在庫検索();

        }


        private void 使用量Click処理()
        {
            // カーソルをWait状態にする
            SetCursor(Cursors.Wait);

            // ItemoSourceを空にしてから、表示し、その後検索。
            this.使用量DataGrid1.name使用量DataGrid.ItemsSource = new ObservableCollection<薬局使用量データ>();
            this.使用量DataGrid1.Visibility = Visibility.Visible;

            使用量検索();

        }


        private void 不動品Click処理()
        {
            // カーソルをWait状態にする
            SetCursor(Cursors.Wait);

            // ItemoSourceを空にしてから、表示し、その後検索。
            this.不動品DataGrid1.name不動品DataGrid.ItemsSource = new ObservableCollection<不動品データ>();
            this.不動品DataGrid1.Visibility = Visibility.Visible;

            不動品検索();

        }

        private void 後発品Click処理()
        {
            // ItemoSourceを空にしてから、表示し、その後検索。
            this.後発品DataGrid1.name後発品DataGrid.ItemsSource = new ObservableCollection<現在庫データ>();
            this.後発品DataGrid1.Visibility = Visibility.Visible;

            // 空文字ならば、検索しない。
            if (this.SearchTextBox1.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {

                // カーソルをもとの状態にする
                this.SetCursorDefault();

                return;
            }

            // カーソルをWait状態にする
            SetCursor(Cursors.Wait);


            bool 全期限 = false;
            bool 期限内 = false;
            bool 期限切 = false;
            bool 期限指定 = false;
            bool 以内指定か = false;
            int 期限加算月 = 0;
            bool 他規格・剤形も表示する = false;


            在庫管理Frame instance = SingletonInstances.在庫管理FrameInstance;
            if (instance.cmb使用期限日.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else if (instance.cmb使用期限日.SelectedIndex == 1)
            {
                期限内 = true;
            }
            else if (instance.cmb使用期限日.SelectedIndex == 2)
            {
                期限切 = true;
            }
            else if (3 <= instance.cmb使用期限日.SelectedIndex)
            {
                期限指定 = true;
                Tuple<int, bool> tp = Set期限加算月();
                期限加算月 = tp.Value1;
                以内指定か = tp.Value2;

            }


            if (instance.cmb後発品検索.SelectedIndex == 0)
            {
                他規格・剤形も表示する = false;
            }
            else
            {
                他規格・剤形も表示する = true;
            }



            後発品DataGrid1.Set後発品ItemSource(this.SearchTextBox1.Text, 全期限, 期限内, 期限切, 期限指定, 以内指定か, 期限加算月, 他規格・剤形も表示する);
        }

        /// <summary>
        /// 在庫検索の各ボタン押下時にボタンの色を変化させる。
        /// </summary>
        /// <param name="b">押されたボタン</param>
        private void ButtonColorChange(Button b)
        {
            foreach (var content in SearchButtonStack.Children)
            {
                Button cButton = content as Button;

                if (cButton == null)
                {
                    continue;
                }

                if (cButton == b)
                {
                    cButton.Background = new SolidColorBrush(Colors.Magenta);
                }
                else
                {
                    cButton.Background = new SolidColorBrush(Colors.Gray);

                }
            }
        }

        public void Set在庫検索結果Result(int 検索ヒット数, string 検索キーワード)
        {
            if (検索ヒット数 <= 0)
            {
                this.tbl検索結果.Text = string.Format("キーワード【{0}】 で検索した結果、該当するデータがありません。", 検索キーワード);
            }
            else
            {
                this.tbl検索結果.Text = string.Format("キーワード【{0}】 で検索した結果、{1}件の該当するデータがありました。", 検索キーワード, 検索ヒット数.ToString());
            }

        }

        private void btnCSV出力_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            // 不要な検索DataGridを見えなくする。
            foreach (var col in this.gdDataGrids.Children)
            {
                string ActiveDataGrigName = "";
                if (col.Visibility == Visibility.Visible)
                {
                    ActiveDataGrigName = col.GetType().Name;
                }
                else
                {
                    continue;
                }

                if (ActiveDataGrigName.Equals("現在庫DataGrid"))
                {
                    List<現在庫データ> itemsource = this.現在庫DataGrid1.name現在庫DataGrid.ItemsSource as List<現在庫データ>;
                    if (itemsource == null)
                    {
                        MessageBox.Show("検索結果が０件の為、CSV出力を中止しました。");
                        return;
                    }
                    if (itemsource.Count < 1)
                    {
                        MessageBox.Show("検索結果が０件の為、CSV出力を中止しました。");
                        return;
                    }


                    // Fileの保存
                    string filter = string.Format("拡張子(*.{0})|*.{0}", "csv");
                    string defaultext = "csv";


                    Stream st = null;
                    FileStream fs = null;
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

                        StringBuilder line = new StringBuilder();

                        // ヘッダー
                        line.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", "店名", "個別医薬品コード", "医薬品名", "在庫数", "使用期限", "薬価", "メーカー", "最終更新日時"));

                        foreach (var row in itemsource)
                        {
                            line.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", row.店名, row.個別医薬品コード, row.医薬品名, row.在庫数, row.使用期限, row.薬価, row.製造会社, row.最終更新日時));
                        }

                        st = new MemoryStream(Encoding.UTF8.GetBytes(line.ToString()));

                        DoSave(st, fs);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                        return;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                        if (st != null)
                        {
                            st.Close();
                        }

                    }

                    MessageBox.Show("CSVを出力しました。");
                    return;


                }
                else if (ActiveDataGrigName.Equals("使用量DataGrid"))
                {
                    List<薬局使用量データ> itemsource = this.使用量DataGrid1.name使用量DataGrid.ItemsSource as List<薬局使用量データ>;
                    if (itemsource == null)
                    {
                        MessageBox.Show("検索時にエラーが起きました。\r\n再度検索後、CSV出力を行って下さい。");
                        return;
                    }
                    if (itemsource.Count < 1)
                    {
                        MessageBox.Show("検索結果が０件の為、CSV出力を中止しました。");
                        return;
                    }

                    // Fileの保存
                    string filter = string.Format("拡張子(*.{0})|*.{0}", "csv");
                    string defaultext = "csv";


                    Stream st = null;
                    FileStream fs = null;
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

                        StringBuilder line = new StringBuilder();

                        // ヘッダー
                        line.Append(string.Format("{0},{1},{2},{3},{4},{5}\r\n", "店名", "使用年月", "医薬品名", "使用量", "薬価", "最終更新日時"));

                        foreach (var row in itemsource)
                        {
                            line.Append(string.Format("{0},{1},{2},{3},{4},{5}\r\n", row.店名, row.使用年月, row.医薬品名, row.使用量, row.薬価, row.最終更新日時));
                        }

                        st = new MemoryStream(Encoding.UTF8.GetBytes(line.ToString()));

                        DoSave(st, fs);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                        return;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                        if (st != null)
                        {
                            st.Close();
                        }

                    }

                    MessageBox.Show("CSVを出力しました。");
                    return;


                }
                else if (ActiveDataGrigName.Equals("不動品DataGrid"))
                {
                    List<不動品データ> itemsource = this.不動品DataGrid1.name不動品DataGrid.ItemsSource as List<不動品データ>;
                    if (itemsource == null)
                    {
                        MessageBox.Show("検索時にエラーが起きました。\r\n再度検索後、CSV出力を行って下さい。");
                        return;
                    }
                    if (itemsource.Count < 1)
                    {
                        MessageBox.Show("検索結果が０件の為、CSV出力を中止しました。");
                        return;
                    }

                    // Fileの保存
                    string filter = string.Format("拡張子(*.{0})|*.{0}", "csv");
                    string defaultext = "csv";


                    Stream st = null;
                    FileStream fs = null;
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

                        StringBuilder line = new StringBuilder();

                        // ヘッダー
                        line.Append(string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", "店名", "個別医薬品コード", "医薬品名", "在庫数", "使用期限", "薬価","最終更新日時"));

                        foreach (var row in itemsource)
                        {
                            line.Append(string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", row.店名, row.個別医薬品コード, row.医薬品名, row.在庫数, row.使用期限, row.薬価, row.最終更新日時));
                        }

                        st = new MemoryStream(Encoding.UTF8.GetBytes(line.ToString()));

                        DoSave(st, fs);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                        return;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                        if (st != null)
                        {
                            st.Close();
                        }

                    }

                    MessageBox.Show("CSVを出力しました。");
                    return;



                }
                else if (ActiveDataGrigName.Equals("後発品DataGrid"))
                {
                    List<現在庫データ> itemsource = this.後発品DataGrid1.name後発品DataGrid.ItemsSource as List<現在庫データ>;
                    if (itemsource == null)
                    {
                        MessageBox.Show("検索時にエラーが起きました。\r\n再度検索後、CSV出力を行って下さい。");
                        return;
                    }
                    if (itemsource.Count < 1)
                    {
                        MessageBox.Show("検索結果が０件の為、CSV出力を中止しました。");
                        return;
                    }

                    // Fileの保存
                    string filter = string.Format("拡張子(*.{0})|*.{0}", "csv");
                    string defaultext = "csv";


                    Stream st = null;
                    FileStream fs = null;
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

                        StringBuilder line = new StringBuilder();

                        // ヘッダー
                        line.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\r\n", "店名", "先発品", "個別医薬品コード", "医薬品名", "在庫数", "使用期限", "薬価", "メーカー", "最終更新日時"));

                        foreach (var row in itemsource)
                        {
                            line.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\r\n", row.店名,row.後発区分.Equals("後発品") ? "" : "●", row.個別医薬品コード, row.医薬品名, row.在庫数, row.使用期限, row.薬価, row.製造会社, row.最終更新日時));
                        }

                        st = new MemoryStream(Encoding.UTF8.GetBytes(line.ToString()));

                        DoSave(st, fs);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                        return;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                        if (st != null)
                        {
                            st.Close();
                        }

                    }

                    MessageBox.Show("CSVを出力しました。");
                    return;

                }
                else
                {
                    MessageBox.Show("検索後、CSV出力を行って下さい。");
                    return;
                }

            }
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);

            }

            fs.Flush();
        }

        private void DoSave(Stream input, FileStream fs)
        {
            SaveFile(input, fs);
        }


    }
}
