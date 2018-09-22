using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MCSystem.Model
{
    public static class DI
    {
        private static Rect _検索名称XY座標;

        public static Rect 検索名称XY座標
        {
            get { return DI._検索名称XY座標; }
            set { DI._検索名称XY座標 = value; }
        }

        private static Rect _通常仕入先XY座標;

        public static Rect 通常仕入先XY座標
        {
            get { return DI._通常仕入先XY座標; }
            set { DI._通常仕入先XY座標 = value; }
        }

        private static Rect _検索名称完了ボタンXY座標;

        public static Rect 検索名称完了ボタンXY座標
        {
            get { return DI._検索名称完了ボタンXY座標; }
            set { DI._検索名称完了ボタンXY座標 = value; }
        }


        private static Rect _個別入力完了ボタンXY座標;

        public static Rect 個別入力完了ボタンXY座標
        {
            get { return DI._個別入力完了ボタンXY座標; }
            set { DI._個別入力完了ボタンXY座標 = value; }
        }


        private static Rect _在庫メンテナンス受付範囲;
        public static Rect 在庫メンテナンス受付範囲
        {
            get { return _在庫メンテナンス受付範囲; }
            set { _在庫メンテナンス受付範囲 = value; }
        }

        private static Rect _在庫メンテナンス範囲;

        public static Rect 在庫メンテナンス範囲
        {
            get { return _在庫メンテナンス範囲; }
            set { _在庫メンテナンス範囲 = value; }
        }


        private static int _メディセオコード;

        public static int メディセオコード
        {
            get { return _メディセオコード; }
            set { _メディセオコード = value; }
        }

        private static int _スズケンコード;

        public static int スズケンコード
        {
            get { return _スズケンコード; }
            set { _スズケンコード = value; }
        }

        private static int _東邦薬品コード;

        public static int 東邦薬品コード
        {
            get { return _東邦薬品コード; }
            set { _東邦薬品コード = value; }
        }


        private static int _東和薬品コード;

        public static int 東和薬品コード
        {
            get { return _東和薬品コード; }
            set { _東和薬品コード = value; }
        }


        private static int _アルフレッサコード;

        public static int アルフレッサコード
        {
            get { return _アルフレッサコード; }
            set { _アルフレッサコード = value; }
        }

        private static int _酒井薬品コード;

        public static int 酒井薬品コード
        {
            get { return _酒井薬品コード; }
            set { _酒井薬品コード = value; }
        }




        private static string _新帳合変更データ表パス;

        public static string 新帳合変更データ表パス
        {
            get { return _新帳合変更データ表パス; }
            set { _新帳合変更データ表パス = value; }
        }

        private static string _在庫テーブルCSVパス;

        public static string 在庫テーブルCSVパス
        {
            get { return _在庫テーブルCSVパス; }
            set { _在庫テーブルCSVパス = value; }
        }
    }
}
