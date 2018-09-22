using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using msg = System.Windows;
using クスリのナカヤマ薬局ツール.共通.Exception;

namespace クスリのナカヤマ薬局ツール.UserControls.移動明細書
{
    /// <summary>
    /// 移動明細書管理トップ画面.xaml の相互作用ロジック
    /// </summary>
    public partial class 移動明細書管理トップ画面 : System.Windows.Controls.UserControl
    {
        private List<int> チェック済リスト = new List<int>();

        public 移動明細書管理トップ画面()
        {
            InitializeComponent();
            SetInit();
        }

        private void SetInit()
        {
            Setデフォルト値();
        }

        private void DoRoutine_Pawaful3(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            if (btn == null)
            {
                return;
            }

            if (this.tb移動明細書FolderPath.Text == "")
            {
                msg.MessageBox.Show("チェックするフォルダを指定して下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!System.IO.Directory.Exists(this.tb移動明細書FolderPath.Text))
            {
                msg.MessageBox.Show("チェックするフォルダが存在しません。再度チェックフォルダを指定して下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.移動明細書合計金額出力ファイルパス = System.IO.Path.Combine(tb移動明細書FolderPath.Text, "移動明細書合計金額出力.csv");

            List<string> Validate用移動明細書チェック店舗リスト = Enumerable.Concat(new List<string>(), 移動明細書チェック店舗リスト).ToList();

            try
            {
                // チェック系
                // ファイル数
                // 店舗名
                // 日付チェック

                // Doマージ
                List<移動明細書項目データ> mergeData = new List<移動明細書項目データ>();

                var arFilePaths = System.IO.Directory.GetFiles(tb移動明細書FolderPath.Text);
                string mergeFilePath = System.IO.Path.Combine(tb移動明細書FolderPath.Text, "移動明細書total.csv");
                if (File.Exists(mergeFilePath))
                {
                    File.Delete(mergeFilePath);
                }

                // チェックする店舗名リストのファイル名があるか先に行う。
                foreach (var filepath in arFilePaths)
                {
                    // マージ用ファイルは飛ばす
                    if (filepath.Equals(mergeFilePath))
                    {
                        continue;
                    }

                    if (filepath.Equals(移動明細書合計金額出力ファイルパス))
                    {
                        continue;
                    }

                    var sepa = filepath.Split('\\');
                    var lastdelimita = sepa[sepa.Length - 1];
                    var delisepa = lastdelimita.Split('.');

                    if (!delisepa[delisepa.Length - 1].Equals("csv"))
                    {
                        continue;
                    }

                    string 店名 = delisepa[0];

                    // [移動明細書チェック店舗リスト]に設定してない店舗名のファイルがある場合は続行するか確認する。
                    if (移動明細書チェック店舗リスト.Contains(店名) == false)
                    {
                        if (msg.MessageBox.Show(string.Format("Settings.iniの[移動明細書チェック店舗リスト]に設定されている\r\n値以外の店舗ファイルが存在します。\r\n設定値以外の店舗ファイル名：{0}\r\n\r\nこのファイルを無視してチェックを続行しますか？", lastdelimita), "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK) != MessageBoxResult.OK)
                        {
                            return;
                        }
                    }

                    // あったものは確認用リストから削除していく
                    Validate用移動明細書チェック店舗リスト.Remove(店名);
                }

                // [移動明細書チェック店舗リスト]に設定してあるが、その店舗名のファイルがない場合はエラーとする。
                if (Validate用移動明細書チェック店舗リスト.Count != 0)
                {
                    string 無かった店舗ファイル = "";
                    Validate用移動明細書チェック店舗リスト.ForEach(x => 無かった店舗ファイル += ((無かった店舗ファイル == "" ? "" : "\r\n") + "・" + x + ".csv"));
                    throw new ExtendException("チェック店舗リストエラー", string.Format("Settings.iniの[移動明細書チェック店舗リスト]に設定されている次の店舗名ファイルが存在しません。\r\n{0}\r\n\r\nSettings.iniの値およびチェックするファイル名とファイル数が正しいことを確認してください。", 無かった店舗ファイル), "", "none");

                }


                using (StreamWriter sw = new StreamWriter(mergeFilePath, true, Encoding.GetEncoding(932)))
                {

                    foreach (var filepath in arFilePaths)
                    {
                        // マージ用ファイルは飛ばす
                        if (filepath.Equals(mergeFilePath))
                        {
                            continue;
                        }


                        if (filepath.Equals(移動明細書合計金額出力ファイルパス))
                        {
                            continue;
                        }

                        var sepa = filepath.Split('\\');
                        var lastdelimita = sepa[sepa.Length - 1];
                        var delisepa = lastdelimita.Split('.');

                        if (!delisepa[delisepa.Length - 1].Equals("csv"))
                        {
                            continue;
                        }

                        string 店名 = delisepa[0];

                        // [移動明細書チェック店舗リスト]に設定してない店舗名のファイルがある場合はすでにチェック済で無視して続行を選択しているので、続行する。
                        if (移動明細書チェック店舗リスト.Contains(店名) == false)
                        {
                            continue;
                            //throw new ExtendException("チェック店舗リストエラー", "Settings.iniの移動明細書チェック店舗リストに設定されている店舗のファイルが存在しません。\r\nSettings.iniの値とチェックするファイル名が正しいか確認してください。", filepath, "none");
                        }

                        if (!System.IO.File.Exists(filepath))
                        {
                            throw new ExtendException("ファイル存在しない", "指定されてフォルダ内にファイルが存在しません。", filepath, "none");
                            // continue;
                        }

                        string line = "";
                        int counter = 1;

                        using (StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding(932)))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                // 頭２行はヘッダーなので飛ばす
                                if (counter < 3)
                                {
                                    counter++;
                                    continue;
                                }

                                移動明細書項目データ data = new 移動明細書項目データ();

                                var sepa2 = line.Replace("\"", "").Split(',');


                                // "区分","伝票日付","伝票No","行番号","相手先コード","相手先","薬品コード","薬品名","形態区分","数量","単価","金額","使用期限","ロットNo"
                                data.明細書店舗名 = 店名;

                                try
                                {
                                    data.区分 = sepa2[0];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("区分データ読み込み", string.Format("以下のファイルの区分データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n区分データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.伝票日付 = sepa2[1];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("伝票日付データ読み込み", string.Format("以下のファイルの伝票日付データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n伝票日付データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.伝票No = sepa2[2];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("伝票Noデータ読み込み", string.Format("以下のファイルの伝票Noデータを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n伝票Noデータを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.相手先 = sepa2[5];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("相手先データ読み込み", string.Format("以下のファイルの相手先データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n相手先データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.薬品コード = sepa2[6].Substring(0, 9);
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("薬品コードデータ読み込み", string.Format("以下のファイルの薬品コードデータを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n薬品コードデータを確認してください。\r\n\r\nエラーとなった薬品コードデータ：\r\n{1}\r\n\r\nPGErrorMessage:\r\n{2}", filepath, sepa2[6], ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.薬品名 = sepa2[7];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("薬品名データ読み込み", string.Format("以下のファイルの薬品名データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n薬品名データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.数量 = sepa2[9];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("数量データ読み込み", string.Format("以下のファイルの数量データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n数量データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.単価 = sepa2[10];
                                    decimal result;

                                    if (decimal.TryParse(data.単価, out result) == false)
                                    {
                                        throw new ExtendException("単価データの変換", string.Format("以下のファイルの単価データの変換を行う際に\r\nエラーが発生しました。\r\n{0}\r\n\r\n単価データを確認してください。\r\n\r\nエラーとなった単価データ：\r\n{1}", filepath, data.単価), filepath, "none");
                                    }


                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("単価データ読み込み", string.Format("以下のファイルの単価データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n単価データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.金額 = sepa2[11];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("金額データ読み込み", string.Format("以下のファイルの金額データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n金額データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.相手伝票No = sepa2[14];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("相手伝票Noデータ読み込み", string.Format("以下のファイルの相手伝票Noデータを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n相手伝票Noデータを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }




                                sw.WriteLine(line);
                                mergeData.Add(data);
                                counter++;
                            }
                        }

                    }

                }

                // 合計金額出力処理
                Calc受払合計(mergeData);


                // 突合開始
                // ①区分チェック（払・受）
                var 店間振替払 = (from x in mergeData
                             where x.区分.Equals("店間振替払") &&
                                   移動明細書チェック店舗リスト.Contains(x.明細書店舗名) &&
                                   移動明細書チェック店舗リスト.Contains(x.相手先)
                             select new 移動明細書項目データ
                             {
                                 明細書店舗名 = x.明細書店舗名,
                                 区分 = x.区分,
                                 伝票日付 = x.伝票日付,
                                 伝票No = x.伝票No,
                                 相手先 = x.相手先,
                                 薬品コード = x.薬品コード,
                                 薬品名 = x.薬品名,
                                 単価 = x.単価,
                                 数量 = x.数量,
                                 相手伝票No = x.相手伝票No,
                                 金額 = x.金額
                             }).ToList();


                var 店間振替受 = (from x in mergeData
                             where x.区分.Equals("店間振替受") &&
                                   移動明細書チェック店舗リスト.Contains(x.明細書店舗名) &&
                                   移動明細書チェック店舗リスト.Contains(x.相手先)
                             select new 移動明細書項目データ
                             {
                                 明細書店舗名 = x.明細書店舗名,
                                 区分 = x.区分,
                                 伝票日付 = x.伝票日付,
                                 伝票No = x.伝票No,
                                 相手先 = x.相手先,
                                 薬品コード = x.薬品コード,
                                 薬品名 = x.薬品名,
                                 単価 = x.単価,
                                 数量 = x.数量,
                                 相手伝票No = x.相手伝票No,
                                 金額 = x.金額

                             }).ToList();


                var 払受あるもの = 
                           (from x in 店間振替払
                           join y in 店間振替受
                           on new {a = x.明細書店舗名,b=x.相手先,c=x.伝票No,d = x.薬品コード} equals new {a= y.相手先,b=y.明細書店舗名,c=y.相手伝票No,d = y.薬品コード}
                           select new
                           {
                                 明細書店舗名払 = x.明細書店舗名,
                                 区分払 = x.区分,
                                 伝票日付払 = x.伝票日付,
                                 伝票No払 = x.伝票No,
                                 相手先払 = x.相手先,
                                 薬品コード払 = x.薬品コード,
                                 薬品名払 = x.薬品名,
                                 単価払 = x.単価,
                                 数量払 = x.数量,
                                 相手伝票No払 = x.相手伝票No,
                                 金額払 = x.金額,

                                 明細書店舗名受 = y.明細書店舗名,
                                 区分受 = y.区分,
                                 伝票日付受 = y.伝票日付,
                                 伝票No受 = y.伝票No,
                                 相手先受 = y.相手先,
                                 薬品コード受 = y.薬品コード,
                                 薬品名受 = y.薬品名,
                                 単価受 = y.単価,
                                 数量受 = y.数量,
                                 相手伝票No受 = y.相手伝票No,
                                 金額受 = x.金額,

                           }).ToList();

                // 払のみあるもの
                // A.区分=払で, 払伝票の店舗名・相手先・伝票Noと受店舗名・相手先・相手伝票No を突合

                var 払のみあるもの =

                            (from a in
                                 店間振替払.Except(
                                                       from x in 店間振替払
                                                       from y in 払受あるもの
                                                       where
                                                           (
                                                               x.明細書店舗名 == y.明細書店舗名払 &&
                                                               x.相手先 == y.相手先払 &&
                                                               x.伝票No == y.伝票No払 &&
                                                               x.薬品コード == y.薬品コード払
                                                           )
                                                       select x)
                             select new エラーリスト表示２用Entity
                             {
                                 払データ = new 移動明細書項目データ
                                 {
                                     明細書店舗名 = a.明細書店舗名,
                                     区分 = a.区分,
                                     伝票日付 = a.伝票日付,
                                     伝票No = a.伝票No,
                                     相手先 = a.相手先,
                                     薬品コード = a.薬品コード,
                                     薬品名 = a.薬品名,
                                     単価 = a.単価,
                                     相手伝票No = a.相手伝票No,
                                     金額 = a.金額,
                                     数量 = a.数量
                                 },
                                 エラー事由 = "受伝票が存在しません。",
                                 受データ = new 移動明細書項目データ()
                             }

                                       ).OrderBy(x => x.払データ.明細書店舗名).ThenBy(x => x.払データ.相手先).ThenBy(x => x.払データ.伝票日付).ToList();


                // 受のみあるもの
                // B.区分=受で, 受伝票の店舗名・相手先・相手伝票Noと払店舗名・相手先・伝票No を突合

  


                var 受のみあるもの =

                                    (from a in
                                         店間振替受.Except(
                                                               from x in 店間振替受
                                                               from y in 払受あるもの
                                                               where
                                                                   (
                                                                       x.明細書店舗名 == y.明細書店舗名受 &&
                                                                       x.相手先 == y.相手先受 &&
                                                                       x.相手伝票No == y.相手伝票No受 &&
                                                                       x.薬品コード == y.薬品コード受
                                                                   )
                                                               select x)
                                     select new エラーリスト表示２用Entity
                                     {
                                         払データ = new 移動明細書項目データ(),
                                         エラー事由 = "払伝票が存在しません。",
                                         受データ = new 移動明細書項目データ
                                         {
                                             明細書店舗名 = a.明細書店舗名,
                                             区分 = a.区分,
                                             伝票日付 = a.伝票日付,
                                             伝票No = a.伝票No,
                                             相手先 = a.相手先,
                                             薬品コード = a.薬品コード,
                                             薬品名 = a.薬品名,
                                             単価 = a.単価,
                                             相手伝票No = a.相手伝票No,
                                             金額 = a.金額,
                                             数量 = a.数量
                                         }
                                     }

                                       ).OrderBy(x => x.受データ.明細書店舗名).ThenBy(x => x.受データ.相手先).ThenBy(x => x.受データ.伝票日付).ToList();




                //// ②薬品名チェック
                //// ①で正しいもののうち、薬品コードが違うもの
                //var 薬品名が違う伝票 =
                //                            (
                //                                from x in 払受あるもの
                //                                where

                //                                    // 薬品コードが違うもの
                //                                    x.薬品名払 != x.薬品名受

                //                                select new エラーリスト表示２用Entity
                //                                {
                //                                    払データ = new 移動明細書項目データ
                //                                     {
                //                                         明細書店舗名 = x.明細書店舗名払,
                //                                         区分 = x.区分払,
                //                                         伝票日付 = x.伝票日付払,
                //                                         伝票No = x.伝票No払,
                //                                         相手先 = x.相手先払,
                //                                         薬品コード = x.薬品コード払,
                //                                         薬品名 = x.薬品名払,
                //                                         単価 = x.単価払,
                //                                         相手伝票No = x.相手伝票No払
                //                                     },
                //                                    エラー事由 = "薬品コードが合致しておりません。",
                //                                    受データ = new 移動明細書項目データ
                //                                    {
                //                                        明細書店舗名 = x.明細書店舗名受,
                //                                        区分 = x.区分受,
                //                                        伝票日付 = x.伝票日付受,
                //                                        伝票No = x.伝票No受,
                //                                        相手先 = x.相手先受,
                //                                        薬品コード = x.薬品コード受,
                //                                        薬品名 = x.薬品名受,
                //                                        単価 = x.単価受,
                //                                        相手伝票No = x.相手伝票No受
                //                                    }
                //                                }
                //                            ).ToList();



                // ②数量チェック
                // ①で正しいもののうち、数量が違うもの。
                var 数量が違う伝票 =
                            (
                                 from x in 払受あるもの
                                 where
                                     // 数量が違うもの
                                     x.数量払 != x.数量受

                                                select new エラーリスト表示２用Entity
                                                {
                                                    払データ = new 移動明細書項目データ
                                                     {
                                                         明細書店舗名 = x.明細書店舗名払,
                                                         区分 = x.区分払,
                                                         伝票日付 = x.伝票日付払,
                                                         伝票No = x.伝票No払,
                                                         相手先 = x.相手先払,
                                                         薬品コード = x.薬品コード払,
                                                         薬品名 = x.薬品名払,
                                                         単価 = x.単価払,
                                                         相手伝票No = x.相手伝票No払,
                                                         金額 = x.金額払,
                                                         数量 = x.数量払

                                                     },
                                     エラー事由 = "数量が合致しておりません。",
                                                    受データ = new 移動明細書項目データ
                                                    {
                                                        明細書店舗名 = x.明細書店舗名受,
                                                        区分 = x.区分受,
                                                        伝票日付 = x.伝票日付受,
                                                        伝票No = x.伝票No受,
                                                        相手先 = x.相手先受,
                                                        薬品コード = x.薬品コード受,
                                                        薬品名 = x.薬品名受,
                                                        単価 = x.単価受,
                                                        相手伝票No = x.相手伝票No受,
                                                        金額 = x.金額受,
                                                        数量 = x.数量受

                                                    }
                                                }
                            ).ToList();





                //var mergeList = 払のみあるもの.Concat(受のみあるもの).Concat(薬品名が違う伝票).Concat(数量が違う伝票);
                var mergeList = 払のみあるもの.Concat(受のみあるもの).Concat(数量が違う伝票).ToList();

                // エラーリスト出力
                エラーリスト表示２ window1 = new エラーリスト表示２();
                string 結果表示 =
                    "【払のみ： " + 払のみあるもの.Count + "件】　" +
                    "【受のみ： " + 受のみあるもの.Count + "件】　" +
                    "【数量違い： " + 数量が違う伝票.Count + "件】　";

                window1.tbl結果.Text = 結果表示;
                window1.lv移動伝票明細書エラー.ItemsSource = mergeList;
                window1.ShowDialog();

            }

            catch (Exception ex)
            {
                ExtendException etdEx = ex as ExtendException;
                if (etdEx == null)
                {
                    msg.MessageBox.Show(string.Format("エラーが発生しました。\r\n\r\nErrorMessage:{0}\r\n\r\nStackTrace:{1}", ex.Message, ex.StackTrace), "エラー確認", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                else
                {
                    msg.MessageBox.Show(string.Format("チェック中にエラーが発生しました。\r\n\r\nErrorArea:{0}\r\nErrorMessage:\r\n{1}\r\n\r\nStacktrace:{2}", etdEx.ErrorArea, etdEx.ErrorMessageExtend, etdEx.StackTrace), "エラー確認", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }

        }

        private void DoRoutine_Pawaful1or2(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            if (btn == null)
            {
                return;
            }

            if (this.tb移動明細書FolderPath.Text == "")
            {
                msg.MessageBox.Show("チェックするフォルダを指定して下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!System.IO.Directory.Exists(this.tb移動明細書FolderPath.Text))
            {
                msg.MessageBox.Show("チェックするフォルダが存在しません。再度チェックフォルダを指定して下さい。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.移動明細書合計金額出力ファイルパス = System.IO.Path.Combine(tb移動明細書FolderPath.Text, "移動明細書合計金額出力.csv");

            List<string> Validate用移動明細書チェック店舗リスト = Enumerable.Concat(new List<string>(), 移動明細書チェック店舗リスト).ToList();

            try
            {
                // チェック系
                // ファイル数
                // 店舗名
                // 日付チェック

                // Doマージ
                List<移動明細書項目データ> mergeData = new List<移動明細書項目データ>();

                var arFilePaths = System.IO.Directory.GetFiles(tb移動明細書FolderPath.Text);
                string mergeFilePath = System.IO.Path.Combine(tb移動明細書FolderPath.Text, "移動明細書total.csv");
                if (File.Exists(mergeFilePath))
                {
                    File.Delete(mergeFilePath);
                }

                // チェックする店舗名リストのファイル名があるか先に行う。
                foreach (var filepath in arFilePaths)
                {
                    // マージ用ファイルは飛ばす
                    if (filepath.Equals(mergeFilePath))
                    {
                        continue;
                    }

                    if (filepath.Equals(移動明細書合計金額出力ファイルパス))
                    {
                        continue;
                    }

                    var sepa = filepath.Split('\\');
                    var lastdelimita = sepa[sepa.Length - 1];
                    var delisepa = lastdelimita.Split('.');

                    if (!delisepa[delisepa.Length - 1].Equals("csv"))
                    {
                        continue;
                    }

                    string 店名 = delisepa[0];

                    // [移動明細書チェック店舗リスト]に設定してない店舗名のファイルがある場合は続行するか確認する。
                    if (移動明細書チェック店舗リスト.Contains(店名) == false)
                    {
                        if (msg.MessageBox.Show(string.Format("Settings.iniの[移動明細書チェック店舗リスト]に設定されている\r\n値以外の店舗ファイルが存在します。\r\n設定値以外の店舗ファイル名：{0}\r\n\r\nこのファイルを無視してチェックを続行しますか？", lastdelimita), "確認", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK) != MessageBoxResult.OK)
                        {
                            return;
                        }
                    }

                    // あったものは確認用リストから削除していく
                    Validate用移動明細書チェック店舗リスト.Remove(店名);
                }

                // [移動明細書チェック店舗リスト]に設定してあるが、その店舗名のファイルがない場合はエラーとする。
                if (Validate用移動明細書チェック店舗リスト.Count != 0)
                {
                    string 無かった店舗ファイル = "";
                    Validate用移動明細書チェック店舗リスト.ForEach(x => 無かった店舗ファイル += ((無かった店舗ファイル == "" ? "" : "\r\n") + "・" + x + ".csv"));
                    throw new ExtendException("チェック店舗リストエラー", string.Format("Settings.iniの[移動明細書チェック店舗リスト]に設定されている次の店舗名ファイルが存在しません。\r\n{0}\r\n\r\nSettings.iniの値およびチェックするファイル名とファイル数が正しいことを確認してください。", 無かった店舗ファイル), "", "none");

                }


                using (StreamWriter sw = new StreamWriter(mergeFilePath, true, Encoding.GetEncoding(932)))
                {

                    foreach (var filepath in arFilePaths)
                    {
                        // マージ用ファイルは飛ばす
                        if (filepath.Equals(mergeFilePath))
                        {
                            continue;
                        }


                        if (filepath.Equals(移動明細書合計金額出力ファイルパス))
                        {
                            continue;
                        }

                        var sepa = filepath.Split('\\');
                        var lastdelimita = sepa[sepa.Length - 1];
                        var delisepa = lastdelimita.Split('.');

                        if (!delisepa[delisepa.Length - 1].Equals("csv"))
                        {
                            continue;
                        }

                        string 店名 = delisepa[0];

                        // [移動明細書チェック店舗リスト]に設定してない店舗名のファイルがある場合はすでにチェック済で無視して続行を選択しているので、続行する。
                        if (移動明細書チェック店舗リスト.Contains(店名) == false)
                        {
                            continue;
                            //throw new ExtendException("チェック店舗リストエラー", "Settings.iniの移動明細書チェック店舗リストに設定されている店舗のファイルが存在しません。\r\nSettings.iniの値とチェックするファイル名が正しいか確認してください。", filepath, "none");
                        }

                        if (!System.IO.File.Exists(filepath))
                        {
                            throw new ExtendException("ファイル存在しない", "指定されてフォルダ内にファイルが存在しません。", filepath, "none");
                            // continue;
                        }

                        string line = "";
                        int counter = 1;

                        using (StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding(932)))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                // 頭２行はヘッダーなので飛ばす
                                if (counter < 3)
                                {
                                    counter++;
                                    continue;
                                }

                                移動明細書項目データ data = new 移動明細書項目データ();

                                var sepa2 = line.Replace("\"", "").Split(',');


                                // "区分","伝票日付","伝票No","行番号","相手先コード","相手先","薬品コード","薬品名","形態区分","数量","単価","金額","使用期限","ロットNo"
                                data.明細書店舗名 = 店名;

                                try
                                {
                                    data.区分 = sepa2[0];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("区分データ読み込み", string.Format("以下のファイルの区分データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n区分データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.伝票日付 = sepa2[1];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("伝票日付データ読み込み", string.Format("以下のファイルの伝票日付データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n伝票日付データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.伝票No = sepa2[2];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("伝票Noデータ読み込み", string.Format("以下のファイルの伝票Noデータを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n伝票Noデータを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.相手先 = sepa2[5];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("相手先データ読み込み", string.Format("以下のファイルの相手先データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n相手先データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.薬品コード = sepa2[6].Substring(0, 9);
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("薬品コードデータ読み込み", string.Format("以下のファイルの薬品コードデータを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n薬品コードデータを確認してください。\r\n\r\nエラーとなった薬品コードデータ：\r\n{1}\r\n\r\nPGErrorMessage:\r\n{2}", filepath, sepa2[6], ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.薬品名 = sepa2[7];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("薬品名データ読み込み", string.Format("以下のファイルの薬品名データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n薬品名データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.数量 = sepa2[9];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("数量データ読み込み", string.Format("以下のファイルの数量データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n数量データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.単価 = sepa2[10];
                                    decimal result;

                                    if (decimal.TryParse(data.単価, out result) == false)
                                    {
                                        throw new ExtendException("単価データの変換", string.Format("以下のファイルの単価データの変換を行う際に\r\nエラーが発生しました。\r\n{0}\r\n\r\n単価データを確認してください。\r\n\r\nエラーとなった単価データ：\r\n{1}", filepath, data.単価), filepath, "none");
                                    }


                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("単価データ読み込み", string.Format("以下のファイルの単価データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n単価データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                try
                                {
                                    data.金額 = sepa2[11];
                                }
                                catch (Exception ex)
                                {
                                    throw new ExtendException("金額データ読み込み", string.Format("以下のファイルの金額データを読み込む時に\r\nエラーが発生しました。\r\n{0}\r\n\r\n金額データを確認してください。\r\n\r\nPGErrorMessage:\r\n{1}", filepath, ex.Message), filepath, ex.StackTrace);
                                }

                                sw.WriteLine(line);
                                mergeData.Add(data);
                                counter++;
                            }
                        }

                    }

                }


                // 合計金額出力処理
                Calc受払合計(mergeData);


                // データの並び替え
                var sortmergedata = (from x in mergeData
                                     orderby x.明細書店舗名,
                                     x.区分,
                                     x.相手先
                                     select new 移動明細書項目データ
                                     {
                                         明細書店舗名 = x.明細書店舗名,
                                         区分 = x.区分,
                                         伝票No = x.伝票No,
                                         伝票日付 = x.伝票日付,
                                         相手先 = x.相手先,
                                         薬品コード = x.薬品コード,
                                         数量 = x.数量,
                                         薬品名 = x.薬品名,
                                         単価 = decimal.Parse(x.単価).ToString(),
                                         金額 = x.金額
                                     }).ToList();


                var sortdata店間振替払 = (from x in mergeData
                                     where x.区分.Equals("店間振替払") &&
                                           移動明細書チェック店舗リスト.Contains(x.明細書店舗名) &&
                                           移動明細書チェック店舗リスト.Contains(x.相手先)
                                     group x by new
                                     {
                                         x.明細書店舗名,
                                         x.区分,
                                         x.相手先,
                                         x.薬品コード,
                                         x.単価
                                     } into grouping
                                     select new 移動明細書項目データ
                                     {
                                         明細書店舗名 = grouping.Key.明細書店舗名,
                                         区分 = grouping.Key.区分,
                                         相手先 = grouping.Key.相手先,
                                         薬品コード = grouping.Key.薬品コード,
                                         合計数量 = grouping.Sum(s => decimal.Parse(s.数量)).ToString(),
                                         薬品名 = grouping.FirstOrDefault().薬品名,
                                         単価 = decimal.Parse(grouping.Key.単価).ToString(),
                                     }).ToList();

                //foreach (var data in sortdata店間振替払)
                //{
                //    if (data.薬品名.Equals("エンシュア・リキッド"))
                //    {
                //    }
                //}


                var sortdata店間振替受 = (from x in mergeData
                                     where x.区分.Equals("店間振替受") &&
                                           移動明細書チェック店舗リスト.Contains(x.明細書店舗名) &&
                                           移動明細書チェック店舗リスト.Contains(x.相手先)
                                     group x by new
                                     {
                                         x.明細書店舗名,
                                         x.区分,
                                         x.相手先,
                                         x.薬品コード,
                                         x.単価
                                     } into grouping
                                     select new 移動明細書項目データ
                                     {
                                         明細書店舗名 = grouping.Key.明細書店舗名,
                                         区分 = grouping.Key.区分,
                                         相手先 = grouping.Key.相手先,
                                         薬品コード = grouping.Key.薬品コード,
                                         合計数量 = grouping.Sum(s => decimal.Parse(s.数量)).ToString(),
                                         薬品名 = grouping.FirstOrDefault().薬品名,
                                         単価 = decimal.Parse(grouping.Key.単価).ToString(),
                                     }).ToList();




                // エラー表示用コレクション
                List<移動明細書項目データ> 受けあり払いなしList = new List<移動明細書項目データ>();
                List<移動明細書項目データ> 払いあり受けなしList = new List<移動明細書項目データ>();
                List<移動明細書項目データ> 受け払いあり入力違いList = new List<移動明細書項目データ>();

                // 払い→受けチェック

                ObservableCollection<移動明細書項目データ> 払いあり受けなしデータ = new ObservableCollection<移動明細書項目データ>();
                //List<int> マッチ済行番号 = new List<int>();


                foreach (var data in sortdata店間振替払)
                {
                    bool マッチしたデータあり = false;

                    if (data.区分.Equals("店間振替払") == false)
                    {
                        continue;
                    }


                    if (data.区分.Equals("店間振替払") == true)
                    {
                        // ToDo 店名にマッチさせてチェック
                        if (data.相手先.Contains("店") == false)
                        {
                            continue;
                        }
                    }

                    foreach (var searchdata in sortdata店間振替受)
                    {
                        if (searchdata.区分.Equals("店間振替受") &&
                            data.相手先.Equals(searchdata.明細書店舗名) &&
                            searchdata.相手先.Equals(data.明細書店舗名) &&
                            data.単価.Equals(searchdata.単価) &&
                            data.薬品コード.Substring(0, 9).Equals(searchdata.薬品コード.Substring(0, 9)))
                        {
                            // 入力間違い
                            if (data.合計数量.Equals(searchdata.合計数量) == false)
                            {
                                マッチしたデータあり = true;
                                受け払いあり入力違いList.Add(searchdata);
                                break;
                            }
                            else
                            {
                                // マッチした
                                マッチしたデータあり = true;


                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (マッチしたデータあり == false)
                    {
                        払いあり受けなしList.Add(data);
                    }
                }

                //// 受けあり払いなしチェック
                int count = 0;
                foreach (var data in sortdata店間振替受)
                {

                    bool マッチしたデータあり = false;

                    if (data.区分.Equals("店間振替受") == false)
                    {
                        count++;
                        continue;
                    }


                    if (data.区分.Equals("店間振替受") == true)
                    {
                        // ToDo 店名にマッチさせてチェック
                        if (data.相手先.Contains("店") == false)
                        {
                            count++;
                            continue;
                        }
                    }

                    foreach (var searchdata in sortdata店間振替払)
                    {
                        if (searchdata.区分.Equals("店間振替払") &&
                            data.相手先.Equals(searchdata.明細書店舗名) &&
                            searchdata.相手先.Equals(data.明細書店舗名) &&
                            data.単価.Equals(searchdata.単価) &&
                            data.薬品コード.Substring(0, 9).Equals(searchdata.薬品コード.Substring(0, 9)))
                        {
                            // 入力間違い
                            if (data.合計数量.Equals(searchdata.合計数量) == false)
                            {
                                // 受け側も加える
                                受け払いあり入力違いList.Add(searchdata);
                                マッチしたデータあり = true;
                                break;
                            }
                            else
                            {
                                // マッチした
                                マッチしたデータあり = true;

                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (マッチしたデータあり == false)
                    {
                        受けあり払いなしList.Add(data);
                    }

                    count++;

                }

                // 薬品コードで合算したものをひも解く

                // 払○受×
                List<移動明細書項目データ> devide払あり受なしList = new List<移動明細書項目データ>();

                // 払×受○
                List<移動明細書項目データ> devide受あり払なしList = new List<移動明細書項目データ>();

                // 払受○入力違い
                List<移動明細書項目データ> devide払受あり入力違いList = new List<移動明細書項目データ>();

                foreach (var data in sortmergedata)
                {
                    foreach (var data1 in 払いあり受けなしList)
                    {
                        if (data.明細書店舗名.Equals(data1.明細書店舗名) &&
                            data.区分.Equals(data1.区分) &&
                            data.相手先.Equals(data1.相手先) &&
                            data.薬品コード.Equals(data1.薬品コード) &&
                            data.単価.Equals(data1.単価))
                        {
                            devide払あり受なしList.Add(data);
                        }
                    }

                    foreach (var data1 in 受けあり払いなしList)
                    {
                        if (data.明細書店舗名.Equals(data1.明細書店舗名) &&
                            data.区分.Equals(data1.区分) &&
                            data.相手先.Equals(data1.相手先) &&
                            data.薬品コード.Equals(data1.薬品コード) &&
                            data.単価.Equals(data1.単価))
                        {
                            devide受あり払なしList.Add(data);
                        }
                    }

                    foreach (var data1 in 受け払いあり入力違いList)
                    {
                        if (data.明細書店舗名.Equals(data1.明細書店舗名) &&
                            data.区分.Equals(data1.区分) &&
                            data.相手先.Equals(data1.相手先) &&
                            data.薬品コード.Equals(data1.薬品コード) &&
                            data.単価.Equals(data1.単価))
                        {
                            devide払受あり入力違いList.Add(data);
                        }
                    }
                }



                var sortdevide払受あり入力違いList = (from x in devide払受あり入力違いList
                                              orderby
                                              x.薬品コード,
                                              x.区分 descending,
                                              x.明細書店舗名
                                              select new 移動明細書項目データ
                                              {
                                                  明細書店舗名 = x.明細書店舗名,
                                                  区分 = x.区分,
                                                  伝票No = x.伝票No,
                                                  伝票日付 = x.伝票日付,
                                                  相手先 = x.相手先,
                                                  薬品コード = x.薬品コード,
                                                  数量 = x.数量,
                                                  薬品名 = x.薬品名,
                                                  単価 = x.単価,
                                                  金額 = x.金額
                                              }).ToList();




                // エラーリスト出力
                エラーリスト表示 window1 = new エラーリスト表示();
                string 結果表示 =
                    "【払○受×： " + devide払あり受なしList.Count + "件】　" +
                    "【払×受○： " + devide受あり払なしList.Count + "件】　" +
                    "【数量違い： " + sortdevide払受あり入力違いList.Count + "件】　";

                window1.tbl結果.Text = 結果表示;
                window1.エラーリスト払いあり受けなしListView.ItemsSource = devide払あり受なしList;
                window1.エラーリスト受けあり払いなしListView.ItemsSource = devide受あり払なしList;
                window1.エラーリスト払い受けあり入力間違いListView.ItemsSource = sortdevide払受あり入力違いList;
                window1.ShowDialog();

            }

            catch (Exception ex)
            {
                ExtendException etdEx = ex as ExtendException;
                if (etdEx == null)
                {
                    msg.MessageBox.Show(string.Format("エラーが発生しました。\r\n\r\nErrorMessage:{0}\r\n\r\nStackTrace:{1}", ex.Message, ex.StackTrace), "エラー確認", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                else
                {
                    msg.MessageBox.Show(string.Format("チェック中にエラーが発生しました。\r\n\r\nErrorArea:{0}\r\nErrorMessage:\r\n{1}\r\n\r\nStacktrace:{2}", etdEx.ErrorArea, etdEx.ErrorMessageExtend, etdEx.StackTrace), "エラー確認", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }

        }



        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //DoRoutine_Pawaful1or2(sender, e);
            DoRoutine_Pawaful3(sender, e);
        }

        private void btn移動明細書Folder参照_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tb移動明細書FolderPath.Text = fbd.SelectedPath;
            }
            else
            {
                return;
            }
        }

        private void Calc受払合計(List<移動明細書項目データ> mergeData)
        {

            // ヘッダー作成
            ////string ヘッダー = string.Format("{0}", "作成年月日");
            string ヘッダー = "";
            foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
            {
                ヘッダー += string.Format(",{0}", チェック店舗リスト);
            }


            // 受けデータの作成
            List<string> 受行リスト = Enumerable.Concat(new List<string>(), 移動明細書チェック店舗リスト).ToList();
            受行リスト.Concat(移動明細書チェック店舗リスト);
            受行リスト.Add("本部センター");
            受行リスト.Add("在庫合わせ");
            受行リスト.Add("その他");
            //受行リスト.Add("他薬局");

            // 他薬局以外の受データに使用
            var groupingmergeData受 =
                (
                    from x in mergeData
                    where
                        x.区分 == "店間振替受" &&
                        移動明細書チェック店舗リスト.Contains(x.明細書店舗名) //&&
                    //(移動明細書チェック店舗リスト.Contains(x.相手先) || x.相手先 == "本部センター" || x.相手先.Contains("在庫合わせ"))
                    group x by new
                    {
                        x.明細書店舗名,
                        x.相手先,
                    } into grouping
                    select new
                    {
                        明細書店舗名 = grouping.Key.明細書店舗名,
                        相手先 = grouping.Key.相手先,
                        合計金額 = grouping.Sum(p => decimal.Parse(p.金額))
                    }
                ).ToList();

            //// 他薬局のデータに使用
            //var groupingmergeData受他薬局用 =
            //  (
            //      from x in mergeData
            //      where
            //          x.区分 == "他店仕入" &&
            //          移動明細書チェック店舗リスト.Contains(x.明細書店舗名) &&
            //          x.相手先 != "ファーマシー・フォーファーマシーズ"
            //      group x by new
            //      {
            //          x.明細書店舗名//,
            //          //x.相手先,
            //      } into grouping
            //      select new
            //      {
            //          明細書店舗名 = grouping.Key.明細書店舗名,
            //          //相手先 = grouping.Key.相手先,
            //          合計金額 = grouping.Sum(p => decimal.Parse(p.金額))
            //      }
            //  ).ToList();

            string 受data = "";

            List<string> stream用受dataList = new List<string>();
            foreach (var 受行header in 受行リスト)
            {

                if (受行header == "本部センター")
                {
                    受data = "本部";
                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData受
                                            where
                                                x.明細書店舗名 == チェック店舗リスト && x.相手先 == "本部センター"
                                            select new
                                            {
                                                明細書店舗名 = x.明細書店舗名,
                                                相手先 = x.相手先,
                                                合計金額 = x.合計金額
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            受data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else
                        {
                            受data += string.Format(",{0}", selectData[0].合計金額);
                        }


                    }
                }
                else if (受行header == "その他")
                {
                    受data = "その他";
                    //foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    //{
                    //    var selectData = (
                    //                        from x in groupingmergeData受他薬局用
                    //                        where
                    //                            x.明細書店舗名 == チェック店舗リスト //&&                         // 明細書作成した店舗名は等しく
                    //                            //(移動明細書チェック店舗リスト.Contains(x.相手先) == false) &&   // 相手先から移動明細書チェックする店舗名をはずし
                    //                            //(x.相手先.Contains("本部センター") == false) &&                 // 相手先から本部センターをはずし
                    //                            //x.相手先 != "在庫合わせ" &&                                     // 相手先から在庫合わせもはずし
                    //                            //x.相手先 != ""                                                  // 相手先が空でない
                    //                        select new
                    //                        {
                    //                            明細書店舗名 = x.明細書店舗名,
                    //                            //相手先 = x.相手先,
                    //                            合計金額 = x.合計金額
                    //                        }
                    //                    ).ToList();

                    //    if (selectData.Count == 0)
                    //    {
                    //        受data += string.Format(",{0}", "0");
                    //        //throw new Exception("データが不正です。");
                    //    }
                    //    else
                    //    {
                    //        受data += string.Format(",{0}", selectData[0].合計金額);
                    //    }


                    //}

                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData受
                                            where
                                            x.明細書店舗名 == チェック店舗リスト &&                         // 明細書作成した店舗名は等しく
                                            (移動明細書チェック店舗リスト.Contains(x.相手先) == false) &&   // 相手先から移動明細書チェックする店舗名をはずし
                                            (!(x.相手先 == "本部センター")) &&                              // 相手先から本部センターをはずし
                                            (x.相手先.Contains("在庫合わせ") == false) &&                   // 相手先から在庫合わせもはずし
                                            x.相手先 != ""
                                            group x by new
                                            {
                                                x.明細書店舗名
                                            } into grouping
                                            // 相手先が空でない
                                            select new
                                            {
                                                明細書店舗名 = grouping.Key.明細書店舗名,
                                                合計金額 = grouping.Sum(s => s.合計金額).ToString()
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            受data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else if (selectData.Count == 1)
                        {
                            受data += string.Format(",{0}", selectData[0].合計金額);
                        }
                        else
                        {
                            throw new Exception(string.Format("その他の合計金額に不正なデータがあります。{0}.csv", チェック店舗リスト));

                        }


                    }


                }
                else if (受行header == "在庫合わせ")
                {
                    受data = "在庫合わせ";
                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData受
                                            where
                                                x.明細書店舗名 == チェック店舗リスト && x.相手先.Contains("在庫合わせ")
                                            select new
                                            {
                                                明細書店舗名 = x.明細書店舗名,
                                                相手先 = x.相手先,
                                                合計金額 = x.合計金額
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            受data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else
                        {
                            受data += string.Format(",{0}", selectData[0].合計金額);
                        }
                    }
                }
                else
                {
                    受data = 受行header;
                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData受
                                            where
                                                x.明細書店舗名 == チェック店舗リスト && x.相手先 == 受行header
                                            select new
                                            {
                                                明細書店舗名 = x.明細書店舗名,
                                                相手先 = x.相手先,
                                                合計金額 = x.合計金額
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            受data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else
                        {
                            受data += string.Format(",{0}", selectData[0].合計金額);
                        }


                    }
                }

                stream用受dataList.Add(受data);
            }





            // 払いデータの作成

            List<string> 払行リスト = Enumerable.Concat(new List<string>(), 移動明細書チェック店舗リスト).ToList();
            払行リスト.Concat(移動明細書チェック店舗リスト);
            払行リスト.Add("本部センター");
            払行リスト.Add("在庫合わせ");
            //払行リスト.Add("他薬局");
            払行リスト.Add("その他");

            // 他薬局以外の払データに使用
            //var groupingmergeData払 =
            //              (
            //                  from x in mergeData
            //                  where
            //                      x.区分 == "店間振替払"
            //                  group x by new
            //                  {
            //                      x.明細書店舗名,
            //                      x.相手先,
            //                  } into grouping
            //                  select new
            //                  {
            //                      明細書店舗名 = grouping.Key.明細書店舗名,
            //                      相手先 = grouping.Key.相手先,
            //                      合計金額 = grouping.Sum(p => decimal.Parse(p.金額))
            //                  }
            //              ).ToList();

            var groupingmergeData払 =
                (
                    from x in mergeData
                    where
                        x.区分 == "店間振替払" &&
                        移動明細書チェック店舗リスト.Contains(x.明細書店舗名) //&&
                    //(移動明細書チェック店舗リスト.Contains(x.相手先) || x.相手先 == "本部センター" || x.相手先.Contains("在庫合わせ"))
                    group x by new
                    {
                        x.明細書店舗名,
                        x.相手先,
                    } into grouping
                    select new
                    {
                        明細書店舗名 = grouping.Key.明細書店舗名,
                        相手先 = grouping.Key.相手先,
                        合計金額 = grouping.Sum(p => decimal.Parse(p.金額))
                    }
                ).ToList();

            //// 他薬局のデータに使用
            //var groupingmergeData払他薬局用 =
            //  (
            //      from x in mergeData
            //      where
            //          x.区分 == "他店売上" &&
            //          移動明細書チェック店舗リスト.Contains(x.明細書店舗名) &&
            //          x.相手先 != "ファーマシー・フォーファーマシーズ"
            //      group x by new
            //      {
            //          x.明細書店舗名//,
            //          //x.相手先,
            //      } into grouping
            //      select new
            //      {
            //          明細書店舗名 = grouping.Key.明細書店舗名,
            //          //相手先 = grouping.Key.相手先,
            //          合計金額 = grouping.Sum(p => decimal.Parse(p.金額))
            //      }
            //  ).ToList();


            string 払data = "";

            List<string> stream用払dataList = new List<string>();
            foreach (var 払行header in 払行リスト)
            {

                if (払行header == "本部センター")
                {
                    払data = "本部";
                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData払
                                            where
                                                x.明細書店舗名 == チェック店舗リスト && x.相手先 == "本部センター"
                                            select new
                                            {
                                                明細書店舗名 = x.明細書店舗名,
                                                相手先 = x.相手先,
                                                合計金額 = x.合計金額
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            払data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else
                        {
                            払data += string.Format(",{0}", selectData[0].合計金額);
                        }


                    }
                }
                else if (払行header == "その他")
                {
                    払data = "その他";
                    //foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    //{
                    //    var selectData = (
                    //                        from x in groupingmergeData払他薬局用
                    //                        where
                    //                            x.明細書店舗名 == チェック店舗リスト //&&                         // 明細書作成した店舗名は等しく
                    //                        //(移動明細書チェック店舗リスト.Contains(x.相手先) == false) &&   // 相手先から移動明細書チェックする店舗名をはずし
                    //                        //(x.相手先.Contains("本部センター") == false) &&                 // 相手先から本部センターをはずし
                    //                        //x.相手先 != "在庫合わせ" &&                                     // 相手先から在庫合わせもはずし
                    //                        //x.相手先 != ""                                                  // 相手先が空でない
                    //                        select new
                    //                        {
                    //                            明細書店舗名 = x.明細書店舗名,
                    //                            //相手先 = x.相手先,
                    //                            合計金額 = x.合計金額
                    //                        }
                    //                    ).ToList();

                    //    if (selectData.Count == 0)
                    //    {
                    //        払data += string.Format(",{0}", "0");
                    //        //throw new Exception("データが不正です。");
                    //    }
                    //    else
                    //    {
                    //        払data += string.Format(",{0}", selectData[0].合計金額);
                    //    }
                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData払
                                            where
                                                x.明細書店舗名 == チェック店舗リスト &&                     // 明細書作成した店舗名は等しく
                                            (移動明細書チェック店舗リスト.Contains(x.相手先) == false) &&   // 相手先から移動明細書チェックする店舗名をはずし
                                            (!(x.相手先 == "本部センター")) &&                              // 相手先から本部センターをはずし
                                            (x.相手先.Contains("在庫合わせ") == false) &&                   // 相手先から在庫合わせもはずし
                                            x.相手先 != ""                                                  // 相手先が空でない
                                            group x by new
                                            {
                                                x.明細書店舗名
                                            } into grouping
                                            // 相手先が空でない
                                            select new
                                            {
                                                明細書店舗名 = grouping.Key.明細書店舗名,
                                                合計金額 = grouping.Sum(s => s.合計金額).ToString()
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            払data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else if (selectData.Count == 1)
                        {
                            払data += string.Format(",{0}", selectData[0].合計金額);
                        }
                        else
                        {
                            throw new Exception(string.Format("その他の合計金額に不正なデータがあります。{0.csv", チェック店舗リスト));

                        }
                    }
                }
                else if (払行header == "在庫合わせ")
                {
                    払data = "在庫合わせ";
                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData払
                                            where
                                                x.明細書店舗名 == チェック店舗リスト && x.相手先.Contains("在庫合わせ")
                                            select new
                                            {
                                                明細書店舗名 = x.明細書店舗名,
                                                相手先 = x.相手先,
                                                合計金額 = x.合計金額
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            払data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else
                        {
                            払data += string.Format(",{0}", selectData[0].合計金額);
                        }
                    }
                }
                else
                {
                    払data = 払行header;
                    foreach (var チェック店舗リスト in 移動明細書チェック店舗リスト)
                    {
                        var selectData = (
                                            from x in groupingmergeData払
                                            where
                                                x.明細書店舗名 == チェック店舗リスト && x.相手先 == 払行header
                                            select new
                                            {
                                                明細書店舗名 = x.明細書店舗名,
                                                相手先 = x.相手先,
                                                合計金額 = x.合計金額
                                            }
                                        ).ToList();

                        if (selectData.Count == 0)
                        {
                            払data += string.Format(",{0}", "0");
                            //throw new Exception("データが不正です。");
                        }
                        else
                        {
                            払data += string.Format(",{0}", selectData[0].合計金額);
                        }


                    }
                }

                stream用払dataList.Add(払data);
            }


            // データの書き込み
            using (StreamWriter sw = new StreamWriter(移動明細書合計金額出力ファイルパス, false, Encoding.GetEncoding(932)))
            {
                // 受けデータ
                sw.WriteLine("【受データ開始】");
                sw.WriteLine(ヘッダー);
                foreach (var data in stream用受dataList)
                {
                    sw.WriteLine(data);
                }

                // セパレーター
                //sw.WriteLine("【受データ終了】");

                sw.WriteLine("");
                sw.WriteLine("");

                // 払いデータ
                sw.WriteLine("【払データ開始】");
                sw.WriteLine(ヘッダー);

                foreach (var data in stream用払dataList)
                {
                    sw.WriteLine(data);
                }

                //sw.WriteLine("【払データ終了】");



            }


        }


        string 移動明細書合計金額出力ファイルパス = "";
        List<string> 移動明細書チェック店舗リスト = new List<string>();
        private void Setデフォルト値()
        {
            string アプリケーション実行パス = System.AppDomain.CurrentDomain.BaseDirectory;
            string 設定ファイルパス = System.IO.Path.Combine(アプリケーション実行パス, "Settings.ini");

            if (!System.IO.File.Exists(設定ファイルパス))
            {
                return;
            }

            using (StreamReader sr = new StreamReader(設定ファイルパス, Encoding.GetEncoding(932)))
            {
                string line = "";
                string 移動明細書フォルダパス = "";

                int 行数counter = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (行数counter == 6)
                    {
                        移動明細書フォルダパス = line.Replace("[移動明細書フォルダパス]=", "");
                    }

                    if (行数counter == 7)
                    {
                        string paramaters = line.Replace("[移動明細書チェック店舗リスト]=", "");
                        var sepa = paramaters.Split(',');
                        foreach (var param in sepa)
                        {
                            移動明細書チェック店舗リスト.Add(param);
                        }

                    }

                    行数counter++;
                }

                this.tb移動明細書FolderPath.Text = 移動明細書フォルダパス;
            }



        }




    }

    /// <summary>
    /// チェックリストの元になるベースクラス Parent
    /// </summary>
    public class 移動明細書差異チェックEntity
    {
        private string _明細書店舗名;

        public string 明細書店舗名
        {
            get { return _明細書店舗名; }
            set { _明細書店舗名 = value; }
        }

        private List<移動明細書相手先データEntity> _相手先List;

        public List<移動明細書相手先データEntity> 相手先List
        {
            get { return _相手先List; }
            set { _相手先List = value; }
        }

        private 受払種別Enum _受払種別;

        public 受払種別Enum 受払種別
        {
            get { return _受払種別; }
            set { _受払種別 = value; }
        }
    }

    /// <summary>
    /// 移動明細書差異チェックEntityの相手先データ Child
    /// </summary>
    public class 移動明細書相手先データEntity
    {
        private string _相手先;

        public string 相手先
        {
            get { return _相手先; }
            set { _相手先 = value; }
        }

        private decimal _合計金額;

        public decimal 合計金額
        {
            get { return _合計金額; }
            set { _合計金額 = value; }
        }

        private bool _差異ありか;

        public bool 差異ありか
        {
            get { return _差異ありか; }
            set { _差異ありか = value; }
        }

    }

    public enum 受払種別Enum
    {
        受け = 0,
        払い = 1
    }

    public class エラーリスト表示２用Entity
    {
        private 移動明細書項目データ _払データ;

        public 移動明細書項目データ 払データ
        {
            get { return _払データ; }
            set { _払データ = value; }
        }

        private 移動明細書項目データ _受データ;

        public 移動明細書項目データ 受データ
        {
            get { return _受データ; }
            set { _受データ = value; }
        }

        private string _エラー事由;

        public string エラー事由
        {
            get { return _エラー事由; }
            set { _エラー事由 = value; }
        }


    }


    public class エラーリスト表示２受払一致チェックComparer : IEqualityComparer<エラーリスト表示２用Entity>
    {

        #region IEqualityComparer<Task> Members

        public bool Equals(エラーリスト表示２用Entity x, エラーリスト表示２用Entity y)
        {
            return x.払データ.明細書店舗名 == y.受データ.相手先 && x.払データ.相手先 == y.受データ.明細書店舗名 && x.払データ.伝票No == y.受データ.相手伝票No;
        }

        public int GetHashCode(エラーリスト表示２用Entity obj)
        {
            return obj.払データ.明細書店舗名.GetHashCode() + obj.受データ.相手先.GetHashCode() + obj.払データ.相手先.GetHashCode() + obj.受データ.明細書店舗名.GetHashCode() + obj.払データ.伝票No.GetHashCode() + obj.受データ.相手伝票No.GetHashCode();
        }

        #endregion
    }

    public class 移動明細書項目データ
    {
        public 移動明細書項目データ()
        {
        }

        public 移動明細書項目データ(string 明細書店舗名, string 区分, string 伝票日付, string 伝票No, string 相手先, string 薬品コード, string 薬品名, string 単価, string 金額, string 相手伝票No)
        {
            明細書店舗名 = this.明細書店舗名;
            区分 = this.区分;
            伝票日付 = this.伝票日付;
            伝票No = this.伝票No;
            相手先 = this.相手先;
            薬品コード = this.薬品コード;
            薬品名 = this.薬品名;
            単価 = this.単価;
            金額 = this.金額;
            相手伝票No = this.相手伝票No;



        }

        private int _行番号;

        public int 行番号
        {
            get { return _行番号; }
            set { _行番号 = value; }
        }

        private string _明細書店舗名;

        public string 明細書店舗名
        {
            get { return _明細書店舗名; }
            set { _明細書店舗名 = value; }
        }

        private string _区分;

        public string 区分
        {
            get { return _区分; }
            set { _区分 = value; }
        }

        private string _伝票日付;

        public string 伝票日付
        {
            get { return _伝票日付; }
            set { _伝票日付 = value; }
        }

        private string _伝票No;

        public string 伝票No
        {
            get { return _伝票No; }
            set { _伝票No = value; }
        }

        private string _相手先;

        public string 相手先
        {
            get { return _相手先; }
            set { _相手先 = value; }
        }


        private string _薬品コード;

        public string 薬品コード
        {
            get { return _薬品コード; }
            set { _薬品コード = value; }
        }

        private string _薬品コード９ケタ変換;

        public string 薬品コード９ケタ変換
        {
            get { return _薬品コード９ケタ変換; }
            set { _薬品コード９ケタ変換 = value; }
        }

        private string _薬品名;

        public string 薬品名
        {
            get { return _薬品名; }
            set { _薬品名 = value; }
        }

        private string _数量;

        public string 数量
        {
            get { return _数量; }
            set { _数量 = value; }
        }

        private string _単価;

        public string 単価
        {
            get { return _単価; }
            set { _単価 = value; }
        }

        private string _合計数量;

        public string 合計数量
        {
            get { return _合計数量; }
            set { _合計数量 = value; }
        }

        private string _金額;

        public string 金額
        {
            get { return _金額; }
            set { _金額 = value; }
        }

        private string _相手伝票No;

        public string 相手伝票No
        {
            get { return _相手伝票No; }
            set { _相手伝票No = value; }
        }

        private bool _チェック済;

        public bool チェック済
        {
            get { return _チェック済; }
            set { _チェック済 = value; }
        }
    }
}
