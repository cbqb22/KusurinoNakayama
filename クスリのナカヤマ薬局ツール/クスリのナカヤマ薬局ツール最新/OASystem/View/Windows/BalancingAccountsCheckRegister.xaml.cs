using System;
using System.Collections.Generic;
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
using System.Linq;
using System.Net;
using Microsoft.Win32;
using OASystem.Model.Entity;
using OASystem.ViewModel.File;
using OASystem.ViewModel.Common.DataConvert;


namespace OASystem.View.Windows
{
    /// <summary>
    /// BalancingAccountsCheckRegister.xaml の相互作用ロジック
    /// </summary>
    public partial class BalancingAccountsCheckRegister : Window
    {

        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        // 帳合先チェックマスタメーカー別のソース
        //Dictionary<string, BalancingAccountsEntity> _BACMMakerDicSource;
        // 帳合先チェックマスタメーカー別のスナップショット
        Dictionary<string, BalancingAccountsEntity> _BACMMakerDicSnapShot;

        // 帳合先チェックマスタ医薬品別のソース
        //List<BalancingAccountsCheckMedicineSortEntity> _BACMedicineSortListSource;
        // 帳合先チェックマスタ医薬品別のスナップショット
        List<BalancingAccountsCheckMedicineSortEntity> _BACMedicineSortListSnapShot;


        //// 帳合先マスタのソース
        //private List<BalancingAccountsEntity> _ListBalanceSource;

        // 帳合先マスタの名称のみリスト化したソース;
        // コンボボックスのItemSource用のDataContext
        private List<string> _ListBAOnlyNameSource = new List<string>();


        public BalancingAccountsCheckRegister()
        {
            InitializeComponent();
            this.Closed += new EventHandler(BalancingAccountsCheckRegister_Closed);
            SetInit();

        }

        void SetMakerListView()
        {
            #region メーカー別

            var loadmaker = LoadCenter.Load帳合先チェックマスタメーカー別_その他のメーカー以外();

            //_BACMMakerDicSource = loadmaker;
            _BACMMakerDicSnapShot = loadmaker;

            var baelist = Model.DI.帳合先マスタ;
            string その他のメーカーの卸ＶＡＮコード = LoadCenter.Load帳合先チェックマスタメーカー別_その他のメーカーのみ()["その他のメーカー"].卸ＶＡＮコード;


            // スナップショットとソースを作成
            _ListBAOnlyNameSource = new List<string>();
            baelist.ForEach(x => _ListBAOnlyNameSource.Add(x.帳合先名称));

            lvBACRegisterメーカー別.DataContext = _ListBAOnlyNameSource;
            lvBACRegisterメーカー別.ItemsSource = _BACMMakerDicSnapShot;

            cmbその他のメーカーリスト.DataContext = _ListBAOnlyNameSource;

            foreach (var bae in baelist)
            {
                if (その他のメーカーの卸ＶＡＮコード == bae.卸ＶＡＮコード)
                {
                    cmbその他のメーカーリスト.SelectedValue = bae.帳合先名称;
                    break;
                }
            }


            #endregion メーカー別

        }

        void Set医薬品別ListView()
        {
            #region 医薬品別

            List<BalancingAccountsCheckMedicineSortEntity> bacmselist = LoadCenter.Load帳合先チェックマスタ医薬品別();
            //_BACMedicineSortListSource = bacmselist;
            _BACMedicineSortListSnapShot = bacmselist;

            lvBACRegister医薬品別.DataContext = _ListBAOnlyNameSource;
            lvBACRegister医薬品別.ItemsSource = _BACMedicineSortListSnapShot;

            #endregion

        }

        private void SetInit()
        {

            SetMakerListView();
            Set医薬品別ListView();
        }

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



