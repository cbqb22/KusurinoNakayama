using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;

namespace PharmacyTool.Web.GenericHandler
{
    /// <summary>
    /// $codebehindclassname$ の概要の説明です
    /// </summary>
    [WebService(Namespace = "http://GenericHandler/FileUpload.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class 在庫データFileUpload : IHttpHandler
    {

#if DEBUG
        string rootPath = PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPathDEBUG;
#elif NAKAYAMA
        string rootPath = PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPathNAKAYAMA;
#else
        string rootPath = PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPath;
#endif


        string 作成Path = "";


        public void ProcessRequest(HttpContext context)
        {
            // マージを行う
            string タイプ = context.Request.QueryString["Type"];
            string 操作 = context.Request.QueryString["Operation"];
            if (タイプ.Equals("Merge"))
            {

                if (操作.Equals("現在庫"))
                {
                    MergeFiles(操作種別.現在庫);
                    context.Response.Write("genzaiko");
                }
                else if (操作.Equals("使用量"))
                {
                    MergeFiles(操作種別.使用量);
                    context.Response.Write("shiyouryo");
                }
                else if (操作.Equals("使用量2"))
                {
                    MergeFiles(操作種別.使用量2);
                    context.Response.Write("shiyouryo2");
                }
                else if (操作.Equals("不動品"))
                {
                    MergeFiles(操作種別.不動品);
                    context.Response.Write("fudouhinn");
                }
                else
                {
                    return;
                }

            }

        }


        //public void ProcessRequest(HttpContext context)
        //{

        //    string str = context.Request.Url.ToString().Replace('@', '\\');

        //    //[0]はベースアドレス
        //    //[1]は店舗名
        //    //[2]は更新年
        //    //[3]は更新月
        //    //[4]は更新種別
        //    string[] sepa = str.Split('?');

        //    if (sepa[4].Replace("更新種別:", "").Equals("現在庫データ(後発品)"))
        //    {
        //        Update現在庫(sepa[1].Replace("店舗名:", ""));
        //        PreSaveFile(context, 作成Path);
        //        MergeFiles(操作種別.現在庫);
        //    }
        //    else if (sepa[4].Replace("更新種別:", "").Equals("使用量データ"))
        //    {
        //        Update使用量(sepa[1].Replace("店舗名:", ""), sepa[2].Replace("更新年:", ""), sepa[3].Replace("更新月:", ""));
        //        PreSaveFile(context, 作成Path);
        //        MergeFiles(操作種別.使用量);
        //    }
        //    else if (sepa[4].Replace("更新種別:", "").Equals("不動品データ"))
        //    {
        //        Update不動品(sepa[1].Replace("店舗名:", ""));
        //        PreSaveFile(context, 作成Path);
        //        MergeFiles(操作種別.不動品);
        //    }
        //    else
        //    {
        //        //AppendMEDISFile();
        //        //UpdateMEDIS();
        //        //PreSaveFile(context, 作成Path);
        //    }
        //}

        private void AppendMEDISFile()
        {
            string tempMEDISPath = Path.Combine(rootPath, @"MEDIS\temp");

            string[] ar = Directory.GetFiles(tempMEDISPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var str in ar)
            {
                string line = "";
                //using (StreamWriter sw = new StreamWriter(this.作成Path, true, Encoding.GetEncoding("Shift_JIS")))
                using (StreamWriter sw = new StreamWriter(@"C:\PharmacyTools\ClientBin\在庫関連\MEDIS\TEMP2.TXT", true, Encoding.GetEncoding("Shift_JIS")))
                {
                    using (StreamReader sr = new StreamReader(str, Encoding.GetEncoding("Shift_JIS")))
                    {
                        //while ((line = sr.ReadLine()) != null)
                        //{
                        //    if (line.Contains(sw.NewLine))
                        //    {
                        //        sw.WriteLine(line);
                        //    }
                        //    else
                        //    {
                        //        sw.Write(line + sw.NewLine);
                        //    }
                        //}
                        int counter = 1;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (counter == 1)
                            {
                                sw.Write(line);

                            }
                            else
                            {
                                sw.Write(sw.NewLine + line);
                            }
                            counter++;
                        }
                    }
                    sw.Flush();
                }

            }

        }


        private void SaveMEDIS(MemoryStream ms)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(this.作成Path, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    string line = "";
                    using (StreamReader sr = new StreamReader(ms, Encoding.GetEncoding("Shift_JIS")))
                    {
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                sw.WriteLine(line);
                            }

                            sw.Flush();
                        }
                    }
                }
            }
            finally
            {
                ms.Close();
            }
        }

        private void PreSaveFile(HttpContext context, string path)
        {
            int count = 1;
            string 作成Path = Path.Combine(path, count.ToString() + ".txt");
            FileStream fs;
            try
            {
                while (File.Exists(作成Path))
                {
                    count++;
                    作成Path = Path.Combine(path, count.ToString() + ".txt");
                }
                using (fs = File.Create(作成Path))
                {
                    SaveFile(context.Request.InputStream, fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Request.InputStream.Close();
            }

        }

        private void MergeFiles(操作種別 種別)
        {

            string MergeFilesRootPath = "";
            switch (種別)
            {
                case (操作種別.現在庫):
                    MergeFilesRootPath = Path.Combine(rootPath, "現在庫");
                    break;

                case (操作種別.使用量):
                    MergeFilesRootPath = Path.Combine(rootPath, "使用量");
                    break;

                case (操作種別.使用量2):
                    MergeFilesRootPath = Path.Combine(rootPath, "使用量2");
                    break;


                case (操作種別.不動品):
                    MergeFilesRootPath = Path.Combine(rootPath, "不動品");
                    break;

                default:
                    break;

            }

            string totalcsvFilePath = Path.Combine(MergeFilesRootPath, "total.csv");

            // total.csvを新規で作成する
            //File.Create(totalcsvFilePath);

            string[] FilesArray = Directory.GetFiles(MergeFilesRootPath, "*", SearchOption.AllDirectories);


            // 使用量のマージの時は、薬品コード追加の為別個にする
            if (種別 == 操作種別.使用量2)
            {
                using (StreamWriter sw = new StreamWriter(totalcsvFilePath, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    foreach (var file in FilesArray)
                    {
                        string[] sepa = file.Split('.');
                        string 拡張子 = sepa[sepa.Length - 1];

                        // 書き込み先なのでtotal.csvは飛ばす
                        if (拡張子.ToLower().Equals("csv") && !file.Contains("total.csv"))
                        {
                            // 最終更新日時を追加する
                            string 最終更新日時 = GetLastWriteTime(file);

                            string line = "";
                            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("Shift_JIS")))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {

                                    // [0] 店舗名
                                    // [1] 使用年月日
                                    // [2] 医薬品コード
                                    // [3] 医薬品名
                                    // [4] 名称２
                                    // [5] 使用量
                                    // [6] 薬価
                                    // [7] 後発品区分
                                    // [8] 代替区分

                                    var sepa2 = line.Split(',');
                                    bool ヘッダーか = false;
                                    if (sepa2[0] == "店舗名")
                                    {
                                        ヘッダーか = true;
                                    }
                                    

                                    // 最終更新日を追加する
                                    string buff = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", sepa2[0], sepa2[1], sepa2[2], sepa2[3], sepa2[4], sepa2[5], sepa2[6], sepa2[7], sepa2[8], ヘッダーか ? "最終更新日時" : 最終更新日時);

                                    //line += string.Format(",{0}", 最終更新日時);
                                    sw.WriteLine(buff);
                                }

                            }
                        }

                        sw.Flush();
                    }
                }

            }
            else if (種別 == 操作種別.使用量)
            {
                using (StreamWriter sw = new StreamWriter(totalcsvFilePath, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    foreach (var file in FilesArray)
                    {
                        string[] sepa = file.Split('.');
                        string 拡張子 = sepa[sepa.Length - 1];

                        // 書き込み先なのでtotal.csvは飛ばす
                        if (拡張子.ToLower().Equals("csv") && !file.Contains("total.csv"))
                        {
                            // 最終更新日時を追加する
                            string 最終更新日時 = GetLastWriteTime(file);

                            string line = "";
                            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("Shift_JIS")))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {

                                    // [0] 店舗名
                                    // [1] 使用年月日
                                    // [2] 商品名
                                    // [3] 使用量
                                    // [4] 薬価
                                    var sepa2 = line.Split(',');
                                    bool ヘッダーか = false;
                                    if (sepa2[0] == "店舗名")
                                    {
                                        ヘッダーか = true;
                                    }


                                    // 最終更新日を追加する
                                    string buff = string.Format("{0},{1},{2},{3},{4},{5}", sepa2[0], sepa2[1], sepa2[2], sepa2[3], sepa2[4], ヘッダーか ? "最終更新日時" : 最終更新日時);

                                    //line += string.Format(",{0}", 最終更新日時);
                                    sw.WriteLine(buff);
                                }

                            }
                        }

                        sw.Flush();
                    }
                }

            }
            else if (種別 == 操作種別.現在庫)
            {
                using (StreamWriter sw = new StreamWriter(totalcsvFilePath, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    foreach (var file in FilesArray)
                    {
                        string[] sepa = file.Split('.');
                        string 拡張子 = sepa[sepa.Length - 1];

                        // 書き込み先なのでtotal.csvは飛ばす
                        if (拡張子.ToLower().Equals("csv") && !file.Contains("total.csv"))
                        {
                            // 最終更新日時を追加する
                            string 最終更新日時 = GetLastWriteTime(file);

                            string line = "";
                            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("Shift_JIS")))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {
                                    // [0] 店舗名
                                    // [1] 薬品コード
                                    // [2] 薬品名
                                    // [3] 在庫数
                                    // [4] 使用期限
                                    // [5] 薬価
                                    // [6] メーカー名
                                    // [7] 後発区分
                                    // [8] 名称２
                                    var sepa2 = line.Split(',');
                                    bool ヘッダーか = false;
                                    if (sepa2[0] == "店舗名")
                                    {
                                        ヘッダーか = true;
                                    }

                                    line += string.Format(",{0}", ヘッダーか ? "最終更新日時" : 最終更新日時);
                                    sw.WriteLine(line);
                                }

                            }
                        }
                        sw.Flush();
                    }
                }

            }
            else if (種別 == 操作種別.不動品)
            {

                using (StreamWriter sw = new StreamWriter(totalcsvFilePath, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    foreach (var file in FilesArray)
                    {
                        string[] sepa = file.Split('.');
                        string 拡張子 = sepa[sepa.Length - 1];

                        // 書き込み先なのでtotal.csvは飛ばす
                        if (拡張子.ToLower().Equals("csv") && !file.Contains("total.csv"))
                        {
                            // 最終更新日時を追加する
                            string 最終更新日時 = GetLastWriteTime(file);

                            string line = "";
                            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("Shift_JIS")))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {
                                    // [0] 店舗名
                                    // [1] 薬品コード
                                    // [2] 薬品名
                                    // [3] 現在庫ｄ
                                    // [4] 使用期限
                                    // [5] 薬価ｂ

                                    // [6] １包単位量  //2015.05.02追加
                                    // [7] 名称２      //2015.05.02追加

                                    var sepa2 = line.Split(',');
                                    bool ヘッダーか = false;
                                    if (sepa2[0] == "店舗名")
                                    {
                                        ヘッダーか = true;
                                    }

                                    line += string.Format(",{0}", ヘッダーか ? "最終更新日時" : 最終更新日時);
                                    sw.WriteLine(line);
                                }

                            }
                        }
                        sw.Flush();
                    }
                }
            }
            else
            {
                throw new Exception("不明な種別の為、マージできませんでした。");
            }


        }

        private string GetLastWriteTime(string FilePath)
        {
            string 最終更新日時 = "";

            try
            {

                if (System.IO.File.Exists(FilePath))
                {
                    最終更新日時 = System.IO.File.GetLastWriteTime(FilePath).ToString("yyyy/MM/dd HH:mm");
                }

            }
            catch
            {
                // 最終更新日時は空のままにする
            }

            return 最終更新日時;

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

        private void AppendFile(Stream stream, string FilePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    string line = "";
                    using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("Shift_JIS")))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            sw.WriteLine(line);
                        }

                        sw.Flush();
                    }

                }
            }
            finally
            {
                stream.Close();
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void Update現在庫(string 店舗名)
        {
            try
            {
                string 在庫rootPath = Path.Combine(rootPath, "現在庫");
                string 店舗名rootPath = Path.Combine(在庫rootPath, 店舗名);
                this.作成Path = Path.Combine(店舗名rootPath, ".csv");

                if (Directory.Exists(在庫rootPath) == false)
                {
                    Directory.CreateDirectory(在庫rootPath);
                }

                if (Directory.Exists(店舗名rootPath) == false)
                {
                    Directory.CreateDirectory(店舗名rootPath);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        /// <summary>
        /// こっちももう使わない
        /// </summary>
        /// <param name="店舗名"></param>
        /// <param name="更新年"></param>
        /// <param name="更新月"></param>
        private void Update使用量(string 店舗名, string 更新年, string 更新月)
        {
            try
            {

                string 在庫rootPath = Path.Combine(rootPath, "使用量");
                string 店舗名rootPath = Path.Combine(在庫rootPath, 店舗名);
                string 更新年rootPath = Path.Combine(店舗名rootPath, 更新年);
                this.作成Path = Path.Combine(更新年rootPath, 更新月 + ".csv");

                if (Directory.Exists(店舗名rootPath) == false)
                {
                    Directory.CreateDirectory(店舗名rootPath);
                }

                if (Directory.Exists(更新年rootPath) == false)
                {
                    Directory.CreateDirectory(更新年rootPath);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Update使用量2(string 店舗名, string 更新年, string 更新月)
        {
            try
            {

                string 在庫rootPath = Path.Combine(rootPath, "使用量2");
                string 店舗名rootPath = Path.Combine(在庫rootPath, 店舗名);
                string 更新年rootPath = Path.Combine(店舗名rootPath, 更新年);
                this.作成Path = Path.Combine(更新年rootPath, 更新月 + ".csv");

                if (Directory.Exists(店舗名rootPath) == false)
                {
                    Directory.CreateDirectory(店舗名rootPath);
                }

                if (Directory.Exists(更新年rootPath) == false)
                {
                    Directory.CreateDirectory(更新年rootPath);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Update不動品(string 店舗名)
        {
            try
            {
                string 在庫rootPath = Path.Combine(rootPath, "不動品");
                string 店舗名rootPath = Path.Combine(在庫rootPath, 店舗名);
                this.作成Path = Path.Combine(店舗名rootPath, ".csv");

                if (Directory.Exists(在庫rootPath) == false)
                {
                    Directory.CreateDirectory(在庫rootPath);
                }

                if (Directory.Exists(店舗名rootPath) == false)
                {
                    Directory.CreateDirectory(店舗名rootPath);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void UpdateMEDIS()
        {
            string MEDISrootPath = Path.Combine(rootPath, "MEDIS");
            this.作成Path = Path.Combine(MEDISrootPath, "temp");
            //this.作成Path = Path.Combine(MEDISrootPath, "MEDIS.TXT");
        }

    }

    enum 操作種別
    {
        現在庫 = 0,
        使用量 = 1,
        不動品 = 2,
        使用量2 = 3,
    }

}
