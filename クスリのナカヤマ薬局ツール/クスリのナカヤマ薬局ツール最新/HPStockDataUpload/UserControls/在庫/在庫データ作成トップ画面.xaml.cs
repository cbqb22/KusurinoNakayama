using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using System.Net;
using クスリのナカヤマ薬局ツール.共通;

namespace クスリのナカヤマ薬局ツール.UserControls.在庫
{
    /// <summary>
    /// 在庫データ作成トップ画面.xaml の相互作用ロジック
    /// </summary>
    public partial class 在庫データ作成トップ画面 : UserControl
    {
        bool 現在庫データ出力するか;
        bool 使用量データ出力するか;
        bool 不動品データ出力するか;
        ObservableCollection<string> 更新年List = new ObservableCollection<string>();
        ObservableCollection<string> 更新月List = new ObservableCollection<string>();
        string DestinationPath = "";


        public 在庫データ作成トップ画面()
        {
            InitializeComponent();

            SetInit();


        }

        public void SetDefaultComboBoxItem()
        {

            Create更新年List();
            int NowYear = System.DateTime.Now.Year;
            this.cmbYear.SelectedIndex = NowYear - 2010 + 1;

            Create更新月List();
            // システム日付を選択
            int Month = System.DateTime.Now.Month;
            this.cmbMonth.SelectedIndex = Month - 1;

            //// システム日付の一つ前の日付を選択
            //int Month = System.DateTime.Now.Month;
            //if (Month == 1)
            //{
            //    // 昨年の12月を選択
            //    this.cmbYear.SelectedIndex -= 1;
            //    this.cmbMonth.SelectedIndex = 11;
            //}
            //else
            //{
            //    this.cmbMonth.SelectedIndex = Month - 1;
            //}


        }

        private void Create更新年List()
        {
            for (int counter = 1; counter < 53; counter++)
            {

                this.更新年List.Add("更新年: " + (2008 + counter).ToString().ToUpper() + "年");
            }

            this.cmbYear.ItemsSource = this.更新年List;
        }

        private void Create更新月List()
        {
            for (int counter = 1; counter <= 12; counter++)
            {

                this.更新月List.Add("更新月: " + counter.ToString().ToUpper() + "月");
            }

            this.cmbMonth.ItemsSource = this.更新月List;
        }


        private void SetInit()
        {
            Setデフォルト値();
            SetDefaultComboBoxItem();
        }

        private void Setデフォルト値()
        {
            // デフォルトの現在庫出力先フォルダを指定
            tb現在庫ファイル名.Text = クスリのナカヤマ薬局ツール.Model.DI.現在庫ファイルパス;

            // デフォルトの使用量出力先フォルダを指定
            tb使用量2ファイル名.Text = クスリのナカヤマ薬局ツール.Model.DI.使用量ファイルパス;

            // デフォルトの不動品出力先フォルダを指定
            tb不動品ファイル名.Text = クスリのナカヤマ薬局ツール.Model.DI.不動品ファイルパス;

            // デフォルトの出力先フォルダを指定
            tb出力先ディレクトリ名.Text = クスリのナカヤマ薬局ツール.Model.DI.出力先フォルダ名;

            // デフォルトの店舗名を設定
            tb店舗名.Text = クスリのナカヤマ薬局ツール.Model.DI.出力店舗名称;
        }

        //private void Setデフォルト値()
        //{
        //    string アプリケーション実行パス = System.AppDomain.CurrentDomain.BaseDirectory;
        //    string 設定ファイルパス = System.IO.Path.Combine(アプリケーション実行パス, "Settings.ini");

        //    if (!System.IO.File.Exists(設定ファイルパス))
        //    {
        //        return;
        //    }

        //    string デフォルト出力先フォルダ = "";
        //    string 店舗名 = "";
        //    string デフォルト現在庫データファイルパス = "";
        //    string デフォルト使用量データファイルパス = "";
        //    string デフォルト不動品データファイルパス = "";
        //    using (StreamReader sr = new StreamReader(設定ファイルパス, Encoding.GetEncoding(932)))
        //    {
        //        string line = "";

        //        // とりあえず２行目だけ
        //        int 行数counter = 1;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            if (行数counter == 1)
        //            {
        //                デフォルト現在庫データファイルパス = line.Replace("[現在庫データファイル]=", "");
        //            }

        //            if (行数counter == 2)
        //            {
        //                デフォルト使用量データファイルパス = line.Replace("[使用量データファイル]=", "");
        //            }

        //            if (行数counter == 3)
        //            {
        //                デフォルト不動品データファイルパス = line.Replace("[不動品データファイル]=", "");
        //            }

        //            if (行数counter == 4)
        //            {
        //                デフォルト出力先フォルダ = line.Replace("[出力先フォルダ名]=", "");
        //            }

        //            if (行数counter == 5)
        //            {
        //                店舗名 = line.Replace("[出力店舗名称]=", "");
        //            }

        //            行数counter++;
        //        }
        //    }

        //    // デフォルトの店舗名を設定
        //    if (店舗名.Equals(""))
        //    {
        //        tb店舗名.Text = "";
        //    }
        //    else
        //    {
        //        tb店舗名.Text = 店舗名;
        //    }

