using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.ServiceModel;
using System.Drawing;
using PharmacyToolMobile.Service.File.Reader;
using PharmacyToolMobile.Util.Common;
using PharmacyToolMobile.Util.ServiceUtil;
using PharmacyToolMobile.Service.DAO.PharmacyTool.店舗;



namespace PharmacyToolMobile.Pages.在庫管理
{
    public partial class StockManagement : System.Web.UI.Page
    {
        enum 在庫検索ボタンEnum
        {
            現在庫,
            使用量,
            不動品,
            後発品
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            // ユーザー名がない場合はログイン画面へRedirect
            if (Session["表示ユーザー名"] == null)
            {
                Response.Redirect("../Login.aspx");
            }


            if (Session["店舗名List"] == null)
            {
                SetSession店舗名List();
            }
        }

        private void SetActiveButton()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //IsPostBack
            //アクティブボタン = 在庫検索ボタンEnum.現在庫;
        }

        protected void btn現在庫_Click(object sender, System.EventArgs e)
        {
            btn現在庫.BackColor = Color.Pink;
            btn使用量.BackColor = Color.LightBlue;
            btn不動品.BackColor = Color.LightBlue;
            btn後発品.BackColor = Color.LightBlue;
            現在庫検索();
        }
        protected void btn使用量_Click(object sender, System.EventArgs e)
        {
            btn現在庫.BackColor = Color.LightBlue;
            btn使用量.BackColor = Color.Pink;
            btn不動品.BackColor = Color.LightBlue;
            btn後発品.BackColor = Color.LightBlue;
            使用量検索();
        }
        protected void btn不動品_Click(object sender, System.EventArgs e)
        {
            btn現在庫.BackColor = Color.LightBlue;
            btn使用量.BackColor = Color.LightBlue;
            btn不動品.BackColor = Color.Pink;
            btn後発品.BackColor = Color.LightBlue;
            不動品検索();
        }
        protected void btn後発品_Click(object sender, System.EventArgs e)
        {
            btn現在庫.BackColor = Color.LightBlue;
            btn使用量.BackColor = Color.LightBlue;
            btn不動品.BackColor = Color.LightBlue;
            btn後発品.BackColor = Color.Pink;
            後発品検索();
        }

        protected void dg現在庫_ItemBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //if (e.Item.Cells[0].Text.IndexOf(".NET TIPS") != -1)
                //{
                //    e.Item.BackColor = System.Drawing.Color.Pink;
                //}
                //foreach (TableCell cell in e.Item.Cells)
                //{
                //    cell.Text = cell.Text.Replace(".NET",
                //      "<B Style='background-color:yellow'>.NET</B>");

                //}


                DateTime result;
                if(DateTime.TryParse(e.Item.Cells[4].Text,out result))
                {
                    // 日付をフォーマット
                    e.Item.Cells[4].Text = result.ToString("yyyy/MM/dd");

                    // 期限切れならば赤
                    if (result < DateTime.Now)
                    {
                        string style = "color:Red;";
                        e.Item.Cells[4].Attributes.Add("style", style);
                    // 使用期限日以内であれば青
                    }else if(0 <= ddl使用期限色.SelectedIndex &&
                             result <= DateTime.Now.AddMonths(ddl使用期限色.SelectedIndex + 1))
                    {
                        string style = "color:Blue;";
                        e.Item.Cells[4].Attributes.Add("style", style);
                    }
                }

                string javascript = "document.getElementById('tb検索キーワード').value = '" + e.Item.Cells[1].Text + "';";
                e.Item.Cells[1].Attributes.Add("onmousedown", javascript);

