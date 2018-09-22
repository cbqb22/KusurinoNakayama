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
    public class 使用期限ForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            DateTime result;

            if (DateTime.TryParse(value.ToString(), out result) == false)
            {
                return "";
            }


            // 期限切れは赤固定
            if (result < System.DateTime.Now.Date)
            {
                return new SolidColorBrush(Colors.Red);
            }

            int 加算月 = 0;

            if (SingletonInstances.在庫管理FrameInstance.cmb使用期限色.SelectedIndex == -1)
            {
                加算月 = 3;
            }
            else
            {
                加算月 = SingletonInstances.在庫管理FrameInstance.cmb使用期限色.SelectedIndex + 1;

            }

            if (result < System.DateTime.Now.AddMonths(加算月).Date)
            {
                Color c = new Color();
                c.R = (byte)0;
                c.G = (byte)200;
                c.B = (byte)255;
                c.A = (byte)255;
                return new SolidColorBrush(c);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
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
