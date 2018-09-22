using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Globalization;
using PharmacyTool.Web.Properties;


namespace PharmacyTool.Web.Service.File.Reader
{
    // メモ: ここでクラス名 "FileReader" を変更する場合は、Web.config で "FileReader" への参照も更新する必要があります。
    public class FileReader : IFileReader
    {
        // ローカルキャッシュ＆ローカル検索用
        public List<現在庫データ> OpenCSV(string Filepath)
        {

#if DEBUG
            string FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\現在庫\total.csv";

#elif NAKAYAMA

            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPathNAKAYAMA;
            string relativePath = @"ClientBin\在庫関連\現在庫\total.csv";
            string FilePath = Path.Combine(srp, relativePath);

#else
            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPath;
            string relativePath = @"ClientBin\在庫関連\現在庫\total.csv";
            string FilePath = Path.Combine(srp, relativePath);
#endif

            List<薬局データ> collection = new List<薬局データ>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {

                while (streamReader.Peek() != -1)
                {
                    string[] stringBuffer;
                    stringBuffer = streamReader.ReadLine().Split(',');
                    薬局データ data = new 薬局データ();

                    if (stringBuffer.GetLength(0) < 7)
                    {
                        throw new Exception("CSVファイルデータが不正です。");
                    }

                    int result;

                    if (stringBuffer[1].Equals("") || stringBuffer[1].Length < 9 || int.TryParse(stringBuffer[1].Substring(0, 9), out result) == false)
                    {
                        continue;
                    }



                    data.店名 = stringBuffer[0];
                    data.薬品コード = stringBuffer[1];

                    data.医薬品名 = stringBuffer[2];

                    decimal res;
                    if (decimal.TryParse(stringBuffer[3], out res))
                    {
                        data.薬価 = res;
                    }
                    else
                    {
                        data.薬価 = 0;
                    }

                    data.在庫数 = stringBuffer[4];

                    // 日付を整形 YYYY/MM/DD
                    DateTime result2;
                    if (DateTime.TryParse(stringBuffer[5], out result2))
                    {
                        //// 9999/12/1はカウントアップできない。
                        if (new DateTime(1900, 1, 1) <= result2.Date && result2.Date < new DateTime(9999, 12, 1))
                        {
                            // システム対応-使用期限を月末に設定
                            // レセコン出力データは1日で出てくる為、ユーザー希望でその月の月末とする
                            data.使用期限 = result2.Date.AddMonths(1).AddDays(-1);
                            //data.使用期限 = result2.Date;

                        }
                        else
                        {
                            data.使用期限 = result2.Date;
                        }
                    }
                    else
                    {
                        data.使用期限 = new DateTime(9999, 12, 1);
                    }

                    data.製造会社 = stringBuffer[6];

                    if (stringBuffer[7].Equals(""))
                    {
                        data.後発区分 = "先発品";
                    }
                    else
                    {
                        data.後発区分 = stringBuffer[7];
                    }




                    collection.Add(data);

                }

            }

#if DEBUG
            //FilePath = @"C:\Users\poohace\Documents\Visual Studio 2008\Projects\PharmacyTool\PharmacyTool.Web\ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\MEDIS\MEDIS.TXT";
#elif NAKAYAMA

            relativePath = @"ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = Path.Combine(srp, relativePath);

#else
            relativePath = @"ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = Path.Combine(srp, relativePath);
#endif





            List<MEDISデータ> collection2 = new List<MEDISデータ>();

            List<string> contains個別医薬品コード = new List<string>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {

                while (streamReader.Peek() != -1)
                {
                    string[] stringBuffer;
                    stringBuffer = streamReader.ReadLine().Split(',');
                    MEDISデータ data = new MEDISデータ();

                    if (stringBuffer.GetLength(0) < 24)
                    {
                        throw new Exception("CSVファイルデータが不正です。");
                    }

                    int result;

                    if (stringBuffer[8].Replace('"', ' ').Replace(" ", "").Equals("") || int.TryParse(stringBuffer[8].Replace('"', ' ').Replace(" ", ""), out result) == false)
                    {
                        continue;
                    }

                    data.個別医薬品コード = stringBuffer[7].Replace('"', ' ').Replace(" ", "");
                    data.レセプト電算処理システムコード = stringBuffer[8].Replace('"', ' ').Replace(" ", "");

                    collection2.Add(data);


                }
            }

            // MEDISのデータは、併売で一つの個別医薬品コードに対して複数の販売会社データも持っている為、重複を省く
            var distinctcollection2 = (from x in collection2
                                       select new
                                       {
                                           x.個別医薬品コード,
                                           x.レセプト電算処理システムコード
                                       }).Distinct();


            var レセ電で結合したデータ = from x in collection
                              join p in distinctcollection2 on x.薬品コード.Substring(0, 9) equals p.レセプト電算処理システムコード into mergeCollection
                              from outputCollection in mergeCollection.DefaultIfEmpty()
                              select new 現在庫データ
                              {
                                  店名 = x.店名,
                                  個別医薬品コード = outputCollection == null ? "" : outputCollection.個別医薬品コード,
                                  医薬品名 = x.医薬品名,
                                  在庫数 = x.在庫数,
                                  使用期限 = x.使用期限,
                                  製造会社 = x.製造会社,
                                  薬価 = x.薬価.ToString("0.0"),
                                  後発区分 = x.後発区分,
                                  レセプト電算処理システムコード = x.薬品コード.Substring(0, 9)
                              };



            return レセ電で結合したデータ.ToList(); ;

        }

        // サーバーで検索用
        // 現在庫検索用
        public 在庫リターンデータセット<現在庫データ> Get現在庫検索データ(string 検索ワード, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月)
        {

#if DEBUG
            string FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\現在庫\total.csv";

#elif NAKAYAMA

            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPathNAKAYAMA;
            string relativePath = @"ClientBin\在庫関連\現在庫\total.csv";
            string FilePath = Path.Combine(srp, relativePath);

#else
            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPath;
            string relativePath = @"ClientBin\在庫関連\現在庫\total.csv";
            string FilePath = Path.Combine(srp, relativePath);
#endif

            List<薬局データ> collection = new List<薬局データ>();
            List<string> 検索ワード分割 = 検索ワード.Replace('　', ' ').Split(' ').ToList(); // ２バイト空白文字は１バイト空白文字へ変換し、１バイト空白文字で分割する

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {

                while (streamReader.Peek() != -1)
                {

                    string rl = streamReader.ReadLine();


                    var stringBuffer = rl.Split(',');


                    薬局データ data = new 薬局データ();

                    // 不正なデータは飛ばす
                    if (stringBuffer.GetLength(0) != 9 && stringBuffer.GetLength(0) != 10)
                    {
                        continue;
                        //throw new Exception("CSVファイルデータが不正です。");
                    }

                    int result;

                    if (stringBuffer[1].Equals("") || stringBuffer[1].Length < 9 || int.TryParse(stringBuffer[1].Substring(0, 9), out result) == false)
                    {
                        continue;
                    }

                    // [0] 店舗名
                    // [1] 薬品コード
                    // [2] 薬品名
                    // [3] 在庫数
                    // [4] 使用期限
                    // [5] 薬価
                    // [6] メーカー名
                    // [7] 後発区分
                    // [8] 名称２
                    // [9] 最終更新日時

                    data.店名 = stringBuffer[0];
                    data.薬品コード = stringBuffer[1];

                    data.医薬品名 = stringBuffer[2];
                    data.在庫数 = stringBuffer[3];

                    // 日付を整形 yyyy/MM/dd
                    // 月までしか入っていないので、日にちは１日とされる
                    DateTime result2;
                    if (DateTime.TryParse(stringBuffer[4], out result2))
                    {
                        //// 9999/12/1はカウントアップできない。
                        if (new DateTime(1900, 1, 1) <= result2.Date && result2.Date < new DateTime(9999, 12, 1))
                        {
                            // システム対応-使用期限を月末に設定
                            // レセコン出力データは1日で出てくる為、ユーザー希望でその月の月末とする
                            data.使用期限 = result2.Date.AddMonths(1).AddDays(-1);
                            //data.使用期限 = result2.Date;

                        }
                        else
                        {
                            data.使用期限 = result2.Date;
                        }

                    }
                    else
                    {
                        data.使用期限 = new DateTime(9999, 12, 1);
                    }

                    // 薬価
                    decimal res;
                    if (decimal.TryParse(stringBuffer[5], out res))
                    {
                        data.薬価 = res;
                    }
                    else
                    {
                        data.薬価 = 0;
                    }

                    data.製造会社 = stringBuffer[6];

                    if (stringBuffer[7].Equals(""))
                    {
                        data.後発区分 = "先発品";
                    }
                    else
                    {
                        data.後発区分 = stringBuffer[7];
                    }

                    data.名称２ = "";

                    if (stringBuffer.GetLength(0) == 9)
                    {
                        data.最終更新日時 = stringBuffer[8];
                    }
                    else if (stringBuffer.GetLength(0) == 10)
                    {
                        data.最終更新日時 = stringBuffer[9];
                        data.名称２ = stringBuffer[8];

                    }
                    else
                    {
                        data.最終更新日時 = "";
                    }




                    collection.Add(data);

                }

            }

#if DEBUG
            //FilePath = @"C:\Users\poohace\Documents\Visual Studio 2008\Projects\PharmacyTool\PharmacyTool.Web\ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\MEDIS\MEDIS.TXT";

#elif NAKAYAMA

            relativePath = @"ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = Path.Combine(srp, relativePath);

#else
            relativePath = @"ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = Path.Combine(srp, relativePath);
#endif





            List<MEDISデータ> collection2 = new List<MEDISデータ>();

            List<string> contains個別医薬品コード = new List<string>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {

                while (streamReader.Peek() != -1)
                {
                    string[] stringBuffer;
                    stringBuffer = streamReader.ReadLine().Split(',');
                    MEDISデータ data = new MEDISデータ();

                    if (stringBuffer.GetLength(0) < 24)
                    {
                        throw new Exception("CSVファイルデータが不正です。");
                    }

                    int result;

                    if (stringBuffer[8].Replace('"', ' ').Replace(" ", "").Equals("") || int.TryParse(stringBuffer[8].Replace('"', ' ').Replace(" ", ""), out result) == false)
                    {
                        continue;
                    }

                    data.薬価基準収載医薬品コード = stringBuffer[6].Replace('"', ' ').Replace(" ", "");
                    data.個別医薬品コード = stringBuffer[7].Replace('"', ' ').Replace(" ", "");
                    data.レセプト電算処理システムコード = stringBuffer[8].Replace('"', ' ').Replace(" ", "");

                    collection2.Add(data);


                }
            }

            // MEDISのデータは、併売で一つの個別医薬品コードに対して複数の販売会社データも持っている為、重複を省く
            var distinctcollection2 = (from x in collection2
                                       select new DistinctCheck
                                       {
                                           個別医薬品コード = x.個別医薬品コード,
                                           レセプト電算処理システムコード = x.レセプト電算処理システムコード,
                                           薬価基準収載医薬品コード = x.薬価基準収載医薬品コード
                                       }).Distinct(new DistinctCheckComparer());



            var レセ電で結合したデータ = from x in collection
                              join p in distinctcollection2 on x.薬品コード.Substring(0, 9) equals p.レセプト電算処理システムコード into mergeCollection
                              from outputCollection in mergeCollection.DefaultIfEmpty()
                              select new 現在庫データ
                              {
                                  店名 = x.店名,
                                  個別医薬品コード = (outputCollection == null) ? "" : outputCollection.個別医薬品コード,
                                  医薬品名 = x.医薬品名,
                                  在庫数 = x.在庫数,
                                  使用期限 = x.使用期限,
                                  製造会社 = x.製造会社,
                                  薬価 = x.薬価.ToString("0.0"),
                                  後発区分 = x.後発区分,
                                  レセプト電算処理システムコード = x.薬品コード.Substring(0, 9),
                                  名称２ = x.名称２,
                                  最終更新日時 = x.最終更新日時
                              };



            // 在庫検索
            var sortitem = from x in レセ電で結合したデータ
                           where
                           (
                               (全期限) ||
                               (期限内 && System.DateTime.Now.Date <= x.使用期限) ||                                                // 期限内:その日の含める
                               (期限切 && x.使用期限 < System.DateTime.Now.Date) ||                                                 // 期限切:その日は含めない
                               (期限指定か && 以内指定か && (x.使用期限 <= System.DateTime.Now.Date.AddMonths(期限加算月).Date)) || // ～以内:その日は含める
                               (期限指定か && !以内指定か && (System.DateTime.Now.Date.AddMonths(期限加算月).Date <= x.使用期限))   // ～以上:その日は含める
                           )
                           select new 現在庫データ
                           {
                               店名 = x.店名,
                               個別医薬品コード = x.個別医薬品コード,
                               医薬品名 = x.医薬品名,
                               在庫数 = x.在庫数,
                               使用期限 = x.使用期限,
                               薬価 = x.薬価,
                               製造会社 = x.製造会社,
                               名称２ = x.名称２,
                               最終更新日時 = x.最終更新日時
                           };

            List<現在庫データ> list = sortitem.ToList();

            List<現在庫データ> matchingList = new List<現在庫データ>();

            list.ForEach(delegate(現在庫データ data)
            {

                bool すべての検索ワードが一致 = true;

                foreach (var word in 検索ワード分割)
                {
                    // 店名・YJ・医薬品名・製造会社のどれにも一致しない場合は次へ
                    if (!data.店名.Contains(word) &&
                        !data.個別医薬品コード.Contains(word) &&
                        !data.医薬品名と名称２連結.Contains(word) &&
                        !data.製造会社.Contains(word) &&
                        !data.名称２.Contains(word))
                    {
                        すべての検索ワードが一致 = false;
                        break;
                    }
                }

                if (すべての検索ワードが一致)
                {
                    //if (!string.IsNullOrEmpty(data.名称２))
                    //{
                    //    data.医薬品名 = data.医薬品名 + " / " + data.名称２;
                    //}

                    matchingList.Add(data);
                }
            }
            );






            在庫リターンデータセット<現在庫データ> rdataset = new 在庫リターンデータセット<現在庫データ>();
            if (500 < matchingList.Count)
            {
                rdataset.エラーメッセージ = "検索件数が５００件を超えたので、中断しました。検索ワードを変更して再度検索して下さい。";
                return rdataset;
            }

            rdataset.検索キーワード = 検索ワード;

            rdataset.検索結果データlist = matchingList;
            return rdataset;

        }