                string javascript2 = "document.getElementById('tb検索キーワード').value = '" + e.Item.Cells[2].Text + "';";
                e.Item.Cells[2].Attributes.Add("onmousedown", javascript2);

            }

        }

        protected void dg使用量_ItemBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                string javascript = "document.getElementById('tb検索キーワード').value = '" + e.Item.Cells[2].Text + "';";
                e.Item.Cells[2].Attributes.Add("onmousedown", javascript);

            }

        }
        protected void dg不動品_ItemBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                     || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                DateTime result;
                if (DateTime.TryParse(e.Item.Cells[4].Text, out result))
                {
                    // 日付をフォーマット
                    e.Item.Cells[4].Text = result.ToString("yyyy/MM/dd");

                    // 期限切れならば赤
                    if (result < DateTime.Now)
                    {
                        string style = "color:Red;";
                        e.Item.Cells[4].Attributes.Add("style", style);
                        // 使用期限日以内であれば青
                    }
                    else if (0 <= ddl使用期限色.SelectedIndex &&
                            result <= DateTime.Now.AddMonths(ddl使用期限色.SelectedIndex + 1))
                    {
                        string style = "color:Blue;";
                        e.Item.Cells[4].Attributes.Add("style", style);
                    }
                }


                string javascript = "document.getElementById('tb検索キーワード').value = '" + e.Item.Cells[1].Text + "';";
                e.Item.Cells[1].Attributes.Add("onmousedown", javascript);

                string javascript2 = "document.getElementById('tb検索キーワード').value = '" + e.Item.Cells[2].Text + "';";
                e.Item.Cells[2].Attributes.Add("onmousedown", javascript2);

            }
        }
        protected void dg後発品_ItemBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                DateTime result;
                if (DateTime.TryParse(e.Item.Cells[5].Text, out result))
                {
                    // 日付をフォーマット
                    e.Item.Cells[5].Text = result.ToString("yyyy/MM/dd");

                    // 期限切れならば赤
                    if (result < DateTime.Now)
                    {
                        string style = "color:Red;";
                        e.Item.Cells[5].Attributes.Add("style", style);
                        // 使用期限日以内であれば青
                    }
                    else if (0 <= ddl使用期限色.SelectedIndex &&
                            result <= DateTime.Now.AddMonths(ddl使用期限色.SelectedIndex + 1))
                    {
                        string style = "color:Blue;";
                        e.Item.Cells[5].Attributes.Add("style", style);
                    }
                }


                if (e.Item.Cells[1].Text.IndexOf("先発品") != -1)
                {
                    e.Item.Cells[1].Text = "";
                }
                else if (e.Item.Cells[1].Text.IndexOf("後発品") != -1)
                {
                    e.Item.Cells[1].Text = "●";
                }

                string javascript = "document.getElementById('tb検索キーワード').value = '" + e.Item.Cells[2].Text + "';";
                e.Item.Cells[2].Attributes.Add("onmousedown", javascript);

                string javascript2 = "document.getElementById('tb検索キーワード').value = '" + e.Item.Cells[3].Text + "';";
                e.Item.Cells[3].Attributes.Add("onmousedown", javascript2);

            }

        }

        protected void btn掲示板_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("../掲示板/BBS.aspx");
        }
        protected void btn在庫管理_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("StockManagement.aspx");
        }
        //void lkb_Click(object sender, EventArgs e)
        //{
        //}
        //protected void tb検索キーワード_TextChanged(object sender, System.EventArgs e)
        //{

        //    if (アクティブボタン == 在庫検索ボタンEnum.現在庫)
        //    {
        //        現在庫検索();
        //    }
        //    else if (アクティブボタン == 在庫検索ボタンEnum.使用量)
        //    {
        //        使用量検索();
        //    }
        //    else if (アクティブボタン == 在庫検索ボタンEnum.不動品)
        //    {
        //        不動品検索();
        //    }
        //    else if (アクティブボタン == 在庫検索ボタンEnum.後発品)
        //    {
        //        後発品検索();
        //    }

        //    //if (e.Item.ItemType == ListItemType.Item
        //    //    || e.Item.ItemType == ListItemType.AlternatingItem) {

        //    //  if (e.Item.Cells[0].Text.IndexOf(".NET TIPS") != -1) {
        //    //    e.Item.BackColor = System.Drawing.Color.Pink;
        //    //  }
        //    //  foreach (TableCell cell in e.Item.Cells) {
        //    //    cell.Text = cell.Text.Replace(".NET",
        //    //      "<B Style='background-color:yellow'>.NET</B>");
        //    //  }

        //}

        private void 現在庫検索()
        {
            //アクティブボタン = 在庫検索ボタンEnum.現在庫;

            dg現在庫.Visible = true;
            dg使用量.Visible = false;
            dg不動品.Visible = false;
            dg後発品.Visible = false;


            bool 全期限 = false;
            bool 期限内 = false;
            bool 期限切 = false;
            bool 期限指定 = false;
            bool 以内指定か = false;
            int 期限加算月 = 0;

            if (ddl使用期限日.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else if (ddl使用期限日.SelectedIndex == 1)
            {
                期限内 = true;
            }
            else if (ddl使用期限日.SelectedIndex == 2)
            {
                期限切 = true;
            }
            else if (3 <= ddl使用期限日.SelectedIndex)
            {
                期限指定 = true;
                Tuple<int, bool> tp = Set期限加算月();
                期限加算月 = tp.Value1;
                以内指定か = tp.Value2;

            }


            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();


            if (tb検索キーワード.Text == "")
            {
                return;
            }

            var rdataset = client.Get現在庫検索データ(tb検索キーワード.Text, 全期限, 期限内, 期限切, 期限指定, 以内指定か, 期限加算月);
            //var rdataset = client.Get現在庫検索データ(tb検索キーワード.Text, true, true, true, true, true, 3);


            if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
            {

                var dataList = rdataset.検索結果データlist.ToList();
                var 表示順序dic = Session["店舗名List"] as Dictionary<int, string>;
                if (表示順序dic == null)
                {
                    return;
                }

                // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                var CopyDic = GenericUtil.Copy(表示順序dic);
                int cnt = CopyDic.Count + 1;

                dataList.Sort(
                    delegate(現在庫データ x, 現在庫データ y)
                    {
                        int xValue = 0;
                        int yValue = 0;
                        foreach (var d in CopyDic)
                        {
                            if (d.Value.Equals(x.店名))
                            {
                                xValue = d.Key;
                            }

                            if (d.Value.Equals(y.店名))
                            {
                                yValue = d.Key;
                            }
                        }

                        // 一致するものがなかったら、臨時のcntの番号にする（表示順序が後ろ）
                        bool xy店名が等しい = false;
                        if (xValue == 0)
                        {
                            if (x.店名.Equals(y.店名))
                            {
                                xy店名が等しい = true;
                            }

                            xValue = cnt;
                            CopyDic.Add(cnt, x.店名);
                            cnt++;
                        }

                        if (yValue == 0)
                        {
                            if (xy店名が等しい)
                            {
                                yValue = xValue;
                            }
                            else
                            {
                                yValue = cnt;
                                CopyDic.Add(cnt, y.店名);
                                cnt++;
                            }
                        }

                        // 店名が等しい場合は、医薬品名のアイウエオ順
                        if (xValue == yValue)
                        {
                            return x.医薬品名.CompareTo(y.医薬品名);

                        }


                        return xValue - yValue;
                    }
                    );


                dg現在庫.DataSource = dataList;
                dg現在庫.DataBind();

                Set在庫検索結果Result(dataList.Count, rdataset.検索キーワード);


            }
            else
            {
                // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                //MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                lbSearchResult.Text = rdataset.エラーメッセージ;
            }


        }
        private void 使用量検索()
        {
            //アクティブボタン = 在庫検索ボタンEnum.使用量;


            dg現在庫.Visible = false;
            dg使用量.Visible = true;
            dg不動品.Visible = false;
            dg後発品.Visible = false;

            // 空文字ならば、検索しない。
            if (tb検索キーワード.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {

                //// カーソルをもとの状態にする
                //this.SetCursorDefault();

                return;
            }

            bool 全期限 = false;
            int 期限加算月 = 3;

            if (ddl使用量検索.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else
            {
                期限加算月 = Set使用量検索期限加算月();
            }

            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();

            //var rdataset = client.Open使用量CSV(tb検索キーワード.Text, 全期限, 期限加算月);
            var rdataset = client.Open使用量2CSV(tb検索キーワード.Text, 全期限, 期限加算月);


            if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
            {
                var dataList = rdataset.検索結果データlist.ToList();
                var 表示順序dic = Session["店舗名List"] as Dictionary<int, string>;
                if (表示順序dic == null)
                {
                    return;
                }

                // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                var CopyDic = GenericUtil.Copy(表示順序dic);
                int cnt = CopyDic.Count + 1;

                dataList.Sort(
                    delegate(薬局使用量データ x, 薬局使用量データ y)
                    {
                        int xValue = 0;
                        int yValue = 0;
                        foreach (var d in CopyDic)
                        {
                            if (d.Value.Equals(x.店名))
                            {
                                xValue = d.Key;
                            }

                            if (d.Value.Equals(y.店名))
                            {
                                yValue = d.Key;
                            }
                        }

                        // 一致するものがなかったら、臨時のcntの番号にする（表示順序が後ろ）
                        bool xy店名が等しい = false;
                        if (xValue == 0)
                        {
                            if (x.店名.Equals(y.店名))
                            {
                                xy店名が等しい = true;
                            }

                            xValue = cnt;
                            CopyDic.Add(cnt, x.店名);
                            cnt++;
                        }

                        if (yValue == 0)
                        {
                            if (xy店名が等しい)
                            {
                                yValue = xValue;
                            }
                            else
                            {
                                yValue = cnt;
                                CopyDic.Add(cnt, y.店名);
                                cnt++;
                            }
                        }

                        if (xValue != yValue)
                        {
                            return xValue - yValue;

                        }

                        // 等しい場合は、さらに使用年月日で比較する
                        if (xValue == yValue)
                        {
                            DateTime xの年月日;
                            DateTime yの年月日;

                            // xがDateTimeにキャストできない場合は、後ろに
                            if (DateTime.TryParse(x.使用年月, out xの年月日) == false)
                            {
                                return 1;
                            }


                            // yがDateTimeにキャストできない場合は、前に
                            if (DateTime.TryParse(y.使用年月, out yの年月日) == false)
                            {
                                return -1;
                            }


                            if (xの年月日 > yの年月日)
                            {
                                return -1;
                            }
                            else if (yの年月日 > xの年月日)
                            {
                                return 1;
                            }
                        }

                        // 店名が等しく、使用年月も等しい場合は、医薬品名のアイウエオ順
                        if (xValue == yValue)
                        {
                            return x.医薬品名.CompareTo(y.医薬品名);

                        }


                        return xValue - yValue;
                    }
                    );

                dg使用量.DataSource = dataList;
                dg使用量.DataBind();

                Set在庫検索結果Result(dataList.Count, rdataset.検索キーワード);
            }
            else
            {
                lbSearchResult.Text = rdataset.エラーメッセージ;
            }



        }
        private void 不動品検索()
        {
            //アクティブボタン = 在庫検索ボタンEnum.不動品;


            dg現在庫.Visible = false;
            dg使用量.Visible = false;
            dg不動品.Visible = true;
            dg後発品.Visible = false;


            // 空文字ならば、検索しない。
            if (tb検索キーワード.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {
                //// カーソルをもとの状態にする
                //this.SetCursorDefault();


                return;
            }


            bool 全期限 = false;
            bool 期限内 = false;
            bool 期限切 = false;
            bool 期限指定 = false;
            bool 以内指定か = false;
            int 期限加算月 = 0;


            if (ddl使用期限日.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else if (ddl使用期限日.SelectedIndex == 1)
            {
                期限内 = true;
            }
            else if (ddl使用期限日.SelectedIndex == 2)
            {
                期限切 = true;
            }
            else if (3 <= ddl使用期限日.SelectedIndex)
            {
                期限指定 = true;
                Tuple<int, bool> tp = Set期限加算月();
                期限加算月 = tp.Value1;
                以内指定か = tp.Value2;

            }

            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();


            var rdataset = client.Open不動品CSV(tb検索キーワード.Text, 全期限, 期限内, 期限切, 期限指定, 以内指定か, 期限加算月);


            if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
            {

                var dataList = rdataset.検索結果データlist.ToList();
                var 表示順序dic = Session["店舗名List"] as Dictionary<int, string>;
                if (表示順序dic == null)
                {
                    return;
                }

                // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                var CopyDic = GenericUtil.Copy(表示順序dic);
                int cnt = CopyDic.Count + 1;

                dataList.Sort(
                    delegate(不動品データ x, 不動品データ y)
                    {
                        int xValue = 0;
                        int yValue = 0;
                        foreach (var d in CopyDic)
                        {
                            if (d.Value.Equals(x.店名))
                            {
                                xValue = d.Key;
                            }

                            if (d.Value.Equals(y.店名))
                            {
                                yValue = d.Key;
                            }
                        }

                        // 一致するものがなかったら、臨時のcntの番号にする（表示順序が後ろ）
                        bool xy店名が等しい = false;
                        if (xValue == 0)
                        {
                            if (x.店名.Equals(y.店名))
                            {
                                xy店名が等しい = true;
                            }

                            xValue = cnt;
                            CopyDic.Add(cnt, x.店名);
                            cnt++;
                        }

                        if (yValue == 0)
                        {
                            if (xy店名が等しい)
                            {
                                yValue = xValue;
                            }
                            else
                            {
                                yValue = cnt;
                                CopyDic.Add(cnt, y.店名);
                                cnt++;
                            }
                        }

                        // 店名が等しい場合は、医薬品名のアイウエオ順
                        if (xValue == yValue)
                        {
                            return x.医薬品名.CompareTo(y.医薬品名);

                        }


                        return xValue - yValue;
                    }
                    );


                dg不動品.DataSource = dataList;
                dg不動品.DataBind();

                Set在庫検索結果Result(dataList.Count, rdataset.検索キーワード);


            }
            else
            {
                // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                //MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
                lbSearchResult.Text = rdataset.エラーメッセージ;
            }





        }
        private void 後発品検索()
        {
            //アクティブボタン = 在庫検索ボタンEnum.後発品;

            dg現在庫.Visible = false;
            dg使用量.Visible = false;
            dg不動品.Visible = false;
            dg後発品.Visible = true;

            // 空文字ならば、検索しない。
            if (tb検索キーワード.Text.Replace("　", "").Replace(" ", "").Equals(""))
            {

                //// カーソルをもとの状態にする
                //this.SetCursorDefault();

                return;
            }

            int result;

            if (tb検索キーワード.Text.Length != 12)
            {
                return;
            }
            else if (int.TryParse(tb検索キーワード.Text.Substring(0, 7), out result) == false)
            {
                return;
            }
            else if (int.TryParse(tb検索キーワード.Text.Substring(8, 4), out result) == false)
            {
                return;
            }

            bool 全期限 = false;
            bool 期限内 = false;
            bool 期限切 = false;
            bool 期限指定 = false;
            bool 以内指定か = false;
            int 期限加算月 = 0;
            bool 他規格・剤形も表示する = false;

            if (ddl使用期限日.SelectedIndex == 0)
            {
                全期限 = true;
            }
            else if (ddl使用期限日.SelectedIndex == 1)
            {
                期限内 = true;
            }
            else if (ddl使用期限日.SelectedIndex == 2)
            {
                期限切 = true;
            }
            else if (3 <= ddl使用期限日.SelectedIndex)
            {
                期限指定 = true;
                Tuple<int, bool> tp = Set期限加算月();
                期限加算月 = tp.Value1;
                以内指定か = tp.Value2;

            }


            if (ddl後発品検索.SelectedIndex == 0)
            {
                他規格・剤形も表示する = false;
            }
            else
            {
                他規格・剤形も表示する = true;
            }



            if (tb検索キーワード.Text == "")
            {
                return;
            }

            Service.File.Reader.FileReaderClient client = ReferenceCreater.GetFileReaderClient();


            var rdataset = client.Get後発品検索データ(tb検索キーワード.Text, 全期限, 期限内, 期限切, 期限指定, 以内指定か, 期限加算月, 他規格・剤形も表示する);

            if (rdataset.検索結果データlist != null)
            {
            }

            if (string.IsNullOrEmpty(rdataset.エラーメッセージ))
            {

                var dataList = rdataset.検索結果データlist.ToList();
                var 表示順序dic = Session["店舗名List"] as Dictionary<int, string>;
                if (表示順序dic == null)
                {
                    return;
                }
                // 表示順序に登録されていない店舗があった場合に使用する臨時dic
                var CopyDic = GenericUtil.Copy(表示順序dic);
                int cnt = CopyDic.Count + 1;

                dataList.Sort(
                    delegate(現在庫データ x, 現在庫データ y)
                    {
                        int xValue = 0;
                        int yValue = 0;
                        foreach (var d in CopyDic)
                        {
                            if (d.Value.Equals(x.店名))
                            {
                                xValue = d.Key;
                            }

                            if (d.Value.Equals(y.店名))
                            {
                                yValue = d.Key;
                            }
                        }

                        // 一致するものがなかったら、臨時のcntの番号にする（表示順序が後ろ）
                        bool xy店名が等しい = false;
                        if (xValue == 0)
                        {
                            if (x.店名.Equals(y.店名))
                            {
                                xy店名が等しい = true;
                            }

                            xValue = cnt;
                            CopyDic.Add(cnt, x.店名);
                            cnt++;
                        }

                        if (yValue == 0)
                        {
                            if (xy店名が等しい)
                            {
                                yValue = xValue;
                            }
                            else
                            {
                                yValue = cnt;
                                CopyDic.Add(cnt, y.店名);
                                cnt++;
                            }
                        }

                        if (xValue != yValue)
                        {
                            return xValue - yValue;

                        }


                        // 店名が等しい場合は、次は先発か後発で判断する。
                        if (xValue == yValue)
                        {
                            bool x先発か = x.後発区分.Equals("") ? false : true;
                            bool y先発か = y.後発区分.Equals("") ? false : true;

                            if (x先発か == y先発か)
                            {
                            }
                            else if (x先発か == true)
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        }

                        // 店名も等しくて、先発かも等しい
                        if (xValue == yValue)
                        {
                            double x薬価;
                            double y薬価;
                            if (double.TryParse(x.薬価, out x薬価) == false)
                            {
                                return 1;
                            }

                            if (double.TryParse(y.薬価, out y薬価) == false)
                            {
                                return -1;
                            }


                            if (x薬価 > y薬価)
                            {
                                return -1;
                            }

                            if (y薬価 > x薬価)
                            {
                                return 1;
                            }
                        }

                        // 店名も等しくて、先発かも等しく、薬価も等しい
                        if (xValue == yValue)
                        {
                            return x.医薬品名.CompareTo(y.医薬品名);
                        }


                        return xValue - yValue;
                        //return yValue - xValue;
                    }
                    );


                dg後発品.DataSource = dataList;
                dg後発品.DataBind();
                Set在庫検索結果Result(dataList.Count, rdataset.検索キーワード);

            }
            else
            {
                lbSearchResult.Text = rdataset.エラーメッセージ;
                // メッセージボックスを出すとXPだとIMEが利かなくなるので中止
                // MessageBox.Show(rdataset.エラーメッセージ, "検索結果", MessageBoxButton.OK);
            }



        }
        private void Set在庫検索結果Result(int 検索ヒット数, string 検索キーワード)
        {
            if (検索ヒット数 <= 0)
            {
                lbSearchResult.Text = string.Format("キーワード【{0}】 で検索した結果、該当するデータがありません。", 検索キーワード);
            }
            else
            {
                lbSearchResult.Text = string.Format("キーワード【{0}】 で検索した結果、{1}件の該当するデータがありました。", 検索キーワード, 検索ヒット数.ToString());
            }

        }
        private int Set使用量検索期限加算月()
        {
            if (ddl使用量検索.SelectedIndex == -1)
            {
                // デフォルトで３ヶ月に設定
                return 3;
            }
            else if (1 <= ddl使用量検索.SelectedIndex)
            {
                return ddl使用量検索.SelectedIndex;
            }
            else
            {
                throw new Exception("使用量検索の選択が正しくありません。");
            }
        }
        Tuple<int, bool> Set期限加算月()
        {
            Tuple<int, bool> result = new Tuple<int, bool>();
            if (ddl使用期限日.SelectedIndex == 3)
            {
                result.Value1 = 1;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 4)
            {
                result.Value1 = 1;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 5)
            {
                result.Value1 = 2;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 6)
            {
                result.Value1 = 2;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 7)
            {
                result.Value1 = 3;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 8)
            {
                result.Value1 = 3;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 9)
            {
                result.Value1 = 4;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 10)
            {
                result.Value1 = 4;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 11)
            {
                result.Value1 = 5;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 12)
            {
                result.Value1 = 5;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 13)
            {
                result.Value1 = 6;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 14)
            {
                result.Value1 = 6;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 15)
            {
                result.Value1 = 7;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 16)
            {
                result.Value1 = 7;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 17)
            {
                result.Value1 = 8;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 18)
            {
                result.Value1 = 8;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 19)
            {
                result.Value1 = 9;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 20)
            {
                result.Value1 = 9;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 21)
            {
                result.Value1 = 10;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 22)
            {
                result.Value1 = 10;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 23)
            {
                result.Value1 = 11;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 24)
            {
                result.Value1 = 11;
                result.Value2 = false;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 25)
            {
                result.Value1 = 12;
                result.Value2 = true;
                return result;
            }
            else if (ddl使用期限日.SelectedIndex == 26)
            {
                result.Value1 = 12;
                result.Value2 = false;
                return result;
            }
            else
            {
                throw new Exception("使用期限日が不正です。");
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