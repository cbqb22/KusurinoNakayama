using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIO = System.IO;
using System.Windows;
using System.ComponentModel;
using StockManagement.Const;
using StockManagement.Settings;
using StockManagement.ViewModel.Excel;
using StockManagement.ViewModel.DSException;
using Office11 = ExcelControllerOffice11;
using StockManagement.ViewModel.Common.MessageBox;
using StockManagement.ViewModel.IO;




namespace StockManagement.ViewModel
{
    public class ExpStockOrderRoutines
    {
        private string _folderDateStr;
        private DateTime _from;
        private DateTime _to;
        private string _baseStoreName;
        private List<string> _applyStoreList;
        private BackgroundWorker _controlledBackgroundWorker;
        private ExcelTypeEnum _excelType;
        private bool _autoPrint;
        private bool _IsCancel;
        private DateTime _folderDate;
        private int _期限切迫設定期間月 = 6;

        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; }
        }

        public ExpStockOrderRoutines(DateTime from, DateTime to, string folderDate, BackgroundWorker worker, ExcelTypeEnum excelType, bool autoPrint, DateTime folderdate, string baseStoreName)
        {
            _folderDateStr = folderDate;
            _from = from;
            _to = to;
            _baseStoreName = baseStoreName;

            // 自店舗と管理対象店舗のみダウンロードする
            var ret = new List<string>();
            ret.Add(baseStoreName); // 自店舗
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
            _applyStoreList = ret; // 自店舗 + 管理対象店舗
            //_applyStoreList = InitialData.DeadMangementSourceStoreAndDeadStockManagementStoresList; // 自店舗 + 管理対象店舗
            _controlledBackgroundWorker = worker;
            _excelType = excelType;
            _autoPrint = autoPrint;
            _folderDate = folderdate;
            _期限切迫設定期間月 = InitialData.ExpireRange;
        }


        public bool CheckBackgroundWorkerCanncelation()
        {
            // バックグラウンドワーカーのキャンセル処理をチェック
            if (_controlledBackgroundWorker != null)
            {
                if (_controlledBackgroundWorker.CancellationPending)
                {
                    IsCancel = true;
                    return false;
                }
            }

            return true;

        }


        public bool DoWork()
        {
            _controlledBackgroundWorker.ReportProgress(0);  // 0% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;


            // ①依頼元店舗で現在庫を読込
            if (_applyStoreList.Contains(_baseStoreName) == false)
            {
                MessageBoxTop.Show("依頼元店舗名が設定ファイルに存在しないため、エラーが発生しました。処理を中断します。");
                return false;
            }

            string expFilePath = SIO.Path.Combine(SMConst.downloadFolder, string.Format(@"{0}\{1}\{2}", _folderDateStr, StockManagement.Common.Settings.LocalStockFolderName, StockManagement.Common.Settings.LocalStockCsvFileName));
            List<ExpStockEntity> expEntList = FileController.現在庫CSVLoader(expFilePath, _期限切迫設定期間月);

            _controlledBackgroundWorker.ReportProgress(10);  // 10% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;



            // 他店舗の使用実績を読み込み
            List<UsedAmountEntity> uaEntList = new List<UsedAmountEntity>();

            // 自店舗の使用実績のみ
            List<UsedAmountEntity> uaEntListOwn = new List<UsedAmountEntity>();
            foreach (var othershop in _applyStoreList)
            {

                for (DateTime d = _from; d <= _to; d = d.AddMonths(1))
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return false;


                    //ダウンロードしたファイルの保存先
                    string makeFoler = SIO.Path.Combine(SMConst.downloadFolder, _folderDateStr);
                    if (!SIO.Directory.Exists(makeFoler))
                    {
                        MessageBoxTop.Show("ダウンロードしたファイルを格納する日付フォルダ存在しない為、処理を中断しました。");
                        return false;
                    }

                    string usedFoler = SIO.Path.Combine(makeFoler, "使用量");

                    string shopFolder = SIO.Path.Combine(usedFoler, othershop);
                    if (!SIO.Directory.Exists(shopFolder))
                    {
                        MessageBoxTop.Show("ダウンロードしたファイルを格納する店舗フォルダ存在しない為、処理を中断しました。");
                        return false;
                    }
                    string yearFolder = SIO.Path.Combine(shopFolder, string.Format("{0}年", d.Year));
                    if (!SIO.Directory.Exists(yearFolder))
                    {
                        MessageBoxTop.Show("ダウンロードしたファイルを格納する使用年フォルダ存在しない為、処理を中断しました。");
                        return false;
                    }

                    string downFile = SIO.Path.Combine(yearFolder, string.Format("{0}月.csv", d.Month));


                    if (!SIO.File.Exists(downFile))
                    {
                        continue;
                    }


                    //string usedFilePath = Path.Combine(SMConst.downloadFolder, string.Format(@"{0}\使用量\{1}\{2}年\{3}月.csv", _folderDate, _baseStoreName, d.Year, d.Month));
                    var list = FileController.使用量CSVLoader(downFile);

                    // 自店と他店舗を分ける
                    if (othershop == _baseStoreName)
                    {
                        uaEntListOwn = uaEntListOwn.Concat(list).ToList();
                    }
                    else
                    {
                        uaEntList = uaEntList.Concat(list).ToList();

                    }


                }
            }

            uaEntList = FileController.合計使用量へGroupBy(uaEntList);



            _controlledBackgroundWorker.ReportProgress(20);  // 20% 


            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;

            List<ExpStockOrderDetailEntity> outList = new List<ExpStockOrderDetailEntity>();


            // ③期限切迫品のうち、他店実績のあるものを抽出

            int AllNoCounter = 1;
            foreach (var row in expEntList)
            {
                var result = (from x in uaEntList
                              where
                                 x.Code == row.Code &&
                                 x.StoreName != _baseStoreName
                              group x by new
                              {
                                  x.StoreName,
                                  x.Code
                              } into g
                              join j in expEntList
                              on
                                  g.Key.Code equals j.Code
                              join y in uaEntListOwn //自店舗使用量合計用
                              on
                                  g.Key.Code equals y.Code
                            　into g2
                              from own in g2.DefaultIfEmpty()
                              orderby j.Name
                              select new ExpStockOrderDetailEntity
                              {
                                  No = AllNoCounter++,
                                  Name = j.Name + (j.Name2 != "" ? " / " + j.Name2 : ""),  //名称２の対応
                                  TotalUsedAmount = g.Sum(x => x.UsedAmount),
                                  Acceptable = false,
                                  ExpireDate = j.ExpireDate,
                                  DeadAmount = j.StockAmount,
                                  PartnerStoreName = g.Key.StoreName,
                                  TotalUsedAmountOwn = (own == null ? 0 : g2.Sum(x => x.UsedAmount))
                              }).ToList();

                outList = outList.Concat(result).Distinct(new ExpStockOrderDetailEntityComparer()).ToList();

            }

            int i = outList.Count;

            System.Diagnostics.Debug.WriteLine(i);
            _controlledBackgroundWorker.ReportProgress(40);  // 40% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;


            //店舗ごとにシートを作成
            if (_excelType == ExcelTypeEnum.Excel2003)
            {
                Office11.ExcelControllerForExpStock ec = new Office11.ExcelControllerForExpStock();
                if (!ec.CreatExcel())
                {
                    MessageBoxTop.Show("ExcelCreate中にエラーが発生した為、処理を中断します。");
                    return false;
                }

                var ex = ExcelWork(outList, ec);
                if (ex != null)
                {
                    MessageBoxTop.Show("処理が中断されました。\r\n" + ex.Message + ex.StackTrace);
                    return false;
                }
            }
            else if (_excelType == ExcelTypeEnum.Excel2007)
            {
                ExcelControllerForExpStock ec = new ExcelControllerForExpStock();
                if (!ec.CreatExcel())
                {
                    return false;
                }
                var ex = ExcelWork(outList, ec);
                if (ex != null)
                {
                    MessageBoxTop.Show("処理が中断されました。\r\n" + ex.Message + ex.StackTrace);
                    return false;
                }
            }

            return true;


        }

        #region 普通参照用
        /// <summary>
        /// 2003への出力
        /// </summary>
        /// <param name="outList"></param>
        /// <param name="ec"></param>
        /// <returns></returns>
        public Exception ExcelWork(List<ExpStockOrderDetailEntity> outList, Office11.ExcelControllerForExpStock ec)
        {
            try
            {
                // 相手先店舗分だけSheetの追加
                int totalSheetCount = 0;
                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");


                    // 自店舗はcontinue
                    if (shop == _baseStoreName)
                    {
                        continue;
                    }

                    totalSheetCount++;
                }
                ec.AddSheet(totalSheetCount);

                int counter2 = 0;
                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");

                    // 自店舗はcontinue
                    if (shop == _baseStoreName)
                    {
                        continue;
                    }
                    counter2++;

                    ec.RenameSheet(shop, counter2);
                }



                int storeCounter = 0;
                int totalInsert = 0;
                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");

                    // 自店舗はcontinue
                    if (shop == _baseStoreName)
                    {
                        continue;
                    }

                    storeCounter++;
                    //ec.AddSheet(shop, storeCounter == 1 ? true :false);

                    ec.SetBasicalForm(storeCounter, _folderDate,
                                shop, _baseStoreName,
                                _from, _to,
                                DateTime.Now);

                    int insertCount = 1;


                    var selectList = (from x in outList
                                      where x.PartnerStoreName == shop
                                      select new ExpStockOrderDetailEntity
                                      {
                                          No = x.No,
                                          Name = x.Name,
                                          TotalUsedAmount = x.TotalUsedAmount,
                                          TotalUsedAmountOwn = x.TotalUsedAmountOwn,
                                          Acceptable = x.Acceptable,
                                          ExpireDate = x.ExpireDate,
                                          DeadAmount = x.DeadAmount,
                                          PartnerStoreName = x.PartnerStoreName

                                      }).ToList();

                    foreach (var dead in selectList)
                    {
                        // バックグラウンドワーカーのキャンセル処理をチェック
                        if (!CheckBackgroundWorkerCanncelation())
                            return new DeadStockException(true, "処理を中断しました。");

                        if (dead.PartnerStoreName == shop)
                        {
                            ec.InsertTableData(storeCounter, insertCount, dead.Name, dead.TotalUsedAmount,dead.DeadAmount,dead.ExpireDate,dead.Acceptable);
                            insertCount++;
                            totalInsert++;
                        }
                    }

                    int ProgressValue = outList.Count == 0 ? 0 : 50 * totalInsert / outList.Count;
                    _controlledBackgroundWorker.ReportProgress(40 + ProgressValue);  // 40% + 進行度 

                }


                _controlledBackgroundWorker.ReportProgress(90);  // 90% 

                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return new DeadStockException(true, "処理を中断しました。");


                // PrintOut
                if (_autoPrint)
                {
                    ec.PrintOut(0);
                }

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputExpOrderFolder, string.Format("期限切迫品依頼書{0}", _folderDateStr));
                ec.CloseExcel(true, ExcelBookFileName);


                //System.Windows.MessageBoxTop.Show( "DeadStock:" +dsEntList.Count + "個\r\n" + "使用量:" + uaEntList.Count + "個");

                _controlledBackgroundWorker.ReportProgress(100);  // 100% 


            }
            catch (DeadStockException dsex)
            {
                return dsex;
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;

        }

        /// <summary>
        /// 2007への出力
        /// </summary>
        /// <param name="outList"></param>
        /// <param name="ec"></param>
        /// <returns></returns>
        private Exception ExcelWork(List<ExpStockOrderDetailEntity> outList, ExcelControllerForExpStock ec)
        {

            try
            {

                // 相手先店舗分だけSheetの追加
                int totalSheetCount = 0;
                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");


                    // 自店舗はcontinue
                    if (shop == _baseStoreName)
                    {
                        continue;
                    }

                    totalSheetCount++;
                }
                ec.AddSheet(totalSheetCount);

                int counter2 = 0;

                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");

                    // 自店舗はcontinue
                    if (shop == _baseStoreName)
                    {
                        continue;
                    }
                    counter2++;

                    ec.RenameSheet(shop, counter2);
                }



                int storeCounter = 0;
                int totalInsert = 0;
                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");

                    // 自店舗はcontinue
                    if (shop == _baseStoreName)
                    {
                        continue;
                    }

                    storeCounter++;
                    //ec.AddSheet(shop, storeCounter == 1 ? true :false);

                    ec.SetBasicalForm(storeCounter, _folderDate,
                                shop, _baseStoreName,
                                _from, _to,
                                DateTime.Now);

                    int insertCount = 1;


                    var selectList = (from x in outList
                                      where x.PartnerStoreName == shop
                                      select new ExpStockOrderDetailEntity
                                      {
                                          No = x.No,
                                          Name = x.Name,
                                          TotalUsedAmount = x.TotalUsedAmount,
                                          TotalUsedAmountOwn = x.TotalUsedAmountOwn,
                                          Acceptable = x.Acceptable,
                                          ExpireDate = x.ExpireDate,
                                          DeadAmount = x.DeadAmount,
                                          PartnerStoreName = x.PartnerStoreName

                                      }).ToList();

                    foreach (var dead in selectList)
                    {
                        // バックグラウンドワーカーのキャンセル処理をチェック
                        if (!CheckBackgroundWorkerCanncelation())
                            return new DeadStockException(true, "処理を中断しました。");

                        if (dead.PartnerStoreName == shop)
                        {
                            ec.InsertTableData(storeCounter, insertCount, dead.Name, dead.TotalUsedAmount, dead.DeadAmount, dead.ExpireDate, dead.Acceptable);
                            insertCount++;
                            totalInsert++;
                        }
                    }


                    int ProgressValue = outList.Count == 0 ? 0 :　50 * totalInsert / outList.Count;
                    _controlledBackgroundWorker.ReportProgress(40 + ProgressValue);  // 40% + 進行度 

                }


                _controlledBackgroundWorker.ReportProgress(90);  // 90% 

                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return new DeadStockException(true, "処理を中断しました。");


                // PrintOut
                if (_autoPrint)
                {
                    ec.PrintOut(0);
                }

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputExpOrderFolder, string.Format("期限切迫品依頼書{0}", _folderDateStr));
                ec.CloseExcel(true, ExcelBookFileName);


                //System.Windows.MessageBoxTop.Show( "DeadStock:" +dsEntList.Count + "個\r\n" + "使用量:" + uaEntList.Count + "個");

                _controlledBackgroundWorker.ReportProgress(100);  // 100% 


            }
            catch (DeadStockException dsex)
            {
                return dsex;
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;

        }

        #endregion
        #region 相対参照用

        ///// <summary>
        ///// 2003への出力
        ///// </summary>
        ///// <param name="outList"></param>
        ///// <param name="ec"></param>
        ///// <returns></returns>
        //public Exception ExcelWork(List<ExpStockOrderDetailEntity> outList, Office11.ExcelControllerForExpStock ec)
        //{
        //    try
        //    {
        //        //ALL用の相対参照先List
        //        var outlistForAllSheet = outList.Distinct(new ExpStockOrderDetailEntityComparerByOnlyNameDeadAmountExpireDateAcceptable()).ToList();

        //        // 相手先店舗分だけSheetの追加
        //        int totalSheetCount = 0;
        //        //ALLシート分
        //        totalSheetCount++;
        //        foreach (var shop in _applyStoreList)
        //        {
        //            // バックグラウンドワーカーのキャンセル処理をチェック
        //            if (!CheckBackgroundWorkerCanncelation())
        //                return new DeadStockException(true, "処理を中断しました。");


        //            // 自店舗はcontinue
        //            if (shop == _baseStoreName)
        //            {
        //                continue;
        //            }

        //            totalSheetCount++;
        //        }
        //        ec.AddSheet(totalSheetCount);

        //        int counter2 = 0;
        //        //ALLシート追加
        //        counter2++;
        //        ec.RenameSheet("ALL", counter2);

        //        //ALLシート基本情報
        //        ec.SetBasicalForm(1);
        //        int AllNo = 1;
        //        foreach (var d in outlistForAllSheet)
        //        {
        //            // AllNoを振りなおす
        //            // 重複がある状態のNoを引き継いでいるため
        //            d.No = AllNo;
        //            ec.InsertTableData(1, d.No, d.Name, d.DeadAmount, d.ExpireDate, false);
        //            AllNo++;
        //        }

        //        foreach (var shop in _applyStoreList)
        //        {
        //            // バックグラウンドワーカーのキャンセル処理をチェック
        //            if (!CheckBackgroundWorkerCanncelation())
        //                return new DeadStockException(true, "処理を中断しました。");

        //            // 自店舗はcontinue
        //            if (shop == _baseStoreName)
        //            {
        //                continue;
        //            }
        //            counter2++;

        //            ec.RenameSheet(shop, counter2);
        //        }



        //        int storeCounter = 0;
        //        int totalInsert = 0;

        //        //ALLシート分
        //        storeCounter++;
        //        foreach (var shop in _applyStoreList)
        //        {
        //            // バックグラウンドワーカーのキャンセル処理をチェック
        //            if (!CheckBackgroundWorkerCanncelation())
        //                return new DeadStockException(true, "処理を中断しました。");

        //            // 自店舗はcontinue
        //            if (shop == _baseStoreName)
        //            {
        //                continue;
        //            }

        //            storeCounter++;
        //            //ec.AddSheet(shop, storeCounter == 1 ? true :false);

        //            ec.SetBasicalForm(storeCounter, _folderDate,
        //                        shop, _baseStoreName,
        //                        _from, _to,
        //                        DateTime.Now);

        //            int insertCount = 1;


        //            var selectList = (from x in outList
        //                              join y in outlistForAllSheet
        //                                  on new { a = x.Name, b = x.DeadAmount, c = x.ExpireDate, d = x.Acceptable } equals new { a = y.Name, b = y.DeadAmount, c = y.ExpireDate, d = y.Acceptable }
        //                              where x.PartnerStoreName == shop
        //                              select new ExpStockOrderDetailEntity
        //                              {
        //                                  //No = x.No,
        //                                  Name = x.Name,
        //                                  TotalUsedAmount = x.TotalUsedAmount,
        //                                  TotalUsedAmountOwn = x.TotalUsedAmountOwn,
        //                                  Acceptable = x.Acceptable,
        //                                  ExpireDate = x.ExpireDate,
        //                                  DeadAmount = x.DeadAmount,
        //                                  PartnerStoreName = x.PartnerStoreName,
        //                                  相対参照先RowNo = y.No + 8 // 9行目から始まるので+8

        //                              }).ToList();

        //            foreach (var dead in selectList)
        //            {
        //                // バックグラウンドワーカーのキャンセル処理をチェック
        //                if (!CheckBackgroundWorkerCanncelation())
        //                    return new DeadStockException(true, "処理を中断しました。");

        //                if (dead.PartnerStoreName == shop)
        //                {
        //                    ec.InsertTableData(storeCounter, insertCount,(dead.TotalUsedAmount < dead.TotalUsedAmountOwn), dead.相対参照先へ変換("B", dead.相対参照先RowNo), dead.TotalUsedAmount.ToString(),dead.TotalUsedAmountOwn.ToString(), dead.相対参照先へ変換("C", dead.相対参照先RowNo), dead.相対参照先へ変換("D", dead.相対参照先RowNo), dead.相対参照先へ変換("E", dead.相対参照先RowNo));
        //                    insertCount++;
        //                    totalInsert++;
        //                }
        //            }

        //            ////ALLシートへの参照テスト
        //            //ec.InsertTableData(storeCounter, insertCount, "=ALL!B2", "=ALL!C2", "=ALL!D2", "=ALL!E2", "=ALL!F2");


        //              int ProgressValue = outList.Count == 0 ? 0 : 50 * totalInsert / outList.Count;
        //            _controlledBackgroundWorker.ReportProgress(40 + ProgressValue);  // 40% + 進行度 

        //        }


        //        _controlledBackgroundWorker.ReportProgress(90);  // 90% 

        //        // バックグラウンドワーカーのキャンセル処理をチェック
        //        if (!CheckBackgroundWorkerCanncelation())
        //            return new DeadStockException(true, "処理を中断しました。");


        //        // PrintOut
        //        if (_autoPrint)
        //        {
        //            ec.PrintOut(0);
        //        }

        //        string ExcelBookFileName = SIO.Path.Combine(SMConst.outputExpOrderFolder, string.Format("期限切迫品依頼書{0}", _folderDateStr));
        //        ec.CloseExcel(true, ExcelBookFileName);


        //        //System.Windows.MessageBoxTop.Show( "DeadStock:" +dsEntList.Count + "個\r\n" + "使用量:" + uaEntList.Count + "個");

        //        _controlledBackgroundWorker.ReportProgress(100);  // 100% 


        //    }
        //    catch (DeadStockException dsex)
        //    {
        //        return dsex;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex;
        //    }

        //    return null;

        //}

        ///// <summary>
        ///// 2007への出力
        ///// </summary>
        ///// <param name="outList"></param>
        ///// <param name="ec"></param>
        ///// <returns></returns>
        //private Exception ExcelWork(List<ExpStockOrderDetailEntity> outList, ExcelControllerForExpStock ec)
        //{

        //    try
        //    {
        //        //ALL用の相対参照先List
        //        var outlistForAllSheet = outList.Distinct(new ExpStockOrderDetailEntityComparerByOnlyNameDeadAmountExpireDateAcceptable()).ToList();

        //        // 相手先店舗分だけSheetの追加
        //        int totalSheetCount = 0;
        //        //ALLシート分
        //        totalSheetCount++;
        //        foreach (var shop in _applyStoreList)
        //        {
        //            // バックグラウンドワーカーのキャンセル処理をチェック
        //            if (!CheckBackgroundWorkerCanncelation())
        //                return new DeadStockException(true, "処理を中断しました。");


        //            // 自店舗はcontinue
        //            if (shop == _baseStoreName)
        //            {
        //                continue;
        //            }

        //            totalSheetCount++;
        //        }
        //        ec.AddSheet(totalSheetCount);

        //        int counter2 = 0;
        //        //ALLシート追加
        //        counter2++;
        //        ec.RenameSheet("ALL", counter2);

        //        //ALLシート基本情報
        //        ec.SetBasicalForm(1);
        //        int AllNo = 1;
        //        foreach (var d in outlistForAllSheet)
        //        {
        //            // AllNoを振りなおす
        //            // 重複がある状態のNoを引き継いでいるため
        //            d.No = AllNo;
        //            ec.InsertTableData(1, d.No, d.Name, d.DeadAmount, d.ExpireDate, false);
        //            AllNo++;
        //        }

        //        foreach (var shop in _applyStoreList)
        //        {
        //            // バックグラウンドワーカーのキャンセル処理をチェック
        //            if (!CheckBackgroundWorkerCanncelation())
        //                return new DeadStockException(true, "処理を中断しました。");

        //            // 自店舗はcontinue
        //            if (shop == _baseStoreName)
        //            {
        //                continue;
        //            }
        //            counter2++;

        //            ec.RenameSheet(shop, counter2);
        //        }



        //        int storeCounter = 0;
        //        int totalInsert = 0;

        //        //ALLシート分
        //        storeCounter++;
        //        foreach (var shop in _applyStoreList)
        //        {
        //            // バックグラウンドワーカーのキャンセル処理をチェック
        //            if (!CheckBackgroundWorkerCanncelation())
        //                return new DeadStockException(true, "処理を中断しました。");

        //            // 自店舗はcontinue
        //            if (shop == _baseStoreName)
        //            {
        //                continue;
        //            }

        //            storeCounter++;
        //            //ec.AddSheet(shop, storeCounter == 1 ? true :false);

        //            ec.SetBasicalForm(storeCounter, _folderDate,
        //                        shop, _baseStoreName,
        //                        _from, _to,
        //                        DateTime.Now);

        //            int insertCount = 1;


        //            var selectList = (from x in outList
        //                              join y in outlistForAllSheet
        //                                  on new { a = x.Name, b = x.DeadAmount, c = x.ExpireDate, d = x.Acceptable } equals new { a = y.Name, b = y.DeadAmount, c = y.ExpireDate, d = y.Acceptable }
        //                              where x.PartnerStoreName == shop
        //                              select new ExpStockOrderDetailEntity
        //                              {
        //                                  //No = x.No,
        //                                  Name = x.Name,
        //                                  TotalUsedAmount = x.TotalUsedAmount,
        //                                  TotalUsedAmountOwn = x.TotalUsedAmountOwn,
        //                                  Acceptable = x.Acceptable,
        //                                  ExpireDate = x.ExpireDate,
        //                                  DeadAmount = x.DeadAmount,
        //                                  PartnerStoreName = x.PartnerStoreName,
        //                                  相対参照先RowNo = y.No + 8 // 9行目から始まるので+8

        //                              }).ToList();

        //            foreach (var dead in selectList)
        //            {
        //                // バックグラウンドワーカーのキャンセル処理をチェック
        //                if (!CheckBackgroundWorkerCanncelation())
        //                    return new DeadStockException(true, "処理を中断しました。");

        //                if (dead.PartnerStoreName == shop)
        //                {
        //                    ec.InsertTableData(storeCounter, insertCount, (dead.TotalUsedAmount < dead.TotalUsedAmountOwn), dead.相対参照先へ変換("B", dead.相対参照先RowNo), dead.TotalUsedAmount.ToString(), dead.TotalUsedAmountOwn.ToString(), dead.相対参照先へ変換("C", dead.相対参照先RowNo), dead.相対参照先へ変換("D", dead.相対参照先RowNo), dead.相対参照先へ変換("E", dead.相対参照先RowNo));
        //                    insertCount++;
        //                    totalInsert++;
        //                }
        //            }

        //            ////ALLシートへの参照テスト
        //            //ec.InsertTableData(storeCounter, insertCount, "=ALL!B2", "=ALL!C2", "=ALL!D2", "=ALL!E2", "=ALL!F2");


        //          int ProgressValue = outList.Count == 0 ? 0 : 50 * totalInsert / outList.Count;
        //            _controlledBackgroundWorker.ReportProgress(40 + ProgressValue);  // 40% + 進行度 

        //        }


        //        _controlledBackgroundWorker.ReportProgress(90);  // 90% 

        //        // バックグラウンドワーカーのキャンセル処理をチェック
        //        if (!CheckBackgroundWorkerCanncelation())
        //            return new DeadStockException(true, "処理を中断しました。");


        //        // PrintOut
        //        if (_autoPrint)
        //        {
        //            ec.PrintOut(0);
        //        }

        //        string ExcelBookFileName = SIO.Path.Combine(SMConst.outputExpOrderFolder, string.Format("期限切迫品依頼書{0}", _folderDateStr));
        //        ec.CloseExcel(true, ExcelBookFileName);


        //        //System.Windows.MessageBoxTop.Show( "DeadStock:" +dsEntList.Count + "個\r\n" + "使用量:" + uaEntList.Count + "個");

        //        _controlledBackgroundWorker.ReportProgress(100);  // 100% 


        //    }
        //    catch (DeadStockException dsex)
        //    {
        //        return dsex;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex;
        //    }

        //    return null;

        //}

        #endregion
    }
}

