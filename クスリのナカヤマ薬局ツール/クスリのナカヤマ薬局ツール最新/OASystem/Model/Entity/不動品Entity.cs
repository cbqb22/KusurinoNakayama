using System;
using System.Collections.Generic;
using System.Text;

namespace OASystem.Model.Entity
{
    public class 不動品Entity
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

        private DateTime _使用期限;

        public DateTime 使用期限
        {
            get { return _使用期限; }
            set { _使用期限 = value; }
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



        private DateTime _最終更新日時;

        public DateTime 最終更新日時
        {
            get { return _最終更新日時; }
            set { _最終更新日時 = value; }
        }

    }



}
