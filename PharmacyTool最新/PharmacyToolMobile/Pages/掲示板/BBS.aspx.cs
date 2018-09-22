using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using PharmacyToolMobile.Service.File.Reader;
using PharmacyToolMobile.Service.File.Writer;
using PharmacyToolMobile.Util.ServiceUtil;
using PharmacyToolMobile.Controls.Custom;
using PharmacyToolMobile.Service.DAO.PharmacyTool.店舗;
using PharmacyToolMobile.Entity;


namespace PharmacyToolMobile.Pages.掲示板
{
    public partial class BBS : System.Web.UI.Page
    {

        #region フィールド

        //テキストファイル	.txt	text/plain
        //HTML	.htm,.html	text/html
        //CSVファイル	.csv	text/comma-separated-values
        //TSVファイル	.txt, .tsv	text/tab-separated-values
        //XMLファイル	.xml	text/xml
        //Microsoft Wordファイル	.doc, .docx	application/msword
        //Microsoft Excelファイル	.xls, xlsx	application/msexcel
        //Microsoft PowerPointファイル	.ppt, pptx	application/mspowerpoint
        //PDFファイル	.pdf	application/pdf
        //ビットマップファイル	.bmp	image/bmp
        //GIFファイル	.gif	image/gif
        //JPEGファイル	.jpg	image/jpeg
        //PNGファイル.png	.png	image/png
        //ZIPファイル	.zip	application/zip
        //LZHファイル	.lzh	application/lha
        //スタイルシートファイル	.css	text/css
        //JavaScriptファイル	.js	text/javascript
        //VBScriptファイル	.vbs	text/vbscript
        //実行ファイル	.exe	application/octet-stream
        /// <summary>
        /// MIMETYPE
        /// </summary>
        /// 
        private Dictionary<string, string> MIMETYPE_DIC = new Dictionary<string, string>() 
        {
            {"txt","text/plain"},
            {"htm","text/html"},
            {"html","text/html"},
            {"csv","text/comma-separated-values"},
            {"tsv","text/tab-separated-values"},
            {"xml","text/xml"},
            {"doc","application/msword"},
            {"docx","application/msword"},
            {"xls","application/msexcel"},
            {"xlsx","application/msexcel"},
            {"ppt","application/mspowerpoint"},
            {"pptx","application/mspowerpoint"},
            {"pdf","application/pdf"},
            {"bmp","image/bmp"},
            {"gif","image/gif"},
            {"jpg","image/jpeg"},
            {"png","image/png"},
            {"zip","application/zip"},
            {"lzh","application/lha"},
            {"css","text/css"},
            {"js","text/javascript"},
            {"vbs","text/vbscript"},
            {"exe","application/octet-stream"},
        };

        #endregion


