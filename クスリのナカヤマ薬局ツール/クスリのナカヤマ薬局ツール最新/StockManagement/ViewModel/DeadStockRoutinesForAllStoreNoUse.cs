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
    public class DeadStockRoutinesForAllStoreNoUse
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

        public DeadStockRoutinesForAllStoreNoUse(DateTime from, DateTime to, string folderDate, BackgroundWorker worker, ExcelTypeEnum excelType, bool autoPrint, DateTime folderdate, string baseStoreName)
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


            // ①依頼元店舗でデッド品を読込
            List<DeadStockEntity> dsEntList = new List<DeadStockEntity>();

            if (_applyStoreList.Contains(_baseStoreName) == false)
            {
                MessageBoxTop.Show("依頼元店舗名が設定ファイルに存在しないため、エラーが発生しました。処理を中断します。");
                return false;
            }

            foreach (var shop in _applyStoreList)
            {
                // バックグラウンドワーカーのキャンセル処理をチェック
                if (!CheckBackgroundWorkerCanncelation())
                    return false;



                // 自店以外は飛ばす
                if (shop != _baseStoreName)
                {
                    continue;
                }


                string deadFilePath = SIO.Path.Combine(SMConst.downloadFolder, string.Format(@"{0}\不動品\{1}\不動品データ.csv", _folderDateStr, shop));

                var loadent = FileController.不動品CSVLoader(deadFilePath,false);
                dsEntList = dsEntList.Concat(loadent).ToList();


            }


            _controlledBackgroundWorker.ReportProgress(10);  // 10% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;



            // 他店舗の使用実績を読み込み
            List<UsedAmountEntity> uaEntList = new List<UsedAmountEntity>();

            foreach (var othershop in _applyStoreList)
            {
                // 自店は飛ばす
                if (othershop == _baseStoreName)
                {
                    continue;
                }

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
                    uaEntList = uaEntList.Concat(list).ToList();

                }
            }

            uaEntList = FileController.合計使用量へGroupBy(uaEntList);



            _controlledBackgroundWorker.ReportProgress(20);  // 20% 


            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;


            // ③使用実績がないものを抽出

            var outList = (from a in dsEntList.Except(from x in dsEntList
                                                     join y in uaEntList
                                                        on new { a = x.Code } equals new { a = y.Code }
                                                     select x)
                          select new DeadStockAllStoreNoUseEntity
                          {
                             Name = a.Name,
                             Code = a.Code,
                             ExpireDate = a.ExpireDate,
                             Price = a.Price,
                             StockAmount = a.StockAmount,
                             StoreName = a.StoreName,
                             Name2 = a.Name2,
                             OneDoseAmount = a.OneDoseAmount
                          }).ToList();





            outList = outList.Distinct(new DeadStockAllStoreNoUseEntityComparer()).ToList();

            _controlledBackgroundWorker.ReportProgress(40);  // 40% 

            // バックグラウンドワーカーのキャンセル処理をチェック
            if (!CheckBackgroundWorkerCanncelation())
                return false;


            //outList.ForEach(x => Console.WriteLine(string.Format("貴店:{0}  薬品名:{1}  使用量:{2} 期限:{3}  デッド数量:{4}  受取可か:{5}  ",x.PartnerStoreName,x.Name,x.TotalUsedAmount.ToString(),x.ExpireDate.ToString("yyyy.MM"),x.DeadAmount.ToString(),x.Acceptable.ToString())));


            //店舗ごとにシートを作成
            if (_excelType == ExcelTypeEnum.Excel2003)
            {
                Office11.ExcelControllerForAllStoreNoUse ec = new Office11.ExcelControllerForAllStoreNoUse();
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
                ExcelControllerForAllStoreNoUse ec = new ExcelControllerForAllStoreNoUse();
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

        public Exception ExcelWork(List<DeadStockAllStoreNoUseEntity> outList, Office11.ExcelControllerForAllStoreNoUse ec)
        {
            try
            {
                // シートは１シートだけなのでnewした自店で１シートになっているのでそのまま。

                // シートをリネーム
                ec.RenameSheet(_baseStoreName, 1);

                // 基本情報を入力
                ec.SetBasicalForm(1, _folderDate, _baseStoreName, _from, _to, DateTime.Now);

                int insertCount = 1;
                foreach (var dead in outList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new Exception("処理が中断されました。");

                    ec.InsertTableData(1, insertCount, dead.NameAndName2Docking, dead.StockAmount, dead.Price, dead.ExpireDate);
                    insertCount++;

                    int ProgressValue = outList.Count == 0 ? 0 : 50 * insertCount / outList.Count;
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

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputAllStoreNoUseFolder, string.Format("{0}作成_全店使用無しデッド品リスト{1}", _baseStoreName, _folderDateStr));

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

        private Exception ExcelWork(List<DeadStockAllStoreNoUseEntity> outList, ExcelControllerForAllStoreNoUse ec)
        {
            try
            {
                // シートは１シートだけなのでnewした自店で１シートになっているのでそのまま。

                // シートをリネーム
                ec.RenameSheet(_baseStoreName, 1);

                // 基本情報を入力
                ec.SetBasicalForm(1, _folderDate, _baseStoreName, _from, _to, DateTime.Now);

                int insertCount = 1;
                foreach (var dead in outList)
                {
                    // バックグラウンドワーカーのキャンセル処理をチェック
                    if (!CheckBackgroundWorkerCanncelation())
                        return new Exception("処理が中断されました。");

                    ec.InsertTableData(1, insertCount, dead.NameAndName2Docking, dead.StockAmount, dead.Price, dead.ExpireDate);
                    insertCount++;

                    int ProgressValue = outList.Count == 0 ? 0 : 50 * insertCount / outList.Count;
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

                string ExcelBookFileName = SIO.Path.Combine(SMConst.outputAllStoreNoUseFolder, string.Format("{0}作成_全店使用無しデッド品リスト{1}", _baseStoreName, _folderDateStr));

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

