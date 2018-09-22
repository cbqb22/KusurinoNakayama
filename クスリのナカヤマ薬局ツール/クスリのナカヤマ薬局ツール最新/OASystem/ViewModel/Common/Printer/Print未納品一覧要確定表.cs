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
    public class Print未納品一覧要確定表
    {
        private int _CurrentPageNumber = 1;
        private int _CurrentMedicineIndex = 0; // ２ページ目以降の場合の現在のインデックス
        private int _FirstPageMaxPrint = 20; // 1ページ目に印刷可能な医薬品数
        private int _OtherPageMaxPrint = 20; // 1ページ目以降に印刷可能な医薬品数
        private bool _２ページ目以降印刷中 = false;
        private PrintPreviewDialog _Ppd;
        private bool _IsPreview;
        private string fontname = "ＭＳ Ｐゴシック";
        private List<OrderScheduledListEntity> OrderScheduleList;

        public Print未納品一覧要確定表(List<OrderScheduledListEntity> orderScheduleList)
        {
            this.OrderScheduleList = orderScheduleList;
        }

        //private void SetTestData()
        //{
        //    OrderScheduleDic.Clear();

        //    for (int i = 0; i < 2; i++) // 1店舗
        //    {
        //        List<ExpDeadListEntity> list = new List<ExpDeadListEntity>();

        //        for (int i2 = 0; i2 < 20; i2++) // 20医薬品
        //        {

        //            ExpDeadListEntity ent = new ExpDeadListEntity();

        //            ent.医薬品名 = "あああ" + (i2 + 1);
        //            ent.注文数 = 520.5;
        //            ent.Is期限切迫 = true;
        //            ent.Isデッド品 = true;
        //            ent.Is注文あり = true;
        //            ent.包装形態 = "PTP";
        //            ent.包装単位数 = "14";
        //            ent.包装単位 = "カプセル";
        //            ent.包装総量 = "100";

        //            list.Add(ent);



        //        }

        //        OrderScheduleDic.Add(i.ToString(), list);

        //    }

        //}

        private void InitFields()
        {
            _CurrentMedicineIndex = 0;
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
                pd.DefaultPageSettings.Landscape = false;
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


            var list = OrderScheduleList.ToList(); // インデックス指定できる様ににListへ変換

            int Current総ページ数 = 0;

            if (list.Count <= _FirstPageMaxPrint)
            {
                Current総ページ数 = 1;
            }
            else
            {
                decimal deci = (decimal)(list.Count - _FirstPageMaxPrint) / (decimal)_OtherPageMaxPrint + 1;
                Current総ページ数 = (int)Math.Ceiling(deci);
            }


            //印刷する初期位置を決定
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top + 20;

            // ①見出し
            //印刷する文字列と位置を設定する
            var text1 = "未納品確定が必要な伝票No一覧表";
            //印刷に使うフォントを指定する
            var font1 = new Font(fontname, 20, FontStyle.Bold);

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
                // ④医薬品
                int fontsize = 10;
                int IndexCounter = -1;
                foreach (var row in list)
                {
                    IndexCounter++;
                    _CurrentMedicineIndex = IndexCounter;

                    if (IndexCounter == _FirstPageMaxPrint)
                    {
                        break;
                    }

                    // FontSize=15で全角1文字=幅20
                    var measureWidth = 0f;
                    int currentfontsize = fontsize;
                    int currentfontsize2 = fontsize;
                    int startX = 0;
                    var text = "";

                    // Column:0 伝票No
                    startX = x;
                    text = string.Format("伝票No{0}", row.レセ発注伝票No);
                    var font = new Font(fontname, fontsize);
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 80;


                    // Column:1 発注先
                    text = string.Format("{0}", row.帳合先名称);
                    font = new Font(fontname, fontsize);
                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 100)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }

                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 100;


                    // Column:2 医薬品名
                    text = string.Format("{0}", row.医薬品名);
                    font = new Font(fontname, currentfontsize2);

                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 300)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize2);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 300;


                    y += (int)font.GetHeight(e.Graphics) + 20;




                }

                // ⑤２ページ目以降印刷か
                if (list.Count <= _FirstPageMaxPrint)
                {
                }
                else
                {
                    _２ページ目以降印刷中 = true;
                }

                // ⑥１ページ目のページ番号

                Printページ番号(e, Current総ページ数, 1);

                _CurrentPageNumber = 2;

            }

            // ２ページ目以降印刷
            else
            {
                ////微調整
                //y += 20;

                int fontsize = 10;
                int IndexCounter2 = -1;
                foreach (var row in list)
                {
                    IndexCounter2++;

                    if (IndexCounter2 < _CurrentMedicineIndex)
                    {
                        continue;
                    }

                    _CurrentMedicineIndex = IndexCounter2;

                    // 最初の印刷前でなく、かつ最大個数印刷した後ならば、次のページへ
                    int check1 = _FirstPageMaxPrint + (_CurrentPageNumber - 2) * _OtherPageMaxPrint;
                    int check2 = (_CurrentMedicineIndex - _FirstPageMaxPrint) % _OtherPageMaxPrint;
                    if (check1 != _CurrentMedicineIndex && check2 == 0)
                    {
                        _２ページ目以降印刷中 = true;
                        break;
                    }

                    var measureWidth = 0f;
                    int currentfontsize = fontsize;
                    int currentfontsize2 = fontsize;
                    int startX = 0;
                    var text = "";

                    // Column:0 伝票No
                    startX = x;
                    text = string.Format("伝票No{0}", row.レセ発注伝票No);
                    var font = new Font(fontname, fontsize);
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 80;


                    // Column:1 発注先
                    text = string.Format("{0}", row.帳合先名称);
                    font = new Font(fontname, fontsize);
                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 100)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize);
                        }
                    }

                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 100;


                    // Column:2 医薬品名
                    text = string.Format("{0}", row.医薬品名);
                    font = new Font(fontname, currentfontsize2);

                    while (true)
                    {
                        measureWidth = e.Graphics.MeasureString(text, font).Width;
                        if (measureWidth < 300)
                        {
                            break;
                        }
                        else
                        {
                            font = new Font(fontname, --currentfontsize2);
                        }
                    }
                    e.Graphics.DrawString(text, font, Brushes.Black, startX, y);
                    startX += 300;


                    y += (int)font.GetHeight(e.Graphics) + 20;


                }

                // ページ番号
                Printページ番号(e, Current総ページ数, _CurrentPageNumber);

                // 全部医薬品を印刷したらフッター
                if (list.Count - 1 <= _CurrentMedicineIndex)
                {
                    _２ページ目以降印刷中 = false;
                }
                else
                {
                }

                _CurrentPageNumber++;

            }

            // この店舗の印刷が残っているか
            if (_２ページ目以降印刷中)
            {
                // 印刷継続を指定
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                InitFields();   // 全て印刷終了したらフィールドを初期化。これをしないとプレビュー時と印刷時でフィールドの値が保持されたままになる。
            }


        }

        private void Printページ番号(PrintPageEventArgs e, int 総ページ数, int 現在のページ数)
        {
            int x6 = 270;
            int y6 = 790;
            var text6 = string.Format("{0} / {1}", 現在のページ数, 総ページ数);
            var font6 = new Font(fontname, 12);
            e.Graphics.DrawString(text6, font6, Brushes.Gray, x6, y6);

        }

    }
}
