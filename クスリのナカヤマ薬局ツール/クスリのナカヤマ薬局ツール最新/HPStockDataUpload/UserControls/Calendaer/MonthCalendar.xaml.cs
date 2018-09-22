using System;
using System.Linq;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using クスリのナカヤマ薬局ツール.共通;

namespace クスリのナカヤマ薬局ツール.UserControls.Calendaer
{
    /// <summary>
    /// Interaction logic for MonthCalendar.xaml
    /// </summary>

    public class DayInformation : INotifyPropertyChanged
    {
        private int _day;
        private int _dayOfWeek;
        private bool _isToday;
        private bool _isSelectedDate;
        private bool _isNationalHoliday;
        private string _tips;
        private bool _isSelectableDate;

        public DayInformation()
        {
        }
        public DayInformation(int dayOfWeek)
        {
            this._dayOfWeek = dayOfWeek;
        }
        public int Day
        {
            get
            {
                return this._day;
            }
            set
            {
                this._day = value;
                this.FirePropertyChanged("Day");
            }
        }
        public int DayOfWeek
        {
            get
            {
                return this._dayOfWeek;
            }
            set
            {
                this._dayOfWeek = value;
                this.FirePropertyChanged("DayOfWeek");
            }
        }
        public bool IsToday
        {
            get
            {
                return this._isToday;
            }
            set
            {
                this._isToday = value;
                this.FirePropertyChanged("IsToday");
            }
        }
        public bool IsSelectedDate
        {
            get
            {
                return this._isSelectedDate;
            }
            set
            {
                this._isSelectedDate = value;
                this.FirePropertyChanged("IsSelectedDate");
            }
        }
        public bool IsNationalHoliday
        {
            get
            {
                return this._isNationalHoliday;
            }
            set
            {
                this._isNationalHoliday = value;
                this.FirePropertyChanged("IsNationalHoliday");
            }
        }
        public string Tips
        {
            get
            {
                return this._tips;
            }
            set
            {
                this._tips = value;
                this.FirePropertyChanged("Tips");
            }
        }

        public bool IsSelectableDate
        {
            get
            {
                return this._isSelectableDate;
            }
            set
            {
                this._isSelectableDate = value;
                this.FirePropertyChanged("IsSelectableDate");
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

    public class DateSelectedEventArgs : RoutedEventArgs
    {
        public DateTime SelectedDate;
    }

    public partial class MonthCalendar : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        public static RoutedEvent DateSelectedEvent;

        private DateTime _selectedDate;
        private DateTime _selectedMonth = DateTime.Today;
        private DayInformation[] _dayInfo = new DayInformation[42];

        static MonthCalendar()
        {
            DateSelectedEvent = EventManager.RegisterRoutedEvent("DateSelcted",
                RoutingStrategy.Bubble,
                typeof(EventHandler<DateSelectedEventArgs>),
                typeof(MonthCalendar));
        }

        public MonthCalendar()
        {
            InitializeComponent();

            // ヘッダ部の年月表示用
            this.DataContext = this;

            Style style = this.FindResource("Day") as Style;
            Label[] lb = new Label[42];
            for (int i = 0; i < lb.Length; i++)
            {
                lb[i] = new Label();
                lb[i].SetValue(Grid.RowProperty, i / 7 + 2);
                lb[i].SetValue(Grid.ColumnProperty, i % 7);
                lb[i].Style = style;
                lb[i].MouseDown += new MouseButtonEventHandler(MonthCalendar_MouseDown);
                this.DayGrid.Children.Add(lb[i]);
                // ラベルごとに曜日は固定
                this._dayInfo[i] = new DayInformation(i % 7);
                lb[i].DataContext = this._dayInfo[i];
            }
        }

        /// <summary>
        /// 日付選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthCalendar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lb = e.Source as Label;
            if (lb != null)
            {
                DayInformation di = lb.DataContext as DayInformation;
                if (di == null || di.Day == -1)
                {
                    return;
                }
                else
                {
                    DateTime checkDate = new DateTime(this.SelectedMonth.Year, this.SelectedMonth.Month, di.Day);
                    if (IsEnableSelectableDate)
                    {
                        if (!_selectableDate.Contains(checkDate))
                        {
                            return;
                        }
                    }
                    this.SelectedDate = checkDate;
                    DateSelectedEventArgs re = new DateSelectedEventArgs();
                    re.SelectedDate = this.SelectedDate;
                    re.RoutedEvent = DateSelectedEvent;
                    RaiseEvent(re);
                }
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedMonth = this.SelectedMonth.AddMonths(1);
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedMonth = this.SelectedMonth.AddMonths(-1);
        }

        private void buttonNextYear_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedMonth = this.SelectedMonth.AddYears(1);
        }

        private void buttonPrevYear_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedMonth = this.SelectedMonth.AddYears(-1);
        }

