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

namespace View.Core.共通.FileOperation
{
    public class FileDownloader
    {
        public FileDownloader(string Filter,string DefaultText,string Filepath ,string category,FileStream Fs,string ファイル名)
        {
            this.filter = Filter;
            this.defaultext = DefaultText;
            this.ファイルパス = Filepath;
            this.カテゴリ = category;
            this.fs = Fs;
            this.ファイル名 = ファイル名;
        }

        string filter = "";
        string defaultext = "";
        string ファイルパス;
        string カテゴリ;
        string ファイル名;
        private FileStream fs;

        public void StartDownload()
        {
            if (MessageBox.Show(string.Format("「{0}」を保存します。\r\n  宜しいですか？", ファイル名), "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                GetDownloadFile(ファイルパス, カテゴリ);
            }
            else
            {
                return;
            }


        }


        public void GetDownloadFile(string filepath,string カテゴリ)
        {

            string relativepath = System.IO.Path.Combine(カテゴリ, filepath).Replace('\\', '/');
            string url = relativepath;

            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);

            //MessageBox.Show(string.Format("カテゴリ:{0}\r\nfilepath:{1}\r\nurl:{2}\r\nfilter:{3}\r\n", カテゴリ, filepath, url, filter));

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

        private void DoSave(Stream input, FileStream fs)
        {
            SaveFile(input, fs);
        }


    }
}
