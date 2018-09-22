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

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表
{
    /// <summary>
    /// IndividualShiftFrame.xaml の相互作用ロジック
    /// </summary>
    public partial class IndividualShiftFrame : UserControl,INotifyPropertyChanged
    {
        public IndividualShiftFrame()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private int _GZindex = 3;

        public int GZindex
        {
            get { return _GZindex; }
            set 
            {
                _GZindex = value;
                this.FirePropertyChanged("GZindex");
            }
        }

        #region INotifyPropertyChanged メンバ

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
