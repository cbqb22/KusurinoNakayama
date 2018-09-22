using System;
using System.Collections.Generic;
using System.Text;

namespace OASystem.Model.Entity
{
    public class SEND01DATEntity
    {
        private string _卸VANコード;

        public string 卸VANコード
        {
            get { return _卸VANコード; }
            set { _卸VANコード = value; }
        }

        private string _JANコード;

        public string JANコード
        {
            get { return _JANコード; }
            set { _JANコード = value; }
        }

        private double _数量;

        public double 数量
        {
            get { return _数量; }
            set { _数量 = value; }
        }

        private string _レセ発注伝票No;

        public string レセ発注伝票No
        {
            get { return _レセ発注伝票No; }
            set { _レセ発注伝票No = value; }
        }

        // DATの中の医薬品の順番
        private int _注文番号;
        public int 注文番号
        {
            get { return _注文番号; }
            set { _注文番号 = value; }
        }

    }
}
