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
    public class ExpDeadInputAmountConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                if ((double)value == 0)
                {
                    return "";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            if (value is string)
            {
                if (value.ToString() == "")
                {
                    return 0;
                }
            }

            return value;

        }

    }
}

