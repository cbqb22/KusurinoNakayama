using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using OASystem.Model.Enum;

namespace OASystem.Model.Entity
{
    public class OrderScheduledListEntity : INotifyPropertyChanged
    {
        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }

        private double _数量;

        public double 数量
        {
            get { return _数量; }
            set { _数量 = value; }
        }

        private double _薬価;

        public double 薬価
        {
            get { return _薬価; }
            set { _薬価 = value; }
        }

        private string _レセプト電算コード;

        public string レセプト電算コード
        {
            get { return _レセプト電算コード; }
            set { _レセプト電算コード = value; }
        }

        private string _JANコード;

        public string JANコード
        {
            get { return _JANコード; }
            set { _JANコード = value; }
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


        private double _包装総量;

        public double 包装総量
        {
            get { return _包装総量; }
            set { _包装総量 = value; }
        }

        //private string _数量ｘ包装総量_包装単位;

        public string 数量ｘ包装総量_包装単位
        {
            get
            {
                return _数量 * _包装総量 + " " +  OASystem.ViewModel.Common.DataConvert.DataConvert.包装単位ショート変換(包装単位);
            }
        }

        private string _包装;

        public string 包装
        {
            get
            {
                return string.Format("{0}{1} {2}({3})", _包装総量, _包装単位, _包装形態, _包装単位数);
            }
            set
            {
                _包装 = value;
            }
        }

        private 剤形区分Enum _剤形区分;

        public 剤形区分Enum 剤形区分
        {
            get { return _剤形区分; }
            set { _剤形区分 = value; }
        }




        private bool _Isデッド品該当;

        public bool Isデッド品該当
        {
            get { return _Isデッド品該当; }
            set { _Isデッド品該当 = value; }
        }

        private bool _Is期限切迫該当;

        public bool Is期限切迫該当
        {
            get { return _Is期限切迫該当; }
            set { _Is期限切迫該当 = value; }
        }

        // とりあえずまだいらないフラグ
        //private bool _Is保護該当;

        //public bool Is保護該当
        //{
        //    get { return _Is保護該当; }
        //    set { _Is保護該当 = value; }
        //}

        private bool _Is優先移動該当;

        public bool Is優先移動該当
        {
            get { return _Is優先移動該当; }
            set { _Is優先移動該当 = value; }
        }

        private string _優先移動コメント;

        public string 優先移動コメント
        {
            get { return _優先移動コメント; }
            set { _優先移動コメント = value; }
        }

        private string _レセ発注伝票No;

        public string レセ発注伝票No
        {
            get { return _レセ発注伝票No; }
            set { _レセ発注伝票No = value; }
        }

        // DATの中の医薬品の順番 Distinct用
        private int _注文番号;
        public int 注文番号
        {
            get { return _注文番号; }
            set { _注文番号 = value; }
        }

        private string _帳合先名称;

        public string 帳合先名称
        {
            get { return _帳合先名称; }
            set { _帳合先名称 = value; }
        }

        private string _卸VANコード;

        public string 卸VANコード
        {
            get { return _卸VANコード; }
            set { _卸VANコード = value; }
        }


        // ListViewItemの背景色変更用
        private bool _Is注文あり;
        public bool Is注文あり
        {
            get { return _Is注文あり; }
            set 
            {
                _Is注文あり = value;
                this.FirePropertyChanged("Is注文あり");
            }
        }

        //private bool _Is他店該当;

        public bool Is他店該当
        {
            get 
            {
                if (_Isデッド品該当 || _Is期限切迫該当 || _Is優先移動該当)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 帳合一致の表示用
        /// trueは"○"
        /// falseは"×"
        /// nullは"-"
        /// </summary>
        private bool? _Is帳合一致 = true;

        public bool? Is帳合一致
        {
            get { return _Is帳合一致; }
            set 
            {
                _Is帳合一致 = value;
                this.FirePropertyChanged("Is帳合一致");

            }
        }


        private bool _SEND01DATから削除するか;

        public bool SEND01DATから削除するか
        {
            get { return _SEND01DATから削除するか; }
            set 
            {
                _SEND01DATから削除するか = value;
                this.FirePropertyChanged("SEND01DATから削除するか");
            }
        }


        #region INotifyPropertyChanged メンバ

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    }


    // Custom comparer for the Product class
    public class OrderScheduledListEntityComparer : IEqualityComparer<OrderScheduledListEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(OrderScheduledListEntity x, OrderScheduledListEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (x.JANコード == y.JANコード &&
                x.医薬品名 == y.医薬品名 &&
                x.数量 == y.数量 &&
                x.包装単位 == y.包装単位 &&
                x.包装総量 == y.包装総量 &&
                x.レセプト電算コード == y.レセプト電算コード &&
                x.レセ発注伝票No == y.レセ発注伝票No &&
                x.注文番号 == y.注文番号
                )
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(OrderScheduledListEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ (ent.JANコード == null ? 0 : ent.JANコード.GetHashCode());
            result = result ^ (ent.医薬品名 == null ? 0 : ent.医薬品名.GetHashCode());
            result = result ^ (ent.数量.GetHashCode());
            result = result ^ (ent.包装単位 == null ? 0 : ent.包装単位.GetHashCode());
            result = result ^ (ent.包装総量.GetHashCode());
            result = result ^ (ent.レセプト電算コード == null ? 0 : ent.レセプト電算コード.GetHashCode());
            result = result ^ (ent.レセ発注伝票No == null ? 0 : ent.レセ発注伝票No.GetHashCode());
            result = result ^ (ent.注文番号.GetHashCode());

            //Calculate the hash code for the product.
            return result;
        }

    }

}