        //    // デフォルトの出力先フォルダを指定
        //    // ディスクトップの専用フォルダのパスを指定
        //    if (デフォルト出力先フォルダ.Equals(""))
        //    {
        //        string Tempデフォルト出力先 = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), System.IO.Path.Combine("在庫管理データ", System.DateTime.Now.ToString("yyyy.MM.dd")));

        //        tb出力先ディレクトリ名.Text = Tempデフォルト出力先;
        //    }
        //    else
        //    {
        //        tb出力先ディレクトリ名.Text = デフォルト出力先フォルダ;
        //    }

        //    // デフォルトの現在庫出力先フォルダを指定
        //    if (デフォルト現在庫データファイルパス.Equals(""))
        //    {
        //        tb現在庫ファイル名.Text = "";
        //    }
        //    else
        //    {
        //        tb現在庫ファイル名.Text = デフォルト現在庫データファイルパス;
        //    }

        //    // デフォルトの使用量出力先フォルダを指定
        //    if (デフォルト使用量データファイルパス.Equals(""))
        //    {
        //        tb使用量ファイル名.Text = "";
        //    }
        //    else
        //    {
        //        tb使用量ファイル名.Text = デフォルト使用量データファイルパス;
        //    }

        //    // デフォルトの不動品出力先フォルダを指定
        //    if (デフォルト不動品データファイルパス.Equals(""))
        //    {
        //        tb不動品ファイル名.Text = "";
        //    }
        //    else
        //    {
        //        tb不動品ファイル名.Text = デフォルト不動品データファイルパス;
        //    }


        //}


        private void bt現在庫参照_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if ((bool)ofd.ShowDialog() == false)
            {
                return;
            }

            tb現在庫ファイル名.Text = ofd.FileName;
        }

        private void bt使用量参照_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if ((bool)ofd.ShowDialog() == false)
            {
                return;
            }

