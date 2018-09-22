using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManagement.ViewModel
{
    public class ExceptionMedicineEntity
    {
        private string _医薬品名称;

        public string 医薬品名称
        {
            get { return _医薬品名称; }
            set { _医薬品名称 = value; }
        }

        private string _レセプト電算コード;

        public string レセプト電算コード
        {
            get { return _レセプト電算コード; }
            set { _レセプト電算コード = value; }
        }
    }
}
