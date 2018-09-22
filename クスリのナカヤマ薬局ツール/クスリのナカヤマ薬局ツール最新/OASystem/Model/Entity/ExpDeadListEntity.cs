using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using OASystem.Model.Enum;

namespace OASystem.Model.Entity
{
    public class ExpDeadListEntity : INotifyPropertyChanged
    {
        private string _店名;

        public string 店名
        {
            get { return _店名; }
            set { _店名 = value; }
        }

        private string _レセプト電算コード;

        public string レセプト電算コード
        {
            get { return _レセプト電算コード; }
            set { _レセプト電算コード = value; }
        }

        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }

        private double _在庫数;

        public double 在庫数
        {
            get { return _在庫数; }
            set { _在庫数 = value; }
        }


        private double _薬価;

        public double 薬価
        {
            get { return _薬価; }
            set { _薬価 = value; }
        }



        private double _一包単位量;
        public double 一包単位量
        {
            get { return _一包単位量; }
            set { _一包単位量 = value; }
        }

        private string _名称２;
        public string 名称２
        {
            get { return _名称２; }
            set { _名称２ = value; }
        }

        private string _医薬品名と名称２連結;
        public string 医薬品名と名称２連結
        {
            get
            {
                if (string.IsNullOrEmpty(_名称２))
                {
                    return _医薬品名;

                }

                return _医薬品名 + "（" + _名称２ + "）";
            }
            set { _医薬品名と名称２連結 = value; }
        }




        public string 在庫数表示用
        {
            get 
            {
                return string.Format("{0} {1}",在庫数,OASystem.ViewModel.Common.DataConvert.DataConvert.包装単位ショート変換(包装単位)); 
            }
        }

        private bool _Is期限切迫;

        public bool Is期限切迫
        {
            get { return _Is期限切迫; }
            set { _Is期限切迫 = value; }
        }

        private bool _Isデッド品;

        public bool Isデッド品
        {
            get { return _Isデッド品; }
            set { _Isデッド品 = value; }
        }



        private bool _Is優先移動;

        public bool Is優先移動
        {
            get { return _Is優先移動; }
            set { _Is優先移動 = value; }
        }

        private string _優先移動コメント;

        public string 優先移動コメント
        {
            get { return _優先移動コメント; }
            set { _優先移動コメント = value; }
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

        private DateTime _使用期限;
        public DateTime 使用期限
        {
            get { return _使用期限; }
            set { _使用期限 = value; }
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


        private double _注文数;
        public double 注文数
        {
            get { return _注文数; }
            set 
            {
                _注文数 = value;
                this.FirePropertyChanged("注文数");
                this.FirePropertyChanged("Is注文あり");
            }
        }

        private bool _Is注文あり;
        public bool Is注文あり
        {
            get
            {
                return _注文数 != 0 ? true : false; 
            }
            set { _Is注文あり = value; }
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


    /// <summary>
    /// 店名、医薬品名、レセプト電算コード、在庫数、包装単位一致、名称２ 、一包単位量、使用期限
    /// </summary>
    public class ExpDeadListEntityComparer : IEqualityComparer<ExpDeadListEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ExpDeadListEntity x, ExpDeadListEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (
                x.店名 == y.店名 &&
                x.レセプト電算コード == y.レセプト電算コード &&
                x.医薬品名 == y.医薬品名 &&
                x.在庫数 == y.在庫数 &&
                x.包装単位 == y.包装単位 &&
                x.名称２ == y.名称２ &&
                x.一包単位量 == y.一包単位量 &&
                x.使用期限 == y.使用期限
                )
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(ExpDeadListEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ (ent.店名 == null ? 0 : ent.店名.GetHashCode());
            result = result ^ (ent.レセプト電算コード == null ? 0 : ent.レセプト電算コード.GetHashCode());
            result = result ^ (ent.医薬品名 == null ? 0 : ent.医薬品名.GetHashCode());
            result = result ^ (ent.在庫数.GetHashCode());
            result = result ^ (ent.包装単位 == null ? 0 : ent.包装単位.GetHashCode());
            result = result ^ (ent.名称２ == null ? 0 : ent.名称２.GetHashCode());
            result = result ^ (ent.一包単位量.GetHashCode());
            result = result ^ (ent.使用期限.GetHashCode());

            //Calculate the hash code for the product.
            return result;
        }

    }



    /// <summary>
    /// 店名、レセプト電算コード、医薬品名、包装単位一致、名称２ 、一包単位量
    /// </summary>
    public class ExpDeadListEntity店名医薬品名一致Comparer : IEqualityComparer<ExpDeadListEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ExpDeadListEntity x, ExpDeadListEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (
                x.店名 == y.店名 &&
                x.レセプト電算コード == y.レセプト電算コード &&
                x.医薬品名 == y.医薬品名 &&
                x.包装単位 == y.包装単位 &&
                x.名称２ == y.名称２ &&
                x.一包単位量 == y.一包単位量
                )
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(ExpDeadListEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ (ent.店名 == null ? 0 : ent.店名.GetHashCode());
            result = result ^ (ent.レセプト電算コード == null ? 0 : ent.レセプト電算コード.GetHashCode());
            result = result ^ (ent.医薬品名 == null ? 0 : ent.医薬品名.GetHashCode());
            result = result ^ (ent.包装単位 == null ? 0 : ent.包装単位.GetHashCode());
            result = result ^ (ent.名称２ == null ? 0 : ent.名称２.GetHashCode());
            result = result ^ (ent.一包単位量.GetHashCode());

            //Calculate the hash code for the product.
            return result;
        }



    }





}
