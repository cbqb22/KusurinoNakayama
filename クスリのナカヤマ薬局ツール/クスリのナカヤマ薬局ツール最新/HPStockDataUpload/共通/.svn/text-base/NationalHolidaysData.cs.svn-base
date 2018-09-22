using System;
using System.Collections.Generic;
using System.Text;
using クスリのナカヤマ薬局ツール.共通;

namespace クスリのナカヤマ薬局ツール.共通
{
    public class NationalHolidaysData : List<NationalHolidayData>
    {
        private static NationalHolidaysData Instance;

        private NationalHolidaysData()
        {
            this.Add(new NationalHolidayData(1, 1, "元旦"));
            this.Add(new NationalHolidayData(1, 2, DayOfWeek.Monday, "成人の日"));
            this.Add(new NationalHolidayData(2, 11, "建国記念の日"));
            // this.Add(new NationalHolidayData(3, 21, "春分の日"));
            this.Add(new NationalHolidayData(4, 29, "みどりの日", NakayamaConst.NAKAYAMA_DATE_MIN.Year, 2006));
            this.Add(new NationalHolidayData(4, 29, "昭和の日", 2007, NakayamaConst.NAKAYAMA_DATE_MAX.Year));
            this.Add(new NationalHolidayData(5, 3, "憲法記念日"));
            this.Add(new NationalHolidayData(5, 4, "国民の休日", NakayamaConst.NAKAYAMA_DATE_MIN.Year, 2006));
            this.Add(new NationalHolidayData(5, 4, "みどりの日", 2007, NakayamaConst.NAKAYAMA_DATE_MAX.Year));
            this.Add(new NationalHolidayData(5, 5, "こどもの日"));
            this.Add(new NationalHolidayData(7, 3, DayOfWeek.Monday, "海の日"));
            this.Add(new NationalHolidayData(9, 3, DayOfWeek.Monday, "敬老の日"));
            // this.Add(new NationalHolidayData(9, 23, "秋分の日"));
            this.Add(new NationalHolidayData(10, 2, DayOfWeek.Monday, "体育の日"));
            this.Add(new NationalHolidayData(11, 3, "文化の日"));
            this.Add(new NationalHolidayData(11, 23, "勤労感謝の日"));
            this.Add(new NationalHolidayData(12, 23, "天皇誕生日"));
        }

        public static NationalHolidaysData I
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NationalHolidaysData();
                }
                return Instance;
            }
        }

        public NationalHolidayData GetData(int year, int month, int day)
        {
            foreach (NationalHolidayData d in this)
            {
                if (d.IsAbsoluteDate == true)
                {
                    if (d.Month == month && d.Day == day && d.StartYear <= year && year <= d.EndYear)
                    {
                        return d;
                    }
                }
                else
                {
                    DateTime dt = new DateTime(year, month, day);
                    if (dt.Month == d.Month &&
                        dt.DayOfWeek == d.DayOfWeek &&
                        this.GetWeekInMonth(year, month, day, d.DayOfWeek) == d.Week
                        && d.StartYear <= year && year <= d.EndYear)
                    {
                        return d;
                    }
                    // 春分の日
                    if (month == 3)
                    {
                        if (day == this.GetSpringHoliday(year))
                        {
                            return new NationalHolidayData(month, day, "春分の日");
                        }
                    }
                    // 秋分の日
                    if (month == 9)
                    {
                        if (day == this.GetAutomnHoliday(year))
                        {
                            return new NationalHolidayData(month, day, "秋分の日");
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 春分の日を取得
        /// </summary>
        /// <param name="year"></param>
        /// <returns>日</returns>
        private int GetSpringHoliday(int year)
        {
            if (1900 <= year && year <= 2099)
            {
                switch (year % 4)
                {
                    case 0:
                        if (this.Between(year, 1900, 1956) == true) return 21;
                        if (this.Between(year, 1960, 2088) == true) return 20;
                        if (this.Between(year, 2092, 2096) == true) return 19;
                        return 21;
                    case 1:
                        if (this.Between(year, 1901, 1989) == true) return 21;
                        if (this.Between(year, 1993, 2097) == true) return 20;
                        return 21;
                    case 2:
                        if (this.Between(year, 1902, 2022) == true) return 21;
                        if (this.Between(year, 2026, 2098) == true) return 20;
                        return 21;
                    case 3:
                        if (this.Between(year, 1903, 1923) == true) return 22;
                        if (this.Between(year, 1927, 2055) == true) return 21;
                        if (this.Between(year, 2059, 2099) == true) return 20;
                        return 21;
                    default:
                        return 21;
                }
            }
            else
            {
                return 21;
            }
        }

        /// <summary>
        /// 秋分の日を取得
        /// </summary>
        /// <param name="year"></param>
        /// <returns>日</returns>
        private int GetAutomnHoliday(int year)
        {
            if (1900 <= year && year <= 2099)
            {
                switch (year % 4)
                {
                    case 0:
                        if (this.Between(year, 1900, 1900) == true) return 24;
                        if (this.Between(year, 1904, 2008) == true) return 23;
                        if (this.Between(year, 2012, 2096) == true) return 22;
                        return 23;
                    case 1:
                        if (this.Between(year, 1901, 1917) == true) return 24;
                        if (this.Between(year, 1921, 2041) == true) return 23;
                        if (this.Between(year, 2045, 2097) == true) return 22;
                        return 23;
                    case 2:
                        if (this.Between(year, 1902, 1946) == true) return 24;
                        if (this.Between(year, 1950, 2074) == true) return 23;
                        if (this.Between(year, 2078, 2098) == true) return 22;
                        return 23;
                    case 3:
                        if (this.Between(year, 1903, 1979) == true) return 24;
                        if (this.Between(year, 1983, 2099) == true) return 23;
                        return 23;
                    default:
                        return 23;
                }
            }
            else
            {
                return 23;
            }
        }

        private bool Between(int value, int from, int to)
        {
            return from <= value && value <= to;
        }

        /// <summary>
        /// 指定日が第何週かを取得
        /// </summary>
        private int GetWeekInMonth(int year, int month, int day, DayOfWeek dow)
        {
            int week = 0;
            int days = DateTime.DaysInMonth(year, month);
            for (int i = 1; i <= days; i++)
            {
                DateTime dt = new DateTime(year, month, i);
                if (dt.DayOfWeek == dow)
                {
                    week++;
                }
                if (i == day)
                {
                    return week;
                }
            }
            return -1;
        }



    }
}
