using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace OASystem.ViewModel.Common.DateCenter
{
    public static class Converter
    {


        public static DateTime OAS_MaxDate = new DateTime(9999,12,31);
        public static DateTime OAS_MinDate = new DateTime(1900, 01, 01);

        public static string Get和暦元号(DateTime 日付)
        {
            JapaneseCalendar jc = new JapaneseCalendar();
            string[] 元号名 = { "明治", "大正", "昭和", "平成" };

            return 元号名[jc.GetEra(日付) - 1];
        }

        public static int Get和暦年(DateTime 日付)
        {
            JapaneseCalendar jc = new JapaneseCalendar();
            return jc.GetYear(日付);
        }

        public static DateTime GetLastDay(DateTime source)
        {

            DateTime d = new DateTime(source.Year, source.Month, DateTime.DaysInMonth(source.Year, source.Month));
            //DateTime d;
            //try
            //{
            //    d = new DateTime(source.Year, source.Month, 1);
            //    d = d.AddMonths(1).AddDays(-1);
            //}
            //catch
            //{
            //    return OAS_MaxDate;
            //}

            return d;
        }


        public static string ConverDayOfWeekEnglishToJapanese(DayOfWeek dayofweekEnglish)
        {
            switch (dayofweekEnglish)
            {
                case DayOfWeek.Monday:
                    return "月";
                case DayOfWeek.Tuesday:
                    return "火";
                case DayOfWeek.Wednesday:
                    return "水";
                case DayOfWeek.Thursday:
                    return "木";
                case DayOfWeek.Friday:
                    return "金";
                case DayOfWeek.Saturday:
                    return "土";
                case DayOfWeek.Sunday:
                    return "日";
                default:
                    return "";
            }

        }
    }
}
