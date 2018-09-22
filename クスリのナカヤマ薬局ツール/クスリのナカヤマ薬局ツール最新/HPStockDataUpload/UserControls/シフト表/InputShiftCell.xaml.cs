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
using クスリのナカヤマ薬局ツール.UserControls.Calendaer;

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表
{
    /// <summary>
    /// InputShiftCell.xaml の相互作用ロジック
    /// </summary>
    public partial class InputShiftCell : UserControl,INotifyPropertyChanged
    {


        public InputShiftCell()
        {
            InitializeComponent();
            //ListBoxItem
        }

        private DateUpDown _DateUpDownUserControl;

        public DateUpDown DateUpDownUserControl
        {
            get { return _DateUpDownUserControl; }
            set
            {
                _DateUpDownUserControl = value;
                OnPropertyChanged("DateUpDownUserControl");
            }
        }

        private bool _IsHoliday;

        public bool IsHoliday
        {
            get { return _IsHoliday; }
            set 
            {
                _IsHoliday = value;

                if (value == true)
                {
                    this.gd00.Visibility = System.Windows.Visibility.Collapsed;
                    this.gd01.Visibility = System.Windows.Visibility.Collapsed;
                    this.gd10.Visibility = System.Windows.Visibility.Collapsed;
                    this.gd11.Visibility = System.Windows.Visibility.Collapsed;
                    this.gdAll.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.gd00.Visibility = System.Windows.Visibility.Visible;
                    this.gd01.Visibility = System.Windows.Visibility.Visible;
                    this.gd10.Visibility = System.Windows.Visibility.Visible;
                    this.gd11.Visibility = System.Windows.Visibility.Visible;
                    this.gdAll.Visibility = System.Windows.Visibility.Collapsed;
                }

                OnPropertyChanged("IsHoliday");
            }
        }




        #region INotifyPropertyChanged メンバ

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        


        #endregion
    }
}
