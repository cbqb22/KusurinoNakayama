using System;
using System.Collections.Generic;
using System.Text;
using OASystem.Model.Enum;

namespace OASystem.Model.Entity
{
    public class BalancingAccountsCheckMedicineSortEntity
    {
        private string _JANコード;

        public string JANコード
        {
            get { return _JANコード; }
            set { _JANコード = value; }
        }

        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }


        private string _包装形態;

        public string 包装形態
        {
            get { return _包装形態; }
            set { _包装形態 = value; }
        }

        private string _包装単位;

        public string 包装単位
        {
            get { return _包装単位; }
            set { _包装単位 = value; }
        }

        private string _包装単位数;

        public string 包装単位数
        {
            get { return _包装単位数; }
            set { _包装単位数 = value; }
        }


        private string _包装総量;

        public string 包装総量
        {
            get { return _包装総量; }
            set { _包装総量 = value; }
        }

        private 剤形区分Enum _剤形区分;

        public 剤形区分Enum 剤形区分
        {
            get { return _剤形区分; }
            set { _剤形区分 = value; }
        }


        private string _包装;

        public string 包装
        {
            get 
            {
                if (_包装総量 ==  "-" && _包装単位 == "-" && _包装形態 == "-" &&  _包装単位数 == "-")
                {
                    return "-";
                }
                else
                {
                    return string.Format("{0}{1} {2}({3})", _包装総量, _包装単位, _包装形態, _包装単位数);
                }
            }
            set
            {
                _包装 = value;
            }
        }



        private string _製薬会社;

        public string 製薬会社
        {
            get { return _製薬会社; }
            set { _製薬会社 = value; }
        }

        private string _販売会社;

        public string 販売会社
        {
            get { return _販売会社; }
            set { _販売会社 = value; }
        }

        private string _帳合先;

        public string 帳合先
        {
            get { return _帳合先; }
            set { _帳合先 = value; }
        }

        private string _修正後帳合先;

        public string 修正後帳合先
        {
            get { return _修正後帳合先; }
            set { _修正後帳合先 = value; }
        }

        private string _レセプト電算コード;

        public string レセプト電算コード
        {
            get { return _レセプト電算コード; }
            set { _レセプト電算コード = value; }
        }

        private string _個別医薬品コード;

        public string 個別医薬品コード
        {
            get { return _個別医薬品コード; }
            set { _個別医薬品コード = value; }
        }

        private bool _IsJAN管理;

        public bool IsJAN管理
        {
            get { return _IsJAN管理; }
            set { _IsJAN管理 = value; }
        }
    }

    /// <summary>
    /// 店名、医薬品名、レセプト電算コード、在庫数、包装単位一致
    /// </summary>
    public class BalancingAccountsCheckMedicineSortEntityNameCodeComparer : IEqualityComparer<BalancingAccountsCheckMedicineSortEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(BalancingAccountsCheckMedicineSortEntity x, BalancingAccountsCheckMedicineSortEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (
                x.医薬品名 == y.医薬品名 &&
                x.レセプト電算コード == y.レセプト電算コード
                )
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(BalancingAccountsCheckMedicineSortEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ (ent.医薬品名 == null ? 0 : ent.医薬品名.GetHashCode());
            result = result ^ (ent.レセプト電算コード == null ? 0 : ent.レセプト電算コード.GetHashCode());

            //Calculate the hash code for the product.
            return result;
        }

    }

}
