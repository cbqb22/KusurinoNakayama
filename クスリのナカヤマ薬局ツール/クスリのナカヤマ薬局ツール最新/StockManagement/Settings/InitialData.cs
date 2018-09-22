using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net.Configuration;
using StockManagement.ViewModel.Excel;
using StockManagement.ViewModel;


namespace StockManagement.Settings
{

    // Settings.iniデータを格納するSingletonクラス
    public static class InitialData
    {

        // [デッド品管理対象店舗]
        private static List<string> _DeadStockManagementStoresList;
        public static List<string> DeadStockManagementStoresList
        {
            get
            {
                return _DeadStockManagementStoresList;
            }

            set
            {
                // 再設定する場合もある為、シングルトンにはしない。

                //if (_DeadStockManagementStoresList == null)
                //{
                _DeadStockManagementStoresList = value;
                //}
            }
        }

        // [デッド品管理自店舗]
        private static string _DeadMangementSourceStore;
        public static string DeadMangementSourceStore
        {
            get
            {
                return _DeadMangementSourceStore;
            }

            set
            {
                //if (_DeadStockManagementStoresList == null)
                //{
                _DeadMangementSourceStore = value;
                //}
            }
        }

        // [全店舗リスト]
        private static List<string> _AllShopList;
        public static List<string> AllShopList
        {
            get
            {
                return _AllShopList;
            }

            set
            {
                //if (_AllShopList == null)
                //{
                _AllShopList = value;
                //}
            }
        }

        // 自店舗とデッド品管理対象店舗（重複削除)
        public static List<string> DeadMangementSourceStoreAndDeadStockManagementStoresList
        {
            get 
            {

                // 自店舗とデッド品管理対象店舗のみダウンロードする
                var ret = new List<string>();

                ret.Add(InitialData.DeadMangementSourceStore); // 自店舗
                InitialData.DeadStockManagementStoresList.ForEach
                    (
                         delegate(string x)
                         {
                             // 自店舗と重複しないようにする。
                             if (!ret.Contains(x))
                             {
                                 ret.Add(x);
                             }
                         }
                        );


                return ret;
            }
        }

        /// <summary>
        /// 使用量期間
        /// </summary>
        private static int _UsedAmountDateRange = 4; //デフォルトは４ヶ月
        public static int UsedAmountDateRange
        {
            get { return InitialData._UsedAmountDateRange; }
            set { InitialData._UsedAmountDateRange = value; }
        }

        /// <summary>
        /// 出力形式
        /// </summary>
        private static ExcelTypeEnum _OutputExcelType;
        public static ExcelTypeEnum OutputExcelType
        {
            get { return _OutputExcelType; }
            set { _OutputExcelType = value; }
        }


        /// <summary>
        /// 期限切迫期間
        /// </summary>
        private static int _ExpireRange = 6; //デフォルトは６ヶ月

        public static int ExpireRange
        {
            get { return InitialData._ExpireRange; }
            set { InitialData._ExpireRange = value; }
        }



        // 除外医薬品リスト ExceptiveMedicines.csvから読込み
        private static List<ExceptionMedicineEntity> _ExceptiveMedicinesList = new List<ExceptionMedicineEntity>();

        public static List<ExceptionMedicineEntity> ExceptiveMedicinesList
        {
            get { return _ExceptiveMedicinesList; }
            set { _ExceptiveMedicinesList = value; }
        }




        /// <summary>
        /// Windows8でFTPのエラーが出るのを対応
        /// 基礎になる接続が閉じられました。サーバーによってプロトコル違反が発生しました。
        /// </summary>
        public static void SetFTPAppConfig()
        {
            // Windows8でFTPのエラーが出るのを対応
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SettingsSection section = (SettingsSection)config.GetSection("system.net/settings");
            section.HttpWebRequest.UseUnsafeHeaderParsing = true;
            config.Save();
        }


        /// <summary>
        /// Settings.iniからデータを読み込む
        /// ExceptiveMedicines.csvからも除外医薬品を読み込む
        /// </summary>
        public static void LoadInitData()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string settingsiniFilePath = Path.Combine(desktopPath, @"デッド品管理\Settings.ini");

