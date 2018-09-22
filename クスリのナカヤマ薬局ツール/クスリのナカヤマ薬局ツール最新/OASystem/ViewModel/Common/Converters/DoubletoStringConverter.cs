using System;
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
    public class DoubletoStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "0.0";
            }

            if (value is double)
            {
                return ((double)value).ToString("#0.0");
            }

            return "0.0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return 0d;
            }

            double result;
            if (double.TryParse(value.ToString(), out result) == false)
            {
                return 0d;
            }

            return result;

        }

    }
}

