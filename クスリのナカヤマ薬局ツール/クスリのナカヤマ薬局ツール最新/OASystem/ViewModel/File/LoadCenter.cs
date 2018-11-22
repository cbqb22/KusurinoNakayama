using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OASystem.Model.Entity;
using OASystem.Model.Enum;
using OASystem.ViewModel.Common.DataConvert;
using OASystem.Common;


namespace OASystem.ViewModel.File
{
    public static class LoadCenter
    {
        public static List<BalancingAccountsCheckMedicineSortEntity> Load帳合先チェックマスタ医薬品別()
        {

            #region 医薬品別

            List<BalancingAccountsCheckMedicineSortEntity> bacmselist = new List<BalancingAccountsCheckMedicineSortEntity>();

            using (StreamReader sr = new StreamReader(Settings.Download帳合先チェックマスタ医薬品別FilePath, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    //JANコード,レセプト電算コード,医薬品名,包装形態,包装単位,包装単位数,包装総量,剤形区分,メーカー名,帳合先,修正後帳合先,JAN管理

                    BalancingAccountsCheckMedicineSortEntity ent = new BalancingAccountsCheckMedicineSortEntity();

                    var sepa = line.Split(',');
                    if (sepa.Length != 12)
                    {
                        continue;
                    }

                    // JAN
                    if (sepa[0] != "-" && sepa[0].Length != 13)
                    {
                        continue;
                    }

                    int result;
                    if (int.TryParse(sepa[7], out result) == false)
                    {
                        result = 0;
                    }

                    ent.JANコード = sepa[0];
                    ent.レセプト電算コード = sepa[1];
                    ent.医薬品名 = sepa[2];
                    ent.包装形態 = sepa[3];
                    ent.包装単位 = sepa[4];
                    ent.包装単位数 = sepa[5];
                    ent.包装総量 = sepa[6];
                    ent.剤形区分 = (剤形区分Enum)result;
                    ent.販売会社 = sepa[8];
                    ent.帳合先 = sepa[9];
                    ent.修正後帳合先 = sepa[10];
                    ent.IsJAN管理 = sepa[11] == "1" ? true : false;


                    bacmselist.Add(ent);
                }
            }

            return bacmselist;

            #endregion

        }
        public static Dictionary<string, BalancingAccountsEntity> Load帳合先チェックマスタメーカー別_その他のメーカー以外()
        {
            #region メーカー別

            var dic = new Dictionary<string, BalancingAccountsEntity>();
            List<BalancingAccountsEntity> baelist = Model.DI.帳合先マスタ;

            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.Download帳合先チェックマスタメーカー別FilePath, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    var sepa = line.Split(',');
                    if (sepa.Length != 3)
                    {
                        continue;
                    }

                    if (sepa[1].Length != 9)
                    {
                        continue;
                    }

                    // その他のメーカーは２行目
                    if (counter == 2)
                    {
                        continue;
                    }


                    foreach (var bae in baelist)
                    {
                        if (sepa[1] == bae.卸ＶＡＮコード)
                        {

                            dic.Add(sepa[0], bae.DeepCopy());
                            break;
                        }
                    }
                }
            }

            return dic;

