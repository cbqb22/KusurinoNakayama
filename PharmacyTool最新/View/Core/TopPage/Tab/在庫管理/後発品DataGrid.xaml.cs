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
using System.Collections.ObjectModel;
using View.Service.File.Reader;
using System.Windows.Controls.Primitives;


namespace View.Core.TopPage.Tab.在庫管理
{
    public partial class 後発品DataGrid : UserControl
    {
        public ObservableCollection<現在庫データ> 原本itemsource;

        public 後発品DataGrid()
        {
            InitializeComponent();
            SingletonInstances.後発品DataGridInstance = this;

            this.LayoutUpdated += new EventHandler(後発品DataGrid_LayoutUpdated);

            Dispatcher.BeginInvoke
            (() =>
            {
                name後発品DataGrid.Width = LayoutRoot.ActualWidth;
                name後発品DataGrid.Height = LayoutRoot.ActualHeight;
            }
            );

        }

        void 後発品DataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke
            (() =>
            {
                name後発品DataGrid.Width = LayoutRoot.ActualWidth;
                name後発品DataGrid.Height = LayoutRoot.ActualHeight;
            }
            );
        }


        public void Set後発品ItemSource(string YJコード, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月, bool 他規格・剤形も表示する)
        {
            int result;
            // nullもしくは、１２ケタでない、もしくは先頭７ケタが数字でない場合はreturn
            if (YJコード == null ||
                YJコード.Equals("") ||
                YJコード.Length != 12 ||
                int.TryParse(YJコード.Substring(0, 7), out result) == false)
            {
                SingletonInstances.在庫管理FrameInstance.SetCursorDefault();
                return;
            }

            Service.File.Reader.FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.Get後発品検索データCompleted += new EventHandler<Get後発品検索データCompletedEventArgs>(client_Get後発品検索データCompleted);
            client.Get後発品検索データAsync(YJコード, 全期限, 期限内, 期限切, 期限指定か, 以内指定か, 期限加算月, 他規格・剤形も表示する);


        }

        void client_Get後発品検索データCompleted(object sender, Get後発品検索データCompletedEventArgs e)
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

                                if (xValue != yValue)
                                {
                                    return xValue - yValue;

                                }


                                // 店名が等しい場合は、次は先発か後発で判断する。
                                if (xValue == yValue)
                                {
                                    bool x先発か = x.後発区分.Equals("") ? false : true;
                                    bool y先発か = y.後発区分.Equals("") ? false : true;

                                    if (x先発か == y先発か)
                                    {
                                    }
                                    else if (x先発か == true)
                                    {
                                        return -1;
                                    }
                                    else
                                    {
                                        return 1;
                                    }
                                }

                                // 店名も等しくて、先発かも等しい
                                if (xValue == yValue)
                                {
                                    double x薬価;
                                    double y薬価;
                                    if (double.TryParse(x.薬価, out x薬価) == false)
                                    {
                                        return 1;
                                    }

                                    if (double.TryParse(y.薬価, out y薬価) == false)
                                    {
                                        return -1;
                                    }


                                    if (x薬価 > y薬価)
                                    {
                                        return -1;
                                    }

                                    if (y薬価 > x薬価)
                                    {
                                        return 1;
                                    }
                                }

                                // 店名も等しくて、先発かも等しく、薬価も等しい
                                if (xValue == yValue)
                                {
                                    return x.医薬品名.CompareTo(y.医薬品名);
                                }


                                return xValue - yValue;
                                //return yValue - xValue;
                            }
                            );



                        this.name後発品DataGrid.ItemsSource = list;
                        Core.共通.SingletonInstances.在庫管理FrameInstance.Set在庫検索結果Result(list.Count, rdataset.検索キーワード);

                    }
                    else
                    {
                        Core.共通.SingletonInstances.在庫管理FrameInstance.tbl検索結果.Text = rdataset.エラーメッセージ;
                        // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                        // MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                    }


                }
                else
                {
                    MessageBox.Show(e.Error.Message + e.Error.StackTrace);
                }

                SingletonInstances.在庫管理FrameInstance.SetCursorDefault();

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

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            TextBlock tbl = sender as TextBlock;

            if (tbl == null)
            {
                return;
            }


            FrameworkElement fe = VisualTreeHelper.GetParent(this.Parent) as FrameworkElement;
            while (true)
            {
                if (fe == null)
                {
                    return;
                }
                if (fe is 在庫管理Frame)
                {
                    ((在庫管理Frame)fe).SearchTextBox1.Text = tbl.Text;

                    return;
                }
                fe = fe.Parent as FrameworkElement;
            }

        }





    }
}
