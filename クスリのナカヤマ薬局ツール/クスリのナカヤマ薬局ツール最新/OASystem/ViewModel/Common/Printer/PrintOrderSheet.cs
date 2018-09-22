using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Globalization;
using OASystem.Model.Entity;
using OASystem.ViewModel.Common.DateCenter;
using OASystem.ViewModel.Common.DataConvert;


namespace OASystem.ViewModel.Common.Printer
{
    public class PrintOrderSheet
    {
        private int _PageNumber = 1;
        private int _Current店舗PageNumber = 1;
        private int _Current店舗Index = 0;
        private int _CurrentMedicineIndex = 0; // ２ページ目以降の場合の現在のインデックス
        private int _FirstPageMaxOrderMax = 6; // 1ページ目に印刷可能な医薬品数
        private int _OtherPageMaxOrderMax = 8; // 1ページ目以降に印刷可能な医薬品数
        private bool _２ページ目以降印刷中 = false;
        private PrintPreviewDialog _Ppd;
        private bool _IsPreview;
        private string fontname = "ＭＳ Ｐゴシック";
        private string 店名;
        private Dictionary<string, List<ExpDeadListEntity>> OrderDic;

        public PrintOrderSheet(string 店名, Dictionary<string, List<ExpDeadListEntity>> orderDic)
        {
            this.店名 = 店名;
            this.OrderDic = orderDic;
        }

        private void SetTestData()
        {
            OrderDic.Clear();

            for (int i = 0; i < 2; i++) // 1店舗
            {
                List<ExpDeadListEntity> list = new List<ExpDeadListEntity>();

                for (int i2 = 0; i2 < 20; i2++) // 20医薬品
                {

                    ExpDeadListEntity ent = new ExpDeadListEntity();

                    ent.医薬品名 = "あああ" + (i2 + 1);
                    ent.注文数 = 520.5;
                    ent.Is期限切迫 = true;
                    ent.Isデッド品 = true;
                    ent.Is注文あり = true;
                    ent.包装形態 = "PTP";
                    ent.包装単位数 = "14";
                    ent.包装単位 = "カプセル";
                    ent.包装総量 = "100";

                    list.Add(ent);



                }

                OrderDic.Add(i.ToString(), list);

            }

        }

        private void InitFields()
        {
            _Current店舗Index = 0;
            _CurrentMedicineIndex = 0;
            _PageNumber = 0;
            _２ページ目以降印刷中 = false;

        }

        public bool HasInstallPrinter(string printerName)
        {
            foreach (string s in PrinterSettings.InstalledPrinters)
            {
                if (s == printerName)
                {
                    return true;
                }
            }

            return false;

        }

