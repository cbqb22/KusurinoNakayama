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

namespace View.Core.共通
{
    public static class PageScope
    {
        /// <summary>
        /// ログインしたユーザーID
        /// 同一ページ内で有効
        /// </summary>
        private static string _ユーザーID;

        public static string ユーザーID
        {
            get { return PageScope._ユーザーID; }
            set { PageScope._ユーザーID = value; }
        }

        private static string _表示名称;

        public static string 表示名称
        {
            get { return PageScope._表示名称; }
            set { PageScope._表示名称 = value; }
        }

        /// <summary>
        /// ログインしたユーザーのアクセス権限
        /// 同一ページ内で有効
        /// -1は初期値で無効状態
        /// </summary>
        private static int _アクティブアクセス権限 = -1;

        public static int アクティブアクセス権限
        {
            get { return PageScope._アクティブアクセス権限; }
            set { PageScope._アクティブアクセス権限 = value; }
        }

        private static Dictionary<int, string> _表示順序;

        public static Dictionary<int, string> 表示順序
        {
            get { return PageScope._表示順序; }
            set { PageScope._表示順序 = value; }
        }
    }
}
