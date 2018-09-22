using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace OASystem.Model.Entity
{
    public class PriorityMoveEntity : INotifyPropertyChanged
    {

        private string _店舗名;

        public string 店舗名
        {
            get { return _店舗名; }
            set { _店舗名 = value; }
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

        private string _コメント;

        public string コメント
        {
            get { return _コメント; }
            set 
            {
                _コメント = value;
                FirePropertyChanged("コメント");
            }
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
}