        public void Print()
        {
            InitFields();
            //SetTestData();

            if (PrinterSettings.InstalledPrinters.Count == 0)
            {
                MessageBox.Show("インストールされているプリンターがありません。\r\n処理を中止します。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            using (PrintDocument pd = new PrintDocument())
            {

                // プリンタの設定
                var pName = pd.PrinterSettings.PrinterName;


                // プリンタ設定のプリンタがない場合
                if (HasInstallPrinter(Model.DI.使用するプリンタ名) == false)
                {
                    // さらにデフォルトで使用するプリンタでない場合
                    if (Model.DI.使用するプリンタ名 != pName)
                    {

                        MessageBox.Show("インストールされたプリンターがありません。\r\n印刷を中止します", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }
                else
                {
                    pd.PrinterSettings.PrinterName = Model.DI.使用するプリンタ名;
                    

                    foreach(PaperSource ps in pd.PrinterSettings.PaperSources)
                    {
                        if (ps.SourceName == Model.DI.選択トレイ)
                        {
                            pd.DefaultPageSettings.PaperSource = ps;
                            break;
                        }
                    }

                }


                // 用紙の向きを設定(横：true、縦：false)
                pd.DefaultPageSettings.Landscape = true;
                pd.DefaultPageSettings.Margins = new Margins(50, 20, 20, 20);


                //string str = "";

                //foreach (PaperSource s in pd.PrinterSettings.PaperSources)
                //{
                //    str += s.SourceName;
                //}

                //MessageBox.Show(str);

                // プリンタがサポートしている用紙サイズを調べる
                foreach (PaperSize ps in pd.PrinterSettings.PaperSizes)
                {
                    // A5用紙に設定する
                    if (ps.Kind == PaperKind.A5)
                    {
                        pd.DefaultPageSettings.PaperSize = ps;
                        //MessageBox.Show("A5に設定しました。");
                        break;

                    }
                }

                // PrintPage イベントハンドル追加
                pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
                pd.EndPrint += new PrintEventHandler(pd_EndPrint);

                //// 印刷実行
                //pd.Print();

                // PrintPreviewDialogオブジェクトの生成
                _Ppd = new PrintPreviewDialog();
                _Ppd.StartPosition = FormStartPosition.CenterScreen;
                _Ppd.Height = 500;
                _Ppd.Width = 500;

                // Documentプロパティの設定
                _Ppd.Document = pd;
                _IsPreview = true;

                _Ppd.ShowDialog();
            }


        }

        void pd_EndPrint(object sender, PrintEventArgs e)
        {
            if (_IsPreview)
            {
                _IsPreview = false;
            }
            else
            {
                _Ppd.Close();
            }
        }



        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {


            var list = OrderDic.ToList(); // インデックス指定できる様ににListへ変換
            var kvp = list[_Current店舗Index];

            int Current店舗の総ページ数 = 0;

            if(kvp.Value.Count <= _FirstPageMaxOrderMax)
            {
                Current店舗の総ページ数 = 1;
            }else
            {
                decimal deci = (decimal)(kvp.Value.Count - _FirstPageMaxOrderMax) / (decimal)_OtherPageMaxOrderMax + 1;
                Current店舗の総ページ数 = (int)Math.Ceiling(deci);
            }


            //印刷する初期位置を決定
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top + 20;

            // ①宛先
            //印刷する文字列と位置を設定する
            var text1 = string.Format("{0}  御中", kvp.Key);
            //印刷に使うフォントを指定する
            var font1 = new Font(fontname, 25, FontStyle.Bold);

            // 領域を塗りつぶす
            var text1width = e.Graphics.MeasureString(text1, font1).Width;
            var text1height = e.Graphics.MeasureString(text1, font1).Height;
            var rect = new Rectangle(x, y, (int)text1width, (int)text1height);
            e.Graphics.FillRectangle(Brushes.LightGray, rect);
            //一行書き出す
            e.Graphics.DrawString(text1, font1, Brushes.Black, x, y);
            //次の行の印刷位置を計算
            y += (int)font1.GetHeight(e.Graphics) + 20;



            if (_２ページ目以降印刷中 == false)
            {
                // ②挨拶文
                var text2 = string.Format("お疲れ様です。{0}でございます。以下の薬の移動をお願いいたします。", 店名);
                var font2 = new Font(fontname, 10);
                e.Graphics.DrawString(text2, font2, Brushes.Gray, x, y);


                y += (int)font2.GetHeight(e.Graphics) + 10;


                // ③配送時期
                var text3_1 = string.Format("□　　  年　   月　   日必着");
                var font3_1 = new Font(fontname, 12);
                e.Graphics.DrawString(text3_1, font3_1, Brushes.Gray, x, y);

                y += (int)font3_1.GetHeight(e.Graphics) + 5;
                var text3_2 = string.Format("□　次の本部便で可");
                var font3_2 = new Font(fontname, 12);
                e.Graphics.DrawString(text3_2, font3_2, Brushes.Gray, x, y);

                y += (int)font3_1.GetHeight(e.Graphics) + 5;
                var text3_3 = string.Format("□　送れないものは結構です。返信も不要。");
                var font3_3 = new Font(fontname, 12);
                e.Graphics.DrawString(text3_3, font3_3, Brushes.Gray, x, y);

                y += (int)font3_2.GetHeight(e.Graphics) + 25;


                // ④医薬品
                int fontsize = 17;
                int IndexCounter = -1;
                foreach (var row in kvp.Value)
                {
                    IndexCounter++;
                    _CurrentMedicineIndex = IndexCounter;

                    if (IndexCounter == _FirstPageMaxOrderMax)
                    {
                        break;
                    }

                    // FontSize=17で全角1文字=幅20
                    var measureWidth = 0f;
                    int currentfontsize = fontsize;
                    int startX = 0;
                    var text = "";

                    // Column:0 番号
                    startX = x;
                    text = string.Format("{0}.", IndexCounter + 1);
                    var font = new Font(fontname, fontsize);
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 40;

                    // Column:1 医薬品名
                    text = string.Format("{0}", row.医薬品名と名称２連結);
                    font = new Font(fontname, currentfontsize);

                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 260)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 260;

                    // Column:2 数量・単位
                    currentfontsize = fontsize;
                    text = string.Format("{0}{1}", row.注文数.ToString().PadLeft(8,' '), DataConvert.DataConvert.包装単位ショート変換(row.包装単位));
                    font = new Font(fontname, currentfontsize);
                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 160)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }

                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 160;


                    // Column:3 包装形態/包装単位数
                    //currentfontsize = fontsize - 1;
                    //text = string.Format("{0}({1})", row.包装形態, row.包装単位数);
                    //font = new Font(fontname, currentfontsize);
                    //while (true)
                    //{
                    //    measureWidth = e.Graphics.MeasureString(text, font).Width;
                    //    if (measureWidth < 100)
                    //    {
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        font = new Font(fontname, --currentfontsize);
                    //    }
                    //}

                    //e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    //startX += 100;


                    // Column:4 期限切迫・デッド・優先
                    currentfontsize = fontsize-1;
                    text = row.Is期限切迫 ? "期迫" : "";
                    if (text != "" && row.Isデッド品)
                    {
                        text += "・デッド";
                    }else if(text == "" && row.Isデッド品)
                    {
                        text += "デッド";
                    }

                    if (text != "" && row.Is優先移動)
                    {
                        text += "・優先";
                    }
                    else if (text == "" && row.Is優先移動)
                    {
                        text += "優先";
                    }



                    font = new Font(fontname, currentfontsize);

                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 160)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 160;

                    // Column:5 使用期限
                    currentfontsize = fontsize - 1;
                    text = row.使用期限.ToString("yyyy/MM");
                    font = new Font(fontname, currentfontsize);

                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 80)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 80;


                    y += (int)font.GetHeight(e.Graphics) + 30;


                }

                // ⑤送り元情報
                // 最大印刷個数が最大以下場合は１ページ目に追加
                if (kvp.Value.Count <= _FirstPageMaxOrderMax)
                {
                    Print送り元情報(e);
                }
                else
                {
                    Print次項あり(e);
                    _２ページ目以降印刷中 = true;
                }

                // ⑥１ページ目のページ番号

                Printページ番号(e, Current店舗の総ページ数, 1);

                _Current店舗PageNumber = 2;

            }

