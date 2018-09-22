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
using View.Service.DAO.PharmacyTool.ユーザー管理;
using View.Core.共通.Entity;
using View.Service.DAO.PharmacyTool.店舗;

namespace View.Core.TopPage.Tab.設定.SubTab.ユーザー管理
{
    public partial class 既存ユーザー更新 : UserControl
    {
        public 既存ユーザー更新()
        {
            InitializeComponent();

            SetDataGridItemSource();


        }




        ObservableCollection<AllUser情報取得結果> itemsource = new ObservableCollection<AllUser情報取得結果>();

        Dictionary<int, AllUser情報取得結果> SelectionChangeDatagridIndex = new Dictionary<int, AllUser情報取得結果>();

        public static ObservableCollection<string> comboboxitemsource = new ObservableCollection<string>();


        public void SetComboBoxItemsource2(ObservableCollection<PT店舗名リターンデータセット> dataset)
        {

            SetDataGridItemSource();
        }



        public void SetDataGridItemSource()
        {
            //Util.ServiceUtil.Call.UserManagement um = new View.Util.ServiceUtil.Call.UserManagement();
            //um.AllUser情報取得();
            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = Util.ServiceUtil.ReferenceCreater.GetUserManagementClient();
            client.AllUser情報取得実行Completed += new EventHandler<AllUser情報取得実行CompletedEventArgs>(client_AllUser情報取得実行Completed);
            client.AllUser情報取得実行Async();


        }

