using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WF = System.Windows.Forms;
using System.IO;
using MediWebOrderWatcher.View;

namespace MediWebOrderWatcher
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        WF.NotifyIcon ni = new WF.NotifyIcon();
        public const string tempSaveFolderPath = @"c:\MediWebOrderWatcherTemp";
        public const string watchingfolderpath = @"C:\MEDICODE DATA";

        public MainWindow()
        {
            InitializeComponent();
            CheckInit();
            StartWatching();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = System.Windows.WindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        private void CheckInit()
        {
            if (!System.IO.Directory.Exists(watchingfolderpath))
            {
                System.IO.Directory.CreateDirectory(watchingfolderpath);
            }

            if (!System.IO.Directory.Exists(tempSaveFolderPath))
            {
                System.IO.Directory.CreateDirectory(tempSaveFolderPath);
            }
        }

        #region FileSystemWatcherW関連

        private FileSystemWatcher watcher = null;
        private bool デッド切迫チェック中か = false;

        private void StartWatching()
        {
            if (watcher != null) return;

            watcher = new FileSystemWatcher();
            //監視するディレクトリを指定
            watcher.Path = @"C:\MEDICODE DATA";
            //最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
            watcher.NotifyFilter =
                (System.IO.NotifyFilters.LastAccess
                | System.IO.NotifyFilters.LastWrite
                | System.IO.NotifyFilters.FileName
                | System.IO.NotifyFilters.DirectoryName);
            //すべてのファイルを監視
            watcher.Filter = "";
            //UIのスレッドにマーシャリングする
            //コンソールアプリケーションでの使用では必要ない
            //watcher.SynchronizingObject = this;

            //watcher = new System.IO.FileSystemWatcher(@"C:\Users\arimura\Desktop", "*.txt");
            //watcher.IncludeSubdirectories = false;


            //イベントハンドラの追加
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);

            //監視を開始する
            watcher.EnableRaisingEvents = true;
            //MessageBox.Show("監視を開始しました。");
        }

        private void StopWatching()
        {
            //監視を終了
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            watcher = null;
            MessageBox.Show("監視を終了しました。");
        }

        private void watcher_Changed(System.Object source,
            System.IO.FileSystemEventArgs e)
        {

            try
            {
                switch (e.ChangeType)
                {
                    case System.IO.WatcherChangeTypes.Changed:
                        //MessageBox.Show(
                        //    "ファイル 「" + e.FullPath + "」が変更されました。");
                        //他店デッド品切迫品有無チェック();
                        MoveChangeFile(e.FullPath, e.Name);
                        break;
                    case System.IO.WatcherChangeTypes.Created:
                        //MessageBox.Show(
                        //    "ファイル 「" + e.FullPath + "」が作成されました。");
                        //他店デッド品切迫品有無チェック();
                        MoveCreateFile(e.FullPath, e.Name);
                        break;
                    case System.IO.WatcherChangeTypes.Deleted:
                        //MessageBox.Show(
                        //    "ファイル 「" + e.FullPath + "」が削除されました。");
                        //他店デッド品切迫品有無チェック();
                        MoveDeleteFile(e.FullPath, e.Name);
                        break;
                }
            }
            catch
            {
            }
        }

        private void MoveDeleteFile(string sourceFilepath, string filename)
        {
            string savefilename = System.IO.Path.Combine(tempSaveFolderPath + "\\", DateTime.Now.ToString("yyyyMMddhhmmss") + "_Delete_" + filename);

            if (System.IO.File.Exists(savefilename))
            {
                return;
            }

            System.IO.File.Copy(sourceFilepath, savefilename);
        }


        private void MoveCreateFile(string sourceFilepath, string filename)
        {
            string savefilename = System.IO.Path.Combine(tempSaveFolderPath + "\\", DateTime.Now.ToString("yyyyMMddhhmmss") + "_Create_" + filename);

            if (System.IO.File.Exists(savefilename))
            {
                return;
            }

            System.IO.File.Copy(sourceFilepath, savefilename);
        }

        private void MoveChangeFile(string sourceFilepath, string filename)
        {
            string savefilename = System.IO.Path.Combine(tempSaveFolderPath + "\\", DateTime.Now.ToString("yyyyMMddhhmmss") + "_Change_" + filename);

            if (System.IO.File.Exists(savefilename))
            {
                return;
            }

            System.IO.File.Copy(sourceFilepath, savefilename);
        }

        private void MoveRenameFile(string sourceFilepath, string filename)
        {
            string savefilename = System.IO.Path.Combine(tempSaveFolderPath + "\\", DateTime.Now.ToString("yyyyMMddhhmmss") + "_Rename_" + filename);

            if (System.IO.File.Exists(savefilename))
            {
                return;
            }

            System.IO.File.Copy(sourceFilepath, savefilename);
        }



        private void watcher_Renamed(System.Object source,
            System.IO.RenamedEventArgs e)
        {
            MoveRenameFile(e.FullPath, e.Name);

            //MessageBox.Show(
            //    "ファイル 「" + e.FullPath + "」の名前が変更されました。");
        }


        // SomeDelegate という名前のデリゲート型を定義
        delegate void SomeDelegate();

        private void 他店デッド品切迫品有無チェック()
        {
            デッド切迫チェック中か = true;


            TextBox tb = new TextBox();
            tb.Text = "あああ";
            this.AddChild(tb);

            デッド切迫チェック中か = false;
        }



        #endregion

        private void btn発注書作成画面起動_Click(object sender, RoutedEventArgs e)
        {
            発注書作成画面Window win = new 発注書作成画面Window();
            win.Show();
        }

    }
}
