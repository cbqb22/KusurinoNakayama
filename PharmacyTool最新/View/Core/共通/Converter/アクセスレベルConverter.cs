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
using View.Core.共通.Entity;
using System.Linq;

namespace View.Core.共通.Converter
{
    public class アクセスレベルConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value == null)
            {
                return false;
            }

            //int result;
            //if (int.TryParse(parameter.ToString(), out result) == false)
            //{
            //    return false;
            //}


            switch ((int)value)
            {
                case (int)アクセスレベルEnum.アクセスレベル.管理薬剤師:

                    return 0;

                //if (result == 1)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                case (int)アクセスレベルEnum.アクセスレベル.正社員薬剤師:

                    return 1;
                //if (result == 2)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                case (int)アクセスレベルEnum.アクセスレベル.パート薬剤師:

                    return 2;
                //if (result == 3)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                case (int)アクセスレベルEnum.アクセスレベル.正社員事務:

                    return 3;
                //if (result == 4)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                case (int)アクセスレベルEnum.アクセスレベル.パート事務:

                    return 4;
                //if (result == 5)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                default: return -1;

            }


            //var type = typeof(アクセスレベルEnum.アクセスレベル);
            //var valueInfos = from field in type.GetFields()
            //                 where field.FieldType == typeof(アクセスレベルEnum.アクセスレベル)
            //                 select field.GetValue(type);
            //foreach (var v in valueInfos)
            //{
            //    if ((int)value == int.Parse(Enum.GetName(type,v.ToString())))
            //    {
            //        if (counter == result)
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //        //return Enum.GetName(type, (int)value);
            //    }
            //    counter++;
            //}

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            //int result;
            //if (int.TryParse(parameter.ToString(), out result) == false)
            //{
            //    return false;
            //}


            switch ((int)value)
            {
                case 0:

                    return (int)アクセスレベルEnum.アクセスレベル.管理薬剤師;

                //if (result == 1)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                case 1:

                    return (int)アクセスレベルEnum.アクセスレベル.正社員薬剤師;
                //if (result == 2)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                case 2:

                    return (int)アクセスレベルEnum.アクセスレベル.パート薬剤師;
                //if (result == 3)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                case 3:

                    return (int)アクセスレベルEnum.アクセスレベル.正社員事務;
                //if (result == 4)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                case 4:

                    return (int)アクセスレベルEnum.アクセスレベル.パート事務;
                //if (result == 5)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                default: return -1;

            }

        }
    }
}
