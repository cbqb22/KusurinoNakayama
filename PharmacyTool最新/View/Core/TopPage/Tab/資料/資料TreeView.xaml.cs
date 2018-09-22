using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using View.Core.共通.Entity;
using System.Collections.Generic;
using View.Service.File.TreeView;
using System.Collections.ObjectModel;
using View.Service.File.Writer;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;


namespace View.Core.TopPage.Tab.資料
{
    public partial class 資料TreeView : UserControl
    {
        private bool _初回ロード済か;
        public bool 初回ロード済か
        {
            get { return _初回ロード済か; }
            set { _初回ロード済か = value; }
        }


        public 資料TreeView()
        {
            // 変数を初期化する必要があります
            InitializeComponent();
            View.Core.共通.SingletonInstances.資料Instance = this;

            DoInit();

            this.Loaded += new RoutedEventHandler(資料TreeView_Loaded);
            this.資料tv.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(資料tv_SelectedItemChanged);

        }



        void 資料TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!初回ロード済か)
            {
                初回ロード済か = true;
                SelectTreeViewItemDefault();
            }

        }

        public void SelectTreeViewItemDefault()
        {
            // まだTreeviewのVisualTreeが作成されていない場合があるので
            // treeviewをアップデートしておく
            this.資料tv.UpdateLayout();

            // Rootフォルダを初期選択する
            if (this.資料tv != null && 1 <= this.資料tv.Items.Count)
            {
                TreeViewItem firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
                if (firstTreeviewitem == null)
                {
                    //// 非同期で、TreeViewItemがnullでなくなるまで行い、その後、一番最初を選択させる。
                    //this.資料tv.Dispatcher.BeginInvoke(delegate()
                    //{
                    //    while (firstTreeviewitem == null)
                    //    {
                    //        firstTreeviewitem = (TreeViewItem)this.資料tv.ItemContainerGenerator.ContainerFromItem(this.資料tv.Items[0]);
                    //    }
                    //    firstTreeviewitem.IsSelected = true;
                    //    this.選択アイテムTextBlock.Text = "\\";

                    //});

                    this.選択アイテムTextBlock.Text = "";

                }
                else
                {
                    firstTreeviewitem.IsExpanded = true; //Rootは開いたままにする
                    firstTreeviewitem.IsSelected = true;
                    this.選択アイテムTextBlock.Text = "\\";
                    return;
                }
            }
        }

        public void DoInit()
        {
            TreeViewManagerClient client = Util.ServiceUtil.ReferenceCreater.GetTreeViewManagerClient();

            client.Create資料TreeViewCompleted += new EventHandler<Create資料TreeViewCompletedEventArgs>(client_Create資料TreeViewCompleted_Init);
            client.Create資料TreeViewAsync();
        }


        public void DoCreate資料TreeViewItemSource()
        {
            TreeViewManagerClient client = Util.ServiceUtil.ReferenceCreater.GetTreeViewManagerClient();

            client.Create資料TreeViewCompleted += new EventHandler<Create資料TreeViewCompletedEventArgs>(client_Create資料TreeViewCompleted);
            client.Create資料TreeViewAsync();
        }

        /// <summary>
        /// 資料タブが読み込まれる前はこちらを使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_Create資料TreeViewCompleted_Init(object sender, Create資料TreeViewCompletedEventArgs e)
        {
            if (e == null)
            {
            }
            else
            {
                ObservableCollection<View.Service.File.TreeView.TreeViewItemData> col = e.Result;

                this.資料tv.ItemsSource = col;
                // UCが読み込まれる前にこれを呼ぶと、止まる。
                // SelectTreeViewItemDefault();
            }

        }


        /// <summary>
        /// 資料タブが読み込まれたあとは、こちらを使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_Create資料TreeViewCompleted(object sender, Create資料TreeViewCompletedEventArgs e)
        {
            try
            {


                if (e == null)
                {
                }
                else
                {
                    ObservableCollection<View.Service.File.TreeView.TreeViewItemData> col = e.Result;

                    this.資料tv.ItemsSource = null;
                    this.資料tv.ItemsSource = col;
                    SelectTreeViewItemDefault();
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


        void 資料tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var s = this.資料tv.SelectedItem as TreeViewItemData;

            if (s == null)
            {
                return;
            }


            string ufrp = Core.共通.Settings.UploadFileRootPath;
            string relativepath = "";
            if (ufrp.Equals(s.PathFromRoot))
            {
                relativepath = "\\"; // Rootディレクトリを指定
            }
            else
            {
                relativepath = s.PathFromRoot.Replace(ufrp, "");
            }



            if (s.IsDirectory == false)
            {
                this.選択アイテムTextBlock.Text = relativepath;
            }
            else
            {
                this.選択アイテムTextBlock.Text = relativepath;
            }
        }


        private void 参照Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "All Files (*.*)|*.*";
            if ((bool)ofd.ShowDialog())
            {
                if (ofd.File.Name.Contains("@"))
                {
                    MessageBox.Show("ファイル名に「@」がつくファイルは選択出来ません。", "エラー", MessageBoxButton.OK);
                    return;
                }

                // プログレスバーのセット
                this.gdProgress.Visibility = Visibility.Visible;
                this.pgUpload.Value = 0d;
                this.tbPercent.Text = "0%";

                this.アップロードファイルTextBox.Text = ofd.File.Name;
                this.File = ofd.File;

            }
        }


        private void UploadFile(string fileName, Stream data)
        {
            Util.Common.UriBuilderBase ub = new View.Util.Common.UriBuilderBase(Core.共通.Settings.GenericHandlerPath);

            ub.Query = string.Format(fileName);

            WebClient c = new WebClient();
            try
            {
                c.OpenWriteCompleted += (sender, e) =>
                {
                    PushData(data, e.Result);
                    e.Result.Close();
                    data.Close();
                    DoCreate資料TreeViewItemSource();
                    SelectTreeViewItemDefault();
                };

                c.OpenWriteAsync(ub.Uri);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void PushData(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {

            if (Status == FileUploadStatus.Uploading)
            {
                MessageBox.Show("他のファイルをアップロード中です。\r\n完了するまでお待ち下さい", "確認", MessageBoxButton.OK);
                return;
            }

            if (File == null)
            {
                MessageBox.Show("ファイルを選択して下さい", "確認", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrEmpty(this.選択アイテムTextBlock.Text))
            {
                MessageBox.Show("アップロードするフォルダを選択して下さい", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.アップロードファイルTextBox.Text.Contains("@"))
            {
                MessageBox.Show("アップロードするファイル名に「@」を含める事は出来ません。", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.アップロードファイルTextBox.Text.Contains("\\"))
            {
                MessageBox.Show("アップロードするファイル名に「\\」を含める事は出来ません。", "確認", MessageBoxButton.OK);
                return;
            }

            TreeViewItemData tvid = this.資料tv.SelectedItem as TreeViewItemData;

            if (!tvid.IsDirectory)
            {
                MessageBox.Show("ファイルが選択されています、フォルダを選択して下さい", "確認", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show(string.Format("「{0}」を追加します。\r\n  宜しいですか？", this.アップロードファイルTextBox.Text), "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {

                CheckFileOnServer();


            }
            else
            {
                return;
            }
        }



        private void フォルダ作成Button_Click(object sender, RoutedEventArgs e)
        {
            var sitem = this.資料tv.SelectedItem as TreeViewItemData;

            if (sitem == null || string.IsNullOrEmpty(this.選択アイテムTextBlock.Text))
            {
                MessageBox.Show("左よりフォルダを追加する場所を選択して下さい。", "フォルダ追加", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrEmpty(this.フォルダ作成TextBox.Text))
            {
                MessageBox.Show("作成するフォルダ名を入力して下さい。", "フォルダ追加", MessageBoxButton.OK);
                return;
            }

            if (this.フォルダ作成TextBox.Text.Contains("@"))
            {
                MessageBox.Show("作成するフォルダ名に「@」を含める事は出来ません。", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.フォルダ作成TextBox.Text.Contains("\\"))
            {
                MessageBox.Show("作成するフォルダ名に「\\」を含める事は出来ません。", "確認", MessageBoxButton.OK);
                return;
            }



            if (!sitem.IsDirectory)
            {
                MessageBox.Show("フォルダ名を選択して下さい。", "フォルダ追加", MessageBoxButton.OK);
                return;
            }




            if (MessageBox.Show(string.Format("「{0}」フォルダを追加します。\r\n  宜しいですか？", this.フォルダ作成TextBox.Text), "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {

                FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();

                client.FileWriter実行Completed += new EventHandler<FileWriter実行CompletedEventArgs>(client_FileWriter実行Completed);

                client.FileWriter実行Async(this.選択アイテムTextBlock.Text, ディレクトリ操作モード.Create, タイプ.ディレクトリ, this.フォルダ作成TextBox.Text);
            }
            else
            {
                return;
            }


        }

        void client_FileWriter実行Completed(object sender, FileWriter実行CompletedEventArgs e)
        {
            try
            {


                if (e == null)
                {
                    MessageBox.Show("操作中にエラーが発生しました。", "エラー", MessageBoxButton.OK);
                }

                MessageBox.Show(e.Result, "結果", MessageBoxButton.OK);

                DoCreate資料TreeViewItemSource();


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

        private void 削除Button_Click(object sender, RoutedEventArgs e)
        {
            var sitem = this.資料tv.SelectedItem as TreeViewItemData;

            if (sitem == null || string.IsNullOrEmpty(this.選択アイテムTextBlock.Text))
            {
                MessageBox.Show("左より削除するアイテムを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.選択アイテムTextBlock.Text.Equals("\\"))
            {
                MessageBox.Show("Rootフォルダは削除できません。", "注意", MessageBoxButton.OK);
                return;
            }

            string[] sepa = this.選択アイテムTextBlock.Text.Split('\\');
            if (MessageBox.Show(string.Format("「{0}」を削除します。\r\n  宜しいですか？", sepa[sepa.GetLength(0) - 1]), "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();

                client.FileWriter実行Completed += new EventHandler<FileWriter実行CompletedEventArgs>(client_FileWriter実行Completed);

                if (sitem.IsDirectory)
                {
                    client.FileWriter実行Async(this.選択アイテムTextBlock.Text, ディレクトリ操作モード.Delete, タイプ.ディレクトリ, null);
                }
                else
                {
                    client.FileWriter実行Async(this.選択アイテムTextBlock.Text, ディレクトリ操作モード.Delete, タイプ.ファイル, null);
                }

                this.選択アイテムTextBlock.Text = "";
            }
            else
            {
                return;
            }


        }


        private FileStream fs;
        private void 保存Button_Click(object sender, RoutedEventArgs e)
        {

            var tv = this.資料tv.SelectedItem as TreeViewItemData;
            if (tv == null || string.IsNullOrEmpty(tv.Name) || tv.IsDirectory == true)
            {
                MessageBox.Show("ファイルを選択して下さい。", "ファイル保存", MessageBoxButton.OK);
                return;
            }




            string filter = "";
            string defaultext = "";
            if (tv.Name.Contains("."))
            {
                string[] sepa = tv.Name.Split('.');
                string 拡張子 = sepa[sepa.GetLength(0) - 1];
                filter = string.Format("拡張子(*.{0})|*.{0}", 拡張子);
                defaultext = 拡張子;
            }
            else
            {
                filter = "All files(*.*)|*.*";
            }

            string[] sepa2 = this.選択アイテムTextBlock.Text.Split('\\');

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Filter = filter,
                    DefaultExt = defaultext
                };
                if (saveFileDialog.ShowDialog() != true)
                {
                    return;
                }



                fs = (FileStream)saveFileDialog.OpenFile();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }


            if (MessageBox.Show(string.Format("「{0}」を保存します。\r\n  宜しいですか？", sepa2[sepa2.GetLength(0) - 1]), "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                GetDownloadFile(this.選択アイテムTextBlock.Text);
            }
            else
            {
                return;
            }

        }

        private void 修正Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(this.変更名称ItemTextBox.Text))
            {
                MessageBox.Show("修正名を入力して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.変更名称ItemTextBox.Text.Contains("@"))
            {
                MessageBox.Show("修正後アイテム名に「@」を使用出来ません。", "確認", MessageBoxButton.OK);
                return;
            }


            if (this.変更名称ItemTextBox.Text.Contains("\\"))
            {
                MessageBox.Show("修正後アイテム名に「\\」を使用出来ません。", "確認", MessageBoxButton.OK);
                return;
            }


            var sitem = this.資料tv.SelectedItem as TreeViewItemData;

            if (sitem == null || string.IsNullOrEmpty(this.選択アイテムTextBlock.Text))
            {
                MessageBox.Show("左より修正するアイテムを選択して下さい。", "確認", MessageBoxButton.OK);
                return;
            }

            if (this.選択アイテムTextBlock.Text.Equals("\\"))
            {
                MessageBox.Show("Rootフォルダは名称を変更できません。", "確認", MessageBoxButton.OK);
                return;
            }


            string[] sepa = this.選択アイテムTextBlock.Text.Split('\\');
            if (MessageBox.Show(string.Format("{0}を{1}へ名称を変更します。\r\n  宜しいですか？", sepa[sepa.GetLength(0) - 1], this.変更名称ItemTextBox.Text), "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                FileWriterClient client = Util.ServiceUtil.ReferenceCreater.GetFileWriterClient();

                client.FileWriter実行Completed += new EventHandler<FileWriter実行CompletedEventArgs>(client_FileWriter実行Completed);

                if (sitem.IsDirectory)
                {
                    client.FileWriter実行Async(this.選択アイテムTextBlock.Text, ディレクトリ操作モード.Rename, タイプ.ディレクトリ, this.変更名称ItemTextBox.Text);
                }
                else
                {
                    client.FileWriter実行Async(this.選択アイテムTextBlock.Text, ディレクトリ操作モード.Rename, タイプ.ファイル, this.変更名称ItemTextBox.Text);
                }
            }
            else
            {
                return;
            }


        }


        public void GetDownloadFile(string filepath)
        {

            string relativepath = System.IO.Path.Combine("資料", filepath).Replace('\\', '/');
            string url = relativepath;
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            webClient.OpenReadAsync(new Uri(url, UriKind.Relative));

        }


        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {


                    DoSave(e.Result, fs);


                }
                else
                {
                    MessageBox.Show(e.Error.Message + e.Error.StackTrace, "ファイルダウンロード", MessageBoxButton.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            finally
            {
                e.Result.Close();
                fs.Close();
            }
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);

            }

            fs.Flush();

        }



        //private void SaveFile(Stream stream, FileStream fs)
        //{
        //    byte[] buffer = new byte[4096];
        //    int bytesRead;

        //    long streamLength = stream.Length;

        //    int totalRead = 0;
        //    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0 || streamLength <= totalRead + bytesRead)
        //    {
        //        totalRead += bytesRead;
        //        try
        //        {
        //            for (int i = 0; i < buffer.Length; i++)
        //            {
        //                fs.WriteByte(buffer[i]);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.Message + e.StackTrace);
        //            return;
        //        }
        //    }

        //    fs.Flush();

        //}


        private void DoSave(Stream input, FileStream fs)
        {
            SaveFile(input, fs);
        }

        private void ThreadSleep(int miliseconds)
        {
            Thread.Sleep(miliseconds);
        }


        #region UploadLogic


        private long fileLength;
        public long FileLength
        {
            get { return fileLength; }
            set
            {
                fileLength = value;
            }
        }


        private long bytesUploaded;
        public long BytesUploaded
        {
            get { return bytesUploaded; }
            set
            {
                bytesUploaded = value;
            }
        }

        public enum FileUploadStatus
        {
            Pending,
            Uploading,
            Complete,
            Error,
            Canceled,
            Removed,
            Resizing
        }

        private FileUploadStatus status;
        public FileUploadStatus Status
        {
            get { return status; }
            set
            {
                status = value;

            }
        }

        public Uri UploadUrl { get; set; }
        private FileInfo file;
        public FileInfo File
        {
            get { return file; }
            set
            {
                file = value;
                Stream temp = file.OpenRead();
                FileLength = temp.Length;
                temp.Close();
            }
        }


        public long ChunkSize = 4194304;
        private MemoryStream resizeStream;


        private string _UploadingFileName;

        public string UploadingFileName
        {
            get { return _UploadingFileName; }
            set { _UploadingFileName = value; }
        }

        private void CheckFileOnServer()
        {
            // 選択されたフォルダがRootならば、ファイル名にする。
            if (this.選択アイテムTextBlock.Text.Equals("\\"))
            {
                // \を@に置換
                UploadingFileName = アップロードファイルTextBox.Text;
            }
            else
            {

                // \を@に置換
                UploadingFileName = this.選択アイテムTextBlock.Text.Replace("\\", "@") + "@" + アップロードファイルTextBox.Text;
            }

            string uri = Core.共通.Settings.GenericHandlerPath;
            UriBuilder ub = new UriBuilder(uri);
            ub.Query = string.Format("{1}filename={0}&GetBytes=true&Type={2}", UploadingFileName, string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&", "資料");
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(ub.Uri);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            long lengthtemp = 0;
            if (!string.IsNullOrEmpty(e.Result))
            {
                lengthtemp = long.Parse(e.Result);
            }

            if (lengthtemp > 0)
            {
                MessageBoxResult result;
                if (lengthtemp == FileLength)
                {
                    result = MessageBox.Show("同名ファイルが存在します。上書きしますか？", "上書き確認？", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                        lengthtemp = 0;
                    else
                    {

                        BytesUploaded = FileLength;
                        Status = FileUploadStatus.Complete;
                        return;
                    }
                }
                else
                {
                    result = MessageBox.Show("同名ファイルが存在します。アップロードを続けますか？", "アップロード継続確認", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.Cancel)
                        lengthtemp = 0;
                }
            }

            UploadFileEx();
        }


        public void UploadFileEx()
        {
            Status = FileUploadStatus.Uploading;
            long temp = FileLength - BytesUploaded;

            UriBuilder ub = new UriBuilder(Core.共通.Settings.GenericHandlerPath);
            bool complete = temp <= ChunkSize;
            ub.Query = string.Format("{3}filename={0}&StartByte={1}&Complete={2}&Type={4}", UploadingFileName, BytesUploaded, complete, string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&", "資料");

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(ub.Uri);
            webrequest.Method = "POST";
            webrequest.BeginGetRequestStream(new AsyncCallback(WriteCallback), webrequest);
        }



        private void WriteCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the operation.
            Stream requestStream = webrequest.EndGetRequestStream(asynchronousResult);

            byte[] buffer = new Byte[4096];
            int bytesRead = 0;
            int tempTotal = 0;

            Stream fileStream = resizeStream != null ? (Stream)resizeStream : File.OpenRead();

            fileStream.Position = BytesUploaded;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0 && tempTotal + bytesRead < ChunkSize && !cancel)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();
                BytesUploaded += bytesRead;
                tempTotal += bytesRead;



                this.Dispatcher.BeginInvoke(delegate()
                {
                    int percent = (int)(((double)BytesUploaded / (double)FileLength) * 100);
                    this.pgUpload.Value = percent;
                    this.tbPercent.Text = percent.ToString() + "%";
                });



            }

            if (resizeStream == null)
                fileStream.Close();
            requestStream.Close();
            webrequest.BeginGetResponse(new AsyncCallback(ReadCallback), webrequest);

        }



        private bool cancel;
        private bool remove;


        public void CancelUpload()
        {
            cancel = true;
        }

        public void RemoveUpload()
        {
            cancel = true;
            remove = true;
            if (Status != FileUploadStatus.Uploading)
                Status = FileUploadStatus.Removed;
        }



        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)webrequest.EndGetResponse(asynchronousResult);
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string responsestring = reader.ReadToEnd();
            reader.Close();

            if (cancel)
            {
                if (resizeStream != null)
                    resizeStream.Close();
                if (remove)
                    Status = FileUploadStatus.Removed;
                else
                    Status = FileUploadStatus.Canceled;
            }
            else if (BytesUploaded < FileLength)
                UploadFileEx();
            else
            {
                if (resizeStream != null)
                    resizeStream.Close();

                Status = FileUploadStatus.Complete;

                this.Dispatcher.BeginInvoke(delegate()
                {
                    // 資料TreeViewのItemSourceを再取得
                    DoCreate資料TreeViewItemSource();
                    BytesUploaded = 0L;

                    //// プログレスバーをリセットし非表示  この対応はせずタブを押したら資料庫が更新される仕様に変更。
                    //this.gdProgress.Visibility = Visibility.Collapsed;
                    //this.pgUpload.Value = 0d;
                    //this.tbPercent.Text = "0%";

                });

            }

        }

        #endregion


    }
}