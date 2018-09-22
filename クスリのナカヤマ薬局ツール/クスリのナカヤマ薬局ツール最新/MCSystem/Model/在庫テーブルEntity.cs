using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCSystem.Model
{
    public class 在庫テーブルEntity
    {
        private string _商品コード;

        public string 商品コード
        {
            get { return _商品コード; }
            set { _商品コード = value; }
        }

        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }

        private string _通常仕入先名;

        public string 通常仕入先名
        {
            get { return _通常仕入先名; }
            set { _通常仕入先名 = value; }
       }

        private string _通常仕入先コード;

        public string 通常仕入先コード
        {
            get { return _通常仕入先コード; }
            set { _通常仕入先コード = value; }
        }

        // 読込み時にエラーがあったデータ
        // はじいて処理し、あとで結果に出力
        private bool _IsErrorData;

        public bool IsErrorData
        {
            get { return _IsErrorData; }
            set { _IsErrorData = value; }
        }
    }
}