        protected void Page_Init(object sender, EventArgs e)
        {


            // ユーザー名がない場合はログイン画面へRedirect
            if (Session["表示ユーザー名"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            //おなまえにセット 変更保持はPage_Unloadで
            tbName.Text = Session["表示ユーザー名"].ToString();

            // 再ログインにはしない。
            if (Session["アクセスカウント"] == null)
            {
                //Response.Redirect("../Login.aspx");
            }
            else
            {
                lbAccessCount.Text = Session["アクセスカウント"].ToString().PadLeft(8, '0');
            }

            if (Session["店舗名List"] == null)
            {
                SetSession店舗名List();
            }

            // MessageShowがあれば表示
            if (Session["ShowMessage"] != null)
            {
                MessageShowEntity mse = Session["ShowMessage"] as MessageShowEntity;
                if (mse != null)
                {
                    string script =
                                    "<script language=javascript>" +
                                    "window.alert('" + mse.Message + "')" +
                                    "</script>";
                    Response.Write(script);
                }

                //ShowMessageをクリア
                Session["ShowMessage"] = null;

            }

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            // 初回読み込み
            if (!IsPostBack)
            {
                // もしContributer.aspxからのRedirectならば

                if (Session["IsRedirectFromContribute"] != null && Session["ThreadName"] != null)
                {
                    if (Session["IsRedirectFromContribute"].ToString() == "1" && Session["ThreadName"].ToString() != "")
                    {
                        GetThreadName(IsPostBack, Session["ThreadName"].ToString());
                        // Sessionをクリア
                        Session["IsRedirectFromContribute"] = null;
                        Session["ThreadName"] = null;
                    }
                }
                else
                {
                    GetThreadName(IsPostBack);
                    //SetUploadFiles();
                }

            }
            // ポストバック時
            else
            {
                //// LinkButtonにイベントを紐付
                //Panel pnLinkButton2 = Session["pnLinkButton"] as Panel;
                //for (int i = 0; i < pnLinkButton2.Controls.Count; i++)
                //{
                //    var b = pnLinkButton2.Controls[i] as LinkButton;
                //    if (b == null)
                //    {
                //        continue;
                //    }

                //    //b.Click -= new EventHandler(lkb_Click);
                //    b.Click += new EventHandler(lkb_Click);
                //}


                //Panel3.Controls.Clear();
                //Panel3.Controls.Add((Panel)Session["pnLinkButton"]);

                if (Session["IsSearchData"] != null && (bool)Session["IsSearchData"])
                {
                    SetThreadDataFromKeyword(tbSearch.Text);
                }
                else
                {
                    int PageNumber = (int)Session["PageNumber"];
                    string ThreadName = Session["ThreadName"].ToString();
                    SetThreadData(GetThreadData(ThreadName, PageNumber));
                }


                // とりあえず、Sessionデータからスレッドを再生
                //Panel1.Controls.Clear();
                //Panel1.Controls.Add((Panel)Session["pnThreadData"]);



            }
        }

        /// <summary>
        /// Unload時にPageがもっているデータをSessionに格納
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            //Session["pnThreadData"] = pn;
            //Session["pnLinkButton"] = pnLinkButton;
            Session["ThreadName"] = ddlスレッド選択.SelectedValue;
            //Session["PageNumber"]はSetThreadDataした時に格納
            Session["表示ユーザー名"] = tbName.Text;


        }


        protected void ddlスレッド選択_SelectedChanged(object sender, EventArgs e)
        {
            SetThreadData(GetThreadData(ddlスレッド選択.SelectedValue, 1));
            Session["IsSearchData"] = null;
        }


        void lkb_Click(object sender, EventArgs e)
        {
            int PageNumber = (int)Session["PageNumber"];
            LinkButton lkb = sender as LinkButton;

            int result = -1;
            if (lkb.Text == "次")
            {
                result = PageNumber + 1;
            }
            else if (lkb.Text == "前")
            {
                result = PageNumber - 1;
            }
            else if (lkb.Text == "＞＞")
            {
                result = (int)Session["MaxPageNumber"];
            }
            else if (lkb.Text == "＜＜")
            {
                result = 1;
            }
            else
            {
                if (int.TryParse(lkb.Text, out result) == false)
                {
                    return;
                }
            }

            // 万が一０となったら1ページ目を表示する。
            if (result <= 0)
            {
                result = 1;
            }

            SetThreadData(GetThreadData(ddlスレッド選択.SelectedValue, result));
        }

        void lkbDownLoad_Click(object sender, EventArgs e)
        {
            ダウンロード用LinkButton lkb = sender as ダウンロード用LinkButton;
            string FileName = lkb.Text;
#if DEBUG
            // ダウンロードテスト用
            string URL = "http://www.kusurinonakayama.jp/PharmacyTool/ClientBin/掲示板資料/全般/1015/11032601.PDF";

#elif NAKAYAMA

            string URL = "http://www.kusurinonakayama.jp/PharmacyTool/ClientBin/掲示板資料/" + lkb.スレッド名 + "/" + lkb.投稿ID + "/" + lkb.Text;

#else
            // ダウンロードテスト用
            string URL = "http://www.kusurinonakayama.jp/PharmacyTool/ClientBin/掲示板資料/全般/1015/11032601.PDF";

#endif
            Response.Redirect(URL);

            #region スニペット
            //            ダウンロード用LinkButton lkb = sender as ダウンロード用LinkButton;
            //            string FileName = lkb.Text;

            //            string MIMETYPE = null;

            //            var sepa = FileName.Split('.');
            //            if (sepa.Count() != 0)
            //            {
            //                MIMETYPE = MIMETYPE_DIC[sepa[sepa.Count() - 1].ToLower()];
            //            }
            //            else
            //            {
            //                MIMETYPE = MIMETYPE_DIC["exe"];
            //            }

            //            string FilePathBase = null;

            //#if DEBUG
            //            FilePathBase = PharmacyToolMobile.Properties.Settings.Default.Upload掲示板FileRootPathDEBUG;

            //#elif NAKAYAMA
            //            FilePathBase = PharmacyToolMobile.Properties.Settings.Default.Upload掲示板FileRootPathNAKAYAMA;
            //#else
            //            FilePathBase = PharmacyToolMobile.Properties.Settings.Default.Upload掲示板FileRootPathDEBUG;
            //#endif
            //            if (FilePathBase == null)
            //            {
            //                throw new Exception("FilePathBaseが指定されていません。");
            //            }

            //            string FilePath = Path.Combine(FilePathBase, (lkb.スレッド名 + "\\" + lkb.投稿ID + "\\" + lkb.Text));

            //            //if (FilePath == "")
            //            //{
            //            //    return;
            //            //}

            //            //// Response情報クリア
            //            //Response.ClearContent();
            //            //// HTTPヘッダー情報設定
            //            //Response.AddHeader("Content-Disposition", "attachment;filename=①ファイル名");
            //            //Response.ContentType = ②MIME;
            //            //// ファイル書込(データによりResponse.WriteFile()、Response.Write()、Response.BinaryWrite()を使い分ける。)
            //            //Response.WriteFile(sFilePath);
            //            //// レスポンス終了
            //            //Response.End();

            //            Response.ContentType = MIMETYPE;
            //            Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
            //            Response.Flush();
            //            Response.WriteFile(FilePath);
            //            Response.End();

            #endregion
        }

        protected void btnスレッド移動_Click(object sender, System.EventArgs e)
        {
            SetThreadData(GetThreadData(ddlスレッド選択.SelectedValue, 1));
            Session["IsSearchData"] = null;
        }

        protected void btn新規投稿_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("Contributer.aspx?Type=New&ThreadName=" + ddlスレッド選択.SelectedValue);
        }

