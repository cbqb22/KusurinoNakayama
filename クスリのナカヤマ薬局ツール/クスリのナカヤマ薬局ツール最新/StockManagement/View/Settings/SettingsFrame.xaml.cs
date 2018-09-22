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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using StockManagement.Settings;
using StockManagement.ViewModel;
using StockManagement.ViewModel.Common.MessageBox;

namespace StockManagement.View.Settings
{
    /// <summary>
    /// SettingsFrame.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingsFrame : UserControl
    {
        private bool IsSaved = false;

        private List<ExceptionMedicineEntity> _ExceptiveMedicinesListSnapshot = new List<ExceptionMedicineEntity>();

        public SettingsFrame()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(SettingsFrame_Loaded);
        }

        void SettingsFrame_Loaded(object sender, RoutedEventArgs e)
        {

            SetInit();
        }

        private void SetInit()
        {
            this.tbデッド品管理自店舗.Text = InitialData.DeadMangementSourceStore;

            spデッド品管理対象店舗.Children.Clear();
            foreach (var data in InitialData.DeadStockManagementStoresList)
            {
                StoreSettingsUserControl uc = new StoreSettingsUserControl();
                uc.tb店舗名.Text = data;

                spデッド品管理対象店舗.Children.Add(uc);
            }


            sp全店舗リスト.Children.Clear();
            foreach (var data in InitialData.AllShopList)
            {
                StoreSettingsUserControl uc = new StoreSettingsUserControl();
                uc.tb店舗名.Text = data;

                sp全店舗リスト.Children.Add(uc);
            }

            cmbUsedAmountDateRange.SelectedIndex = InitialData.UsedAmountDateRange -1; //Index分-1 
            cmbOutputType.SelectedIndex = (int)InitialData.OutputExcelType;
            cmbExpireRange.SelectedIndex = InitialData.ExpireRange - 1; //Index分-1


            // 除外医薬品リストのスナップショットを設定
            Set除外医薬品SnapShotListFromInitialData();

        }

        private void Set除外医薬品SnapShotListFromInitialData()
        {
            _ExceptiveMedicinesListSnapshot = new List<ExceptionMedicineEntity>();

            if (InitialData.ExceptiveMedicinesList == null)
            {
                InitialData.Set除外医薬品List();
            }

            foreach(var list in InitialData.ExceptiveMedicinesList)
            {
                ExceptionMedicineEntity ent = new ExceptionMedicineEntity();
                ent.医薬品名称 = list.医薬品名称;
                ent.レセプト電算コード = list.レセプト電算コード;

                _ExceptiveMedicinesListSnapshot.Add(ent);
            }
        }


        private void btnデッド品管理対象店舗追加_Click(object sender, RoutedEventArgs e)
        {
            StoreSettingsUserControl uc = new StoreSettingsUserControl();

            spデッド品管理対象店舗.Children.Add(uc);

            svデッド品管理対象店舗.ScrollToEnd();


        }

