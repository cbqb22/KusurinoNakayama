using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManagement.ViewModel
{
    public class DeadStockEntity
    {
        private string _storeName;

        public string StoreName
        {
            get { return _storeName; }
            set {
                _storeName = value; }
        }

        private string _code;

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private double _stockAmount;

        public double StockAmount
        {
            get { return _stockAmount; }
            set { _stockAmount = value; }
        }

        private DateTime expireDate;

        public DateTime ExpireDate
        {
            get { return expireDate; }
            set { expireDate = value; }
        }

        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
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



    }

    // Custom comparer for the Product class
    public class DeadStockEntityComparer : IEqualityComparer<DeadStockEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(DeadStockEntity x, DeadStockEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (
                x.Name == y.Name &&
                x.Price == y.Price &&
                x.StockAmount == y.StockAmount &&
                x.StoreName == y.StoreName &&
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

        public int GetHashCode(DeadStockEntity ent)
        {

            //Check whether the object is null
            if (Object.ReferenceEquals(ent, null)) return 0;

            int result = 0;


            //Get hash code for the Name field if it is not null.
            result = result ^ (ent.Name == null ? 0 : ent.Name.GetHashCode());
            result = result ^ (ent.Price.GetHashCode());
            result = result ^ ent.StockAmount.GetHashCode();
            result = result ^ (ent.StoreName == null ? 0 : ent.StoreName.GetHashCode());
            result = result ^ ent.ExpireDate.GetHashCode();
            result = result ^ (ent.Name2 == null ? 0 : ent.Name2.GetHashCode());
            result = result ^ ent.OneDoseAmount.GetHashCode();

            //Calculate the hash code for the product.
            return result;
        }

    }

}
