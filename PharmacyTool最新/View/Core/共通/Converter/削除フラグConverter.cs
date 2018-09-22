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

namespace View.Core.共通.Converter
{
    public class 削除フラグConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }


            // 削除ならば、SelectedIndex = 1
            // でなければ、SelectedIndex = 0
            if ((bool)value == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                throw new Exception("操作に失敗しました。再度実行して下さい。");
            }


            // 削除ならば、SelectedIndex = 1
            // でなければ、SelectedIndex = 0
            if ((int)value == 1)
            {
                return "true";
            }
            else
            {
                return "false";
            }

        }

    }
}
