using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManagement.ViewModel
{
    public class ExpStockEntity
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

        private string _name2;

        public string Name2
        {
            get { return _name2; }
            set { _name2 = value; }
        }

        
        private string _nameAndName2;
        public string NameAndName2
        {
            get
            {
                if (string.IsNullOrEmpty(_name2))
                {
                    return _name;
                }
                else
                {
                    return string.Format("{0} / {1}",_name,_name2);
                }
            }
            set { _nameAndName2 = value; }
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
    }

    // Custom comparer for the Product class
    public class ExpStockEntityComparer : IEqualityComparer<ExpStockEntity>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ExpStockEntity x, ExpStockEntity y)
        {


            //Check whether the compared objects reference the same data.
            if (x.Name == y.Name &&
                x.Price == y.Price &&
                x.StockAmount == y.StockAmount &&
                x.StoreName == y.StoreName &&
                x.ExpireDate == y.ExpireDate
                )
            {
                return true;

            }

            return false;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(ExpStockEntity ent)
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

            //Calculate the hash code for the product.
            return result;
        }

    }

}