        // サーバーで検索用
        // 後発品検索用
        public 在庫リターンデータセット<現在庫データ> Get後発品検索データ(string YJコード, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月, bool 他規格・剤形も表示する)
        {

#if DEBUG
            string FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\現在庫\total.csv";
#elif NAKAYAMA

            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPathNAKAYAMA;
            string relativePath = @"ClientBin\在庫関連\現在庫\total.csv";
            string FilePath = Path.Combine(srp, relativePath);

#else
            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPath;
            string relativePath = @"ClientBin\在庫関連\現在庫\total.csv";
            string FilePath = Path.Combine(srp, relativePath);
#endif

            List<薬局データ> collection = new List<薬局データ>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {

                while (streamReader.Peek() != -1)
                {
                    string[] stringBuffer;
                    stringBuffer = streamReader.ReadLine().Split(',');
                    薬局データ data = new 薬局データ();


                    // 不正なデータは飛ばす
                    if (stringBuffer.GetLength(0) != 9 && stringBuffer.GetLength(0) != 10)
                    {
                        continue;
                        //throw new Exception("CSVファイルデータが不正です。");
                    }

                    //if (stringBuffer.GetLength(0) < 8)
                    //{
                    //    throw new Exception("CSVファイルデータが不正です。");
                    //}

                    int result;

                    if (stringBuffer[1].Equals("") || (stringBuffer[1].Length != 9 && stringBuffer[1].Length != 13) || int.TryParse(stringBuffer[1].Substring(0, 9), out result) == false)
                    {
                        continue;
                    }


                    // [0] 店舗名
                    // [1] 薬品コード
                    // [2] 薬品名
                    // [3] 在庫数
                    // [4] 使用期限
                    // [5] 薬価
                    // [6] メーカー名
                    // [7] 後発区分
                    // [8] 名称２
                    // [9] 最終更新日時

                    data.店名 = stringBuffer[0];
                    data.薬品コード = stringBuffer[1];
                    data.医薬品名 = stringBuffer[2];
                    data.在庫数 = stringBuffer[3];

                    // 日付を整形 YYYY/MM/DD
                    DateTime result2;
                    if (DateTime.TryParse(stringBuffer[4], out result2))
                    {
                        //// 9999/12/1はカウントアップできない。
                        if (new DateTime(1900, 1, 1) <= result2.Date && result2.Date < new DateTime(9999, 12, 1))
                        {
                            // システム対応-使用期限を月末に設定
                            // レセコン出力データは1日で出てくる為、ユーザー希望でその月の月末とする
                            data.使用期限 = result2.Date.AddMonths(1).AddDays(-1);
                            //data.使用期限 = result2.Date;

                        }
                        else
                        {
                            data.使用期限 = result2.Date;
                        }
                    }
                    else
                    {
                        data.使用期限 = new DateTime(9999, 12, 1);
                    }

                    decimal res;
                    if (decimal.TryParse(stringBuffer[5], out res))
                    {
                        data.薬価 = res;
                    }
                    else
                    {
                        data.薬価 = 0;
                    }

                    data.製造会社 = stringBuffer[6];

                    if (stringBuffer[7].Equals(""))
                    {
                        data.後発区分 = "先発品";
                    }
                    else
                    {
                        data.後発区分 = stringBuffer[7];
                    }



                    data.名称２ = "";

                    if (stringBuffer.GetLength(0) == 9)
                    {
                        data.最終更新日時 = stringBuffer[8];
                    }
                    else if (stringBuffer.GetLength(0) == 10)
                    {
                        data.最終更新日時 = stringBuffer[9];
                        data.名称２ = stringBuffer[8];

                    }
                    else
                    {
                        data.最終更新日時 = "";
                    }

                    collection.Add(data);

                }

            }

#if DEBUG
            //FilePath = @"C:\Users\poohace\Documents\Visual Studio 2008\Projects\PharmacyTool\PharmacyTool.Web\ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\MEDIS\MEDIS.TXT";
#elif NAKAYAMA

            relativePath = @"ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = Path.Combine(srp, relativePath);

#else
            relativePath = @"ClientBin\在庫関連\MEDIS\MEDIS.TXT";
            FilePath = Path.Combine(srp, relativePath);
#endif





            List<MEDISデータ> collection2 = new List<MEDISデータ>();

            List<string> contains個別医薬品コード = new List<string>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {

                while (streamReader.Peek() != -1)
                {
                    string[] stringBuffer;
                    stringBuffer = streamReader.ReadLine().Split(',');
                    MEDISデータ data = new MEDISデータ();

                    if (stringBuffer.GetLength(0) < 24)
                    {
                        throw new Exception("CSVファイルデータが不正です。");
                    }

                    int result;

                    if (stringBuffer[8].Replace('"', ' ').Replace(" ", "").Equals("") || int.TryParse(stringBuffer[8].Replace('"', ' ').Replace(" ", ""), out result) == false)
                    {
                        continue;
                    }

                    data.薬価基準収載医薬品コード = stringBuffer[6].Replace('"', ' ').Replace(" ", "");
                    data.個別医薬品コード = stringBuffer[7].Replace('"', ' ').Replace(" ", "");
                    data.レセプト電算処理システムコード = stringBuffer[8].Replace('"', ' ').Replace(" ", "");

                    collection2.Add(data);


                }
            }

            // MEDISのデータは、併売で一つの個別医薬品コードに対して複数の販売会社データも持っている為、重複を省く
            var distinctcollection2 = (from x in collection2
                                       select new DistinctCheck
                                       {
                                           個別医薬品コード = x.個別医薬品コード,
                                           レセプト電算処理システムコード = x.レセプト電算処理システムコード,
                                           薬価基準収載医薬品コード = x.薬価基準収載医薬品コード
                                       }).Distinct(new DistinctCheckComparer());


            // ここの薬価データは、次に薬価の降順で使用する為、decimalを使用している
            // 後発品はInner JoinでOK. Outer Joinで個別医薬品コードを空で結合しても、次の検索ワードの個別医薬品コードとの結合が出来ないから。
            var レセ電で結合したデータ = from x in collection
                              join p in distinctcollection2 on x.薬品コード.Substring(0, 9) equals p.レセプト電算処理システムコード //into mergeCollection

                              //追加! 薬品コードのデータが入っていても、個別医薬品コードが空の場合がある。そうすると、この次のLinqの１行目でsubstringできずに落ちる。
                              //例)消毒用エタノール　フヂミ 2615703X1019
                              where
                                p.個別医薬品コード != "" 
                              //from outputCollection in mergeCollection.DefaultIfEmpty()
                              select new
                              {
                                  店名 = x.店名,
                                  個別医薬品コード = p.個別医薬品コード,
                                  //個別医薬品コード = (outputCollection == null) ? "" : outputCollection.個別医薬品コード,
                                  医薬品名 = x.医薬品名,
                                  在庫数 = x.在庫数,
                                  使用期限 = x.使用期限,
                                  製造会社 = x.製造会社,
                                  薬価 = x.薬価,
                                  後発区分 = x.後発区分,
                                  レセプト電算処理システムコード = p.レセプト電算処理システムコード,
                                  名称２ = x.名称２,
                                  最終更新日時 = x.最終更新日時

                              };

            // デフォルトは９ケタ一致（同成分、同一剤形）
            int CheckLength = 9;
            if (他規格・剤形も表示する)
            {
                CheckLength = 7;
            }


            // 店名(昇順)、先後発品(先発品が上)、薬価(高薬価が上)、医薬品名(昇順)
            // 埋め込み式の場合は、orderbyは下から順にソート。





            var selectMedicine = from x in レセ電で結合したデータ
                                 where x.個別医薬品コード.Substring(0, CheckLength).Equals(YJコード.Substring(0, CheckLength)) &&
                                   (
                                       (全期限) ||
                                       (期限内 && System.DateTime.Now.Date <= x.使用期限) ||                                                // 期限内:その日の含める
                                       (期限切 && x.使用期限 < System.DateTime.Now.Date) ||                                                 // 期限切:その日は含めない
                                       (期限指定か && 以内指定か && (x.使用期限 <= System.DateTime.Now.Date.AddMonths(期限加算月).Date)) || // ～以内:その日は含める
                                       (期限指定か && !以内指定か && (System.DateTime.Now.Date.AddMonths(期限加算月).Date <= x.使用期限))   // ～以上:その日は含める
                                   )
                                 orderby x.医薬品名 ascending
                                 orderby x.薬価 descending
                                 orderby x.後発区分 descending
                                 orderby x.店名 ascending
                                 select new 現在庫データ
                                 {
                                     店名 = x.店名,
                                     後発区分 = x.後発区分,
                                     個別医薬品コード = x.個別医薬品コード,
                                     医薬品名 = x.医薬品名,
                                     在庫数 = x.在庫数,
                                     使用期限 = x.使用期限,
                                     薬価 = x.薬価.ToString("0.0"),
                                     製造会社 = x.製造会社,
                                     名称２ = x.名称２,
                                     最終更新日時 = x.最終更新日時
                                 };


            List<現在庫データ> list = selectMedicine.ToList();

            在庫リターンデータセット<現在庫データ> rdataset = new 在庫リターンデータセット<現在庫データ>();
            if (500 < list.Count)
            {
                rdataset.エラーメッセージ = "検索件数が５００件を超えたので、中断しました。検索ワードを変更して再度検索して下さい。";
                return rdataset;
            }

            rdataset.検索キーワード = YJコード;


            rdataset.検索結果データlist = list;
            return rdataset;

        }


