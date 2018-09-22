using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ComponentModel;
using View.Service.File.Reader;
using View.Core.TopPage.Tab.在庫管理;
using System.Collections.Generic;
using View.Core.共通;
using System.Linq;

namespace View.Util.ServiceUtil.Call
{
    public class FileReader
    {
        public void CallFileReader(string Filepath)
        {
            Service.File.Reader.FileReaderClient webclient = ServiceUtil.ReferenceCreater.GetFileReaderClient();

            webclient.OpenCSVCompleted += new EventHandler<OpenCSVCompletedEventArgs>(webclient_OpenCSVCompleted);

            webclient.OpenCSVAsync("");


        }

        public void CallFileReader2(string 検索文字列, bool 全期間, int 期限加算月)
        {
            Service.File.Reader.FileReaderClient webclient = ServiceUtil.ReferenceCreater.GetFileReaderClient();

            webclient.Open使用量CSVCompleted += new EventHandler<Open使用量CSVCompletedEventArgs>(webclient_Open使用量CSVCompleted);

            webclient.Open使用量CSVAsync(検索文字列, 全期間, 期限加算月);
        }


        public void CallFileReader3(string 検索文字列, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月)
        {
            Service.File.Reader.FileReaderClient webclient = ServiceUtil.ReferenceCreater.GetFileReaderClient();

            webclient.Open不動品CSVCompleted += new EventHandler<Open不動品CSVCompletedEventArgs>(webclient_Open不動品CSVCompleted);

            webclient.Open不動品CSVAsync(検索文字列, 全期限, 期限内, 期限切, 期限指定か, 以内指定か, 期限加算月);



        }


        public void CallFileReader4(string 検索文字列, bool 全期間, int 期限加算月)
        {
            Service.File.Reader.FileReaderClient webclient = ServiceUtil.ReferenceCreater.GetFileReaderClient();

            webclient.Open使用量2CSVCompleted += new EventHandler<Open使用量2CSVCompletedEventArgs>(webclient_Open使用量2CSVCompleted);

            webclient.Open使用量2CSVAsync(検索文字列, 全期間, 期限加算月);
        }

        void webclient_Open使用量2CSVCompleted(object sender, Open使用量2CSVCompletedEventArgs e)
        {
            try
            {
                if (e == null || e.Result == null)
                {
                    return;
                }
                在庫リターンデータセットOf薬局使用量データZYtYGqK_P rdataset = e.Result;

                if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
                {
                    ObservableCollection<薬局使用量データ> data = rdataset.検索結果データlist;
                    var list = data.ToList();
                    var 表示順序dic = Core.共通.PageScope.表示順序;

                    // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                    var CopyDic = View.Util.Common.GenericUtil.Copy(表示順序dic);
                    int cnt = CopyDic.Count + 1;

                    list.Sort(
                        delegate(薬局使用量データ x, 薬局使用量データ y)
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

                            // 等しい場合は、さらに使用年月日で比較する
                            if (xValue == yValue)
                            {
                                DateTime xの年月日;
                                DateTime yの年月日;

                                // xがDateTimeにキャストできない場合は、後ろに
                                if (DateTime.TryParse(x.使用年月, out xの年月日) == false)
                                {
                                    return 1;
                                }


                                // yがDateTimeにキャストできない場合は、前に
                                if (DateTime.TryParse(y.使用年月, out yの年月日) == false)
                                {
                                    return -1;
                                }


                                if (xの年月日 > yの年月日)
                                {
                                    return -1;
                                }
                                else if (yの年月日 > xの年月日)
                                {
                                    return 1;
                                }
                            }

                            // 店名が等しく、使用年月も等しい場合は、医薬品名のアイウエオ順
                            if (xValue == yValue)
                            {
                                return x.医薬品名.CompareTo(y.医薬品名);

                            }


                            return xValue - yValue;
                        }
                        );

                    SingletonInstances.使用量DataGridInstance.name使用量DataGrid.ItemsSource = list;
                    Core.共通.SingletonInstances.在庫管理FrameInstance.Set在庫検索結果Result(list.Count, rdataset.検索キーワード);
                    // 原本はおかない。容量が大きいため、キャッシュせず直接ItemSourceに入れる
                    //SingletonInstances.使用量DataGridInstance.原本itemsource = 使用量DataGridItemSource;
                }
                else
                {
                    // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                    //MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                    Core.共通.SingletonInstances.在庫管理FrameInstance.tbl検索結果.Text = rdataset.エラーメッセージ;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace + e.Error.InnerException.Message + e.Error.InnerException.StackTrace);
                return;
            }
            finally
            {
                // カーソルをもとの状態にする
                SingletonInstances.在庫管理FrameInstance.SetCursorDefault();

                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }
        }


