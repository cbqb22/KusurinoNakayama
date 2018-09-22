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

namespace StockManagement.View.ProgressBar
{
    /// <summary>
    /// CompletedCountProgress.xaml の相互作用ロジック
    /// </summary>
    public partial class CompletedCountProgress : UserControl
    {
        public CompletedCountProgress()
        {
            InitializeComponent();
        }

        public void SetInit(string tbLowerText)
        {
            //tbLower.Text = tbLowerText;
            tbUpper.Text = "0";
        }



        private void pbCompletedCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // プログレスバーの現在値を更新する
            //pbCompletedCount.Value = e.NewValue;

            // 進捗率を表示する
            tbUpper.Text = pbCompletedCount.Value.ToString();
        }

    }
}
