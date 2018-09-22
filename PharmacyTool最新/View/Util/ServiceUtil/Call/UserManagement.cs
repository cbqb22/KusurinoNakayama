using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace View.Util.ServiceUtil.Call
{
    public class UserManagement
    {
        public void CreateNewUser実行(string UserID, string Confidential, int アクセス権限)
        {
            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = Util.ServiceUtil.ReferenceCreater.GetUserManagementClient();

            client.CreateNewUser実行Completed += new EventHandler<View.Service.DAO.PharmacyTool.ユーザー管理.CreateNewUser実行CompletedEventArgs>(client_CreateNewUser実行Completed);

            client.CreateNewUser実行Async(UserID, Confidential, アクセス権限);

        }

        void client_CreateNewUser実行Completed(object sender, View.Service.DAO.PharmacyTool.ユーザー管理.CreateNewUser実行CompletedEventArgs e)
        {

            try
            {


                View.Service.DAO.PharmacyTool.ユーザー管理.CreateNewUser結果 result = e.Result;

                MessageBox.Show(result.エラーメッセージ, "ユーザー作成", MessageBoxButton.OK);




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

        public void DeleteUser実行(string UserID)
        {
            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = Util.ServiceUtil.ReferenceCreater.GetUserManagementClient();

            client.DeleteUser実行Completed += new EventHandler<View.Service.DAO.PharmacyTool.ユーザー管理.DeleteUser実行CompletedEventArgs>(client_DeleteUser実行Completed);

            client.DeleteUser実行Async(UserID);
        }

        void client_DeleteUser実行Completed(object sender, View.Service.DAO.PharmacyTool.ユーザー管理.DeleteUser実行CompletedEventArgs e)
        {
            try
            {



                View.Service.DAO.PharmacyTool.ユーザー管理.DeleteUser結果 result = e.Result;

                MessageBox.Show(result.エラーメッセージ, "ユーザー削除", MessageBoxButton.OK);




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

        public void AllUser情報取得()
        {
            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = Util.ServiceUtil.ReferenceCreater.GetUserManagementClient();
            client.AllUser情報取得実行Completed += new EventHandler<View.Service.DAO.PharmacyTool.ユーザー管理.AllUser情報取得実行CompletedEventArgs>(client_AllUser情報取得実行Completed);

            client.AllUser情報取得実行Async();
        }

        void client_AllUser情報取得実行Completed(object sender, View.Service.DAO.PharmacyTool.ユーザー管理.AllUser情報取得実行CompletedEventArgs e)
        {
            try
            {




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
