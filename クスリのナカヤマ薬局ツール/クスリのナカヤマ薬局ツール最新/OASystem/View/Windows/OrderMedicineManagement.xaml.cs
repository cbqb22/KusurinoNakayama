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
    public partial class OrderMedicineManagement : Window
    {

        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        // 保護リストのスナップショット
        List<ProtectEntity> _ProtectListSnapShot;


        // 優先移動リストのスナップショット
        List<PriorityMoveEntity> _PriorityMoveListSnapShot;



        public OrderMedicineManagement()
        {
            InitializeComponent();
            this.Closed += new EventHandler(OrderMedicineManagement_Closed);
            SetInit();
           
        }

        void SetStoreName()
        {
            tblStoreNamerTitleProtext.Text = OASystem.Model.DI.自店舗名 + " 保護リスト";
            tblStoreNameTitlePriorityMove.Text = OASystem.Model.DI.自店舗名 + " 優先移動リスト";

            //System.Diagnostics.Debug.WriteLine(tblStoreNamerTitleProtext.Text + this.GetHashCode());

        }

        void SetProtectListView()
        {
            #region 保護リスト
            
            var protectlist = LoadCenter.Load保護リスト(OASystem.Model.DI.自店舗名);

            _ProtectListSnapShot = protectlist;
            lvProtectList.ItemsSource = _ProtectListSnapShot;

            #endregion 保護リスト

        }

        void SetPriorityMoveListView()
        {
            #region 優先移動リスト

            var prioritylist = LoadCenter.Load優先移動リスト(OASystem.Model.DI.自店舗名);
            _PriorityMoveListSnapShot = prioritylist;

            lvPriorityMoveList.ItemsSource = _PriorityMoveListSnapShot;

            #endregion　優先移動リスト

        }

        private void SetInit()
        {
            SetStoreName();
            SetProtectListView();
            SetPriorityMoveListView();

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

        void OrderMedicineManagement_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }



        private void ファイルからUpload_優先移動リスト()
        {
            ////OpenFileDialogクラスのインスタンスを作成
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.InitialDirectory = @"C:\";
            //ofd.Filter =
            //    "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            //ofd.FilterIndex = 1;
            //ofd.Title = "開くファイルを選択してください";
            //ofd.RestoreDirectory = true;
            //ofd.CheckFileExists = true;
            //ofd.CheckPathExists = true;

            ////ダイアログを表示する
            //if (ofd.ShowDialog() == true)
            //{
            //    using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding(932)))
            //    {
            //        int counter = 0;
            //        var line = "";
            //        while ((line = sr.ReadLine()) != null)
            //        {
            //            counter++;
            //            if (counter == 1)
            //            {
            //                continue;
            //            }

            //            var sepa = line.Split(',');
            //            if (sepa.Length != 12)
            //            {
            //                MessageBox.Show("項目が１２個ありません。Ａ列：JANコード\r\nＢ列：レセプト電算コード\r\nＣ列：医薬品名\r\nＤ列：包装形態\r\nＥ列：包装単位\r\nＦ列：包装単位数\r\nＧ列：包装総量\r\nＨ列：剤形区分(その他=0 内=1 外=2 注=3 歯=4)\r\nＩ列：メーカー名\r\nＪ列：帳合先VANコード\r\nＫ列：修正後帳合先VANコード\r\nＬ列：JAN管理", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            double result0;
            //            if (sepa[0].ToString() == "-")
            //            {
            //                // スルー
            //            }
            //            else if (sepa[0].Length != 13 || double.TryParse(sepa[0].ToString(), out result0) == false)
            //            {
            //                MessageBox.Show("Ａ列は帳合先卸のJANコードを半角数字で１３桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            double result1;
            //            if (sepa[1].ToString() == "-")
            //            {
            //                // スルー
            //            }
            //            else if (sepa[1].Length != 9 || double.TryParse(sepa[1].ToString(), out result1) == false)
            //            {
            //                MessageBox.Show("Ｂ列はレセプト電算コードを半角数字で９桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }


            //            if (sepa[2].ToString().Replace(" ", "").Replace("　", "") == "")
            //            {
            //                MessageBox.Show("Ｃ列に空欄が含まれます。医薬品名を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            if (sepa[3].ToString().Replace(" ", "").Replace("　", "") == "")
            //            {
            //                MessageBox.Show("Ｄ列に空欄が含まれます。包装形態を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            if (sepa[4].ToString().Replace(" ", "").Replace("　", "") == "")
            //            {
            //                MessageBox.Show("Ｅ列に空欄が含まれます。包装単位を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }


            //            double result5;
            //            if (sepa[5].ToString() == "-")
            //            {
            //                // スルー
            //            }
            //            else if (double.TryParse(sepa[5].ToString(), out result5) == false)
            //            {
            //                MessageBox.Show("Ｆ列は包装単位数を半角数字で入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            double result6;
            //            if (sepa[6].ToString() == "-")
            //            {
            //                // スルー
            //            }
            //            else if (double.TryParse(sepa[6].ToString(), out result6) == false)
            //            {
            //                MessageBox.Show("Ｇ列は包装総量を半角数字で入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            int result7;
            //            if (int.TryParse(sepa[7].ToString(), out result7) == false)
            //            {
            //                MessageBox.Show("Ｈ列は剤形区分を半角数字で入力してください。\r\nその他=0 内服=1 外用=2 注射=3 歯科=4", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            if (sepa[8].ToString().Replace(" ", "").Replace("　", "") == "")
            //            {
            //                MessageBox.Show("Ｉ列に空欄が含まれます。メーカー名を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            double result9;
            //            if (sepa[9].ToString() == "-")
            //            {
            //                // スルー
            //            }
            //            else if (sepa[9].Length != 9 || double.TryParse(sepa[9].ToString(), out result9) == false)
            //            {
            //                MessageBox.Show("Ｊ列は帳合先VANコードを半角数字で９桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            double result10;
            //            if (sepa[10].ToString() == "-")
            //            {
            //                // スルー
            //            }
            //            else if (sepa[10].Length != 9 || double.TryParse(sepa[10].ToString(), out result10) == false)
            //            {
            //                MessageBox.Show("Ｋ列は修正後帳合先VANコードを半角数字で９桁または'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            int result11;
            //            if (int.TryParse(sepa[11].ToString(), out result11) == false)
            //            {
            //                MessageBox.Show("Ｌ列はJAN管理を半角数字で入力してください。\r\nJAN管理しない場合は0、する場合は1", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            if (result11 == 0 &&
            //               sepa[0].ToString() != "-" &&
            //               sepa[3].ToString() != "-" &&
            //               sepa[4].ToString() != "-" &&
            //               sepa[5].ToString() != "-" &&
            //               sepa[6].ToString() != "-" &&
            //               sepa[8].ToString() != "-" &&
            //               sepa[9].ToString() != "-")
            //            {
            //                MessageBox.Show("Ｌ列のJAN管理を(しない:0)に設定した場合は、Ａ・Ｄ・Ｅ・Ｆ・Ｇ・Ｉ・Ｊ列は'-'を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;

            //            }

            //        }
            //    }
            //}
            //else
            //{
            //    return;
            //}

            //UploadCenter.Upload帳合先チェックマスタ_医薬品別(ofd.FileName);

            //// ローカルも更新
            //System.IO.File.Copy(ofd.FileName, OASystem.Properties.Settings.Default.Download帳合先チェックマスタ医薬品別FilePath, true);

            //SetPriorityMoveListView();

            //MessageBox.Show("帳合先チェックマスタ_医薬品別データを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        private void ファイルからUpload_保護リスト()
        {
            ////OpenFileDialogクラスのインスタンスを作成
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.InitialDirectory = @"C:\";
            //ofd.Filter =
            //    "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            //ofd.FilterIndex = 1;
            //ofd.Title = "開くファイルを選択してください";
            //ofd.RestoreDirectory = true;
            //ofd.CheckFileExists = true;
            //ofd.CheckPathExists = true;

            ////ダイアログを表示する
            //if (ofd.ShowDialog() == true)
            //{
            //    using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding(932)))
            //    {
            //        int counter = 0;
            //        var line = "";
            //        while ((line = sr.ReadLine()) != null)
            //        {
            //            counter++;
            //            if (counter == 1)
            //            {
            //                continue;
            //            }

            //            var sepa = line.Split(',');
            //            if (sepa.Length != 3)
            //            {
            //                MessageBox.Show("項目が３つありません。Ａ列をメーカー名、Ｂ列をVANコード、Ｃ列を帳合先名称としてください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            int result;
            //            if (sepa[1].Length != 9 || int.TryParse(sepa[1].ToString(), out result) == false)
            //            {
            //                MessageBox.Show("Ｂ列は帳合先卸のVANコードを半角数字で９桁で入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            // その他のメーカーは２行目
            //            if (counter == 2)
            //            {
            //                if (sepa[0] != "その他のメーカー")
            //                {
            //                    MessageBox.Show("２行目のＡ列は'その他のメーカー'としてください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    return;
            //}

            //UploadCenter.Upload帳合先チェックマスタ_メーカー別(ofd.FileName);

            //// ローカルも更新
            //System.IO.File.Copy(ofd.FileName, OASystem.Properties.Settings.Default.Download帳合先チェックマスタメーカー別FilePath, true);

            //SetProtectListView();

            //MessageBox.Show("帳合先チェックマスタ_メーカー別データを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void 表示リストからUpload_優先移動リスト()
        {
            try
            {
                // TempFileをつくる
                TempFilesManager.FolderCheck();

                using (StreamWriter sw = new StreamWriter(OASystem.Properties.Settings.Default.Temp優先移動リストFilePath, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("レセプト電算コード,医薬品名称,コメント");
                    sw.WriteLine(OASystem.Model.DI.自店舗名);


                    foreach (PriorityMoveEntity item in lvPriorityMoveList.Items)
                    {
                        sw.WriteLine("{0},{1},{2}", item.レセプト電算コード, item.医薬品名,item.コメント);
                    }




                    sw.Flush();
                }

                UploadCenter.Upload優先移動リスト(OASystem.Properties.Settings.Default.Temp優先移動リストFilePath, OASystem.Model.DI.自店舗名);

                // ローカルも更新
                string localDownload優先移動リストパス = System.IO.Path.Combine(OASystem.Properties.Settings.Default.Download優先移動リストFolderPath, OASystem.Model.DI.自店舗名 + ".csv");
                System.IO.File.Copy(OASystem.Properties.Settings.Default.Temp優先移動リストFilePath, localDownload優先移動リストパス, true);

                MessageBox.Show("優先移動リストを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生した為、アップロードを中止しました。\r\nStackTrace:" + ex.StackTrace + "\r\nMessage:" + ex.Message, "エラー");
                return;
            }
        }

        private void 表示リストからUpload_保護リスト()
        {
            try
            {
                // TempFileをつくる
                TempFilesManager.FolderCheck();

                using (StreamWriter sw = new StreamWriter(OASystem.Properties.Settings.Default.Temp保護リストFilePath, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("レセプト電算コード,医薬品名称");
                    sw.WriteLine(OASystem.Model.DI.自店舗名);


                    foreach (ProtectEntity item in lvProtectList.Items)
                    {
                        sw.WriteLine("{0},{1}", item.レセプト電算コード, item.医薬品名);
                    }


          

                    sw.Flush();
                }

                UploadCenter.Upload保護リスト(OASystem.Properties.Settings.Default.Temp保護リストFilePath,OASystem.Model.DI.自店舗名);

                // ローカルも更新
                string localDownload保護リストパス = System.IO.Path.Combine(OASystem.Properties.Settings.Default.Download保護リストFolderPath, OASystem.Model.DI.自店舗名 + ".csv");
                System.IO.File.Copy(OASystem.Properties.Settings.Default.Temp保護リストFilePath, localDownload保護リストパス, true);

                MessageBox.Show("保護リストを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生した為、アップロードを中止しました。\r\nStackTrace:" + ex.StackTrace + "\r\nMessage:" + ex.Message, "エラー");
                return;
            }


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



        private void bdProtectUpdateButtonListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            bdProtectUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;
        }


        private void bdProtectUpdateButtonListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            // すでに表示されていたら引き続き印刷ボタンから離れても表示させる
            if (bdProtectUpdateButtonListBox.Visibility == System.Windows.Visibility.Visible)
            {
                bdProtectUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void lbProtectUpdate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bdProtectUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;

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
                表示リストからUpload_保護リスト();
            }
            else if (lbi.Content.ToString() == "ファイルからUP")
            {
                ファイルからUpload_保護リスト();
            }

            lb.SelectedIndex = -1;
        }





        private void lbPriorityMoveUpdate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bdPriorityMoveUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;

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
                表示リストからUpload_優先移動リスト();
            }
            else if (lbi.Content.ToString() == "ファイルからUP")
            {
                ファイルからUpload_優先移動リスト();
            }

            lb.SelectedIndex = -1;

        }







        private void bdPriorityMoveUpdateButtonListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            // すでに表示されていたら引き続き印刷ボタンから離れても表示させる
            if (bdPriorityMoveUpdateButtonListBox.Visibility == System.Windows.Visibility.Visible)
            {
                bdPriorityMoveUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void bdPriorityMoveUpdateButtonListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            bdPriorityMoveUpdateButtonListBox.Visibility = System.Windows.Visibility.Hidden;

        }




        private void tcOrderMedicineManagement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnProtectDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvProtectList.SelectedIndex == -1)
            {
                MessageBox.Show("削除する項目が選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var val = lvProtectList.SelectedValue as ProtectEntity;

            _ProtectListSnapShot.Remove(val);
            lvProtectList.ItemsSource = null;
            lvProtectList.ItemsSource = _ProtectListSnapShot;
        }

        private void btnProtectAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> _すでに追加されているレセプト電算コード = new List<string>();
            foreach (var row in _ProtectListSnapShot)
            {
                _すでに追加されているレセプト電算コード.Add(row.レセプト電算コード);
            }

            OrderMedicineManagementProtectAdd ommpa = new OrderMedicineManagementProtectAdd();
            ommpa.PriorityMoveListSnapShot = _PriorityMoveListSnapShot;
            ommpa.すでに追加されているレセプト電算コード = _すでに追加されているレセプト電算コード;
            ommpa.ShowDialog();

            if (ommpa.AddFlag)
            {
                var pe = ommpa.lvMedicineInfo.SelectedValue as ProtectEntity;
                _ProtectListSnapShot.Add(pe);

                lvProtectList.ItemsSource = null;
                lvProtectList.ItemsSource = _ProtectListSnapShot;
                lvProtectList.ScrollIntoView(_ProtectListSnapShot.Last());
            }
            else
            {
                return;
            }
        }

        private void btnProtectUpload_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("保護リストをアップロードします。\r\n\r\n宜しいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                表示リストからUpload_保護リスト();
            }

            //bdProtectUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;
        }



        private void btnPriorityMoveDelete_Click(object sender, RoutedEventArgs e)
        {

            if (lvPriorityMoveList.SelectedIndex == -1)
            {
                MessageBox.Show("削除する項目が選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var val = lvPriorityMoveList.SelectedValue as PriorityMoveEntity;

            _PriorityMoveListSnapShot.Remove(val);
            lvPriorityMoveList.ItemsSource = null;
            lvPriorityMoveList.ItemsSource = _PriorityMoveListSnapShot;

        }

        private void btnPriorityMoveAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> _すでに追加されているレセプト電算コード = new List<string>();
            foreach (var row in _PriorityMoveListSnapShot)
            {
                _すでに追加されているレセプト電算コード.Add(row.レセプト電算コード);
            }

            OrderMedicineManagementPriorityMoveAdd ommpma = new OrderMedicineManagementPriorityMoveAdd();
            ommpma.すでに追加されているレセプト電算コード = _すでに追加されているレセプト電算コード;
            ommpma.ProtectListSnapShot = _ProtectListSnapShot;
            ommpma.ShowDialog();

            if (ommpma.AddFlag)
            {
                var pe = ommpma.lvMedicineInfo.SelectedValue as PriorityMoveEntity;
                _PriorityMoveListSnapShot.Add(pe);

                lvPriorityMoveList.ItemsSource = null;
                lvPriorityMoveList.ItemsSource = _PriorityMoveListSnapShot;
                lvPriorityMoveList.ScrollIntoView(_PriorityMoveListSnapShot.Last());
            }
            else
            {
                return;
            }

        }

        private void btnPriorityMoveUpload_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("優先移動リストをアップロードします。\r\n\r\n宜しいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                表示リストからUpload_優先移動リスト();
            }

            //bdPriorityMoveUpdateButtonListBox.Visibility = System.Windows.Visibility.Visible;

        }

        private void btnProtectCSV_Click(object sender, RoutedEventArgs e)
        {
            if (lvProtectList.Items == null || lvProtectList.Items != null && lvProtectList.Items.Count == 0)
            {
                MessageBox.Show("保護リストに登録がありません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "保護リスト"; // Default file name
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
                    sw.WriteLine("レセプト電算コード,医薬品名");

                    foreach (ProtectEntity item in lvProtectList.Items)
                    {
                        sw.WriteLine("{0},{1}",  item.レセプト電算コード,item.医薬品名);
                    }

                    sw.Flush();
                }

                MessageBox.Show(string.Format("{0}に出力しました。", filename), "確認", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnPriorityMoveCSV_Click(object sender, RoutedEventArgs e)
        {

            if (lvPriorityMoveList.Items == null || lvPriorityMoveList.Items != null && lvPriorityMoveList.Items.Count == 0)
            {
                MessageBox.Show("優先移動リストに登録がありません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "優先移動リスト"; // Default file name
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
                    sw.WriteLine("レセプト電算コード,医薬品名,コメント");

                    foreach (PriorityMoveEntity item in lvPriorityMoveList.Items)
                    {
                        sw.WriteLine("{0},{1},{2}", item.レセプト電算コード, item.医薬品名,item.コメント);
                    }

                    sw.Flush();
                }

                MessageBox.Show(string.Format("{0}に出力しました。", filename), "確認", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }










    }
}
