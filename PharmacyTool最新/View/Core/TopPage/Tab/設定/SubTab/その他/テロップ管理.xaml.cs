using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using View.Service.File.Writer;
using View.Service.File.Reader;
using System.Collections.Generic;

namespace View.Core.TopPage.Tab.設定.SubTab.その他
{
    public partial class テロップ管理 : UserControl
    {
        public テロップ管理()
        {
            // 変数を初期化する必要があります
            InitializeComponent();
            LoadTeropData();
        }

        private string _再生テロップ;

        public string 再生テロップ
        {
            get { return _再生テロップ; }
            set
            {
                _再生テロップ = value;
            }
        }

        private void LoadTeropData()
        {
            FileReaderClient client = Util.ServiceUtil.ReferenceCreater.GetFileReaderClient();
            client.テロップ記事読み込みCompleted += new EventHandler<テロップ記事読み込みCompletedEventArgs>(client_テロップ記事読み込みCompleted);
            client.テロップ記事読み込みAsync();
        }

        void client_テロップ記事読み込みCompleted(object sender, テロップ記事読み込みCompletedEventArgs e)
        {
            try
            {


                if (e.Error == null)
                {
                    テロップ記事リターンEntity ent = e.Result as テロップ記事リターンEntity;
                    if (ent.読み込み成功か)
                    {
                        this.再生テロップ = ent.テロップ記事;
                        this.tbTerop.Text = ent.テロップ記事;
                    }
                    else
                    {
                        this.再生テロップ = "";
                        this.tbTerop.Text = "";
                    }
                }
                else
                {
                    this.再生テロップ = "";
                    this.tbTerop.Text = "";
                }

                List<string> teroplist = new List<string>();

                int startindex = 0;
                int endindex = 0;
                int charlength = 0;
                while ((endindex = 再生テロップ.IndexOf("\r", startindex) - 1) != -2)
                {
                    charlength = endindex - startindex + 1;
                    string str = 再生テロップ.Substring(startindex, charlength);
                    teroplist.Add(str);

                    // 現在の①から\r分をずらす
                    startindex = endindex + 2;
                }

                // 残りを追記
                string st = 再生テロップ.Substring(startindex);
                teroplist.Add(st);

                if (0 < teroplist.Count)
                {
                    // Listの設定、カウンター0
                    Core.共通.SingletonInstances.TabItemControlInstance.StoryboardLogoReStart(teroplist, 0);
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

        private void bt更新_Click(object sender, RoutedEventArgs e)
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

            if (this.tbTerop.Text == null)
            {
                return;
            }

            List<string> teroplist = new List<string>();

            int startindex = 0;
            int endindex = 0;
            int charlength = 0;
            while ((endindex = tbTerop.Text.IndexOf("\r", startindex) - 1) != -2)
            {
                charlength = endindex - startindex + 1;
                string str = tbTerop.Text.Substring(startindex, charlength);
                teroplist.Add(str);

                // 現在の①から\r分をずらす
                startindex = endindex + 2;
            }

            foreach (var list in teroplist)
            {
                if (100 <= list.Length)
                {
                    if (MessageBox.Show("テロップ１行の文字数が１００文字を超えると、\r\nテロップが途中で切れる可能性があります。\r\n\r\nよろしいですか？", "注意", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
            }



            FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
            client.テロップ変更実行Completed += new EventHandler<テロップ変更実行CompletedEventArgs>(client_テロップ変更実行Completed);
            client.テロップ変更実行Async(this.tbTerop.Text);


        }

        void client_テロップ変更実行Completed(object sender, テロップ変更実行CompletedEventArgs e)
        {

            try
            {


                if (e.Error == null)
                {
                    MessageBox.Show(e.Result.ToString());
                    LoadTeropData();
                }
                else
                {
                    MessageBox.Show("テロップ編集に失敗しました。");
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

        private void bt元に戻す_Click(object sender, RoutedEventArgs e)
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

            if (this.再生テロップ == null)
            {
                return;
            }

            this.tbTerop.Text = 再生テロップ;
        }
    }
}