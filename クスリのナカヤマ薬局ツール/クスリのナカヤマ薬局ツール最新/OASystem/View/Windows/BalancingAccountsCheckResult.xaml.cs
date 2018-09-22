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
    public partial class BalancingAccountsCheckResult : Window
    {

        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        public BalancingAccountsCheckResult()
        {
            InitializeComponent();
            this.Closed += new EventHandler(BalancingAccountsCheckResult_Closed);
            this.Loaded += new RoutedEventHandler(BalancingAccountsCheckResult_Loaded);

        }

        void BalancingAccountsCheckResult_Loaded(object sender, RoutedEventArgs e)
        {
            tbl帳合チェック結果.Text = string.Format("帳合先チェック結果：{0} 件のエラーがありました。",lvBACheckResult.Items.Count);
        }

        void BalancingAccountsCheckResult_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }


        private void btnCSV_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "帳合先チェック結果"; // Default file name
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
                    sw.WriteLine("JANコード,レセプト電算コード,医薬品名,包装,メーカー名,今回帳合先,正しい帳合先,エラー内容");

                    foreach (BalancingAccountsCheckResultEntity item in lvBACheckResult.Items)
                    {
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", item.JANコード, item.レセプト電算コード, item.医薬品名, item.包装, item.販売会社, DataConvert.VANコードTo帳合先名称Convert(item.注文帳合先VANコード), DataConvert.VANコードTo帳合先名称Convert(item.設定帳合先VANコード), item.エラー内容.Replace("\r\n", ""));
                    }


                    //下のやり方だとListViewでまだスクロールされていない部分はVisualTreeができていない為、rowpresenterがnullになる
                    //for (int i = 0; i < lvBACheckResult.Items.Count; i++)
                    //{
                    //    ListViewItem listViewItem = (ListViewItem)lvBACheckResult.ItemContainerGenerator.ContainerFromIndex(i);
                    //    var rowpresenter = FindVisualChild<GridViewRowPresenter>(listViewItem);

                    //    var dc = listViewItem.DataContext as BalancingAccountsCheckResultEntity;
                    //    if (dc == null)
                    //    {
                    //        continue;
                    //    }


                    //    sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", dc.JANコード,dc.レセプト電算コード,dc.医薬品名, dc.包装,dc.販売会社,DataConvert.VANコードTo帳合先名称Convert(dc.注文帳合先VANコード),DataConvert.VANコードTo帳合先名称Convert(dc.設定帳合先VANコード),dc.エラー内容.Replace("\r\n",""));

                    //}

                    sw.Flush();
                }

            }


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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





  

 

    }
}
