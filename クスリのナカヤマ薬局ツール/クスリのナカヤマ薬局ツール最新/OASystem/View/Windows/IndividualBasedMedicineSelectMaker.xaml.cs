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
    /// IndividualBasedMedicineSelectMaker.xaml の相互作用ロジック
    /// </summary>
    public partial class IndividualBasedMedicineSelectMaker : Window
    {
        private bool _選択Flag;

        public bool 選択Flag
        {
            get { return _選択Flag; }
            set { _選択Flag = value; }
        }

        public IndividualBasedMedicineSelectMaker()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(IndividualBasedMedicineSelectMaker_Loaded);
        }

        void IndividualBasedMedicineSelectMaker_Loaded(object sender, RoutedEventArgs e)
        {
            tbキーワード.Focus();
        }

        private void btnキーワード検索_Click(object sender, RoutedEventArgs e)
        {
            if (this.tbキーワード.Text == "")
            {
                MessageBox.Show("キーワードが入力されておりません。","確認",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                return;
            }

            var searchResult = 
                               (
                               from x in OASystem.Model.DI.メーカー名リスト
                               where
                                    x.Contains(tbキーワード.Text)
                               select x
                               ).ToList();

            if (searchResult.Count == 0)
            {
                MessageBox.Show("キーワードに該当する会社名が見つかりませんでした。\r\n細かいキーワードで検索してください。\r\n漢字も使用可能です。カナは全角で入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            this.lvCompanyName.ItemsSource = searchResult;


                                    
        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn選択_Click(object sender, RoutedEventArgs e)
        {
            if (lvCompanyName.SelectedIndex == -1)
            {
                MessageBox.Show("メーカー名が選択されてません。\r\nキーワード検索後、メーカー名を表示されたリストから選択してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            選択Flag = true;
            this.Close();
        }
    }
}
