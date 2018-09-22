using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using WF = System.Windows.Forms;
using MCSystem.Model;


namespace MCSystem.View.Windows
{
    /// <summary>
    /// OperationAdd.xaml の相互作用ロジック
    /// </summary>
    public partial class OperationAdd : Window, INotifyPropertyChanged
    {

        private OriginalMacroMaker _HostWindow;
        public OriginalMacroMaker HostWindow
        {
            get { return _HostWindow; }
            set { _HostWindow = value; }
        }


        private Rect _ClickRect;

        public Rect ClickRect
        {
            get { return _ClickRect; }
            set
            {
                _ClickRect = value;
                FirePropertyChanged("ClickRect");
            }
        }

        private Rect _ImageMatchRect;
        public Rect ImageMatchRect
        {
            get { return _ImageMatchRect; }
            set
            {
                _ImageMatchRect = value;
                FirePropertyChanged("ImageMatchRect");
            }
        }


        private DragMeasure _Drag;
        public DragMeasure Drag
        {
            get { return _Drag; }
            set 
            {
                _Drag = value;
                FirePropertyChanged("Drag");
            }
        }





        public OperationAdd()
        {
            InitializeComponent();
            this.Closed += new EventHandler(OperationAdd_Closed);

            this.DataContext = this;
        }

        void OperationAdd_Closed(object sender, EventArgs e)
        {
            SingletonWindows.XYLocationWindow.Close();
            HostWindow.Show();
            HostWindow.Activate();
        }


        private void btn追加_Click(object sender, RoutedEventArgs e)
        {

            if (rbtnClickOperation.IsChecked == false && 
                rbtnInputOperation.IsChecked == false && 
                rbtn終了Operation.IsChecked == false && 
                rbtn待機Operation.IsChecked == false && 
                rbtnImageMatchOperation.IsChecked == false &&
                rbtnDragOperation.IsChecked == false &&
                rbtnDragFastOperation.IsChecked == false &&
                rbtnScreenShotOperation.IsChecked == false
                )
            {
                MessageBox.Show("操作する内容にチェックを１つ入れて下さい。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if (rbtnClickOperation.IsChecked == true)
            {
                int clickx;
                if (int.TryParse(tbClickX.Text, out clickx) == false)
                {
                    MessageBox.Show("X座標に数字を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int clicky;
                if (int.TryParse(tbClickY.Text, out clicky) == false)
                {
                    MessageBox.Show("Y座標に数字を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (rbtnDragOperation.IsChecked == true || rbtnDragFastOperation.IsChecked == true)
            {
                int DragX;
                if (int.TryParse(tbDragX.Text, out DragX) == false)
                {
                    MessageBox.Show("DragX座標に数字を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int DragY;
                if (int.TryParse(tbDragY.Text, out DragY) == false)
                {
                    MessageBox.Show("DragY座標に数字を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int Width;
                if (int.TryParse(tbDragWidth.Text, out Width) == false)
                {
                    MessageBox.Show("Widthに数字を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int Height;
                if (int.TryParse(tbDragHeight.Text, out Height) == false)
                {
                    MessageBox.Show("Heightに数字を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }


            int waittime = 0;
            if (rbtn待機Operation.IsChecked == true)
            {
                if (int.TryParse(tbSleepTime.Text, out waittime) == false)
                {
                    MessageBox.Show("待機時間に数字を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }

            if (rbtnInputOperation.IsChecked == true && string.IsNullOrEmpty(tbInputData.Text))
            {
                MessageBox.Show("入力データを入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if (rbtnImageMatchOperation.IsChecked == true)
            {
                if (this.ImageMatchRect == null || this.ImageMatchRect.X == 0 || this.ImageMatchRect.Y == 0)
                {
                    MessageBox.Show("画像一致のDrag取得から範囲を指定してください。", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (rbtnScreenShotOperation.IsChecked == true && string.IsNullOrEmpty(tbSaveScreenshotPath.Text))
            {
                MessageBox.Show("スクリーンショットの保存先を入れてください", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OriginalMacroDetailEntity omdEnt = new OriginalMacroDetailEntity();

            if (rbtnClickOperation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.Click;
                omdEnt.操作座標 = this.ClickRect;
                //omdEnt.操作座標 = new Rect(double.Parse(tbClickX.Text), double.Parse(tbClickY.Text), 0, 0);
            }
            else if (rbtnDragOperation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.Drag;
                omdEnt.Drag座標 = this.Drag;
            }
            else if (rbtnDragFastOperation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.DragFast; //OperationはDragFast
                omdEnt.Drag座標 = this.Drag;
            }
            else if (rbtnInputOperation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.Input;
                omdEnt.入力データ = tbInputData.Text;
            }
            else if (rbtn待機Operation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.Sleep;
                omdEnt.待機時間 = waittime;
            }
            else if (rbtnImageMatchOperation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.ImageMatch;
                omdEnt.操作座標 = this.ImageMatchRect;
            }
            else if (rbtnScreenShotOperation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.ScreenShot;
                omdEnt.入力データ = tbSaveScreenshotPath.Text;
            }
            else if (rbtn終了Operation.IsChecked == true)
            {
                omdEnt.OperationEnum = Enum.MacroOperationEnum.End;
            }

            omdEnt.IsUnMatchOperation = (bool)chkUnMatchTask.IsChecked;

            HostWindow.lvMacroContents.ItemsSource = null;

            HostWindow.LvItemssource.Add(omdEnt);
            HostWindow.lvMacroContents.ItemsSource = HostWindow.LvItemssource;

            this.Close();
        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void tbtnClick取得_Checked(object sender, RoutedEventArgs e)
        {
            //自分以外のToggleはOFFにする
            tbtnDragOperationDrag取得.IsChecked = false;
            tbtnImageMatchDrag取得.IsChecked = false;
        }


        private void tbtnImageMatchDrag取得_Checked(object sender, RoutedEventArgs e)
        {
            //自分以外のToggleはOFFにする
            tbtnClick取得.IsChecked = false;
            tbtnDragOperationDrag取得.IsChecked = false;

        }

        private void tbtnDragOperationDrag取得_Checked(object sender, RoutedEventArgs e)
        {
            //自分以外のToggleはOFFにする
            tbtnClick取得.IsChecked = false;
            tbtnImageMatchDrag取得.IsChecked = false;
        }

        #region INotifyPropertyChanged メンバ

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private void btnGetScreenshotPath_Click(object sender, RoutedEventArgs e)
        {
            WF.FolderBrowserDialog fbd = new WF.FolderBrowserDialog();
            WF.DialogResult dr = fbd.ShowDialog();

            if (dr == WF.DialogResult.OK)
            {
                tbSaveScreenshotPath.Text = fbd.SelectedPath;
            }

        }

    }

}
