using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using StockManagement.Common.Encrypts;

namespace StockManagement.Common
{
    public static class Settings
    {
        /// <summary>
        /// 
        /// </summary>
        static Settings()
        {
            FtpAddress = new Uri(ConfigurationManager.AppSettings["FtpAddress"].ToString());
            FtpId = AESCrypter.Decrypt(ConfigurationManager.AppSettings["FtpId"].ToString(), AESCrypter.AES_IV, AESCrypter.AES_Key);
            FtpCredential = AESCrypter.Decrypt(ConfigurationManager.AppSettings["FtpCredential"].ToString(), AESCrypter.AES_IV, AESCrypter.AES_Key);
            UsePassive = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePassive"]);

            if (string.IsNullOrEmpty(FtpAddress.OriginalString))
                throw new ConfigurationErrorsException("設定ファイルに 'FtpAddress 'が設定されていません。");

            if (string.IsNullOrEmpty(FtpId))
                throw new ConfigurationErrorsException("設定ファイルに 'FtpId 'が設定されていません。");

            if (string.IsNullOrEmpty(FtpCredential))
                throw new ConfigurationErrorsException("設定ファイルに 'FtpCredential 'が設定されていません。");
        }

        #region Local Paths

        public const string VersionFileName = "Version.dat";
        public const string TempVersionFileName = "TempVersion.dat";
        public const string SuggestFileName = "ここはdatとこのファイル以外は全てフォルダ.txt"; 
        public const string ExeName = "デッド品管理ツール.exe";
        public const string OldExeName = "デッド品管理ツール.old";

        public const string LocalStockFolderName = "現在庫";
        public const string LocalStockCsvFileName = "現在庫データ.csv"; 
        public const string LocalUsageFolderName = "使用量";
        public const string LocalDeadStockFolderName = "不動品";
        public const string LocalDeadStockCsvFileName = "不動品データ.csv";

        #endregion

        #region Ftps

        // From exe.config
        public static Uri FtpAddress;
        public static string FtpId;
        public static string FtpCredential;
        public static bool UsePassive;

        public static string UpdateFolderServerPath { get { return new Uri(FtpAddress, "Update/StockManagement").ToString(); } }
        public static string VersionDatServerPath { get { return new Uri(FtpAddress, "Update/StockManagement/Version.dat").ToString(); } }

        // FtpServerInfo
        //ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/現在庫
        public static string Ftp現在庫Path { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/在庫関連/現在庫").ToString(); } }

        //ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/使用量2
        public static string Ftp使用量2Path { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/在庫関連/使用量2").ToString(); } }

        //ftp://ftp.kusurinonakayama.jp/httpdocs/PharmacyTool/ClientBin/在庫関連/不動品
        public static string Ftp不動品Path { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/在庫関連/不動品").ToString(); } }

        #endregion

    }
}
