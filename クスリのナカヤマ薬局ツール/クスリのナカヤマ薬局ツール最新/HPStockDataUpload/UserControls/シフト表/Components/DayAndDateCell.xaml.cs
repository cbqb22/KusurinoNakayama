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


namespace クスリのナカヤマ薬局ツール.UserControls.シフト表.Components
{
    /// <summary>
    /// DayAndDateCell.xaml の相互作用ロジック
    /// </summary>
    public partial class DayAndDateCell : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public DayAndDateCell(DateTime Date)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ShortTimeDate = Date;
        }

        private DateTime _ShortTimeDate;

        public DateTime ShortTimeDate
        {
            get 
            {
                return _ShortTimeDate;

            }
            set { _ShortTimeDate = value; }
        }


        public string DayOfWeek
        {
            get 
            {
                switch (ShortTimeDate.DayOfWeek)
                {
                    case System.DayOfWeek.Sunday:
                        return "日";

                    case System.DayOfWeek.Monday:
                        return "月";

                    case System.DayOfWeek.Tuesday:
                        return "火";

                    case System.DayOfWeek.Wednesday:
                        return "水";

                    case System.DayOfWeek.Thursday:
                        return "木";

                    case System.DayOfWeek.Friday:
                        return "金";

                    case System.DayOfWeek.Saturday:
                        return "土";

                    default:
                        throw new Exception("曜日が不明です。");
                }

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
