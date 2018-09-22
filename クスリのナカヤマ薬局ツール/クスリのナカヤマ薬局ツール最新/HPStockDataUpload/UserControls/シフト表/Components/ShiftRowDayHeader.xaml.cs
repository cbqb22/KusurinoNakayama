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

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表.Components
{
    /// <summary>
    /// ShiftRowDayHeader.xaml の相互作用ロジック
    /// </summary>
    public partial class ShiftRowDayHeader : UserControl
    {
        private DateTime _StartDay;

        public DateTime StartDay
        {
            get { return _StartDay; }
            set { _StartDay = value; }
        }

        private DateTime _EndDay;

        public DateTime EndDay
        {
            get { return _EndDay; }
            set { _EndDay = value; }
        }


        public ShiftRowDayHeader()
        {
            InitializeComponent();
            this.StartDay = DateTime.Now;
            this.EndDay = StartDay.AddDays(16);

            this.SetDayAndDayOfWeeks();

        }

        public ShiftRowDayHeader(DateTime StartDay,DateTime EndDay)
        {
            this.StartDay = StartDay;
            this.EndDay = EndDay;
        }

        private void SetDayAndDayOfWeeks()
        {
            DateTime dateCounter = this.StartDay;

            while (dateCounter != EndDay)
            {
                Thickness th = new Thickness();
                th.Left = 0;
                th.Top = 0;
                th.Right = 0;
                th.Bottom = 0;

                DayAndDateCell dadc = new DayAndDateCell(dateCounter);
                dadc.Margin = th;
                spRowHeader.Children.Add(dadc);

                dateCounter = dateCounter.AddDays(1);
            }
        }



    }
}
