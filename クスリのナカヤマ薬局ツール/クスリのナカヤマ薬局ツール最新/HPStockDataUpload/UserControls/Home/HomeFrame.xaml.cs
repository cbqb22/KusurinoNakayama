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
using クスリのナカヤマ薬局ツール.Windows;

namespace クスリのナカヤマ薬局ツール.UserControls.Home
{
    /// <summary>
    /// HomeFrame.xaml の相互作用ロジック
    /// </summary>
    public partial class HomeFrame : UserControl
    {
        public HomeFrame()
        {
            InitializeComponent();
        }

        private void btnシフト表作成_Click(object sender, RoutedEventArgs e)
        {
            ShiftViewer sv = new ShiftViewer();
            sv.Show();

        }

        private void btn人件費計算_Click(object sender, RoutedEventArgs e)
        {
            CostCalcViewer ccv = new CostCalcViewer();
            ccv.Show();

        }

        private void btn印刷_Click(object sender, RoutedEventArgs e)
        {

        }

        private void 設定_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn作業割り当て表_Click(object sender, RoutedEventArgs e)
        {
            AssignmentTableViewer atv = new AssignmentTableViewer();
            atv.Show();

        }
    }
}