            #endregion メーカー別

        }
        public static Dictionary<string, BalancingAccountsEntity> Load帳合先チェックマスタメーカー別_その他のメーカーのみ()
        {
            #region メーカー別

            var dic = new Dictionary<string, BalancingAccountsEntity>();
            List<BalancingAccountsEntity> baelist = Model.DI.帳合先マスタ;

            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.Download帳合先チェックマスタメーカー別FilePath, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    var sepa = line.Split(',');
                    if (sepa.Length != 3)
                    {
                        continue;
                    }

                    if (sepa[1].Length != 9)
                    {
                        continue;
                    }

                    // その他のメーカーは２行目
                    if (counter == 2)
                    {
                        foreach (var bae in baelist)
                        {
                            if (sepa[1] == bae.卸ＶＡＮコード)
                            {
                                dic.Add(sepa[0], bae.DeepCopy());

                                return dic;
                            }
                        }
                    }
                }
            }

            throw new Exception("帳合先チェックマスタのその他のメーカーを読み込み中にエラーが発生しました。\r\n処理を中断します。");


            #endregion メーカー別

        }

        public static List<BalancingAccountsEntity> Load帳合先マスタ()
        {
            List<BalancingAccountsEntity> baelist = new List<BalancingAccountsEntity>();
            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.Download帳合先マスタFilePath, Encoding.GetEncoding(932)))
            {
                int rowcounter = 0;
                int counter = 0;
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    BalancingAccountsEntity ent = new BalancingAccountsEntity();

                    var sepa = line.Split(',');
                    if (sepa.Length != 3)
                    {
                        continue;
                    }

                    if (sepa[0].Length != 9)
                    {
                        continue;
                    }

                    ent.卸ＶＡＮコード = sepa[0];
                    ent.帳合先名称 = sepa[1];
                    ent.表示順 = ++rowcounter;

                    baelist.Add(ent);
                }
            }

            return baelist;

        }
        public static List<IndividualBasedManagementMedicineEntity> Load個別管理医薬品マスタ()
        {
            List<IndividualBasedManagementMedicineEntity> ibmmEnt = new List<IndividualBasedManagementMedicineEntity>();
            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.Download個別管理医薬品マスタFilePath, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    IndividualBasedManagementMedicineEntity ibma = new IndividualBasedManagementMedicineEntity();

                    var sepa = line.Split(',');
                    if (sepa.Length != 9)
                    {
                        continue;
                    }

                    if (sepa[0].Length != 13)
                    {
                        continue;
                    }

                    int result剤形区分;
                    if (int.TryParse(sepa[6], out result剤形区分) == false)
                    {
                        result剤形区分 = 0;
                    }

                    //JANコード,医薬品名称,包装形態,包装単位,包装単位数,包装総量,剤形区分,製薬会社,販売会社
                    ibma.JANコード = sepa[0];
                    ibma.医薬品名 = sepa[1];
                    ibma.包装形態 = sepa[2];
                    ibma.包装単位 = sepa[3];
                    ibma.包装単位数 = sepa[4];
                    ibma.包装総量 = sepa[5];
                    ibma.剤形区分 = (剤形区分Enum)result剤形区分;
                    ibma.製薬会社 = sepa[7];
                    ibma.販売会社 = sepa[8];

                    ibmmEnt.Add(ibma);
                }
            }

            return ibmmEnt;

        }

        public static List<ProtectEntity> Load保護リスト(string 自店舗名)
        {

            #region 保護リスト

            List<ProtectEntity> protectlist = new List<ProtectEntity>();

            string パス = Path.Combine(OASystem.Common.Settings.Download保護リストFolderPath, 自店舗名 + ".csv");

            if (!System.IO.File.Exists(パス))
            {
                return protectlist;
            }


            using (StreamReader sr = new StreamReader(パス, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;

                    // 1行目はヘッダー、２行目は店舗名
                    if (counter <= 2)
                    {
                        continue;
                    }

                    //レセプト電算コード,医薬品名
                    ProtectEntity ent = new ProtectEntity();

                    var sepa = line.Split(',');
                    if (sepa.Length != 2)
                    {
                        continue;
                    }

                    if (sepa[0] == "" || sepa[0].Length != 9)
                    {
                        continue;
                    }


                    ent.レセプト電算コード = sepa[0];
                    ent.医薬品名 = sepa[1];


                    protectlist.Add(ent);
                }
            }

            return protectlist;

            #endregion

        }
        public static List<ProtectEntity> Load保護リストAll()
        {

            #region 保護リストAll

            List<ProtectEntity> protectlist = new List<ProtectEntity>();

            var filesPath = System.IO.Directory.GetFiles(OASystem.Common.Settings.Download保護リストFolderPath);


            foreach (var file in filesPath)
            {

                var sepa = file.Split('.');
                if (sepa.Length == 0)
                {
                    continue;
                }

                var 拡張子 = sepa[sepa.Length - 1];
                if (拡張子 != "csv" && 拡張子 != "CSV")
                {
                    continue;
                }

                var sepa2 = file.Split('\\');
                if (sepa2.Length <= 2)
                {
                    continue;
                }

                if (!System.IO.File.Exists(file))
                {
                    continue;
                }

                var ファイル名 = sepa2[sepa2.Length - 1];
                var 店舗名 = ファイル名.Replace(".csv", "").Replace(".CSV", "");


                using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding(932)))
                {
                    int counter = 0;
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;

                        // 1行目はヘッダー、２行目は店舗名
                        if (counter <= 2)
                        {
                            continue;
                        }

                        //レセプト電算コード,医薬品名
                        ProtectEntity ent = new ProtectEntity();

                        var sepa3 = line.Split(',');
                        if (sepa3.Length != 2)
                        {
                            continue;
                        }

                        if (sepa3[0] == "" || sepa3[0].Length != 9)
                        {
                            continue;
                        }


                        ent.レセプト電算コード = sepa3[0];
                        ent.医薬品名 = sepa3[1];
                        ent.店舗名 = 店舗名;


                        protectlist.Add(ent);
                    }
                }

            }


            return protectlist;

            #endregion

        }


        public static List<PriorityMoveEntity> Load優先移動リスト(string 自店舗名)
        {

            #region 優先移動リスト

            List<PriorityMoveEntity> prioritymovelist = new List<PriorityMoveEntity>();

            string パス = Path.Combine(OASystem.Common.Settings.Download優先移動リストFolderPath, 自店舗名 + ".csv");

            if (!System.IO.File.Exists(パス))
            {
                return prioritymovelist;
            }


            using (StreamReader sr = new StreamReader(パス, Encoding.GetEncoding(932)))
            {
                int counter = 0;
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;

                    // 1行目はヘッダー、２行目は店舗名
                    if (counter <= 2)
                    {
                        continue;
                    }

                    //レセプト電算コード,医薬品名,コメント
                    PriorityMoveEntity ent = new PriorityMoveEntity();

                    var sepa = line.Split(',');
                    if (sepa.Length != 3)
                    {
                        continue;
                    }

                    if (sepa[0] == "" || sepa[0].Length != 9)
                    {
                        continue;
                    }

                    ent.レセプト電算コード = sepa[0];
                    ent.医薬品名 = sepa[1];
                    ent.コメント = sepa[2];

                    prioritymovelist.Add(ent);
                }
            }

            return prioritymovelist;

            #endregion

        }
        public static List<PriorityMoveEntity> Load優先移動リストAll()
        {

            #region 優先移動リストAll

            List<PriorityMoveEntity> prioritymovelist = new List<PriorityMoveEntity>();

            var filesPath = System.IO.Directory.GetFiles(OASystem.Common.Settings.Download優先移動リストFolderPath);


            foreach (var file in filesPath)
            {

                var sepa = file.Split('.');
                if (sepa.Length == 0)
                {
                    continue;
                }

                var 拡張子 = sepa[sepa.Length - 1];
                if (拡張子 != "csv" && 拡張子 != "CSV")
                {
                    continue;
                }

                var sepa2 = file.Split('\\');
                if (sepa2.Length <= 2)
                {
                    continue;
                }

                if (!System.IO.File.Exists(file))
                {
                    continue;
                }

                var ファイル名 = sepa2[sepa2.Length - 1];
                var 店舗名 = ファイル名.Replace(".csv", "").Replace(".CSV", "");


                using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding(932)))
                {
                    int counter = 0;
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;

                        // 1行目はヘッダー、２行目は店舗名
                        if (counter <= 2)
                        {
                            continue;
                        }

                        //レセプト電算コード,医薬品名,コメント
                        PriorityMoveEntity ent = new PriorityMoveEntity();

                        var sepa3 = line.Split(',');
                        if (sepa3.Length != 3)
                        {
                            continue;
                        }

                        if (sepa3[0] == "" || sepa3[0].Length != 9)
                        {
                            continue;
                        }


                        ent.レセプト電算コード = sepa3[0];
                        ent.医薬品名 = sepa3[1];
                        ent.コメント = sepa3[2];
                        ent.店舗名 = 店舗名;


                        prioritymovelist.Add(ent);
                    }
                }

            }


            return prioritymovelist;

            #endregion

        }


    }
}
