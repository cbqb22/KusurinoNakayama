using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using OASystem.Model.Entity;
using OASystem.ViewModel.Common.Printer;

namespace OASystem.View.Windows
{
    /// <summary>
    /// OrderCenter.xaml の相互作用ロジック
    /// </summary>
    public partial class OrderCenter : Window
    {


        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        private bool _IsSaved;

        private List<ExpDeadListEntity> _ExpDeadList;
        public List<ExpDeadListEntity> ExpDeadList
        {
            get { return _ExpDeadList; }
            set { _ExpDeadList = value; }
        }

        public OrderCenter()
        {
            InitializeComponent();
            this.Closed += new EventHandler(OrderCenter_Closed);
            this.tblStoreNameOrderTitle.Text = string.Format("{0} 発注書", OASystem.Model.DI.自店舗名);
        }



        void OrderCenter_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            if (lv == null)
            {
                return;
            }

            var item = lv.SelectedItem as OrderScheduledListEntity;

            // 全て表示用の処理
            int itemIndex = 0;
            bool find = false;
            ExpDeadListEntity findItem = null;
            foreach (ExpDeadListEntity i in lvExpDeadAllDisp.ItemsSource)
            {
                itemIndex++;
                if (i.レセプト電算コード == item.レセプト電算コード)
                {
                    findItem = i;
                    find = true;
                    break;
                }
            }

            if (find && findItem != null)
            {
                lvExpDeadAllDisp.SelectedIndex = itemIndex - 1;
                lvExpDeadAllDisp.ScrollIntoView(findItem);
            }



            // 選択表示用の処理
            var lvi = lv.SelectedItem as ListViewItem;

            var select = (
                         from x in ExpDeadList
                         where
                            x.レセプト電算コード == item.レセプト電算コード
                         select x
                         ).ToList();
            lvExpDeadSelectMode.ItemsSource = select;
            lvExpDeadSelectMode.SelectedIndex = 0;

            lvExpDeadSelectMode.UpdateLayout();

            // Handledにしないと、TabControlのSelection_Changeイベントが走ってしまう。
            e.Handled = true;

        }


        private Dictionary<string, List<ExpDeadListEntity>> GetOrderExpDeadListEntity()
        {


            var onlyOrder = (
                from x in this.lvExpDeadAllDisp.ItemsSource as List<ExpDeadListEntity>
                where
                    x.注文数 > 0
                select x
                ).OrderBy(x => x.店名).ThenBy(x => x.医薬品名).ToList();

            Dictionary<string, List<ExpDeadListEntity>> dic = new Dictionary<string, List<ExpDeadListEntity>>();

            foreach (var o in onlyOrder)
            {
                if (dic.ContainsKey(o.店名))
                {
                    dic[o.店名].Add(o);
                }
                else
                {
                    var list = new List<ExpDeadListEntity>();
                    list.Add(o);
                    dic.Add(o.店名, list);
                }
            }

            return dic;

        }

        private void btnComplate_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            var dic = GetOrderExpDeadListEntity();

