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

namespace StockManagement.View.Settings
{
    /// <summary>
    /// StoreSettingsUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class StoreSettingsUserControl : UserControl
    {
        public StoreSettingsUserControl()
        {
            InitializeComponent();
        }

        private void btn削除_Click(object sender, RoutedEventArgs e)
        {

            var fe =  VisualTreeHelper.GetParent(this) as FrameworkElement;

            while (fe != null)
            {
                if (fe is StackPanel)
                {
                    var sp = fe as StackPanel;
                    sp.Children.Remove(this);
                    break;
                }

                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;

            }

        }
    }
}