        /// <summary>
        /// 代替したか否かで使用量が分かれるので、必ずGroupByする。
        /// </summary>
        /// <param name="検索文字列"></param>
        /// <param name="全期間"></param>
        /// <param name="期限加算月"></param>
        /// <returns></returns>
        public 在庫リターンデータセット<薬局使用量データ> Open使用量2CSV(string 検索文字列, bool 全期間, int 期限加算月)
        {

#if DEBUG
            string FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\使用量2\total.csv";

#elif NAKAYAMA

            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPathNAKAYAMA;
            string relativePath = @"ClientBin\在庫関連\使用量2\total.csv";
            string FilePath = Path.Combine(srp, relativePath);

#else
            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPath;
            string relativePath = @"ClientBin\在庫関連\使用量2\total.csv";
            string FilePath = Path.Combine(srp, relativePath);
#endif
            // 現在日時
            DateTime now = System.DateTime.Now;
            // 検索対象期間
            DateTime 過去起点;
            if (全期間)
            {
                過去起点 = now;
            }
            else
            {
                DateTime temp = now.AddMonths(-(期限加算月));
                過去起点 = new DateTime(temp.Year, temp.Month, 1);
            }



            List<薬局使用量データ> collection = new List<薬局使用量データ>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                while (((line = streamReader.ReadLine()) != null))
                {

                    string[] stringBuffer;
                    stringBuffer = line.Split(',');

                    // カラムヘッダーの場合は飛ばす (マージしたファイルなので、複数ある場合がある）
                    if (stringBuffer[0].Equals("店舗名") && stringBuffer[1].Equals("使用年月日") && stringBuffer[2].Equals("商品コード") && stringBuffer[3].Equals("医薬品名") && stringBuffer[4].Equals("名称２") && stringBuffer[5].Equals("使用量") && stringBuffer[6].Equals("薬価") && stringBuffer[7].Equals("後発品区分") && stringBuffer[8].Equals("代替区分") && stringBuffer[9].Equals("最終更新日時"))
                    {
                        continue;
                    }

                    薬局使用量データ data = new 薬局使用量データ();

                    // １０要素ない場合は不正なデータとみなして飛ばす


                    // [0] 店舗名
                    // [1] 使用年月日
                    // [2] 商品コード
                    // [3] 医薬品名
                    // [4] 名称２
                    // [5] 使用量
                    // [6] 薬価
                    // [7] 後発品区分
                    // [8] 代替区分
                    // [9] 最終更新日時

                    if (stringBuffer.Length != 10)
                    {
                        continue;
                        //throw new Exception("CSVファイルデータが不正です。");
                    }


                    // 検索オプションの過去データ検索
                    if (!全期間 && 過去起点 != now)
                    {
                        // 使用年月日をDatetime型に変換
                        int Year;
                        if (int.TryParse(stringBuffer[1].Substring(0, 4), out Year) == false)
                        {
                            continue;
                        }
                        int Month;

                        int startindex = 0;
                        int endindex = 0;
                        startindex = stringBuffer[1].IndexOf("年");
                        endindex = stringBuffer[1].IndexOf("月");

                        // データが壊れている。
                        if (startindex == 0 || endindex == 0)
                        {
                            continue;
                        }

                        int size = endindex - startindex - 1;

                        // これもデータが壊れている。
                        if (size <= 0)
                        {
                            continue;
                        }

                        if (int.TryParse(stringBuffer[1].Substring(startindex + 1, size), out Month) == false)
                        {
                            continue;
                        }

                        // ～以内:その日は含める
                        DateTime 使用年月日 = new DateTime(Year, Month, 1);

                        if (使用年月日 < 過去起点)
                        {
                            continue;
                        }
                    }


                    // 全角空白は半角空白に置換
                    string[] 分割検索文字 = 検索文字列.Replace("　", " ").Split(' ');

                    // 店舗名、使用年月日、医薬品名、名称２にマッチしたもののみ検索結果に出す
                    bool 合致あり = true;
                    foreach (var matching in 分割検索文字)
                    {

                        if (stringBuffer[0].Contains(matching) ||
                            stringBuffer[1].Contains(matching) ||
                            stringBuffer[3].Contains(matching) ||
                            stringBuffer[4].Contains(matching))
                        {
                            continue;
                        }
                        else
                        {
                            合致あり = false;
                            continue;
                        }
                    }

                    if (!合致あり)
                    {
                        continue;
                    }
                    
                    // [0] 店舗名
                    // [1] 使用年月日
                    // [2] 商品コード
                    // [3] 医薬品名
                    // [4] 名称２
                    // [5] 使用量
                    // [6] 薬価
                    // [7] 後発品区分
                    // [8] 代替区分
                    // [9] 最終更新日時

                    data.店名 = stringBuffer[0];
                    data.使用年月 = stringBuffer[1];
                    data.医薬品名 = stringBuffer[3];
                    data.名称２ = stringBuffer[4];
                    //data.医薬品名 = stringBuffer[3] + (string.IsNullOrEmpty(stringBuffer[4]) ? "" : string.Format(" / {0}", stringBuffer[4])); //名称２がある場合は　/ 名称２を後ろにつける。
                    //data.医薬品名 = stringBuffer[4] == "" ? stringBuffer[3] : stringBuffer[4]; //名称２が空白の場合は医薬品名をセット
                    data.使用量 = stringBuffer[5];

                    decimal res;
                    if (decimal.TryParse(stringBuffer[6], out res))
                    {
                        data.薬価 = res.ToString("0.0");
                    }
                    else
                    {
                        data.薬価 = "0.0";
                    }

                    if (stringBuffer.Length == 10)
                    {
                        data.最終更新日時 = stringBuffer[9];
                    }
                    else
                    {
                        data.最終更新日時 = "";
                    }


                    collection.Add(data);

                }

            }

                    // [0] 店舗名
                    // [1] 使用年月日
                    // [2] 商品コード
                    // [3] 医薬品名
                    // [4] 名称２
                    // [5] 使用量
                    // [6] 薬価
                    // [7] 後発品区分
                    // [8] 代替区分
                    // [9] 最終更新日時


            var 代替・非代替を統合 = 
                                    (from x in collection
                                    group x by new
                                    {
                                        x.店名,
                                        x.使用年月,
                                        x.商品コード,
                                        x.医薬品名,
                                        x.名称２,
                                        //x.使用量,
                                        x.薬価,
                                        x.最終更新日時,
                                    } into grouping
                                    select new 薬局使用量データ
                                    {
                                        店名 = grouping.Key.店名,
                                        使用年月 = grouping.Key.使用年月,
                                        商品コード = grouping.Key.商品コード,
                                        医薬品名 = grouping.Key.医薬品名,
                                        名称２ = grouping.Key.名称２,
                                        薬価 = grouping.Key.薬価,
                                        最終更新日時 = grouping.Key.最終更新日時,
                                        使用量 = grouping.Sum(p => decimal.Parse(p.使用量)).ToString()
                                    }).ToList();


            在庫リターンデータセット<薬局使用量データ> rdataset = new 在庫リターンデータセット<薬局使用量データ>();

            if (1000 < 代替・非代替を統合.Count)
            //if (1000 < collection.Count)
                {
                rdataset.エラーメッセージ = "検索件数が１０００件を超えたので、中断しました。検索ワードを変更して再度検索して下さい。";
                return rdataset;
            }

            rdataset.検索キーワード = 検索文字列;

            //rdataset.検索結果データlist = collection;
            rdataset.検索結果データlist = 代替・非代替を統合;
            return rdataset;

        }


        /// <summary>
        /// こっちは使わない。
        /// </summary>
        /// <param name="検索文字列"></param>
        /// <param name="全期間"></param>
        /// <param name="期限加算月"></param>
        /// <returns></returns>
        public 在庫リターンデータセット<薬局使用量データ> Open使用量CSV(string 検索文字列, bool 全期間, int 期限加算月)
        {

#if DEBUG
            string FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\使用量\total.csv";

#elif NAKAYAMA

            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPathNAKAYAMA;
            string relativePath = @"ClientBin\在庫関連\使用量\total.csv";
            string FilePath = Path.Combine(srp, relativePath);

#else
            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPath;
            string relativePath = @"ClientBin\在庫関連\使用量\total.csv";
            string FilePath = Path.Combine(srp, relativePath);
#endif
            // 現在日時
            DateTime now = System.DateTime.Now;
            // 検索対象期間
            DateTime 過去起点;
            if (全期間)
            {
                過去起点 = now;
            }
            else
            {
                DateTime temp = now.AddMonths(-(期限加算月));
                過去起点 = new DateTime(temp.Year, temp.Month, 1);
            }



            List<薬局使用量データ> collection = new List<薬局使用量データ>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                while (((line = streamReader.ReadLine()) != null))
                {

                    string[] stringBuffer;
                    stringBuffer = line.Split(',');

                    // カラムヘッダーの場合は飛ばす (マージしたファイルなので、複数ある場合がある）
                    if (stringBuffer[0].Equals("店舗名") && stringBuffer[1].Equals("使用年月日") && stringBuffer[2].Equals("商品名") && stringBuffer[3].Equals("使用量") && stringBuffer[4].Equals("薬価"))
                    {
                        continue;
                    }

                    薬局使用量データ data = new 薬局使用量データ();

                    // ５、６、７要素ない場合は不正なデータとみなして飛ばす
                    // [0] 店舗名
                    // [1] 使用年月日
                    // [2] 商品名
                    // [3] 使用量
                    // [4] 薬価
                    // [5] 最終更新日時

                    if (stringBuffer.GetLength(0) != 5 && stringBuffer.GetLength(0) != 6 && stringBuffer.GetLength(0) != 7)
                    {
                        continue;
                        //throw new Exception("CSVファイルデータが不正です。");
                    }


                    // 検索オプションの過去データ検索
                    if (!全期間 && 過去起点 != now)
                    {
                        // 使用年月日をDatetime型に変換
                        int Year;
                        if (int.TryParse(stringBuffer[1].Substring(0, 4), out Year) == false)
                        {
                            continue;
                        }
                        int Month;

                        int startindex = 0;
                        int endindex = 0;
                        startindex = stringBuffer[1].IndexOf("年");
                        endindex = stringBuffer[1].IndexOf("月");

                        // データが壊れている。
                        if (startindex == 0 || endindex == 0)
                        {
                            continue;
                        }

                        int size = endindex - startindex - 1;

                        // これもデータが壊れている。
                        if (size <= 0)
                        {
                            continue;
                        }

                        if (int.TryParse(stringBuffer[1].Substring(startindex + 1, size), out Month) == false)
                        {
                            continue;
                        }

                        // ～以内:その日は含める
                        DateTime 使用年月日 = new DateTime(Year, Month, 1);

                        if (使用年月日 < 過去起点)
                        {
                            continue;
                        }
                    }


                    // 全角空白は半角空白に置換
                    string[] 分割検索文字 = 検索文字列.Replace("　", " ").Split(' ');

                    // 店舗名、使用年月日、商品名にマッチしたもののみ検索結果に出す
                    bool 合致あり = true;
                    foreach (var matching in 分割検索文字)
                    {
                        //if (stringBuffer[0].Contains(matching) ||
                        //    stringBuffer[1].Contains(matching) ||
                        //    stringBuffer[2].Contains(matching) ||
                        //    stringBuffer[3].Contains(matching) ||
                        //    stringBuffer[4].Contains(matching))
                        //{
                        if (stringBuffer[0].Contains(matching) ||
                            stringBuffer[1].Contains(matching) ||
                            stringBuffer[2].Contains(matching))
                        {
                            continue;
                        }
                        else
                        {
                            合致あり = false;
                            continue;
                        }
                    }

                    if (!合致あり)
                    {
                        continue;
                    }

                    // [0] 店舗名
                    // [1] 使用年月日
                    // [2] 商品名
                    // [3] 使用量
                    // [4] 薬価
                    // [5] 最終更新日時

                    data.店名 = stringBuffer[0];
                    data.使用年月 = stringBuffer[1];
                    data.医薬品名 = stringBuffer[2];
                    data.使用量 = stringBuffer[3];

                    decimal res;
                    if (decimal.TryParse(stringBuffer[4], out res))
                    {
                        data.薬価 = res.ToString("0.0");
                    }
                    else
                    {
                        data.薬価 = "0.0";
                    }

                    if (stringBuffer.GetLength(0) == 6)
                    {
                        data.最終更新日時 = stringBuffer[5];
                    }
                    else
                    {
                        data.最終更新日時 = "";
                    }


                    collection.Add(data);

                }

            }


