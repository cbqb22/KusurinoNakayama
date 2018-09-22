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
using System.Text.RegularExpressions;

namespace OASystem.View.Windows
{
    /// <summary>
    /// BalancingAccountsAdd.xaml の相互作用ロジック
    /// </summary>
    public partial class BalancingAccountsAdd : Window
    {
        private bool _AddFlag;
        public bool AddFlag
        {
            get { return _AddFlag; }
            set { _AddFlag = value; }
        }

        public BalancingAccountsAdd()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BalancingAccountsAdd_Loaded);
        }

        void BalancingAccountsAdd_Loaded(object sender, RoutedEventArgs e)
        {
            tbＶＡＮコード.Focus();
        }

        private void btn追加_Click(object sender, RoutedEventArgs e)
        {
            if (tbＶＡＮコード.Text == "")
            {
                MessageBox.Show("卸ＶＡＮコードが入力されておりません。","確認",MessageBoxButton.OK);
                return;
            }

            var Spattern = @"\d{9}"; // S群
            var Smatches = Regex.Matches(tbＶＡＮコード.Text, Spattern);

            if (tbＶＡＮコード.Text.Length != 9 || Smatches.Count == 0)
            {
                MessageBox.Show("卸ＶＡＮコードは半角９桁で入力してください。", "確認", MessageBoxButton.OK);
                return;
            }


            if (tb帳合先名称.Text == "")
            {
                MessageBox.Show("帳合先名称が入力されておりません。", "確認", MessageBoxButton.OK);
                return;
            }

            tbＶＡＮコード.Text = OASystem.ViewModel.Common.DataConvert.DataConvert.全角To半角変換(tbＶＡＮコード.Text);


            this.AddFlag = true;
            this.Close();
        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
