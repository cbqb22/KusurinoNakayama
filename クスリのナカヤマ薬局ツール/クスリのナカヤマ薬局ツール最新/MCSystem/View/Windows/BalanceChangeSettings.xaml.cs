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
using System.Windows.Controls.Primitives;
using WF = System.Windows.Forms;
using MCSystem.Model;
using MCSystem.ViewModel;

namespace MCSystem.View.Windows
{
    /// <summary>
    /// BalanceChangeSettings.xaml の相互作用ロジック
    /// </summary>
    public partial class BalanceChangeSettings : Window
    {

        private Rect _在庫メンテナンス受付範囲Rect;
        public Rect 在庫メンテナンス受付範囲Rect
        {
            get { return _在庫メンテナンス受付範囲Rect; }
            set { _在庫メンテナンス受付範囲Rect = value; }
        }

        private Rect _在庫メンテナンス範囲Rect;

        public Rect 在庫メンテナンス範囲Rect
        {
            get { return _在庫メンテナンス範囲Rect; }
            set { _在庫メンテナンス範囲Rect = value; }
        }

        private bool _IsSaved;

        public bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        //private bool _Is検索名称クリック;
        //public bool Is検索名称クリック
        //{
        //    get { return _Is検索名称クリック; }
        //    set { _Is検索名称クリック = value; }
        //}

        //private bool _Is通常仕入先クリック;

        public BalanceChangeSettings()
        {
            InitializeComponent();
            this.Closed += new EventHandler(BalanceChangeSettings_Closed);
            SetInit();
        }

        private void SetInit()
        {
            tb検索名称X.Text = ((int)DI.検索名称XY座標.X).ToString();
            tb検索名称Y.Text = ((int)DI.検索名称XY座標.Y).ToString();
            tb検索名称完了ボタンX.Text = ((int)DI.検索名称完了ボタンXY座標.X).ToString();
            tb検索名称完了ボタンY.Text = ((int)DI.検索名称完了ボタンXY座標.Y).ToString();
            tb通常仕入先X.Text = ((int)DI.通常仕入先XY座標.X).ToString();
            tb通常仕入先Y.Text = ((int)DI.通常仕入先XY座標.Y).ToString();
            tb個別入力完了ボタンX.Text = ((int)DI.個別入力完了ボタンXY座標.X).ToString();
            tb個別入力完了ボタンY.Text = ((int)DI.個別入力完了ボタンXY座標.Y).ToString();
            this._在庫メンテナンス受付範囲Rect = DI.在庫メンテナンス受付範囲;
            this._在庫メンテナンス範囲Rect = DI.在庫メンテナンス範囲;

            tb在庫テーブルCSVFilePath.Text = DI.在庫テーブルCSVパス;
            tb新帳合変更データ表FilePath.Text = DI.新帳合変更データ表パス;

            //tbメディセオ.Text = DI.メディセオコード == -1 ? "" : DI.メディセオコード.ToString();
            //tbスズケン.Text = DI.スズケンコード == -1 ? "" : DI.スズケンコード.ToString();
            //tbアルフレッサ.Text = DI.アルフレッサコード == -1 ? "" : DI.アルフレッサコード.ToString();
            //tb酒井薬品.Text = DI.酒井薬品コード == -1 ? "" : DI.酒井薬品コード.ToString();
            //tb東邦薬品.Text = DI.東邦薬品コード == -1 ? "" : DI.東邦薬品コード.ToString();
            //tb東和.Text = DI.東和薬品コード == -1 ? "" : DI.東和薬品コード.ToString();
        }

        void BalanceChangeSettings_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;

            SingletonWindows.XYLocationWindow.Close();

            // メニューを前面にする
            SingletonWindows.BalanceChangeMenuWindow.WindowState = System.Windows.WindowState.Normal;
            SingletonWindows.BalanceChangeMenuWindow.Show();
            SingletonWindows.BalanceChangeMenuWindow.Activate();
        }

        private void tbtn検索名称クリック取得_Click(object sender, RoutedEventArgs e)
        {

        }


        private void tbtn通常仕入先クリック取得_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbtn検索名称完了ボタンクリック取得_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbtn個別入力完了ボタンクリック取得_Click(object sender, RoutedEventArgs e)
        {

        }


