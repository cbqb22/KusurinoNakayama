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
using System.Net;
using OASystem.Properties;
using OASystem.Model.Entity;
using OASystem.Model.Enum;
using OASystem.ViewModel.File;


namespace OASystem.View.Windows
{
    /// <summary>
    /// MasterDataManagement.xaml の相互作用ロジック
    /// </summary>
    public partial class MasterDataManagement : Window
    {

        //private List<BalancingAccountsEntity> _SourceBAList;
        private List<BalancingAccountsEntity> _SnapShotBAList;
        private List<IndividualBasedManagementMedicineEntity> _SnapShotIBMMList;

        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }


        public MasterDataManagement()
        {
            InitializeComponent();
            this.Closed += new EventHandler(MasterDataManagement_Closed);
            this.Loaded += new RoutedEventHandler(MasterDataManagement_Loaded);
        }

        void MasterDataManagement_Loaded(object sender, RoutedEventArgs e)
        {
            // 帳合先マスタ読み込み
            List<BalancingAccountsEntity> list = new List<BalancingAccountsEntity>();
            using (StreamReader sr = new StreamReader(OASystem.Properties.Settings.Default.Download帳合先マスタFilePath, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                string line = "";
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
                        continue;
                    }

                    BalancingAccountsEntity ent = new BalancingAccountsEntity();
                    ent.卸ＶＡＮコード = sepa[0];
                    ent.帳合先名称 = sepa[1];

                    int result;
                    if (int.TryParse(sepa[2], out result) == false)
                    {
                        continue;
                    }
                    ent.表示順 = result;

                    list.Add(ent);
                }
            }

            //_SourceBAList = list;
            _SnapShotBAList = list;
            lvBalancingAccountsMaster.ItemsSource = _SnapShotBAList;


            // 個別管理医薬品マスタ読み込み
            List<IndividualBasedManagementMedicineEntity> ibmmlist = OASystem.Model.DI.個別管理医薬品マスタ;