            在庫リターンデータセット<薬局使用量データ> rdataset = new 在庫リターンデータセット<薬局使用量データ>();

            if (1000 < collection.Count)
            {
                rdataset.エラーメッセージ = "検索件数が１０００件を超えたので、中断しました。検索ワードを変更して再度検索して下さい。";
                return rdataset;
            }

            rdataset.検索キーワード = 検索文字列;

            rdataset.検索結果データlist = collection;
            return rdataset;

        }


        public 在庫リターンデータセット<不動品データ> Open不動品CSV(string 検索文字列, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月)
        {
#if DEBUG
            string FilePath = @"C:\PharmacyTools\ClientBin\在庫関連\不動品\total.csv";

#elif NAKAYAMA

            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPathNAKAYAMA;
            string relativePath = @"ClientBin\在庫関連\不動品\total.csv";
            string FilePath = Path.Combine(srp, relativePath);
#else
            string srp = PharmacyTool.Web.Properties.Settings.Default.ServiceRootPath;
            string relativePath = @"ClientBin\在庫関連\不動品\total.csv";
            string FilePath = Path.Combine(srp, relativePath);
#endif

            在庫リターンデータセット<不動品データ> rdataset = new 在庫リターンデータセット<不動品データ>();

            List<不動品データ> collection = new List<不動品データ>();

            if (string.IsNullOrEmpty(検索文字列))
            {
                rdataset.エラーメッセージ = "検索キーワードが入力されていません。キーワードを入力後、再検索を行って下さい。";
                rdataset.検索結果データlist = collection;
                return rdataset;
            }

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {

                string line = "";
                while (((line = streamReader.ReadLine()) != null))
                {

                    string[] stringBuffer;
                    stringBuffer = line.Split(',');
                    不動品データ data = new 不動品データ();

                    // 不正なデータは飛ばす
                    if (stringBuffer.GetLength(0) != 7 && stringBuffer.GetLength(0) != 9)  //2015.05.02変更
                    {
                        continue;
                        //throw new Exception("CSVファイルデータが不正です。");
                    }


                    // カラムヘッダーの場合は飛ばす
                    if (stringBuffer[0].Equals("店舗名") && stringBuffer[1].Equals("薬品コード") && stringBuffer[2].Equals("薬品名") && stringBuffer[3].Equals("現在庫") && stringBuffer[4].Equals("使用期限") && stringBuffer[5].Equals("薬価ｂ"))
                    {
                        continue;
                    }


                    // [0]店舗名
                    // [1]薬品コード
                    // [2]薬品名
                    // [3]現在庫
                    // [4]使用期限
                    // [5]薬価ｂ
                    // [6]１包単位量  //2015.05.02追加
                    // [7]名称２      //2015.05.02追加

                    // [8]最終更新日時

                    data.店名 = stringBuffer[0];
                    data.レセプト電算処理システムコード = stringBuffer[1];
                    data.医薬品名 = stringBuffer[2];
                    data.在庫数 = stringBuffer[3];

                    double result;
                    if (double.TryParse(stringBuffer[5], out result) == false)
                    {
                        continue;
                    }
                    data.薬価 = result;

                    // 日付を整形 YYYY/MM/DD
                    // 月までしか入っていないので、日にちは１日とされる
                    DateTime result2;
                    if (DateTime.TryParse(stringBuffer[4], out result2))
                    {
                        //DateTime temp = result2.AddMonths(1).AddDays(-1);　// 不動品データ.csvには月までのデータなので、その月の月末を期限日とする。

                        //// 9999/12/1はカウントアップできない。
                        if (new DateTime(1900, 1, 1) <= result2.Date && result2.Date < new DateTime(9999, 12, 1))
                        {
                            // システム対応-使用期限を月末に設定
                            // レセコン出力データは1日で出てくる為、ユーザー希望でその月の月末とする
                            data.使用期限 = result2.Date.AddMonths(1).AddDays(-1);
                            //data.使用期限 = result2.Date;

                        }
                        else
                        {
                            data.使用期限 = result2.Date;
                        }
                    }
                    else
                    {
                        data.使用期限 = new DateTime(9999, 12, 1);
                        //data.使用期限 = new DateTime(9999, 12, 31);
                    }


                    bool 対象か = false;
                    if (全期限)
                    {
                        対象か = true;
                    }


                    // 期限内:その日の含める
                    else if (期限内)
                    {
                        if (System.DateTime.Now.Date <= data.使用期限)
                        {
                            対象か = true;
                        }
                    }
                    // 期限切:その日は含めない
                    else if (期限切)
                    {
                        if (data.使用期限 < System.DateTime.Now.Date)
                        {
                            対象か = true;
                        }
                    }
                    else if (期限指定か)
                    {
                        // ～以内:その日は含める
                        if (以内指定か)
                        {
                            if (data.使用期限 <= System.DateTime.Now.Date.AddMonths(期限加算月))
                            {
                                対象か = true;
                            }
                        }
                        // ～以上:その日は含める
                        else
                        {
                            if (System.DateTime.Now.Date.AddMonths(期限加算月) <= data.使用期限)
                            {
                                対象か = true;
                            }
                        }
                    }
                    else
                    {
                        対象か = false;
                    }

                    if (!対象か)
                    {
                        continue;
                    }

                    data.一包単位量 = "";
                    data.名称２ = "";

                    //2015.05.02変更
                    if (stringBuffer.GetLength(0) == 7)
                    {
                        data.最終更新日時 = stringBuffer[6];
                    }
                    else if (stringBuffer.GetLength(0) == 9)
                    {
                        data.一包単位量 = stringBuffer[6];
                        data.名称２ = stringBuffer[7];
                        data.最終更新日時 = stringBuffer[8];

                    }
                    else
                    {
                        data.最終更新日時 = "";
                    }


                    collection.Add(data);

                }

            }

#if DEBUG
            FilePath = PharmacyTool.Web.Properties.Settings.Default.MEDISFilePathDEBUG;

#elif NAKAYAMA

            FilePath = PharmacyTool.Web.Properties.Settings.Default.MEDISFilePathNAKAYAMA;

#else
            FilePath = PharmacyTool.Web.Properties.Settings.Default.MEDISFilePath;

#endif


            List<MEDISデータ> collection2 = new List<MEDISデータ>();

            using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] stringBuffer;
                    stringBuffer = line.Split(',');
                    MEDISデータ data = new MEDISデータ();

                    if (stringBuffer.GetLength(0) < 24)
                    {
                        continue;
                        throw new Exception("MEDIS.TXTファイルデータが不正です。");
                    }

                    int result;

                    if (stringBuffer[8].Replace('"', ' ').Replace(" ", "").Equals("") || int.TryParse(stringBuffer[8].Replace('"', ' ').Replace(" ", ""), out result) == false)
                    {
                        continue;
                    }


                    data.薬価基準収載医薬品コード = stringBuffer[6].Replace('"', ' ').Replace(" ", "");
                    data.個別医薬品コード = stringBuffer[7].Replace('"', ' ').Replace(" ", "");
                    data.レセプト電算処理システムコード = stringBuffer[8].Replace('"', ' ').Replace(" ", "");


                    collection2.Add(data);

                }
            }

            // MEDISのデータは、併売で一つの個別医薬品コードに対して複数の販売会社データも持っている為、重複を省く
            var distinctcollection2 = (from x in collection2
                                       select new DistinctCheck
                                       {
                                           個別医薬品コード = x.個別医薬品コード,
                                           レセプト電算処理システムコード = x.レセプト電算処理システムコード,
                                           薬価基準収載医薬品コード = x.薬価基準収載医薬品コード
                                       }).Distinct(new DistinctCheckComparer());

            //var distinctcollection2 = (from x in collection2
            //                           select new
            //                           {
            //                               x.個別医薬品コード,
            //                               x.レセプト電算処理システムコード
            //                           }).Distinct();





            var レセ電で結合したデータリスト = from x in collection
                                 join p in distinctcollection2 on x.レセプト電算処理システムコード.Substring(0, 9) equals p.レセプト電算処理システムコード into mergeCollection
                                 from outputCollection in mergeCollection.DefaultIfEmpty()
                                 select new 不動品データ
                                 {
                                     店名 = x.店名,
                                     個別医薬品コード = outputCollection == null ? "" : outputCollection.個別医薬品コード,
                                     医薬品名 = x.医薬品名,
                                     在庫数 = x.在庫数,
                                     使用期限 = x.使用期限,
                                     薬価 = x.薬価,
                                     レセプト電算処理システムコード = x.レセプト電算処理システムコード.Substring(0, 9),
                                     名称２ = x.名称２,
                                     一包単位量 = x.一包単位量,
                                     最終更新日時 = x.最終更新日時
                                 };





            string[] sepa = 検索文字列.Replace("　", " ").Split(' ');

            List<不動品データ> returnValue = new List<不動品データ>();
            foreach (var レセ電結合データ in レセ電で結合したデータリスト.ToList())
            {
                // 店名、コード、医薬品名、名称２、医薬品名と名称２連結のいずれかにマッチしたデータのみ検索結果に表示する。
                bool 一致 = true;
                foreach (var matching in sepa)
                {
                    if (レセ電結合データ.医薬品名.Contains(matching) ||
                        レセ電結合データ.店名.Contains(matching) ||
                        レセ電結合データ.個別医薬品コード.Contains(matching) ||
                        レセ電結合データ.医薬品名と名称２連結.Contains(matching) ||
                        レセ電結合データ.名称２.Contains(matching)
                        )
                    {
                        continue;
                    }
                    else
                    {
                        一致 = false;
                    }
                }

                //// 個別医薬品コードは異なるが、レセ電コードが一緒のものは省く
                //var check = from x in returnValue
                //            where x.レセプト電算処理システムコード.Equals(レセ電結合データ.レセプト電算処理システムコード) &&
                //                    x.医薬品名.Equals(レセ電結合データ.医薬品名) &&
                //                    x.在庫数.Equals(レセ電結合データ.在庫数) &&
                //                    x.使用期限.Equals(レセ電結合データ.使用期限) &&
                //                    x.店名.Equals(レセ電結合データ.店名) &&
                //                    x.薬価.Equals(レセ電結合データ.薬価)
                //            select new
                //            {

                //            };

                //if (1 <= check.Count())
                //{
                //    continue;
                //}

                if (一致)
                {
                    returnValue.Add(レセ電結合データ);
                }
            }


            if (500 < returnValue.Count)
            {
                rdataset.エラーメッセージ = "検索件数が５００件を超えたので、中断しました。検索ワードを変更して再度検索して下さい。";
                return rdataset;
            }

            rdataset.検索キーワード = 検索文字列;

            rdataset.検索結果データlist = returnValue;
            return rdataset;


        }


        /// <summary>
        /// 掲示板データを取得する
        /// </summary>
        /// <param name="カテゴリ名">取得するカテゴリ</param>
        /// <param name="グループNo">何グループ目のデータを取得するか</param>
        /// <returns></returns>
        public 掲示板リターンデータセット Open掲示板データ(string カテゴリ名, int グループNo)
        {

            if (グループNo <= 0)
            {
                throw new Exception("指定のページ番号に誤りがあります。");
            }

#if DEBUG
            string RootPath = @"C:\PharmacyTools\ClientBin\掲示板";
            string DirectoryRootPath = Path.Combine(RootPath, カテゴリ名);

#elif NAKAYAMA

            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPathNAKAYAMA;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);

