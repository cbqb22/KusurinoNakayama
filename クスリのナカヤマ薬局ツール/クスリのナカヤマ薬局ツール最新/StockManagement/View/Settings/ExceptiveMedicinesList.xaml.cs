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
using StockManagement.ViewModel;
using StockManagement.ViewModel.Common.MessageBox;


namespace StockManagement.View.Settings
{
    /// <summary>
    /// OriginalMacroMaker.xaml の相互作用ロジック
    /// </summary>
    public partial class ExceptiveMedicinesList : Window
    {

        //private bool _IsSaved;

        //public bool IsSaved
        //{
        //    get { return _IsSaved; }
        //    set { _IsSaved = value; }
        //}

        private List<ExceptionMedicineEntity> _lvItemssource = new List<ExceptionMedicineEntity>();

        public List<ExceptionMedicineEntity> LvItemssource
        {
            get { return _lvItemssource; }
            set { _lvItemssource = value; }
        }

        public ExceptiveMedicinesList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ExceptiveMedicinesList_Loaded);
            this.Closed += new EventHandler(ExceptiveMedicinesList_Closed);
        }

        void ExceptiveMedicinesList_Loaded(object sender, RoutedEventArgs e)
        {
            this.lvExceptiveMedicinesList.ItemsSource = LvItemssource;
        }

        void ExceptiveMedicinesList_Closed(object sender, EventArgs e)
        {
        }



        //private void btn保存_Click(object sender, RoutedEventArgs e)
        //{
        //    Save();
        //}

        //private void Save()
        //{
        //}

        private void btn閉じる_Click(object sender, RoutedEventArgs e)
        {
            //if(!IsSaved && MessageBoxTop.Show("保存しますか？", "確認", MessageBoxTopButton.YesNo, MessageBoxTopImage.Question) == MessageBoxTopResult.Yes)
            //{
            //    Save();
            //}

            this.Close();
        }

        private void btn除外追加_Click(object sender, RoutedEventArgs e)
        {
            ExceptiveMedicineAdd ema = new ExceptiveMedicineAdd();
            ema.LvItemssourceForDuplicationCheck = LvItemssource;
            ema.ShowDialog();

            if (ema.AddFlag)
            {
                ExceptionMedicineEntity ent = new ExceptionMedicineEntity();
                ent.医薬品名称 = ema.tb医薬品名称.Text;
                ent.レセプト電算コード = ema.tbレセプト電算コード.Text;

                this.lvExceptiveMedicinesList.ItemsSource = null;
                LvItemssource.Add(ent);
                this.lvExceptiveMedicinesList.ItemsSource = LvItemssource;
                this.lvExceptiveMedicinesList.ScrollIntoView(LvItemssource.Last());
            }

        }

        private void lvExceptiveMedicinesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn選択削除_Click(object sender, RoutedEventArgs e)
        {
            if (lvExceptiveMedicinesList.SelectedIndex == -1)
            {
                MessageBoxTop.Show("削除する項目が選択されていません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var val = lvExceptiveMedicinesList.SelectedValue as ExceptionMedicineEntity;

            LvItemssource.Remove(val);
            lvExceptiveMedicinesList.ItemsSource = null;
            lvExceptiveMedicinesList.ItemsSource = LvItemssource;


        }


    }
}
