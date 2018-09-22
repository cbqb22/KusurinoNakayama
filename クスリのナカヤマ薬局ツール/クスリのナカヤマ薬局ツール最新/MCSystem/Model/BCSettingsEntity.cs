using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MCSystem.Model
{
    public class BCSettingsEntity
    {
        private Rect _検索名称XY座標;

        public Rect 検索名称XY座標
        {
            get { return _検索名称XY座標; }
            set { _検索名称XY座標 = value; }
        }

        private Rect _検索名称完了ボタンXY座標;

        public Rect 検索名称完了ボタンXY座標
        {
            get { return _検索名称完了ボタンXY座標; }
            set { _検索名称完了ボタンXY座標 = value; }
        }

        private Rect _通常仕入先XY座標;

        public Rect 通常仕入先XY座標
        {
            get { return _通常仕入先XY座標; }
            set { _通常仕入先XY座標 = value; }
        }

        private Rect _個別入力完了ボタンXY座標;

        public Rect 個別入力完了ボタンXY座標
        {
            get { return _個別入力完了ボタンXY座標; }
            set { _個別入力完了ボタンXY座標 = value; }
        }


        private Rect _在庫メンテナンス受付範囲;
        public Rect 在庫メンテナンス受付範囲
        {
            get { return _在庫メンテナンス受付範囲; }
            set { _在庫メンテナンス受付範囲 = value; }
        }

        private Rect _在庫メンテナンス範囲;

        public Rect 在庫メンテナンス範囲
        {
            get { return _在庫メンテナンス範囲; }
            set { _在庫メンテナンス範囲 = value; }
        }


        private int _メディセオコード;

        public int メディセオコード
        {
            get { return _メディセオコード; }
            set { _メディセオコード = value; }
        }

        private int _スズケンコード;

        public int スズケンコード
        {
            get { return _スズケンコード; }
            set { _スズケンコード = value; }
        }

        private int _東邦薬品コード;

        public int 東邦薬品コード
        {
            get { return _東邦薬品コード; }
            set { _東邦薬品コード = value; }
        }


        private int _東和薬品コード;

        public int 東和薬品コード
        {
            get { return _東和薬品コード; }
            set { _東和薬品コード = value; }
        }


        private int _アルフレッサコード;

        public int アルフレッサコード
        {
            get { return _アルフレッサコード; }
            set { _アルフレッサコード = value; }
        }

        private int _酒井薬品コード;

        public int 酒井薬品コード
        {
            get { return _酒井薬品コード; }
            set { _酒井薬品コード = value; }
        }


        private string _新帳合変更データ表パス;

        public string 新帳合変更データ表パス
        {
            get { return _新帳合変更データ表パス; }
            set { _新帳合変更データ表パス = value; }
        }

        private string _在庫テーブルCSVパス;

        public string 在庫テーブルCSVパス
        {
            get { return _在庫テーブルCSVパス; }
            set { _在庫テーブルCSVパス = value; }
        }


    }
}
