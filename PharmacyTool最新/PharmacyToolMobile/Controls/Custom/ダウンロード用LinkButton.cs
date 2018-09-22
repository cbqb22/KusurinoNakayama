using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace PharmacyToolMobile.Controls.Custom
{
    public class ダウンロード用LinkButton : LinkButton
    {
        private string _投稿ID;

        public string 投稿ID
        {
            get { return _投稿ID; }
            set { _投稿ID = value; }
        }

        private string _スレッド名;

        public string スレッド名
        {
            get { return _スレッド名; }
            set { _スレッド名 = value; }
        }

        public ダウンロード用LinkButton(string 投稿ID, string スレッド名)
            : base()
        {
            this.投稿ID = 投稿ID;
            this.スレッド名 = スレッド名;
        }
    }
}