        private void tbtn検索名称クリック取得_Checked(object sender, RoutedEventArgs e)
        {
            tbtn個別入力完了ボタンクリック取得.IsChecked = false;
            tbtn検索名称完了ボタンクリック取得.IsChecked = false;
            tbtn通常仕入先クリック取得.IsChecked = false;
            tbtn在庫メンテナンスDrag取得.IsChecked = false;
            tbtn在庫メンテナンス受付Drag取得.IsChecked = false;
        }

        private void tbtn通常仕入先クリック取得_Checked(object sender, RoutedEventArgs e)
        {
            tbtn検索名称完了ボタンクリック取得.IsChecked = false;
            tbtn検索名称クリック取得.IsChecked = false;
            tbtn個別入力完了ボタンクリック取得.IsChecked = false;
            tbtn在庫メンテナンスDrag取得.IsChecked = false;
            tbtn在庫メンテナンス受付Drag取得.IsChecked = false;
        }

        private void tbtn検索名称完了ボタンクリック取得_Checked(object sender, RoutedEventArgs e)
        {
            tbtn通常仕入先クリック取得.IsChecked = false;
            tbtn検索名称クリック取得.IsChecked = false;
            tbtn個別入力完了ボタンクリック取得.IsChecked = false;
            tbtn在庫メンテナンスDrag取得.IsChecked = false;
            tbtn在庫メンテナンス受付Drag取得.IsChecked = false;
        }

        private void tbtn個別入力完了ボタンクリック取得_Checked(object sender, RoutedEventArgs e)
        {
            tbtn検索名称完了ボタンクリック取得.IsChecked = false;
            tbtn通常仕入先クリック取得.IsChecked = false;
            tbtn検索名称クリック取得.IsChecked = false;
            tbtn在庫メンテナンスDrag取得.IsChecked = false;
            tbtn在庫メンテナンス受付Drag取得.IsChecked = false;

        }

        private void btn保存_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void btn閉じる_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSaved && MessageBox.Show("設定を保存しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (SaveSettings() == false)
                {
                    return;
                }
            }


