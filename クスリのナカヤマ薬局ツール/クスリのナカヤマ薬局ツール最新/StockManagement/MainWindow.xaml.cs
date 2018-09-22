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
using System.ComponentModel;
using StockManagement.ViewModel;
using StockManagement.ViewModel.Excel;

namespace StockManagement
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += new CancelEventHandler(MainWindow_Closing);
            this.Closed += new EventHandler(MainWindow_Closed);
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            ucDeadStockFrame.CloseAllProgressBar();
        }
    }
}
