using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using OASystem.Model.Entity;
using OASystem.ViewModel.Common.DataConvert;

namespace OASystem.View.Windows
{
    /// <summary>
    /// IndividualBasedMedicineSelectMaker.xaml の相互作用ロジック
    /// </summary>
    public partial class BalancingAccountsCheckMedicineSortSelectMedicine : Window
    {
        private bool _非JAN管理選択Flag;
        public bool 非JAN管理選択Flag
        {
            get { return _非JAN管理選択Flag; }
            set { _非JAN管理選択Flag = value; }
        }

        private bool _選択Flag;
        public bool 選択Flag
        {
            get { return _選択Flag; }
            set { _選択Flag = value; }
        }

        //子ウィンドウをホスト
        private BalancingAccountsCheckMedicineSortSelectMedicineDetail _DetailWindowChild;
        public BalancingAccountsCheckMedicineSortSelectMedicineDetail DetailWindowChild
        {
            get { return _DetailWindowChild; }
            set { _DetailWindowChild = value; }
        }



        // 帳合先チェックマスタ医薬品別のスナップショット
        // すでに追加されているものをチェックする為のスナップ
        List<BalancingAccountsCheckMedicineSortEntity> _BACMedicineSortListSnapShotCopy;
        public List<BalancingAccountsCheckMedicineSortEntity> BACMedicineSortListSnapShotCopy
        {
            get { return _BACMedicineSortListSnapShotCopy; }
            set { _BACMedicineSortListSnapShotCopy = value; }
        }

        /// <summary>
        /// 次のJAN選択画面で使用
        /// </summary>
        private List<BalancingAccountsCheckMedicineSortEntity> _JANで分けたBACList;


