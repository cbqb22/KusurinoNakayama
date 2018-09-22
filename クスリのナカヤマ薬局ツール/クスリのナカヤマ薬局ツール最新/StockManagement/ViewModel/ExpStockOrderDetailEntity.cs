using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManagement.ViewModel
{
    public class ExpStockOrderDetailEntity
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

        // 他店舗使用量
        private double _totalUsedAmount;

        public double TotalUsedAmount
        {
            get { return _totalUsedAmount; }
            set { _totalUsedAmount = value; }
        }

        // 自店舗使用量
        private double _totalUsedAmountOwn;

        public double TotalUsedAmountOwn
        {
            get { return _totalUsedAmountOwn; }
            set { _totalUsedAmountOwn = value; }
        }

        // 自店舗デッド数量
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
            return string.Format("=IF(ISBLANK(ALL!{0}{1}), \"\",ALL!{0}{1})",column,No);

        }
    }


    // Custom comparer for the Product class
    public class ExpStockOrderDetailEntityComparerByOnlyNameDeadAmountExpireDateAcceptable : IEqualityComparer<ExpStockOrderDetailEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ExpStockOrderDetailEntity x, ExpStockOrderDetailEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (x.Name == y.Name &&
                x.Acceptable == y.Acceptable &&
                x.DeadAmount == y.DeadAmount &&
                x.ExpireDate == y.ExpireDate
                )
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(ExpStockOrderDetailEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ (ent.Name == null ? 0 : ent.Name.GetHashCode());
            result = result ^ ent.Acceptable.GetHashCode();
            result = result ^ ent.DeadAmount.GetHashCode();
            result = result ^ ent.ExpireDate.GetHashCode();

            //Calculate the hash code for the product.
            return result;
        }

    }


    // Custom comparer for the Product class
    public class ExpStockOrderDetailEntityComparer : IEqualityComparer<ExpStockOrderDetailEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ExpStockOrderDetailEntity x, ExpStockOrderDetailEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (x.Name == y.Name &&
                x.PartnerStoreName == y.PartnerStoreName &&
                x.TotalUsedAmount == y.TotalUsedAmount &&
                x.Acceptable == y.Acceptable &&
                x.DeadAmount == y.DeadAmount &&
                x.ExpireDate == y.ExpireDate
                ) 
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(ExpStockOrderDetailEntity ent)
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

            //Calculate the hash code for the product.
            return result;
        }

    }
}
