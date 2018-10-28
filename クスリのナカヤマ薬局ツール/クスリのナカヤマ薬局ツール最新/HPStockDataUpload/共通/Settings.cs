using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using クスリのナカヤマ薬局ツール.共通.Encrypts;

namespace クスリのナカヤマ薬局ツール.共通
{
    public static class Settings
    {
        static Settings()
        {
            FtpAddress = ConfigurationManager.AppSettings["FtpAddress"].ToString();
            FtpId = AESCrypter.Decrypt(ConfigurationManager.AppSettings["FtpId"].ToString(), AESCrypter.AES_IV, AESCrypter.AES_Key);
            FtpCredential = AESCrypter.Decrypt(ConfigurationManager.AppSettings["FtpCredential"].ToString(), AESCrypter.AES_IV, AESCrypter.AES_Key);
            UsePassive = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePassive"]);

            BasicId = AESCrypter.Decrypt(ConfigurationManager.AppSettings["BasicId"].ToString(), AESCrypter.AES_IV, AESCrypter.AES_Key);
            BasicCredential = AESCrypter.Decrypt(ConfigurationManager.AppSettings["BasicCredential"].ToString(), AESCrypter.AES_IV, AESCrypter.AES_Key);

            if (string.IsNullOrEmpty(FtpAddress))
                throw new ConfigurationErrorsException("設定ファイルに 'FtpAddress 'が設定されていません。");

            if (string.IsNullOrEmpty(FtpId))
                throw new ConfigurationErrorsException("設定ファイルに 'FtpId 'が設定されていません。");

            if (string.IsNullOrEmpty(FtpCredential))
                throw new ConfigurationErrorsException("設定ファイルに 'FtpCredential 'が設定されていません。");

            if (string.IsNullOrEmpty(BasicId))
                throw new ConfigurationErrorsException("設定ファイルに 'BasicId 'が設定されていません。");

            if (string.IsNullOrEmpty(BasicCredential))
                throw new ConfigurationErrorsException("設定ファイルに 'BasicCredential 'が設定されていません。");
        }

        // From exe.config
        public static string FtpAddress;
        public static string FtpId;
        public static string FtpCredential;
        public static bool UsePassive;

        // FileName
        public const string 現在庫出力ファイル名 = "現在庫データ.csv";
        public const string 使用量2出力ファイル名 = "使用量2データ.csv";
        public const string 不動品出力ファイル名 = "不動品データ.csv";
        public const string VerdatFileName = "Version.dat";
        public const string OldExeName = "在庫HP更新ツール.old";
        public const string ExeName = "在庫HP更新ツール.exe";

        // LocalInfo
        public const string StockUpdaterRootFolder = @"C:\在庫HP更新ツール\";
        public const string UpdateLocalFolder = StockUpdaterRootFolder + "Update";
        public const string VersionDatLocalPath = StockUpdaterRootFolder + VerdatFileName;
        public const string SettingsIniLocalPath = StockUpdaterRootFolder + "Settings.ini";

        public const string StockUpdaterRootBackUpPath = @"C:\在庫HP更新ツールBkUp";

        // FtpServerInfo
        public static string Ftp現在庫Path { get { return FtpAddress + "PharmacyTool/ClientBin/在庫関連/現在庫"; } }
        public static string Ftp使用量2Path { get { return FtpAddress + "PharmacyTool/ClientBin/在庫関連/使用量2"; } }
        public static string Ftp不動品Path { get { return FtpAddress + "PharmacyTool/ClientBin/在庫関連/不動品"; } }

        public static string UpdateFolderServerPath { get { return FtpAddress + "Update/在庫HP更新ツール"; } }
        public static string VersionDatServerPath { get { return FtpAddress + "Update/在庫HP更新ツール/" + VerdatFileName; } }

        // HpInfo
        public const string HpBaseAddress = @"http://www.kusurinonakayama.jp/";
        public const string ZaikoGenericHandlerPath = HpBaseAddress + @"PharmacyTool/GenericHandler/在庫データFileUpload.ashx";
        public static string BasicId;
        public static string BasicCredential;

#if DEBUG


        //// Credencial
        //public const string FtpID = "a10254737";
        //public const string FtpCredent = "C_hwJ5Eg";

        //// Http Basic
        //public const string BasicID = "poohace";
        //public const string BasicCredent = "cbqb22";

        //// Path   
        //public const string ZaikoGenericHandlerPath = @"http://localhost:56305/GenericHandler/在庫データFileUpload.ashx";
        //public const string FtpRootPath = "ftp://ftp.my-world.me/httpdocs/";
        //public const string Ftp現在庫Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/現在庫";
        ////こっちはもう使わない
        ////public const string Ftp使用量Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/使用量";
        //public const string Ftp使用量2Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/使用量2";
        //public const string Ftp不動品Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/不動品";

#else

        // Credencial
        public const string FtpID = "a10254737";
        public const string FtpCredent = "C_hwJ5Eg";

        // Http Basic
        public const string BasicID = "poohace";
        public const string BasicCredent = "cbqb22";

        // Path   
        public const string ZaikoGenericHandlerPath = @"http://www.my-world.me/PharmacyTool/GenericHandler/在庫データFileUpload.ashx";
        public const string FtpRootPath = "ftp://ftp.my-world.me/httpdocs/";
        public const string Ftp現在庫Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/現在庫";
        //こっちはもう使わない
        //public const string Ftp使用量Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/使用量";
        public const string Ftp使用量2Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/使用量2";
        public const string Ftp不動品Path = "ftp://ftp.my-world.me/httpdocs/PharmacyTool/ClientBin/在庫関連/不動品";

#endif

    }

}
