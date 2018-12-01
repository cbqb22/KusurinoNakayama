using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using OASystem.Common.Encrypts;

namespace OASystem.Common
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

        public const string DownloadFilesPath = @"C:\OASystem\DownloadFiles";
        public const string Download不動品TotalFilePath = @"C:\OASystem\DownloadFiles\不動品total.csv";
        public const string Download現在庫TotalFilePath = @"C:\OASystem\DownloadFiles\現在庫total.csv";
        public const string DownloadMEDIS_HOT13lFilePath = @"C:\OASystem\DownloadFiles\MEDIS_HOT13.TXT";
        public const string Download帳合先マスタFilePath = @"C:\OASystem\DownloadFiles\帳合先マスタ.csv";
        public const string Download帳合先チェックマスタ医薬品別FilePath = @"C:\OASystem\DownloadFiles\帳合先チェックマスタ医薬品別.csv";
        public const string Download個別管理医薬品マスタFilePath = @"C:\OASystem\DownloadFiles\個別管理医薬品マスタ.csv";
        public const string Download帳合先チェックマスタメーカー別FilePath = @"C:\OASystem\DownloadFiles\帳合先チェックマスタメーカー別.csv";
        public const string Download保護リストFolderPath = @"C:\OASystem\DownloadFiles\保護リスト";
        public const string Download優先移動リストFolderPath = @"C:\OASystem\DownloadFiles\優先移動リスト";

        public const string SENDO1DATFilePath = @"C:\MEDICODE DATA\SEND01.DAT";
        public const string SettingsIniFilePath = @"C:\OASystem\Settings.ini";
        public const string OrderLogsFolderPath = @"C:\OASystem\OrderLogs";
        
        public const string TempFilesFolderPath = @"C:\OASystem\TempFiles";
        public const string TempBlancingAccountsFilePath = @"C:\OASystem\TempFiles\帳合先マスタTemp.csv";
        public const string TempBalancingAccountsCheckMakerSortFilePath = @"C:\OASystem\TempFiles\帳合先チェックマスタメーカー別Temp.csv";
        public const string TempBalancingAccountsCheckMedicineSortFilePath = @"C:\OASystem\TempFiles\帳合先チェックマスタ医薬品別Temp.csv";
        public const string TempIndividualBasedMedicineFilePath = @"C:\OASystem\TempFiles\個別管理医薬品マスタTemp.csv";
        public const string Temp保護リストFilePath = @"C:\OASystem\TempFiles\保護リストTemp.csv";
        public const string Temp優先移動リストFilePath = @"C:\OASystem\TempFiles\優先移動リストTemp.csv";

        public const string OASystemRootPath = @"C:\OASystem";

        public const string OASystemProcessName = @"OASystem.exe";
        public const string MediWebProcessName = @"FESTA";
        public const string MediWebProgramPath = @"C:\Program Files\MEDICODE-Web SR\FESTA.exe";

        public const string UpdateLocalFolder = @"C:\OASystem\Update";
        public const string VersionDatLocalPath = @"C:\OASystem\Version.dat";
        public const string OASystemBackUpPath = @"C:\OASystemBkUp";

        #endregion

        #region Ftps

        // From exe.config
        public static Uri FtpAddress;
        public static string FtpId;
        public static string FtpCredential;
        public static bool UsePassive;

        public static string UpdateFolderServerPath { get { return new Uri(FtpAddress, "Update/OASystem").ToString(); } }
        public static string UploadPath帳合先マスタCSV { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/発注支援システムマスタ/帳合先マスタ.csv").ToString(); } }
        public static string UploadPath帳合先チェックマスタ医薬品別CSV { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/発注支援システムマスタ/帳合先チェックマスタ医薬品別.csv").ToString(); } }
        public static string UploadPath個別管理医薬品マスタCSV { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/発注支援システムマスタ/個別管理医薬品マスタ.csv").ToString(); } }
        public static string UploadPath帳合先チェックマスタメーカー別CSV { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/発注支援システムマスタ/帳合先チェックマスタメーカー別.csv").ToString(); } }
        public static string UploadFolderPath保護リスト { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/発注支援システムマスタ/保護リスト").ToString(); } }
        public static string UploadMEDIS_HOT13FilePath { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/在庫関連/MEDIS/MEDIS_HOT13.TXT").ToString(); } }
        public static string UploadFolderPath優先移動リスト { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/発注支援システムマスタ/優先移動リスト").ToString(); } }
        public static string VersionDatServerPath { get { return new Uri(FtpAddress, "Update/OASystem/Version.dat").ToString(); } }

        public static string ServerDeadStockTotalPath { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/在庫関連/不動品/total.csv").ToString(); } }
        public static string ServerCurrentStockTotalPath { get { return new Uri(FtpAddress, "PharmacyTool/ClientBin/在庫関連/現在庫/total.csv").ToString(); } }

        #endregion

    }
}
