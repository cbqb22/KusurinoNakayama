using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OASystem.Model.Entity
{
    public class BalancingAccountsEntity
    {
        private string _卸ＶＡＮコード;

        public string 卸ＶＡＮコード
        {
            get { return _卸ＶＡＮコード; }
            set { _卸ＶＡＮコード = value; }
        }

        private string _帳合先名称;

        public string 帳合先名称
        {
            get { return _帳合先名称; }
            set { _帳合先名称 = value; }
        }

        private int _表示順;

        public int 表示順
        {
            get { return _表示順; }
            set { _表示順 = value; }
        }

        public BalancingAccountsEntity DeepCopy()
        {
            BalancingAccountsEntity ent = new BalancingAccountsEntity();
            ent.卸ＶＡＮコード = this.卸ＶＡＮコード;
            ent.帳合先名称 = this.帳合先名称;
            ent.表示順 = this.表示順;

            return ent;
        }

    }
}