            this.Close();
        }

        private bool SaveSettings()
        {
            SingletonWindows.XYLocationWindow.Hide();

            try
            {
                BCSettingsEntity ent = new BCSettingsEntity();

                double 検索名称X;
                if (double.TryParse(tb検索名称X.Text, out 検索名称X) == false)
                {
                    MessageBox.Show("検索名称Xには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                double 検索名称Y;
                if (double.TryParse(tb検索名称Y.Text, out 検索名称Y) == false)
                {
                    MessageBox.Show("検索名称Yには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                double 検索名称完了ボタンX;
                if (double.TryParse(tb検索名称完了ボタンX.Text, out 検索名称完了ボタンX) == false)
                {
                    MessageBox.Show("検索名称完了ボタンXには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                double 検索名称完了ボタンY;
                if (double.TryParse(tb検索名称完了ボタンY.Text, out 検索名称完了ボタンY) == false)
                {
                    MessageBox.Show("検索名称完了ボタンYには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                double 通常仕入先X;
                if (double.TryParse(tb通常仕入先X.Text, out 通常仕入先X) == false)
                {
                    MessageBox.Show("通常仕入先Xには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                double 通常仕入先Y;
                if (double.TryParse(tb通常仕入先Y.Text, out 通常仕入先Y) == false)
                {
                    MessageBox.Show("通常仕入先Yには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                double 個別入力完了ボタンX;
                if (double.TryParse(tb個別入力完了ボタンX.Text, out 個別入力完了ボタンX) == false)
                {
                    MessageBox.Show("個別入力完了ボタンXには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                double 個別入力完了ボタンY;
                if (double.TryParse(tb個別入力完了ボタンY.Text, out 個別入力完了ボタンY) == false)
                {
                    MessageBox.Show("個別入力完了ボタンYには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                if (_在庫メンテナンス受付範囲Rect == null)
                {
                    MessageBox.Show("在庫メンテナンス受付範囲をDrag取得より設定してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                if (_在庫メンテナンス範囲Rect == null)
                {
                    MessageBox.Show("在庫メンテナンス範囲をDrag取得より設定してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                if (string.IsNullOrEmpty(tb新帳合変更データ表FilePath.Text))
                {
                    MessageBox.Show("新帳合変更データ表ファイルパスを参照ボタンより入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                if (string.IsNullOrEmpty(tb在庫テーブルCSVFilePath.Text))
                {
                    MessageBox.Show("tb在庫テーブルCSVファイルパスを参照ボタンより入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                //int メディセオ = -1;
                //if (tbメディセオ.Text != "" && int.TryParse(tbメディセオ.Text, out メディセオ) == false)
                //{
                //    MessageBox.Show("メディセオには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //    return false;
                //}

                //int スズケン = -1;
                //if (tbスズケン.Text != "" && int.TryParse(tbスズケン.Text, out スズケン) == false)
                //{
                //    MessageBox.Show("スズケンには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //    return false;
                //}
                //int アルフレッサ = -1;
                //if (tbアルフレッサ.Text != "" && int.TryParse(tbアルフレッサ.Text, out アルフレッサ) == false)
                //{
                //    MessageBox.Show("アルフレッサには半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //    return false;
                //}
                //int 東邦薬品 = -1;
                //if (tb東邦薬品.Text != "" && int.TryParse(tb東邦薬品.Text, out 東邦薬品) == false)
                //{
                //    MessageBox.Show("東邦薬品には半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //    return false;
                //}
                //int 東和 = -1;
                //if (tb東和.Text != "" && int.TryParse(tb東和.Text, out 東和) == false)
                //{
                //    MessageBox.Show("東和には半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //    return false;
                //}
                //int 酒井薬品 = -1;
                //if (tb酒井薬品.Text != "" && int.TryParse(tb酒井薬品.Text, out 酒井薬品) == false)
                //{
                //    MessageBox.Show("酒井薬品には半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //    return false;
                //}

                ent.検索名称XY座標 = new Rect(検索名称X, 検索名称Y, 0, 0);
                ent.検索名称完了ボタンXY座標 = new Rect(検索名称完了ボタンX, 検索名称完了ボタンY, 0, 0);
                ent.通常仕入先XY座標 = new Rect(通常仕入先X, 通常仕入先Y, 0, 0);
                ent.個別入力完了ボタンXY座標 = new Rect(個別入力完了ボタンX, 個別入力完了ボタンY, 0, 0);
                ent.新帳合変更データ表パス = tb新帳合変更データ表FilePath.Text;
                ent.在庫テーブルCSVパス = tb在庫テーブルCSVFilePath.Text;
                ent.在庫メンテナンス受付範囲 = this._在庫メンテナンス受付範囲Rect;
                ent.在庫メンテナンス範囲 = this._在庫メンテナンス範囲Rect;
                //ent.メディセオコード = メディセオ;
                //ent.スズケンコード = スズケン;
                //ent.アルフレッサコード = アルフレッサ;
                //ent.酒井薬品コード = 酒井薬品;
                //ent.東邦薬品コード = 東邦薬品;
                //ent.東和薬品コード = 東和;

                ControlBCSettings.WriteSettings(ent);
                ControlBCSettings.SetSettingsToDI(ent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生した為、保存できませんでした。" + ex.Message + ex.StackTrace,"保存エラー",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }

            IsSaved = true;
            return true;
        }

        private void btn在庫テーブル参照_Click(object sender, RoutedEventArgs e)
        {
            WF.OpenFileDialog ofd = new WF.OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if (ofd.ShowDialog() == WF.DialogResult.OK)
            {
                tb在庫テーブルCSVFilePath.Text = ofd.FileName;
            }


        }

        private void btn新帳合変更参照_Click(object sender, RoutedEventArgs e)
        {
            WF.OpenFileDialog ofd = new WF.OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if (ofd.ShowDialog() == WF.DialogResult.OK)
            {
                tb新帳合変更データ表FilePath.Text = ofd.FileName;
            }


        }

        private void tbtn在庫メンテナンス受付Drag取得_Checked(object sender, RoutedEventArgs e)
        {
            tbtn個別入力完了ボタンクリック取得.IsChecked = false;
            tbtn検索名称完了ボタンクリック取得.IsChecked = false;
            tbtn通常仕入先クリック取得.IsChecked = false;
            tbtn検索名称クリック取得.IsChecked = false;
            tbtn在庫メンテナンスDrag取得.IsChecked = false;

        }

        private void tbtn在庫メンテナンスDrag取得_Checked(object sender, RoutedEventArgs e)
        {
            tbtn個別入力完了ボタンクリック取得.IsChecked = false;
            tbtn検索名称完了ボタンクリック取得.IsChecked = false;
            tbtn通常仕入先クリック取得.IsChecked = false;
            tbtn検索名称クリック取得.IsChecked = false;
            tbtn在庫メンテナンス受付Drag取得.IsChecked = false;

        }


    }
}
