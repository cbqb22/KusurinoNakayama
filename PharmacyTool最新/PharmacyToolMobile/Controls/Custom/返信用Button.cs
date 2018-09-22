using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PharmacyToolMobile.Controls.Custom
{
    public class 返信用Button : Button
    {
        #region コンストラクタ
        public 返信用Button(string 返信投稿No,string 本記事Title)
            : base()
        {
            this.返信投稿No = 返信投稿No;
            this.本記事Title = 本記事Title;
        }

        #endregion

        #region フィールド
        private string _返信投稿No;

        public string 返信投稿No
        {
            get { return _返信投稿No; }
            set { _返信投稿No = value; }
        }


        private string _本記事Title;

        public string 本記事Title
        {
            get { return _本記事Title; }
            set { _本記事Title = value; }
        }

        #endregion

    }
}