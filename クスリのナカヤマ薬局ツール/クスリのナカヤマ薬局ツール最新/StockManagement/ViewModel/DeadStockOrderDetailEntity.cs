using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManagement.ViewModel
{
    public class DeadStockOrderDetailEntity
    {
        private int _no;

        public int No
        {
            get { return _no; }
            set { _no = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _totalUsedAmount;

        public double TotalUsedAmount
        {
            get { return _totalUsedAmount; }
            set { _totalUsedAmount = value; }
        }
        private double _deadAmount;

        public double DeadAmount
        {
            get { return _deadAmount; }
            set { _deadAmount = value; }
        }
        private DateTime _expireDate;

        public DateTime ExpireDate
        {
            get { return _expireDate; }
            set { _expireDate = value; }
        }
        private bool _acceptable;

        public bool Acceptable
        {
            get { return _acceptable; }
            set { _acceptable = value; }
        }

        private string _partnerStoreName;

        public string PartnerStoreName
        {
            get { return _partnerStoreName; }
            set { _partnerStoreName = value; }
        }

        /// <summary>
        /// 名称２
        /// </summary>
        private string _name2;

        public string Name2
        {
            get { return _name2; }
            set { _name2 = value; }
        }

        /// <summary>
        /// １包単位量
        /// </summary>
        private double _OneDoseAmount;

        public double OneDoseAmount
        {
            get { return _OneDoseAmount; }
            set { _OneDoseAmount = value; }
        }


        private string _NameAndName2Docking;
        public string NameAndName2Docking
        {
            get
            {
                if (string.IsNullOrEmpty(_name2))
                {
                    return _name;

                }

                return _name + "（" + _name2 + "）";
            }
        }


        /// <summary>
        /// 相対参照先RowNo
        /// ALLシートの該当するRowNo
        /// </summary>
        private int _相対参照先RowNo;
        public int 相対参照先RowNo
        {
            get { return _相対参照先RowNo; }
            set { _相対参照先RowNo = value; }
        }


        public string 相対参照先へ変換(string column, int No)
        {
            return string.Format("=IF(ISBLANK(ALL!{0}{1}), \"\",ALL!{0}{1})", column, No);

        }



    }

    // Custom comparer for the Product class
    public class DeadStockOrderDetailEntityComparerByOnlyNameDeadAmountExpireDateAcceptable : IEqualityComparer<DeadStockOrderDetailEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(DeadStockOrderDetailEntity x, DeadStockOrderDetailEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (x.Name == y.Name &&
                x.Acceptable == y.Acceptable &&
                x.DeadAmount == y.DeadAmount &&
                x.ExpireDate == y.ExpireDate &&
                x.Name2 == y.Name2 &&
                x.OneDoseAmount == y.OneDoseAmount
                )
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(DeadStockOrderDetailEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ (ent.Name == null ? 0 : ent.Name.GetHashCode());
            result = result ^ ent.Acceptable.GetHashCode();
            result = result ^ ent.DeadAmount.GetHashCode();
            result = result ^ ent.ExpireDate.GetHashCode();
            result = result ^ (ent.Name2 == null ? 0 : ent.Name2.GetHashCode());
            result = result ^ ent.OneDoseAmount.GetHashCode();

            //Calculate the hash code for the product.
            return result;
        }

    }


    // Custom comparer for the Product class
    public class DeadStockOrderDetailEntityComparer : IEqualityComparer<DeadStockOrderDetailEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(DeadStockOrderDetailEntity x, DeadStockOrderDetailEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (x.Name == y.Name &&
                x.PartnerStoreName == y.PartnerStoreName &&
                x.TotalUsedAmount == y.TotalUsedAmount &&
                x.Acceptable == y.Acceptable &&
                x.DeadAmount == y.DeadAmount &&
                x.ExpireDate == y.ExpireDate &&
                x.Name2 == y.Name2 &&
                x.OneDoseAmount == y.OneDoseAmount
                ) 
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(DeadStockOrderDetailEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ ( ent.Name == null ? 0 : ent.Name.GetHashCode());
            result = result ^ (ent.PartnerStoreName == null ? 0 : ent.PartnerStoreName.GetHashCode());
            result = result ^ ent.TotalUsedAmount.GetHashCode();
            result = result ^ ent.Acceptable.GetHashCode();
            result = result ^ ent.DeadAmount.GetHashCode();
            result = result ^ ent.ExpireDate.GetHashCode();
            result = result ^ (ent.Name2 == null ? 0 : ent.Name2.GetHashCode());
            result = result ^ ent.OneDoseAmount.GetHashCode();

            //Calculate the hash code for the product.
            return result;
        }

    }
}
