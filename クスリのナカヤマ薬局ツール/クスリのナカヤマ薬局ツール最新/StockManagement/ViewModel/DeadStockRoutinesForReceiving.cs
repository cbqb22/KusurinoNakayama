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
using Office11 = ExcelControllerOffice11;
using StockManagement.ViewModel.Common.MessageBox;
using StockManagement.ViewModel.IO;





namespace StockManagement.ViewModel
{
    public class DeadStockRoutinesForReceiving
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

        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; }
        }

        public DeadStockRoutinesForReceiving(DateTime from, DateTime to, string folderDate, BackgroundWorker worker, ExcelTypeEnum excelType, bool autoPrint, DateTime folderdate, string baseStoreName)
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


            // 依頼元店舗名の使用量リストを読込
            List<UsedAmountEntity> uaEntList = new List<UsedAmountEntity>();


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

                string shopFolder = SIO.Path.Combine(usedFoler, _baseStoreName);
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
                uaEntList = uaEntList.Concat(list).ToList();

            }

            uaEntList = FileController.合計使用量へGroupBy(uaEntList);


            _controlledBackgroundWorker.ReportProgress(10);  // 10% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;


            // 依頼先店舗でデッド品を読込
            List<DeadStockEntity> dsEntList = new List<DeadStockEntity>();


            foreach (var shop in _applyStoreList)
            {
                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return false;

                // 貰い受け可能リストでは自店のデッド品は使用しない。
                if (shop == _baseStoreName)
                {
                    continue;
                }


                string deadFilePath = SIO.Path.Combine(SMConst.downloadFolder, string.Format(@"{0}\不動品\{1}\不動品データ.csv", _folderDateStr, shop));


                var loadent = FileController.不動品CSVLoader(deadFilePath, false);
                dsEntList = dsEntList.Concat(loadent).ToList();


            }

            _controlledBackgroundWorker.ReportProgress(20);  // 20% 


            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;

            // 依頼元の使用量をグループ化する
            var uaGroupResult = (from x in uaEntList
                                 group x by new
                                 {
                                     x.StoreName,
                                     x.Name,
                                     x.Code,
                                     x.Price
                                 }
                                     into g
                                     orderby g.Key.Name
                                     select new UsedAmountEntity
                                     {
                                         StoreName = g.Key.StoreName,
                                         Name = g.Key.Name,
                                         UsedAmount = g.Sum(x => x.UsedAmount),
                                         Code = g.Key.Code,
                                         Price = g.Key.Price
                                         // UsedDateは不要なので空

                                     }).ToList();



            _controlledBackgroundWorker.ReportProgress(30);  // 30% 


            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;

            List<DeadStockReceivingDetailEntity> outList = new List<DeadStockReceivingDetailEntity>();


            // 元の店舗の使用量のうち、依頼先のデッド品でもらえるものがあるかどうか確認
            int rowcounter = 0;
            foreach (var row in uaGroupResult)
            {
                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return false;

                rowcounter++;
                var result = (from x in dsEntList
                              where
                                 x.Code == row.Code
                              //group x by new
                              //{
                              //    x.StoreName,
                              //    //x.Name,
                              //    x.Code
                              //}
                              //into g
                              join j in uaGroupResult
                              on
                                  x.Code equals j.Code
                              orderby j.Name
                              select new DeadStockReceivingDetailEntity
                              {

                                  //No = rowcounter++,
                                  Name = x.Name,
                                  TotalUsedAmount = j.UsedAmount,
                                  Acceptable = false,
                                  ExpireDate = x.ExpireDate,
                                  DeadAmount = x.StockAmount,
                                  PartnerStoreName = x.StoreName,
                                  Name2 = x.Name2,
                                  OneDoseAmount = x.OneDoseAmount

                              }).ToList();

                outList = outList.Concat(result).ToList();

                int ProgressValue = 10 * rowcounter / dsEntList.Count;
                _controlledBackgroundWorker.ReportProgress(30 + ProgressValue);  // 30% + 進行度 


            }

            outList = outList.Distinct(new DeadStockReceivingDetailEntityComparer()).OrderBy(x => x.NameAndName2Docking).ToList();


            _controlledBackgroundWorker.ReportProgress(40);  // 40% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;


            //outList.ForEach(x => Console.WriteLine(string.Format("貴店:{0}  薬品名:{1}  使用量:{2} 期限:{3}  デッド数量:{4}  受取可か:{5}  ",x.PartnerStoreName,x.Name,x.TotalUsedAmount.ToString(),x.ExpireDate.ToString("yyyy.MM"),x.DeadAmount.ToString(),x.Acceptable.ToString())));


            //店舗ごとにシートを作成
            if (_excelType == ExcelTypeEnum.Excel2003)
            {
                Office11.ExcelControllerForReceiving ec = new Office11.ExcelControllerForReceiving();
                if (!ec.CreatExcel())
                {
                    MessageBoxTop.Show("ExcelCreate中にエラーが発生した為、処理を中断します。");
                    return false;
                }

                var ex = ExcelWork(outList, ec);
                if (ex != null)
                {
                    MessageBoxTop.Show("エラーが発生しました。処理を中止します。\r\n" + ex.Message + ex.StackTrace);
                    return false;
                }
            }
            else if (_excelType == ExcelTypeEnum.Excel2007)
            {
                ExcelControllerForReceiving ec = new ExcelControllerForReceiving();
                if (!ec.CreatExcel())
                {
                    return false;
                }
                var ex = ExcelWork(outList, ec);
                if (ex != null)
                {
                    MessageBoxTop.Show("エラーが発生しました。処理を中止します。\r\n" + ex.Message + ex.StackTrace);
                    return false;
                }
            }

            return true;


        }

        public Exception ExcelWork(List<DeadStockReceivingDetailEntity> outList, Office11.ExcelControllerForReceiving ec)
        {
            try
            {
                // 相手先店舗分だけSheetの追加
                int totalSheetCount = 0;
                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new Exception("処理が中断されました。");


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
                        return new Exception("処理が中断されました。");

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
                        return new Exception("処理が中断されました。");

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
                                      select new DeadStockReceivingDetailEntity
                                      {
                                          //No = x.No,
                                          Name = x.Name,
                                          TotalUsedAmount = x.TotalUsedAmount,
                                          Acceptable = x.Acceptable,
                                          ExpireDate = x.ExpireDate,
                                          DeadAmount = x.DeadAmount,
                                          PartnerStoreName = x.PartnerStoreName,
                                          Name2 = x.Name2,
                                          OneDoseAmount = x.OneDoseAmount

                                      }).ToList();

                    foreach (var dead in selectList)
                    {
                        // バックグラウンドワーカーのキャンセル処理をチェック
                        if (!CheckBackgroundWorkerCanncelation())
                            return new Exception("処理が中断されました。");

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
                    return new Exception("処理が中断されました。");



                // PrintOut
                if (_autoPrint)
                {
                    ec.PrintOut(0);
                }

                // Save
                //string ExcelBookFileName = @"goodworkexcel";


                //Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputReceiveFolder, string.Format("貰受可能リスト{0}", _folderDateStr));

                ec.CloseExcel(true, ExcelBookFileName);


                //System.Windows.MessageBoxTop.Show( "DeadStock:" +dsEntList.Count + "個\r\n" + "使用量:" + uaEntList.Count + "個");

                _controlledBackgroundWorker.ReportProgress(100);  // 100% 


            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;

        }

        private Exception ExcelWork(List<DeadStockReceivingDetailEntity> outList, ExcelControllerForReceiving ec)
        {

            try
            {

                // 相手先店舗分だけSheetの追加
                int totalSheetCount = 0;
                foreach (var shop in _applyStoreList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new Exception("処理が中断されました。");


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
                        return new Exception("処理が中断されました。");


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
                        return new Exception("処理が中断されました。");

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
                                      select new DeadStockReceivingDetailEntity
                                      {
                                          //No = x.No,
                                          Name = x.Name,
                                          TotalUsedAmount = x.TotalUsedAmount,
                                          Acceptable = x.Acceptable,
                                          ExpireDate = x.ExpireDate,
                                          DeadAmount = x.DeadAmount,
                                          PartnerStoreName = x.PartnerStoreName,
                                          Name2 = x.Name2,
                                          OneDoseAmount = x.OneDoseAmount

                                      }).ToList();

                    foreach (var dead in selectList)
                    {
                        // バックグラウンドワーカーのキャンセル処理をチェック
                        if (!CheckBackgroundWorkerCanncelation())
                            return new Exception("処理が中断されました。");

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
                    return new Exception("処理が中断されました。");


                // PrintOut
                if (_autoPrint)
                {
                    ec.PrintOut(0);
                }

                // Save
                //string ExcelBookFileName = @"goodworkexcel";


                //Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputReceiveFolder, string.Format("貰受可能リスト{0}", _folderDateStr));
                ec.CloseExcel(true, ExcelBookFileName);


                //System.Windows.MessageBoxTop.Show( "DeadStock:" +dsEntList.Count + "個\r\n" + "使用量:" + uaEntList.Count + "個");

                _controlledBackgroundWorker.ReportProgress(100);  // 100% 


            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;


        }

    }
}

