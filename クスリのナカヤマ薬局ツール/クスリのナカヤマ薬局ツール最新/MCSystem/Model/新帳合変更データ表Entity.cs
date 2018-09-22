using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCSystem.Model
{
    public class 帳合変更結果Entity
    {

        /// <summary>
        /// 処理が成功したか
        /// </summary>
        private bool _IsSucceeded;

        public bool IsSucceeded
        {
            get { return _IsSucceeded; }
            set { _IsSucceeded = value; }
        }

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

        private string _新帳合先名;

        public string 新帳合先名
        {
            get { return _新帳合先名; }
            set { _新帳合先名 = value; }
        }

        private string _新帳合先コード;

        public string 新帳合先コード
        {
            get { return _新帳合先コード; }
            set { _新帳合先コード = value; }
        }

    }
}
