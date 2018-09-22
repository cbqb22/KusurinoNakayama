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
    public class LoginCheck
    {
        public void CallLoginCheck(string 入力ユーザーID,string 入力コンフィデンシャル)
        {
            Service.DAO.PharmacyTool.LoginCheckClient client = Util.ServiceUtil.ReferenceCreater.GetログインチェックClient();

            client.LoginCheck実行Completed += new EventHandler<View.Service.DAO.PharmacyTool.LoginCheck実行CompletedEventArgs>(client_LoginCheck実行Completed);

            client.LoginCheck実行Async(入力ユーザーID, 入力コンフィデンシャル);


        }

        void client_LoginCheck実行Completed(object sender, View.Service.DAO.PharmacyTool.LoginCheck実行CompletedEventArgs e)
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