        void webclient_Open不動品CSVCompleted(object sender, Open不動品CSVCompletedEventArgs e)
        {
            try
            {
                if (e == null || e.Result == null)
                {
                    return;
                }
                在庫リターンデータセットOf不動品データZYtYGqK_P rdataset = e.Result;
                if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
                {
                    ObservableCollection<不動品データ> 現在庫DataGridItemSource = rdataset.検索結果データlist;

                    不動品DataGrid instance = Core.共通.SingletonInstances.不動品DataGridInstance;


                    instance.name不動品DataGrid.ItemsSource = 現在庫DataGridItemSource;

                }
                else
                {
                    // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                    //MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                    Core.共通.SingletonInstances.在庫管理FrameInstance.tbl検索結果.Text = rdataset.エラーメッセージ;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace + e.Error.InnerException.Message + e.Error.InnerException.StackTrace);
                return;
            }
            finally
            {
                // カーソルをもとの状態にする
                SingletonInstances.在庫管理FrameInstance.SetCursorDefault();

                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }
        }

        void webclient_Open使用量CSVCompleted(object sender, Open使用量CSVCompletedEventArgs e)
        {
            try
            {
                if (e == null || e.Result == null)
                {
                    return;
                }
                在庫リターンデータセットOf薬局使用量データZYtYGqK_P rdataset = e.Result;

                if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
                {
                    ObservableCollection<薬局使用量データ> data = rdataset.検索結果データlist;
                    var list = data.ToList();
                    var 表示順序dic = Core.共通.PageScope.表示順序;

                    // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                    var CopyDic = View.Util.Common.GenericUtil.Copy(表示順序dic);
                    int cnt = CopyDic.Count + 1;

                    list.Sort(
                        delegate(薬局使用量データ x, 薬局使用量データ y)
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

                            // 等しい場合は、さらに使用年月日で比較する
                            if (xValue == yValue)
                            {
                                DateTime xの年月日;
                                DateTime yの年月日;

                                // xがDateTimeにキャストできない場合は、後ろに
                                if (DateTime.TryParse(x.使用年月, out xの年月日) == false)
                                {
                                    return 1;
                                }


                                // yがDateTimeにキャストできない場合は、前に
                                if (DateTime.TryParse(y.使用年月, out yの年月日) == false)
                                {
                                    return -1;
                                }


                                if (xの年月日 > yの年月日)
                                {
                                    return -1;
                                }
                                else if (yの年月日 > xの年月日)
                                {
                                    return 1;
                                }
                            }

                            // 店名が等しく、使用年月も等しい場合は、医薬品名のアイウエオ順
                            if (xValue == yValue)
                            {
                                return x.医薬品名.CompareTo(y.医薬品名);

                            }


                            return xValue - yValue;
                        }
                        );

                    SingletonInstances.使用量DataGridInstance.name使用量DataGrid.ItemsSource = list;
                    Core.共通.SingletonInstances.在庫管理FrameInstance.Set在庫検索結果Result(list.Count, rdataset.検索キーワード);
                    // 原本はおかない。容量が大きいため、キャッシュせず直接ItemSourceに入れる
                    //SingletonInstances.使用量DataGridInstance.原本itemsource = 使用量DataGridItemSource;
                }
                else
                {
                    // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                    //MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                    Core.共通.SingletonInstances.在庫管理FrameInstance.tbl検索結果.Text = rdataset.エラーメッセージ;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace + e.Error.InnerException.Message + e.Error.InnerException.StackTrace);
                return;
            }
            finally
            {
                // カーソルをもとの状態にする
                SingletonInstances.在庫管理FrameInstance.SetCursorDefault();

                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }

        }

        void webclient_OpenCSVCompleted(object sender, OpenCSVCompletedEventArgs e)
        {
            try
            {

                if (e == null || e.Result == null)
                {
                    return;
                }

                ObservableCollection<View.Service.File.Reader.現在庫データ> 現在庫DataGridItemSource = e.Result;

                View.Core.TopPage.Tab.在庫管理.現在庫DataGrid instance = Core.共通.SingletonInstances.現在庫DataGridInstance;
                View.Core.TopPage.Tab.在庫管理.後発品DataGrid instance2 = Core.共通.SingletonInstances.後発品DataGridInstance;

                // 現在庫データと後発品データは同じItemSource
                instance.原本itemsource = 現在庫DataGridItemSource;
                instance2.原本itemsource = 現在庫DataGridItemSource;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace + e.Error.InnerException.Message + e.Error.InnerException.StackTrace);
                return;
            }
            finally
            {
                // カーソルをもとの状態にする
                SingletonInstances.在庫管理FrameInstance.SetCursorDefault();

                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }


        }
    }
}
