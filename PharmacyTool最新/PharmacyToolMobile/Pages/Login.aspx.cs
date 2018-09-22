using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PharmacyToolMobile.Util.ServiceUtil;
using PharmacyToolMobile.Service.DAO.PharmacyTool;


namespace PharmacyToolMobile.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        protected void btnLogin_Click(object sender, System.EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
            {
                //認証失敗時時→掲示板画面へRedirect
                Response.Redirect("../Error401.html");
            }

            //認証
            LoginCheckClient client = ReferenceCreater.GetログインチェックClient();
            var チェック結果 = client.LoginCheck実行(Login1.UserName, Login1.Password).ToList();
            if (チェック結果 == null)
            {
                //認証失敗時時→掲示板画面へRedirect
                Response.Redirect("../Error401.html");
            }

            if (チェック結果.Count != 1)
            {
                //認証失敗時時→掲示板画面へRedirect
                Response.Redirect("../Error401.html");
            }

            // ログイン成功時にアクセスカウントアップ
            // Sessionへ格納:アクセスカウント
            var Accessclient = ReferenceCreater.GetAccessManagementClient();
            var 結果 = Accessclient.Doアクセス数カウントアップ取得();
            if (結果.取得成功か && 結果.エラーメッセージ == "")
            {
                Session["アクセスカウント"] = 結果.アクセス数;
            }

            //Sessionへ格納
            //1.ユーザー名(店舗名)
            Session["表示ユーザー名"] = チェック結果[0].表示名称;

            //認証成功時→掲示板画面へRedirect
            Response.Redirect("./掲示板/BBS.aspx");


        }

    }
}