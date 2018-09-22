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
using StockManagement.Const;
using StockManagement.ViewModel.Common.MessageBox;

namespace StockManagement.View
{
    /// <summary>
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }


        private void txtNum_LostFocus(object sender, RoutedEventArgs e)
        {

            TextBlock tb = sender as TextBlock;
            if (tb == null)
            {
                return;
            }

            DateTime result;
            if (DateTime.TryParse(tb.Text, out result) == false)
            {
                MessageBoxTop.Show("日付を入力して下さい。");
                //tb.Focus();
                //tb.SelectAll();
                return;
            }

            if (result < SMConst.deadStockMinDate)
            {
                MessageBoxTop.Show("使用期間を2012年1月より以前を指定できません。");
                return;
            }


        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            DateTime result;
            if (DateTime.TryParse(tbNum.Text, out result) == false)
            {
                MessageBoxTop.Show("日付を入力して下さい。");
                return;
            }


            tbNum.Text = result.AddMonths(1).ToString("yyyy/MM");
            
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            DateTime result;
            if (DateTime.TryParse(tbNum.Text, out result) == false)
            {
                MessageBoxTop.Show("日付を入力して下さい。");
                return;
            }


            tbNum.Text = result.AddMonths(-1).ToString("yyyy/MM");

        }

    }
}
