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
using OASystem.Model.Enum;

namespace OASystem.View.Windows
{
    /// <summary>
    /// IndividualBasedMedicineAdd.xaml の相互作用ロジック
    /// </summary>
    public partial class IndividualBasedMedicineAdd : Window
    {



        private bool _AddFlag;
        public bool AddFlag
        {
            get { return _AddFlag; }
            set { _AddFlag = value; }
        }

        public IndividualBasedMedicineAdd()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(IndividualBasedMedicineAdd_Loaded);
        }

        void IndividualBasedMedicineAdd_Loaded(object sender, RoutedEventArgs e)
        {
            var ent = new OASystem.Model.Entity.IndividualBasedManagementMedicineEntity();
            //フリーワードで入れられるようにする。日本ベクトン・ディッキンソンみたいに医療材料のみ販売していてMEDISにのっていない会社もある為
            //ent.製薬会社 = ent.販売会社 ="キーワード検索してください。";
            this.DataContext = ent;

            foreach (var enu in Enum.GetValues(typeof(剤形区分Enum)))
            {
                cmb剤形区分.Items.Add(enu.ToString());
            }

            tbＪＡＮコード.Focus();
        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn追加_Click(object sender, RoutedEventArgs e)
        {

            //OASystem.ViewModel.Common.Validator.PreValidation.AllTextBoxFocusAndLostFocus(this, btn追加);
            

            if (string.IsNullOrEmpty(tbＪＡＮコード.Text))
            {
                MessageBox.Show("ＪＡＮコードが入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var Spattern = @"\d{13}"; // S群
            var Smatches = Regex.Matches(tbＪＡＮコード.Text, Spattern);

            if (tbＪＡＮコード.Text.Length != 13 || Smatches.Count == 0)
            {
                MessageBox.Show("ＪＡＮコードは半角数字を13文字で入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            tbＪＡＮコード.Text = OASystem.ViewModel.Common.DataConvert.DataConvert.全角To半角変換(tbＪＡＮコード.Text);


            if (string.IsNullOrEmpty(tb医薬品名.Text))
            {
                MessageBox.Show("医薬品名が入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(tb包装形態.Text))
            {
                MessageBox.Show("包装形態が入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(tb包装単位.Text))
            {
                MessageBox.Show("tb包装単位が入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(tb包装単位数.Text))
            {
                MessageBox.Show("tb包装単位数が入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }



            if (string.IsNullOrEmpty(tb包装総量.Text))
            {
                MessageBox.Show("tb包装総量が入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            double result2;
            if (double.TryParse(tb包装総量.Text, out result2) == false)
            {
                MessageBox.Show("包装総量は半角数字を入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            if (cmb剤形区分.SelectedIndex == -1)
            {
                MessageBox.Show("剤形区分がドロップダウンリストより選択されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(tb製薬会社.Text))
            {
                MessageBox.Show("tb製薬会社が入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(tb販売会社.Text))
            {
                MessageBox.Show("tb販売会社が入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            this.AddFlag = true;
            this.Close();
        }

        private void btn販売会社キーワード検索_Click(object sender, RoutedEventArgs e)
        {
            IndividualBasedMedicineSelectMaker ibmsm = new IndividualBasedMedicineSelectMaker();
            ibmsm.Title = " 販売会社名 検索";
            ibmsm.ShowDialog();

            if (ibmsm.選択Flag)
            {
                this.tb販売会社.Text = ibmsm.lvCompanyName.SelectedValue.ToString();
                //this.tb販売会社.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void btn製薬会社キーワード検索_Click(object sender, RoutedEventArgs e)
        {
            IndividualBasedMedicineSelectMaker ibmsm = new IndividualBasedMedicineSelectMaker();
            ibmsm.Title = "製薬会社名 検索";
            ibmsm.ShowDialog();

            if (ibmsm.選択Flag)
            {
                this.tb製薬会社.Text = ibmsm.lvCompanyName.SelectedValue.ToString();
                //this.tb製薬会社.Foreground = new SolidColorBrush(Colors.Black);
            }

        }
    }
}
