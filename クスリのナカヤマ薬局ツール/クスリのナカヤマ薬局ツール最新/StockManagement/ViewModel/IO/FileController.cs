using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SIO = System.IO;
using StockManagement.Const;
using StockManagement.ViewModel.Common.MessageBox;
using StockManagement.Settings;

namespace StockManagement.ViewModel.IO
{
    public static class FileController
    {
        public static bool FileCheck()
        {
            if (!SIO.Directory.Exists(SMConst.rootFolder) ||
                !SIO.Directory.Exists(SMConst.downloadFolder))
            {
                FolderController.FolderCheck();
            }

            if (!SIO.File.Exists(SMConst.settingsFile))
            {
                using (SIO.StreamWriter sw = new SIO.StreamWriter(SMConst.settingsFile, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine("[デッド品管理自店舗]=");
                    sw.WriteLine("[デッド品管理対象店舗]=");
                    sw.WriteLine("[全店舗リスト]=");

                    sw.Flush();

                }
            }

            if (!SIO.File.Exists(SMConst.VersionDatLocalPath))
            {
                MessageBoxTop.Show("Version.datファイルが存在しません、プログラムを終了します。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }



        /// <summary>
        /// 使用量.CSVは代替区分で使用量がわかれるので、医薬品コードごとにまとめる。
        /// </summary>
        /// <param name="uaList"></param>
        /// <returns></returns>
        public static List<UsedAmountEntity> 合計使用量へGroupBy(List<UsedAmountEntity> uaList)
        {
            var gb = (from x in uaList

                      group x by new
                         {
                             x.Code,
                             //x.Name,
                             x.StoreName,
                             x.UsedDate,
                             //x.Name2,
                         } into grouping
                      select new UsedAmountEntity
                      {
                          Code = grouping.Key.Code,
                          Name = null, //この名前は使わない。
                          Name2 = null,//この名前は使わない。
                          Price = grouping.Max(s => s.Price),
                          StoreName = grouping.Key.StoreName,
                          UsedDate = grouping.Key.UsedDate,
                          UsedAmount = grouping.Sum(s => s.UsedAmount)
                      }).ToList();



            //// 代替・非代替を統合
            //var 代替・非代替を統合 =
            //                        (from x in uaEntList
            //                         group x by new
            //                         {
            //                             x.StoreName,
            //                             x.UsedDate,
            //                             x.Code,
            //                             x.Name,
            //                             x.Name2,
            //                             //x.使用量,
            //                             x.Price
            //                         } into grouping
            //                         select new UsedAmountEntity
            //                         {
            //                             StoreName = grouping.Key.StoreName,
            //                             UsedDate = grouping.Key.UsedDate,
            //                             Code = grouping.Key.Code,
            //                             Name = grouping.Key.Name,
            //                             Name2 = grouping.Key.Name2,
            //                             Price = grouping.Key.Price,
            //                             UsedAmount = grouping.Sum(p => p.UsedAmount)
            //                         }).ToList();

            return gb;


        }

        public static List<UsedAmountEntity> 使用量CSVLoader(string FilePath)
        {

            List<UsedAmountEntity> uaEntList = new List<UsedAmountEntity>();

            using (SIO.StreamReader sr = new SIO.StreamReader(FilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {

                    // 出力形式
                    // [0] 店舗名
                    // [1] 使用年月日
                    // [2] 商品コード
                    // [3] 医薬品名
                    // [4] 名称２
                    // [5] 使用量
                    // [6] 薬価
                    // [7] 後発品区分
                    // [8] 代替区分

                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }



                    UsedAmountEntity ent = new UsedAmountEntity();
                    var sepa = line.Split(',');


                    //旧使用量の場合はエラー
                    if (sepa.Count() < 9)
                    {
                        //MessageBox.Show("旧使用量ファイル(在庫使用量.CSV)がアップロードされている可能性があります。\r\n管理者へご連絡お願いします。","エラー",MessageBoxButton.OK,MessageBoxImage.Error);
                        throw new Exception("旧使用量ファイル(在庫使用量.CSV)がアップロードされている可能性があります。\r\n管理者へご連絡お願いします。");
                    }


                    ent.StoreName = sepa[0];


                    int year;
                    if (int.TryParse(sepa[1].Substring(0, 4), out year) == false)
                    {
                        continue;
                    }
                    int month;
                    if (int.TryParse(sepa[1].Substring(5, sepa[1].Length == 7 ? 1 : 2), out month) == false)
                    {
                        continue;
                    }


                    ent.UsedDate = new DateTime(year, month, 1); // １日を指定とする

                    //現在庫や不動品が医薬品コード９桁なのでそっちに合わせる必要あり。
                    ent.Code = sepa[2].Substring(0, 9);
                    //ent.Code = sepa[2];

                    ent.Name = sepa[3];

                    double dResult;
                    if (double.TryParse(sepa[5], out dResult) == false)
                    {
                        continue;
                    }
                    ent.UsedAmount = dResult;


                    double dResult2;
                    if (double.TryParse(sepa[6], out dResult2) == false)
                    {
                        ent.Price = 0; // 変換できない場合は、薬価0とする
                        //continue;
                    }

                    ent.Price = dResult2;

                    // 現時点でGroupByでGroup項目に入らない為、使わない。勝手にFalseになるため。
                    //ent.後発品区分 = sepa[7] == "1" ? true : false;
                    //ent.代替区分 = sepa[8] == "1" ? true : false; 

                    uaEntList.Add(ent);
                }
            }

            var 代替・非代替を統合 = 合計使用量へGroupBy(uaEntList);



            return 代替・非代替を統合;
            //return uaEntList;


        }


        public static List<ExpStockEntity> 現在庫CSVLoader(string FilePath, int 期限切迫設定期間月)
        {
            List<ExpStockEntity> loadlist = new List<ExpStockEntity>();

            using (SIO.StreamReader sr = new SIO.StreamReader(FilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }
                    //0店舗名
                    //1薬品コード
                    //2薬品名
                    //3在庫数
                    //4使用期限
                    //5薬価
                    //6メーカー名
                    //7後発区分
                    //8名称２
                    ExpStockEntity ent = new ExpStockEntity();

                    var sepa = line.Split(',');

                    ent.StoreName = sepa[0];
                    ent.Code = sepa[1].Substring(0, 9);
                    ent.Name = sepa[2];

                    double dResult;
                    if (double.TryParse(sepa[3], out dResult) == false)
                    {
                        continue;
                    }
                    ent.StockAmount = dResult;

                    DateTime dateResult;
                    if (DateTime.TryParse(sepa[4], out dateResult) == false)
                    {
                        continue;
                    }

                    if (SMConst.deadStockMaxDate1 < dateResult)
                    {
                        continue;
                    }

                    ent.ExpireDate = dateResult.AddMonths(1).AddDays(-1);

                    // 期限切迫品でかつ期限できれていないもののみ抽出
                    if (DateTime.Now.AddMonths(期限切迫設定期間月) < ent.ExpireDate || ent.ExpireDate < DateTime.Now)
                    {
                        continue;
                    }

                    if (ent.ExpireDate < SMConst.deadStockMinDate || SMConst.deadStockMaxDate1 < ent.ExpireDate)
                    {
                        continue;
                    }



                    double dResult2;
                    if (double.TryParse(sepa[5], out dResult2) == false)
                    {
                        continue;
                    }

                    ent.Price = dResult2;

                    if (9 <= sepa.Count())
                    {
                        ent.Name2 = sepa[8];
                    }
                    else
                    {
                        ent.Name2 = "";
                    }

                    loadlist.Add(ent);
                }
            }

            //// 除外医薬品
            //// 現在は除外対象はデッド品貰い受け依頼書のみとしている
            //var excluded = (from x in loadlist
            //                join y in InitialData.ExceptiveMedicinesList
            //                on
            //                     x.Code equals y.レセプト電算コード
            //                select x).ToList();

            //return loadlist.Except(excluded).ToList();
            return loadlist;

        }



        /// <summary>
        /// 不動品CSVファイルを読み込んでEntityに格納
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="除外対象品を除外するか">デッド品貰い受け依頼の場合は除外対象を行う為のフラグ</param>
        /// <returns></returns>
        public static List<DeadStockEntity> 不動品CSVLoader(string FilePath, bool 除外対象品を除外するか)
        {
            List<DeadStockEntity> loadlist = new List<DeadStockEntity>();

            using (SIO.StreamReader sr = new SIO.StreamReader(FilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    DeadStockEntity ent = new DeadStockEntity();
                    var sepa = line.Split(',');

                    ent.StoreName = sepa[0];
                    ent.Code = sepa[1].Substring(0, 9);
                    ent.Name = sepa[2];

                    double dResult;
                    if (double.TryParse(sepa[3], out dResult) == false)
                    {
                        continue;
                    }

                    ent.StockAmount = dResult;

                    DateTime dateResult;
                    if (DateTime.TryParse(sepa[4], out dateResult) == false)
                    {
                        ent.ExpireDate = Const.SMConst.deadStockMaxDate2; // 変換できない場合は、9999/12/31とする。
                        //continue;
                    }
                    else
                    {
                        ent.ExpireDate = dateResult;
                    }

                    double dResult2;
                    if (double.TryParse(sepa[5], out dResult2) == false)
                    {
                        ent.Price = 0; // 変換できない場合は、薬価0とする
                        //continue;
                    }
                    else
                    {
                        ent.Price = dResult2;
                    }

                    double dResult3;
                    if (double.TryParse(sepa[6], out dResult3) == false)
                    {
                        ent.OneDoseAmount = dResult3;
                    }
                    else
                    {
                        ent.OneDoseAmount = 0;
                    }

                    ent.Name2 = sepa[7];


                    loadlist.Add(ent);

                }
            }

            if (除外対象品を除外するか)
            {

                // 除外医薬品
                var excluded = (from x in loadlist
                                join y in InitialData.ExceptiveMedicinesList
                                on
                                     x.Code equals y.レセプト電算コード
                                select x).ToList();

                return loadlist.Except(excluded).ToList();
            }
            else
            {
                return loadlist;
            }
        }

    }
}
