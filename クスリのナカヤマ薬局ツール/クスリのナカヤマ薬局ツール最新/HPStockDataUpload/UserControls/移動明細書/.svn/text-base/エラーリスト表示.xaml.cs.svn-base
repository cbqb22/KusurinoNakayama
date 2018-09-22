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
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace クスリのナカヤマ薬局ツール.UserControls.移動明細書
{
    /// <summary>
    /// エラーリスト表示.xaml の相互作用ロジック
    /// </summary>
    public partial class エラーリスト表示 : Window
    {
        public エラーリスト表示()
        {
            InitializeComponent();

            window.LayoutUpdated += new EventHandler(window_LayoutUpdated);
        }


        // Listviewに幅調整
        void window_LayoutUpdated(object sender, EventArgs e)
        {
            var halfAW = window.ActualWidth / 2 - 30;
            var halfAH = window.ActualHeight / 2 - 30;

            this.エラーリスト受けあり払いなしListView.Width = halfAW;
            this.エラーリスト払いあり受けなしListView.Width = halfAW;
            this.エラーリスト払い受けあり入力間違いListView.Width = window.ActualWidth - 40;

        }

        private void btnCSV_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                

                // 払○受×
                string savefilepath = System.IO.Path.Combine(fbd.SelectedPath ,"払○受×.csv");
                using (StreamWriter sw = new StreamWriter(savefilepath, false, Encoding.GetEncoding(932)))
                {
                    // "区分","伝票日付","伝票No","行番号","相手先コード","相手先","薬品コード","薬品名","形態区分","数量","単価","金額","使用期限","ロットNo"

                    string cammma = ",";
                    sw.WriteLine("チェック済" + cammma + "移動明細書作成店舗" + cammma + "区分" + cammma + "伝票日付" + cammma + "伝票No" + cammma + "相手先" + cammma + "薬品コード" + cammma + "薬品名" + cammma + "数量" + cammma + "単価" + cammma + "金額");
                    var col = エラーリスト払いあり受けなしListView.ItemsSource as List<移動明細書項目データ>;
                    string writedata = "";
                    foreach (var data in col)
                    {
                        writedata = (data.チェック済 ? "済" : "") + cammma + data.明細書店舗名 + cammma + data.区分 + cammma + data.伝票日付 + cammma + data.伝票No + cammma + data.相手先 + cammma + data.薬品コード + cammma + data.薬品名 + cammma + data.数量 + cammma + data.単価 + cammma + data.金額;
                        sw.WriteLine(writedata);
                    }
                }


                // 払×受○
                string savefilepath2 = System.IO.Path.Combine(fbd.SelectedPath , "払×受○.csv");
                using (StreamWriter sw = new StreamWriter(savefilepath2, false, Encoding.GetEncoding(932)))
                {
                    // "区分","伝票日付","伝票No","行番号","相手先コード","相手先","薬品コード","薬品名","形態区分","数量","単価","金額","使用期限","ロットNo"

                    string cammma = ",";
                    var col = エラーリスト受けあり払いなしListView.ItemsSource as List<移動明細書項目データ>;
                    string writedata = "";
                    sw.WriteLine("チェック済" + cammma + "移動明細書作成店舗" + cammma + "区分" + cammma + "伝票日付" + cammma + "伝票No" + cammma + "相手先" + cammma + "薬品コード" + cammma + "薬品名" + cammma + "数量" + cammma + "単価" + cammma + "金額");
                    foreach (var data in col)
                    {
                        writedata = (data.チェック済 ? "済" : "") + cammma + data.明細書店舗名 + cammma + data.区分 + cammma + data.伝票日付 + cammma + data.伝票No + cammma + data.相手先 + cammma + data.薬品コード + cammma + data.薬品名 + cammma + data.数量 + cammma + data.単価 + cammma + data.金額;
                        sw.WriteLine(writedata);
                    }
                }


                // 払受○入力違い
                string savefilepath3 = System.IO.Path.Combine(fbd.SelectedPath, "払受○入力違い.csv");
                using (StreamWriter sw = new StreamWriter(savefilepath3, false, Encoding.GetEncoding(932)))
                {
                    // "区分","伝票日付","伝票No","行番号","相手先コード","相手先","薬品コード","薬品名","形態区分","数量","単価","金額","使用期限","ロットNo"

                    string cammma = ",";
                    sw.WriteLine("チェック済" + cammma + "移動明細書作成店舗" + cammma + "区分" + cammma + "伝票日付" + cammma + "伝票No" + cammma + "相手先" + cammma + "薬品コード" + cammma + "薬品名" + cammma + "数量" + cammma + "単価" + cammma + "金額");
                    var col = エラーリスト払い受けあり入力間違いListView.ItemsSource as List<移動明細書項目データ>;
                    string writedata = "";
                    foreach (var data in col)
                    {
                        writedata = (data.チェック済 ? "済" : "") + cammma + data.明細書店舗名 + cammma + data.区分 + cammma + data.伝票日付 + cammma + data.伝票No + cammma + data.相手先 + cammma + data.薬品コード + cammma + data.薬品名 + cammma + data.数量 + cammma + data.単価 + cammma + data.金額;
                        sw.WriteLine(writedata);
                    }
                }


            }


        }


    }
}