            if (dic.Count != 0 && _IsSaved == false && MessageBox.Show("閉じる前に発注データを履歴に保存しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                SaveToOrderLog(dic);
            }

            // DAT内容の削除
            var r1 =
                //(from x in lvOrderOtherStoreMatch.ItemsSource as List<OrderScheduledListEntity>
                    (from x in lvOrderAllDisp.ItemsSource as List<OrderScheduledListEntity>
                     where
                         x.SEND01DATから削除するか == true
                     select x).OrderBy(x => x.レセ発注伝票No).ThenBy(x => x.医薬品名).ToList();

            // 削除しないチェックかつVANコードが"1"から始まるもの
            var r2 =
                //(from x in lvOrderOtherStoreMatch.ItemsSource as List<OrderScheduledListEntity>
                    (from x in lvOrderAllDisp.ItemsSource as List<OrderScheduledListEntity>
                     where
                         x.SEND01DATから削除するか == false
                         &&
                         x.卸VANコード.StartsWith("1")
                     select x).OrderBy(x => x.レセ発注伝票No).ThenBy(x => x.医薬品名).ToList();

            // 削除対象のものをピックアップ
            var r12 = r1.Concat(r2).OrderBy(x => x.レセ発注伝票No).ThenBy(x => x.医薬品名).ToList();

            //r1がある場合
            if (r1.Count != 0)
            {
                string msg = "発注予定リストの削除にチェックが入った伝票番号の注文を\r\n卸の注文データ(SEND01.DAT)から削除します。\r\n\r\n宜しいですか？";
                //r2もある場合はメッセージを少し変えてあげる
                if (r2.Count != 0)
                {
                    msg = "発注予定リストの削除にチェックが入った伝票番号の注文を\r\n卸の注文データ(SEND01.DAT)から削除します。\r\n\r\nまた他店にオンライン発注となっている注文(※)を\r\n卸の注文データ(SEND01.DAT)から削除します。\r\n※卸VANコードが[1]から始まる注文\r\n\r\n宜しいですか？";
                }

                if (MessageBox.Show(msg, "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (MessageBox.Show("削除操作を完了し、発注書作成画面を終了します。", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        string message = "";
                        if (DatDelete(r12))
                        {
                            message = "全ての卸の注文データ(SEND01.DAT)を削除しました。\r\n次の医薬品を未納品確定の訂正で必ず行って下さい。\r\n\r\nこの一覧表を印刷しますか？\r\n";
                        }
                        else
                        {
                            message = "次の医薬品を卸の注文データ(SEND01.DAT)から削除しました。\r\n未納品確定の訂正を必ず行って下さい。\r\n\r\nこの一覧表を印刷しますか？\r\n";
                        }

                        int writecounter = 0;
                        foreach (var data in r12)
                        {
                            writecounter++;
                            if (11 <= writecounter)
                            {
                                message += "\r\n\r\n※※未表示分があります。一覧表を印刷してください。※※";
                                break;
                            }


                            string 伝票No = string.Format("伝票No:{0}", data.レセ発注伝票No).PadRight(11, '　');
                            string 帳合先 = data.帳合先名称.PadRight(8, '　');
                            message += string.Format("\r\n{0}{1} {2}", 伝票No, 帳合先, data.医薬品名);
                        }

                        if (MessageBox.Show(message, "確認", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            要削除一覧表印刷(r12);
                        }
                    }
                    else
                    {
                        MessageBox.Show("削除操作をキャンセルしました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("卸の注文データ(SEND01.DAT)を削除せず、このまま発注書作成画面を閉じます。\r\n\r\n宜しいですか？", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        // 何もしない。このままCloseへ
                    }
                    else
                    {
                        return;
                    }

                }
            }

            // r1は0だけど、r2がある場合
            else if (r2.Count != 0)
            {
                if (MessageBox.Show("他店にオンライン発注となっている注文(※)を\r\n卸の注文データ(SEND01.DAT)から削除します。\r\n※卸VANコードが[1]から始まる注文\r\n\r\n宜しいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (MessageBox.Show("削除操作を完了し、発注書作成画面を終了します。", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        string message = "";
                        if (DatDelete(r12))
                        {
                            message = "全ての卸の注文データ(SEND01.DAT)を削除しました。\r\n次の医薬品を未納品確定の訂正で必ず行って下さい。\r\n\r\nこの一覧表を印刷しますか？\r\n";
                        }
                        else
                        {
                            message = "次の医薬品を卸の注文データ(SEND01.DAT)から削除しました。\r\n未納品確定の訂正を必ず行って下さい。\r\n\r\nこの一覧表を印刷しますか？\r\n";
                        }

                        int writecounter = 0;
                        foreach (var data in r12)
                        {
                            writecounter++;
                            if (11 <= writecounter)
                            {
                                message += "\r\n\r\n※※未表示分があります。一覧表を印刷してください。※※";
                                break;
                            }


                            string 伝票No = string.Format("伝票No:{0}", data.レセ発注伝票No).PadRight(11, '　');
                            string 帳合先 = data.帳合先名称.PadRight(8, '　');
                            message += string.Format("\r\n{0}{1} {2}", 伝票No, 帳合先, data.医薬品名);
                        }

                        if (MessageBox.Show(message, "確認", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            要削除一覧表印刷(r12);
                        }
                    }
                    else
                    {
                        MessageBox.Show("削除操作をキャンセルしました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("卸の注文データ(SEND01.DAT)を削除せず、このまま発注書作成画面を閉じます。\r\n\r\n宜しいですか？", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        // 何もしない。このままCloseへ
                    }
                    else
                    {
                        return;
                    }

                }

            }
            // 発注数があるのに、削除にチェックがない場合
            else if (dic.Count != 0)
            {
                if (MessageBox.Show("発注予定リストの削除にチェックがありませんでした。\r削除操作を行わず発注書作成画面を終了します。\r\n\r\n宜しいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // 何もしない。このままCloseへ
                }
                else
                {
                    return;
                }
            }
            // それ以外の場合(発注もなく、削除もない)
            else
            {
                if (MessageBox.Show("発注書作成画面を終了します。\r\n\r\n宜しいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // 何もしない。このままCloseへ
                }
                else
                {
                    return;
                }
            }


            this.Close();

        }


        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }


            bdButtonListBox.Visibility = System.Windows.Visibility.Visible;

        }


        private void フリーフォーム印刷()
        {
            PrintOrderSheetFree pos = new PrintOrderSheetFree(OASystem.Model.DI.自店舗名, 10);
            pos.Print();
        }

        private void 要削除一覧表印刷(List<OrderScheduledListEntity> orderScheduleList)
        {
            Print未納品一覧要確定表 pos = new Print未納品一覧要確定表(orderScheduleList);
            pos.Print();
        }

        private bool 発注書印刷()
        {
            var dic = GetOrderExpDeadListEntity();

            if (dic.Count == 0)
            {
                MessageBox.Show("注文数が入力されておりません。\r\n期限切迫・デッド品のリストより注文数を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            //ここでは聞かず、閉じるときに聞く。
            //if (MessageBox.Show("発注データを履歴に保存しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            //{
            //    SaveToOrderLog(dic);
            //}

            PrintOrderSheet pos = new PrintOrderSheet(OASystem.Model.DI.自店舗名, dic);
            pos.Print();

            return true;


        }

        void lb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bdButtonListBox.Visibility = System.Windows.Visibility.Hidden;

            ListBox lb = sender as ListBox;
            if (lb == null)
            {
                return;
            }

            ListBoxItem lbi = lb.SelectedItem as ListBoxItem;
            if (lbi == null)
            {
                return;
            }

            if (lbi.Content.ToString() == "発注書印刷")
            {
                if (!発注書印刷())
                {
                    return;
                }

                //// DAT内容の削除
                //var r = 
                //        //(from x in lvOrderOtherStoreMatch.ItemsSource as List<OrderScheduledListEntity>
                //        (from x in lvOrderAllDisp.ItemsSource as List<OrderScheduledListEntity>
                //         where
                //             x.SEND01DATから削除するか == true
                //         select x).OrderBy(x => x.レセ発注伝票No).ThenBy(x => x.医薬品名).ToList();
                //if (r.Count != 0)
                //{
                //    if (MessageBox.Show("発注予定リストの削除にチェックが入った伝票番号の注文を\r\n卸の注文データ(SEND01.DAT)から削除します。\r\n\r\n宜しいですか？", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                //    {
                //        string message = "";
                //        if (DatDelete(r))
                //        {
                //            message = "全ての卸の注文データ(SEND01.DAT)を削除しました。\r\n次の医薬品を未納品確定の訂正で必ず行って下さい。\r\n\r\nこの一覧表を印刷しますか？\r\n";
                //        }
                //        else
                //        {
                //            message = "次の医薬品を卸の注文データ(SEND01.DAT)から削除しました。\r\n未納品確定の訂正を必ず行って下さい。\r\n\r\nこの一覧表を印刷しますか？\r\n";
                //        }

                //        int writecounter = 0;
                //        foreach (var data in r)
                //        {
                //            writecounter++;
                //            if (11 <= writecounter)
                //            {
                //                message += "\r\n\r\n※※未表示分があります。一覧表を印刷してください。※※";
                //                break;
                //            }


                //            string 伝票No = string.Format("伝票No:{0}", data.レセ発注伝票No).PadRight(11, '　');
                //            string 帳合先 = data.帳合先名称.PadRight(8, '　');
                //            message += string.Format("\r\n{0}{1} {2}", 伝票No, 帳合先, data.医薬品名);
                //        }

                //        if (MessageBox.Show(message, "確認", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                //        {
                //            要削除一覧表印刷(r);
                //        }
                //    }
                //}

                //this.Close();

            }
            else if (lbi.Content.ToString() == "フリーフォーム")
            {
                フリーフォーム印刷();
            }

            lb.SelectedIndex = -1;
        }

        /// <summary>
        /// SEND01.DATから、チェックの入ったデータを削除する。
        /// </summary>
        /// <param name="oslist"></param>
        /// <returns>全て削除された場合はtrue</returns>
        private bool DatDelete(List<OrderScheduledListEntity> oslist)
        {
            List<SEND01DATEntity> deletelist = new List<SEND01DATEntity>();
            foreach (var osle in oslist)
            {
                if (osle.SEND01DATから削除するか)
                {
                    SEND01DATEntity ent = new SEND01DATEntity();
                    ent.JANコード = osle.JANコード;
                    ent.レセ発注伝票No = osle.レセ発注伝票No;
                    ent.数量 = osle.数量;
                    ent.注文番号 = osle.注文番号;
                    //ent.卸VANコード = ;
                    deletelist.Add(ent);
                }
            }

            return OASystem.ViewModel.File.SEND01DATAnalyzer.DeleteMedicineFromList(deletelist);
        }

        private void SaveToOrderLog(Dictionary<string, List<ExpDeadListEntity>> orderdic)
        {
            var savepath = System.IO.Path.Combine(OASystem.Properties.Settings.Default.OrderLogsFolderPath, DateTime.Now.ToString("yyyyMMddHHmmss.fff") + ".csv");
            using (StreamWriter sw = new StreamWriter(savepath, false, Encoding.GetEncoding(932)))
            {
                sw.WriteLine(string.Format("自店舗名,発注先店舗名,レセプト電算コード,医薬品名,包装形態,包装単位,包装単位数,包装総量,剤形区分,注文数,期限切迫,デッド,薬価,優先移動"));

                foreach (var kvp in orderdic)
                {
                    foreach (var list in kvp.Value)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                            , OASystem.Model.DI.自店舗名
                            , list.店名
                            , list.レセプト電算コード
                            , list.医薬品名と名称２連結
                            , list.包装形態
                            , list.包装単位
                            , list.包装単位数
                            , list.包装総量
                            , (int)list.剤形区分
                            , list.注文数
                            , list.Is期限切迫 ? 1 : 0
                            , list.Isデッド品 ? 1 : 0
                            , list.薬価
                            , list.Is優先移動 ? 1 : 0
                            ));
                    }
                }
            }

            _IsSaved = true;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            var dic = GetOrderExpDeadListEntity();

            if (dic.Count == 0)
            {
                MessageBox.Show("注文数が入力されておりません。\r\n期限切迫・デッド品のリストより注文数を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            SaveToOrderLog(dic);
            MessageBox.Show("発注履歴に保存しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            var fe = VisualTreeHelper.GetParent(tb) as FrameworkElement;

            ListViewItem lvi = null;
            ListView lv = null;
            while (fe != null)
            {
                if (fe is ListViewItem)
                {
                    lvi = fe as ListViewItem;
                }
                else if (fe is ListView)
                {
                    lv = fe as ListView;
                    break;
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;

            }

            if (lv != null && lvi != null)
            {
                lv.SelectedItem = lvi.DataContext;
            }


        }


        private ListSortDirection _lastDiresction = ListSortDirection.Ascending;
        private GridViewColumnHeader _lastHeaderclicked = null;
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader clickheader = e.OriginalSource as GridViewColumnHeader;
            if (clickheader == null)
            {
                return;
            }

            var fe = VisualTreeHelper.GetParent(clickheader) as FrameworkElement;
            ListView parentlv = null;
            while (fe != null)
            {
                if (fe is ListView)
                {
                    parentlv = fe as ListView;
                    break;
                }
                else
                {
                    fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
                }
            }

            if (parentlv == null)
            {
                return;
            }
            if (parentlv.ItemsSource == null)
            {
                return;
            }

            if (clickheader.Role != GridViewColumnHeaderRole.Padding)
            {
                ListSortDirection direction = ListSortDirection.Ascending;
                if (clickheader == _lastHeaderclicked && _lastDiresction == ListSortDirection.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }

                // ▲▼の表示
                if (direction == ListSortDirection.Ascending)
                {
                    clickheader.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                }
                else
                {
                    clickheader.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                }

                if (_lastHeaderclicked != null && _lastHeaderclicked != clickheader)
                {
                    _lastHeaderclicked.Column.HeaderTemplate = null;
                }

                //string header = (clickheader.Column.DisplayMemberBinding as Binding).Path.Path;
                var cc = clickheader.Column.Header as ContentControl;

                //string header = clickheader.Column.Header;
                Sort(parentlv, cc.Content.ToString(), direction);
                _lastHeaderclicked = clickheader;
                _lastDiresction = direction;
            }



        }


        private ListSortDirection _lastDiresctionOrderAllDisp = ListSortDirection.Ascending;
        private GridViewColumnHeader _lastHeaderclickedOrderAllDisp = null;
        private void GridViewColumnHeaderOrderAllDisp_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader clickheader = e.OriginalSource as GridViewColumnHeader;
            if (clickheader == null)
            {
                return;
            }

            var fe = VisualTreeHelper.GetParent(clickheader) as FrameworkElement;
            ListView parentlv = null;
            while (fe != null)
            {
                if (fe is ListView)
                {
                    parentlv = fe as ListView;
                    break;
                }
                else
                {
                    fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
                }
            }

            if (parentlv == null)
            {
                return;
            }
            if (parentlv.ItemsSource == null)
            {
                return;
            }

            if (clickheader.Role != GridViewColumnHeaderRole.Padding)
            {
                ListSortDirection direction = ListSortDirection.Ascending;
                if (clickheader == _lastHeaderclickedOrderAllDisp && _lastDiresctionOrderAllDisp == ListSortDirection.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }

                // ▲▼の表示
                if (direction == ListSortDirection.Ascending)
                {
                    clickheader.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                }
                else
                {
                    clickheader.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                }

                if (_lastHeaderclickedOrderAllDisp != null && _lastHeaderclickedOrderAllDisp != clickheader)
                {
                    _lastHeaderclickedOrderAllDisp.Column.HeaderTemplate = null;
                }

                //string header = (clickheader.Column.DisplayMemberBinding as Binding).Path.Path;
                var cc = clickheader.Column.Header as ContentControl;

                //string header = clickheader.Column.Header;
                SortForOrderScheduledList(parentlv, cc.Content.ToString(), direction);
                _lastHeaderclickedOrderAllDisp = clickheader;
                _lastDiresctionOrderAllDisp = direction;
            }



        }



        private ListSortDirection _lastDiresctionDeadAndExpWithBalancingAccounts = ListSortDirection.Ascending;
        private GridViewColumnHeader _lastHeaderclickedDeadAndExpWithBalancingAccounts = null;
        private void GridViewColumnHeaderDeadAndExpWithBalancingAccounts_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader clickheader = e.OriginalSource as GridViewColumnHeader;
            if (clickheader == null)
            {
                return;
            }

            var fe = VisualTreeHelper.GetParent(clickheader) as FrameworkElement;
            ListView parentlv = null;
            while (fe != null)
            {
                if (fe is ListView)
                {
                    parentlv = fe as ListView;
                    break;
                }
                else
                {
                    fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
                }
            }

            if (parentlv == null)
            {
                return;
            }
            if (parentlv.ItemsSource == null)
            {
                return;
            }

            if (clickheader.Role != GridViewColumnHeaderRole.Padding)
            {
                ListSortDirection direction = ListSortDirection.Ascending;
                if (clickheader == _lastHeaderclickedDeadAndExpWithBalancingAccounts && _lastDiresctionDeadAndExpWithBalancingAccounts == ListSortDirection.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }

                // ▲▼の表示
                if (direction == ListSortDirection.Ascending)
                {
                    clickheader.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                }
                else
                {
                    clickheader.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                }

                if (_lastHeaderclickedDeadAndExpWithBalancingAccounts != null && _lastHeaderclickedDeadAndExpWithBalancingAccounts != clickheader)
                {
                    _lastHeaderclickedDeadAndExpWithBalancingAccounts.Column.HeaderTemplate = null;
                }

                //string header = (clickheader.Column.DisplayMemberBinding as Binding).Path.Path;
                var cc = clickheader.Column.Header as ContentControl;

                //string header = clickheader.Column.Header;
                SortForOrderScheduledList(parentlv, cc.Content.ToString(), direction);
                _lastHeaderclickedDeadAndExpWithBalancingAccounts = clickheader;
                _lastDiresctionDeadAndExpWithBalancingAccounts = direction;
            }



        }

        private void SortForOrderScheduledList(ListView lv, string name, ListSortDirection direction)
        {
            var itemsource = lv.ItemsSource as List<OrderScheduledListEntity>;
            var selectedsnap = lv.SelectedItem;

            if (name == "医薬品名")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.医薬品名).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.医薬品名).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "伝票No")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.レセ発注伝票No).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.レセ発注伝票No).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "帳合先")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.帳合先名称).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.帳合先名称).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }


        }



        private void Sort(ListView lv, string name, ListSortDirection direction)
        {
            var itemsource = lv.ItemsSource as List<ExpDeadListEntity>;
            var selectedsnap = lv.SelectedItem;

            if (name == "期迫")
            {
                //期限切迫・デッド・優先は昇順を降順にしておくと見やすい
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderByDescending(x => x.Is期限切迫).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderBy(x => x.Is期限切迫).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "デッド")
            {
                //期限切迫・デッド・優先は昇順を降順にしておくと見やすい
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderByDescending(x => x.Isデッド品).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderBy(x => x.Isデッド品).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "優先")
            {
                //期限切迫・デッド・優先は昇順を降順にしておくと見やすい
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderByDescending(x => x.Is優先移動).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderBy(x => x.Is優先移動).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "医薬品名")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.医薬品名).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.医薬品名).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "店舗名")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.店名).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.店名).ToList();
                }

                //lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }

        }


        private void bdButtonListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            // すでに表示されていたら引き続き印刷ボタンから離れても表示させる
            if (bdButtonListBox.Visibility == System.Windows.Visibility.Visible)
            {
                bdButtonListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void bdButtonListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            bdButtonListBox.Visibility = System.Windows.Visibility.Hidden;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            var fe = VisualTreeHelper.GetParent(tb) as FrameworkElement;

            ListViewItem lvi = null;
            ListView lv = null;
            while (fe != null)
            {
                if (fe is ListViewItem)
                {
                    lvi = fe as ListViewItem;
                }
                else if (fe is ListView)
                {
                    lv = fe as ListView;
                    break;
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;

            }

            if (lvi == null || lv == null)
            {
                return;
            }

            ExpDeadListEntity edl = lvi.DataContext as ExpDeadListEntity;
            var lvitems = lv.ItemsSource as List<ExpDeadListEntity>;

            //他に同じ医薬品で注文あるか
            var r = from x in lvitems
                    where
                        x.Is注文あり == true &&
                        x.レセプト電算コード == edl.レセプト電算コード &&
                        x.店名 != edl.店名 //違う店名ならば別となるので
                    select x;
            //1つ以上他店注文ありならば、OrderListの背景色はそのまま・DAT削除もいじらない
            if (1 <= r.Count())
            {
                return;
            }

            double result;
            double.TryParse(tb.Text, out result);


            foreach (OrderScheduledListEntity i in lvOrderOnlyCheckBalancingFalse.ItemsSource)
            {
                if (i.レセプト電算コード == edl.レセプト電算コード)
                {
                    if (string.IsNullOrEmpty(tb.Text) || result == 0)
                    {
                        i.Is注文あり = false;
                        // i.SEND01DATから削除するか = false;  //とりあえずユーザー手動で削除するか選択させる
                    }
                    else
                    {
                        i.Is注文あり = true;
                        // i.SEND01DATから削除するか = true; //とりあえずユーザー手動で削除するか選択させる
                    }
                    break;
                }
            }

            foreach (OrderScheduledListEntity i in lvOrderAllDisp.ItemsSource)
            {
                if (i.レセプト電算コード == edl.レセプト電算コード)
                {
                    if (string.IsNullOrEmpty(tb.Text) || result == 0)
                    {
                        i.Is注文あり = false;
                    }
                    else
                    {
                        i.Is注文あり = true;
                    }
                    break;
                }
            }
            foreach (OrderScheduledListEntity i in lvOrderDeadAndExpWithBalancingAccounts.ItemsSource)
            {
                if (i.レセプト電算コード == edl.レセプト電算コード)
                {
                    if (string.IsNullOrEmpty(tb.Text) || result == 0)
                    {
                        i.Is注文あり = false;
                    }
                    else
                    {
                        i.Is注文あり = true;
                    }
                    break;
                }
            }
        }

        #region MoveFocus
        private bool _FromMessageBoxEvent;


        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // これがないとMessageBoxでOKをEnter押したら、またここが呼ばれてしまう。
            if (_FromMessageBoxEvent)
            {
                _FromMessageBoxEvent = false;
                return;
            }

            var tb = sender as TextBox;
            if (e == null)
            {
                return;
            }

            UIElement uie = e.OriginalSource as UIElement;

            // 'Ctrl + Tab' or 'Shift + Enter' (Reverse Tab)
            if ((e.Key == Key.Up) && uie != null)
            //if ((e.Key == Key.Return) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && uie != null)
            {
                if (!string.IsNullOrEmpty(tb.Text))
                {
                    double result;
                    if (double.TryParse(tb.Text, out result) == false)
                    {
                        MessageBox.Show("半角数値を入力してください。", "入力チェック", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        _FromMessageBoxEvent = true;
                        return;
                    }
                }

                MoveTextBoxPreviousFocus(tb);

                e.Handled = true;
            }
            else if (((e.Key == Key.Return) || (e.Key == Key.Down)) && uie != null)
            {
                if (!string.IsNullOrEmpty(tb.Text))
                {
                    double result;
                    if (double.TryParse(tb.Text, out result) == false)
                    {
                        MessageBox.Show("半角数値を入力してください。", "入力チェック", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        _FromMessageBoxEvent = true;
                        return;
                    }
                }

                MoveTextBoxNextFocus(tb, e.Key == Key.Return ? true : false);

                // Normal 'Enter' or 'Tab' key click
                //MoveFocusNext(uie, (UIElement)sender);

                e.Handled = true;
            }
        }


        //private void Sort(string sortBy, ListSortDirection direction)
        //{
        //    SortDescription sd = new SortDescription(sortBy, direction);

        //    _viewSource.SortDescriptions.Clear();
        //    _viewSource.SortDescriptions.Add(sd);
        //    _viewSource.Refresh();

        //}

        private void MoveTextBoxPreviousFocus(TextBox tb)
        {
            var fe = VisualTreeHelper.GetParent(tb);
            VirtualizingStackPanel vsp = null;
            ListViewItem currentlvi = null;

            while (fe != null)
            {
                if (fe is ListViewItem)
                {
                    currentlvi = fe as ListViewItem;
                }
                else if (fe is VirtualizingStackPanel)
                {
                    vsp = fe as VirtualizingStackPanel;
                    break;
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
            }

            if (currentlvi == null || vsp == null)
            {
                return;
            }

            if (vsp.Children.Count == 0)
            {
                return;
            }

            int indexcounter = -1;
            bool find = false;
            foreach (object child in vsp.Children)
            {
                indexcounter++;
                ListViewItem childlvi = child as ListViewItem;
                if (childlvi == null)
                {
                    return;
                }

                if (childlvi == currentlvi)
                {
                    find = true;
                    break;
                }
            }

            if (!find)
            {
                return;
            }

            // 最初のListviewitemならば
            if (indexcounter == 0)
            {
                //何もしない
                return;
            }
            else
            {
                var nextlvi = vsp.Children[indexcounter - 1] as ListViewItem;
                if (nextlvi == null)
                {
                    return;
                }


                var nexttextbox = FindVisualChild<TextBox>(nextlvi);
                nexttextbox.Focus();

            }


        }


        private void MoveTextBoxNextFocus(TextBox tb, bool isEnter)
        {
            var fe = VisualTreeHelper.GetParent(tb);
            VirtualizingStackPanel vsp = null;
            ListViewItem currentlvi = null;

            while (fe != null)
            {
                if (fe is ListViewItem)
                {
                    currentlvi = fe as ListViewItem;
                }
                else if (fe is VirtualizingStackPanel)
                {
                    vsp = fe as VirtualizingStackPanel;
                    break;
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
            }

            if (currentlvi == null || vsp == null)
            {
                return;
            }

            if (vsp.Children.Count == 0)
            {
                return;
            }

            int indexcounter = -1;
            bool find = false;
            foreach (object child in vsp.Children)
            {
                indexcounter++;
                ListViewItem childlvi = child as ListViewItem;
                if (childlvi == null)
                {
                    return;
                }

                if (childlvi == currentlvi)
                {
                    find = true;
                    break;
                }
            }

            if (!find)
            {
                return;
            }

            // 最後のListviewitemならば
            if (vsp.Children.Count == indexcounter + 1)
            {
                //// ListViewItemが１つしかないときは印刷ボタンへ移動させて、背景色を変える。
                //if (vsp.Children.Count == 1)
                //{
                //}

                // Enterのみ印刷ボタンへフォーカス
                if (isEnter)
                {
                    // 印刷ボタンへフォーカスを試みる
                    btnPrint.Focus();
                }


            }
            else
            {
                var nextlvi = vsp.Children[indexcounter + 1] as ListViewItem;
                if (nextlvi == null)
                {
                    return;
                }


                var nexttextbox = FindVisualChild<TextBox>(nextlvi);
                nexttextbox.Focus();

            }


        }

        #endregion MoveFocus


        private static TChild FindVisualChild<TChild>(DependencyObject parent)
where TChild : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is TChild)
                {
                    return (TChild)child;
                }
                else
                {
                    TChild subItem = FindVisualChild<TChild>(child);
                    if (subItem != null)
                    {
                        return subItem;
                    }
                }
            }
            return null;
        }

        private void tcOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tc = sender as TabControl;

            // 全て表示ならば
            if (tc.SelectedIndex == 0)
            {
                if (lvOrderAllDisp.ItemsSource != null)
                {
                    var isource = lvOrderAllDisp.ItemsSource as List<OrderScheduledListEntity>;
                    isource = isource.OrderBy(x => x.レセ発注伝票No).ThenBy(x => x.医薬品名).ToList();
                    lvOrderAllDisp.ItemsSource = isource;

                    if (_lastHeaderclickedOrderAllDisp != null)
                    {
                        _lastHeaderclickedOrderAllDisp.Column.HeaderTemplate = null; // ▲▼を元に戻す
                    }
                }
            }
        }




    }
}
