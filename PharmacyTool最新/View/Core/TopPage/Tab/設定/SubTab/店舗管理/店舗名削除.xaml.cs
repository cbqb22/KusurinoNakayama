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
using View.Service.DAO.PharmacyTool.店舗;
using System.Collections.ObjectModel;
using View.Core.共通;

namespace View.Core.TopPage.Tab.設定.SubTab.店舗管理
{
    public partial class 店舗名削除 : UserControl
    {
        public 店舗名削除()
        {
            InitializeComponent();
        }

        private void bt削除_Click(object sender, RoutedEventArgs e)
        {

#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif

            if (this.cmb削除店舗名.SelectedIndex == -1)
            {
                MessageBox.Show("削除する店舗名を選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            Service.DAO.PharmacyTool.店舗.StoreDataClient client = Util.ServiceUtil.ReferenceCreater.GetStoreDataClient();
            client.店舗名削除Completed += new EventHandler<店舗名削除CompletedEventArgs>(client_店舗名削除Completed);
            client.店舗名削除Async(this.cmb削除店舗名.SelectedItem.ToString());

        }

        void client_店舗名削除Completed(object sender, 店舗名削除CompletedEventArgs e)
        {
            try
            {

                if (e.Error == null)
                {
                    MessageBox.Show(e.Result.ToString());
                    // すべての店舗名を更新する
                    SingletonInstances.在庫更新Instance.Create店舗名List();
                }
                else
                {
                    MessageBox.Show("店舗の削除に失敗しました。");
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

        public void Setcmb店舗名(ObservableCollection<PT店舗名リターンデータセット> dataset)
        {
            if (this.cmb削除店舗名.Items != null)
            {
                this.cmb削除店舗名.Items.Clear();
            }

            // 店舗名はUNIQUEなので、削除する店舗名で選択させる
            foreach (var set in dataset)
            {
                this.cmb削除店舗名.Items.Add(set.店舗名);
            }

            if (1 <= this.cmb削除店舗名.Items.Count)
            {
                this.cmb削除店舗名.SelectedIndex = 0;
            }


        }


    }
}