            using (StreamReader sr = new StreamReader(settingsiniFilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int rowcounter = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    rowcounter++;
                    if (rowcounter == 1)
                    {
                        InitialData.DeadMangementSourceStore = line.Replace("[デッド品管理自店舗]=", "");

                        //MessageBoxTop.Show(InitialData.DeadMangementSourceStore);
                    }
                    else if (rowcounter == 2)
                    {
                        string deadManagementStoreString = line.Replace("[デッド品管理対象店舗]=", "");
                        var sepa = deadManagementStoreString.Split(',');

                        if (sepa != null)
                        {
                            InitialData.DeadStockManagementStoresList = sepa.ToList();
                        }
                        else
                        {
                            InitialData.DeadStockManagementStoresList = new List<string>();
                        }

                        //string str = "";
                        //InitialData.DeadStockManagementStoresList.ForEach(x => str = str + x);
                        //MessageBoxTop.Show(str);

                    }
                    else if (rowcounter == 3)
                    {

                        //[全店舗リスト]
                        string allshopList = line.Replace("[全店舗リスト]=", "");
                        var sepa = allshopList.Split(',');

                        if (sepa != null)
                        {
                            InitialData.AllShopList = sepa.ToList();
                        }
                        else
                        {
                            InitialData.AllShopList = new List<string>();
                        }

                    }
                    else if (rowcounter == 4)
                    {

                        //[使用量期間]
                        string UsedAmountDateRange = line.Replace("[使用量期間]=", "");
                        var sepa = UsedAmountDateRange.Split(',');

                        int result;
                        if (int.TryParse(UsedAmountDateRange, out result) == false)
                        {
                            InitialData.UsedAmountDateRange = 4; // 変換に失敗したら初期の当月を含め４ヶ月以内
                        }
                        else
                        {

                            InitialData.UsedAmountDateRange = result;
                        }


                    }

                    else if (rowcounter == 5)
                    {

                        //[出力形式]
                        string OutputTypeNumber = line.Replace("[出力形式]=", "");

                        int result;
                        if (int.TryParse(OutputTypeNumber, out result) == false)
                        {
                            InitialData.OutputExcelType = ExcelTypeEnum.Unknown;
                        }
                        else
                        {

                            switch (result)
                            {
                                case (int)ExcelTypeEnum.Excel2003:
                                    InitialData.OutputExcelType = ExcelTypeEnum.Excel2003;
                                    break;
                                case (int)ExcelTypeEnum.Excel2007:
                                    InitialData.OutputExcelType = ExcelTypeEnum.Excel2007;
                                    break;
                                default:
                                    InitialData.OutputExcelType = ExcelTypeEnum.Unknown;
                                    break;
                            }
                        }


                    }

                    else if (rowcounter == 6)
                    {

                        //[使用量期間]
                        string ExpireRangeStr = line.Replace("[期限切迫期間]=", "");
                        var sepa = ExpireRangeStr.Split(',');

                        int result;
                        if (int.TryParse(ExpireRangeStr, out result) == false)
                        {
                            InitialData.ExpireRange = 6; // 変換に失敗したらデフォルトの６ヶ月以内
                        }
                        else
                        {

                            InitialData.ExpireRange = result;
                        }


                    }


                }
            }

            Set除外医薬品List();


        }



        public static void Set除外医薬品List()
        {
            _ExceptiveMedicinesList = new List<ExceptionMedicineEntity>();

            if (!File.Exists(StockManagement.Const.SMConst.exceptiveMedicinesFile))
            {
                File.Create(StockManagement.Const.SMConst.exceptiveMedicinesFile);
                return;
            }

            using (StreamReader sr = new StreamReader(StockManagement.Const.SMConst.exceptiveMedicinesFile, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    var sepa = line.Split(',');
                    if (sepa.Count() != 2)
                    {
                        continue;
                    }

                    ExceptionMedicineEntity ent = new ExceptionMedicineEntity();
                    ent.医薬品名称 = sepa[0];
                    ent.レセプト電算コード = sepa[1];

                    _ExceptiveMedicinesList.Add(ent);

                }
            }



        }







    }
}
