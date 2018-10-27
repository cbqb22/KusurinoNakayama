using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using クスリのナカヤマ薬局ツール.Properties;

namespace クスリのナカヤマ薬局ツール.共通.File
{
    public static class SettingsIniController
    {
        public static void SetVersionNameToDI()
        {
            using (StreamReader sr = new StreamReader(Settings.VersionDatLocalPath, Encoding.GetEncoding(932)))
            {
                string line = "";
                string version = "";
                while ((line = sr.ReadLine()) != null)
                {
                    // 最終行を保持
                    version = line;
                }

                クスリのナカヤマ薬局ツール.Model.DI.在庫HP更新ツールVersionName = version;
            }
        }


        public static void DoWrite(string 現在庫ファイルパス, string 使用量ファイルパス, string 不動品ファイルパス, string 出力先フォルダ名,string 出力店舗名称)
        {
            using (StreamWriter sw = new StreamWriter(Settings.SettingsIniLocalPath, false, Encoding.GetEncoding(932)))
            {
                sw.WriteLine(string.Format("[現在庫データファイル]={0}", 現在庫ファイルパス));
                sw.WriteLine(string.Format("[使用量データファイル]={0}", 使用量ファイルパス));
                sw.WriteLine(string.Format("[不動品データファイル]={0}", 不動品ファイルパス));
                sw.WriteLine(string.Format("[出力先フォルダ名]={0}", 出力先フォルダ名));
                sw.WriteLine(string.Format("[出力店舗名称]={0}", 出力店舗名称));

                sw.Flush();
            }

            クスリのナカヤマ薬局ツール.Model.DI.現在庫ファイルパス = 現在庫ファイルパス;
            クスリのナカヤマ薬局ツール.Model.DI.使用量ファイルパス = 使用量ファイルパス;
            クスリのナカヤマ薬局ツール.Model.DI.不動品ファイルパス = 不動品ファイルパス;
            クスリのナカヤマ薬局ツール.Model.DI.出力先フォルダ名 = 出力先フォルダ名;
            クスリのナカヤマ薬局ツール.Model.DI.出力店舗名称 = 出力店舗名称;
        }


        public static void DoLoad()
        {
            if (!System.IO.File.Exists(Settings.SettingsIniLocalPath))
            {
                using (StreamWriter sw = new StreamWriter(Settings.SettingsIniLocalPath, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("[現在庫データファイル]=");
                    sw.WriteLine("[使用量データファイル]=");
                    sw.WriteLine("[不動品データファイル]=");
                    sw.WriteLine("[出力先フォルダ名]=");
                    sw.WriteLine("[出力店舗名称]=");

                    sw.Flush();
                }
            }

            using (StreamReader sr = new StreamReader(Settings.SettingsIniLocalPath, Encoding.GetEncoding(932)))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {

                    if (line.StartsWith("[現在庫データファイル]="))
                    {
                        クスリのナカヤマ薬局ツール.Model.DI.現在庫ファイルパス = line.Substring(13);
                    }
                    else if (line.StartsWith("[使用量データファイル]="))
                    {
                        クスリのナカヤマ薬局ツール.Model.DI.使用量ファイルパス = line.Substring(13);
                    }
                    else if (line.StartsWith("[不動品データファイル]="))
                    {
                        クスリのナカヤマ薬局ツール.Model.DI.不動品ファイルパス = line.Substring(13);
                    }
                    else if (line.StartsWith("[出力先フォルダ名]="))
                    {
                        クスリのナカヤマ薬局ツール.Model.DI.出力先フォルダ名 = line.Substring(11);
                    }
                    else if (line.StartsWith("[出力店舗名称]="))
                    {
                        クスリのナカヤマ薬局ツール.Model.DI.出力店舗名称 = line.Substring(9);
                    }
                }
            }
        }
    }
}
