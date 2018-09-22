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
using View.Core.共通;
using View.Core.TopPage.Tab.掲示板.メイン;

namespace View.Core.Login
{
    public partial class LoginCheckDialog : ChildWindow
    {
        public LoginCheckDialog()
        {
            InitializeComponent();

            this.Closed += new EventHandler(LoginCheckDialog_Closed);
            this.Loaded += new RoutedEventHandler(LoginCheckDialog_Loaded);

            //if (System.Windows.Browser.HtmlPage.IsPopupWindowAllowed)
            //{
            //    System.Windows.Browser.HtmlPage.PopupWindow(new Uri("http://www.my-world.me/PharmacyTool/ClientBin/MEDIS/MEDIS.TXT"), "_blank", null);
            //}


            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.my-world.me/PharmacyTool/ClientBin/MEDIS/MEDIS.TXT"));


            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.my-world.me/PharmacyTool/ClientBin/MEDIS/MEDIS.TXT"), "_blank", "toolbar=no,location=no,status=no,menubar=no,resizable=yes");


            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.my-world.me/PharmacyTool/ClientBin/MEDIS/MEDIS.TXT"), "_newWindow");            
        }

        void LoginCheckDialog_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Plugin.Focus();
            this.ユーザーIDTextBox.Focus();
        }



