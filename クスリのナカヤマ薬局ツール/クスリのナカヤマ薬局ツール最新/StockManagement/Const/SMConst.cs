using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StockManagement.Const
{
    public static class SMConst
    {
        public static string rootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "デッド品管理");
        public static string downloadFolder = Path.Combine(rootFolder, "DownloadData");
        public static string outputFolder = Path.Combine(rootFolder, "作成データ");
        public static string outputOrderFolder = Path.Combine(outputFolder, "貰受依頼書");
        public static string outputReceiveFolder = Path.Combine(outputFolder, "貰受可能リスト");
        public static string outputAllStoreNoUseFolder = Path.Combine(outputFolder, "全店使用無しデッド品リスト");
        public static string outputExpOrderFolder = Path.Combine(outputFolder, "期限切迫品依頼書");
        public static string settingsFile = Path.Combine(rootFolder, "Settings.ini");
        public static string exceptiveMedicinesFile = Path.Combine(rootFolder, "ExceptiveMedicines.csv");

        public static string VersionDatLocalPath = Path.Combine(rootFolder, "Version.dat");
        public static string UpdateLocalFolder = Path.Combine(rootFolder, "Update");
        public static string StockManagementBackUpPath = @"C:\StockManagementBkUp";

        

        public static DateTime deadStockMinDate = new DateTime(2012, 01, 01, 0, 0, 0);
        public static DateTime deadStockMaxDate1 = new DateTime(2999, 12, 31, 23, 59, 59);
        public static DateTime deadStockMaxDate2 = new DateTime(9999, 12, 31, 23, 59, 59);


        

    }
}