        void client_AllUser情報取得実行Completed(object sender, AllUser情報取得実行CompletedEventArgs e)
        {
            try
            {



                itemsource = e.Result;


                this.stpUserList.Children.Clear();

                StackPanel stp0 = new StackPanel();
                stp0.Orientation = Orientation.Horizontal;
                stp0.Margin = new Thickness(0d, 0d, 0d, 5d);

                TextBlock tb = new TextBlock();
                tb.Text = "UserID";
                tb.Width = 140d;
                tb.TextDecorations = TextDecorations.Underline;

                TextBlock tb2 = new TextBlock();
                tb2.Text = "New Password";
                tb2.Width = 140d;
                tb2.TextDecorations = TextDecorations.Underline;

                TextBlock tb3 = new TextBlock();
                tb3.Text = "アクセスレベル";
                tb3.Width = 140d;
                tb3.TextDecorations = TextDecorations.Underline;

                TextBlock tb4 = new TextBlock();
                tb4.Text = "ステータス";
                tb4.Width = 140d;
                tb4.TextDecorations = TextDecorations.Underline;


                stp0.Children.Add(tb);
                stp0.Children.Add(tb2);
                stp0.Children.Add(tb3);
                stp0.Children.Add(tb4);

                this.stpUserList.Children.Add(stp0);


                int stp_counter = 0;
                foreach (var i in itemsource)
                {
                    StackPanel stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;


                    // [0]
                    TextBox tbUserid = new TextBox();
                    tbUserid.Text = i.UserID;
                    tbUserid.Width = 140d;
                    tbUserid.TextChanged += new TextChangedEventHandler(tbUserid_TextChanged);

                    // [1]
                    // パスワードは復号化させない。
                    PasswordBox pb = new PasswordBox();
                    pb.Width = 140d;
                    pb.PasswordChanged += new RoutedEventHandler(pb_PasswordChanged);

                    // [2]
                    ComboBox cmbアクセスレベル = new ComboBox();
                    cmbアクセスレベル.Width = 140d;
                    var dset = Core.共通.SingletonInstances.在庫更新Instance.Dataset;
                    cmbアクセスレベル.Items.Add(string.Format("{0}:{1}", "0", "管理ユーザー"));
                    cmbアクセスレベル.Items.Add(string.Format("{0}:{1}", "1", "本部ユーザー"));
                    cmbアクセスレベル.SelectionChanged += new SelectionChangedEventHandler(UserList_ComboBoxChanged);

                    int counter = 0;
                    foreach (var set in dset)
                    {
                        cmbアクセスレベル.Items.Add(string.Format("{0}:{1}", set.ID, set.店舗名));

                        if (i.アクセス権限 == 0)
                        {
                            cmbアクセスレベル.SelectedIndex = 0;
                        }
                        else if (i.アクセス権限 == 1)
                        {
                            cmbアクセスレベル.SelectedIndex = 1;
                        }

                        if (set.ID == i.アクセス権限)
                        {
                            cmbアクセスレベル.SelectedIndex = counter + 2;
                        }
                        counter++;
                    }

                    // [3]
                    ComboBox cmbステータス = new ComboBox();
                    cmbステータス.Items.Add("1:使用");
                    cmbステータス.Items.Add("2:削除");
                    cmbステータス.Width = 140d;
                    cmbステータス.SelectionChanged += new SelectionChangedEventHandler(UserList_ComboBoxChanged);

                    if (!i.削除フラグ)
                    {
                        cmbステータス.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbステータス.SelectedIndex = 1;
                    }

                    // [4]
                    TextBlock hide_counter = new TextBlock();
                    hide_counter.Text = stp_counter.ToString();
                    hide_counter.Visibility = Visibility.Collapsed;

                    // [5]
                    TextBlock tbl変更前UserUD = new TextBlock();
                    tbl変更前UserUD.Text = i.UserID;
                    tbl変更前UserUD.Visibility = Visibility.Collapsed;



                    stp.Children.Add(tbUserid);
                    stp.Children.Add(pb);
                    stp.Children.Add(cmbアクセスレベル);
                    stp.Children.Add(cmbステータス);
                    stp.Children.Add(hide_counter);
                    stp.Children.Add(tbl変更前UserUD);

                    this.stpUserList.Children.Add(stp);

                    stp_counter++;

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


        void tbUserid_TextChanged(object sender, TextChangedEventArgs e)
        {
            AllUser情報取得結果 items = new AllUser情報取得結果();
            int stpChildNumber = -1;

            TextBox tbl = sender as TextBox;
            if (tbl == null)
            {
                return;
            }

            FrameworkElement fe = VisualTreeHelper.GetParent(tbl) as FrameworkElement;
            if (fe == null)
            {
                return;
            }
            if (fe is StackPanel)
            {
                var stpParent = ((StackPanel)fe) as StackPanel;


                // ０番目が入力UserID
                // １番目がPassword
                // ２番目がアクセスレベル
                // ３番目がステータス
                // ４番目がhide_counter でスタックパネルの番号
                // ５番目が変更前UserID
                if (VisualTreeHelper.GetChildrenCount(stpParent) != 6)
                {
                    return;
                }

                FrameworkElement e0 = VisualTreeHelper.GetChild(stpParent, 0) as FrameworkElement;
                FrameworkElement e1 = VisualTreeHelper.GetChild(stpParent, 1) as FrameworkElement;
                FrameworkElement e2 = VisualTreeHelper.GetChild(stpParent, 2) as FrameworkElement;
                FrameworkElement e3 = VisualTreeHelper.GetChild(stpParent, 3) as FrameworkElement;
                FrameworkElement e4 = VisualTreeHelper.GetChild(stpParent, 4) as FrameworkElement;
                FrameworkElement e5 = VisualTreeHelper.GetChild(stpParent, 5) as FrameworkElement;

                if (e0 == null || e1 == null || e2 == null || e3 == null || e4 == null || e5 == null)
                {
                    return;
                }

                if (e0 is TextBox)
                {
                    items.UserID = ((TextBox)e0).Text;
                }
                else
                {
                    return;
                }

                if (e1 is PasswordBox)
                {
                    items.Password = ((PasswordBox)e1).Password;
                }
                else
                {
                    return;
                }

                if (e2 is ComboBox)
                {
                    ComboBox tempcmb = (ComboBox)e2;
                    int startindex = tempcmb.SelectedItem.ToString().IndexOf(':');
                    string str = tempcmb.SelectedItem.ToString().Substring(0, startindex);
                    int result;
                    if (int.TryParse(str, out result) == false)
                    {
                        return;
                    }

                    items.アクセス権限 = result;
                }
                else
                {
                    return;
                }

                if (e3 is ComboBox)
                {
                    ComboBox cmb2 = (ComboBox)e3;
                    if (cmb2.SelectedIndex == 0)
                    {
                        items.削除フラグ = false;
                    }
                    else if (cmb2.SelectedIndex == 1)
                    {
                        items.削除フラグ = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }


                if (e4 is TextBlock)
                {
                    stpChildNumber = int.Parse(((TextBlock)e4).Text);



                }
                else
                {
                    return;
                }

                if (e5 is TextBlock)
                {
                    items.変更前のUserID = ((TextBlock)e5).Text;
                }
                else
                {
                    return;
                }

            }

            if (stpChildNumber == -1)
            {
                return;
            }

            if (items.変更前のUserID.Equals(""))
            {
                return;
            }


            if (SelectionChangeDatagridIndex.ContainsKey(stpChildNumber))
            {
                SelectionChangeDatagridIndex.Remove(stpChildNumber);
            }

            SelectionChangeDatagridIndex.Add(stpChildNumber, items);


        }


        void pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // パスワードは入力があれば、それを使用。　入力がなければ、変更しない。

            AllUser情報取得結果 items = new AllUser情報取得結果();
            int stpChildNumber = -1;

            PasswordBox pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }

            FrameworkElement fe = VisualTreeHelper.GetParent(pb) as FrameworkElement;
            if (fe == null)
            {
                return;
            }
            if (fe is StackPanel)
            {
                var stpParent = ((StackPanel)fe) as StackPanel;


                // ０番目が入力UserID
                // １番目がPassword
                // ２番目がアクセスレベル
                // ３番目がステータス
                // ４番目がhide_counter でスタックパネルの番号
                // ５番目が変更前UserID
                if (VisualTreeHelper.GetChildrenCount(stpParent) != 6)
                {
                    return;
                }
                FrameworkElement e0 = VisualTreeHelper.GetChild(stpParent, 0) as FrameworkElement;
                FrameworkElement e1 = VisualTreeHelper.GetChild(stpParent, 1) as FrameworkElement;
                FrameworkElement e2 = VisualTreeHelper.GetChild(stpParent, 2) as FrameworkElement;
                FrameworkElement e3 = VisualTreeHelper.GetChild(stpParent, 3) as FrameworkElement;
                FrameworkElement e4 = VisualTreeHelper.GetChild(stpParent, 4) as FrameworkElement;
                FrameworkElement e5 = VisualTreeHelper.GetChild(stpParent, 5) as FrameworkElement;

                if (e0 == null || e1 == null || e2 == null || e3 == null || e4 == null || e5 == null)
                {
                    return;
                }

                if (e0 is TextBox)
                {
                    items.UserID = ((TextBox)e0).Text;
                }
                else
                {
                    return;
                }

                if (e1 is PasswordBox)
                {
                    items.Password = ((PasswordBox)e1).Password;
                }
                else
                {
                    return;
                }

                if (e2 is ComboBox)
                {
                    ComboBox tempcmb = (ComboBox)e2;
                    int startindex = tempcmb.SelectedItem.ToString().IndexOf(':');
                    string str = tempcmb.SelectedItem.ToString().Substring(0, startindex);
                    int result;
                    if (int.TryParse(str, out result) == false)
                    {
                        return;
                    }

                    items.アクセス権限 = result;
                }
                else
                {
                    return;
                }

                if (e3 is ComboBox)
                {
                    ComboBox cmb2 = (ComboBox)e3;
                    if (cmb2.SelectedIndex == 0)
                    {
                        items.削除フラグ = false;
                    }
                    else if (cmb2.SelectedIndex == 1)
                    {
                        items.削除フラグ = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }


                if (e4 is TextBlock)
                {
                    stpChildNumber = int.Parse(((TextBlock)e4).Text);



                }
                else
                {
                    return;
                }

                if (e5 is TextBlock)
                {
                    items.変更前のUserID = ((TextBlock)e5).Text;
                }
                else
                {
                    return;
                }

            }

            if (stpChildNumber == -1)
            {
                return;
            }

            if (items.変更前のUserID.Equals(""))
            {
                return;
            }


            if (SelectionChangeDatagridIndex.ContainsKey(stpChildNumber))
            {
                SelectionChangeDatagridIndex.Remove(stpChildNumber);
            }

            SelectionChangeDatagridIndex.Add(stpChildNumber, items);

        }

        private void UserList_ComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {

            AllUser情報取得結果 items = new AllUser情報取得結果();
            int stpChildNumber = -1;

            ComboBox cmb = sender as ComboBox;
            if (cmb == null)
            {
                return;
            }

            FrameworkElement fe = VisualTreeHelper.GetParent(cmb) as FrameworkElement;
            if (fe == null)
            {
                return;
            }
            if (fe is StackPanel)
            {
                var stpParent = ((StackPanel)fe) as StackPanel;


                // ０番目が入力UserID
                // １番目がPassword
                // ２番目がアクセスレベル
                // ３番目がステータス
                // ４番目がhide_counter でスタックパネルの番号
                // ５番目が変更前UserID
                if (VisualTreeHelper.GetChildrenCount(stpParent) != 6)
                {
                    return;
                }

                FrameworkElement e0 = VisualTreeHelper.GetChild(stpParent, 0) as FrameworkElement;
                FrameworkElement e1 = VisualTreeHelper.GetChild(stpParent, 1) as FrameworkElement;
                FrameworkElement e2 = VisualTreeHelper.GetChild(stpParent, 2) as FrameworkElement;
                FrameworkElement e3 = VisualTreeHelper.GetChild(stpParent, 3) as FrameworkElement;
                FrameworkElement e4 = VisualTreeHelper.GetChild(stpParent, 4) as FrameworkElement;
                FrameworkElement e5 = VisualTreeHelper.GetChild(stpParent, 5) as FrameworkElement;

                if (e0 == null || e1 == null || e2 == null || e3 == null || e4 == null || e5 == null)
                {
                    return;
                }

                if (e0 is TextBox)
                {
                    items.UserID = ((TextBox)e0).Text;
                }
                else
                {
                    return;
                }

                if (e1 is PasswordBox)
                {
                    items.Password = ((PasswordBox)e1).Password;
                }
                else
                {
                    return;
                }

                if (e2 is ComboBox)
                {
                    ComboBox tempcmb = (ComboBox)e2;
                    int startindex = tempcmb.SelectedItem.ToString().IndexOf(':');
                    string str = tempcmb.SelectedItem.ToString().Substring(0, startindex);
                    int result;
                    if (int.TryParse(str, out result) == false)
                    {
                        return;
                    }

                    items.アクセス権限 = result;
                }
                else
                {
                    return;
                }

                if (e3 is ComboBox)
                {
                    ComboBox cmb2 = (ComboBox)e3;
                    if (cmb2.SelectedIndex == 0)
                    {
                        items.削除フラグ = false;
                    }
                    else if (cmb2.SelectedIndex == 1)
                    {
                        items.削除フラグ = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }


                if (e4 is TextBlock)
                {
                    stpChildNumber = int.Parse(((TextBlock)e4).Text);
                }
                else
                {
                    return;
                }


                if (e5 is TextBlock)
                {
                    items.変更前のUserID = ((TextBlock)e5).Text;

                }
                else
                {
                    return;
                }

            }

            if (stpChildNumber == -1)
            {
                return;
            }

            if (items.変更前のUserID.Equals(""))
            {
                return;
            }


            if (SelectionChangeDatagridIndex.ContainsKey(stpChildNumber))
            {
                SelectionChangeDatagridIndex.Remove(stpChildNumber);
            }

            SelectionChangeDatagridIndex.Add(stpChildNumber, items);



        }

        private void 更新_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif

            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = Util.ServiceUtil.ReferenceCreater.GetUserManagementClient();

            client.AllUser情報更新実行Completed += new EventHandler<AllUser情報更新実行CompletedEventArgs>(client_AllUser情報更新実行Completed);

            ObservableCollection<AllUser情報取得結果> oc = new ObservableCollection<AllUser情報取得結果>();

            foreach (KeyValuePair<int, AllUser情報取得結果> kvp in this.SelectionChangeDatagridIndex)
            {
                oc.Add(kvp.Value);
            }

            this.SelectionChangeDatagridIndex.Clear();

            client.AllUser情報更新実行Async(oc);


        }

        void client_AllUser情報更新実行Completed(object sender, AllUser情報更新実行CompletedEventArgs e)
        {
            try
            {

                if (e == null)
                {
                    MessageBox.Show("更新に失敗しました。", "更新", MessageBoxButton.OK);
                }

                MessageBox.Show(e.Result, "更新", MessageBoxButton.OK);

                SetDataGridItemSource();



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



    }
}
