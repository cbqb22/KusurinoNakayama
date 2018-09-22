using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using OASystem.Model.Entity;

namespace OASystem.ViewModel.Common.Converters
{
    public class VanCodeToBalancingAccountConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            foreach (var bamaster in Model.DI.帳合先マスタ)
            {
                if (bamaster.卸ＶＡＮコード == value.ToString())
                {
                    return bamaster.帳合先名称;
                }
            }

            return value;


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            foreach (var bamaster in Model.DI.帳合先マスタ)
            {
                if (bamaster.帳合先名称 == value.ToString())
                {
                    return bamaster.卸ＶＡＮコード;
                }
            }

            return value;

        }

    }
}

