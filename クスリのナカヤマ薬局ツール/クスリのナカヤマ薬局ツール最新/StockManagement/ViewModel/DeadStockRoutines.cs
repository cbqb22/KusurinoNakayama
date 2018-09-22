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
    public class DeadStockRoutines
    {
        private string _folderDateStr;
        private DateTime _from;
        private DateTime _to;
        private string _baseStoreName;
        private List<string> _applyStoreList;
        private BackgroundWorker _controlledBackgroundWorker;
        private ExcelTypeEnum _excelType;
        private bool _autoPrint;
        private DateTime _folderDate;

        public DeadStockRoutines(DateTime from, DateTime to, string folderDate, BackgroundWorker worker, ExcelTypeEnum excelType, bool autoPrint, DateTime folderdate, string baseStoreName)
        {
            _folderDateStr = folderDate;
            _from = from;
            _to = to;
            _baseStoreName = baseStoreName;

            // 自店舗とデッド品管理対象店舗のみダウンロードする
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
            _applyStoreList = ret; // 自店舗 + デッド品管理対象店舗
            //_applyStoreList = InitialData.DeadMangementSourceStoreAndDeadStockManagementStoresList; // 自店舗 + デッド品管理対象店舗
            _controlledBackgroundWorker = worker;
            _excelType = excelType;
            _autoPrint = autoPrint;
            _folderDate = folderdate;
        }

        public bool CheckBackgroundWorkerCanncelation()
        {
            // バックグラウンドワーカーのキャンセル処理をチェック
            if (_controlledBackgroundWorker != null)
            {
                if (_controlledBackgroundWorker.CancellationPending)
                {
                    return false;
                }
            }

            return true;

        }

        public Exception DoWork()
        {
            _controlledBackgroundWorker.ReportProgress(0);  // 0% 

            if (!CheckBackgroundWorkerCanncelation())
                return new DeadStockException(true,"処理を中断しました。");


            // 依頼元店舗名のデッド品リストを読込
            
            string deadFilePath = SIO.Path.Combine(SMConst.downloadFolder, string.Format(@"{0}\不動品\{1}\不動品データ.csv", _folderDateStr, _baseStoreName));

            // 依頼書は不動品ファイルがなければエラー
            if (!SIO.File.Exists(deadFilePath))
            {
                throw new DeadStockException(false, string.Format("ダウンロードした次の不動品ファイルが存在しない為、処理を中断しました。\r\n見つからなかったファイル：\r\n{0}",deadFilePath));
            }

            List<DeadStockEntity> dsEntList = FileController.不動品CSVLoader(deadFilePath,true);


            _controlledBackgroundWorker.ReportProgress(10);  // 10% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return new DeadStockException(true, "処理を中断しました。");

            // 依頼先店舗で使用量を読込
            List<UsedAmountEntity> uaEntList = new List<UsedAmountEntity>();
            foreach (var shop in _applyStoreList)
            {
                // 貰い受け依頼書では自店の使用量は使用しない。
                if (shop == _baseStoreName)
                {
                    continue;
                }

                for (DateTime d = _from; d <= _to; d = d.AddMonths(1))
                {

                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new DeadStockException(true, "処理を中断しました。");

                    //ダウンロードしたファイルの保存先
                    string makeFoler = SIO.Path.Combine(SMConst.downloadFolder, _folderDateStr);
                    if (!SIO.Directory.Exists(makeFoler))
                    {
                        return new DeadStockException(false, "処理を中断しました。");
                    }

                    string usedFoler = SIO.Path.Combine(makeFoler, "使用量");

                    string shopFolder = SIO.Path.Combine(usedFoler, shop);
                    if (!SIO.Directory.Exists(shopFolder))
                    {
                        return new DeadStockException(false, "ダウンロードしたファイルを格納する店舗フォルダ存在しない為、処理を中断しました。");
                    }
                    string yearFolder = SIO.Path.Combine(shopFolder, string.Format("{0}年", d.Year));
                    if (!SIO.Directory.Exists(yearFolder))
                    {
                        return new DeadStockException(false, "ダウンロードしたファイルを格納する使用年フォルダ存在しない為、処理を中断しました。");
                    }

                    string downFile = SIO.Path.Combine(yearFolder, string.Format("{0}月.csv", d.Month));

                    if (!SIO.File.Exists(downFile))
                    {
                        continue;
                    }

                    //string usedFilePath = Path.Combine(SMConst.downloadFolder, string.Format(@"{0}\使用量\{1}\{2}年\{3}月.csv", _folderDate, shop, d.Year, d.Month));
                    var list = FileController.使用量CSVLoader(downFile);
                    uaEntList = uaEntList.Concat(list).ToList();

                }

            }

            uaEntList = FileController.合計使用量へGroupBy(uaEntList);


            _controlledBackgroundWorker.ReportProgress(20);  // 20% 


            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return new DeadStockException(true, "処理を中断しました。");

            List<DeadStockOrderDetailEntity> outList = new List<DeadStockOrderDetailEntity>();


            // 依頼先店舗で元の店舗で使用しているデッド品で使用があるかどうか確認
            int rowcounter = 0;
            int AllNoCounter = 1;
            foreach (var row in dsEntList)
            {
                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return new DeadStockException(true, "処理を中断しました。");

                rowcounter++;
                var result = (from x in uaEntList
                              where
                                 x.Code == row.Code
                              group x by new
                              {
                                  x.StoreName,
                                  //x.Name,
                                  x.Code
                              }
                                  into g
                                  join j in dsEntList
                                  on
                                     g.Key.Code equals j.Code
                                  orderby j.Name
                                  select new DeadStockOrderDetailEntity
                                  {

                                      No = AllNoCounter++, 
                                      Name = j.Name, 
                                      TotalUsedAmount = g.Sum(x => x.UsedAmount),
                                      Acceptable = false,
                                      ExpireDate = j.ExpireDate,
                                      DeadAmount = j.StockAmount,
                                      PartnerStoreName = g.Key.StoreName,
                                      Name2 = j.Name2,
                                      OneDoseAmount = j.OneDoseAmount

                                  }).ToList();

                outList = outList.Concat(result).ToList();

                int ProgressValue = 20 * rowcounter / dsEntList.Count;
                _controlledBackgroundWorker.ReportProgress(20 + ProgressValue);  // 20% + 進行度 


            }



            outList = outList.Distinct(new DeadStockOrderDetailEntityComparer()).OrderBy(x => x.NameAndName2Docking).ToList();

            _controlledBackgroundWorker.ReportProgress(40);  // 40% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return new DeadStockException(true, "処理を中断しました。");


            //outList.ForEach(x => Console.WriteLine(string.Format("貴店:{0}  薬品名:{1}  使用量:{2} 期限:{3}  デッド数量:{4}  受取可か:{5}  ",x.PartnerStoreName,x.Name,x.TotalUsedAmount.ToString(),x.ExpireDate.ToString("yyyy.MM"),x.DeadAmount.ToString(),x.Acceptable.ToString())));


            //店舗ごとにシートを作成
            if (_excelType == ExcelTypeEnum.Excel2003)
            {
                Office11.ExcelController ec = new Office11.ExcelController();
                // TODO1:CreateExcelがきちんと動作するか確認する。 Ver1.03の確認
                // TODO2:全店使用無しデッド品リストの実装
                if (!ec.CreatExcel())
                {
                    MessageBoxTop.Show("ExcelCreate中にエラーが発生した為、処理を中断します。");
                    return null;
                }

                var ex = ExcelWork(outList, ec) ;
                if (ex != null)
                {
                    if (ex is DeadStockException)
                    {
                        var dsex = ex as DeadStockException;
                        return dsex;
                    }

                    return ex;

                }
            }
            else if (_excelType == ExcelTypeEnum.Excel2007)
            {
                ExcelController ec = new ExcelController();
                if (!ec.CreatExcel())
                {
                    return null;
                }
                var ex = ExcelWork(outList, ec);
                if (ex != null)
                {
                    if (ex is DeadStockException)
                    {
                        var dsex = ex as DeadStockException;
                        return dsex;
                    }

                    return ex;

                }
            }

            return null;

        }

        #region 普通参照用


        /// <summary>
        /// 2003への出力
        /// </summary>
        /// <param name="outList"></param>
        /// <param name="ec"></param>
        /// <returns></returns>
        public Exception ExcelWork(List<DeadStockOrderDetailEntity> outList, Office11.ExcelController ec)
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
                                      select new DeadStockOrderDetailEntity
                                      {
                                          No = x.No,
                                          Name = x.Name,
                                          TotalUsedAmount = x.TotalUsedAmount,
                                          Acceptable = x.Acceptable,
                                          ExpireDate = x.ExpireDate,
                                          DeadAmount = x.DeadAmount,
                                          PartnerStoreName = x.PartnerStoreName,
                                          Name2 = x.Name2,
                                          OneDoseAmount = x.OneDoseAmount

                                      }).OrderBy(x => x.NameAndName2Docking).ToList();

                    foreach (var dead in selectList)
                    {
                        // バックグラウンドワーカーのキャンセル処理をチェック
                        if (!CheckBackgroundWorkerCanncelation())
                            return new DeadStockException(true, "処理を中断しました。");

                        if (dead.PartnerStoreName == shop)
                        {
                            ec.InsertTableData(storeCounter, insertCount, dead.NameAndName2Docking, dead.TotalUsedAmount, dead.DeadAmount, dead.ExpireDate, false);
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

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputOrderFolder, string.Format("貰受依頼書{0}", _folderDateStr));
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
        private Exception ExcelWork(List<DeadStockOrderDetailEntity> outList, ExcelController ec)
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
                                      select new DeadStockOrderDetailEntity
                                      {
                                          No = x.No,
                                          Name = x.Name,
                                          TotalUsedAmount = x.TotalUsedAmount,
                                          Acceptable = x.Acceptable,
                                          ExpireDate = x.ExpireDate,
                                          DeadAmount = x.DeadAmount,
                                          PartnerStoreName = x.PartnerStoreName,
                                          Name2 = x.Name2,
                                          OneDoseAmount = x.OneDoseAmount

                                      }).OrderBy(x => x.NameAndName2Docking).ToList();

                    foreach (var dead in selectList)
                    {
                        // バックグラウンドワーカーのキャンセル処理をチェック
                        if (!CheckBackgroundWorkerCanncelation())
                            return new DeadStockException(true, "処理を中断しました。");

                        if (dead.PartnerStoreName == shop)
                        {
                            ec.InsertTableData(storeCounter, insertCount, dead.NameAndName2Docking, dead.TotalUsedAmount, dead.DeadAmount, dead.ExpireDate, false);
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

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputOrderFolder, string.Format("貰受依頼書{0}", _folderDateStr));
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

        ////※名称２の対応が入っていない為、含めること※////

        ///// <summary>
        ///// 相対参照用
        ///// 2003への出力
        ///// </summary>
        ///// <param name="outList"></param>
        ///// <param name="ec"></param>
        ///// <returns></returns>
        //public Exception ExcelWork(List<DeadStockOrderDetailEntity> outList, Office11.ExcelController ec)
        //{
        //    try
        //    {

        //        //ALL用の相対参照先List
        //        var outlistForAllSheet = outList.Distinct(new DeadStockOrderDetailEntityComparerByOnlyNameDeadAmountExpireDateAcceptable()).ToList();

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
        //                              select new DeadStockOrderDetailEntity
        //                              {
        //                                  //No = x.No,
        //                                  Name = x.Name,
        //                                  TotalUsedAmount = x.TotalUsedAmount,
        //                                  Acceptable = x.Acceptable,
        //                                  ExpireDate = x.ExpireDate,
        //                                  DeadAmount = x.DeadAmount,
        //                                  PartnerStoreName = x.PartnerStoreName,
        //                                  相対参照先RowNo = y.No + 8 // 9行目から始まるので+8

        //                              }).OrderBy(x => x.Name).ToList();

        //            foreach (var dead in selectList)
        //            {
        //                // バックグラウンドワーカーのキャンセル処理をチェック
        //                if (!CheckBackgroundWorkerCanncelation())
        //                    return new DeadStockException(true, "処理を中断しました。");

        //                if (dead.PartnerStoreName == shop)
        //                {
        //                    ec.InsertTableData(storeCounter, insertCount, dead.相対参照先へ変換("B", dead.相対参照先RowNo), dead.TotalUsedAmount.ToString(), dead.相対参照先へ変換("C", dead.相対参照先RowNo), dead.相対参照先へ変換("D", dead.相対参照先RowNo), dead.相対参照先へ変換("E", dead.相対参照先RowNo));
        //                    insertCount++;
        //                    totalInsert++;
        //                    //ec.InsertTableData(storeCounter, insertCount, dead.Name, dead.TotalUsedAmount, dead.DeadAmount, dead.ExpireDate, false);
        //                    //insertCount++;
        //                    //totalInsert++;
        //                }
        //            }

        //            int ProgressValue = 50 * totalInsert / outList.Count;
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

        //        string ExcelBookFileName = SIO.Path.Combine(SMConst.outputOrderFolder, string.Format("貰受依頼書{0}", _folderDateStr));
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
        ///// 相対参照用
        ///// 2007への出力
        ///// </summary>
        ///// <param name="outList"></param>
        ///// <param name="ec"></param>
        ///// <returns></returns>
        //private Exception ExcelWork(List<DeadStockOrderDetailEntity> outList, ExcelController ec)
        //{

        //    try
        //    {

        //        //ALL用の相対参照先List
        //        var outlistForAllSheet = outList.Distinct(new DeadStockOrderDetailEntityComparerByOnlyNameDeadAmountExpireDateAcceptable()).ToList();

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
        //                              select new DeadStockOrderDetailEntity
        //                              {
        //                                  //No = x.No,
        //                                  Name = x.Name,
        //                                  TotalUsedAmount = x.TotalUsedAmount,
        //                                  Acceptable = x.Acceptable,
        //                                  ExpireDate = x.ExpireDate,
        //                                  DeadAmount = x.DeadAmount,
        //                                  PartnerStoreName = x.PartnerStoreName,
        //                                  相対参照先RowNo = y.No + 8 // 9行目から始まるので+8

        //                              }).OrderBy(x=> x.Name).ToList();

        //            foreach (var dead in selectList)
        //            {
        //                // バックグラウンドワーカーのキャンセル処理をチェック
        //                if (!CheckBackgroundWorkerCanncelation())
        //                    return new DeadStockException(true, "処理を中断しました。");

        //                if (dead.PartnerStoreName == shop)
        //                {
        //                    ec.InsertTableData(storeCounter, insertCount, dead.相対参照先へ変換("B", dead.相対参照先RowNo), dead.TotalUsedAmount.ToString(), dead.相対参照先へ変換("C", dead.相対参照先RowNo), dead.相対参照先へ変換("D", dead.相対参照先RowNo), dead.相対参照先へ変換("E", dead.相対参照先RowNo));
        //                    insertCount++;
        //                    totalInsert++;
        //                    //ec.InsertTableData(storeCounter, insertCount, dead.Name, dead.TotalUsedAmount, dead.DeadAmount, dead.ExpireDate, false);
        //                    //insertCount++;
        //                    //totalInsert++;
        //                }
        //            }

        //            int ProgressValue = 50 * totalInsert / outList.Count;
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

        //        string ExcelBookFileName = SIO.Path.Combine(SMConst.outputOrderFolder, string.Format("貰受依頼書{0}", _folderDateStr));
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