#else
            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPath;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);
#endif
            掲示板リターンデータセット dataset = new 掲示板リターンデータセット();

            List<Dictionary<投稿Entity, List<返信Entity>>> result = new List<Dictionary<投稿Entity, List<返信Entity>>>();
            List<string> list = Directory.GetDirectories(DirectoryRootPath, "*", SearchOption.TopDirectoryOnly).ToList();

            Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
            foreach (var ls in list)
            {
                string[] DirectoryArray = ls.Split('\\');
                string DirectoryName = DirectoryArray[DirectoryArray.Length - 1];

                string[] str = Directory.GetDirectories(ls, "*", SearchOption.TopDirectoryOnly);

                foreach (var s in str)
                {
                    if (dic.ContainsKey(ls))
                    {
                        DateTime dt = Directory.GetLastWriteTime(s);
                        if (dic[ls] < dt)
                        {
                            dic.Remove(ls);
                            dic.Add(ls, dt);
                        }
                    }
                    else
                    {
                        dic.Add(ls, Directory.GetLastWriteTime(s));
                    }
                }
            }


            var sortWriteTime = from x in dic
                                orderby x.Value descending
                                select new
                                {
                                    str = x.Key
                                };


            int endindex = グループNo * 10;
            int startindex = endindex - 9;

            // ありえないはずだが、念のためチェック
            if (sortWriteTime.Count() != 0 && sortWriteTime.Count() < startindex)
            {
                throw new Exception("記事データがありません。");
            }


            int cnt = 1;
            foreach (var dir in sortWriteTime)
            {
                if (startindex <= cnt && cnt <= endindex)
                {

#if DEBUG

                    string[] dir_array = dir.str.Split('\\');

                    if (dir_array[dir_array.Length - 1].Equals(".svn"))
                    {
                        continue;
                    }
#endif

                    Dictionary<投稿Entity, List<返信Entity>> 記事set = new Dictionary<投稿Entity, List<返信Entity>>();
                    List<返信Entity> 返信set = new List<返信Entity>();

                    //メイン記事の取得
                    string mainPath = Path.Combine(dir.str, "メイン");
                    string[] mainFilePath = Directory.GetFiles(mainPath);

                    if (mainFilePath.GetLength(0) != 1)
                    {
                        throw new Exception("記事のデータが存在しない為、データを取得出来ませんでした。");
                    }

                    //ファイル名を取得
                    string[] sep = mainFilePath[0].Split('\\');
                    string filename = sep[sep.GetLength(0) - 1];

                    投稿Entity 投稿ent = new 投稿Entity();
                    using (StreamReader st = new StreamReader(mainFilePath[0], Encoding.GetEncoding("Shift_JIS")))
                    {
                        string line = "";
                        int counter = 1;
                        List<string> 添付ファイルパスlist = new List<string>();

                        while ((line = st.ReadLine()) != null)
                        {
                            // [0]タイトル
                            // [1]投稿者名
                            // [2]投稿日付
                            // [3]HomepageURL
                            // [4]Email
                            // [5]暗証キー
                            // [6]文字色
                            if (counter == 1)
                            {
                                string[] sepa = line.Split(',');
                                if (sepa.GetLength(0) != 7)
                                {
                                    throw new Exception("記事のデータが壊れている為、データを取得出来ませんでした。");
                                }
                                string no = filename.Substring(0, 8);
                                while (no.Substring(0, 1).Equals("0"))
                                {
                                    no = no.Remove(0, 1);
                                }

                                投稿ent.No = string.Format("No:{0}", no);
                                投稿ent.Title = sepa[0];
                                投稿ent.投稿者名 = sepa[1];
                                投稿ent.投稿日 = sepa[2];
                                投稿ent.HomepageUrl = sepa[3];
                                投稿ent.Email = sepa[4];
                                投稿ent.文字色 = sepa[6];
                                投稿ent.カテゴリ名 = カテゴリ名;
                            }
                            else if (counter == 2)
                            {
                                string removedStr = line.Replace("[ImagePath]=", "");

                                if (!removedStr.Equals(""))
                                {
                                    string[] sepa = removedStr.Split(',');
                                    foreach (var path in sepa)
                                    {
                                        添付ファイルパスlist.Add(path);
                                    }
                                }

                            }
                            else
                            {
                                if (counter == 3)
                                {
                                    投稿ent.記事 = line;
                                }
                                else
                                {
                                    投稿ent.記事 += "\n" + line;
                                }

                            }

                            counter++;


                        }

                        投稿ent.添付ファイルlist = 添付ファイルパスlist;
                    }


                    //レスの取得
                    string resPath = Path.Combine(dir.str, "レス");
                    string[] resFilePath = Directory.GetFiles(resPath);

                    foreach (var res in resFilePath)
                    {
                        //ファイル名を取得
                        string[] ressepa = res.Split('\\');
                        string resfilename = ressepa[ressepa.GetLength(0) - 1];

                        返信Entity 返信ent = new 返信Entity();

                        using (StreamReader st = new StreamReader(res, Encoding.GetEncoding("Shift_JIS")))
                        {
                            string line = "";
                            int counter = 1;
                            List<string> 添付ファイルパスlist = new List<string>();
                            while ((line = st.ReadLine()) != null)
                            {

                                // [0]タイトル
                                // [1]投稿者名
                                // [2]投稿日付
                                // [3]HomepageURL
                                // [4]Email
                                // [5]暗証キー
                                // [6]文字色
                                if (counter == 1)
                                {

                                    string[] sepa = line.Split(',');
                                    if (sepa.GetLength(0) != 7)
                                    {
                                        throw new Exception("記事のデータが壊れている為、データを取得出来ませんでした。");
                                    }
                                    string no = resfilename.Substring(0, 8);
                                    while (no.Substring(0, 1).Equals("0"))
                                    {
                                        no = no.Remove(0, 1);
                                    }
                                    返信ent.No = string.Format("No:{0}", no);
                                    返信ent.Title = sepa[0];
                                    返信ent.投稿者名 = sepa[1];
                                    返信ent.投稿日 = sepa[2];
                                    返信ent.HomepageUrl = sepa[3];
                                    返信ent.Email = sepa[4];
                                    返信ent.文字色 = sepa[6];
                                    返信ent.カテゴリ名 = カテゴリ名;
                                }
                                else if (counter == 2)
                                {

                                    string removedStr = line.Replace("[ImagePath]=", "");
                                    if (!removedStr.Equals(""))
                                    {
                                        string[] sepa = removedStr.Split(',');
                                        foreach (var path in sepa)
                                        {
                                            添付ファイルパスlist.Add(path);
                                        }
                                    }

                                }
                                else
                                {
                                    if (counter == 3)
                                    {
                                        返信ent.記事 = line;
                                    }
                                    else
                                    {
                                        返信ent.記事 += "\n" + line;
                                    }
                                }


                                counter++;

                            }

                            返信ent.添付ファイルlist = 添付ファイルパスlist;

                        }

                        返信set.Add(返信ent);

                    }

                    記事set.Add(投稿ent, 返信set);

                    result.Add(記事set);

                }

                cnt++;
            }

            dataset.掲示板データ = result;
            dataset.作成グループNo = グループNo;
            dataset.メイン記事数 = sortWriteTime.Count();

            return dataset;
        }


        /// <summary>
        /// キーワード検索で掲示板データを取得
        /// </summary>
        /// <param name="キーワード"></param>
        /// <param name="カテゴリ名"></param>
        /// <returns></returns>
        public 掲示板リターンデータセット Open掲示板データキーワード検索(string キーワード, string カテゴリ名)
        {

            //if (グループNo <= 0)
            //{
            //    throw new Exception("指定のページ番号に誤りがあります。");
            //}

#if DEBUG
            string RootPath = @"C:\PharmacyTools\ClientBin\掲示板";
            string DirectoryRootPath = Path.Combine(RootPath, カテゴリ名);
#elif NAKAYAMA

            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPathNAKAYAMA;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);

#else
            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPath;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);
