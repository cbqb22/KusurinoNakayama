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

namespace StockManagement.View.ProgressBar
{
    /// <summary>
    /// ProgressBarWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ProgressBarWindow : Window
    {
        public ProgressBarWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(ProgressBarWindow_Loaded);


        }

        void ProgressBarWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dp.SetInit("100");
        }
    }
}
