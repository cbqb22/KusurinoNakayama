using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace クスリのナカヤマ薬局ツール.Model
{
    public static class DI
    {
        /// <summary>
        /// Version名
        /// </summary>
        private static string _在庫HP更新ツールVersionName;

        public static string 在庫HP更新ツールVersionName
        {
            get { return DI._在庫HP更新ツールVersionName; }
            set { DI._在庫HP更新ツールVersionName = value; }
        }


        private static string _現在庫ファイルパス;

        public static string 現在庫ファイルパス
        {
            get { return DI._現在庫ファイルパス; }
            set { DI._現在庫ファイルパス = value; }
        }


        private static string _使用量ファイルパス;

        public static string 使用量ファイルパス
        {
            get { return DI._使用量ファイルパス; }
            set { DI._使用量ファイルパス = value; }
        }
        private static string _不動品ファイルパス;

        public static string 不動品ファイルパス
        {
            get { return DI._不動品ファイルパス; }
            set { DI._不動品ファイルパス = value; }
        }
        private static string _出力先フォルダ名;

        public static string 出力先フォルダ名
        {
            get { return DI._出力先フォルダ名; }
            set { DI._出力先フォルダ名 = value; }
        }
        private static string _出力店舗名称;

        public static string 出力店舗名称
        {
            get { return DI._出力店舗名称; }
            set { DI._出力店舗名称 = value; }
        }
    }
}
