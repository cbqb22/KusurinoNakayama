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
            using (StreamReader sr = new StreamReader(クスリのナカヤマ薬局ツール.Properties.Settings.Default.VersionDatLocalPath, Encoding.GetEncoding(932)))
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
    }
}
