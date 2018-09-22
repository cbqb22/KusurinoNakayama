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
using View.Service.DAO.PharmacyTool.店舗;


namespace View.Core.TopPage.Tab.設定.SubTab.ユーザー管理
{
    public partial class 新規ユーザー作成 : UserControl
    {
        public 新規ユーザー作成()
        {
            InitializeComponent();

            Setアクセス権限ComboBoxItem(null);
            SetInputFormToDefaultValue();
        }

        private List<Tuple<int, string>> ltp = new List<Tuple<int, string>>();



        public void Setアクセス権限ComboBoxItem(ObservableCollection<PT店舗名リターンデータセット> pT店舗名リターンデータセット)
        {

            // 一旦ComboBoxItemをきれいにする
            this.アクセス権限ComboBox.Items.Clear();

            ltp.Clear();

            // アクセスレベルとコンボボックスの名称

            Tuple<int, string> tp = new Tuple<int, string>();
            tp.Value1 = 0;
            tp.Value2 = "0:管理ユーザー";

            Tuple<int, string> tp2 = new Tuple<int, string>();

            tp2.Value1 = 1;
            tp2.Value2 = "1:本部ユーザー";


            ltp.Add(tp);
            ltp.Add(tp2);

            if (pT店舗名リターンデータセット != null)
            {
                foreach (var dset in pT店舗名リターンデータセット)
                {
                    Tuple<int, string> tp3 = new Tuple<int, string>();
                    tp3.Value1 = dset.ID;
                    tp3.Value2 = string.Format("{0}:{1}", dset.ID, dset.店舗名);

                    ltp.Add(tp3);

                }
            }

            foreach (var t in ltp)
            {
                this.アクセス権限ComboBox.Items.Add(t.Value2);
            }
            if (1 <= this.アクセス権限ComboBox.Items.Count)
            {
                this.Setアクセス権限ComboBoxSelectedIndex(0);
            }
        }

        private void Setアクセス権限ComboBoxSelectedIndex(int index)
        {
            this.アクセス権限ComboBox.SelectedIndex = index;
        }

        private void ユーザー作成Button_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
#elif NAKAYAMA
#else
            MessageBox.Show("デモ版の為、この操作は実行できません。");
            return;
#endif
            if (string.IsNullOrEmpty(this.UserIDTextBox.Text))
            {
                MessageBox.Show("UserIDを入力して下さい。", "入力チェック", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(this.PasswordTextBox.Password))
            {
                MessageBox.Show("Passwordを入力して下さい。", "入力チェック", MessageBoxButton.OK);
                return;
            }

            if (this.アクセス権限ComboBox.SelectedItem == null)
            {
                MessageBox.Show("アクセス権限を選択して下さい。", "入力チェック", MessageBoxButton.OK);
                return;
            }

            if (ltp.Count == 0)
            {
                return;
            }

            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = Util.ServiceUtil.ReferenceCreater.GetUserManagementClient();
            client.CreateNewUser実行Completed += new EventHandler<View.Service.DAO.PharmacyTool.ユーザー管理.CreateNewUser実行CompletedEventArgs>(client_CreateNewUser実行Completed);
            client.CreateNewUser実行Async(this.UserIDTextBox.Text, this.PasswordTextBox.Password, ltp[this.アクセス権限ComboBox.SelectedIndex].Value1);



        }

        void client_CreateNewUser実行Completed(object sender, View.Service.DAO.PharmacyTool.ユーザー管理.CreateNewUser実行CompletedEventArgs e)
        {

            try
            {



                if (e.Error == null)
                {
                    View.Service.DAO.PharmacyTool.ユーザー管理.CreateNewUser結果 result = e.Result;

                    if (result.ユーザー作成成功かどうか)
                    {
                        MessageBox.Show(result.エラーメッセージ, "ユーザー作成", MessageBoxButton.OK);
                        // 成功した場合は、入力欄をデフォルトに設定する
                        SetInputFormToDefaultValue();
                    }
                    else
                    {
                        MessageBox.Show(result.エラーメッセージ, "ユーザー作成", MessageBoxButton.OK);
                    }

                    Core.共通.SingletonInstances.ユーザー管理TabItemInstance.uc既存ユーザー更新.SetDataGridItemSource();
                }
                else
                {
                    MessageBox.Show("新規ユーザー作成に失敗しました。再度、行って下さい。");
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
            this.UserIDTextBox.Text = "";
            this.PasswordTextBox.Password = "";
            this.アクセス権限ComboBox.SelectedIndex = 0;
        }
    }
}