            tb使用量2ファイル名.Text = ofd.FileName;

        }

        private void bt不動品参照_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if ((bool)ofd.ShowDialog() == false)
            {
                return;
            }

            tb不動品ファイル名.Text = ofd.FileName;

        }

        private void bt出力先参照_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb出力先ディレクトリ名.Text = fbd.SelectedPath;
            }
        }


        private void Doアップロード開始(bool 現在庫出力成功, bool 使用量出力成功, bool 不動品出力成功)
        {

            try
            {
                if (string.IsNullOrEmpty(DestinationPath))
                {
                    MessageBox.Show("出力先フォルダ名が入力されていません。\r\nアップロードを中止しました。");
                    return;
                }

                // マージ結果の出力用のカウンター
                if (現在庫出力成功)
                {
                    MergeCount++;
                }


                if (使用量出力成功)
                {
                    MergeCount++;
                }

                if (不動品出力成功)
                {
                    MergeCount++;
                }


                if (現在庫出力成功)
                {

                    string 店名ファイル名 = tb店舗名.Text + "/" + Settings.現在庫出力ファイル名;
                    string uri = クスリのナカヤマ薬局ツール.共通.Settings.Ftp現在庫Path + "/" + 店名ファイル名;
                    string myFile = System.IO.Path.Combine(DestinationPath, Settings.現在庫出力ファイル名);

                    WebRequest req = WebRequest.Create(uri);
                    NetworkCredential nc = new NetworkCredential(Settings.FtpID, Settings.FtpCredent);
                    req.Credentials = nc;
                    req.Method = WebRequestMethods.Ftp.UploadFile;

                    // TODO:新年ディレクトリ作成されているか Ver1.21確認
                    // 店舗名ディレクトリがあるか。なければ作成
                    bool 店舗名ディレクトリ作成成功 = dirExistsOrCreate(nc, クスリのナカヤマ薬局ツール.共通.Settings.Ftp現在庫Path + "/" + tb店舗名.Text + "/");


                    // ディレクトリが成功時にファイルを作成
                    if (店舗名ディレクトリ作成成功)
                    {
                        using (Stream st = req.GetRequestStream())
                        using (FileStream fs = new FileStream(myFile, FileMode.Open))
                        {
                            Byte[] buf = new Byte[1024];
                            int count = 0;

                            do
                            {
                                count = fs.Read(buf, 0, buf.Length);
                                st.Write(buf, 0, count);
                            } while (count != 0);
                        }

                        MessageBox.Show(string.Format("{0}をアップロードしました。", Settings.現在庫出力ファイル名));

                        DoMerge("現在庫");

                    }
                    else
                    {
                        MessageBox.Show(string.Format("FTPサーバー{0}にを出力出来ませんでした。", Settings.現在庫出力ファイル名));
                    }
                }

                if (使用量出力成功)
                {
                    string 店名ファイル名 = tb店舗名.Text + "/" + cmbYear.SelectedValue.ToString().Replace("更新年: ", "") + "/" + cmbMonth.SelectedValue.ToString().Replace("更新月: ", "") + ".csv";
                    string uri = クスリのナカヤマ薬局ツール.共通.Settings.Ftp使用量2Path + "/" + 店名ファイル名;
                    string myFile = System.IO.Path.Combine(DestinationPath, string.Format("使用量2データ_{0}{1}.csv", cmbYear.SelectedValue.ToString().Replace("更新年: ", "").Replace("年", ""), cmbMonth.SelectedValue.ToString().Replace("更新月: ", "").Replace("月", "")));
                    //string myFile = System.IO.Path.Combine(DestinationPath, string.Format("使用量データ_{0}{1}.csv", cmbYear.SelectedValue.ToString().Replace("更新年: ", "").Replace("年", ""), cmbMonth.SelectedValue.ToString().Replace("更新月: ", "").Replace("月", "")));

                    WebRequest req = WebRequest.Create(uri);
                    NetworkCredential nc = new NetworkCredential(Settings.FtpID, Settings.FtpCredent);
                    req.Credentials = nc;
                    req.Method = WebRequestMethods.Ftp.UploadFile;


                    // 店舗名ディレクトリがあるか。なければ作成
                    // ※※※※ティレクトリは最後に"/"がないと、エラーにならない。※※※※
                    bool 店舗名ディレクトリ作成成功 = dirExistsOrCreate(nc, クスリのナカヤマ薬局ツール.共通.Settings.Ftp使用量2Path + "/" + tb店舗名.Text + "/");
                    bool 更新年ディレクトリ作成成功 = false;
                    if (店舗名ディレクトリ作成成功)
                    {
                        // 更新年ディレクトリがあるか。なければ作成
                        更新年ディレクトリ作成成功 = dirExistsOrCreate(nc, クスリのナカヤマ薬局ツール.共通.Settings.Ftp使用量2Path + "/" + tb店舗名.Text + "/" + cmbYear.SelectedValue.ToString().Replace("更新年: ", "") + "/");
                    }

                    // ディレクトリが成功時にファイルを作成
                    if (店舗名ディレクトリ作成成功 && 更新年ディレクトリ作成成功)
                    {
                        // ディレクトリ作成完了したら、ファイルをアップロード
                        using (Stream st = req.GetRequestStream())
                        using (FileStream fs = new FileStream(myFile, FileMode.Open))
                        {
                            Byte[] buf = new Byte[1024];
                            int count = 0;

                            do
                            {
                                count = fs.Read(buf, 0, buf.Length);
                                st.Write(buf, 0, count);
                            } while (count != 0);
                        }

                        MessageBox.Show(string.Format("{0}をアップロードしました。", Settings.使用量2出力ファイル名));

                        DoMerge("使用量2");

                    }
                    else
                    {
                        MessageBox.Show(string.Format("FTPサーバーに{0}を出力出来ませんでした。", Settings.使用量2出力ファイル名));

                    }
                }

                if (不動品出力成功)
                {
                    string 店名ファイル名 = tb店舗名.Text + "/" + Settings.不動品出力ファイル名;
                    string uri = Settings.Ftp不動品Path + "/" + 店名ファイル名;
                    string myFile = System.IO.Path.Combine(DestinationPath, Settings.不動品出力ファイル名);

                    WebRequest req = WebRequest.Create(uri);
                    NetworkCredential nc = new NetworkCredential(Settings.FtpID, Settings.FtpCredent);
                    req.Credentials = nc;
                    req.Method = WebRequestMethods.Ftp.UploadFile;


                    // 店舗名ディレクトリがあるか。なければ作成
                    bool 店舗名ディレクトリ作成成功 = dirExistsOrCreate(nc, クスリのナカヤマ薬局ツール.共通.Settings.Ftp不動品Path + "/" + tb店舗名.Text + "/");

                    // ディレクトリの確認および作成
                    if (店舗名ディレクトリ作成成功)
                    {
                        using (Stream st = req.GetRequestStream())
                        using (FileStream fs = new FileStream(myFile, FileMode.Open))
                        {
                            Byte[] buf = new Byte[1024];
                            int count = 0;

                            do
                            {
                                count = fs.Read(buf, 0, buf.Length);
                                st.Write(buf, 0, count);
                            } while (count != 0);
                        }

                        MessageBox.Show(string.Format("{0}をアップロードしました。", Settings.不動品出力ファイル名));
                        DoMerge("不動品");
                    }
                    else
                    {
                        MessageBox.Show(string.Format("FTPサーバーに{0}を出力出来ませんでした。", Settings.不動品出力ファイル名));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ｴﾗｰ発生" + ex.Message + ex.StackTrace);
            }
        }

        private bool チェックファイル名()
        {
            string ファイル名 = "";

            現在庫データ出力するか = (bool)chk現在庫.IsChecked == false || string.IsNullOrEmpty(tb現在庫ファイル名.Text) ? false : true;
            使用量データ出力するか = (bool)chk使用量2.IsChecked == false || string.IsNullOrEmpty(tb使用量2ファイル名.Text) ? false : true;
            不動品データ出力するか = (bool)chk不動品.IsChecked == false || string.IsNullOrEmpty(tb不動品ファイル名.Text) ? false : true;

            if (現在庫データ出力するか)
            {
                ファイル名 = tb現在庫ファイル名.Text.Split('\\')[tb現在庫ファイル名.Text.Split('\\').Length - 1];
                if (string.IsNullOrEmpty(ファイル名) || (!ファイル名.Equals("在庫管理表.CSV") && !ファイル名.Equals("在庫管理表.csv")))
                {
                    MessageBox.Show("選択した現在庫ファイル名が【在庫管理表.CSV】でありません。\r\n現在庫データファイル名は【在庫管理表.CSV】として下さい。", "エラー", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }

            //if (使用量データ出力するか)
            //{
            //    ファイル名 = tb使用量ファイル名.Text.Split('\\')[tb使用量ファイル名.Text.Split('\\').Length - 1];
            //    if (string.IsNullOrEmpty(ファイル名) || (!ファイル名.Equals("在庫使用量.CSV") && !ファイル名.Equals("在庫使用量.csv")))
            //    {
            //        MessageBox.Show("選択した使用量ファイル名が【在庫使用量.CSV】ではありません。\r\n使用量データファイル名は【在庫使用量.CSV】として下さい。", "エラー", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //        return false;
            //    }
            //}


            if (使用量データ出力するか)
            {
                ファイル名 = tb使用量2ファイル名.Text.Split('\\')[tb使用量2ファイル名.Text.Split('\\').Length - 1];
                if (string.IsNullOrEmpty(ファイル名) || (!ファイル名.Equals("使用量.CSV") && !ファイル名.Equals("使用量.csv")))
                {
                    MessageBox.Show("選択した使用量ファイル名が【使用量.CSV】ではありません。\r\n使用量データファイル名は【使用量.CSV】として下さい。", "エラー", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }

            if (不動品データ出力するか)
            {
                ファイル名 = tb不動品ファイル名.Text.Split('\\')[tb不動品ファイル名.Text.Split('\\').Length - 1];
                if (string.IsNullOrEmpty(ファイル名) || (!ファイル名.Equals("デッド品.CSV") && !ファイル名.Equals("デッド品.csv")))
                {
                    MessageBox.Show("選択した不動品データファイル名が【デッド品.CSV】でありません。\r\n不動品データファイル名は【デッド品.CSV】として下さい。", "エラー", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }

            return true;


        }

        private bool[] Doデータ作成開始()
        {

            現在庫データ出力するか = (bool)chk現在庫.IsChecked == false || string.IsNullOrEmpty(tb現在庫ファイル名.Text) ? false : true;
            使用量データ出力するか = (bool)chk使用量2.IsChecked == false || string.IsNullOrEmpty(tb使用量2ファイル名.Text) ? false : true;
            不動品データ出力するか = (bool)chk不動品.IsChecked == false || string.IsNullOrEmpty(tb不動品ファイル名.Text) ? false : true;

            if (!現在庫データ出力するか &&
                !使用量データ出力するか &&
                !不動品データ出力するか)
            {

                MessageBox.Show("１つ以上の作業を選択して下さい。", "確認", MessageBoxButton.OK);
                return new bool[] { false, false, false };
            }

            string folderName = System.DateTime.Now.ToString("yyyy.MM.dd");
            DestinationPath = System.IO.Path.Combine(this.tb出力先ディレクトリ名.Text, folderName);

            bool 現在庫出力成功 = false;
            bool 使用量2出力成功 = false;
            bool 不動品出力成功 = false;

            if (現在庫データ出力するか)
            {
                現在庫出力成功 = Do現在庫データ作成Start(tb現在庫ファイル名.Text, tb店舗名.Text, DestinationPath);
            }

            if (使用量データ出力するか)
            {
                使用量2出力成功 = Do使用量データ作成Start2(tb使用量2ファイル名.Text, tb店舗名.Text, cmbYear.SelectedItem.ToString().Replace("更新年: ", "").Replace("年", ""), cmbMonth.SelectedItem.ToString().Replace("更新月: ", "").Replace("月", ""), DestinationPath);
            }

            if (不動品データ出力するか)
            {
                不動品出力成功 = Do不動品データ作成Start(tb不動品ファイル名.Text, tb店舗名.Text, DestinationPath);
            }


            return new bool[] { 現在庫出力成功, 使用量2出力成功, 不動品出力成功 };




        }


        private void bt開始_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null)
            {
                return;
            }

            if (!DoStartCheck())
            {
                return;
            }


            if (チェックファイル名() == false)
            {
                return;
            }

            Doデータ作成開始();

        }


        /// <summary>
        /// FTPサーバーにディレクトリが存在するか確認
        /// ※※※※ティレクトリは最後に"/"がないと、req.GetResponse()でエラーにならない。※※※※
        /// </summary>
        /// <param name="cred"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private bool dirExistsOrCreate(NetworkCredential cred, string uri)
        {
            WebRequest req = WebRequest.Create(uri);
            req.Proxy = null; // 接続の高速化
            req.Credentials = cred;
            req.Method = WebRequestMethods.Ftp.ListDirectory; ;

            WebResponse res = null;
            try
            {
                res = req.GetResponse();

            }
            catch (WebException e)
            {

                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    FtpWebResponse r = (FtpWebResponse)e.Response;
                    if (r.StatusCode ==
                            FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        // ディレクトリを作成
                        //FtpWebRequestの作成
                        FtpWebRequest ftpReq = (System.Net.FtpWebRequest)WebRequest.Create(uri);
                        //ログインユーザー名とパスワードを設定
                        ftpReq.Credentials = cred;
                        //MethodにWebRequestMethods.Ftp.MakeDirectoryを設定
                        ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory;
                        FtpWebResponse ftpRes = null;
                        try
                        {
                            //FtpWebResponseを取得
                            ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                        }
                        catch
                        {
                            return false;
                        }
                        finally
                        {
                            ftpRes.Close();
                        }
                    }
                }
                else
                {
                    throw; // ファイル関連以外の例外は再スロー
                }
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return true;
        }


        private void btキャンセル_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();

        }


        private bool Do現在庫データ作成Start(string FilePath, string 出力店舗名, string destinationPath)
        {
            try
            {
                if (!System.IO.File.Exists(FilePath))
                {
                    MessageBox.Show("現在庫データのファイルが存在しない為、データを作成できませんでした。");
                    return false;
                }

                if (!System.IO.Directory.Exists(destinationPath))
                {
                    System.IO.Directory.CreateDirectory(destinationPath);
                }

                string 出力先パス = System.IO.Path.Combine(DestinationPath, Settings.現在庫出力ファイル名);


                using (StreamReader sr = new StreamReader(FilePath, Encoding.GetEncoding(932)))
                {
                    using (StreamWriter sw = new StreamWriter(出力先パス, false, Encoding.GetEncoding(932)))
                    {
                        string line = "";
                        int counter = 1;
                        // とりあえず１行目だけ
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] sepa = line.Split(',');

                            // 1～2行目はヘッダー

                            // sepa[0] 店コード 　　 *変換　
                            // sepa[1] 薬品コード
                            // sepa[2] 薬品名
                            // sepa[3] 形態区分     　削除
                            // sepa[4] １包単位量     削除     
                            // sepa[5] 単位           削除
                            // sepa[6] 剤型           削除
                            // sepa[7] 薬価
                            // sepa[8] 最終仕入単価   削除
                            // sepa[9] 期首在庫数     削除
                            // sepa[10]入庫累計       削除
                            // sepa[11]出庫累計       削除
                            // sepa[12]在庫数
                            // sepa[13]在庫補正値     削除
                            // sepa[14]最終仕入元ＣＤ 削除
                            // sepa[15]最終仕入先名   削除
                            // sepa[16]最終入庫日     削除
                            // sepa[17]最終出庫日     削除
                            // sepa[18]使用期限
                            // sepa[19]発注点         削除
                            // sepa[20]最終JAN        削除
                            // sepa[21]包装           削除
                            // sepa[22]メーカー名
                            // sepa[23]通常仕入先ＣＤ 削除
                            // sepa[24]通常仕入先名   削除
                            // sepa[25]麻毒区分       削除
                            // sepa[26]後発区分
                            // sepa[27]棚番１
                            // sepa[28]棚番２
                            // sepa[29]棚番３
                            // sepa[30]名称２


                            // 1～2行目はヘッダー情報
                            if (counter == 1)
                            {
                                // １行目はカラムヘッダーなので出力
                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", "店舗名", RemoveDoubleQuotationChar(sepa[1]), RemoveDoubleQuotationChar(sepa[2]), RemoveDoubleQuotationChar(sepa[12]), RemoveDoubleQuotationChar(sepa[18]), RemoveDoubleQuotationChar(sepa[7]), RemoveDoubleQuotationChar(sepa[22]), RemoveDoubleQuotationChar(sepa[26]), RemoveDoubleQuotationChar(sepa[30])));
                            }
                            if (counter == 2)
                            {
                                // ２行目は出力時間なので飛ばす
                            }
                            else
                            {
                                // 在庫数が０以下ならば、飛ばす。
                                double result;
                                if (double.TryParse(RemoveDoubleQuotationChar(sepa[12]), out result) == false)
                                {
                                    counter++;
                                    continue;
                                }
                                if (result <= 0)
                                {
                                    counter++;
                                    continue;
                                }

                                //string 名称 = "";

                                //// 名称２が空白でなければ、名称２を採用する
                                //if (RemoveDoubleQuotationChar(sepa[30]) != "")
                                //{
                                //    名称 = RemoveDoubleQuotationChar(sepa[30]);
                                //}
                                //else
                                //{
                                //    名称 = RemoveDoubleQuotationChar(sepa[2]);
                                //}

                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", 出力店舗名, RemoveDoubleQuotationChar(sepa[1]), RemoveDoubleQuotationChar(sepa[2]), RemoveDoubleQuotationChar(sepa[12]), RemoveDoubleQuotationChar(sepa[18]), RemoveDoubleQuotationChar(sepa[7]), RemoveDoubleQuotationChar(sepa[22]), RemoveDoubleQuotationChar(sepa[26]), RemoveDoubleQuotationChar(sepa[30])));
                            }

                            counter++;

                        }

                        sw.Flush();
                    }

                    MessageBox.Show(string.Format("{0}を作成しました。", Settings.現在庫出力ファイル名));

                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.StackTrace);
                return false;
            }

            return true;


        }

        /// <summary>
        /// ※※こっちはもう使わない※※
        /// 使用量.CSVのダウンロード (=裏画面の在庫使用量.CSV)
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="出力店舗名"></param>
        /// <param name="更新年"></param>
        /// <param name="更新月"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        private bool Do使用量データ作成Start(string FilePath, string 出力店舗名, string 更新年, string 更新月, string destinationPath)
        {
            try
            {

                if (!System.IO.File.Exists(FilePath))
                {
                    MessageBox.Show("使用量データのファイルが存在しない為、データを作成できませんでした。");
                    return false;
                }

                if (!System.IO.Directory.Exists(destinationPath))
                {
                    System.IO.Directory.CreateDirectory(destinationPath);
                }

                string 出力使用年月日 = string.Format("{0}年{1}月", 更新年, 更新月);
                string 出力先パス = System.IO.Path.Combine(DestinationPath, string.Format("使用量データ_{0}{1}.csv", 更新年, 更新月));

                using (StreamReader sr = new StreamReader(FilePath, Encoding.GetEncoding(932)))
                {
                    using (StreamWriter sw = new StreamWriter(出力先パス, false, Encoding.GetEncoding(932)))
                    {

                        string line = "";
                        int counter = 1;

                        // とりあえず１行目だけ
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] sepa = line.Split(',');

                            // 1行目はヘッダー

                            // ONE FAMILY? (丸子使用量)　←こっちが正解？
                            // sepa[0] 店舗ＣＤ　     *変換
                            // sepa[1] 仕入先ＣＤ　　 *変換　
                            // sepa[2] 集計開始日　　　削除
                            // sepa[3] 集計終了日　　　削除
                            // sepa[4] 薬品ＣＤ 　　  *使用 
                            // sepa[5] 商品名 　　　   
                            // sepa[6] 剤型      　　　削除
                            // sepa[7] 使用量         *0 <= 使用量を抽出
                            // sepa[8] 薬価            削除
                            // sepa[9] 薬価金額        削除
                            // sepa[10] 構成比         削除
                            // sepa[11] 購入価         削除
                            // sepa[12] 購入価格       削除
                            // sepa[13] 薬価差         削除
                            // sepa[14] 薬価差額       削除


                            // PWFUL? (遊園使用量)　←なんかデータがおかしい。仕入先データがない。
                            // sepa[0] 医薬品コード　 *削除
                            // sepa[1] 医薬品名
                            // sepa[2] 薬価
                            // sepa[3] 使用量
                            // sepa[4] 後発品区分     削除
                            // sepa[5] 計量混合区分   削除
                            // sepa[6] 代替区分       削除






                            // 1行目はカラムヘッダーのため、そのまま出力
                            if (counter == 1)
                            {
                                // ToDo 仕入先はいらない？
                                // カラムヘッダー出力
                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "店舗名", "使用年月日", RemoveDoubleQuotationChar(sepa[5]), RemoveDoubleQuotationChar(sepa[7]), RemoveDoubleQuotationChar(sepa[8]), "薬品コード"));

                            }
                            else
                            {
                                // 使用量で０以下ならば、飛ばす。
                                double result;
                                if (double.TryParse(RemoveDoubleQuotationChar(sepa[7]), out result) == false)
                                {
                                    counter++;
                                    continue;
                                }
                                if (result <= 0)
                                {
                                    counter++;
                                    continue;
                                }

                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", 出力店舗名, 出力使用年月日, RemoveDoubleQuotationChar(sepa[5]), RemoveDoubleQuotationChar(result.ToString()), RemoveDoubleQuotationChar(sepa[8]), RemoveDoubleQuotationChar(sepa[4])));
                            }

                            counter++;

                        }

                        sw.Flush();
                    }

                    MessageBox.Show(string.Format("{0}を作成しました。", Settings.使用量2出力ファイル名));

                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.StackTrace);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 使用量2.CSVのダウンロード (=表画面の使用量.CSV)
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="出力店舗名"></param>
        /// <param name="更新年"></param>
        /// <param name="更新月"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        private bool Do使用量データ作成Start2(string FilePath, string 出力店舗名, string 更新年, string 更新月, string destinationPath)
        {
            try
            {

                if (!System.IO.File.Exists(FilePath))
                {
                    MessageBox.Show("使用量2データのファイルが存在しない為、データを作成できませんでした。");
                    return false;
                }

                if (!System.IO.Directory.Exists(destinationPath))
                {
                    System.IO.Directory.CreateDirectory(destinationPath);
                }

                string 出力使用年月日 = string.Format("{0}年{1}月", 更新年, 更新月);
                string 出力先パス = System.IO.Path.Combine(DestinationPath, string.Format("使用量2データ_{0}{1}.csv", 更新年, 更新月));

                using (StreamReader sr = new StreamReader(FilePath, Encoding.GetEncoding(932)))
                {
                    using (StreamWriter sw = new StreamWriter(出力先パス, false, Encoding.GetEncoding(932)))
                    {

                        string line = "";
                        int counter = 1;

                        // とりあえず１行目だけ
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] sepa = line.Split(',');

                            // sepa[0] 医薬品コード　 
                            // sepa[1] 商品コード　　 　
                            // sepa[2] 医薬品名　　　
                            // sepa[3] 薬価　　　
                            // sepa[4] 使用量 　　   0 <= 使用量を抽出
                            // sepa[5] 後発品区分 　　　   
                            // sepa[6] 計量混合加算 
                            // sepa[7] 代替区分         
                            // sepa[8] 名称２           
                            // sepa[9] 後発体制      


                            // 1行目はカラムヘッダーのため、そのまま出力
                            if (counter == 1)
                            {
                                // ToDo 仕入先はいらない？
                                // カラムヘッダー出力
                                // 出力形式
                                // [0] 店舗名
                                // [1] 使用年月日
                                // [2] 商品コード
                                // [3] 医薬品名
                                // [4] 名称２
                                // [5] 使用量
                                // [6] 薬価
                                // [7] 後発品区分
                                // [8] 代替区分
                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", "店舗名", "使用年月日", RemoveDoubleQuotationChar(sepa[1]), RemoveDoubleQuotationChar(sepa[2]), RemoveDoubleQuotationChar(sepa[8]), RemoveDoubleQuotationChar(sepa[4]), RemoveDoubleQuotationChar(sepa[3]), RemoveDoubleQuotationChar(sepa[5]), RemoveDoubleQuotationChar(sepa[7])));

                            }
                            // 2行目は日付などの情報なので飛ばす
                            else if(counter == 2)
                            {
                                counter++;
                                continue;
                            }
                            else
                            {
                                // 使用量で０以下ならば、飛ばす。
                                double result;
                                if (double.TryParse(RemoveDoubleQuotationChar(sepa[4]), out result) == false)
                                {
                                    counter++;
                                    continue;
                                }
                                if (result <= 0)
                                {
                                    counter++;
                                    continue;
                                }

                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", 出力店舗名, 出力使用年月日, RemoveDoubleQuotationChar(sepa[1]), RemoveDoubleQuotationChar(sepa[2]), RemoveDoubleQuotationChar(sepa[8]), result.ToString(), RemoveDoubleQuotationChar(sepa[3]), RemoveDoubleQuotationChar(sepa[5]), RemoveDoubleQuotationChar(sepa[7])));
                            }

                            counter++;

                        }

                        sw.Flush();
                    }

                    MessageBox.Show(string.Format("{0}を作成しました。", Settings.使用量2出力ファイル名));

                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.StackTrace);
                return false;
            }

            return true;
        }

        private bool Do不動品データ作成Start(string FilePath, string 出力店舗名, string destinationPath)
        {
            try
            {

                if (!System.IO.File.Exists(FilePath))
                {
                    MessageBox.Show("不動品データのファイルが存在しない為、データを作成できませんでした。");
                    return false;
                }

                if (!System.IO.Directory.Exists(destinationPath))
                {
                    System.IO.Directory.CreateDirectory(destinationPath);
                }

                string 出力先パス = System.IO.Path.Combine(destinationPath, Settings.不動品出力ファイル名);


                using (StreamReader sr = new StreamReader(FilePath, Encoding.GetEncoding(932)))
                {
                    using (StreamWriter sw = new StreamWriter(出力先パス, false, Encoding.GetEncoding(932)))
                    {

                        string line = "";
                        int counter = 1;
                        // とりあえず１行目だけ
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] sepa = line.Split(',');

                            // 1～2行目はヘッダー

                            // sepa[0] 薬品コード 　　 *変換　
                            // sepa[1] 商品コード     削除(レセコン独自コード)
                            // sepa[2] 薬品名         
                            // sepa[3] 形態区分     　削除
                            // sepa[4] 購入価格ａ     削除     
                            // sepa[5] 薬価ｂ         
                            // sepa[6] 使用量ｃ       削除
                            // sepa[7] ｃ×ａ         削除
                            // sepa[8] 現在庫ｄ       
                            // sepa[9] ｄ×ａ         削除
                            // sepa[10]最終仕入日     削除
                            // sepa[11]最終引落日     削除
                            // sepa[12]使用期限

                            // sepa[13]棚番1          削除
                            // sepa[14]棚番2          削除
                            // sepa[15]棚番3          削除
                            // sepa[16]１包単位量    
                            // sepa[17]名称２

                            				


                            // 1～2行目はヘッダーのため、そのまま出力
                            if (counter == 1)
                            {
                                // １行目は店舗情報等なので、飛ばす
                            }
                            else if (counter == 2)
                            {
                                if (sepa.Count() < 18)
                                {
                                    throw new Exception("デッド品.CSVの列の数が18以下です。レセコンより最新データを出力してから再度操作を実行してください。");
                                }

                                // ２行目はカラムヘッダーの為、そのまま出力
                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                                            "店舗名"
                                                            , RemoveDoubleQuotationChar(sepa[0])
                                                            , RemoveDoubleQuotationChar(sepa[2])
                                                            , RemoveDoubleQuotationChar(sepa[8])
                                                            , RemoveDoubleQuotationChar(sepa[12])
                                                            , RemoveDoubleQuotationChar(sepa[5])
                                                            , RemoveDoubleQuotationChar(sepa[16])
                                                            , RemoveDoubleQuotationChar(sepa[17])
                                                            ));
                            }
                            else
                            {
                                // 在庫数が０以下ならば、飛ばす。
                                double result;
                                if (double.TryParse(RemoveDoubleQuotationChar(sepa[8]), out result) == false)
                                {
                                    string s = RemoveDoubleQuotationChar(sepa[8]);
                                    counter++;
                                    continue;
                                }
                                if (result <= 0)
                                {
                                    counter++;
                                    continue;
                                }

                                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                                                            , 出力店舗名
                                                            , RemoveDoubleQuotationChar(sepa[0])
                                                            , RemoveDoubleQuotationChar(sepa[2])
                                                            , RemoveDoubleQuotationChar(sepa[8])
                                                            , RemoveDoubleQuotationChar(sepa[12])
                                                            , RemoveDoubleQuotationChar(sepa[5])
                                                            , RemoveDoubleQuotationChar(sepa[16])
                                                            , RemoveDoubleQuotationChar(sepa[17])
                                                            
                                                            ));
                            }

                            counter++;

                        }

                        sw.Flush();
                    }

                    MessageBox.Show(string.Format("{0}を作成しました。", Settings.不動品出力ファイル名));

                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.StackTrace);
                return false;
            }

            return true;

        }


        string MergeResultMessage = "";
        int MergeCount = 0;
        int FinishMergeCount = 0;


        private void DoMerge(string マージタイプ)
        {
            string uri = Settings.ZaikoGenericHandlerPath;
            UriBuilder ub = new UriBuilder(uri);
            ub.Query = string.Format("Type={0}&Operation={1}", "Merge", マージタイプ);
            //ub.Query = string.Format("{1}filename={0}&GetBytes=true&Type={2}", UploadingFileName, string.IsNullOrEmpty(ub.Query) ? "" : ub.Query.Remove(0, 1) + "&", "在庫関連");
            WebClient client = new WebClient();
            client.Credentials = new System.Net.NetworkCredential(クスリのナカヤマ薬局ツール.共通.Settings.BasicID, クスリのナカヤマ薬局ツール.共通.Settings.BasicCredent);
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(ub.Uri);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (!MergeResultMessage.Equals("一部のCSVファイルのマージができませんでした。"))
                {
                    string message = "";
                    if (e.Result.Equals("genzaiko"))
                    {
                        message = "現在庫";
                    }
                    else if (e.Result.Equals("shiyouryo"))
                    {
                        message = "使用量";
                    }
                    else if (e.Result.Equals("shiyouryo2"))
                    {
                        message = "使用量2";
                    }
                    else if (e.Result.Equals("fudouhinn"))
                    {
                        message = "不動品";
                    }

                    MergeResultMessage += string.Format("{0}データ.csvのマージが完了しました。\r\n", message);
                }
            }
            else
            {
                MergeResultMessage = "一部のCSVファイルのマージができませんでした。";
            }

            FinishMergeCount++;

            if (MergeCount == FinishMergeCount)
            {
                if (MergeResultMessage.Equals("一部のCSVファイルのマージができませんでした。"))
                {
                    MessageBox.Show(MergeResultMessage, "更新結果", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show(MergeResultMessage, "更新結果", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MergeCount = 0;
                FinishMergeCount = 0;
                MergeResultMessage = "";
            }

        }


        private string RemoveDoubleQuotationChar(string str)
        {
            return str.Replace("\"", "");
        }

        private bool DoStartCheck()
        {
            if ((bool)chk現在庫.IsChecked && this.tb現在庫ファイル名.Text.Equals(""))
            {
                MessageBox.Show("現在庫ファイル名を選択して下さい。");
                return false;
            }

            if ((bool)chk使用量2.IsChecked && this.tb使用量2ファイル名.Text.Equals(""))
            {
                MessageBox.Show("使用量2ファイル名を選択して下さい。");
                return false;
            }

            if ((bool)chk不動品.IsChecked && this.tb不動品ファイル名.Text.Equals(""))
            {
                MessageBox.Show("不動品ファイル名を選択して下さい。");
                return false;
            }

            if (tb店舗名.Text.Equals(""))
            {
                MessageBox.Show("出力店舗名を入力して下さい。");
                return false;
            }

            if (tb出力先ディレクトリ名.Text.Equals(""))
            {
                MessageBox.Show("出力先フォルダ名を入力して下さい。");
                return false;
            }

            return true;
        }

        private void bt開始アップロード_Click(object sender, RoutedEventArgs e)
        {

            if (!DoStartCheck())
            {
                return;
            }


            if (チェックファイル名() == false)
            {
                return;
            }

            bool[] 出力結果 = Doデータ作成開始();
            if (出力結果.Length != 3)
            {
                return;
            }

            if (出力結果[0] == false &&
                出力結果[1] == false &&
                出力結果[2] == false)
            {
                return;
            }

            Doアップロード開始(出力結果[0], 出力結果[1], 出力結果[2]);

        }

        private void btSetting_Click(object sender, RoutedEventArgs e)
        {
            Windows.Settings setwin = new Windows.Settings();
            setwin.ShowDialog();

        }

    }
}
