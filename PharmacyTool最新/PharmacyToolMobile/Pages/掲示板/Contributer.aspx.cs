using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PharmacyToolMobile.Service.File.Writer;
using PharmacyToolMobile.Service.File.Reader;
using PharmacyToolMobile.Util.ServiceUtil;


namespace PharmacyToolMobile.Pages.掲示板
{
    public partial class Contributer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ThreadName = (string)Request.QueryString["ThreadName"];
            var Title = (string)Request.QueryString["Title"];
            var No = (string)Request.QueryString["No"];
            var Kubun = (string)Request.QueryString["Kubun"];



            // スレッド名または記事Noが空の場合はBBS.aspxへ
            if (string.IsNullOrEmpty(ThreadName) || string.IsNullOrEmpty(No))
            {
                Response.Redirect("BBS.aspx");
            }

            // 修正の場合でかつタイトルがない場合はredirect
            if (Kubun != null && Kubun == "Mod" && string.IsNullOrEmpty(Title) == false)
            {
                Response.Redirect("BBS.aspx");
            }





            // 初回時
            if (!IsPostBack)
            {
                //List<FileUpload> fuList = new List<FileUpload>();
                //FileUpload fu = new FileUpload();
                //fuList.Add(fu);
                //Session["fuList"] = fuList;
                //Panel1.Controls.Add(fu);




                // 記事修正からリクエストが飛んできた場合はデータを格納
                if (Kubun != null)
                {
                    var ent = Session["修正用記事データ"] as 投稿Entity;
                    tbName.Text = ent.投稿者名;
                    tbTitle.Text = ent.Title;
                    tbComment.Text = ent.記事;
                    tbEMail.Text = ent.Email;
                    tbURL.Text = ent.HomepageUrl;
                    tbPassword.Text = ent.暗証キー;

                    Set文字色RadioButtonBy文字色(ent.文字色);

                    btnContribute.Text = "修正する";

                    // 修正の場合はNo、記 事 修 正と表示する
                    lbContributerHeaderForNo.Text = "";
                    lbContributerHeaderForTitle.Text = "下記の記事を修正します。";

                }
                else
                {
                    // 返信の場合はNoと本記事のタイトルを表示
                    if (string.IsNullOrEmpty(No) == false)
                    {
                        lbContributerHeaderForNo.Text = string.Format("No:{0}",No);
                    }
                    if (string.IsNullOrEmpty(Title) == false)
                    {
                        lbContributerHeaderForTitle.Text = Title;
                        lbContributerHeaderForFooter.Text = "　への返信をします。";
                    }



                }

            }
        }

        //protected void btnAdd_Click(object sender, System.EventArgs e)
        //{

        //    List<FileUpload> fuList = new List<FileUpload>();
        //    fuList = Session["fuList"] as List<FileUpload>;

        //    if (fuList == null)
        //    {
        //        fuList = new List<FileUpload>();
        //    }

        //    FileUpload fu = new FileUpload();
        //    fuList.Add(fu);
        //    Session["fuList"] = fuList;

        //    foreach (var f in fuList)
        //    {
        //        Panel1.Controls.Add(f);
        //    }

        //}

        private 掲示板リターンデータセット GetThreadData(string カテゴリ名, int グループNo)
        {
            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();
            return client.Open掲示板データ(カテゴリ名, グループNo);
        }

        private bool CheckPreContribute()
        {
            if (string.IsNullOrEmpty(tbName.Text))
            {
                return false;
            }

            if (string.IsNullOrEmpty(tbComment.Text))
            {
                return false;
            }

            // 返信するスレッド名の確認
            if (string.IsNullOrEmpty(Request.QueryString["ThreadName"]))
            {
                return false;
            }

            // 返信する記事Noの確認
            if (string.IsNullOrEmpty(Request.QueryString["No"]))
            {
                return false;
            }


            string 投稿タイトル = "";

            if (string.IsNullOrEmpty(tbTitle.Text))
            {
                投稿タイトル = "無題";
                // タイトルが入っていなかったら「無題」とする。
                //MessageBox.Show("タイトルを入力して下さい。", "確認", MessageBoxButton.OK);
                //return;
            }
            else
            {
                投稿タイトル = tbTitle.Text;
            }

            return true;

        }


        protected void btnContribute_Click(object sender, System.EventArgs e)
        {
            #region アップロード用スニペット

            //for (int i = 0; i < Request.Files.Count; i++)
            //{
            //    HttpPostedFile uploadFile = Request.Files[i];
            //    if (uploadFile.FileName != "")
            //    {
            //        uploadFile.SaveAs(@"c:\temptest\" + uploadFile.FileName);
            //        //Server.MapPath("./") + "file\" + System.IO.Path.GetFileName(uploadFile.FileName)) 
            //    }
            //}

            ////Session クリア
            //Session["fuList"] = null;
            #endregion


            if (!CheckPreContribute())
            {
                return;
            }

            string 投稿タイトル = "";

            if (string.IsNullOrEmpty(tbTitle.Text))
            {
                投稿タイトル = "無題";
                // タイトルが入っていなかったら「無題」とする。
                //MessageBox.Show("タイトルを入力して下さい。", "確認", MessageBoxButton.OK);
                //return;
            }
            else
            {
                投稿タイトル = tbTitle.Text;
            }

            string convertEmail = "";
            if (!string.IsNullOrEmpty(tbEMail.Text))
            {
                convertEmail = "mailto:" + tbEMail.Text;
            }
            List<string> 添付画像パスリスト = new List<string>();

            //for (int i = 0; i < Request.Files.Count; i++)
            //{
            //    HttpPostedFile uploadFile = Request.Files[i];
            //    if (uploadFile.FileName != "")
            //    {
            //        添付画像パスリスト.Add(uploadFile.FileName);
            //    }
            //}

            string pass = null;
            if (tbPassword.Text == "")
            {
                pass = "1234";
            }
            else
            {
                pass = tbPassword.Text;
            }

            文字色 文字色 = Check文字色();


            FileWriterClient client = PharmacyToolMobile.Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();

            var Kubun = (string)Request.QueryString["Kubun"];

            // 記事修正の場合
            if (Kubun != null)
            {
                // Fileの追加、変更、削除は対応しないので、そのまま
                var ent = Session["修正用記事データ"] as 投稿Entity;
                if (ent != null)
                {
                    client.掲示板データ書込(Request.QueryString["ThreadName"], tbTitle.Text, tbName.Text, tbComment.Text, 掲示板書込タイプ.記事修正, Request.QueryString["No"], tbURL.Text, tbEMail.Text, ent.添付ファイルlist, pass, 文字色);
                }

            }
            // 返信の場合
            else
            {
                client.掲示板データ書込(Request.QueryString["ThreadName"], 投稿タイトル, tbName.Text, tbComment.Text, 掲示板書込タイプ.返信投稿, Request.QueryString["No"], tbURL.Text, convertEmail, 添付画像パスリスト.ToArray(), pass, 文字色);

            }

            // 投稿画面へ遷移
            // IsRedirectFromContribute = 1 の時は BBS.aspxでそのThreadを読み込む
            Session["IsRedirectFromContribute"] = 1;
            Session["ThreadName"] = Request.QueryString["ThreadName"];
            Session["IsSearchData"] = null; //検索キーワードからの返信の場合もSessionはnull
            Session["修正用記事データ"] = null; //修正用記事データもSessionはnull

            Response.Redirect("BBS.aspx");



        }

        private 文字色 Check文字色()
        {
            if ((bool)rbBrown.Checked)
            {
                return Service.File.Writer.文字色.茶色;
            }
            else if ((bool)rbBlue.Checked)
            {
                return Service.File.Writer.文字色.青;
            }
            else if ((bool)rbBlack.Checked)
            {
                return Service.File.Writer.文字色.黒;
            }
            else if ((bool)rbGreen.Checked)
            {
                return Service.File.Writer.文字色.緑;
            }
            else if ((bool)rbOrange.Checked)
            {
                return Service.File.Writer.文字色.オレンジ;
            }
            else if ((bool)rbPink.Checked)
            {
                return Service.File.Writer.文字色.ピンク;
            }
            else if ((bool)rbPurple.Checked)
            {
                return Service.File.Writer.文字色.紫;
            }
            else if ((bool)rbRed.Checked)
            {
                return Service.File.Writer.文字色.赤;
            }
            else
            {
                return Service.File.Writer.文字色.茶色;
            }

        }


        private void Set文字色RadioButtonBy文字色(string moji)
        {
            if (moji == "茶色")
            {
                rbBrown.Checked = true;
            }
            else if (moji == "青")
            {
                rbBlue.Checked = true;
            }
            else if (moji == "黒")
            {
                rbBlack.Checked = true;
            }
            else if (moji == "緑")
            {
                rbGreen.Checked = true;
            }
            else if (moji == "オレンジ")
            {
                rbOrange.Checked = true;
            }
            else if (moji == "ピンク")
            {
                rbPink.Checked = true;
            }
            else if (moji == "紫")
            {
                rbPurple.Checked = true;
            }
            else if (moji == "赤")
            {
                rbRed.Checked = true;
            }
            else
            {
                //それ以外は茶色
                rbBrown.Checked = true;
            }

        }


    }
}