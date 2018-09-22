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

namespace MediWebOrderWatcher.View
{
    /// <summary>
    /// 発注書作成画面Window.xaml の相互作用ロジック
    /// </summary>
    public partial class 発注書作成画面Window : Window
    {
        public 発注書作成画面Window()
        {
            InitializeComponent();
        }

        private void btnMinimized_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
