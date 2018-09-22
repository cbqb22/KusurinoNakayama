using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace StockManagement.ViewModel.Common.DataConvert
{
    public static class DataConvert
    {

        public static string 全角To半角変換(string source)
        {
            Regex re = new Regex("[０-９Ａ-Ｚａ-ｚ：－　]+");
            string output = re.Replace(source, myReplacer);

            return output;

        }

        public static string myReplacer(Match m)
        {
            return Strings.StrConv(m.Value, VbStrConv.Narrow, 0);
        }
    }
}
