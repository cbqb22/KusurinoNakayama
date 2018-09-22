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
    public class PrintOrderSheetFree
    {

        // とりあえず１ページ目のフォームだけとする。
        private int _CurrentFirstPageNumber = 0;
        private int _TotalFirstPagePrintNumber = 0;
        //private int _CurrentSubPageNumber = 0;
        //private int _TotalSubPagePrintNumber = 0;
        private bool _２ページ目以降印刷中 = false;
        private PrintPreviewDialog _Ppd;
        private bool _IsPreview;
        private string fontname = "ＭＳ Ｐゴシック";
        private string 店名;

        public PrintOrderSheetFree(string 店名, int totalFirstpage)
        {
            this.店名 = 店名;
            _TotalFirstPagePrintNumber = totalFirstpage;
            //_TotalSubPagePrintNumber = totalSubpage;
        }


        private void InitFields()
        {
            _CurrentFirstPageNumber = 1;
            //_CurrentSubPageNumber = 1;
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


                    foreach (PaperSource ps in pd.PrinterSettings.PaperSources)
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
            //印刷する初期位置を決定
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top + 20;
 
            // １ページ目印刷
            if (_２ページ目以降印刷中 == false && _CurrentFirstPageNumber <= _TotalFirstPagePrintNumber)
            {

                // ①宛先
                //印刷する文字列と位置を設定する
                var text1 = string.Format("　　　　　　　　　　　　　御中", "");
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



                // ②挨拶文
                var text2 = string.Format("お疲れ様です。{0}でございます。以下の薬の移動をお願いいたします。", 店名);
                var font2 = new Font(fontname, 10);
                e.Graphics.DrawString(text2, font2, Brushes.Gray, x, y);


                y += (int)font2.GetHeight(e.Graphics) + 10;


                // ③配送時期
                var text3_1 = string.Format("□　　  年　   月　   日必着");
                var font3_1 = new Font(fontname, 13);
                e.Graphics.DrawString(text3_1, font3_1, Brushes.Gray, x, y);

                y += (int)font3_1.GetHeight(e.Graphics) + 5;
                var text3_2 = string.Format("□　　次の本部便で可");
                var font3_2 = new Font(fontname, 13);
                e.Graphics.DrawString(text3_2, font3_2, Brushes.Gray, x, y);

                y += (int)font3_1.GetHeight(e.Graphics) + 5;
                var text3_3 = string.Format("□　送れないものは結構です。返信も不要。");
                var font3_3 = new Font(fontname, 12);
                e.Graphics.DrawString(text3_3, font3_3, Brushes.Gray, x, y);

                y += (int)font3_2.GetHeight(e.Graphics) + 25;


                // ⑤送り元情報
                // 最大印刷個数が最大以下場合は１ページ目に追加
                Print送り元情報(e);

                // ⑥１ページ目のページ番号
                Printページ番号(e);

                _CurrentFirstPageNumber++;

            }

            // 全部印刷したかチェック
            if (_TotalFirstPagePrintNumber < _CurrentFirstPageNumber)
            {
                // 印刷継続を指定
                e.HasMorePages = false;
                InitFields();   // 全て印刷終了したらフィールドを初期化。これをしないとプレビュー時と印刷時でフィールドの値が保持されたままになる。
            }
        }

        private void Print送り元情報(PrintPageEventArgs e)
        {
            int x5 = 330;
            int y5 = 500;
            var text5 = string.Format("平成　　　年　　月　　日 (  )　{0}　担当者", 店名);
            //印刷に使うフォントを指定する
            var font5 = new Font(fontname, 13);
            //一行書き出す
            e.Graphics.DrawString(text5, font5, Brushes.Gray, x5, y5);


            var text5width = e.Graphics.MeasureString(text5, font5).Width;
            var underline = "＿＿＿＿＿";
            var underlinefont = new Font(fontname, 13, FontStyle.Underline);
            e.Graphics.DrawString(underline, underlinefont, Brushes.Gray, x5 + text5width, y5);

        }

        private void Printページ番号(PrintPageEventArgs e)
        {
            int x6 = 400;
            int y6 = 530;
            var text6 = " / ";
            var font6 = new Font(fontname, 7);
            e.Graphics.DrawString(text6, font6, Brushes.Gray, x6, y6);

        }

    }
}
