using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OASystem.Model.Enum;
using System.ComponentModel.DataAnnotations;

namespace OASystem.Model.Entity
{
    public class IndividualBasedManagementMedicineEntity
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

    }

}
