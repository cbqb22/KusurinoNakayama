using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Reflection;
using MCSystem.ViewModel.Common.Image;
using MCSystem.Model;


namespace MCSystem.View.Windows
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class XYLocation : Window
    {

        private Point _position;
        private bool _trimEnable = false;


        private Window _ControlWindow;
        public Window ControlWindow
        {
            get { return _ControlWindow; }
            set { _ControlWindow = value; }
        }


        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }


        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void SetCursorPos(int X, int Y);


        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;


        public XYLocation()
        {
            InitializeComponent();

            this.MouseMove += new MouseEventHandler(MainWindow_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(MainWindow_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(XYLocation_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(XYLocation_MouseMove);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Closed += new EventHandler(XYLocation_Closed);
            //this.StateChanged += new EventHandler(XYLocation_StateChanged);



        }

        void XYLocation_MouseMove(object sender, MouseEventArgs e)
        {


            if (!_trimEnable)
                return;

            var posi = e.GetPosition(this);
            //var scrposi = this.PointToScreen(posi);



            // キャプチャ領域枠の描画
            DrawStroke(posi);
        }

        void XYLocation_StateChanged(object sender, EventArgs e)
        {
            var window = sender as XYLocation;
            if (window == null)
            {
                return;
            }

            //ノーマルに変更されたならば
            if (window.WindowState == System.Windows.WindowState.Normal)
            {
                SingletonWindows.BalanceChangeSettingsWindow.Show();
                SingletonWindows.BalanceChangeSettingsWindow.Activate();
            }

        }


        void XYLocation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (ControlWindow == null)
            {
                return;
            }

            if (ControlWindow is BalanceChangeSettings &&
                (SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンス受付Drag取得.IsChecked == true || SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンスDrag取得.IsChecked == true))
            {
                // マウスキャプチャの設定
                _trimEnable = true;
            }
            else if (ControlWindow is OperationAdd)
            {
                var oa = ControlWindow as OperationAdd;
                if (oa.tbtnImageMatchDrag取得.IsChecked == true || oa.tbtnDragOperationDrag取得.IsChecked == true)
                {
                    // マウスキャプチャの設定
                    _trimEnable = true;
                }
            }

            this.Cursor = Cursors.Cross;
            var posi = e.GetPosition(this);
            //var scrposi = this.PointToScreen(posi);
            _position = posi;



            ControlWindow.Show();
            ControlWindow.Activate();


            //SingletonWindows.BalanceChangeSettingsWindow.Show();
            //SingletonWindows.BalanceChangeSettingsWindow.Activate();

        }

        void XYLocation_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var rect = System.Windows.SystemParameters.WorkArea;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            ToMaxWindow();

            // ジオメトリサイズの設定
            this.ScreenArea.Geometry1 = new RectangleGeometry(new Rect(rect.X, rect.Y, rect.Width, rect.Height));
        }

        private void ToMinWindow()
        {
            var rect = System.Windows.SystemParameters.WorkArea;
            this.Height = 100;
            this.Width = 200;
            //this.bdTransparent.Width = 200;
            //this.bdTransparent.Height = 100;

        }

        private void ToMaxWindow()
        {
            var rect = System.Windows.SystemParameters.WorkArea;
            this.Height = rect.Height;
            this.Width = rect.Width;
            this.bdTransparent.Width = rect.Width;
            this.bdTransparent.Height = rect.Height;


        }

        private void DrawStroke(Point point)
        {
            // 矩形の描画
            var x = _position.X < point.X ? _position.X : point.X;
            var y = _position.Y < point.Y ? _position.Y : point.Y;
            var width = Math.Abs(point.X - _position.X);
            var height = Math.Abs(point.Y - _position.Y);

            this.ScreenArea.Geometry2 = new RectangleGeometry(new Rect(x, y, width, height));

            System.Diagnostics.Debug.WriteLine(string.Format("x:{0} y:{1} width:{2} height:{3}", x, y, width, height));

        }

        void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ControlWindow == null)
            {
                return;
            }

            var posi = e.GetPosition(this);
            var scrposi = this.PointToScreen(posi);

            this.Cursor = Cursors.Arrow;




            if (ControlWindow is BalanceChangeSettings)
            {
                if (SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンス受付Drag取得.IsChecked == true || SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンスDrag取得.IsChecked == true)
                {
                    // マウスキャプチャの終了
                    _trimEnable = false;

                    CaptureScreen(posi);

                }
                else if (SingletonWindows.BalanceChangeSettingsWindow.tbtn検索名称クリック取得.IsChecked == true)
                {
                    SingletonWindows.BalanceChangeSettingsWindow.tb検索名称X.Text = scrposi.X.ToString();
                    SingletonWindows.BalanceChangeSettingsWindow.tb検索名称Y.Text = scrposi.Y.ToString();

                    SingletonWindows.BalanceChangeSettingsWindow.tbtn検索名称クリック取得.IsChecked = false;
                }
                else if (SingletonWindows.BalanceChangeSettingsWindow.tbtn通常仕入先クリック取得.IsChecked == true)
                {
                    SingletonWindows.BalanceChangeSettingsWindow.tb通常仕入先X.Text = scrposi.X.ToString();
                    SingletonWindows.BalanceChangeSettingsWindow.tb通常仕入先Y.Text = scrposi.Y.ToString();

                    SingletonWindows.BalanceChangeSettingsWindow.tbtn通常仕入先クリック取得.IsChecked = false;
                }
                else if (SingletonWindows.BalanceChangeSettingsWindow.tbtn検索名称完了ボタンクリック取得.IsChecked == true)
                {
                    SingletonWindows.BalanceChangeSettingsWindow.tb検索名称完了ボタンX.Text = scrposi.X.ToString();
                    SingletonWindows.BalanceChangeSettingsWindow.tb検索名称完了ボタンY.Text = scrposi.Y.ToString();

                    SingletonWindows.BalanceChangeSettingsWindow.tbtn検索名称完了ボタンクリック取得.IsChecked = false;
                }
                else if (SingletonWindows.BalanceChangeSettingsWindow.tbtn個別入力完了ボタンクリック取得.IsChecked == true)
                {
                    SingletonWindows.BalanceChangeSettingsWindow.tb個別入力完了ボタンX.Text = scrposi.X.ToString();
                    SingletonWindows.BalanceChangeSettingsWindow.tb個別入力完了ボタンY.Text = scrposi.Y.ToString();

                    SingletonWindows.BalanceChangeSettingsWindow.tbtn個別入力完了ボタンクリック取得.IsChecked = false;
                }
                else
                {
                    return;
                }
            }
            else if (ControlWindow is OperationAdd)
            {
                var oa = ControlWindow as OperationAdd;
                if (oa.tbtnImageMatchDrag取得.IsChecked == true || oa.tbtnDragOperationDrag取得.IsChecked == true)
                {
                    // マウスキャプチャの終了
                    _trimEnable = false;

                    CaptureScreen(posi);

                }
                else if (oa.tbtnClick取得.IsChecked == true)
                {

                    oa.ClickRect = new Rect(scrposi.X, scrposi.Y,0,0);
                    //oa.tbClickX.Text = scrposi.X.ToString();
                    //oa.tbClickY.Text = scrposi.Y.ToString();
                }
            }


            //MessageBox.Show(string.Format("X:{0} Y:{1}",scrposi.X,scrposi.Y));

            //SetCursorPos(0, 0);
            //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);



        }

        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            var posi = e.GetPosition(this);
            var scrposi = this.PointToScreen(posi);


            tblXYPosition.Text = string.Format("x:{0} y:{1}", scrposi.X, scrposi.Y);

        }

        private bool _IsSmall;

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            if (_IsSmall)
            {
                btnHide.Content = "最小化";
                ToMaxWindow();
                _IsSmall = false;

                if (ControlWindow != null)
                {
                    ControlWindow.Activate();
                }

            }
            else
            {
                btnHide.Content = "最大化";
                ToMinWindow();
                _IsSmall = true;

                if (ControlWindow != null)
                {
                    ControlWindow.Activate();
                }

            }




            //this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void CaptureScreen(Point point)
        {
            // 座標変換
            var start = PointToScreen(_position);
            var end = PointToScreen(point);

            // キャプチャエリアの取得
            var x = start.X < end.X ? (int)start.X : (int)end.X;
            var y = start.Y < end.Y ? (int)start.Y : (int)end.Y;
            var width = (int)Math.Abs(end.X - start.X);
            var height = (int)Math.Abs(end.Y - start.Y);
            if (width == 0 || height == 0)
                return;

            // スクリーンイメージの取得
            using (var bmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            using (var graph = System.Drawing.Graphics.FromImage(bmp))
            {
                // 画面をコピーする
                graph.CopyFromScreen(new System.Drawing.Point(x, y), new System.Drawing.Point(), bmp.Size);


                if (SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンス受付Drag取得.IsChecked == true)
                {
                    SingletonWindows.BalanceChangeSettingsWindow.img在庫メンテナンス受付.Source = BitmapChecker.ToBitmapSource(bmp);
                    SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンス受付Drag取得.IsChecked = false;
                    SingletonWindows.BalanceChangeSettingsWindow.在庫メンテナンス受付範囲Rect = new Rect(x, y, width, height);
                }
                else if (SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンスDrag取得.IsChecked == true)
                {
                    SingletonWindows.BalanceChangeSettingsWindow.img在庫メンテナンス.Source = BitmapChecker.ToBitmapSource(bmp);
                    SingletonWindows.BalanceChangeSettingsWindow.tbtn在庫メンテナンスDrag取得.IsChecked = false;
                    SingletonWindows.BalanceChangeSettingsWindow.在庫メンテナンス範囲Rect = new Rect(x, y, width, height);
                }
                else if (ControlWindow is OperationAdd)
                {
                    var oa = ControlWindow as OperationAdd;

                    if (oa.tbtnImageMatchDrag取得.IsChecked == true)
                    {
                        oa.imgImageMatch.Source = BitmapChecker.ToBitmapSource(bmp);
                        oa.tbtnImageMatchDrag取得.IsChecked = false;

                        //イメージキャプチャは開始、終了が逆になってもOK。
                        oa.ImageMatchRect = new Rect(x, y, width, height);
                    }
                    else if (oa.tbtnDragOperationDrag取得.IsChecked == true)
                    {
                        oa.tbtnDragOperationDrag取得.IsChecked = false;

                        //ドラッグは開始、終了が逆になってはいけないので、end-startを引いて算出する。
                        oa.Drag = new DragMeasure(start.X,start.Y,end.X,end.Y);
                        //oa.DragMeasure = new Rect(start.X, start.Y, end.X - start.X, end.Y - start.Y);
                    }
                }




                // イメージの保存
                //string folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //bmp.Save(System.IO.Path.ChangeExtension(System.IO.Path.Combine(folder, "image"), "png"), System.Drawing.Imaging.ImageFormat.Png);
            }
        }








    }
}
