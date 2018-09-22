using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using View.Core.TopPage.Tab.掲示板.メイン;
namespace View.Core.共通.UserControls
{
    public partial class PageSelector : UserControl
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="PagesStrings">ページになる番号</param>
        public PageSelector(List<string> PagesStrings,bool 前へ,bool 次へ,int 現ページ番号,int メイン記事総数)
        {
            InitializeComponent();

            this.現在のページ番号 = 現ページ番号;
            this.メイン記事数 = メイン記事総数;
            SetPages(PagesStrings,前へ,次へ);


        }

        private int 現在のページ番号;
        private int メイン記事数;


        private readonly string Previous = "前";
        private readonly string Next = "次";
        private readonly string First = "＜＜";
        private readonly string End = "＞＞";

        /// <summary>
        /// spPagesにページをセットする。
        /// </summary>
        private void SetPages(List<string> PagesStrings,bool 前へ,bool 次へ)
        {
            if (PagesStrings == null || PagesStrings.Count == 0)
            {
                throw new Exception("PagesStringsが設定されてません。");
            }

            if (前へ)
            {
                PagesStrings.Insert(0, this.First);
                PagesStrings.Insert(1, this.Previous);
            }

            if (次へ)
            {
                PagesStrings.Insert(PagesStrings.Count, this.Next);
                PagesStrings.Insert(PagesStrings.Count, this.End);
            }


            foreach (var str in PagesStrings)
            {

                Button bt = new Button();
                bt.Click += new RoutedEventHandler(bt_Click);
                bt.Background = new SolidColorBrush(Colors.White);
                bt.BorderBrush = new SolidColorBrush(Colors.White);

                if (str.Equals(現在のページ番号.ToString()))
                {
                    bt.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    bt.Foreground = new SolidColorBrush(Colors.Blue);
                }
                bt.FontSize = 20d;
                bt.Content = str;

                this.spPages.Children.Add(bt);
            }
        }

        void bt_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            TreeViewItemData tvid = SingletonInstances.掲示板FrameInstance.ThreadSelectorName.資料tv.SelectedItem as TreeViewItemData;

            if (tvid == null || string.IsNullOrEmpty(tvid.Name))
            {
                MessageBox.Show("スレッドを選択して下さい。", "エラー", MessageBoxButton.OK);
                return;
            }


            string 押された番号 = bt.Content.ToString();
            int グループNo;
            if (押された番号.Equals("次"))
            {
                グループNo = this.現在のページ番号 + 1;
            }
            else if (押された番号.Equals("前"))
            {
                グループNo = this.現在のページ番号 - 1;
            }
            else if (押された番号.Equals("＜＜"))
            {
                グループNo = 1;
            }
            else if (押された番号.Equals("＞＞"))
            {
                double d = (double)メイン記事数 / 10;

                グループNo = ((int)Math.Ceiling(d));
            }
            else
            {
                if (int.TryParse(bt.Content.ToString(), out グループNo) == false)
                {
                    return;
                }
            }


            SingletonInstances.掲示板FrameInstance.Get掲示板データ(tvid.Name, グループNo);



                 
        }
    }
}
