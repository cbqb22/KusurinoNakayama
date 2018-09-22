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
    /// エラーリスト表示２.xaml の相互作用ロジック
    /// </summary>
    public partial class エラーリスト表示２ : Window
    {
        public エラーリスト表示２()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(エラーリスト表示２_Loaded);
            lv移動伝票明細書エラー.LayoutUpdated += new EventHandler(lv移動伝票明細書エラー_LayoutUpdated);
        }

        void lv移動伝票明細書エラー_LayoutUpdated(object sender, EventArgs e)
        {
            if (this.IsLoaded == true)
            {
                FixWidth();
            }
        }

        private void FixWidth()
        {
            var lv = lv移動伝票明細書エラー;
            if (lv != null)
            {
                var gv = lv.View as GridView;
                if (gv != null)
                {
                    double total払width = 0; // 店間振替払は0～6
                    for (int i = 0; i < 7; i++)
                    {
                        total払width += gv.Columns[i].ActualWidth;
                    }

                    double エラー事由width = gv.Columns[7].ActualWidth; // エラー事由は7

                    double total受width = 0; // 店間振替受けは8～15
                    for (int i = 8; i < 16; i++)
                    {
                        total受width += gv.Columns[i].ActualWidth;
                    }

                    double total = total受width + total払width + エラー事由width;

                    // Windowの方が大きい場合
                    if (total < this.ActualWidth)
                    {
                        gdListTitle払.Width = total払width;
                        gdListTitleエラー事由.Width = エラー事由width;
                        gdListTitle受.Width = total受width;

                        gdListTitle払.UpdateLayout();
                        gdListTitle受.UpdateLayout();
                        gdListTitleエラー事由.UpdateLayout();
                    }
                    else
                    {

                        gdListTitle払.Width = total払width;
                        gdListTitleエラー事由.Width = エラー事由width;
                        gdListTitle受.Width = total受width;
                        gdListTitle払.UpdateLayout();
                        gdListTitle受.UpdateLayout();
                        gdListTitleエラー事由.UpdateLayout();
                    }

                }
            }

        }


        void エラーリスト表示２_Loaded(object sender, RoutedEventArgs e)
        {
            FixWidth();
        }


        private void btnCSV_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // エラーリスト出力
                string savefilepath = System.IO.Path.Combine(fbd.SelectedPath, "エラーリスト出力.csv");
                using (StreamWriter sw = new StreamWriter(savefilepath, false, Encoding.GetEncoding(932)))
                {

                    // "伝票No","伝票日付" ,"発行店舗名","相手先","薬品コード","薬品名","数量" ,"単価"  ,"金額" ,"エラー事由","伝票No","相手伝票No","伝票日付","入力店舗名 ","相手先","薬品コード","薬品名","数量" ,"単価" ,"金額"
                    string cammma = ",";
                    sw.WriteLine("伝票No" + cammma + "伝票日付" + cammma + "発行店舗名" + cammma + "相手先" + cammma + "薬品名" + cammma + "薬品コード" + cammma + "数量" + cammma + "単価" + cammma + "金額" + cammma + "エラー事由" + cammma + "伝票No" + cammma + "相手伝票No" + cammma + "伝票日付" + cammma + "入力店舗名 " + cammma + "相手先" + cammma + "薬品名" + cammma + "薬品コード" + cammma + "数量" + cammma + "単価" + cammma + "金額");
                    var col = lv移動伝票明細書エラー.ItemsSource as List<エラーリスト表示２用Entity>;
                    string writedata = "";
                    foreach (var data in col)
                    {
                        writedata = data.払データ.伝票No + cammma + data.払データ.伝票日付 + cammma + data.払データ.明細書店舗名 + cammma + data.払データ.相手先 + cammma + data.払データ.薬品コード + cammma + data.払データ.薬品名 + cammma + data.払データ.数量 + cammma + data.払データ.単価 + cammma + data.払データ.金額 + cammma + data.エラー事由 + cammma + data.受データ.伝票No + cammma + data.受データ.相手伝票No + cammma + data.受データ.伝票日付 + cammma + data.受データ.明細書店舗名 + cammma + data.受データ.相手先 + cammma + data.受データ.薬品コード + cammma + data.受データ.薬品名 + cammma + data.受データ.数量 + cammma + data.受データ.単価 + cammma + data.受データ.金額;
                        sw.WriteLine(writedata);
                    }
                }
            }
        }
    }


    public class ListViewStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var d = item as エラーリスト表示２用Entity;

            if (d.エラー事由 == "受伝票が存在しません。" || d.エラー事由 == "払伝票が存在しません。")
            {
            }
            else if (d.エラー事由 == "")
            {
            }

            return null;
            //if (d % 2 != 0)
            //{
            //    var f = (FrameworkElement)container;
            //    return (Style)f.FindResource("odd");
            //}
            //else
            //{
            //    return null;
            //}
        }
    }
}