        private void btn全店舗リスト追加_Click(object sender, RoutedEventArgs e)
        {
            StoreSettingsUserControl uc = new StoreSettingsUserControl();

            sp全店舗リスト.Children.Add(uc);

            sv全店舗リスト.ScrollToEnd();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Save())
            {
                MessageBoxTop.Show("設定を保存しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private bool Save()
        {
            if (string.IsNullOrEmpty(tbデッド品管理自店舗.Text))
            {
                MessageBoxTop.Show("デッド品管理自店舗を入力してください。","入力エラー",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }

            if (cmbUsedAmountDateRange.SelectedIndex == -1)
            {
                MessageBoxTop.Show("使用量期間を選択してください。", "選択エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }



            if (cmbOutputType.SelectedIndex == -1)
            {
                MessageBoxTop.Show("出力形式を選択してください。", "選択エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cmbExpireRange.SelectedIndex == -1)
            {
                MessageBoxTop.Show("期限切迫期間を選択してください。", "選択エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            // Settings.iniへ保存

            List<string> 管理対象店舗List = new List<string>();
            foreach (var child in spデッド品管理対象店舗.Children)
            {
                if (child is StoreSettingsUserControl)
                {
                    var ssuc = child as StoreSettingsUserControl;
                    if (string.IsNullOrEmpty(ssuc.tb店舗名.Text) == false)
                    {
                        管理対象店舗List.Add(ssuc.tb店舗名.Text);
                    }
                }
                else
                {
                    throw new Exception("エラーが発生しました。\r\nデッド品管理対象店舗に不正な子要素を含んでいる為、プログラムを中断しました。");
                }
            }

            List<string> 全店舗リストList = new List<string>();
            foreach (var child in sp全店舗リスト.Children)
            {
                if (child is StoreSettingsUserControl)
                {
                    var ssuc = child as StoreSettingsUserControl;
                    if (string.IsNullOrEmpty(ssuc.tb店舗名.Text) == false)
                    {
                        全店舗リストList.Add(ssuc.tb店舗名.Text);
                    }
                }
                else
                {
                    throw new Exception("エラーが発生しました。\r\n全店舗リストに不正な子要素を含んでいる為、プログラムを中断しました。");
                }
            }


            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string settingsiniFilePath = System.IO.Path.Combine(desktopPath, @"デッド品管理\Settings.ini");

            using (StreamWriter sr = new StreamWriter(settingsiniFilePath, false, Encoding.GetEncoding(932)))
            {

                sr.WriteLine(string.Format("[デッド品管理自店舗]={0}", tbデッド品管理自店舗.Text));

                string str = "";
                管理対象店舗List.ForEach
                (
                    delegate(string x)
                    {
                        if (str == "")
                        {
                            str = x;
                        }
                        else
                        {
                            str += "," + x;
                        }
                    }
                );
                sr.WriteLine(string.Format("[デッド品管理対象店舗]={0}", str));


                string str2 = "";
                全店舗リストList.ForEach
                (
                    delegate(string x)
                    {
                        if (str2 == "")
                        {
                            str2 = x;
                        }
                        else
                        {
                            str2 += "," + x;
                        }
                    }
                );

                sr.WriteLine(string.Format("[全店舗リスト]={0}", str2));

                sr.WriteLine(string.Format("[使用量期間]={0}", (cmbUsedAmountDateRange.SelectedIndex + 1).ToString()));

                sr.WriteLine(string.Format("[出力形式]={0}", cmbOutputType.SelectedIndex.ToString()));

                sr.WriteLine(string.Format("[期限切迫期間]={0}", (cmbExpireRange.SelectedIndex + 1).ToString()));
            }

            IsSaved = true;


            using (StreamWriter sw = new StreamWriter(StockManagement.Const.SMConst.exceptiveMedicinesFile, false, Encoding.GetEncoding(932)))
            {
                sw.WriteLine("医薬品名称,レセプト電算コード");
                foreach (var list in _ExceptiveMedicinesListSnapshot)
                {
                    sw.WriteLine(string.Format("{0},{1}",list.医薬品名称,list.レセプト電算コード));
                }
            }

            // ConstのSettingsへも反映
            InitialData.LoadInitData();

            return true;

        }
            

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSaved)
            {
                if (MessageBoxTop.Show("保存しますか？","保存確認",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    // 保存処理

                    if (!Save())
                    {
                        return;
                    }
                    else
                    {
                        MessageBoxTop.Show("設定を保存しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }


            var fe =  VisualTreeHelper.GetParent(this) as FrameworkElement;


            while (fe != null)
            {
                if (fe is SettingsWindow )
                {
                    break;
                    
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
            }

            if (fe == null)
            {
                throw new Exception("エラーが発生しました。設定保存が適切に開かれていないためプログラムを中断します。");
            }
            else
            {
                var window = fe as SettingsWindow;
                window.Close();
            }


        }

        private void btnExceptMedicines_Click(object sender, RoutedEventArgs e)
        {
            ExceptiveMedicinesList eml = new ExceptiveMedicinesList();
            eml.LvItemssource = _ExceptiveMedicinesListSnapshot;
            eml.ShowDialog();



        }

    }
}
