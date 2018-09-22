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
using Microsoft.Win32;
using クスリのナカヤマ薬局ツール.共通.File;

namespace クスリのナカヤマ薬局ツール.Windows
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

        private void SetInit()
        {
            tb現在庫FilePath.Text = クスリのナカヤマ薬局ツール.Model.DI.現在庫ファイルパス;
            tb使用量FilePath.Text = クスリのナカヤマ薬局ツール.Model.DI.使用量ファイルパス;
            tb不動品FilePath.Text = クスリのナカヤマ薬局ツール.Model.DI.不動品ファイルパス;
            tb出力先FolderName.Text = クスリのナカヤマ薬局ツール.Model.DI.出力先フォルダ名;
            tb出力店舗名称.Text = クスリのナカヤマ薬局ツール.Model.DI.出力店舗名称;
        }



        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            this.HasChange = true;
            SetInit();
        }

        void Settings_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
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
            if (this.tb出力店舗名称.Text == "")
            {
                MessageBox.Show("出力店舗名称が未入力です。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            SettingsIniController.DoWrite(tb現在庫FilePath.Text, tb使用量FilePath.Text, tb不動品FilePath.Text,tb出力先FolderName.Text,tb出力店舗名称.Text);

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

        private void btn現在庫参照_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if ((bool)ofd.ShowDialog() == false)
            {
                return;
            }

            tb現在庫FilePath.Text = ofd.FileName;


        }

        private void btn使用量参照_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if ((bool)ofd.ShowDialog() == false)
            {
                return;
            }

            tb使用量FilePath.Text = ofd.FileName;

        }

        private void btn不動品参照_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if ((bool)ofd.ShowDialog() == false)
            {
                return;
            }

            tb不動品FilePath.Text = ofd.FileName;


        }

        private void btn出力先フォルダ名参照_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb出力先FolderName.Text = fbd.SelectedPath;
            }


        }

    }
}
