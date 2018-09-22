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
using System.Collections.ObjectModel;
using View.Service.File.Reader;

namespace View.Core.TopPage.Tab.掲示板.メイン
{
    public partial class ThreadSelector : UserControl
    {
        public ThreadSelector()
        {
            InitializeComponent();
            GetThreadTitles();

            this.資料tv.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(資料tv_SelectedItemChanged);
            this.資料tv.MouseLeftButtonUp += new MouseButtonEventHandler(資料tv_MouseLeftButtonUp);

        }

        private bool _掲示板タブ再更新中か;

        public bool 掲示板タブ再更新中か
        {
            get { return _掲示板タブ再更新中か; }
            set { _掲示板タブ再更新中か = value; }
        }

        void 資料tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tv = sender as TreeView;
            if (tv == null)
            {
                return;
            }

            TreeViewItemData tvid = tv.SelectedItem as TreeViewItemData;

            if (tvid == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(tvid.Name))
            {
                return;
            }

            if (掲示板タブ再更新中か)
            {
                掲示板タブ再更新中か = false;
                return;
            }

            View.Core.共通.SingletonInstances.掲示板FrameInstance.Get掲示板データ(tvid.Name, 1);

        }

        /// <summary>
        /// TreeViewが最初に読み込まれた際に一番上のアイテムを選択する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void 資料tv_Loaded(object sender, RoutedEventArgs e)
        //{

        //    //if (this.資料tv != null && 1 <= this.資料tv.Items.Count)
        //    //{
        //    //    TreeViewItem firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
        //    //    if (firstTreeviewitem == null)
        //    //    {
        //    //        //this.Dispatcher.BeginInvoke(delegate()
        //    //        //{
        //    //        //    while (firstTreeviewitem == null)
        //    //        //    {
        //    //        //        firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
        //    //        //    }

        //    //        //});
        //    //        return;
        //    //    }
        //    //    firstTreeviewitem.IsSelected = true;
        //    //    return;
        //    //}
        //    //else
        //    //{
        //    //    this.Dispatcher.BeginInvoke(delegate()
        //    //    {
        //    //        while (this.資料tv == null || (this.資料tv != null && this.資料tv.Items.Count <= 0))
        //    //        {
        //    //        }

        //    //        TreeViewItem firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
        //    //        if (firstTreeviewitem == null)
        //    //        {
        //    //            //this.Dispatcher.BeginInvoke(delegate()
        //    //            //{
        //    //            //    while (firstTreeviewitem == null)
        //    //            //    {
        //    //            //        firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
        //    //            //    }

        //    //            //});
        //    //            return;
        //    //        }
        //    //        firstTreeviewitem.IsSelected = true;
        //    //        return;


        //    //    });

        //    //}
        //}


        public void GetThreadTitles()
        {

            FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.GetThreadTitlesCompleted += new EventHandler<GetThreadTitlesCompletedEventArgs>(client_GetThreadTitlesCompleted);
            client.GetThreadTitlesAsync();
        }

        private bool _スレッド操作メッセージありか;

        public bool スレッド操作メッセージありか
        {
            get { return _スレッド操作メッセージありか; }
            set { _スレッド操作メッセージありか = value; }
        }

        private string _スレッド操作メッセージ;

        public string スレッド操作メッセージ
        {
            get { return _スレッド操作メッセージ; }
            set { _スレッド操作メッセージ = value; }
        }


        private bool _初回ロード済か;

        public bool 初回ロード済か
        {
            get { return _初回ロード済か; }
            set { _初回ロード済か = value; }
        }




        void client_GetThreadTitlesCompleted(object sender, GetThreadTitlesCompletedEventArgs e)
        {
            try
            {

                if (スレッド操作メッセージありか)
                {
                    MessageBox.Show(スレッド操作メッセージ, "確認", MessageBoxButton.OK);
                    this.スレッド操作メッセージ = "";
                    this.スレッド操作メッセージありか = false;
                }

                if (e == null)
                {
                    return;
                }

                if (1 <= e.Result.Count && !string.IsNullOrEmpty(e.Result[0].スレッド名))
                {
                    SetTreeViewerItemSource(e.Result);
                    // ここだとデッドロックする場合があるので、Get掲示板データ内に移動した。
                    //SetTreeViewItemSelect();
                    View.Core.共通.SingletonInstances.掲示板FrameInstance.Get掲示板データ(e.Result[0].スレッド名, 1);
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

        public void SetTreeViewItemSelect()
        {
            if (this.資料tv != null && 1 <= this.資料tv.Items.Count)
            {
                TreeViewItem firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
                if (firstTreeviewitem == null)
                {
                    // 非同期で、TreeViewItemがnullでなくなるまで行い、その後、一番最初を選択させる。
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        while (firstTreeviewitem == null)
                        {
                            firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
                        }
                        firstTreeviewitem.IsSelected = true;

                    });
                }
                else
                {
                    firstTreeviewitem.IsSelected = true;
                    return;
                }
            }
        }

        private void SetTreeViewerItemSource(ObservableCollection<ThreadTitlesEntity> tte)
        {

            List<TreeViewItemData> listtvi = new List<TreeViewItemData>();


            foreach (var ent in tte)
            {
                TreeViewItemData tvi = new TreeViewItemData();
                tvi.Name = ent.スレッド名;
                tvi.Image = ent.画像パス;
                listtvi.Add(tvi);
            }

            this.資料tv.ItemsSource = listtvi;
        }

        private void 資料tv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TreeView tv = sender as TreeView;
            if (tv == null)
            {
                return;
            }

            TreeViewItemData tvid = tv.SelectedItem as TreeViewItemData;

            if (tvid == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(tvid.Name))
            {
                return;
            }

            View.Core.共通.SingletonInstances.掲示板FrameInstance.Get掲示板データ(tvid.Name, 1);


        }
    }

    public class TreeViewData
    {
        public List<TreeViewItemData> TreeItemList { get; set; }

    }


    public class TreeViewItemData
    {
        public bool IsDirectory { get; set; }
        public string PathFromRoot { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public List<TreeViewItemData> Children { get; set; }
    }

}
