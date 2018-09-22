using System;
using System.Text;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
namespace View.Util.Common
{
    public static class StringConverter
    {
        public static string 数字全角to半角変換(string 全角数値文字列)
        {
            Dictionary<string,string> dic = new Dictionary<string,string>();
            dic.Add("０","0");
            dic.Add("１","1");
            dic.Add("２","2");
            dic.Add("３","3");
            dic.Add("４","4");
            dic.Add("５","5");
            dic.Add("６","6");
            dic.Add("７","7");
            dic.Add("８","8");
            dic.Add("９","9");

            string returnvalue = 全角数値文字列;
            foreach (var d in dic)
            {
                returnvalue = returnvalue.Replace(d.Key, d.Value);
            }

            return returnvalue;
        }
    }
}
