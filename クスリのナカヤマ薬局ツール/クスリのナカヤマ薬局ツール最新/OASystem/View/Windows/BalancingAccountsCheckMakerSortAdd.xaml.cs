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

namespace OASystem.View.Windows
{
    /// <summary>
    /// BalancingAccountsAdd.xaml の相互作用ロジック
    /// </summary>
    public partial class BalancingAccountsCheckMakerSortAdd : Window
    {
        private bool _AddFlag;
        public bool AddFlag
        {
            get { return _AddFlag; }
            set { _AddFlag = value; }
        }

        /// <summary>
        /// 次のwindowへ受け渡し用のプロパティ
        /// </summary>
        private List<string> _すでに追加されているメーカー名;
        public List<string> すでに追加されているメーカー名
        {
            get { return _すでに追加されているメーカー名; }
            set { _すでに追加されているメーカー名 = value; }
        }

        public BalancingAccountsCheckMakerSortAdd()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BalancingAccountsCheckMakerSortAdd_Loaded);
        }

        void BalancingAccountsCheckMakerSortAdd_Loaded(object sender, RoutedEventArgs e)
        {
            btn検索メーカー名.Focus();
        }

        private void btn追加_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbxメーカー名.Text) || tbxメーカー名.Text == "キーワード検索してください。")
            {
                MessageBox.Show("メーカー名をキーワード検索ボタンより選択してください。", "確認", MessageBoxButton.OK);
                return;
            }
            if (cmb帳合先.SelectedIndex == -1)
            {
                MessageBox.Show("帳合先を選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }


            this.AddFlag = true;
            this.Close();
        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn検索メーカー名_Click(object sender, RoutedEventArgs e)
        {
            BalancingAccountsCheckMakerSortSelectMaker bacmssm = new BalancingAccountsCheckMakerSortSelectMaker();
            bacmssm.すでに追加されているメーカー名 = _すでに追加されているメーカー名;
            bacmssm.ShowDialog();

            if (bacmssm.選択Flag)
            {
                this.tbxメーカー名.Text = bacmssm.lvCompanyName.SelectedValue.ToString();
                this.tbxメーカー名.Foreground = new SolidColorBrush(Colors.Black);
            }


        }
    }
}
