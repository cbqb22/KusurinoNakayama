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
using View.Service.File.Writer;
using View.Core.共通;
using View.Core.TopPage.Tab.掲示板.メイン;


namespace View.Core.TopPage.Tab.掲示板.子画面
{
    public partial class スレッド追加ChildWindow : ChildWindow
    {
        public スレッド追加ChildWindow()
        {
            InitializeComponent();

        }

        private bool _スレッド修正削除か;

        public bool スレッド修正削除か
        {
            get { return _スレッド修正削除か; }
            set { _スレッド修正削除か = value; }
        }

        private string _修正前スレッド名;

        public string 修正前スレッド名
        {
            get { return _修正前スレッド名; }
            set { _修正前スレッド名 = value; }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbInputTitle.Text))
            {
                MessageBox.Show("タイトルを入力して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.imageLabel.Content.ToString().Equals("右の画像を選択") || string.IsNullOrEmpty(this.imageLabel.Content.ToString()))
            {
                MessageBox.Show("画像を選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
            client.WriteThreadTitlesCompleted += new EventHandler<WriteThreadTitlesCompletedEventArgs>(client_WriteThreadTitlesCompleted);

            // スレッド修正か
            if (this.スレッド修正削除か)
            {
                if (this.imageLabel.Content.ToString().Equals("home.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.home, スレッド操作タイプ.修正, this.修正前スレッド名);
                }
                else if (this.imageLabel.Content.ToString().Equals("book1.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.book1, スレッド操作タイプ.修正, this.修正前スレッド名);
                }
                else if (this.imageLabel.Content.ToString().Equals("kinds1.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.kinds1, スレッド操作タイプ.修正, this.修正前スレッド名);
                }
                else if (this.imageLabel.Content.ToString().Equals("folder2.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.folder2, スレッド操作タイプ.修正, this.修正前スレッド名);
                }
                else if (this.imageLabel.Content.ToString().Equals("cross.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.cross, スレッド操作タイプ.修正, this.修正前スレッド名);
                }
                else if (this.imageLabel.Content.ToString().Equals("exclame.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.exclame, スレッド操作タイプ.修正, this.修正前スレッド名);
                }
                else
                {
                    MessageBox.Show("画像が選択されていない為、スレッドが追加されませんでした。", "エラー", MessageBoxButton.OK);
                    return;
                }
            }
            else
            {
                if (this.imageLabel.Content.ToString().Equals("home.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.home, スレッド操作タイプ.新規, null);
                }
                else if (this.imageLabel.Content.ToString().Equals("book1.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.book1, スレッド操作タイプ.新規, null);
                }
                else if (this.imageLabel.Content.ToString().Equals("kinds1.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.kinds1, スレッド操作タイプ.新規, null);
                }
                else if (this.imageLabel.Content.ToString().Equals("folder2.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.folder2, スレッド操作タイプ.新規, null);
                }
                else if (this.imageLabel.Content.ToString().Equals("cross.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.cross, スレッド操作タイプ.新規, null);
                }
                else if (this.imageLabel.Content.ToString().Equals("exclame.png"))
                {
                    client.WriteThreadTitlesAsync(this.tbInputTitle.Text, Service.File.Writer.画像種類Enum.exclame, スレッド操作タイプ.新規, null);
                }
                else
                {
                    MessageBox.Show("画像が選択されていない為、スレッドが追加されませんでした。", "エラー", MessageBoxButton.OK);
                    return;
                }
            }

        }

        void client_WriteThreadTitlesCompleted(object sender, WriteThreadTitlesCompletedEventArgs e)
        {
            try
            {

                if (e.Error == null)
                {
                    string msg = e.Result as string;
                    if (msg == null)
                    {
                        return;
                    }

                    // MessageBox.Show(msg, "確認", MessageBoxButton.OK);

                    SingletonInstances.掲示板FrameInstance.ThreadSelectorName.スレッド操作メッセージありか = true;
                    SingletonInstances.掲示板FrameInstance.ThreadSelectorName.スレッド操作メッセージ = msg;

                    SingletonInstances.掲示板FrameInstance.ThreadSelectorName.GetThreadTitles();
                }
                else
                {
                    MessageBox.Show("スレッド操作に失敗しました。再度、操作を行って下さい。");
                }

                this.DialogResult = true;

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


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void img_home_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img == null)
            {
                return;
            }

            if (img.Name.Equals("img_home"))
            {
                this.imageLabel.Content = "home.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (img.Name.Equals("img_book1"))
            {
                this.imageLabel.Content = "book1.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (img.Name.Equals("img_kinds1"))
            {
                this.imageLabel.Content = "kinds1.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (img.Name.Equals("img_folder2"))
            {
                this.imageLabel.Content = "folder2.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (img.Name.Equals("img_cross"))
            {
                this.imageLabel.Content = "cross.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (img.Name.Equals("img_exclame"))
            {
                this.imageLabel.Content = "exclame.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.imageLabel.Content = "";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }


        /// <summary>
        /// スレッド修正時にImagePathより対象のイメージを選択しておく
        /// </summary>
        /// <param name="ImagePath"></param>
        public void SetImageSelector(string ImagePath)
        {
            if (ImagePath.Equals("/etc/Icon/home.png"))
            {
                this.imageLabel.Content = "home.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (ImagePath.Equals("/etc/Icon/book1.png"))
            {
                this.imageLabel.Content = "book1.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (ImagePath.Equals("/etc/Icon/kinds1.png"))
            {
                this.imageLabel.Content = "kinds1.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (ImagePath.Equals("/etc/Icon/folder2.png"))
            {
                this.imageLabel.Content = "folder2.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (ImagePath.Equals("/etc/Icon/cross.png"))
            {
                this.imageLabel.Content = "cross.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.Red);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else if (ImagePath.Equals("/etc/Icon/exclame.png"))
            {
                this.imageLabel.Content = "exclame.png";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.imageLabel.Content = "";
                this.bd_home.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_book1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_kinds1.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_folder2.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_cross.BorderBrush = new SolidColorBrush(Colors.White);
                this.bd_exclame.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }

        private void btDeleteThread_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            if (MessageBox.Show("このスレッドを削除しますか？", "確認", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }


            FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();
            client.WriteThreadTitlesCompleted += new EventHandler<WriteThreadTitlesCompletedEventArgs>(client_WriteThreadTitlesCompleted);
            client.WriteThreadTitlesAsync(this.修正前スレッド名, 画像種類Enum.画像なし, スレッド操作タイプ.削除, null);


        }
    }
}