            _SnapShotIBMMList= new List<IndividualBasedManagementMedicineEntity>(ibmmlist);
            lvIndividualBasedManagementMedicine.ItemsSource = _SnapShotIBMMList;

        }

        void MasterDataManagement_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }


        private void btnBalancingAccountsMasterDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvBalancingAccountsMaster.SelectedIndex == -1)
            {
                MessageBox.Show("削除する項目が選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (MessageBox.Show("登録済の帳合先マスタを削除するとプログラムが正しく動作しなくなる可能性があります。\r\n十分に確認の上、削除を行って下さい。\r\n\r\n選択された項目を削除しますか？", "注意", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                _SnapShotBAList.RemoveAt(lvBalancingAccountsMaster.SelectedIndex);
                lvBalancingAccountsMaster.ItemsSource = null;
                lvBalancingAccountsMaster.ItemsSource = _SnapShotBAList;
            }



        }

        private void btnBalancingAccountsMasterAdd_Click(object sender, RoutedEventArgs e)
        {
            BalancingAccountsAdd baa = new BalancingAccountsAdd();
            baa.ShowDialog();

            if (baa.AddFlag)
            {
                BalancingAccountsEntity ent = new BalancingAccountsEntity();
                ent.卸ＶＡＮコード = baa.tbＶＡＮコード.Text;
                ent.帳合先名称 = baa.tb帳合先名称.Text;
                ent.表示順 = _SnapShotBAList.Count + 1;
                _SnapShotBAList.Add(ent);
            }


            lvBalancingAccountsMaster.ItemsSource = null;
            lvBalancingAccountsMaster.ItemsSource = _SnapShotBAList;

        }

        private void btnBalancingAccountsMasterUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // TempFileをつくる
                TempFilesManager.FolderCheck();

                if (lvBalancingAccountsMaster.Items.Count == 0)
                {
                    if (MessageBox.Show("項目が１つもありません。\r\n追加ボタンで追加してください。\r\n\r\nこのままリスト無しでアップロードする場合は、\r\n[はい]を選択してください。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                using (StreamWriter sw = new StreamWriter(OASystem.Properties.Settings.Default.TempBlancingAccountsFilePath, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("卸VANコード,帳合先名称,表示順");

                    foreach (var item in _SnapShotBAList)
                    {
                        sw.WriteLine("{0},{1},{2}", item.卸ＶＡＮコード, item.帳合先名称, item.表示順);
                    }
                    sw.Flush();
                }


                string uri = OASystem.Properties.Settings.Default.UploadPath帳合先マスタCSV;
                string myFile = OASystem.Properties.Settings.Default.TempBlancingAccountsFilePath;

                WebRequest req = WebRequest.Create(uri);
                NetworkCredential nc = new NetworkCredential("a10254880", "hxzn9jXQ");
                req.Credentials = nc;
                req.Method = WebRequestMethods.Ftp.UploadFile;

                using (Stream st = req.GetRequestStream())
                using (FileStream fs = new FileStream(myFile, FileMode.Open))
                {
                    Byte[] buf = new Byte[1024];
                    int count = 0;

                    do
                    {
                        count = fs.Read(buf, 0, buf.Length);
                        st.Write(buf, 0, count);
                    } while (count != 0);
                }

                // ローカルも更新
                System.IO.File.Copy(OASystem.Properties.Settings.Default.TempBlancingAccountsFilePath,OASystem.Properties.Settings.Default.Download帳合先マスタFilePath,true);
                // DI更新
                Model.DI.帳合先マスタ = lvBalancingAccountsMaster.ItemsSource as List<BalancingAccountsEntity>;

                MessageBox.Show("帳合先マスタデータを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生した為、アップロードを中止しました。\r\nStackTrace:" + ex.StackTrace + "\r\nMessage:"+ ex.Message,"エラー");
                return;
            }

        }


        private void btnIndividualBasedManagementMedicineDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvIndividualBasedManagementMedicine.SelectedIndex == -1)
            {
                MessageBox.Show("削除する項目が選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            _SnapShotIBMMList.RemoveAt(lvIndividualBasedManagementMedicine.SelectedIndex);
            lvIndividualBasedManagementMedicine.ItemsSource = null;
            lvIndividualBasedManagementMedicine.ItemsSource = _SnapShotIBMMList;


        }

        private void btnIndividualBasedManagementMedicineAdd_Click(object sender, RoutedEventArgs e)
        {
            IndividualBasedMedicineAdd ibma = new IndividualBasedMedicineAdd();
            ibma.ShowDialog();

            if (ibma.AddFlag)
            {
                IndividualBasedManagementMedicineEntity ent = new IndividualBasedManagementMedicineEntity();
                ent.JANコード = ibma.tbＪＡＮコード.Text;
                ent.医薬品名 = ibma.tb医薬品名.Text;
                ent.包装形態 = ibma.tb包装形態.Text;
                ent.包装単位 = ibma.tb包装単位.Text;
                ent.包装単位数 = ibma.tb包装単位数.Text;
                ent.包装総量 = ibma.tb包装総量.Text;
                ent.剤形区分 = (剤形区分Enum)ibma.cmb剤形区分.SelectedIndex;
                ent.製薬会社 = ibma.tb製薬会社.Text;
                ent.販売会社 = ibma.tb販売会社.Text;
                _SnapShotIBMMList.Add(ent);

                
            }

            lvIndividualBasedManagementMedicine.ItemsSource = null;
            lvIndividualBasedManagementMedicine.ItemsSource = _SnapShotIBMMList;

        }

        private void btnIndividualBasedManagementMedicineUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // TempFileをつくる
                TempFilesManager.FolderCheck();

                if (lvIndividualBasedManagementMedicine.Items.Count == 0)
                {
                    if (MessageBox.Show("項目が１つもありません。\r\n追加ボタンで追加してください。\r\n\r\nこのままリスト無しでアップロードする場合は、\r\n[はい]を選択してください。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                using (StreamWriter sw = new StreamWriter(OASystem.Properties.Settings.Default.TempIndividualBasedMedicineFilePath, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("JANコード,医薬品名称,包装形態,包装単位,包装単位数,包装総量,剤形区分,製薬会社,販売会社");

                    foreach (var item in _SnapShotIBMMList)
                    {
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", item.JANコード, item.医薬品名, item.包装形態,item.包装単位,item.包装単位数,item.包装総量,(int)item.剤形区分,item.製薬会社,item.販売会社);
                    }
                    sw.Flush();
                }


                string uri = OASystem.Properties.Settings.Default.UploadPath個別管理医薬品マスタCSV;
                string myFile = OASystem.Properties.Settings.Default.TempIndividualBasedMedicineFilePath;

                WebRequest req = WebRequest.Create(uri);
                NetworkCredential nc = new NetworkCredential("a10254880", "hxzn9jXQ");
                req.Credentials = nc;
                req.Method = WebRequestMethods.Ftp.UploadFile;

                using (Stream st = req.GetRequestStream())
                using (FileStream fs = new FileStream(myFile, FileMode.Open))
                {
                    Byte[] buf = new Byte[1024];
                    int count = 0;

                    do
                    {
                        count = fs.Read(buf, 0, buf.Length);
                        st.Write(buf, 0, count);
                    } while (count != 0);
                }

                // ローカルも更新
                System.IO.File.Copy(OASystem.Properties.Settings.Default.TempIndividualBasedMedicineFilePath, OASystem.Properties.Settings.Default.Download個別管理医薬品マスタFilePath, true);
                // DI更新
                Model.DI.個別管理医薬品マスタ = lvIndividualBasedManagementMedicine.ItemsSource as List<IndividualBasedManagementMedicineEntity>;
                DownloadCenter.Setメーカー名リスト(); // メーカー名が追加されているかもしれないので再セット

                MessageBox.Show("個別管理医薬品マスタデータを更新しました。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

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

    }
}