        protected void btn返信_Click(object sender, System.EventArgs e)
        {
            返信用Button btn = sender as 返信用Button;

            string ThreadName = ddlスレッド選択.SelectedValue;
            string No = btn.返信投稿No;
            string Title = btn.本記事Title;
            Response.Redirect("Contributer.aspx?ThreadName=" + ThreadName + "&No=" + No + "&Title=" + Title);
        }

        protected void btn掲示板_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("BBS.aspx");
            Session["IsSearchData"] = null;
        }

        protected void btn在庫管理_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("../在庫管理/StockManagement.aspx");
            Session["IsSearchData"] = null;
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
        //        Panel2.Controls.Add(f);
        //    }
        //}


        protected void btn処理送信_Click(object sender, System.EventArgs e)
        {
            Button btn = sender as Button;
            if(btn == null)
            {
                return;
            }

            if (tb記事No.Text == "")
            {
                return;
            }

            if (tb処理暗証キー.Text == "")
            {
                return;
            }


            // 修正処理
            if (ddl処理.SelectedIndex == 0)
            {
                // パスワード確認
                FileReaderClient client = ReferenceCreater.GetFileReaderClient();
                var rEnt = client.掲示板記事修正確認チェック(ddlスレッド選択.SelectedValue, tb記事No.Text, tb処理暗証キー.Text, false);
                if (!rEnt.暗証キーチェック成功 || rEnt.エラーメッセージ != null)
                {
                    return;
                }

                Session["修正用記事データ"] = rEnt.記事データ;
                // Kubun = Mod : Modify
                Response.Redirect("Contributer.aspx?ThreadName=" + ddlスレッド選択.SelectedValue + "&No=" + tb記事No.Text + "&Kubun=Mod");
            }
            // 削除処理
            else if (ddl処理.SelectedIndex == 1)
            {
                FileWriterClient client2 = ReferenceCreater.GetFileWriterClient();
                var 結果メッセージ = client2.掲示板データ削除(ddlスレッド選択.SelectedValue, tb記事No.Text, tb処理暗証キー.Text, false);

                //string script =
                //                "<script language=javascript>" +
                //                "window.alert('" + 結果メッセージ + "')" +
                //                "</script>";
                //Response.Write(script);

                // Redirect後のMessageShow
                MessageShowEntity mse = new MessageShowEntity();
                mse.Message = 結果メッセージ;
                Session["ShowMessage"] = mse;
            }
            else
            {
                return;
            }


            // Redirect用のセッション
            Session["IsRedirectFromContribute"] = 1;
            Session["ThreadName"] = Request.QueryString["ThreadName"];

            // 掲示板画面へ遷移
            Response.Redirect("BBS.aspx");



        }

