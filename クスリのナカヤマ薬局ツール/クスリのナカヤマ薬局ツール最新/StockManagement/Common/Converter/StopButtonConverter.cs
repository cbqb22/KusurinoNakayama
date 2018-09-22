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

namespace StockManagement.Common.Converter
{
    public class StopButtonConverter : IValueConverter
    {

        /// <summary>
        /// valueはIsBusyの値
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
 
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            bool b;

            try
            {
                b = (bool)value;
            }
            catch (Exception ex)
            {
                throw new Exception("コンバーターに渡された値の型が不適切な為、処理を中断しました。\r\n" + ex.Message + ex.StackTrace);
            }

            // Busyならば表示
            if (b)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
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

