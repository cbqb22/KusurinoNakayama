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
using System.Windows.Shapes;
using System.Drawing.Printing;
using OASystem.ViewModel.File;

namespace OASystem.View.Windows
{
    /// <summary>
    /// Settings.xaml の相互作用ロジック
    /// </summary>
    public partial class Settings : Window
    {

        private bool _HasChange;
        public bool HasChange
        {
            get { return _HasChange; }
            set { _HasChange = value; }
        }


        // ウィンドウが閉じたかどうか
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        public Settings()
        {
            InitializeComponent();
            this.Closed += new EventHandler(Settings_Closed);
            this.Loaded += new RoutedEventHandler(Settings_Loaded);
        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbOwnerStoreName.Text = OASystem.Model.DI.自店舗名;

            var ip = PrinterSettings.InstalledPrinters;
            this.cmbPrinterSettings.ItemsSource = ip;
            int counter = 0;
            foreach (var s in ip)
            {
                if (s.ToString() == OASystem.Model.DI.使用するプリンタ名)
                {
                    this.cmbPrinterSettings.SelectedIndex = counter;
                    break;
                }
                counter++;
            }

            using (PrintDocument pd = new PrintDocument())
            {
                foreach (PaperSource ps in pd.PrinterSettings.PaperSources)
                {
                    //ComboBoxItem cmi = new ComboBoxItem();
                    //cmi.Content = ps.SourceName;
                    cmbPrinterトレイ選択.Items.Add(ps.SourceName);
                }
            }

            cmbPrinterトレイ選択.SelectedValue = OASystem.Model.DI.選択トレイ;


            this.tbMEDICODEWebSRPGPath.Text = OASystem.Model.DI.MEDICODEWebSRFIlePath;
        }

        void Settings_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DoSave())
            {
                this.HasChange = false;
            }

        }

        /// <summary>
        /// 保存処理
        /// </summary>
        /// <returns>保存が成功した場合</returns>
        private bool DoSave()
        {
            if (this.tbOwnerStoreName.Text == "")
            {
                MessageBox.Show("自店舗名が未入力です。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            // プリンタは０台の場合や、そのときは設定したくない場合もあるのでチェックしない。
            //if (this.cmbPrinterSettings.SelectedIndex == -1)
            //{
            //    MessageBox.Show("プリンター名が未入力です。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

            string printerName = "";
            if (this.cmbPrinterSettings.SelectedIndex == -1)
            {
                printerName = "";
            }
            else
            {
                printerName = this.cmbPrinterSettings.SelectedValue.ToString();
            }

            string printerトレイ = "";
            if (this.cmbPrinterトレイ選択.SelectedIndex == -1)
            {
                printerトレイ = "";
            }
            else
            {
                printerトレイ = this.cmbPrinterトレイ選択.SelectedValue.ToString();
            }



            if (this.tbMEDICODEWebSRPGPath.Text == "")
            {
                MessageBox.Show("MEDICODE-WebSR ファイルパスが未入力です。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            SettingsIniController.DoWrite(this.tbOwnerStoreName.Text, printerName,printerトレイ, this.tbMEDICODEWebSRPGPath.Text);

            SettingsIniController.DoLoad(); // DIへもセット

            // DIには反映しているが、念のため再起動を促す。
            MessageBox.Show("保存が完了しました。\r\n\r\n設定を反映するにはプログラムを再起動してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

            return true;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (this.HasChange)
            {
                if (MessageBox.Show("保存しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (!DoSave())
                    {
                        return;
                    }
                    else
                    {
                        this.HasChange = false;
                    }
                }
            }

            this.Close();
        }

        private void tbOwnerStoreName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.HasChange = true;
        }

        private void cmbPrinterSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            this.HasChange = true;
        }

        private void tbMEDICODEWebSRPGPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.HasChange = true;
        }

        private void cmbPrinterトレイ選択_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
