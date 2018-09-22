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

namespace MCSystem.ViewModel.Common.Converters
{
    public class RectToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            if (value is Rect)
            {
                var rect = (Rect)value;

                return string.Format("X:{0} Y:{1} / Width:{2} Heigh:{3}",rect.X,rect.Y,rect.Width,rect.Height);
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return value;

        }

    }
}

