﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace OASystem.ViewModel.Common.Converters
{
    public class DateTimetoDispStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            if (value is DateTime)
            {
                return ((DateTime)value).ToString("yyyy/MM/dd");
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return new DateTime(1900, 1, 1);
            }

            DateTime result;
            if (DateTime.TryParse(value.ToString(), out result) == false)
            {
                return new DateTime(1900, 1, 1);
            }


            return result;

        }

    }
}