        public BalancingAccountsCheckMedicineSortSelectMedicine()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BalancingAccountsCheckMedicineSortSelectMedicine_Loaded);
        }

        void BalancingAccountsCheckMedicineSortSelectMedicine_Loaded(object sender, RoutedEventArgs e)
        {
            tbキーワード.Focus();
        }

        private void btnキーワード検索_Click(object sender, RoutedEventArgs e)
        {
            if (this.tbキーワード.Text == "")
            {
                MessageBox.Show("キーワードが入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var keysplit = tbキーワード.Text.Replace('　', ' ').Split(' ');


            List<BalancingAccountsCheckMedicineSortEntity> entlist = new List<BalancingAccountsCheckMedicineSortEntity>();


            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.DownloadMEDIS_HOT13lFilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    var sepa = line.Split(',');

                    // 今後増える可能性もあるので現状の24以下とする
                    if (sepa.Length < 24)
                    {
                        throw new Exception("MEDIS_HOT13にインデックスが２４ではない不正なデータが含まれて降ります。\r\nCounter: " + counter);
                    }

                    bool nokey = false;

                    foreach (var key in keysplit)
                    {
                        if (!sepa[12].Contains(key))
                        {
                            nokey = true;
                            break;
                        }
                    }

                    if (!nokey)
                    {

                        //  6.JANコード
                        //  8.個別医薬品コード
                        //  9.レセプト電算コード
                        // 12.医薬品名
                        // 15.包装形態
                        // 16.包装単位数
                        // 17.包装単位
                        // 18.包装総量
                        // 20.剤形区分
                        // 21.製薬会社
                        // 22.販売会社

                        BalancingAccountsCheckMedicineSortEntity ent = new BalancingAccountsCheckMedicineSortEntity();

                        ent.JANコード = sepa[5].Replace("\"", "");
                        ent.個別医薬品コード = sepa[7].Replace("\"", "");
                        ent.レセプト電算コード = sepa[8].Replace("\"", "");
                        ent.医薬品名 = sepa[11].Replace("\"", "");
                        ent.包装形態 = sepa[14].Replace("\"", "");
                        ent.包装単位数 = sepa[15].Replace("\"", "");
                        ent.包装単位 = sepa[16].Replace("\"", "");
                        ent.包装総量 = sepa[17].Replace("\"", "");
                        ent.剤形区分 = DataConvert.漢字To剤形Enum(sepa[19].Replace("\"", ""));
                        ent.製薬会社 = sepa[20].Replace("\"", "");
                        ent.販売会社 = sepa[21].Replace("\"", "");
                        ent.IsJAN管理 = true;

                        entlist.Add(ent);
                    }
                }
            }

            _JANで分けたBACList = new List<BalancingAccountsCheckMedicineSortEntity>(entlist);
            entlist = entlist.Distinct(new BalancingAccountsCheckMedicineSortEntityNameCodeComparer()).ToList();


            if (entlist.Count == 0)
            {
                MessageBox.Show("キーワードに該当する医薬品名が見つかりませんでした。\r\n細かいキーワードで検索してください。\r\n漢字も使用可能です。カナは全角で入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            this.lvMedicineInfo.ItemsSource = entlist;

        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btn非JAN管理で追加_Click(object sender, RoutedEventArgs e)
        {
            if (lvMedicineInfo.SelectedIndex == -1)
            {
                MessageBox.Show("医薬品が選択されてません。\r\nキーワード検索後、医薬品を表示されたリストから選択してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            BalancingAccountsCheckMedicineSortEntity ent = lvMedicineInfo.SelectedValue as BalancingAccountsCheckMedicineSortEntity;

            bool hasData = false;
            foreach (var snaprow in _BACMedicineSortListSnapShotCopy)
            {
                if (snaprow.レセプト電算コード == ent.レセプト電算コード && !snaprow.IsJAN管理 )
                {
                    hasData = true;
                    break;
                }
            }

            if (hasData)
            {
                MessageBox.Show("この医薬品はすでに非JAN管理として追加されてます。\r\n修正または削除する場合は医薬品別のチェックリストより直接編集してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            非JAN管理選択Flag = true;
            this.Close();
            
        }

        private void btnJANを選択_Click(object sender, RoutedEventArgs e)
        {
            if (lvMedicineInfo.SelectedIndex == -1)
            {
                MessageBox.Show("医薬品が選択されてません。\r\nキーワード検索後、医薬品を表示されたリストから選択してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var selected = lvMedicineInfo.SelectedValue as BalancingAccountsCheckMedicineSortEntity;


            List<BalancingAccountsCheckMedicineSortEntity> janlist = new List<BalancingAccountsCheckMedicineSortEntity>();
            foreach (var list in _JANで分けたBACList)
            {
                if (list.レセプト電算コード == selected.レセプト電算コード)
                {
                    // すでにJAN追加されているものは次の画面のJAN選択リストに表示させない。
                    var search = from x in BACMedicineSortListSnapShotCopy
                                 where
                                    x.JANコード == list.JANコード
                                 select x;

                    if (search.Count() == 0)
                    {
                        janlist.Add(list);
                    }

                }
            }

            DetailWindowChild = new BalancingAccountsCheckMedicineSortSelectMedicineDetail();
            DetailWindowChild.BACMedicineSortListSnapShotCopy = BACMedicineSortListSnapShotCopy;
            DetailWindowChild.PreviousSelectedBACMSEnt = selected;
            DetailWindowChild.JANList = janlist;
            DetailWindowChild.ShowDialog();


            if (DetailWindowChild.選択Flag)
            {
                選択Flag = true;
                this.Close();
            }
            else
            {
                選択Flag = false;
            }
        }

        private void btn個別管理医薬品追加_Click(object sender, RoutedEventArgs e)
        {

            List<BalancingAccountsCheckMedicineSortEntity> janlist = new List<BalancingAccountsCheckMedicineSortEntity>();

            var indimaster = OASystem.Model.DI.個別管理医薬品マスタ;

            foreach (var list in indimaster)
            {

                // すでにJAN追加されているものは次の画面のJAN選択リストに表示させない。
                var search = from x in BACMedicineSortListSnapShotCopy
                             where
                                x.JANコード == list.JANコード
                             select x;

                if (search.Count() == 0)
                {
                    BalancingAccountsCheckMedicineSortEntity ent = new BalancingAccountsCheckMedicineSortEntity();
                    ent.JANコード = list.JANコード;
                    ent.レセプト電算コード = "";
                    ent.医薬品名 = list.医薬品名;
                    ent.包装形態 = list.包装形態;
                    ent.包装単位 = list.包装単位;
                    ent.包装単位数 = list.包装単位数;
                    ent.包装総量 = list.包装総量;
                    ent.販売会社 = list.販売会社;
                    ent.製薬会社 = list.製薬会社;
                    ent.剤形区分 = list.剤形区分;
                    janlist.Add(ent);

                }
            }


            DetailWindowChild = new BalancingAccountsCheckMedicineSortSelectMedicineDetail();
            DetailWindowChild.BACMedicineSortListSnapShotCopy = BACMedicineSortListSnapShotCopy;
            DetailWindowChild.Is個別管理医薬品マスタ = true;
            //DetailWindowChild.PreviousSelectedBACMSEnt = selected;
            DetailWindowChild.JANList = janlist;
            DetailWindowChild.ShowDialog();


            if (DetailWindowChild.選択Flag)
            {
                選択Flag = true;
                this.Close();
            }
            else
            {
                選択Flag = false;
            }

        }
    }
}
