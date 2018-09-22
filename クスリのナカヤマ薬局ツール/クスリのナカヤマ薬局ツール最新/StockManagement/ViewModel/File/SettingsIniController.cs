using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StockManagement.Properties;

namespace StockManagement.ViewModel.File
{
    public static class SettingsIniController
    {

        public static void SetVersionNameToDI()
        {
            using (StreamReader sr = new StreamReader(StockManagement.Const.SMConst.VersionDatLocalPath, Encoding.GetEncoding(932)))
            {
                string line = "";
                string version = "";
                while ((line = sr.ReadLine()) != null)
                {
                    // 最終行を保持
                    version = line;
                }

                StockManagement.Model.DI.VersionName = version;

            }

        }
    }
}
