using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Globalization;
using OASystem.Properties;
using OASystem.Model.Entity;
using OASystem.Model.Enum;
using OASystem.ViewModel.Common.Printer;


namespace OASystem.View.Windows
{
    /// <summary>
    /// OrderLogs.xaml の相互作用ロジック
    /// </summary>
    public partial class OrderLogs : Window
    {
        /// <summary>
        /// 管理のデータ
        /// </summary>
        private Dictionary<DateTime, List<OrderLogListEntity>> _OrderlogList;
        public Dictionary<DateTime, List<OrderLogListEntity>> OrderlogList
        {
            get { return _OrderlogList; }
            set { _OrderlogList = value; }
        }

        private bool _IsSaved;


        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        public OrderLogs()
        {
            InitializeComponent();
            this.Closed += new EventHandler(OrderLogs_Closed);
            this.tblStoreNameOrderTitle.Text = string.Format("{0} 発注書", OASystem.Model.DI.自店舗名);
            Init();
        }

        private void Init()
        {
            var paths = Directory.GetFiles(OASystem.Properties.Settings.Default.OrderLogsFolderPath).OrderByDescending(x => x).ToList();

            Dictionary<DateTime, List<OrderLogListEntity>> sl = new Dictionary<DateTime, List<OrderLogListEntity>>();

            foreach (var path in paths)
            {

                var split = path.Split('\\');

                split = split.OrderByDescending(x => x).ToArray();

                DateTime dresult;
                string format = "yyyyMMddHHmmss.fff";
                string str = split[split.Count() - 1].Replace(".csv", "");
                if (DateTime.TryParseExact(str, format, null, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out dresult) == false)
                {
                    continue;
                }


                List<OrderLogListEntity> list = new List<OrderLogListEntity>();

                int counter = 0;
                string line = "";
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(932)))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;
                        if (counter == 1)
                        {
                            continue;
                        }

                        //自店舗名	発注先店舗名	レセプト電算コード	医薬品名	包装形態	包装単位	包装単位数	包装総量	剤形区分	注文数	期限切迫	デッド     薬価      優先移動
                        //薬価は後から後ろへ追加 2014/08/18
                        var sepa = line.Split(',');
                        if (sepa.Count() < 13)  //途中から追加があるので、13を固定
                        {
                            continue;
                        }


                        OrderLogListEntity ent = new OrderLogListEntity();

                        int result区分;
                        if (int.TryParse(sepa[8], out result区分) == false)
                        {
                            result区分 = 0;
                        }


                        double result;
                        if (double.TryParse(sepa[9], out result) == false)
                        {
                            continue;
                        }
                        ent.注文数 = result;
                        ent.Is期限切迫 = sepa[10].ToString() == "1" ? true : false;
                        ent.Isデッド品 = sepa[11].ToString() == "1" ? true : false;

                        double result薬価;
                        if (13 <= sepa.Count())
                        {
                            if (double.TryParse(sepa[12], out result薬価) == false)
                            {
                                result薬価 = 0;
                            }
                        }
                        else
                        {
                            result薬価 = 0;
                        }
                        if (14 <= sepa.Count())
                        {
                            ent.Is優先移動 = sepa[13].ToString() == "1" ? true : false;
                        }



                        ent.店名 = sepa[1];
                        ent.レセプト電算コード = sepa[2];
                        ent.医薬品名 = sepa[3];
                        ent.包装形態 = sepa[4];
                        ent.包装単位 = sepa[5];
                        ent.包装単位数 = sepa[6];
                        ent.包装総量 = sepa[7];
                        ent.剤形区分 = (剤形区分Enum)result区分;
                        ent.薬価 = result薬価;


