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
using windos = クスリのナカヤマ薬局ツール.Windows;
using クスリのナカヤマ薬局ツール.共通.Printing;

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表
{
    /// <summary>
    /// ShiftTable.xaml の相互作用ロジック
    /// </summary>
    public partial class ShiftTable : UserControl
    {
        public ShiftTable()
        {
            InitializeComponent();
        }


        private InputShiftCell _CurrentFocus;

        public InputShiftCell CurrentFocus
        {
            get { return _CurrentFocus; }
            set { _CurrentFocus = value; }
        }

        private void btn休セット_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentFocus == null)
            {
                return;
            }

            if (CurrentFocus.IsHoliday)
            {
                CurrentFocus.IsHoliday = false;
            }
            else
            {
                CurrentFocus.IsHoliday = true;
            }



        }

        private void btn印刷_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Canvas cvShiftTable = this.cvShiftTable;
                this.gdRoot.Children.Remove(cvShiftTable);
                this.gdRoot.Children.Clear();

                PrintCenter pc = new PrintCenter(cvShiftTable);


                // 印刷UIを解放する
                if (pc != null)
                {
                    pc.RemoveFixedPageChild();
                }

                this.gdRoot.Children.Add(cvShiftTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("印刷時にエラーが発生：" + ex.Message + ex.StackTrace);
            }
        }
    }
}