        void LoginCheckDialog_Closed(object sender, EventArgs e)
        {
            if (this.DialogResult == false)
            {
                string Error401Path = Core.共通.Settings.Error401Path;

                // ログインに失敗したら、401エラー認証失敗のページへ遷移
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(Error401Path), "_self");

            }
            else
            {
                Core.共通.SingletonInstances.在庫管理FrameInstance.SetSearchTextBoxFocus();

                // 管理ユーザーもしくは本部ユーザーならば、掲示板のスレッド追加と修正ボタンをアクティブ
                if (Core.共通.PageScope.アクティブアクセス権限 == 0 || Core.共通.PageScope.アクティブアクセス権限 == 1)
                {
                    Core.共通.SingletonInstances.掲示板FrameInstance.btスレット追加.Visibility = Visibility.Visible;
                    Core.共通.SingletonInstances.掲示板FrameInstance.bt修正.Visibility = Visibility.Visible;
                }

                // すでに表示中の記事の削除ボタンを権限によって再表示できるかチェック
                if (SingletonInstances.掲示板FrameInstance != null && SingletonInstances.掲示板FrameInstance.StackP1 != null)
                {
                    StackPanel stp = View.Core.共通.SingletonInstances.掲示板FrameInstance.StackP1;


                    int count = VisualTreeHelper.GetChildrenCount(stp);

                    for (int i = 0; i < count; i++)
                    {
                        FrameworkElement fe = VisualTreeHelper.GetChild(stp, i) as FrameworkElement;

                        if (fe == null)
                        {
                            continue;
                        }

                        if (fe is 投稿返信セット)
                        {
                            投稿返信セット set = fe as 投稿返信セット;
                            set.Set削除ボタン表示設定();

                            foreach (var s in set.stp返信セット.Children)
                            {
                                if (s is 返信セット)
                                {
                                    ((返信セット)s).Set削除ボタン表示設定();
                                }
                            }

                            continue;
                        }
                    }

                }

                // 管理ユーザーならば、設定タブを表示し、すべての機能をオン
                if (Core.共通.PageScope.アクティブアクセス権限 == 0)
                {
                    TopPage.TabItemControl instance;
                    if ((instance = Core.共通.SingletonInstances.TabItemControlInstance) != null)
                    {
                        instance.設定TabItem.Visibility = Visibility.Visible;
                    }
                }

                // 本部ユーザーならば、設定タブを表示し、すべての機能から既存ユーザーのパスワードを変更できない様にする
                if (Core.共通.PageScope.アクティブアクセス権限 == 1)
                {
                    TopPage.TabItemControl instance;
                    if ((instance = Core.共通.SingletonInstances.TabItemControlInstance) != null)
                    {
                        instance.設定TabItem.Visibility = Visibility.Visible;
                    }

                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.更新_管理者TabItem.Visibility = Visibility.Collapsed;
                }

                // 在庫更新店舗名称をログインユーザーごとに切替
                Core.共通.SingletonInstances.在庫更新Instance.Set一般ユーザーSelectIndex();

                // 投稿フォームのおなまえをユーザーの表示名称で設定する

                int cnt = VisualTreeHelper.GetChildrenCount(Core.共通.SingletonInstances.掲示板FrameInstance.StackP1);
                for (int i = 0; i < cnt; i++)
                {
                    var fe = VisualTreeHelper.GetChild(Core.共通.SingletonInstances.掲示板FrameInstance.StackP1, i) as FrameworkElement;

                    if (fe == null)
                    {
                        continue;
                    }

                    if (fe is 投稿フォーム)
                    {
                        ((投稿フォーム)fe).SetDefaltToおなまえTextBox();
                        break;
                    }
                }

                // 在庫更新タブの更新種別でMEDISデータを管理者権限もしくは、本部ユーザーならば加える。
                Core.共通.SingletonInstances.在庫更新Instance.Create更新種別ListAfterLogin();
            }

        }

        private int failurecounter = 0;

        void client_LoginCheck実行Completed(object sender, View.Service.DAO.PharmacyTool.LoginCheck実行CompletedEventArgs e)
        {
            try
            {
                ObservableCollection<Service.DAO.PharmacyTool.ログインチェック結果> 結果 = e.Result;

                if (e == null || 結果.Count == 0 || 結果[0].チェック成功か == false)
                {
                    if (e == null)
                    {
                        MessageBox.Show("通信でエラーになりました。再度、ログインして下さい。", "エラー", MessageBoxButton.OK);

                    }
                    else if (結果.Count == 0)
                    {
                        MessageBox.Show("ユーザーIDまたはパスワードが違います。", "エラー", MessageBoxButton.OK);
                    }
                    else if (結果[0].チェック成功か == false)
                    {
                        MessageBox.Show(結果[0].エラーメッセージ, "エラー", MessageBoxButton.OK);

                    }

                    failurecounter++;
                    if (failurecounter == 3)
                    {
                        this.DialogResult = false;

                        string Error401Path = Core.共通.Settings.Error401Path;

                        // ログインに失敗したら、401エラー認証失敗のページへ遷移
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(Error401Path), "_self");

                    }

                }
                else
                {

                    SetLoginInfo(結果[0]);


                    this.DialogResult = true;
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

        private void SetLoginInfo(Service.DAO.PharmacyTool.ログインチェック結果 結果)
        {
            // PageScopeインスタンスにログイン情報を格納
            Core.共通.PageScope.アクティブアクセス権限 = 結果.アクセス権限;
            Core.共通.PageScope.表示名称 = 結果.表示名称;
            Core.共通.PageScope.ユーザーID = 結果.ユーザーID;

            // 管理ユーザーまたは本部ユーザーでログインした場合は、設定タブを表示
            if (PageScope.アクティブアクセス権限 == 0 || PageScope.アクティブアクセス権限 == 1)
            {
                SingletonInstances.ユーザー管理TabItemInstance.更新_管理者TabItem.Visibility = Visibility.Visible;
                SingletonInstances.ユーザー管理TabItemInstance.更新TabItem.Visibility = Visibility.Collapsed;
                SingletonInstances.ユーザー管理TabItemInstance.新規TabItem.Visibility = Visibility.Visible;
                SingletonInstances.ユーザー管理TabItemInstance.新規TabItem.IsSelected = true;
            }
            else
            {
                SingletonInstances.ユーザー管理TabItemInstance.更新_管理者TabItem.Visibility = Visibility.Collapsed;
                SingletonInstances.ユーザー管理TabItemInstance.更新TabItem.Visibility = Visibility.Visible;
                SingletonInstances.ユーザー管理TabItemInstance.新規TabItem.Visibility = Visibility.Collapsed;
                SingletonInstances.ユーザー管理TabItemInstance.更新TabItem.IsSelected = true;
            }

            // ログインユーザー更新タブ
            SingletonInstances.ログインユーザー更新Instance.UserIDTextBlock2.Text = Core.共通.PageScope.ユーザーID;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Service.DAO.PharmacyTool.LoginCheckClient client = Util.ServiceUtil.ReferenceCreater.GetログインチェックClient();
            client.LoginCheck実行Completed += new EventHandler<View.Service.DAO.PharmacyTool.LoginCheck実行CompletedEventArgs>(client_LoginCheck実行Completed);
            client.LoginCheck実行Async(ユーザーIDTextBox.Text, パスワードTextBox.Password);

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Browser.HtmlPage.Window.Eval("(window.open('','_top').opener=top).close();");
            this.DialogResult = false;
        }

        private void UserIDTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
            {
                return;
            }

            if (e.Key == Key.Enter)
            {
                this.パスワードTextBox.Focus();
            }
        }

        private void PasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }

            if (e.Key == Key.Enter)
            {
                Service.DAO.PharmacyTool.LoginCheckClient client = Util.ServiceUtil.ReferenceCreater.GetログインチェックClient();
                client.LoginCheck実行Completed += new EventHandler<View.Service.DAO.PharmacyTool.LoginCheck実行CompletedEventArgs>(client_LoginCheck実行Completed);
                client.LoginCheck実行Async(ユーザーIDTextBox.Text, パスワードTextBox.Password);
                this.LoginButton.Focus();
            }

        }


    }
}

