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
using WF= System.Windows.Forms;
using MCSystem.Model;
using MCSystem.ViewModel;

namespace MCSystem.View.Windows
{
    /// <summary>
    /// OriginalMacroMaker.xaml の相互作用ロジック
    /// </summary>
    public partial class OriginalMacroMaker : Window
    {

        private bool _IsClosed;

        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }


        private bool _IsSaved;

        public bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        private List<OriginalMacroDetailEntity> _lvItemssource = new List<OriginalMacroDetailEntity>();

        public List<OriginalMacroDetailEntity> LvItemssource
        {
            get { return _lvItemssource; }
            set { _lvItemssource = value; }
        }

        public OriginalMacroMaker()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OriginalMacroMaker_Loaded);
            this.Closed += new EventHandler(OriginalMacroMaker_Closed);
        }

        void OriginalMacroMaker_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;

            SingletonWindows.XYLocationWindow.Close();

            // メニューを前面にする
            SingletonWindows.BalanceChangeMenuWindow.WindowState = System.Windows.WindowState.Normal;
            SingletonWindows.BalanceChangeMenuWindow.Show();
            SingletonWindows.BalanceChangeMenuWindow.Activate();
        }

        void OriginalMacroMaker_Loaded(object sender, RoutedEventArgs e)
        {
            lvMacroContents.ItemsSource = _lvItemssource;
        }

        private void btnマクロ内容追加_Click(object sender, RoutedEventArgs e)
        {
            OperationAdd oa = new OperationAdd();
            oa.HostWindow = this;
            SingletonWindows.XYLocationWindow.Show();
            SingletonWindows.XYLocationWindow.Activate();
            SingletonWindows.XYLocationWindow.ControlWindow = oa;
            oa.Show();

            SingletonWindows.BalanceChangeMenuWindow.Hide();
            this.Hide();



        }

        private void btn保存_Click(object sender, RoutedEventArgs e)
        {
            Save();
            MessageBox.Show("保存しました。","保存完了",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Save()
        {
            var details = lvMacroContents.ItemsSource as List<OriginalMacroDetailEntity>;
            OriginalMacroEntity omEnt = new OriginalMacroEntity();
            omEnt.ListDetail = details;
            omEnt.データファイルパス = tbデータファイルPath.Text;
            ControlOriginalMacro.WriteSettings(omEnt,tb保存先フォルダPath.Text);
            IsSaved = true;
        }

        private void btn閉じる_Click(object sender, RoutedEventArgs e)
        {
            if(!IsSaved && MessageBox.Show("保存しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Save();
                MessageBox.Show("保存しました。", "保存完了", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.Close();
        }

        private void lvMacroContents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnデータファイル参照_Click(object sender, RoutedEventArgs e)
        {
            WF.OpenFileDialog ofd = new WF.OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if (ofd.ShowDialog() == WF.DialogResult.OK)
            {
                tbデータファイルPath.Text = ofd.FileName;
            }

        }

        private void btn保存先フォルダ参照_Click(object sender, RoutedEventArgs e)
        {
            WF.FolderBrowserDialog fbd = new WF.FolderBrowserDialog();
            WF.DialogResult dr = fbd.ShowDialog();

            if (dr == WF.DialogResult.OK)
            {
                tb保存先フォルダPath.Text = fbd.SelectedPath;
            }
        }

    }
}