#endif
            掲示板リターンデータセット dataset = new 掲示板リターンデータセット();
            dataset.キーワード検索結果か = true;
            dataset.検索したキーワード = キーワード;

            List<Dictionary<投稿Entity, List<返信Entity>>> result = new List<Dictionary<投稿Entity, List<返信Entity>>>();
            List<string> list = Directory.GetDirectories(DirectoryRootPath, "*", SearchOption.TopDirectoryOnly).ToList();

            Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
            foreach (var ls in list)
            {
                string[] DirectoryArray = ls.Split('\\');
                string DirectoryName = DirectoryArray[DirectoryArray.Length - 1];

                string[] str = Directory.GetDirectories(ls, "*", SearchOption.TopDirectoryOnly);

                foreach (var s in str)
                {
                    if (dic.ContainsKey(ls))
                    {
                        DateTime dt = Directory.GetLastWriteTime(s);
                        if (dic[ls] < dt)
                        {
                            dic.Remove(ls);
                            dic.Add(ls, dt);
                        }
                    }
                    else
                    {
                        dic.Add(ls, Directory.GetLastWriteTime(s));
                    }
                }
            }


            var sortWriteTime = from x in dic
                                orderby x.Value descending
                                select new
                                {
                                    str = x.Key
                                };


            //int endindex = グループNo * 10;
            //int startindex = endindex - 9;

            //// ありえないはずだが、念のためチェック
            //if (sortWriteTime.Count() != 0 && sortWriteTime.Count() < startindex)
            //{
            //    throw new Exception("記事データがありません。");
            //}


            int cnt = 1;

            // 全角もしくは半角スペースでキーワードを分割
            string[] 検索ワード群 = キーワード.Replace("　", " ").Split(' ');
            foreach (var dir in sortWriteTime)
            {
                //if (startindex <= cnt && cnt <= endindex)
                //{

#if DEBUG

                string[] dir_array = dir.str.Split('\\');

                if (dir_array[dir_array.Length - 1].Equals(".svn"))
                {
                    continue;
                }
#endif

                Dictionary<投稿Entity, List<返信Entity>> 記事set = new Dictionary<投稿Entity, List<返信Entity>>();
                List<返信Entity> 返信set = new List<返信Entity>();



                // メイン記事パス
                string mainPath = Path.Combine(dir.str, "メイン");
                string[] mainFilePath = Directory.GetFiles(mainPath);
                if (mainFilePath.GetLength(0) != 1)
                {
                    throw new Exception("記事のデータが存在しない為、データを取得出来ませんでした。");
                }
                string[] sep = mainFilePath[0].Split('\\');
                string filename = sep[sep.GetLength(0) - 1];                 //ファイル名を取得


                //レスの取得
                string resPath = Path.Combine(dir.str, "レス");
                string[] resFilePath = Directory.GetFiles(resPath);



                // キーワードに一致するか先に確認
                // レスかメイン記事に含まれていれば表示する
                bool キーワード一致 = false;

                //メイン記事のキーワード一致確認
                bool 全て含んでいる = true;
                using (StreamReader st = new StreamReader(mainFilePath[0], Encoding.GetEncoding("Shift_JIS")))
                {
                    string s = st.ReadToEnd();
                    foreach (var word in 検索ワード群)
                    {
                        if (!s.Contains(word))
                        {
                            全て含んでいる = false;
                            break;

                        }
                    }

                }

                if (全て含んでいる)
                {
                    キーワード一致 = true;
                }


                if (!キーワード一致)
                {
                    // レス記事のキーワード一致確認
                    foreach (var res in resFilePath)
                    {
                        //ファイル名を取得
                        string[] ressepa = res.Split('\\');
                        string resfilename = ressepa[ressepa.GetLength(0) - 1];
                        bool 全て含んでいる2 = true;

                        using (StreamReader st = new StreamReader(res, Encoding.GetEncoding("Shift_JIS")))
                        {
                            string s = st.ReadToEnd();
                            foreach (var word in 検索ワード群)
                            {
                                if (!s.Contains(word))
                                {
                                    全て含んでいる2 = false;
                                    break;
                                }
                            }
                        }

                        if (全て含んでいる2)
                        {
                            キーワード一致 = true;
                            break;
                        }
                    }

                }

                // キーワード全一致確認完了
                if (!キーワード一致)
                {
                    continue;
                }


                //メイン記事の取得
                投稿Entity 投稿ent = new 投稿Entity();
                using (StreamReader st = new StreamReader(mainFilePath[0], Encoding.GetEncoding("Shift_JIS")))
                {

                    string line = "";
                    int counter = 1;
                    List<string> 添付ファイルパスlist = new List<string>();

                    while ((line = st.ReadLine()) != null)
                    {
                        // [0]タイトル
                        // [1]投稿者名
                        // [2]投稿日付
                        // [3]HomepageURL
                        // [4]Email
                        // [5]暗証キー
                        // [6]文字色

                        if (counter == 1)
                        {
                            string[] sepa = line.Split(',');
                            if (sepa.GetLength(0) != 7)
                            {
                                throw new Exception("記事のデータが壊れている為、データを取得出来ませんでした。");
                            }
                            string no = filename.Substring(0, 8);
                            while (no.Substring(0, 1).Equals("0"))
                            {
                                no = no.Remove(0, 1);
                            }

                            投稿ent.No = string.Format("No:{0}", no);
                            投稿ent.Title = sepa[0];
                            投稿ent.投稿者名 = sepa[1];
                            投稿ent.投稿日 = sepa[2];
                            投稿ent.HomepageUrl = sepa[3];
                            投稿ent.Email = sepa[4];
                            投稿ent.文字色 = sepa[6];
                            投稿ent.カテゴリ名 = カテゴリ名;
                        }
                        else if (counter == 2)
                        {
                            string removedStr = line.Replace("[ImagePath]=", "");

                            if (!removedStr.Equals(""))
                            {
                                string[] sepa = removedStr.Split(',');
                                foreach (var path in sepa)
                                {
                                    添付ファイルパスlist.Add(path);
                                }
                            }

                        }
                        else
                        {
                            if (counter == 3)
                            {
                                投稿ent.記事 = line;
                            }
                            else
                            {
                                投稿ent.記事 += "\n" + line;
                            }

                        }

                        counter++;


                    }

                    投稿ent.添付ファイルlist = 添付ファイルパスlist;
                }


                //レスの取得

                foreach (var res in resFilePath)
                {
                    //ファイル名を取得
                    string[] ressepa = res.Split('\\');
                    string resfilename = ressepa[ressepa.GetLength(0) - 1];

                    返信Entity 返信ent = new 返信Entity();

                    using (StreamReader st = new StreamReader(res, Encoding.GetEncoding("Shift_JIS")))
                    {
                        string line = "";
                        int counter = 1;
                        List<string> 添付ファイルパスlist = new List<string>();
                        while ((line = st.ReadLine()) != null)
                        {

                            // [0]タイトル
                            // [1]投稿者名
                            // [2]投稿日付
                            // [3]HomepageURL
                            // [4]Email
                            // [5]暗証キー
                            // [6]文字色


                            if (counter == 1)
                            {

                                string[] sepa = line.Split(',');
                                if (sepa.GetLength(0) != 7)
                                {
                                    throw new Exception("記事のデータが壊れている為、データを取得出来ませんでした。");
                                }
                                string no = resfilename.Substring(0, 8);
                                while (no.Substring(0, 1).Equals("0"))
                                {
                                    no = no.Remove(0, 1);
                                }
                                返信ent.No = string.Format("No:{0}", no);
                                返信ent.Title = sepa[0];
                                返信ent.投稿者名 = sepa[1];
                                返信ent.投稿日 = sepa[2];
                                返信ent.HomepageUrl = sepa[3];
                                返信ent.Email = sepa[4];
                                返信ent.文字色 = sepa[6];
                                返信ent.カテゴリ名 = カテゴリ名;

                            }
                            else if (counter == 2)
                            {

                                string removedStr = line.Replace("[ImagePath]=", "");
                                if (!removedStr.Equals(""))
                                {
                                    string[] sepa = removedStr.Split(',');
                                    foreach (var path in sepa)
                                    {
                                        添付ファイルパスlist.Add(path);
                                    }
                                }

                            }
                            else
                            {
                                if (counter == 3)
                                {
                                    返信ent.記事 = line;
                                }
                                else
                                {
                                    返信ent.記事 += "\n" + line;
                                }
                            }


                            counter++;

                        }

                        返信ent.添付ファイルlist = 添付ファイルパスlist;

                    }

                    返信set.Add(返信ent);

                }

                記事set.Add(投稿ent, 返信set);

                result.Add(記事set);

                //}

                cnt++;
            }

            if (result.Count == 100)
            {
                dataset.記事取得成功か = false;
                dataset.エラーメッセージ = "検索ワードに一致した記事数を１００を超えました。\r\n検索ワードを変更して再検索して下さい。";
            }
            else
            {
                dataset.記事取得成功か = true;
                dataset.掲示板データ = result;
            }

            //dataset.作成グループNo = グループNo;
            //dataset.メイン記事数 = sortWriteTime.Count();



            return dataset;
        }

        public 掲示板記事修正確認Entity 掲示板記事修正確認チェック(string カテゴリ名, string 記事No, string 暗証キー, bool 管理者か)
        {

            掲示板記事修正確認Entity 記事確認Ent = new 掲示板記事修正確認Entity();

            if (string.IsNullOrEmpty(カテゴリ名))
            {
                記事確認Ent.暗証キーチェック成功 = false;
                記事確認Ent.エラーメッセージ = "カテゴリ名が不明の為、修正出来ませんでした。";
                return 記事確認Ent;
            }

            if (string.IsNullOrEmpty(記事No))
            {
                記事確認Ent.暗証キーチェック成功 = false;
                記事確認Ent.エラーメッセージ = "記事Noが不明の為、修正出来ませんでした。";
                return 記事確認Ent;
            }

            if (!管理者か && string.IsNullOrEmpty(暗証キー))
            {
                記事確認Ent.暗証キーチェック成功 = false;
                記事確認Ent.エラーメッセージ = "暗証キーが不明の為、修正出来ませんでした。";
                return 記事確認Ent;
            }

#if DEBUG
            string RootPath = @"C:\PharmacyTools\ClientBin\掲示板";
            string DirectoryRootPath = Path.Combine(RootPath, カテゴリ名);
#elif NAKAYAMA

            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPathNAKAYAMA;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);

#else
            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPath;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);