            // ２ページ目以降印刷
            else
            {
                //微調整
                y += 20;

                int fontsize = 17;
                int IndexCounter2 = -1;
                foreach (var row in kvp.Value)
                {
                    IndexCounter2++;

                    if (IndexCounter2 < _CurrentMedicineIndex)
                    {
                        continue;
                    }

                    _CurrentMedicineIndex = IndexCounter2;

                    // 最初の印刷前でなく、かつ最大個数印刷した後ならば、次のページへ
                    int check1 = _FirstPageMaxOrderMax + (_Current店舗PageNumber - 2) * _OtherPageMaxOrderMax;
                    int check2 = (_CurrentMedicineIndex - _FirstPageMaxOrderMax) % _OtherPageMaxOrderMax;
                    if (check1 != _CurrentMedicineIndex && check2 == 0)
                    {
                        _２ページ目以降印刷中 = true;
                        break;
                    }


                    // FontSize=17で全角1文字=幅20
                    var measureWidth = 0f;
                    int currentfontsize = fontsize;
                    int startX = 0;
                    var text = "";


                    // Column:0 番号
                    startX = x;
                    text = string.Format("{0}.", IndexCounter2 + 1);
                    var font = new Font(fontname, fontsize);
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 40;

                    // Column:1 医薬品名
                    text = string.Format("{0}", row.医薬品名と名称２連結);
                    font = new Font(fontname, currentfontsize);

                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 260)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 260;


                    // Column:2 数量・単位
                    currentfontsize = fontsize;
                    text = string.Format("{0}{1}", row.注文数.ToString().PadLeft(8, ' '), DataConvert.DataConvert.包装単位ショート変換(row.包装単位));
                    font = new Font(fontname, currentfontsize);
                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 160)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }

                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 160;