                        list.Add(ent);


                    }
                }

                sl.Add(dresult, list);


            }

            this.OrderlogList = sl;
            this.lvOrderLogs.ItemsSource = this.OrderlogList;

        }


        void OrderLogs_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }

        private void lvOrderLogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            if (lv == null)
            {
                return;
            }

            // 削除された場合の対応
            if (lvOrderLogs.SelectedValue == null)
            {
                lvOrderLogList.ItemsSource = null;

                return;
            }


            var dt = (KeyValuePair<DateTime, List<OrderLogListEntity>>)lvOrderLogs.SelectedValue;

            lvOrderLogList.ItemsSource = dt.Value;


        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            this.Close();
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

        private Dictionary<string, List<ExpDeadListEntity>> GetOrderLogListEntity()
        {

            var onlyOrder = (
                from x in this.lvOrderLogList.ItemsSource as List<OrderLogListEntity>
                where
                    x.注文数 > 0
                select x
                ).OrderBy(x => x.店名).ThenBy(x => x.医薬品名).ToList();

            Dictionary<string, List<ExpDeadListEntity>> dic = new Dictionary<string, List<ExpDeadListEntity>>();

            foreach (var o in onlyOrder)
            {
                if (dic.ContainsKey(o.店名))
                {
                    dic[o.店名].Add(o.EDLEntToOLLList変換(o));
                }
                else
                {
                    var list = new List<ExpDeadListEntity>();
                    list.Add(o.EDLEntToOLLList変換(o));
                    dic.Add(o.店名, list);
                }
            }

            return dic;

        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //var dic = GetOrderLogListEntity();
            if (!SaveToOrderLog())
            {
                return;
            }

            MessageBox.Show("発注履歴を上書き保存しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            if (lvOrderLogList.ItemsSource == null)
            {
                MessageBox.Show("印刷する発注履歴を左の日付リストより選択してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;

            }

            var dic = GetOrderLogListEntity();
            if (dic.Count == 0)
            {
                MessageBox.Show("注文数が入力されておりません。\r\n期限切迫・デッド品のリストより注文数を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (MessageBox.Show("発注履歴を上書き保存しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                SaveToOrderLog();
            }

            PrintOrderSheet pos = new PrintOrderSheet(OASystem.Model.DI.自店舗名, dic);
            pos.Print();

            this.Close();

        }


        private ListSortDirection _lastDiresction = ListSortDirection.Ascending;
        private GridViewColumnHeader _lastHeaderclicked = null;
        //private ICollectionView _viewSource;

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

                var cc = clickheader.Column.Header as ContentControl;

                Sort(parentlv, cc.Content.ToString(), direction);
                _lastHeaderclicked = clickheader;
                _lastDiresction = direction;
            }
        }



        private void Sort(ListView lv, string name, ListSortDirection direction)
        {
            var itemsource = lv.ItemsSource as List<OrderLogListEntity>;
            var selectedsnap = lv.SelectedItem;

            if (name == "期迫")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.Is期限切迫).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.Is期限切迫).ToList();
                }

                lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "デッド")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.Isデッド品).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.Isデッド品).ToList();
                }

                lv.ItemsSource = null;
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

                lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }
            else if (name == "発注先店舗名")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    itemsource = itemsource.OrderBy(x => x.店名).ToList();
                }
                else
                {
                    itemsource = itemsource.OrderByDescending(x => x.店名).ToList();
                }

                lv.ItemsSource = null;
                lv.ItemsSource = itemsource;
                lv.SelectedItem = selectedsnap;
            }

        }

        private bool SaveToOrderLog()
        {
            try
            {
                var existsLogFilepaths = System.IO.Directory.GetFiles(OASystem.Properties.Settings.Default.OrderLogsFolderPath);
                var modifyFilePath = new List<string>();

                foreach (KeyValuePair<DateTime, List<OrderLogListEntity>> logdata in lvOrderLogs.ItemsSource)
                {
                    //var selecteditem = (KeyValuePair<DateTime, List<OrderLogListEntity>>)lvOrderLogs.SelectedItem;

                    var savepath = System.IO.Path.Combine(OASystem.Properties.Settings.Default.OrderLogsFolderPath, logdata.Key.ToString("yyyyMMddHHmmss.fff") + ".csv");
                    using (StreamWriter sw = new StreamWriter(savepath, false, Encoding.GetEncoding(932)))
                    {
                        sw.WriteLine(string.Format("自店舗名,発注先店舗名,レセプト電算コード,医薬品名,包装形態,包装単位,包装単位数,包装総量,剤形区分,注文数,期限切迫,デッド"));

                        //foreach (var kvp in orderdic)
                        //{
                        foreach (var list in logdata.Value)
                        {
                            sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                                , OASystem.Model.DI.自店舗名
                                , list.店名
                                , list.レセプト電算コード
                                , list.医薬品名
                                , list.包装形態
                                , list.包装単位
                                , list.包装単位数
                                , list.包装総量
                                , (int)list.剤形区分
                                , list.注文数
                                , list.Is期限切迫 ? 1 : 0
                                , list.Isデッド品 ? 1 : 0
                                , list.薬価
                                ));
                        }
                        //}

                        modifyFilePath.Add(savepath);
                    }
                }

                //発注履歴削除されたファイルは消す
                foreach (var exists in existsLogFilepaths)
                {
                    if (!modifyFilePath.Contains(exists))
                    {
                        System.IO.File.Delete(exists);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("発注履歴を保存中にエラーが発生しました。\r\nMessage:\r\n" + ex.Message + "\r\nStackTrace:\r\n" + ex.StackTrace, "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            _IsSaved = true;

            return true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (lvOrderLogs.SelectedIndex == -1)
            {
                MessageBox.Show("削除する項目が選択されていません。\r\n左の発注日付から削除する履歴を選択してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var val = (KeyValuePair<DateTime, List<OrderLogListEntity>>)lvOrderLogs.SelectedValue;


            if (MessageBox.Show(string.Format("【{0}】の発注履歴を削除します。\r\n\r\n削除を実行しますか？", val.Key.ToString("yyyy/MM/dd HH:mm:ss")), "確認", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                var deletepath = System.IO.Path.Combine(OASystem.Properties.Settings.Default.OrderLogsFolderPath, val.Key.ToString("yyyyMMddHHmmss.fff") + ".csv");


                if (!File.Exists(deletepath))
                {
                    MessageBox.Show("選択した発注日付のファイルは既に削除されています。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    File.Delete(deletepath);

                    MessageBox.Show("削除しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                _OrderlogList.Remove(val.Key);
                lvOrderLogs.ItemsSource = null;
                lvOrderLogs.ItemsSource = _OrderlogList;


                return;

            }




        }




    }
}
