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

namespace MCSystem.View.Windows
{
    /// <summary>
    /// Cancel.xaml の相互作用ロジック
    /// </summary>
    public partial class Cancel : Window
    {
        private bool _PushedCancel;
        public bool PushedCancel
        {
            get { return _PushedCancel; }
            set { _PushedCancel = value; }
        }

        public Cancel()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Cancel_Loaded);
            this.Closed += new EventHandler(Cancel_Closed);
        }

        void Cancel_Closed(object sender, EventArgs e)
        {
        }

        void Cancel_Loaded(object sender, RoutedEventArgs e)
        {
            var rect = System.Windows.SystemParameters.WorkArea;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

            var left = rect.Width - this.Width;
            var top = rect.Height - this.Height;
            this.Top = 0;
            this.Left = left;

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.PushedCancel = true;
            this.Close();
        }
    }
}
