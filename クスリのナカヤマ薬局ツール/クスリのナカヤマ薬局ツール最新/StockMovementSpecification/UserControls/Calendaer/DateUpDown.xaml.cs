#region Using

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using クスリのナカヤマ薬局ツール.共通;

#endregion

namespace クスリのナカヤマ薬局ツール.UserControls.Calendaer
{
    /// <summary>
    /// Interaction logic for MonthCalendar.xaml
    /// </summary>
    public partial class DateUpDown
    {
        public static readonly DependencyProperty MaximumProperty;
        public static readonly DependencyProperty MinimumProperty;
        public static readonly DependencyProperty SelectedDateProperty;

        private bool _isReadOnly;
        private bool _isEnabled;

        static DateUpDown()
        {
            SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof (DateTime), typeof (DateUpDown), new PropertyMetadata(NakayamaConst.NAKAYAMA_DATE_MAX));
            MaximumProperty = DependencyProperty.Register("MaximumDate", typeof (DateTime), typeof (DateUpDown), new PropertyMetadata(NakayamaConst.NAKAYAMA_DATE_MAX));
            MinimumProperty = DependencyProperty.Register("MinimumDate", typeof (DateTime), typeof (DateUpDown), new PropertyMetadata(NakayamaConst.NAKAYAMA_DATE_MIN));
        }

        public DateUpDown()
        {
            InitializeComponent();

            Focusable = true;

            textBox.LostFocus += textBox_LostFocus;
            PreviewKeyDown += MonthCalendar_PreviewKeyDown;
            TargetUpdated += MonthCalendar_TargetUpdated;
        }

        protected void MonthCalendar_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (SelectedDate == NakayamaConst.NAKAYAMA_DATE_MAX)
            {
                textBox.Text = "";
            }
            else
            {
                textBox.Text = SelectedDate.ToShortDateString();
            }
        }

        protected void MonthCalendar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                textBox_LostFocus(null, null);
                repeatButtonUp_Click(null, null);
            }
            else if (e.Key == Key.Down)
            {
                textBox_LostFocus(null, null);
                repeatButtonDown_Click(null, null);
            }
        }

        protected void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //入力値のチェック
            DateTime result;
            if (DateTime.TryParse(textBox.Text, out result) == false)
            {
                SelectedDate = NakayamaConst.NAKAYAMA_DATE_MAX;
                return;
            }
            if (result < Minimum)
            {
                SelectedDate = Minimum;
                return;
            }
            if (Maximum < result)
            {
                SelectedDate = NakayamaConst.NAKAYAMA_DATE_MAX;
                return;
            }
            SelectedDate = result;
        }

        protected void repeatButtonUp_Click(object sender, RoutedEventArgs e)
        {
            textBox_LostFocus(null, null);
            SelectedDate = SelectedDate.AddDays(1);
            textBox.Select(textBox.Text.Length, 0);
            textBox.Focus();
        }

        protected void repeatButtonDown_Click(object sender, RoutedEventArgs e)
        {
            textBox_LostFocus(null, null);
            if (SelectedDate == NakayamaConst.NAKAYAMA_DATE_MAX)
            {
                SelectedDate = DateTime.Now;
            }
            SelectedDate = SelectedDate.AddDays(-1);
            textBox.Select(textBox.Text.Length, 0);
            textBox.Focus();
        }

        public DateTime SelectedDate
        {
            get { return (DateTime) GetValue(SelectedDateProperty); }
            set
            {
                if (value < Minimum)
                {
                    return;
                }
                else if (Maximum < value)
                {
                    textBox.Text = "";
                    return;
                }
                else if (Maximum == value)
                {
                    SetValue(SelectedDateProperty, value);
                    textBox.Text = "";
                    return;
                }
                else
                {
                    SetValue(SelectedDateProperty, value);
                    textBox.Text = value.ToShortDateString();
                }
            }
        }

        public DateTime Maximum
        {
            get { return (DateTime) GetValue(MaximumProperty); }
            set
            {
                if (value < NakayamaConst.NAKAYAMA_DATE_MIN)
                {
                    return;
                }
                if (value > NakayamaConst.NAKAYAMA_DATE_MAX)
                {
                    return;
                }
                SetValue(MaximumProperty, value);
                if (value < Minimum)
                {
                    Minimum = value;
                }
                if (value < SelectedDate)
                {
                    SelectedDate = value;
                }
            }
        }

        public DateTime Minimum
        {
            get { return (DateTime) GetValue(MinimumProperty); }
            set
            {
                if (value < NakayamaConst.NAKAYAMA_DATE_MIN)
                {
                    return;
                }
                if (value > NakayamaConst.NAKAYAMA_DATE_MAX)
                {
                    return;
                }
                SetValue(MinimumProperty, value);
                if (Maximum < value)
                {
                    Maximum = value;
                }
                if (SelectedDate < value)
                {
                    SelectedDate = value;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                textBox.IsReadOnly = value;
                rbUp.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                rbDown.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public new bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                textBox.IsEnabled = value;
                rbUp.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                rbDown.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}