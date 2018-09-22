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
using View.Util.Common;
using View.Core.共通;
using View.Service.DAO.PharmacyTool.ユーザー管理;


namespace View.Core.TopPage.Tab.設定.SubTab.ユーザー管理
{
    public partial class ログインユーザー更新 : UserControl
    {
        public ログインユーザー更新()
        {
            InitializeComponent();

            SingletonInstances.ログインユーザー更新Instance = this;

            SetInputFormToDefaultValue();
        }


        private void ユーザー更新Button_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif
            if (string.IsNullOrEmpty(this.PasswordTextBox.Password))
            {
                MessageBox.Show("Passwordを入力して下さい。", "入力チェック", MessageBoxButton.OK);
                return;
            }

            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = Util.ServiceUtil.ReferenceCreater.GetUserManagementClient();

            client.LoginUser情報更新実行Completed += new EventHandler<LoginUser情報更新実行CompletedEventArgs>(client_LoginUser情報更新実行Completed);

            AllUser情報取得結果 結果 = new AllUser情報取得結果();
            結果.UserID = PageScope.ユーザーID;
            結果.Password = this.PasswordTextBox.Password;

            client.LoginUser情報更新実行Async(結果);


        }

        void client_LoginUser情報更新実行Completed(object sender, LoginUser情報更新実行CompletedEventArgs e)
        {
            try
            {


                if (e == null)
                {
                    MessageBox.Show("更新に失敗しました。", "更新", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(e.Result, "更新", MessageBoxButton.OK);
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

        private void クリア_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif

            SetInputFormToDefaultValue();
        }

        private void SetInputFormToDefaultValue()
        {
            this.UserIDTextBlock2.Text = PageScope.ユーザーID;
            this.PasswordTextBox.Password = "";
        }


    }
}
