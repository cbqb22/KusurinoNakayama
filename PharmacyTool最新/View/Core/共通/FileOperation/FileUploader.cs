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
using System.IO;
using System.Windows.Threading;

namespace View.Core.共通.FileOperation
{
    public class FileUploader
    {
        public FileUploader(Dispatcher dispatch,FileInfo fileinfo,bool 掲示板資料か,string 記事No,string カテゴリ)
        {
            this.ThreadDispathcer = dispatch;
            this.File = fileinfo;
            this.掲示板資料か = 掲示板資料か;
            this.記事No = 記事No;
            this.カテゴリ = カテゴリ;
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


        private Dispatcher _ThreadDispathcer;

        public Dispatcher ThreadDispathcer
        {
            get { return _ThreadDispathcer; }
            set { _ThreadDispathcer = value; }
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

        private bool _掲示板資料か;

        public bool 掲示板資料か
        {
            get { return _掲示板資料か; }
            set { _掲示板資料か = value; }
        }

        private string _記事No;

        public string 記事No
        {
            get { return _記事No; }
            set { _記事No = value; }
        }

        private string _カテゴリ;

        public string カテゴリ
        {
            get { return _カテゴリ; }
            set { _カテゴリ = value; }
        }

        public void CheckFileOnServer(string DirectoryPath)
        {
            if (掲示板資料か)
            {
                // 記事Noフォルダに格納する
                // \を@に置換
                string comb = System.IO.Path.Combine(DirectoryPath, カテゴリ);
                UploadingFileName = System.IO.Path.Combine(comb,記事No).Replace("\\", "@") + "@" + File.Name;
            }
            else
            {
                // \を@に置換
                UploadingFileName = DirectoryPath.Replace("\\", "@") + "@" + File.Name;
            }


            string uri = Core.共通.Settings.GenericHandlerPath;
            UriBuilder ub = new UriBuilder(uri);
            ub.Query = string.Format("{1}filename={0}&GetBytes=true&Type={2}", UploadingFileName, string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&", "掲示板資料");
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


                ThreadDispathcer.BeginInvoke(delegate()
                {
                    int percent = (int)(((double)BytesUploaded / (double)FileLength) * 100);
                    //this.pgUpload.Value = percent;
                    //this.tbPercent.Text = percent.ToString() + "%";
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

                ThreadDispathcer.BeginInvoke(delegate()
                {
                    BytesUploaded = 0L;
                });

                // 完了メッセージを出す？

            }

        }

        #endregion

    }
}
