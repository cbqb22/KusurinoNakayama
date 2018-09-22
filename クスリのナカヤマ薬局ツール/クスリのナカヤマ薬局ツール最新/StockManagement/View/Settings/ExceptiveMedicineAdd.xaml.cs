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
using StockManagement.ViewModel;
using StockManagement.ViewModel.Common.MessageBox;

namespace StockManagement.View.Settings
{
    /// <summary>
    /// BalancingAccountsAdd.xaml の相互作用ロジック
    /// </summary>
    public partial class ExceptiveMedicineAdd : Window
    {
        private bool _AddFlag;
        public bool AddFlag
        {
            get { return _AddFlag; }
            set { _AddFlag = value; }
        }


        private List<ExceptionMedicineEntity> _lvItemssourceForDuplicationCheck = new List<ExceptionMedicineEntity>();

        public List<ExceptionMedicineEntity> LvItemssourceForDuplicationCheck
        {
            get { return _lvItemssourceForDuplicationCheck; }
            set { _lvItemssourceForDuplicationCheck = value; }
        }


        public ExceptiveMedicineAdd()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ExceptiveMedicineAdd_Loaded);
        }

        void ExceptiveMedicineAdd_Loaded(object sender, RoutedEventArgs e)
        {
            tb医薬品名称.Focus();
        }


        private void btn追加_Click(object sender, RoutedEventArgs e)
        {
            if (tb医薬品名称.Text == "")
            {
                MessageBoxTop.Show("医薬品名称が入力されておりません。", "確認", MessageBoxButton.OK);
                return;
            }

            var Spattern = @"\d{9}"; // S群
            var Smatches = Regex.Matches(tbレセプト電算コード.Text, Spattern);

            if (tbレセプト電算コード.Text.Length != 9 || Smatches.Count == 0)
            {
                MessageBoxTop.Show("レセプト電算コードは半角９桁で入力してください。", "確認", MessageBoxButton.OK);
                return;
            }

            tbレセプト電算コード.Text = StockManagement.ViewModel.Common.DataConvert.DataConvert.全角To半角変換(tbレセプト電算コード.Text);

            var checkresult = from x in LvItemssourceForDuplicationCheck
                              where
                                x.レセプト電算コード == tbレセプト電算コード.Text
                              select x;

            if (checkresult.Count() != 0)
            {
                MessageBoxTop.Show("このレセプト電算コードは既に追加されています。", "確認", MessageBoxButton.OK);
                return;
            }


            this.AddFlag = true;
            this.Close();
        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
