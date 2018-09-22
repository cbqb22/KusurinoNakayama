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

namespace View.Core.共通.Entity
{
    public static class アクセスレベルEnum
    {
        public enum アクセスレベル
        {
            管理薬剤師 = 1,
            正社員薬剤師 = 2,
            パート薬剤師 = 3,
            正社員事務 = 101,
            パート事務 = 102
        }


    }
}