#endif

            string Search記事No = 記事No.PadLeft(8, '0');
            List<string> allFiles = Directory.GetFiles(DirectoryRootPath, "*.txt", SearchOption.AllDirectories).ToList();

            string TargetFilePath = "";
            foreach (var file in allFiles)
            {
                string[] sepa = file.Split('\\');
                string filename = sepa[sepa.Length - 1];

                string[] sepa2 = filename.Split('.');
                if (sepa2.Length < 2)
                {
                    記事確認Ent.暗証キーチェック成功 = false;
                    記事確認Ent.エラーメッセージ = "記事データが壊れている為(拡張子がない)、修正出来ませんでした。";
                    return 記事確認Ent;
                }
                string 名称 = sepa2[sepa2.Length - 2];

                if (Search記事No.Equals(名称))
                {
                    TargetFilePath = file;
                    break;
                }
            }

            if (string.IsNullOrEmpty(TargetFilePath))
            {
                記事確認Ent.暗証キーチェック成功 = false;
                記事確認Ent.エラーメッセージ = "削除対象の記事が存在しないの為、修正出来ませんでした。";
                return 記事確認Ent;
            }

            投稿Entity 投稿Ent = new 投稿Entity();

            using (StreamReader sr = new StreamReader(TargetFilePath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    // 1行目
                    if (counter == 1)
                    {
                        string[] sepa3 = line.Split(',');
                        if (sepa3.Length != 7)
                        {
                            記事確認Ent.暗証キーチェック成功 = false;
                            記事確認Ent.エラーメッセージ = "記事データが壊れている為(カンマ区切り不足)、修正出来ませんでした。";
                            return 記事確認Ent;
                        }

                        // [0]タイトル
                        // [1]投稿者名
                        // [2]投稿日付
                        // [3]HomepageURL
                        // [4]Email
                        // [5]暗証キー
                        // [6]文字色

                        // 暗証キーが一致しなかったら失敗
                        if (!管理者か && !sepa3[5].Equals(暗証キー))
                        {
                            記事確認Ent.暗証キーチェック成功 = false;
                            記事確認Ent.エラーメッセージ = "暗証キーが一致しなかった為、修正出来ませんでした。";
                            return 記事確認Ent;
                        }

                        投稿Ent.Title = sepa3[0];
                        投稿Ent.投稿者名 = sepa3[1];
                        投稿Ent.投稿日 = sepa3[2];
                        投稿Ent.HomepageUrl = sepa3[3];
                        投稿Ent.Email = sepa3[4];
                        投稿Ent.暗証キー = sepa3[5];
                        投稿Ent.文字色 = sepa3[6];
                        投稿Ent.No = 記事No;

                        counter++;
                    }
                    // 2行目
                    else if (counter == 2)
                    {
                        string removedStr = line.Replace("[ImagePath]=", "");

                        List<string> 添付ファイルリスト = new List<string>();
                        if (!removedStr.Equals(""))
                        {
                            string[] sepa4 = removedStr.Split(',');
                            foreach (var path in sepa4)
                            {
                                添付ファイルリスト.Add(path);
                            }
                        }

                        投稿Ent.添付ファイルlist = 添付ファイルリスト;

                        counter++;

                    }
                    // 3行目以降
                    else
                    {
                        if (counter == 3)
                        {
                            投稿Ent.記事 = line;
                        }
                        else
                        {
                            投稿Ent.記事 += "\n" + line;
                        }
                        counter++;
                    }

                }

            }

            記事確認Ent.記事データ = 投稿Ent;
            記事確認Ent.暗証キーチェック成功 = true;
            return 記事確認Ent;

        }

        public 最終更新日時リターンデータセット GetMEDISデータ最終更新日時()
        {

#if DEBUG

            string MEDISFilePath = Settings.Default.MEDISFilePathDEBUG;

#elif NAKAYAMA

            string MEDISFilePath = Settings.Default.MEDISFilePathNAKAYAMA;

#else

            string MEDISFilePath = Settings.Default.MEDISFilePath;

#endif

            最終更新日時リターンデータセット rdataset = new 最終更新日時リターンデータセット();

            try
            {
                string MEDIS最終更新日時 = "";

                if (System.IO.File.Exists(MEDISFilePath))
                {
                    MEDIS最終更新日時 = System.IO.File.GetLastWriteTime(MEDISFilePath).ToString("yyyy/MM/dd HH:mm");
                }
                rdataset.データ取得成功か = true;
                rdataset.MEDIS最終更新日時 = string.Format("MEDISデータ最終更新日時 : {0}", MEDIS最終更新日時);

            }
            catch (Exception ex)
            {
                rdataset.データ取得成功か = false;
                rdataset.エラーメッセージ = ex.Message + ex.StackTrace;
            }

            return rdataset;


        }


        public テロップ記事リターンEntity テロップ記事読み込み()
        {
#if DEBUG
            string TeropFilePath = PharmacyTool.Web.Properties.Settings.Default.TeropFilePathDEBUG;
#elif NAKAYAMA

            string TeropFilePath = PharmacyTool.Web.Properties.Settings.Default.TeropFilePathNAKAYAMA;

#else
            string TeropFilePath = PharmacyTool.Web.Properties.Settings.Default.TeropFilePath;
#endif
            StringBuilder sb = new StringBuilder();

            テロップ記事リターンEntity ent = new テロップ記事リターンEntity();

            try
            {
                using (StreamReader sr = new StreamReader(TeropFilePath, Encoding.GetEncoding(932)))
                {
                    string line = "";
                    int counter = 1;
                    while (((line = sr.ReadLine()) != null))
                    {
                        if (counter == 1)
                        {
                            sb.Append(line);
                        }
                        else
                        {
                            sb.Append("\r");
                            sb.Append(line);
                        }
                        counter++;
                    }
                }
            }
            catch (Exception exp)
            {

                ent.読み込み成功か = false;
                ent.エラーメッセージ = exp.Message + exp.StackTrace;
                ent.テロップ記事 = "";

                return ent;

            }

            ent.読み込み成功か = true;
            ent.エラーメッセージ = "";
            ent.テロップ記事 = sb.ToString();

            return ent;


        }


        public List<ThreadTitlesEntity> GetThreadTitles()
        {

#if DEBUG
            string RootPath = @"C:\PharmacyTools\ClientBin\掲示板";
#elif NAKAYAMA

            string RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPathNAKAYAMA;

#else
            string RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPath;
#endif

            List<ThreadTitlesEntity> tempresult = new List<ThreadTitlesEntity>();


            List<string> directorylist = Directory.GetDirectories(RootPath, "*", SearchOption.TopDirectoryOnly).ToList();


            foreach (var dir in directorylist)
            {

#if DEBUG

                string[] dir_array = dir.Split('\\');

                if (dir_array[dir_array.Length - 1].Equals(".svn"))
                {
                    continue;
                }
#endif

                string imagefilepath = "";
                string datfilepath = Path.Combine(dir, "Thread.dat");
                DateTime 作成日 = Directory.GetCreationTime(dir);
                ThreadTitlesEntity tte = new ThreadTitlesEntity();

                using (StreamReader st = new StreamReader(datfilepath, Encoding.GetEncoding("Shift_JIS")))
                {
                    string line = "";
                    while ((line = st.ReadLine()) != null)
                    {
                        //先頭から8バイトは属性 [image]=
                        imagefilepath = line.Remove(0, 8);

                    }
                }
                string[] threadname = dir.Split('\\');
                tte.スレッド名 = threadname[threadname.GetLength(0) - 1];
                tte.画像パス = imagefilepath;
                tte.スレッド作成日 = 作成日;

                tempresult.Add(tte);

            }


            List<ThreadTitlesEntity> result = tempresult.OrderBy(t => t.スレッド作成日).ToList();

            return result;
        }

        public 在庫MergeリターンEntity DoMerge(MergeType タイプ)
        {
            在庫MergeリターンEntity ent = new 在庫MergeリターンEntity();
            try
            {
                // マージを行う
                if (タイプ == MergeType.現在庫)
                {
                    MergeFiles(MergeType.現在庫);
                    ent.Merge成功か = true;
                    return ent;
                }
                else if (タイプ == MergeType.使用量)
                {
                    MergeFiles(MergeType.使用量);
                    ent.Merge成功か = true;
                    return ent;
                }
                else if (タイプ == MergeType.不動品)
                {
                    MergeFiles(MergeType.不動品);
                    ent.Merge成功か = true;
                    return ent;
                }
                else
                {
                    ent.Merge成功か = false;
                    ent.エラーメッセージ = "マージ種別が適切でない為、マージ出来ませんでした。";
                    return ent;
                }
            }
            catch (Exception exp)
            {
                ent.Merge成功か = false;
                ent.エラーメッセージ = string.Format("マージが出来ませんでした。\r\n原因:{0}{1}", exp.Message, exp.StackTrace);
                return ent;

            }


        }


        #region クラス内からの呼び出しメソッド
        private void MergeFiles(MergeType 種別)
        {

#if DEBUG
            string rootPath = PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPathDEBUG;
#elif NAKAYAMA
            string rootPath = PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPathNAKAYAMA;
#else
        string rootPath = PharmacyTool.Web.Properties.Settings.Default.在庫データUploadFileRootPath;
#endif

            string MergeFilesRootPath = "";
            switch (種別)
            {
                case (MergeType.現在庫):
                    MergeFilesRootPath = Path.Combine(rootPath, "現在庫");
                    break;

                case (MergeType.使用量):
                    MergeFilesRootPath = Path.Combine(rootPath, "使用量");
                    break;

                case (MergeType.不動品):
                    MergeFilesRootPath = Path.Combine(rootPath, "不動品");
                    break;

                default:
                    break;

            }

            string totalcsvFilePath = Path.Combine(MergeFilesRootPath, "total.csv");

            // total.csvを新規で作成する
            //File.Create(totalcsvFilePath);

            string[] FilesArray = Directory.GetFiles(MergeFilesRootPath, "*", SearchOption.AllDirectories);

            using (StreamWriter sw = new StreamWriter(totalcsvFilePath, false, Encoding.GetEncoding("Shift_JIS")))
            {
                foreach (var file in FilesArray)
                {
                    string[] sepa = file.Split('.');
                    string 拡張子 = sepa[sepa.Length - 1];

                    // 書き込み先なのでtotal.csvは飛ばす
                    if (拡張子.ToLower().Equals("csv") && !file.Contains("total.csv"))
                    {
                        // 最終更新日時を追加する
                        string 最終更新日時 = GetLastWriteTime(file);

                        string line = "";
                        using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("Shift_JIS")))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                line += string.Format(",{0}", 最終更新日時);
                                sw.WriteLine(line);
                            }

                        }
                    }
                    sw.Flush();
                }
            }

        }

        private string GetLastWriteTime(string FilePath)
        {
            string 最終更新日時 = "";

            try
            {

                if (System.IO.File.Exists(FilePath))
                {
                    最終更新日時 = System.IO.File.GetLastWriteTime(FilePath).ToString("yyyy/MM/dd HH:mm");
                }

            }
            catch
            {
                // 最終更新日時は空のままにする
            }

            return 最終更新日時;

        }

        #endregion

    }



    /// <summary>
    /// Entityクラス
    /// </summary>
    #region 在庫管理データ

    public class 在庫リターンデータセット<T>
    {
        private List<T> _在庫データlist;

        public List<T> 検索結果データlist
        {
            get { return _在庫データlist; }
            set { _在庫データlist = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }

        private string _検索キーワード;

        public string 検索キーワード
        {
            get { return _検索キーワード; }
            set { _検索キーワード = value; }
        }
    }


    [DataContract]
    public class 現在庫データ
    {
        private string _店名;

        [DataMember]
        public string 店名
        {
            get { return _店名; }
            set { _店名 = value; }
        }


        private string _医薬品名;

        [DataMember]
        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }


        [DataMember]
        private string _名称２;
        public string 名称２
        {
            get { return _名称２; }
            set { _名称２ = value; }
        }

        private string _薬価;

        [DataMember]
        public string 薬価
        {
            get { return _薬価; }
            set { _薬価 = value; }
        }

        private string _在庫数;

        [DataMember]
        public string 在庫数
        {
            get { return _在庫数; }
            set { _在庫数 = value; }
        }

        private DateTime _使用期限;

        [DataMember]
        public DateTime 使用期限
        {
            get { return _使用期限; }
            set { _使用期限 = value; }
        }

        private string _後発区分;

        [DataMember]
        public string 後発区分
        {
            get { return _後発区分; }
            set { _後発区分 = value; }
        }

        private string _個別医薬品コード;

        [DataMember]
        public string 個別医薬品コード
        {
            get { return _個別医薬品コード; }
            set { _個別医薬品コード = value; }
        }

        private string _レセプト電算処理システムコード;

        [DataMember]
        public string レセプト電算処理システムコード
        {
            get { return _レセプト電算処理システムコード; }
            set { _レセプト電算処理システムコード = value; }
        }

        private string _製造会社;

        [DataMember]
        public string 製造会社
        {
            get { return _製造会社; }
            set { _製造会社 = value; }
        }

        private string _最終更新日時;

        [DataMember]
        public string 最終更新日時
        {
            get { return _最終更新日時; }
            set { _最終更新日時 = value; }
        }


        private string _医薬品名と名称２連結;
        [DataMember]
        public string 医薬品名と名称２連結
        {
            get
            {
                if (string.IsNullOrEmpty(_名称２))
                {
                    return _医薬品名;

                }

                return _医薬品名 + "（" + _名称２ + "）";
            }
            set { _医薬品名と名称２連結 = value; }
        }


        public bool HasValues(string[] strarray)
        {
            foreach (var str in strarray)
            {
                if (this.店名.Contains(str) ||
                    this.レセプト電算処理システムコード.Contains(str) ||
                    this.医薬品名.Contains(str) ||
                    this.名称２.Contains(str) ||
                    this.個別医薬品コード.Contains(str) ||
                    this.後発区分.Contains(str) ||
                    this.在庫数.Contains(str) ||
                    this.使用期限.ToString().Contains(str) ||
                    this.製造会社.Contains(str) ||
                    this.薬価.ToString().Contains(str))
                {
                    return true;
                }
            }

            return false;
        }


        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }
    }

    public class 使用量データ
    {
        private string _店名;

        public string 店名
        {
            get { return _店名; }
            set { _店名 = value; }
        }

        private string _使用年月;

        public string 使用年月
        {
            get { return _使用年月; }
            set { _使用年月 = value; }
        }


        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }

        private string _薬価;

        public string 薬価
        {
            get { return _薬価; }
            set { _薬価 = value; }
        }

        private string _在庫数;

        public string 在庫数
        {
            get { return _在庫数; }
            set { _在庫数 = value; }
        }

        private DateTime _使用期限;

        public DateTime 使用期限
        {
            get { return _使用期限; }
            set { _使用期限 = value; }
        }

        private string _後発区分;

        public string 後発区分
        {
            get { return _後発区分; }
            set { _後発区分 = value; }
        }

        private string _個別医薬品コード;

        public string 個別医薬品コード
        {
            get { return _個別医薬品コード; }
            set { _個別医薬品コード = value; }
        }

        private string _レセプト電算処理システムコード;

        public string レセプト電算処理システムコード
        {
            get { return _レセプト電算処理システムコード; }
            set { _レセプト電算処理システムコード = value; }
        }

        private string _製造会社;

        public string 製造会社
        {
            get { return _製造会社; }
            set { _製造会社 = value; }
        }

        private string _最終更新日時;

        public string 最終更新日時
        {
            get { return _最終更新日時; }
            set { _最終更新日時 = value; }
        }


    }

    public class 不動品データ
    {
        private string _店名;

        public string 店名
        {
            get { return _店名; }
            set { _店名 = value; }
        }



        private string _レセプト電算処理システムコード;

        public string レセプト電算処理システムコード
        {
            get { return _レセプト電算処理システムコード; }
            set { _レセプト電算処理システムコード = value; }
        }


        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }


        private string _在庫数;

        public string 在庫数
        {
            get { return _在庫数; }
            set { _在庫数 = value; }
        }

        private DateTime _使用期限;

        public DateTime 使用期限
        {
            get { return _使用期限; }
            set { _使用期限 = value; }
        }


        private double _薬価;

        public double 薬価
        {
            get { return _薬価; }
            set { _薬価 = value; }
        }

        private string _個別医薬品コード;

        public string 個別医薬品コード
        {
            get { return _個別医薬品コード; }
            set { _個別医薬品コード = value; }
        }

        private string _名称２;

        public string 名称２
        {
            get { return _名称２; }
            set { _名称２ = value; }
        }

        private string _一包単位量;

        public string 一包単位量
        {
            get { return _一包単位量; }
            set { _一包単位量 = value; }
        }

        private string _最終更新日時;

        public string 最終更新日時
        {
            get { return _最終更新日時; }
            set { _最終更新日時 = value; }
        }

        private string _医薬品名と名称２連結;
        public string 医薬品名と名称２連結
        {
            get
            {
                if (string.IsNullOrEmpty(_名称２))
                {
                    return _医薬品名;

                }

                return _医薬品名 + "（" + _名称２ + "）";
            }
            set { _医薬品名と名称２連結 = value; }
        }

    }

    public class 薬局使用量データ
    {
        private string _店名;

        public string 店名
        {
            get { return _店名; }
            set { _店名 = value; }
        }

        private string _使用年月;

        public string 使用年月
        {
            get { return _使用年月; }
            set { _使用年月 = value; }
        }

        private string _商品コード;

        public string 商品コード
        {
            get { return _商品コード; }
            set { _商品コード = value; }
        }


        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }

        private string _名称２;

        public string 名称２
        {
            get { return _名称２; }
            set { _名称２ = value; }
        }


        private string _使用量;

        public string 使用量
        {
            get { return _使用量; }
            set { _使用量 = value; }
        }

        private string _薬価;

        public string 薬価
        {
            get { return _薬価; }
            set { _薬価 = value; }
        }

        private string _最終更新日時;

        public string 最終更新日時
        {
            get { return _最終更新日時; }
            set { _最終更新日時 = value; }
        }


        private string _医薬品名と名称２連結;
        public string 医薬品名と名称２連結
        {
            get
            {
                if (string.IsNullOrEmpty(_名称２))
                {
                    return _医薬品名;

                }

                return _医薬品名 + " / " + _名称２;
            }
            set { _医薬品名と名称２連結 = value; }
        }

    }

    public class 薬局データ
    {
        private string _店名;

        public string 店名
        {
            get { return _店名; }
            set { _店名 = value; }
        }

        private string _薬品コード;

        public string 薬品コード
        {
            get { return _薬品コード; }
            set { _薬品コード = value; }
        }


        private string _医薬品名;

        public string 医薬品名
        {
            get { return _医薬品名; }
            set { _医薬品名 = value; }
        }

        private decimal _薬価;

        public decimal 薬価
        {
            get { return _薬価; }
            set { _薬価 = value; }
        }

        private string _在庫数;

        public string 在庫数
        {
            get { return _在庫数; }
            set { _在庫数 = value; }
        }

        private DateTime _使用期限;

        public DateTime 使用期限
        {
            get { return _使用期限; }
            set { _使用期限 = value; }
        }

        private string _製造会社;

        public string 製造会社
        {
            get { return _製造会社; }
            set { _製造会社 = value; }
        }

        private string _後発区分;

        public string 後発区分
        {
            get { return _後発区分; }
            set { _後発区分 = value; }
        }

        private string _名称２;

        public string 名称２
        {
            get { return _名称２; }
            set { _名称２ = value; }
        }

        private string _最終更新日時;

        public string 最終更新日時
        {
            get { return _最終更新日時; }
            set { _最終更新日時 = value; }
        }

        private string _医薬品名と名称２連結;

        public string 医薬品名と名称２連結
        {
            get 
            {
                if (string.IsNullOrEmpty(_名称２))
                {
                    return _医薬品名;

                }
                else
                {
                }

                return _医薬品名 + "（" + _名称２ + "）";
                //return _医薬品名と名称２連結 + " / " + _名称２; 
            }
            set { _医薬品名と名称２連結 = value; }
        }

    }

    public class MEDISデータ
    {
        private string _個別医薬品コード;

        public string 個別医薬品コード
        {
            get { return _個別医薬品コード; }
            set { _個別医薬品コード = value; }
        }

        private string _レセプト電算処理システムコード;

        public string レセプト電算処理システムコード
        {
            get { return _レセプト電算処理システムコード; }
            set { _レセプト電算処理システムコード = value; }
        }

        private string _薬価基準収載医薬品コード;

        public string 薬価基準収載医薬品コード
        {
            get { return _薬価基準収載医薬品コード; }
            set { _薬価基準収載医薬品コード = value; }
        }

    }

    public enum 検索タイプ
    {
        現在庫検索 = 0,
        使用量検索 = 1,
        不動品検索 = 2,
        後発品検索 = 3
    }


    /// <summary>
    /// MEDISデータで個別医薬品コードとレセプト電算処理システムコードが１対１でない場合の重複削除処理用のクラス
    /// </summary>
    public class DistinctCheck
    {
        private string _個別医薬品コード;

        public string 個別医薬品コード
        {
            get { return _個別医薬品コード; }
            set { _個別医薬品コード = value; }
        }

        private string _レセプト電算処理システムコード;

        public string レセプト電算処理システムコード
        {
            get { return _レセプト電算処理システムコード; }
            set { _レセプト電算処理システムコード = value; }
        }

        private string _薬価基準収載医薬品コード;

        public string 薬価基準収載医薬品コード
        {
            get { return _薬価基準収載医薬品コード; }
            set { _薬価基準収載医薬品コード = value; }
        }
    }

    /// <summary>
    /// レセプト電算処理システムコードと薬価基準収載医薬品コードが等しい場合は同じものとする
    /// </summary>
    public class DistinctCheckComparer : System.Collections.Generic.IEqualityComparer<DistinctCheck>
    {
        public bool Equals(DistinctCheck x, DistinctCheck y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.レセプト電算処理システムコード == y.レセプト電算処理システムコード && x.薬価基準収載医薬品コード == y.薬価基準収載医薬品コード;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(DistinctCheck dc)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(dc, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductName = dc.レセプト電算処理システムコード == null ? 0 : dc.レセプト電算処理システムコード.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = dc.薬価基準収載医薬品コード == null ? 0 : dc.薬価基準収載医薬品コード.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }
    }

    #endregion
    #region 掲示板データ



    public class 掲示板リターンデータセット
    {
        private List<Dictionary<投稿Entity, List<返信Entity>>> _掲示板データ;

        public List<Dictionary<投稿Entity, List<返信Entity>>> 掲示板データ
        {
            get { return _掲示板データ; }
            set { _掲示板データ = value; }
        }

        private int _作成グループNo;

        public int 作成グループNo
        {
            get { return _作成グループNo; }
            set { _作成グループNo = value; }
        }

        private int _メイン記事数;

        public int メイン記事数
        {
            get { return _メイン記事数; }
            set { _メイン記事数 = value; }
        }

        private bool _記事取得成功か;

        public bool 記事取得成功か
        {
            get { return _記事取得成功か; }
            set { _記事取得成功か = value; }
        }

        private string _エラーメッセージ = "";

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }

        private bool _キーワード検索結果か;

        public bool キーワード検索結果か
        {
            get { return _キーワード検索結果か; }
            set { _キーワード検索結果か = value; }
        }

        private string _検索したキーワード = "";

        public string 検索したキーワード
        {
            get { return _検索したキーワード; }
            set { _検索したキーワード = value; }
        }

    }

    public class 掲示板記事修正確認Entity
    {
        private 投稿Entity _記事データ;

        public 投稿Entity 記事データ
        {
            get { return _記事データ; }
            set { _記事データ = value; }
        }

        private bool _暗証キーチェック成功;

        public bool 暗証キーチェック成功
        {
            get { return _暗証キーチェック成功; }
            set { _暗証キーチェック成功 = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }


    }

    /// <summary>
    /// 投稿したデータを保持するクラス
    /// </summary>
    public class 投稿Entity
    {
        private string _カテゴリ名;

        public string カテゴリ名
        {
            get { return _カテゴリ名; }
            set { _カテゴリ名 = value; }
        }

        private string _No;

        public string No
        {
            get { return _No; }
            set { _No = value; }
        }
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _投稿者名;

        public string 投稿者名
        {
            get { return _投稿者名; }
            set { _投稿者名 = value; }
        }
        private string _投稿日;

        public string 投稿日
        {
            get { return _投稿日; }
            set { _投稿日 = value; }
        }


        private string _記事;
        public string 記事
        {
            get { return _記事; }
            set { _記事 = value; }
        }

        private List<string> _添付ファイルlist;
        public List<string> 添付ファイルlist
        {
            get { return _添付ファイルlist; }
            set { _添付ファイルlist = value; }
        }

        private string _HomepageUrl;

        public string HomepageUrl
        {
            get { return _HomepageUrl; }
            set { _HomepageUrl = value; }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _文字色;

        public string 文字色
        {
            get { return _文字色; }
            set { _文字色 = value; }
        }

        private string _暗証キー;

        public string 暗証キー
        {
            get { return _暗証キー; }
            set { _暗証キー = value; }
        }

    }

    /// <summary>
    /// 返信したデータを保持するクラス
    /// </summary>
    public class 返信Entity
    {
        private string _カテゴリ名;

        public string カテゴリ名
        {
            get { return _カテゴリ名; }
            set { _カテゴリ名 = value; }
        }

        private string _No;

        public string No
        {
            get { return _No; }
            set { _No = value; }
        }
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _投稿者名;

        public string 投稿者名
        {
            get { return _投稿者名; }
            set { _投稿者名 = value; }
        }
        private string _投稿日;

        public string 投稿日
        {
            get { return _投稿日; }
            set { _投稿日 = value; }
        }


        private string _記事;

        public string 記事
        {
            get { return _記事; }
            set { _記事 = value; }
        }

        private List<string> _添付ファイルlist;
        public List<string> 添付ファイルlist
        {
            get { return _添付ファイルlist; }
            set { _添付ファイルlist = value; }
        }


        private string _HomepageUrl;

        public string HomepageUrl
        {
            get { return _HomepageUrl; }
            set { _HomepageUrl = value; }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _文字色;

        public string 文字色
        {
            get { return _文字色; }
            set { _文字色 = value; }
        }

    }

    #endregion
    #region ThreadTiles
    public class ThreadTitlesEntity
    {
        private string _スレッド名;

        public string スレッド名
        {
            get { return _スレッド名; }
            set { _スレッド名 = value; }
        }

        private string _画像パス;

        public string 画像パス
        {
            get { return _画像パス; }
            set { _画像パス = value; }
        }

        private DateTime _スレッド作成日;

        public DateTime スレッド作成日
        {
            get { return _スレッド作成日; }
            set { _スレッド作成日 = value; }
        }

    }

    #endregion
    #region Terop

    public class テロップ記事リターンEntity
    {
        private string _テロップ記事;
        public string テロップ記事
        {
            get { return _テロップ記事; }
            set { _テロップ記事 = value; }
        }

        private bool _読み込み成功か;

        public bool 読み込み成功か
        {
            get { return _読み込み成功か; }
            set { _読み込み成功か = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }
    }

    #endregion
    #region 在庫Merge
    public class 在庫MergeリターンEntity
    {
        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }

        private bool _Merge成功か;

        public bool Merge成功か
        {
            get { return _Merge成功か; }
            set { _Merge成功か = value; }
        }
    }

    public enum MergeType
    {
        現在庫 = 0,
        使用量 = 1,
        不動品 = 2
    }
    #endregion
    /// <summary>
    /// 最終更新日時を取得する
    /// </summary>
    public class 最終更新日時リターンデータセット
    {
        private bool _データ取得成功か;

        public bool データ取得成功か
        {
            get { return _データ取得成功か; }
            set { _データ取得成功か = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }


        private string _MEDIS最終更新日時;

        public string MEDIS最終更新日時
        {
            get { return _MEDIS最終更新日時; }
            set { _MEDIS最終更新日時 = value; }
        }
    }









}


