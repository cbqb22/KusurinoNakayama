﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;//実際は不要
using System.Windows.Navigation;//実際は不要
using System.Windows.Shapes;//実際は不要
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Printing;
using System.Drawing.Printing;
using System.IO.Packaging;
using form = System.Windows.Forms;

namespace クスリのナカヤマ薬局ツール.共通.Printing
{
    class PrintCenter
    {
        private XpsDocument _xpsDocument;
        private readonly Uri _packageUri;
        private FixedPage _FixedPage;

        private UIElement _uielement;

        Canvas printCanvas;
        Label lblHello;

        public PrintCenter(UIElement ui)
        {

            this._uielement = ui;

            this._xpsDocument = null;
            this._packageUri = new Uri("pack://work.xps");

            MakeCanvasForPrint();


            FixedDocument fixedDocument
                                 = CreateFixedDocumentFromFixedPages();


            SendFixedDocumentToPrinter(fixedDocument);

            CreateImageFromXPS();

            //《コード：４》
        }


        public void CreateImageFromXPS()
        {
            Uri uri = new Uri(string.Format("memorystream://{0}", "file.xps"));
            FixedDocumentSequence seq;


            DocumentPaginator paginator = null;
            paginator = this._xpsDocument.GetFixedDocumentSequence().DocumentPaginator;
            Visual visual = paginator.GetPage(0).Visual;  // first page - loop for all

            FrameworkElement fe = (FrameworkElement)visual;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)fe.ActualWidth,
                                      (int)fe.ActualHeight, 96d, 96d, PixelFormats.Default);
            bmp.Render(fe);

            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(bmp));

            using (Stream stream = System.IO.File.Create(@"c:\tempNakayama.png"))
            {
                png.Save(stream);
            }

            //PrintDocumentオブジェクトの作成
            System.Drawing.Printing.PrintDocument pd =
                new System.Drawing.Printing.PrintDocument();
            //PrintPageイベントハンドラの追加
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            //印刷を開始する
            //pd.Print();
            pd.DefaultPageSettings.Landscape = true;

            //PrintPreviewDialogオブジェクトの作成
            form.PrintPreviewDialog ppd = new form.PrintPreviewDialog();
            //プレビューするPrintDocumentを設定
            ppd.Document = pd;
            //印刷プレビューダイアログを表示する

            //ppd.Width = (int)App.Current.MainWindow.MaxWidth;
            form.Form f = new form.Form();
            f.WindowState = form.FormWindowState.Maximized;
            App.Current.MainWindow.Hide();

            ppd.ShowDialog();

            //form.PrintPreviewControl ppc = new form.PrintPreviewControl();
            //ppc.Document = pd;
            //ppc.InvalidatePreview();
            //f.Controls.Add(ppc);
            //f.ShowDialog();

            form.PageSetupDialog psd = new form.PageSetupDialog();
            psd.PageSettings = new PageSettings();
            psd.PrinterSettings = new PrinterSettings();
            psd.ShowNetwork = false;
            psd.ShowDialog();





            App.Current.MainWindow.Show();





        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //画像を読み込む
            System.Drawing.Image img = System.Drawing.Image.FromFile(@"c:\tempNakayama.png");
            //画像を描画する

            e.Graphics.DrawImage(img, 0, 0);
            //次のページがないことを通知する
            e.HasMorePages = false;
            //後始末をする
            img.Dispose();
        }

        public static IDisposable StorePackage(Uri uri, Package package)
        {
            PackageStore.AddPackage(uri, package);
            return new Disposer(() => PackageStore.RemovePackage(uri));
        }

        public class Disposer : IDisposable
        {
            private bool _disposed = false;
            private Action _onDisposal;

            public Disposer(Action onDisposal)
            {
                _onDisposal = onDisposal;
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _onDisposal();
                }
            }
        }
        //-------------------------------------------------
        //《コード：４》　［第４段階］　Printer(Queue)への書き出し
        //-------------------------------------------------   


        public void RemoveFixedPageChild()
        {
            this._FixedPage.Children.Remove(this._uielement);

        }
        void 通常使用するプリンタの設定(string printerName)
        {
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher("Select * from Win32_Printer");
            System.Management.ManagementObjectCollection moc = mos.Get();

            //プリンタを列挙する
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (((string)mo["Name"]) == printerName)
                {
                    //名前を見つけたとき、デフォルトプリンタに設定する
                    System.Management.ManagementBaseObject mbo = mo.InvokeMethod("SetDefaultPrinter", null, null);
                    if (((uint)mbo["returnValue"]) != 0)
                    {
                        throw new System.Exception("プリントエラー");
                    }
                }
            }
        }


        void XpsDocumentの作成()
        {

            /*** XpsDocument の作成 ***/
            MemoryStream packageStream = new MemoryStream();
            Package package = Package.Open(packageStream, FileMode.Create, FileAccess.ReadWrite);
            // メモリ上の XPS を DocumentViewer に表示するためには Package に URI を割り当てる必要がある
            PackageStore.AddPackage(this._packageUri, package);
            this._xpsDocument = new XpsDocument(package, CompressionOption.NotCompressed, this._packageUri.AbsoluteUri);
        }

        void SendFixedDocumentToPrinter(FixedDocument fixedDocument)
        {
            PrintDocument pd = new PrintDocument();
            var ip = PrinterSettings.InstalledPrinters;
            pd.DefaultPageSettings.Landscape = true;

            this.XpsDocumentの作成();

            //this.通常使用するプリンタの設定("Canon MP500 Series Printer");

            //LocalPrintServer ps = new LocalPrintServer();
            //PrintQueue pq = ps.DefaultPrintQueue;

            ////PrintDocumentImageableArea imgArea = null;

            XpsDocumentWriter xpsdw
                         = XpsDocument.CreateXpsDocumentWriter(this._xpsDocument);

            ////XpsDocumentWriter xpsdw
            ////             = PrintQueue.CreateXpsDocumentWriter(ref imgArea);



            ////上記メソッドの引数並びは、いろいろな種類があり、
            ////それによって、印刷ダイアログを表示する。
            ////PrintDialog クラスがあることとの関係は、まだよくわかりません。

            //// プレビューは独自のUIで表示する

            // 印刷がかかる
            xpsdw.Write(fixedDocument);

            // this.DocumentViewer.Document = this._xpsDocument.GetFixedDocumentSequence();
        }

        //-------------------------------------------------
        //《コード：３》［第３段階］FixedPage から FixedDocument を作る
        //-------------------------------------------------   

        private FixedDocument CreateFixedDocumentFromFixedPages()
        {
            FixedDocument fixedDocument = new FixedDocument();
            fixedDocument.DocumentPaginator.PageSize
                                                      = new Size(96 * 8.5, 96 * 11);
            //---------------------------------------------
            PageContent pageContent = CreateFixedPageFromCanvas();
            //ここで《コード：２》が使われる
            fixedDocument.Pages.Add(pageContent);
            //複数ページの場合は、ここで複数回 Add()を繰り返す。
            //このプログラムでは、１ページのみを作る。
            return fixedDocument;
        }//CreateFixedDocumentFromFixedPages()


        //-------------------------------------------------
        //《コード：２》    ［第２段階］Canvas の内容を 
        //                    FixedPage(=>そして 最後に PageContent) にする
        //-------------------------------------------------   

        PageContent CreateFixedPageFromCanvas()
        {
            //作られた FixedPage は、最後に PageContent の子要素とされて、
            //次の FixedDocument 作成の準備が整う
            PageContent pageContent = new PageContent();
            _FixedPage = new FixedPage();

            //Canvas がFixedPage(=印刷用紙) 中のどこに位置づけられるか。
            FixedPage.SetLeft(printCanvas, 0);
            FixedPage.SetTop(printCanvas, 0);

            //この近辺に、Canvas サイズを用紙サイズに合わせる
            //調整用のコードが加わってくるものと考えられる。

            //double pageWidth = 96 * 8.5;
            //double pageHeight = 96 * 11;

            //double pageWidth = 980;
            //double pageHeight = 700;

            var cv = this._uielement as Canvas;


            double pageWidth = cv.ActualWidth;
            double pageHeight = cv.ActualHeight;

            _FixedPage.Width = pageWidth;
            _FixedPage.Height = pageHeight;

            try
            {

                _FixedPage.Children.Add((UIElement)printCanvas);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);

            }

            //printCanvas=親(例えばWindow)があるとエラーになる
            //これが、Canvas は親要素を持ってはならない、という根拠

            Size sz = new Size(pageWidth, pageHeight);
            _FixedPage.Measure(sz);
            _FixedPage.Arrange(new Rect(new Point(), sz));
            _FixedPage.UpdateLayout();

            ((IAddChild)pageContent).AddChild(_FixedPage);
            return pageContent;
        }//CreateFixedPageFromCanvas() 

        //-------------------------------------------------
        //《コード：１》［第１段階］Canvas とその内容の用意
        //-------------------------------------------------

        void MakeCanvasForPrint()
        {
            //Canvas 全体が、１ページになる
            //this.WindowState = WindowState.Maximized;//Window
            //---------------  Canvas  --------------------
            //printCanvas = new Canvas();
            printCanvas = this._uielement as Canvas;
            // ここで this.Content = printCanvas; を
            // 絶対入れてはならない（親要素を持たない）
            printCanvas.Background = Brushes.Transparent;
            //printCanvas.Width = 980;//数値はともに適当
            //printCanvas.Height = 700;
            //後述の数値（8.5 * 96, 11 * 96）と不整合
            //とりあえず、御勘弁を！

            //try
            //{
            //    printCanvas.Children.Add(this._uielement);
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show(ex.Message + ex.StackTrace);
            //}

            ////--------------  Label  ------------------------
            //lblHello = new Label();
            //printCanvas.Children.Add(lblHello);
            //lblHello.Background = Brushes.Transparent;
            //lblHello.Width = 160;
            //lblHello.Height = 30;
            //Canvas.SetTop(lblHello, 0);
            //Canvas.SetLeft(lblHello, 0);
            //lblHello.Content = "Hello World !";//これを印刷するのが最終目標           
        }//MakeCanvasForPrint()


    }
}
