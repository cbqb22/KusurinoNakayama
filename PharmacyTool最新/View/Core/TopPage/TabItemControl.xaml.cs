using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using View.Service.DAO.PharmacyTool.アクセス数管理;

namespace View.Core.TopPage
{
    public partial class TabItemControl : UserControl
    {
        public TabItemControl()
        {
            InitializeComponent();

            View.Core.共通.SingletonInstances.TabItemControlInstance = this;
            this.SetImageLogo();
            this.StoryboardLogo.Completed += new EventHandler(StoryboardLogo_Completed);



#if DEBUG
            //this.資料TabItem.MouseLeftButtonUp += new MouseButtonEventHandler(資料TabItem_MouseLeftButtonUp);

#elif NAKAYAMA

            this.掲示板TabItem.MouseLeftButtonUp += new MouseButtonEventHandler(掲示板TabItem_MouseLeftButtonUp);
            this.資料TabItem.MouseLeftButtonUp += new MouseButtonEventHandler(資料TabItem_MouseLeftButtonUp);
#else
            // RELEASEモードの時は掲示板クリック時はリフレッシュしない
#endif

            SetFirstTimeSettingStoryboardLogo();
            SetAccessCounter();
        }


        void SetImageLogo()
        {

#if DEBUG

            Uri imgUri = new Uri("/etc/Icon/nakayamalogo.png", UriKind.Relative);
            this.imgLogo.Source = new BitmapImage(imgUri);
#elif NAKAYAMA

            Uri imgUri = new Uri("/etc/Icon/nakayamalogo.png", UriKind.Relative);
            this.imgLogo.Source = new BitmapImage(imgUri);
#else

            Uri imgUri = new Uri("", UriKind.Relative);
            //this.imgLogo.Source = new BitmapImage(imgUri);
#endif


            
        }

        void 掲示板TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TabItem ti = sender as TabItem;
            if (ti == null)
            {
                return;
            }

            View.Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.初回ロード済か = false;
            View.Core.共通.SingletonInstances.掲示板FrameInstance.ThreadSelectorName.GetThreadTitles();
        }

        void 資料TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TabItem ti = sender as TabItem;
            if (ti == null)
            {
                return;
            }

            View.Core.共通.SingletonInstances.資料Instance.DoCreate資料TreeViewItemSource();

        }


        /// <summary>
        /// 再生するテロップ記事のリスト
        /// </summary>
        private List<string> _TeropList;

        public List<string> TeropList
        {
            get { return _TeropList; }
            set { _TeropList = value; }
        }



        /// <summary>
        /// 再生しているテロップ記事
        /// </summary>
        private int _Teropcounter;

        public int Teropcounter
        {
            get { return _Teropcounter; }
            set { _Teropcounter = value; }
        }


        private void SetFirstTimeSettingStoryboardLogo()
        {
            AccessManagementClient client = Util.ServiceUtil.ReferenceCreater.GetAccessManagementClient();
            client.Doアクセス数カウントアップ取得Completed += new EventHandler<Doアクセス数カウントアップ取得CompletedEventArgs>(client_Doアクセス数カウントアップ取得Completed);
            client.Doアクセス数カウントアップ取得Async();
        }

        void client_Doアクセス数カウントアップ取得Completed(object sender, Doアクセス数カウントアップ取得CompletedEventArgs e)
        {

            try
            {



                if (e.Error == null)
                {
                    アクセス数取得結果 結果 = e.Result as アクセス数取得結果;
                    if (結果.取得成功か)
                    {
                        this.nameAccessCounter.lbAccessCounter.Content = 結果.アクセス数.ToString().PadLeft(8, '0');
                    }
                    else
                    {
                        this.nameAccessCounter.lbAccessCounter.Content = "00000000";
                    }
                }
                else
                {
                    this.nameAccessCounter.lbAccessCounter.Content = "00000000";
                }


            }
            catch (Exception exp)
            {
                MessageBox.Show("エラーが発生しました。" + exp.Message + exp.StackTrace);
            }
            finally
            {
                // Clientを閉じる
                Type type = sender.GetType();
                System.Reflection.MethodInfo mi = type.GetMethod("CloseAsync", Type.EmptyTypes);
                Object[] paramlist = null; // メソッドの引数の配列
                mi.Invoke(sender, paramlist);

            }

        }

        private void SetAccessCounter()
        {

        }

        void StoryboardLogo_Completed(object sender, EventArgs e)
        {
            //((DoubleAnimation)this.StoryboardLogo.Children[0]).From
            //((DoubleAnimation)this.StoryboardLogo.Children[0]).To
            //((DoubleAnimation)this.StoryboardLogo.Children[1]).From
            //((DoubleAnimation)this.StoryboardLogo.Children[1]).To
            //this.tbtelop.Text = "２回目";

            if (TeropList.Count - 1 == Teropcounter)
            {
                this.Teropcounter = 0;
            }
            else
            {
                this.Teropcounter += 1;
            }

            this.tbtelop.Text = TeropList[this.Teropcounter];
            this.StoryboardLogo.Begin();
        }


        //void TabItemControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    this.StoryboardLogo.Begin();
        //}

        public void StoryboardLogoReStart(List<string> teroplist, int teropcounter)
        {
            this.TeropList = teroplist;
            this.Teropcounter = teropcounter;
            this.tbtelop.Text = teroplist[0];

            this.StoryboardLogo.Stop();
            this.StoryboardLogo.Begin();

        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //            if (e != null && 0 < e.RemovedItems.Count)
            //            {
            //                var ri = e.RemovedItems[0] as TabItem;
            //                ri.Foreground = new SolidColorBrush(Colors.Black);
            //                //ri.Background= new SolidColorBrush(Colors.Black);
            //            }
            //
            //            var tc = sender as TabControl;
            //            var ti = tc.SelectedItem as TabItem;
            //            ti.Foreground = new SolidColorBrush(Colors.Red);
            //            //ti.Background = new SolidColorBrush(Colors.Red);

        }

    }

}
