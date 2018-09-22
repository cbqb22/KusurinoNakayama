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

namespace View.Core.TopPage.Tab.設定.SubTab.店舗管理
{
    public partial class 新規店舗名作成 : UserControl
    {
        public 新規店舗名作成()
        {
            InitializeComponent();
        }

        private void bt作成_Click(object sender, RoutedEventArgs e)
        {

#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif

            if (string.IsNullOrEmpty(this.tb新規店舗名.Text))
            {
                MessageBox.Show("作成する店舗名を入力して下さい。", "入力チェック", MessageBoxButton.OK);
                return;
            }

            Service.DAO.PharmacyTool.店舗.StoreDataClient client = Util.ServiceUtil.ReferenceCreater.GetStoreDataClient();
            client.新規店舗名作成Completed += new EventHandler<Service.DAO.PharmacyTool.店舗.新規店舗名作成CompletedEventArgs>(client_新規店舗名作成Completed);
            client.新規店舗名作成Async(this.tb新規店舗名.Text);


        }

        void client_新規店舗名作成Completed(object sender, Service.DAO.PharmacyTool.店舗.新規店舗名作成CompletedEventArgs e)
        {

            try
            {

                if (e.Error == null)
                {
                    string s = e.Result as string;

                    MessageBox.Show(s);

                    if (s.Equals("店舗名を作成しました。"))
                    {
                        this.tb新規店舗名.Text = "";
                        Core.共通.SingletonInstances.在庫更新Instance.Create店舗名List();
                    }

                }
                else
                {
                    MessageBox.Show(e.Error.Message + e.Error.StackTrace);
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

        private void btクリア_Click(object sender, RoutedEventArgs e)
        {

#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif

            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            this.tb新規店舗名.Text = "";


        }
    }
}
