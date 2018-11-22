using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OASystem.Properties;
using OASystem.Common;

namespace OASystem.ViewModel.File
{
    public static class SettingsIniController
    {
        public static void DoLoad()
        {
            if (!System.IO.File.Exists(Settings.SettingsIniFilePath))
            {
                using (StreamWriter sw = new StreamWriter(Settings.SettingsIniFilePath, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("[自店舗名]=");
                    sw.WriteLine("[使用するプリンタ名]=");
                    sw.WriteLine("[選択トレイ]=");
                    sw.WriteLine(@"[MEDICODE-WebSR FIlePath]=C:\Program Files\MEDICODE-Web SR\FESTA.exe");

                    sw.Flush();
                }
            }

            using (StreamReader sr = new StreamReader(Settings.SettingsIniFilePath, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        OASystem.Model.DI.自店舗名 = line.Substring(7);
                    }
                    else if (counter == 2)
                    {
                        OASystem.Model.DI.使用するプリンタ名 = line.Substring(12);
                    }
                    else if (counter == 3)
                    {
                        OASystem.Model.DI.選択トレイ = line.Substring(8);
                    }
                    else if (counter == 4)
                    {
                        OASystem.Model.DI.MEDICODEWebSRFIlePath = line.Substring(26);
                    }
                }
            }
        }

        public static void SetVersionNameToDI()
        {
            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.VersionDatLocalPath, Encoding.GetEncoding(932)))
            {
                string line = "";
                string version = "";
                while ((line = sr.ReadLine()) != null)
                {
                    // 最終行を保持
                    version = line;
                }

                OASystem.Model.DI.VersionName = version;

            }

        }

        public static void DoWrite(string 自店舗名, string 使用するプリンタ名,string 選択トレイ, string MEDICODEFilePath)
        {
            using (StreamWriter sw = new StreamWriter(OASystem.Common.Settings.SettingsIniFilePath, false, Encoding.GetEncoding(932)))
            {
                sw.WriteLine(string.Format("[自店舗名]={0}", 自店舗名));
                sw.WriteLine(string.Format("[使用するプリンタ名]={0}", 使用するプリンタ名));
                sw.WriteLine(string.Format("[選択トレイ]={0}", 選択トレイ));
                sw.WriteLine(string.Format("[MEDICODE-WebSR FilePath]={0}", MEDICODEFilePath));

                sw.Flush();
            }

            OASystem.Model.DI.自店舗名 = 自店舗名;
            OASystem.Model.DI.使用するプリンタ名 = 使用するプリンタ名;
            OASystem.Model.DI.MEDICODEWebSRFIlePath = MEDICODEFilePath;
            OASystem.Model.DI.選択トレイ = 選択トレイ;
        }

    }
}