                    // Column:3 包装形態/包装単位数
                    //currentfontsize = fontsize - 1;
                    //text = string.Format("{0}({1})", row.包装形態, row.包装単位数);
                    //font = new Font(fontname, currentfontsize);
                    //while (true)
                    //{
                    //    measureWidth = e.Graphics.MeasureString(text, font).Width;
                    //    if (measureWidth < 100)
                    //    {
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        font = new Font(fontname, --currentfontsize);
                    //    }
                    //}

                    //e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    //startX += 100;


                    // Column:4 期限切迫・デッド・優先
                    currentfontsize = fontsize - 1;
                    text = row.Is期限切迫 ? "期迫" : "";
                    if (text != "" && row.Isデッド品)
                    {
                        text += "・デッド";
                    }
                    else if (text == "" && row.Isデッド品)
                    {
                        text += "デッド";
                    }
                    if (text != "" && row.Is優先移動)
                    {
                        text += "・優先";
                    }
                    else if (text == "" && row.Is優先移動)
                    {
                        text += "優先";
                    }

                    font = new Font(fontname, currentfontsize);
                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 160)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 160;





                    // Column:5 使用期限
                    currentfontsize = fontsize - 1;
                    text = row.使用期限.ToString("yyyy/MM");
                    font = new Font(fontname, currentfontsize);

                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 80)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 80;


                    y += (int)font.GetHeight(e.Graphics) + 30;

                 

               


                    // Column:5 備考

                    y += (int)font.GetHeight(e.Graphics) + 30;

                }

                // ページ番号
                Printページ番号(e, Current店舗の総ページ数, _Current店舗PageNumber);

                // 全部医薬品を印刷したらフッター
                if (kvp.Value.Count - 1 <= _CurrentMedicineIndex)
                {
                    Print送り元情報(e);

                    _２ページ目以降印刷中 = false;
                }
                else
                {
                    Print次項あり(e);
                }

                _Current店舗PageNumber++;

            }

            // この店舗の印刷が残っているか
            if (_２ページ目以降印刷中)
            {
                // 印刷継続を指定
                e.HasMorePages = true;
            }
            else
            {
                // 次の店舗があれば継続
                if (_Current店舗Index < list.Count - 1)
                {
                    _Current店舗Index++;
                    _PageNumber++;

                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                    InitFields();   // 全て印刷終了したらフィールドを初期化。これをしないとプレビュー時と印刷時でフィールドの値が保持されたままになる。
                }
            }


        }

        private void Print送り元情報(PrintPageEventArgs e)
        {
            int x5 = 350;
            int y5 = 500;
            var text5 = string.Format("{0}{1}年{2}月{3}日({4})  {5}　担当者", Converter.Get和暦元号(DateTime.Now), Converter.Get和暦年(DateTime.Now), DateTime.Now.Month, DateTime.Now.Day, Converter.ConverDayOfWeekEnglishToJapanese(DateTime.Now.DayOfWeek), 店名);
            //印刷に使うフォントを指定する
            var font5 = new Font(fontname, 13);
            //一行書き出す
            e.Graphics.DrawString(text5, font5, Brushes.Gray, x5, y5);


            var text5width = e.Graphics.MeasureString(text5, font5).Width;
            var underline = "＿＿＿＿＿";
            var underlinefont = new Font(fontname, 13, FontStyle.Underline);
            e.Graphics.DrawString(underline, underlinefont, Brushes.Gray, x5 + text5width, y5);

        }

        private void Printページ番号(PrintPageEventArgs e, int 総ページ数, int 現在のページ数)
        {
            int x6 = 400;
            int y6 = 530;
            var text6 = string.Format("{0} / {1}", 現在のページ数, 総ページ数);
            var font6 = new Font(fontname, 7);
            e.Graphics.DrawString(text6, font6, Brushes.Gray, x6, y6);

        }

        private void Print次項あり(PrintPageEventArgs e)
        {
            int x = 700;
            int y = 505;
            var text = "次項あり⇒";
            var font = new Font(fontname, 10);
            e.Graphics.DrawString(text, font, Brushes.Black, x, y);
        }
    }
}
