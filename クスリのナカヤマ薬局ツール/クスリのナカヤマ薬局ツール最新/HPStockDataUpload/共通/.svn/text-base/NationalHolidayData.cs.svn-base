using System;
using System.Collections.Generic;
using System.Text;

namespace クスリのナカヤマ薬局ツール.共通
{
    public class NationalHolidayData
    {
        /// <summary>
        /// 「第二月曜」などの場合はIsAbsoluteDate=FalseでWeekとDayOfWeekを使用する
        /// </summary>
        public bool IsAbsoluteDate;
        public int Month;
        public int Day;

        public int Week;
        public DayOfWeek DayOfWeek;

        public string Tips;

        public int StartYear = int.MinValue;
        public int EndYear = int.MaxValue;

        public NationalHolidayData(int month, int day, string tips)
        {
            this.IsAbsoluteDate = true;
            this.Month = month;
            this.Day = day;
            this.Tips = tips;
        }
        public NationalHolidayData(int month, int day, string tips, int startYear, int endYear)
        {
            this.IsAbsoluteDate = true;
            this.Month = month;
            this.Day = day;
            this.Tips = tips;

            this.StartYear = startYear;
            this.EndYear = endYear;
        }
        public NationalHolidayData(int month, int week, DayOfWeek dayOfWeek, string tips)
        {
            this.IsAbsoluteDate = false;
            this.Month = month;
            this.Week = week;
            this.DayOfWeek = dayOfWeek;
            this.Tips = tips;
        }


    }



}