        void BalancingAccountsCheckRegister_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }
        private void tcBalancingAccountCheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnMakerSortDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvBACRegisterメーカー別.SelectedIndex == -1)
            {
                MessageBox.Show("削除する項目が選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var val = (KeyValuePair<string, BalancingAccountsEntity>)lvBACRegisterメーカー別.SelectedValue;

            _BACMMakerDicSnapShot.Remove(val.Key);
            lvBACRegisterメーカー別.ItemsSource = null;
            lvBACRegisterメーカー別.ItemsSource = _BACMMakerDicSnapShot;

        }

        private void btnMakerSortAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> _すでに追加されているメーカー名 = new List<string>();
            foreach (var kvp in _BACMMakerDicSnapShot)
            {
                _すでに追加されているメーカー名.Add(kvp.Key);
            }

            BalancingAccountsCheckMakerSortAdd bacmsa = new BalancingAccountsCheckMakerSortAdd();
            bacmsa.cmb帳合先.ItemsSource = this._ListBAOnlyNameSource;
            bacmsa.すでに追加されているメーカー名 = _すでに追加されているメーカー名;
            bacmsa.ShowDialog();

            if (bacmsa.AddFlag)
            {
                var ent = from x in Model.DI.帳合先マスタ
                          where
                               bacmsa.cmb帳合先.SelectedValue.ToString() == x.帳合先名称
                          select x;

                if (ent.Count() != 1)
                {
                    MessageBox.Show(string.Format("不正なデータが存在した為、追加できませんでした。\r\n追加エラーとなったデータ：{0}", bacmsa.cmb帳合先.SelectedValue.ToString()), "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _BACMMakerDicSnapShot.Add(bacmsa.tbxメーカー名.Text, ent.First());

                lvBACRegisterメーカー別.ItemsSource = null;
                lvBACRegisterメーカー別.ItemsSource = _BACMMakerDicSnapShot;
                lvBACRegisterメーカー別.ScrollIntoView(_BACMMakerDicSnapShot.Last());
            }
            else
            {
                return;
            }





        }

        private void ファイルからUpload_医薬品別()
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.Filter =
                "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == true)
            {
                using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding(932)))
                {
                    int counter = 0;
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;
                        if (counter == 1)
                        {
                            continue;
                        }

                        var sepa = line.Split(',');
                        if (sepa.Length != 12)
                        {
                            MessageBox.Show("項目が１２個ありません。Ａ列：JANコード\r\nＢ列：レセプト電算コード\r\nＣ列：医薬品名\r\nＤ列：包装形態\r\nＥ列：包装単位\r\nＦ列：包装単位数\r\nＧ列：包装総量\r\nＨ列：剤形区分(その他=0 内=1 外=2 注=3 歯=4)\r\nＩ列：メーカー名\r\nＪ列：帳合先VANコード\r\nＫ列：修正後帳合先VANコード\r\nＬ列：JAN管理", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        double result0;
                        if (sepa[0].ToString() == "-")
                        {
                            // スルー
                        }
                        else if (sepa[0].Length != 13 || double.TryParse(sepa[0].ToString(), out result0) == false)
                        {
                            MessageBox.Show("Ａ列は帳合先卸のJANコードを半角数字で１３桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        double result1;
                        if (sepa[1].ToString() == "-")
                        {
                            // スルー
                        }
                        else if (sepa[1].Length != 9 || double.TryParse(sepa[1].ToString(), out result1) == false)
                        {
                            MessageBox.Show("Ｂ列はレセプト電算コードを半角数字で９桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }


                        if (sepa[2].ToString().Replace(" ", "").Replace("　", "") == "")
                        {
                            MessageBox.Show("Ｃ列に空欄が含まれます。医薬品名を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (sepa[3].ToString().Replace(" ", "").Replace("　", "") == "")
                        {
                            MessageBox.Show("Ｄ列に空欄が含まれます。包装形態を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (sepa[4].ToString().Replace(" ", "").Replace("　", "") == "")
                        {
                            MessageBox.Show("Ｅ列に空欄が含まれます。包装単位を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }


                        double result5;
                        if (sepa[5].ToString() == "-")
                        {
                            // スルー
                        }
                        else if (double.TryParse(sepa[5].ToString(), out result5) == false)
                        {
                            MessageBox.Show("Ｆ列は包装単位数を半角数字で入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        double result6;
                        if (sepa[6].ToString() == "-")
                        {
                            // スルー
                        }
                        else if (double.TryParse(sepa[6].ToString(), out result6) == false)
                        {
                            MessageBox.Show("Ｇ列は包装総量を半角数字で入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        int result7;
                        if (int.TryParse(sepa[7].ToString(), out result7) == false)
                        {
                            MessageBox.Show("Ｈ列は剤形区分を半角数字で入力してください。\r\nその他=0 内服=1 外用=2 注射=3 歯科=4", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (sepa[8].ToString().Replace(" ", "").Replace("　", "") == "")
                        {
                            MessageBox.Show("Ｉ列に空欄が含まれます。メーカー名を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        double result9;
                        if (sepa[9].ToString() == "-")
                        {
                            // スルー
                        }
                        else if (sepa[9].Length != 9 || double.TryParse(sepa[9].ToString(), out result9) == false)
                        {
                            MessageBox.Show("Ｊ列は帳合先VANコードを半角数字で９桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        double result10;
                        if (sepa[10].ToString() == "-")
                        {
                            // スルー
                        }
                        else if (sepa[10].Length != 9 || double.TryParse(sepa[10].ToString(), out result10) == false)
                        {
                            MessageBox.Show("Ｋ列は修正後帳合先VANコードを半角数字で９桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        int result11;
                        if (int.TryParse(sepa[11].ToString(), out result11) == false)
                        {
                            MessageBox.Show("Ｌ列はJAN管理を半角数字で入力してください。\r\nJAN管理しない場合は0、する場合は1", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (result11 == 0 &&
                           sepa[0].ToString() != "-" &&
                           sepa[3].ToString() != "-" &&
                           sepa[4].ToString() != "-" &&
                           sepa[5].ToString() != "-" &&
                           sepa[6].ToString() != "-" &&
                           sepa[8].ToString() != "-" &&
                           sepa[9].ToString() != "-")
                        {
                            MessageBox.Show("Ｌ列のJAN管理を(しない:0)に設定した場合は、Ａ・Ｄ・Ｅ・Ｆ・Ｇ・Ｉ・Ｊ列は'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;

                        }

                    }
                }
            }
            else
            {
                return;
            }

            UploadCenter.Upload帳合先チェックマスタ_医薬品別(ofd.FileName);

            // ローカルも更新
            System.IO.File.Copy(ofd.FileName, OASystem.Properties.Settings.Default.Download帳合先チェックマスタ医薬品別FilePath, true);

            Set医薬品別ListView();

            MessageBox.Show("帳合先チェックマスタ_医薬品別データを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        private void ファイルからUpload_メーカー別()
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.Filter =
                "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == true)
            {
                using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding(932)))
                {
                    int counter = 0;
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;
                        if (counter == 1)
                        {
                            continue;
                        }

                        var sepa = line.Split(',');
                        if (sepa.Length != 3)
                        {
                            MessageBox.Show("項目が３つありません。Ａ列をメーカー名、Ｂ列をVANコード、Ｃ列を帳合先名称としてください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        int result;
                        if (sepa[1].Length != 9 || int.TryParse(sepa[1].ToString(), out result) == false)
                        {
                            MessageBox.Show("Ｂ列は帳合先卸のVANコードを半角数字で９桁で入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        // その他のメーカーは２行目
                        if (counter == 2)
                        {
                            if (sepa[0] != "その他のメーカー")
                            {
                                MessageBox.Show("２行目のＡ列は'その他のメーカー'としてください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                return;
            }

            UploadCenter.Upload帳合先チェックマスタ_メーカー別(ofd.FileName);

            // ローカルも更新
            System.IO.File.Copy(ofd.FileName, OASystem.Properties.Settings.Default.Download帳合先チェックマスタメーカー別FilePath, true);

            SetMakerListView();

            MessageBox.Show("帳合先チェックマスタ_メーカー別データを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void 表示リストからUpload_医薬品別()
        {
            try
            {
                // TempFileをつくる
                TempFilesManager.FolderCheck();

                if (lvBACRegister医薬品別.Items.Count == 0)
                {
                    if (MessageBox.Show("医薬品別リストの項目が１つもありません。\r\n追加ボタンで追加してください。\r\n\r\nこのままリスト無しでアップロードする場合は、\r\n[はい]を選択してください。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                // 先にコンボボックスの値が選択されているかチェック
                foreach (BalancingAccountsCheckMedicineSortEntity dc in lvBACRegister医薬品別.Items)
                {
                    if (dc.修正後帳合先 == "")
                    {
                        MessageBox.Show("修正後帳合先のドロップダウンリストが選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }

                // 先にコンボボックスの値が選択されているかチェック
                // ItemContainerGeneratorは使わない。まだリストが表示されていないときはnullになるので
                //for (int i = 0; i < lvBACRegister医薬品別.Items.Count; i++)
                //{
                //    ListViewItem listViewItem = (ListViewItem)lvBACRegister医薬品別.ItemContainerGenerator.ContainerFromIndex(i);
                //    if (CheckComboBoxSelect(listViewItem) == false)
                //    {
                //        MessageBox.Show("修正後帳合先のドロップダウンリストが選択されていません。\r\n処理を中止します。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //        return;
                //    }
                //}



                using (StreamWriter sw = new StreamWriter(OASystem.Properties.Settings.Default.TempBalancingAccountsCheckMedicineSortFilePath, false, Encoding.GetEncoding(932)))
                {

                    sw.WriteLine("JANコード,レセプト電算コード,医薬品名,包装形態,包装単位,包装単位数,包装総量,剤形区分,メーカー名,帳合先,修正後帳合先,JAN管理");

                    foreach (BalancingAccountsCheckMedicineSortEntity dc in lvBACRegister医薬品別.Items)
                    {
                        string 修正後帳合先VAN = "";
                        string 帳合先VAN = "";

                        foreach (var ent in Model.DI.帳合先マスタ)
                        {
                            if (ent.卸ＶＡＮコード == dc.修正後帳合先)
                            {
                                修正後帳合先VAN = ent.卸ＶＡＮコード;
                                break;
                            }
                        }

                        if (dc.帳合先 != "-")
                        {
                            foreach (var ent in Model.DI.帳合先マスタ)
                            {
                                if (ent.卸ＶＡＮコード == dc.帳合先)
                                {
                                    帳合先VAN = ent.卸ＶＡＮコード;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            帳合先VAN = dc.帳合先;
                        }


                        if (修正後帳合先VAN == "" || 帳合先VAN == "")
                        {
                            MessageBox.Show("エラーが発生した為、ＣＳＶ出力できませんでした。\r\n。管理者に連絡してください。\r\nエラー内容：帳合先マスタとリストの帳合先名称が一致しませんでした。", "確認", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }


                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", dc.JANコード, dc.レセプト電算コード, dc.医薬品名, dc.包装形態, dc.包装単位, dc.包装単位数, dc.包装総量, (int)dc.剤形区分, dc.販売会社, 帳合先VAN, 修正後帳合先VAN, dc.IsJAN管理 ? 1 : 0);

                    }


                    //for (int i = 0; i < lvBACRegister医薬品別.Items.Count; i++)
                    //{

                    //    ListViewItem listViewItem = (ListViewItem)lvBACRegister医薬品別.ItemContainerGenerator.ContainerFromIndex(i);
                    //    var rowpresenter = FindVisualChild<GridViewRowPresenter>(listViewItem);

                    //    var dc = listViewItem.DataContext as BalancingAccountsCheckMedicineSortEntity;
                    //    if (dc == null)
                    //    {
                    //        continue;
                    //    }

                    //    // column:0
                    //    var cp0 = VisualTreeHelper.GetChild(rowpresenter, 0) as ContentPresenter;
                    //    var temp0 = rowpresenter.Columns[0].CellTemplate;
                    //    var tbl0 = temp0.FindName("tblJAN", cp0) as TextBlock;

                    //    // column:1
                    //    var cp1 = VisualTreeHelper.GetChild(rowpresenter, 1) as ContentPresenter;
                    //    var temp1 = rowpresenter.Columns[1].CellTemplate;
                    //    var tbl1 = temp1.FindName("tbl医薬品名", cp1) as TextBlock;

                    //    // レセプト電算コード
                    //    string レセプト電算コード = dc.レセプト電算コード;

                    //    // 包装形態
                    //    string 包装形態 = dc.包装形態;

                    //    // 包装単位
                    //    string 包装単位 = dc.包装単位;

                    //    // 包装単位数
                    //    string 包装単位数 = dc.包装単位数;

                    //    // 包装総量
                    //    string 包装総量 = dc.包装総量;

                    //    // IsJAN管理
                    //    int JAN管理 = dc.IsJAN管理 ? 1 : 0;

                    //    // column:3
                    //    var cp3 = VisualTreeHelper.GetChild(rowpresenter, 3) as ContentPresenter;
                    //    var temp3 = rowpresenter.Columns[3].CellTemplate;
                    //    var tbl3 = temp3.FindName("tblメーカー名", cp3) as TextBlock;

                    //    // column:4
                    //    var cp4 = VisualTreeHelper.GetChild(rowpresenter, 4) as ContentPresenter;
                    //    var temp4 = rowpresenter.Columns[4].CellTemplate;
                    //    var tbl4 = temp4.FindName("tbl帳合先", cp4) as TextBlock;

                    //    var 帳合Van = DataConvert.帳合先名称ToVANコードConvert(tbl4.Text);

                    //    // column:5
                    //    var cp5 = VisualTreeHelper.GetChild(rowpresenter, 5) as ContentPresenter;
                    //    var temp5 = rowpresenter.Columns[5].CellTemplate;
                    //    var cmb5 = temp5.FindName("cmb修正後帳合先", cp5) as ComboBox;

                    //    var 修正後帳合Van = DataConvert.帳合先名称ToVANコードConvert(cmb5.SelectedValue.ToString());

                    //    if (修正後帳合Van == "")
                    //    {
                    //        MessageBox.Show("エラーが発生した為、ＣＳＶ出力できませんでした。\r\n。管理者に連絡してください。\r\nエラー内容：帳合先マスタとリストの帳合先名称が一致しませんでした。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                    //    }

                    //    sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", tbl0.Text, レセプト電算コード, tbl1.Text, 包装形態, 包装単位, 包装単位数, 包装総量, (int)dc.剤形区分, tbl3.Text, 帳合Van, 修正後帳合Van, JAN管理);

                    //}


                    sw.Flush();
                }

                UploadCenter.Upload帳合先チェックマスタ_医薬品別(OASystem.Properties.Settings.Default.TempBalancingAccountsCheckMedicineSortFilePath);

                // ローカルも更新
                System.IO.File.Copy(OASystem.Properties.Settings.Default.TempBalancingAccountsCheckMedicineSortFilePath, OASystem.Properties.Settings.Default.Download帳合先チェックマスタ医薬品別FilePath, true);

                MessageBox.Show("帳合先チェックマスタ_医薬品別データを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生した為、アップロードを中止しました。\r\nStackTrace:" + ex.StackTrace + "\r\nMessage:" + ex.Message, "エラー");
                return;
            }

        }

        private void 表示リストからUpload_メーカー別()
        {
            try
            {
                // TempFileをつくる
                TempFilesManager.FolderCheck();

                if (lvBACRegisterメーカー別.Items.Count == 0)
                {
                    if (MessageBox.Show("メーカー別リストの項目が１つもありません。\r\n追加ボタンで追加してください。\r\n\r\nこのままリスト無しでアップロードする場合は、\r\n[はい]を選択してください。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                if (cmbその他のメーカーリスト.SelectedIndex == -1)
                {
                    MessageBox.Show("その他のメーカーの帳合先が選択されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                foreach (KeyValuePair<string, BalancingAccountsEntity> item in lvBACRegisterメーカー別.Items)
                {
                     
                    if (string.IsNullOrEmpty(item.Value.帳合先名称))
                    {
                        MessageBox.Show("帳合先名称が選択されていない項目があります。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    if (string.IsNullOrEmpty(item.Value.卸ＶＡＮコード))
                    {
                        MessageBox.Show("帳合先名称が卸VANコードに変換できませんでした。\r\n管理者に連絡してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                }


                //// 先にコンボボックスの値が選択されているかチェック
                //for (int i = 0; i < lvBACRegisterメーカー別.Items.Count; i++)
                //{
                //    ListViewItem listViewItem = (ListViewItem)lvBACRegisterメーカー別.ItemContainerGenerator.ContainerFromIndex(i);
                //    if (CheckComboBoxSelect(listViewItem) == false)
                //    {
                //        MessageBox.Show("帳合先のドロップダウンリストが選択されていません。\r\n処理を中止します。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //        return;
                //    }
                //}


                string その他のメーカーVANコード = "";
                string その他のメーカー帳合先名称 = "";
                foreach (var ent in Model.DI.帳合先マスタ)
                {
                    if (ent.帳合先名称 == cmbその他のメーカーリスト.SelectedValue.ToString())
                    {
                        その他のメーカーVANコード = ent.卸ＶＡＮコード;
                        その他のメーカー帳合先名称 = ent.帳合先名称;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(その他のメーカーVANコード))
                {
                    MessageBox.Show("エラーが発生した為、アップロードできませんでした。\r\n。管理者に連絡してください。\r\nエラー内容：帳合先マスタとその他のメーカーの帳合先名称が一致しませんでした。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                using (StreamWriter sw = new StreamWriter(OASystem.Properties.Settings.Default.TempBalancingAccountsCheckMakerSortFilePath, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("メーカー名,卸VANコード,帳合先名称");
                    sw.WriteLine(string.Format("その他のメーカー,{0},{1}", その他のメーカーVANコード, その他のメーカー帳合先名称));


                    foreach (KeyValuePair<string, BalancingAccountsEntity> item in lvBACRegisterメーカー別.Items)
                    {
                        sw.WriteLine("{0},{1},{2}", item.Key, item.Value.卸ＶＡＮコード, item.Value.帳合先名称);
                    }


                    //var ig = lvBACRegisterメーカー別.ItemContainerGenerator;
                    //var item0 = ig.ContainerFromIndex(0);

                    //for (int i = 0; i < lvBACRegisterメーカー別.Items.Count; i++)
                    //{
                    //    ListViewItem listViewItem = (ListViewItem)lvBACRegisterメーカー別.ItemContainerGenerator.ContainerFromIndex(i);

                    //    // ListViewItem の VisualTree より、ContentPresenter を検索する
                    //    TextBlock tb = FindVisualChild<TextBlock>(listViewItem);
                    //    ComboBox cmb = FindVisualChild<ComboBox>(listViewItem);

                    //    string van = "";
                    //    string baname = "";
                    //    foreach (var ent in Model.DI.帳合先マスタ)
                    //    {
                    //        if (ent.帳合先名称 == cmb.SelectedValue.ToString())
                    //        {
                    //            van = ent.卸ＶＡＮコード;
                    //            baname = ent.帳合先名称;
                    //            break;
                    //        }
                    //    }

                    //    if (string.IsNullOrEmpty(van))
                    //    {
                    //        MessageBox.Show("エラーが発生した為、アップロードできませんでした。\r\n。管理者に連絡してください。\r\nエラー内容：帳合先マスタとリストの帳合先名称が一致しませんでした。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                    //    }


                    //    sw.WriteLine("{0},{1},{2}", tb.Text, van, baname);

                    //}


                    sw.Flush();
                }

                UploadCenter.Upload帳合先チェックマスタ_メーカー別(OASystem.Properties.Settings.Default.TempBalancingAccountsCheckMakerSortFilePath);

                // ローカルも更新
                System.IO.File.Copy(OASystem.Properties.Settings.Default.TempBalancingAccountsCheckMakerSortFilePath, OASystem.Properties.Settings.Default.Download帳合先チェックマスタメーカー別FilePath, true);

                MessageBox.Show("帳合先チェックマスタ_メーカー別データを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生した為、アップロードを中止しました。\r\nStackTrace:" + ex.StackTrace + "\r\nMessage:" + ex.Message, "エラー");
                return;
            }


        }

        private void btnMakerSortUpload_Click(object sender, RoutedEventArgs e)
        {

            bdMakerSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;


        }

        private void btnMakerSortCSV_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "帳合先チェックマスタ_メーカー別"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV File(.csv)|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                BalancingAccountsEntity baen = null;
                foreach (var ent in Model.DI.帳合先マスタ)
                {
                    if (ent.帳合先名称 == cmbその他のメーカーリスト.SelectedValue.ToString())
                    {
                        baen = ent;
                        break;
                    }
                }
                if (baen == null)
                {
                    MessageBox.Show("エラーが発生した為、ＣＳＶ出力できませんでした。\r\n。管理者に連絡してください。\r\nエラー内容：帳合先マスタとその他のメーカーの帳合先名称が一致しませんでした。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                using (StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("メーカー名,卸VANコード,帳合先名称");
                    sw.WriteLine(string.Format("その他のメーカー,{0},{1}", baen.卸ＶＡＮコード, baen.帳合先名称));


                    foreach (KeyValuePair<string, BalancingAccountsEntity> item in lvBACRegisterメーカー別.Items)
                    {
                        sw.WriteLine("{0},{1},{2}", item.Key, item.Value.卸ＶＡＮコード, item.Value.帳合先名称);
                    }


                
                    sw.Flush();
                }

                MessageBox.Show(string.Format("{0}に出力しました。", filename), "確認", MessageBoxButton.OK, MessageBoxImage.Information);


            }


        }

        private void btnMedicineSortCSV_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "帳合先チェックマスタ_医薬品別"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV File(.csv)|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                using (StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(932)))
                {


                    sw.WriteLine("JANコード,レセプト電算コード,医薬品名,包装形態,包装単位,包装単位数,包装総量,剤形区分(その他=0 内=1 外=2 注=3 歯=4),メーカー名,帳合先VANコード,修正後帳合先VANコード,JAN管理");

                    //sw.WriteLine("JANコード,レセプト電算コード,医薬品名,包装,メーカー名,帳合先,修正後帳合先,JAN管理");

                    foreach (BalancingAccountsCheckMedicineSortEntity dc in lvBACRegister医薬品別.Items)
                    {

                        string 修正後帳合先VAN = "";
                        string 帳合先VAN = "";

                        foreach (var ent in Model.DI.帳合先マスタ)
                        {
                            if (ent.卸ＶＡＮコード == dc.修正後帳合先)
                            {
                                修正後帳合先VAN = ent.卸ＶＡＮコード;
                                break;
                            }
                        }

                        if (dc.帳合先 != "-")
                        {
                            foreach (var ent in Model.DI.帳合先マスタ)
                            {
                                if (ent.卸ＶＡＮコード == dc.帳合先)
                                {
                                    帳合先VAN = ent.卸ＶＡＮコード;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            帳合先VAN = dc.帳合先;
                        }


                        if (修正後帳合先VAN == "" || 帳合先VAN == "")
                        {
                            MessageBox.Show("エラーが発生した為、ＣＳＶ出力できませんでした。\r\n。管理者に連絡してください。\r\nエラー内容：帳合先マスタとリストの帳合先名称が一致しませんでした。", "確認", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }


                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", dc.JANコード, dc.レセプト電算コード, dc.医薬品名, dc.包装形態, dc.包装単位, dc.包装単位数, dc.包装総量, (int)dc.剤形区分, dc.販売会社, 帳合先VAN, 修正後帳合先VAN, dc.IsJAN管理 ? 1 : 0);

                    }


                    sw.Flush();
                }

                MessageBox.Show(string.Format("{0}に出力しました。", filename), "確認", MessageBoxButton.OK, MessageBoxImage.Information);


            }



        }

        private void btnMedicineSortDelete_Click(object sender, RoutedEventArgs e)
        {

            if (lvBACRegister医薬品別.SelectedIndex == -1)
            {
                MessageBox.Show("削除する項目が選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var val = lvBACRegister医薬品別.SelectedValue as BalancingAccountsCheckMedicineSortEntity;

            _BACMedicineSortListSnapShot.Remove(val);
            lvBACRegister医薬品別.ItemsSource = null;
            lvBACRegister医薬品別.ItemsSource = _BACMedicineSortListSnapShot;

        }

        private void btnMedicineSortAdd_Click(object sender, RoutedEventArgs e)
        {
            BalancingAccountsCheckMedicineSortSelectMedicine bacmssm = new BalancingAccountsCheckMedicineSortSelectMedicine();
            bacmssm.BACMedicineSortListSnapShotCopy = _BACMedicineSortListSnapShot;
            bacmssm.ShowDialog();

            if (bacmssm.非JAN管理選択Flag)
            {

                var selectedEnt = bacmssm.lvMedicineInfo.SelectedValue as BalancingAccountsCheckMedicineSortEntity;


                // TODO:非JAN管理の修正後帳合先をデフォルトでselectedindex=0にするかどうか
                BalancingAccountsCheckMedicineSortEntity addEnt = new BalancingAccountsCheckMedicineSortEntity();
                addEnt.JANコード = "-";
                addEnt.レセプト電算コード = selectedEnt.レセプト電算コード;
                addEnt.医薬品名 = selectedEnt.医薬品名;
                addEnt.包装形態 = "-";
                addEnt.包装単位 = "-";
                addEnt.包装単位数 = "-";
                addEnt.包装総量 = "-";
                addEnt.剤形区分 = selectedEnt.剤形区分;
                addEnt.販売会社 = "-";
                addEnt.帳合先 = "-";
                addEnt.修正後帳合先 = "";
                addEnt.IsJAN管理 = false;

                _BACMedicineSortListSnapShot.Add(addEnt);

                // そのうち表示順を並び替えできるようにする。
                //_BACMedicineSortListSnapShot = _BACMedicineSortListSnapShot.OrderBy(x => x.医薬品名).ToList();

                lvBACRegister医薬品別.ItemsSource = null;
                lvBACRegister医薬品別.ItemsSource = _BACMedicineSortListSnapShot;

                lvBACRegister医薬品別.ScrollIntoView(_BACMedicineSortListSnapShot.Last());
            }
            else if (bacmssm.選択Flag)
            {

                var selected = bacmssm.DetailWindowChild.lvMedicineInfo.SelectedValue as BalancingAccountsCheckMedicineSortEntity;

                string 帳合先 = "";
                foreach (var makersnap in _BACMMakerDicSnapShot)
                {
                    if (selected.販売会社 == makersnap.Key)
                    {
                        帳合先 = makersnap.Value.卸ＶＡＮコード;
                        break;
                    }
                }

                if (帳合先 == "")
                {
                    帳合先 = cmbその他のメーカーリスト.SelectedValue.ToString();
                }

                selected.帳合先 = 帳合先;
                selected.修正後帳合先 = 帳合先;
                selected.IsJAN管理 = true;

                _BACMedicineSortListSnapShot.Add(selected);

                if (bacmssm.DetailWindowChild.非JAN管理としても追加Flag)
                {

                    // TODO:非JAN管理の修正後帳合先をデフォルトでselectedindex=0にするかどうか
                    BalancingAccountsCheckMedicineSortEntity addEnt = new BalancingAccountsCheckMedicineSortEntity();
                    addEnt.JANコード = "-";
                    addEnt.レセプト電算コード = selected.レセプト電算コード;
                    addEnt.医薬品名 = selected.医薬品名;
                    addEnt.包装形態 = "-";
                    addEnt.包装単位 = "-";
                    addEnt.包装単位数 = "-";
                    addEnt.包装総量 = "-";
                    addEnt.剤形区分 = selected.剤形区分;
                    addEnt.販売会社 = "-";
                    addEnt.帳合先 = "-";  //上のJAN管理で帳合先を入れているので、もし今後入れるなら直接使用しないように
                    addEnt.修正後帳合先 = ""; //同上
                    addEnt.IsJAN管理 = false;

                    _BACMedicineSortListSnapShot.Add(addEnt);

                }

                // そのうち表示順を並び替えできるようにする。
                //_BACMedicineSortListSnapShot = _BACMedicineSortListSnapShot.OrderBy(x => x.医薬品名).ToList();

                lvBACRegister医薬品別.ItemsSource = null;
                lvBACRegister医薬品別.ItemsSource = _BACMedicineSortListSnapShot;

                lvBACRegister医薬品別.ScrollIntoView(_BACMedicineSortListSnapShot.Last());
            }

        }

        private void btnMedicineSortUpload_Click(object sender, RoutedEventArgs e)
        {
            bdMedicineSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;

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

        /// <summary>
        /// 指定のListViewItem内のComboBoxの値が選択されているかチェック
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool CheckComboBoxSelect(ListViewItem item)
        {
            var cmb = FindVisualChild<ComboBox>(item);

            if (cmb != null && cmb.SelectedIndex != -1)
            {
                return true;
            }

            return false;
        }


        private void bdUpdateButtonListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            // すでに表示されていたら引き続き印刷ボタンから離れても表示させる
            if (bdMakerSortUpdateButtonListBox.Visibility == System.Windows.Visibility.Visible)
            {
                bdMakerSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void bdUpdateButtonListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            bdMakerSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private void lbMakerSortUpdate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bdMakerSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;

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

            if (lbi.Content.ToString() == "表示リストからUP")
            {
                表示リストからUpload_メーカー別();
            }
            else if (lbi.Content.ToString() == "ファイルからUP")
            {
                ファイルからUpload_メーカー別();
            }

            lb.SelectedIndex = -1;

        }


        private void lbMedicineSortUpdate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bdMedicineSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;

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

            if (lbi.Content.ToString() == "表示リストからUP")
            {
                表示リストからUpload_医薬品別();
            }
            else if (lbi.Content.ToString() == "ファイルからUP")
            {
                ファイルからUpload_医薬品別();
            }

            lb.SelectedIndex = -1;

        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                return;
            }

            
            var cmb = sender as ComboBox;
            if(cmb == null)
            {
                return;
            }

            var fe = VisualTreeHelper.GetParent(cmb) as FrameworkElement;
            ListViewItem lvi = null;
            while (fe != null)
            {
                if (fe is ListViewItem)
                {
                    lvi = fe as ListViewItem;
                    break;
                }
                else
                {
                    fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
                }
            }

            if (lvi == null)
            {
                return;
            }

            var ent = (KeyValuePair<string, BalancingAccountsEntity>)lvi.DataContext;

            // ここでVANコードを置き換える
            foreach (var data in Model.DI.帳合先マスタ)
            {
                if (data.帳合先名称 == ent.Value.帳合先名称)
                {
                    ent.Value.卸ＶＡＮコード = data.卸ＶＡＮコード;
                    break;
                }
            }

        }



        private void bdMedicineSortUpdateButtonListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            // すでに表示されていたら引き続き印刷ボタンから離れても表示させる
            if (bdMedicineSortUpdateButtonListBox.Visibility == System.Windows.Visibility.Visible)
            {
                bdMedicineSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void bdMedicineSortUpdateButtonListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            bdMedicineSortUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;

        }



    }
}
