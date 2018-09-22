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
    /// DownloadingProgress.xaml の相互作用ロジック
    /// </summary>
    public partial class DownloadingProgress : UserControl
    {
        public DownloadingProgress()
        {
            InitializeComponent();
        }

        public void SetInit(string tbLowerText)
        {
            //tbLower.Text = tbLowerText;
            //tbUpper.Text = "0";
        }

        

        private void pbDownloading_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // プログレスバーの現在値を更新する
            pbDownloading.Value = e.NewValue;
            // 進捗率を表示する
            tbProgressPercentage.Text = pbDownloading.Value.ToString();
            //tbUpper.Text = pbDownloading.Value.ToString();
        }
    }
}