        protected void btnContribute_Click(object sender, System.EventArgs e)
        {
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

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile uploadFile = Request.Files[i];
                if (uploadFile.FileName != "")
                {
                    添付画像パスリスト.Add(uploadFile.FileName);
                }
            }

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
            client.掲示板データ書込(ddlスレッド選択.SelectedValue, 投稿タイトル, tbName.Text, tbComment.Text, 掲示板書込タイプ.新規投稿, "", tbURL.Text, convertEmail, 添付画像パスリスト.ToArray(), pass, 文字色);


#if DEBUG
            string UploadFolderPath = PharmacyToolMobile.Properties.Settings.Default.Upload掲示板FileRootPathDEBUG;
#elif NAKAYAMA
            string UploadFolderPath = PharmacyToolMobile.Properties.Settings.Default.Upload掲示板FileRootPathNAKAYAMA;
#else
            throw new Exception("実装していません");
#endif

            //UploadFolderPath = Path.Combine(UploadFolderPath,ddlスレッド選択.SelectedValue + "\\" + );


            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile uploadFile = Request.Files[i];
                if (uploadFile.FileName != "")
                {
                    uploadFile.SaveAs(UploadFolderPath + uploadFile.FileName);
                    //Server.MapPath("./") + "file\" + System.IO.Path.GetFileName(uploadFile.FileName)) 
                }
            }

            // Redirect用のセッション
            Session["IsRedirectFromContribute"] = 1;
            Session["ThreadName"] = Request.QueryString["ThreadName"];
            //Session クリア
            Session["IsSearchData"] = null;
            //Session["fuList"] = null;


            // 掲示板画面へ遷移
            Response.Redirect("BBS.aspx");

        }
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            SetThreadDataFromKeyword(tbSearch.Text);



        }

        private void SetThreadDataFromKeyword(string keyword)
        {
            FileReaderClient client = ReferenceCreater.GetFileReaderClient();
            var rdataset = client.Open掲示板データキーワード検索(keyword, ddlスレッド選択.SelectedValue);
            rdataset.作成グループNo = 1; // キーワード検索では1を返すことにする。//本来ならば、svc側に書くべきだが。やっつけで。

            SetThreadData(rdataset);

            // SessionにIsSearchDataを格納
            // クリアのタイミングは、新規投稿・掲示板ボタンクリック、在庫管理クリック、スレッド移動の時、返信画面で投稿した時
            Session["IsSearchData"] = true;

        }

        //private void SetUploadFiles()
        //{
        //    List<FileUpload> fuList = new List<FileUpload>();
        //    FileUpload fu = new FileUpload();
        //    fuList.Add(fu);
        //    Session["fuList"] = fuList;

        //    Panel2.Controls.Add(fu);

        //}
        private void GetThreadName(bool IsPostBack)
        {
            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();

            var thEnt = client.GetThreadTitles().ToList();

            ddlスレッド選択.DataSource = thEnt;
            ddlスレッド選択.DataValueField = "スレッド名";
            ddlスレッド選択.DataBind();

            // 初回ならば一番上のデータを取得
            if (1 <= thEnt.Count)
            {
                if (!IsPostBack)
                {
                    SetThreadData(GetThreadData(thEnt[0].スレッド名, 1));
                }
            }


        }

        /// <summary>
        /// 表示するThreadNameも指定する場合
        /// </summary>
        /// <param name="IsPostBack"></param>
        /// <param name="ThreadName"></param>
        private void GetThreadName(bool IsPostBack, string ThreadName)
        {
            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();

            var thEnt = client.GetThreadTitles().ToList();

            ddlスレッド選択.DataSource = thEnt;
            ddlスレッド選択.DataValueField = "スレッド名";
            ddlスレッド選択.DataBind();

            ddlスレッド選択.SelectedValue = ThreadName;

            // 初回ならば一番上のデータを取得
            if (1 <= thEnt.Count)
            {
                if (!IsPostBack)
                {
                    SetThreadData(GetThreadData(ThreadName, 1));
                }
            }


        }


        private 掲示板リターンデータセット GetThreadData(string カテゴリ名, int グループNo)
        {
            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();

            return client.Open掲示板データ(カテゴリ名, グループNo);

        }
        private void SetPageSelector(掲示板リターンデータセット dataset)
        {

            pnLinkButton.Controls.Clear();

            // ページ選択ボタンを追加
            List<string> pages = new List<string>();
            Stack<string> backBages = new Stack<string>();
            if (dataset.メイン記事数 <= 0)
            {
                //ページセレクターは作らずスルーする キーワード検索時用
            }
            else
            {
                double d = (double)dataset.メイン記事数 / 10;
                int maxpage = ((int)Math.Ceiling(d) < dataset.作成グループNo + 9) ? (int)Math.Ceiling(d) : dataset.作成グループNo + 9;

                if (dataset.作成グループNo != 1)
                {
                    // (int)Math.Ceiling(d) < dataset.作成グループNo + 9 がtrueの時
                    if ((int)Math.Ceiling(d) < dataset.作成グループNo + 9)
                    {
                        // 差分を前に追加する
                        int 前に追加する差分数 = dataset.作成グループNo + 9 - (int)Math.Ceiling(d);
                        for (int i = 1; ; i++)
                        {
                            // 1になったら最後
                            if ((dataset.作成グループNo - i) == 1)
                            {
                                backBages.Push((dataset.作成グループNo - i).ToString());
                                break;
                            }
                            // 追加する差分数になったら最後
                            else if (i == 前に追加する差分数)
                            {
                                backBages.Push((dataset.作成グループNo - i).ToString());
                                break;
                            }

                            backBages.Push((dataset.作成グループNo - i).ToString());
                        }
                    }
                }

                // スタックから順番に取り出す
                foreach (var stack in backBages)
                {
                    pages.Add(stack);
                }

                for (int i = dataset.作成グループNo; i <= maxpage; i++)
                {
                    pages.Add(i.ToString());
                }

                string Previous = "前";
                string Next = "次";
                string First = "＜＜";
                string End = "＞＞";

                // Link作成
                if (pages == null || pages.Count == 0)
                {
                    throw new Exception("pagesが設定されてません。");
                }

                if (dataset.作成グループNo <= 1 ? false : true)
                {
                    pages.Insert(0, First);
                    pages.Insert(1, Previous);
                }

                if (dataset.作成グループNo * 10 < dataset.メイン記事数 ? true : false)
                {
                    pages.Insert(pages.Count, Next);
                    pages.Insert(pages.Count, End);
                }


                foreach (var str in pages)
                {
                    LinkButton lkb = new LinkButton();
                    lkb.Text = str;
                    lkb.Click += new EventHandler(lkb_Click);
                    lkb.ID = "pageselector" + str;

                    if (str == dataset.作成グループNo.ToString())
                    {
                        pnLinkButton.Controls.Add(new LiteralControl("<span style=\"font-weight:bold;\">"));
                        lkb.ForeColor = System.Drawing.Color.Red;
                    }

                    pnLinkButton.Controls.Add(lkb);
                    pnLinkButton.Controls.Add(new LiteralControl("</span>"));
                    pnLinkButton.Controls.Add(new LiteralControl("　"));

                }
            }


        }
        private void SetThreadData(掲示板リターンデータセット rdataset)
        {
            pn.Controls.Clear();

            foreach (var data in rdataset.掲示板データ)
            {
                foreach (var data2 in data)
                {
                    var 投稿Ent = data2.Key;

                    DateTime dt;
                    if (DateTime.TryParse(投稿Ent.投稿日, out dt) == false)
                    {
                        dt = new DateTime();
                    }



                    string html =
                        "<table width=\"100%\" class=\"tbContribute\">" +

                            //投稿
                            "<tr>" +
                                "<td colspan=\"5\">"
                                    + "<span class=\"MainNo\">" + 投稿Ent.No + "　　" + "</span>"
                                    + "<span class=\"MainTitle\">" + 投稿Ent.Title + "　" + "</span>"
                                    + "<span class=\"MainPersonNameHeader\">投稿者名：</span>" + "<span class=\"MainPersonName\">" + 投稿Ent.投稿者名 + "　" + "</span>";

                    pn.Controls.Add(new LiteralControl(html));


                    if (投稿Ent.HomepageUrl != "http://" && 投稿Ent.HomepageUrl != "")
                    {
                        Image img = new Image();
                        img.ImageUrl = "~/Images/Icon/homepage.png";
                        img.Width = 15;
                        img.Height = 15;
                        LinkButton lb = new LinkButton();
                        lb.PostBackUrl = 投稿Ent.HomepageUrl;
                        lb.Controls.Add(img);
                        pn.Controls.Add(lb);

                    }

                    string html10 = "　"
                                   + "<span class=\"MainDateHeader\">投稿日：</span>" + "<span class=\"MainDate\">" + dt.ToString("yyyy/MM/dd HH:mm:ss") + "　　" + "</span>" +
                                 "</td>" +
                                 "<td align=\"right\">";
                    pn.Controls.Add(new LiteralControl(html10));

                    string 記事No = 投稿Ent.No.Replace("No:", "");

                    返信用Button b = new 返信用Button(記事No,投稿Ent.Title);
                    b.Text = "返信";
                    b.ID = "hn" + 投稿Ent.カテゴリ名 + 記事No;
                    b.Click += new EventHandler(btn返信_Click);
                    pn.Controls.Add(b);

                    string 記事ColorCode = Convert文字色ToColorCode(投稿Ent.文字色);

                    string html2 =
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td colspan=\"6\" style=\"padding:10px 0px 0px 50px;color:" + 記事ColorCode + ";\">" + 投稿Ent.記事.Replace("\n", "<br/>");




                    if (投稿Ent.添付ファイルlist.Count() != 0)
                    {
                        html2 += "<br/><br/><br/>";
                    }

                    pn.Controls.Add(new LiteralControl(html2));

                    int counter = 1;
                    foreach (var ls in 投稿Ent.添付ファイルlist)
                    {
                        ダウンロード用LinkButton lkb = new ダウンロード用LinkButton(投稿Ent.No.Replace("No:", ""), ddlスレッド選択.SelectedValue);
                        lkb.ID = "dl" + 投稿Ent.カテゴリ名 + 記事No + counter;
                        lkb.Click += new EventHandler(lkbDownLoad_Click);
                        lkb.Text = ls;
                        lkb.Style.Add(HtmlTextWriterStyle.FontSize, "12px");
                        pn.Controls.Add(lkb);

                        string html3 =
                                    "<br/>";

                        pn.Controls.Add(new LiteralControl(html3));

                        counter++;


                    }

                    string html4 =
                            "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td><br/></td>" +
                            "</tr>";

                    // 返信があれば水平線を入れる
                    if (1 <= data2.Value.Count())
                    {
                        html4 += "<tr>" +
                                "<td colspan=\"6\">" +
                                    "<hr />" +
                                "</td>";

                    }


                    pn.Controls.Add(new LiteralControl(html4));

                    foreach (var 返信ent in data2.Value)
                    {

                        DateTime dt2;
                        if (DateTime.TryParse(返信ent.投稿日, out dt2) == false)
                        {
                            dt2 = new DateTime();
                        }



                        string html5 =
                            // 返信
                                "<tr >" +
                                    "<td colspan=\"6\" style=\"padding:20px 0px 0px 50px\">"
                                        + "<span class=\"SubNo\">" + 返信ent.No + "　　" + "</span>"
                                        + "<span class=\"SubTitle\">" + 返信ent.Title + "　" + "</span>"
                                        + "<span class=\"SubPersonNameHeader\">投稿者名：</span>" + "<span class=\"SubPersonName\">" + 返信ent.投稿者名 + "　" + "</span>";

                        pn.Controls.Add(new LiteralControl(html5));

                        if (返信ent.HomepageUrl != "http://" && 返信ent.HomepageUrl != "")
                        {
                            Image img = new Image();
                            img.ImageUrl = "~/Images/Icon/homepage.png";
                            img.Width = 15;
                            img.Height = 15;
                            LinkButton lb = new LinkButton();
                            lb.PostBackUrl = 返信ent.HomepageUrl;
                            lb.Controls.Add(img);
                            pn.Controls.Add(lb);

                        }

                        string 記事ColorCode2 = Convert文字色ToColorCode(返信ent.文字色);

                        //+ (返信ent.HomepageUrl == "http://" ? "" : 返信ent.HomepageUrl) + " 　 "
                        string html9 = " 　 " +
                                        "<span class=\"SubDateHeader\">投稿日：</span>" + "<span class=\"SubDate\">" + dt2.ToString("yyyy/MM/dd HH:mm:ss") + "　　" + "</span>" +
                                     "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td colspan=\"6\" style=\"padding:20px 0px 0px 105px;color:" + 記事ColorCode2 + ";\">" + 返信ent.記事.Replace("\n", "<br/>");

                        if (返信ent.添付ファイルlist.Count() != 0)
                        {
                            html9 += "<br/><br/><br/>";
                        }

                        pn.Controls.Add(new LiteralControl(html9));

                        string 記事No2 = 返信ent.No.Replace("No:", "");
                        int counter2 = 1;
                        foreach (var ls in 返信ent.添付ファイルlist)
                        {
                            ダウンロード用LinkButton lkb = new ダウンロード用LinkButton(記事No2, ddlスレッド選択.SelectedValue);
                            lkb.ID = "dl" + 投稿Ent.カテゴリ名 + 記事No2 + counter2;
                            lkb.Click += new EventHandler(lkbDownLoad_Click);
                            lkb.Text = ls;
                            lkb.Style.Add(HtmlTextWriterStyle.FontSize, "12px");

                            pn.Controls.Add(lkb);

                            string html6 =
                                        "<br/>";

                            pn.Controls.Add(new LiteralControl(html6));

                            counter2++;
                        }

                        string html7 =
                            "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td><br/></td>" +
                                "</tr>";

                        pn.Controls.Add(new LiteralControl(html7));


                    }

                    string html8 = "</table>";
                    pn.Controls.Add(new LiteralControl(html8));


                }
            }

            // 表示しているページ番号をセッションに保存
            Session["PageNumber"] = rdataset.作成グループNo;
            SetPageSelector(rdataset);
            Session["MaxPageNumber"] = (int)Math.Ceiling((double)rdataset.メイン記事数 / 10);

            //Session["Panel"] = pn;



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

            if (string.IsNullOrEmpty(ddlスレッド選択.SelectedValue))
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
        private string Convert文字色ToColorCode(string moji)
        {
            if (moji == "茶色")
            {
                return "#A52A2A";
            }
            else if (moji == "青")
            {
                return "#0000FF";
            }
            else if (moji == "黒")
            {
                return "#000000";
            }
            else if (moji == "緑")
            {
                return "#008000";
            }
            else if (moji == "オレンジ")
            {
                return "#FFA500";
            }
            else if (moji == "ピンク")
            {
                return "#FF00FF"; //メゼンタ magenta
            }
            else if (moji == "紫")
            {
                return "#800080";
            }
            else if (moji == "赤")
            {
                return "#FF0000";
            }
            else
            {
                //それ以外は茶色
                return "#A52A2A";
            }

        }
        private void SetSession店舗名List()
        {
            StoreDataClient client = PharmacyToolMobile.Util.ServiceUtil.ReferenceCreater.GetStoreDataClient();
            var rdataset = client.店舗名取得();

            // 店舗名の表示順序をセット
            Dictionary<int, string> dic = new Dictionary<int, string>();
            int compareValue = 1;
            foreach (var l in rdataset)
            {
                dic.Add(compareValue, l.店舗名);
                compareValue++;
            }

            Session["店舗名List"] = dic;
        }



    }
}