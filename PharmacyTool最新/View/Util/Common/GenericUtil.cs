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
using System.Collections.Generic;

namespace View.Util.Common
{
    public static class GenericUtil
    {
        public static Dictionary<int, string> Copy(Dictionary<int,string> Source)
        {
            Dictionary<int, string> ReturnValue = new Dictionary<int, string>();
            foreach (var s in Source)
            {
                ReturnValue.Add(s.Key, s.Value);
            }

            return ReturnValue;
        }
    }
}
