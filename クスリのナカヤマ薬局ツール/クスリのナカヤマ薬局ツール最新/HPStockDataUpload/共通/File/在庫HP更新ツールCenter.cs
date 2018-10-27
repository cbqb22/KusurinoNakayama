using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIO = System.IO;
using クスリのナカヤマ薬局ツール.Properties;
using System.Diagnostics;
using System.Windows;

namespace クスリのナカヤマ薬局ツール.共通.File
{
    public static class 在庫HP更新ツールCenter
    {

        public static void FolderCheck()
        {
            if (!SIO.Directory.Exists(Settings.StockUpdaterRootFolder))
                SIO.Directory.CreateDirectory(Settings.StockUpdaterRootFolder);

            if (!SIO.Directory.Exists(Settings.StockUpdaterRootBackUpPath))
                SIO.Directory.CreateDirectory(Settings.StockUpdaterRootBackUpPath);
        }


        public static bool FileCheck()
        {
            if (!SIO.File.Exists(Settings.VersionDatLocalPath))
            {
                MessageBox.Show(@"Version.datファイルが存在しない為、処理を中止しました。\r\nダウンロードしたファイルを確認してください。\r\nセキュリティソフトを使用している場合は[C:\在庫HP更新ツール]フォルダを除外設定する必要がある場合があります。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!SIO.File.Exists(Settings.SettingsIniLocalPath))
            {
                MessageBox.Show(@"Settings.iniファイルが存在しない為、処理を中止しました。\r\nダウンロードしたファイルを確認してください。\r\nセキュリティソフトを使用している場合は[C:\在庫HP更新ツール]フォルダを除外設定する必要がある場合があります。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}

