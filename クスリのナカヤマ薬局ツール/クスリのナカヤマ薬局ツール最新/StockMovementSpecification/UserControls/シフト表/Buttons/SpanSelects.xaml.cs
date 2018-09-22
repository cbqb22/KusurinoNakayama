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

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表.Buttons
{
    /// <summary>
    /// SpanSelects.xaml の相互作用ロジック
    /// </summary>
    public partial class SpanSelects : UserControl, INotifyPropertyChanged
    {
        public SpanSelects()
        {
            InitializeComponent();

            this.DataContext = this;
            this.SelectStartDate = DateTime.Now;

        }

        public event PropertyChangedEventHandler PropertyChanged;


        private DateTime _SelectStartDate;

        public DateTime SelectStartDate
        {
            get { return _SelectStartDate; }
            set 
            {
                _SelectStartDate = value;
                OnPropertyChanged("SelectStartDate");
            }
        }


        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
