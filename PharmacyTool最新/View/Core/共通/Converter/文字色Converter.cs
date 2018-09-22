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
using View.Core.共通;


namespace View.Core.共通.Converter
{
    public class 文字色Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Brown);
            }

            string 色 = value as string;
            if (色 == null)
            {
                return new SolidColorBrush(Colors.Brown);
            }
            else if (色.Equals("茶色"))
            {
                return new SolidColorBrush(Colors.Brown);
            }
            else if (色.Equals("赤"))
            {
                return new SolidColorBrush(Colors.Red);
            }
            else if (色.Equals("緑"))
            {
                return new SolidColorBrush(Colors.Green);
            }
            else if (色.Equals("青"))
            {
                return new SolidColorBrush(Colors.Blue);
            }
            else if (色.Equals("紫"))
            {
                return new SolidColorBrush(Colors.Purple);
            }
            else if (色.Equals("ピンク"))
            {
                return new SolidColorBrush(Colors.Magenta);
            }
            else if (色.Equals("オレンジ"))
            {
                Color col = new Color();
                col.R = (byte)255;
                col.G = (byte)80;
                col.B = (byte)0;
                col.A = (byte)255;
                return new SolidColorBrush(col);
            }
            else if (色.Equals("黒"))
            {
                return new SolidColorBrush(Colors.Black);
            }
            else
            {
                return new SolidColorBrush(Colors.Brown);
            }

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