        /// <summary>
        /// カレンダー作成
        /// </summary>
        private void CreateCalender()
        {
            try
            {
                this.BeginInit();
                // 開始曜日取得 0:日曜～6:土曜
                int dow = (int)new DateTime(this.SelectedMonth.Year, this.SelectedMonth.Month, 1).DayOfWeek;
                // 日数取得
                int days = DateTime.DaysInMonth(this.SelectedMonth.Year, this.SelectedMonth.Month);
                for (int i = 0; i < this._dayInfo.Length; i++)
                {
                    this._dayInfo[i].Day = -1;
                }
                // 振替休日発生フラグ
                bool compensatoryHoliday = false;
                for (int i = dow; i < dow + days; i++)
                {
                    int day = i - dow + 1;
                    this._dayInfo[i].Day = day;
                    this._dayInfo[i].IsToday = this.IsSame(DateTime.Today, this.SelectedMonth.Year, this.SelectedMonth.Month, day);
                    this._dayInfo[i].IsSelectedDate = this.IsSame(this.SelectedDate, this.SelectedMonth.Year, this.SelectedMonth.Month, day);
                    NationalHolidayData holiday = NationalHolidaysData.I.GetData(this.SelectedMonth.Year, this.SelectedMonth.Month, day);
                    if (holiday != null)
                    {
                        this._dayInfo[i].IsNationalHoliday = true;
                        this._dayInfo[i].Tips = holiday.Tips;
                        // 日曜日が祝日の場合は振替休日が発生する
                        if (this._dayInfo[i].DayOfWeek == (int)DayOfWeek.Sunday)
                        {
                            compensatoryHoliday = true;
                        }
                    }
                    else
                    {
                        this._dayInfo[i].IsNationalHoliday = false;
                        // 振替休日
                        // 祝日が日曜にあたるときは、その日後において、その日に最も近い「国民の祝日」でない日を休日
                        // if (compensatoryHoliday == true && this._dayInfo[i].DayOfWeek != DayOfWeek.Sunday)
                        if (compensatoryHoliday == true)// 日曜日は国民の祝日ではないよなぁ…
                        {
                            compensatoryHoliday = false;
                            this._dayInfo[i].IsNationalHoliday = true;
                            this._dayInfo[i].Tips = "振替休日";
                        }
                        //// 振り替え休日チェック
                        // if (this._dayInfo[i].DayOfWeek == (int)DayOfWeek.Monday)
                        // {
                        //    if (0 < i && this._dayInfo[i - 1].IsNationalHoliday == true)
                        //    {
                        //        this._dayInfo[i].IsNationalHoliday = true;
                        //        this._dayInfo[i].Tips = "振替休日";
                        //    }
                        // }
                    }
                    DateTime date = new DateTime(this.SelectedMonth.Year, this.SelectedMonth.Month, day);
                    if (IsEnableSelectableDate && _selectableDate.Contains(date))
                    {
                        this._dayInfo[i].IsSelectableDate = true;
                    }
                    else
                    {
                        this._dayInfo[i].IsSelectableDate = false;
                    }
                }
                this.EndInit();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private bool IsSame(DateTime dt, int year, int month, int day)
        {
            return dt.Year == year && dt.Month == month && dt.Day == day;
        }

        public event EventHandler<DateSelectedEventArgs> DateSelected
        {
            add
            {
                AddHandler(DateSelectedEvent, value);
            }
            remove
            {
                RemoveHandler(DateSelectedEvent, value);
            }
        }

        public DateTime SelectedMonth
        {
            get
            {
                return this._selectedMonth;
            }
            set
            {
                this._selectedMonth = value;
                this.CreateCalender();
                this.FirePropertyChanged("SelectedMonth");
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return this._selectedDate;
            }
            set
            {
                DateTime TargetDate;
                if (IsEnableSelectableDate)
                {
                    if (value == NakayamaConst.NAKAYAMA_DATE_MAX)
                    {
                        DateTime ?maxDate = _selectableDate.Max();
                        if (maxDate != null)
                        {
                            TargetDate = maxDate ?? DateTime.Today;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (_selectableDate.Contains(value))
                        {
                            TargetDate = value;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (value == NakayamaConst.NAKAYAMA_DATE_MAX)
                    {
                        TargetDate = DateTime.Now;
                    }
                    else
                    {
                        TargetDate = value;
                    }
                }
                this._selectedDate = TargetDate;
                this.SelectedMonth = TargetDate;
                for (int i = 0; i < this._dayInfo.Length; i++)
                {
                    this._dayInfo[i].IsSelectedDate = this.IsSame(this.SelectedDate,
                        this.SelectedMonth.Year, this.SelectedMonth.Month, this._dayInfo[i].Day);
                }
                // this.CreateCalender();
                this.FirePropertyChanged("SelectedDate");
            }
        }

        public bool IsEnableSelectableDate
        {
            get { return (bool)GetValue(IsEnableSelectableDateProperty); }
            set { SetValue(IsEnableSelectableDateProperty, value); this.FirePropertyChanged("IsEnableSelectableDate"); }
        }

        public static readonly DependencyProperty IsEnableSelectableDateProperty =
            DependencyProperty.Register("IsEnableSelectableDate", typeof(bool), typeof(MonthCalendar), new UIPropertyMetadata(false));

        private IEnumerable<DateTime> _selectableDate = Enumerable.Empty<DateTime>();
        public IEnumerable<DateTime> SelectableDate 
        {
            get
            {
                return _selectableDate;
            }
            set
            {
                _selectableDate = value;
                this.FirePropertyChanged("SelectableDate");
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
