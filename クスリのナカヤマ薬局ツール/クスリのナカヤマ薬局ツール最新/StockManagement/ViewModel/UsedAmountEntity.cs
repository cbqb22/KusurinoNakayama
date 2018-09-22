using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManagement.ViewModel
{
    public class UsedAmountEntity
    {
        private string _storeName;

        public string StoreName
        {
            get { return _storeName; }
            set { _storeName = value; }
        }

        private DateTime _usedDate;

        public DateTime UsedDate
        {
            get { return _usedDate; }
            set { _usedDate = value; }
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

        private double _usedAmout;

        public double UsedAmount
        {
            get { return _usedAmout; }
            set { _usedAmout = value; }
        }
        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private string _code;

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }


        //private bool _代替区分;

        //public bool 代替区分
        //{
        //    get { return _代替区分; }
        //    set { _代替区分 = value; }
        //}

        //private bool _後発品区分;

        //public bool 後発品区分
        //{
        //    get { return _後発品区分; }
        //    set { _後発品区分 = value; }
        //}

    }
}
