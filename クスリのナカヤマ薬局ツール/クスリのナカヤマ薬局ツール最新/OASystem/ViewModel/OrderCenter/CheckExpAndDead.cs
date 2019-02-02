using System;
using System.Collections.Generic;
using System.Text;
using OASystem.Model.Entity;
using System.IO;
using System.Linq;
using OASystem.ViewModel.File;
using OASystem.View.Windows;
using OASystem.ViewModel.Common.DataConvert;

namespace OASystem.ViewModel.OrderCenter
{
    public static class CheckExpAndDead
    {
        /// <summary>
        /// OrderCenterのデータをセットする
        /// </summary>
        /// <param name="win"></param>
        /// <param name="sendList"></param>
        public static void SetOrderData(OASystem.View.Windows.OrderCenter win, List<SEND01DATEntity> sendList, List<BalancingAccountsCheckResultEntity> checkresult)
        {
            //帳合チェックはしていない時は、checkresult=nullで飛んでくることがあるので注意。

            DateTime start = DateTime.Now;

            int 期限切迫期間 = 6; // 6ヶ月に仮設定
            var FirstDayThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            string 自店舗名 = OASystem.Model.DI.自店舗名;

            var 不動品list = Load不動品Total();
            //var MEDIS_HOT13= LoadMEDIS_HOT13();
            var MEDIS_HOT13And個別管理医薬品list = LoadMEDIS_HOT13And個別管理医薬品マスタ().Where(x => 12 <= x.JANコード.Length);
            var 現在庫list = Load現在庫Total();

            var 保護リストAll = LoadCenter.Load保護リストAll();
            var 優先移動リストAll = LoadCenter.Load優先移動リストAll();


            // JANコード13桁一致の旧ロジック
            // SEND01にはJANコードまたはGS-1コードが入ってくることがあるので新ロジックへ
            //var SENDとMEDIS結合2 =
            //                    (from x in sendList
            //                     join y in MEDIS_HOT13And個別管理医薬品list
            //                     on
            //                         x.JANコード equals y.JANコード
            //                     where
            //                        string.IsNullOrEmpty(y.レセプト電算コード) == false &&  //レセプト電算コードが不明なもの同士が結合しないようにする対応
            //                        string.IsNullOrEmpty(y.個別医薬品コード) == false
            //                     select new { send = x, medis = y }).ToList();


            // 12桁で一致を探す(JANコードの13桁目チェックデジットのため）
            var SENDとMEDIS結合 = sendList.Where(x => 12 <= x.JANコード.Length)
                                          .Join(MEDIS_HOT13And個別管理医薬品list,
                                                outer => outer.JANコード.Substring(0, 12),
                                                inner => inner.JANコード.Substring(0, 12),
                                                (outer, inner) => new { send = outer, medis = inner });


            DateTime end = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("A:" + (end - start).TotalMilliseconds);


            start = DateTime.Now;

            // 店舗ごとにGroupingした不動品list
            // 自店舗の不動品は外す
            var 自店舗を外した店舗ごと不動品list =
                                    (
                                     from x in 不動品list
                                     where
                                        x.店名 != 自店舗名
                                     group x by new
                                     {
                                         x.店名,
                                         x.医薬品名,
                                         x.レセプト電算コード,
                                         x.在庫数,
                                         x.使用期限,
                                         x.名称２,
                                         x.一包単位量,
                                         x.最終更新日時
                                     } into grouping
                                     select new 不動品Entity
                                     {
                                         店名 = grouping.Key.店名,
                                         医薬品名 = grouping.Key.医薬品名,
                                         レセプト電算コード = grouping.Key.レセプト電算コード,
                                         在庫数 = grouping.Key.在庫数,
                                         薬価 = grouping.First().薬価,
                                         使用期限 = grouping.Key.使用期限,
                                         名称２ = grouping.Key.名称２,
                                         一包単位量 = grouping.Key.一包単位量,
                                         最終更新日時 = grouping.Key.最終更新日時
                                     })
                                     .ToList();


            var 自店舗を外した店舗ごとで保護リスト除外した不動品list =

                          from x in 自店舗を外した店舗ごと不動品list
                          join y in 保護リストAll  //保護リストは外す
                              on new { a = x.店名, b = x.レセプト電算コード } equals new { a = y.店舗名, b = y.レセプト電算コード } into gj
                          where
                              gj.FirstOrDefault() == null // 該当なしのものだけ
                              &&
                              FirstDayThisMonth <= x.使用期限 // 期限が切れていないもの
                          select x;




            // 店舗ごとにGroupingした現在庫list 
            var 店舗ごと現在庫list =
                                    (
                                     from x in 現在庫list
                                     group x by new
                                     {
                                         x.店名,
                                         x.医薬品名,
                                         x.レセプト電算コード,
                                         x.在庫数,
                                         x.薬価,
                                         x.使用期限,
                                         x.名称２,
                                         x.Is後発品,
                                         x.最終更新日時
                                     } into grouping
                                     select new 現在庫Entity
                                     {
                                         店名 = grouping.Key.店名,
                                         医薬品名 = grouping.Key.医薬品名,
                                         レセプト電算コード = grouping.Key.レセプト電算コード,
                                         在庫数 = grouping.Key.在庫数,
                                         薬価 = grouping.First().薬価,
                                         使用期限 = grouping.Key.使用期限,
                                         名称２ = grouping.Key.名称２,
                                         最終更新日時 = grouping.Key.最終更新日時
                                     }).ToList();

            // 店舗ごとにGroupingした現在庫list
            // 自店舗を外したもの
            var 自店舗を外した店舗ごと現在庫list =
                                    (
                                     from x in 現在庫list
                                     where
                                        x.店名 != 自店舗名
                                     group x by new
                                     {
                                         x.店名,
                                         x.医薬品名,
                                         x.レセプト電算コード,
                                         x.在庫数,
                                         x.薬価,
                                         x.使用期限,
                                         x.名称２,
                                         x.Is後発品,
                                         x.最終更新日時
                                     } into grouping
                                     select new 現在庫Entity
                                     {
                                         店名 = grouping.Key.店名,
                                         医薬品名 = grouping.Key.医薬品名,
                                         レセプト電算コード = grouping.Key.レセプト電算コード,
                                         在庫数 = grouping.Key.在庫数,
                                         薬価 = grouping.First().薬価,
                                         使用期限 = grouping.Key.使用期限,
                                         名称２ = grouping.Key.名称２,
                                         Is後発品 = grouping.Key.Is後発品,
                                         最終更新日時 = grouping.Key.最終更新日時
                                     }).ToList();

            var 自店舗を外した店舗ごと保護リストを除外した現在庫list =

                         from x in 自店舗を外した店舗ごと現在庫list
                         join y in 保護リストAll  //保護リストは外す
                             on new { a = x.店名, b = x.レセプト電算コード } equals new { a = y.店舗名, b = y.レセプト電算コード } into gj
                         where
                           gj.FirstOrDefault() == null // 保護リスト該当なしのものだけ
                           &&
                           FirstDayThisMonth <= x.使用期限 // 期限が切れていないもの
                         //&& 
                         //x.使用期限 <= DateTime.Now.AddMonths(期限切迫期間)
                         select x;

            int i = 自店舗を外した店舗ごと保護リストを除外した現在庫list.Count();



            // 発注予定リストで全て表示用
            // 薬価を入れる場合は修正が必要
            var order全て = (
                    from x in SENDとMEDIS結合
                    join z in Model.DI.帳合先マスタ
                    on
                        x.send.卸VANコード equals z.卸ＶＡＮコード
                    join u in 現在庫list
                    on
                         x.medis.レセプト電算コード equals u.レセプト電算コード into gj
                    //from sub現在庫 in gj.DefaultIfEmpty()
                    select new OrderScheduledListEntity
                    {
                        JANコード = x.medis.JANコード, // 以降はMEDIS-DCのJANをセットする。DATのGS1と混在するので以降のチェックは12桁で
                        医薬品名 = x.medis.医薬品名, // Order(注文)はMEDISから取ってきた名称でＯＫ。名称２とか関係ない。
                        数量 = x.send.数量,
                        薬価 = gj.FirstOrDefault() == null ? 0d : gj.FirstOrDefault().薬価,
                        包装形態 = x.medis.包装形態,
                        包装単位数 = x.medis.包装単位数,
                        包装単位 = x.medis.包装単位,
                        包装総量 = double.Parse(x.medis.包装総量),
                        剤形区分 = x.medis.剤形区分,
                        レセプト電算コード = x.medis.レセプト電算コード,
                        Isデッド品該当 = false,
                        Is期限切迫該当 = false,
                        レセ発注伝票No = x.send.レセ発注伝票No,
                        注文番号 = x.send.注文番号,
                        帳合先名称 = z.帳合先名称,
                        卸VANコード = x.send.卸VANコード
                    }
                    ).ToList();

            end = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("B:" + (end - start).TotalMilliseconds);


            start = DateTime.Now;


            var insertdead = (
                             from x in order全て
                             join y in 自店舗を外した店舗ごとで保護リスト除外した不動品list
                                 on new { a = x.レセプト電算コード } equals new { a = y.レセプト電算コード }
                             select x
                            ).ToList();
            insertdead.ForEach(x => x.Isデッド品該当 = true);


            var insertexp = (
                 from x in order全て
                 join y in 自店舗を外した店舗ごと保護リストを除外した現在庫list
                     on new { a = x.レセプト電算コード } equals new { a = y.レセプト電算コード }
                 where
                     y.使用期限 <= DateTime.Now.AddMonths(期限切迫期間)  // 期限切迫確認
                 select x).ToList();

            insertexp.ForEach(x => x.Is期限切迫該当 = true);

            if (checkresult != null)
            {
                var insertcheckresult =

                     (from x in order全て
                      join y in checkresult
                         on x.JANコード.Substring(0, 12) equals y.JANコード.Substring(0, 12)
                         //on x.JANコード equals y.JANコード
                      select x).ToList();

                insertcheckresult.ForEach(x => x.Is帳合一致 = false);
            }


            var insertPrioritymove =

                         (from x in order全て
                          join y in 自店舗を外した店舗ごと保護リストを除外した現在庫list
                              on new { a = x.レセプト電算コード } equals new { a = y.レセプト電算コード }
                          join z in 優先移動リストAll
                              on new { a = y.レセプト電算コード, b = y.店名 } equals new { a = z.レセプト電算コード, b = z.店舗名 }
                          where
                               x.Is他店該当 == false
                          select x).ToList();

            insertPrioritymove.ForEach(x => x.Is優先移動該当 = true);


            end = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("C':" + (end - start).TotalMilliseconds);



            start = DateTime.Now;


            order全て = order全て.OrderBy(x => x.レセ発注伝票No).ThenBy(x => x.医薬品名).ToList();

            end = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("C:" + (end - start).TotalMilliseconds);



            start = DateTime.Now;


            // 帳合不一致のみを抽出
            var OrderOnlyCheckBalancingFalse = (from x in order全て
                                                where
                                                  x.Is帳合一致 == false
                                                select x).ToList();

            //1.03aで対応
            //帳合不一致のものはあらかじめDAT削除にチェックを入れておく
            foreach (var oocbf in OrderOnlyCheckBalancingFalse)
            {
                oocbf.SEND01DATから削除するか = true;
            }


            // 発注予定リストでデッド
            // TODO:再度Joinしなくても、Isデッド品該当、Is期限切迫該当フラグをWhere指定すればOK。
            var orderDead = (
                from x in order全て
                join z in 自店舗を外した店舗ごと不動品list
                on
                    x.レセプト電算コード equals z.レセプト電算コード
                join u in Model.DI.帳合先マスタ
                on
                    x.卸VANコード equals u.卸ＶＡＮコード
                where
                    FirstDayThisMonth <= z.使用期限 // 期限が切れていないもの
                select x).ToList();


            // 発注予定リストで期限切迫
            var orderExp = (
                            from x in order全て
                            join z in 自店舗を外した店舗ごと現在庫list
                            on
                                x.レセプト電算コード equals z.レセプト電算コード
                            join u in Model.DI.帳合先マスタ
                            on
                                x.卸VANコード equals u.卸ＶＡＮコード
                            where
                                //z.使用期限 <= DateTime.Now.AddMonths(期限切迫期間)
                                FirstDayThisMonth <= z.使用期限 && // 期限が切れていないもの
                                z.使用期限 <= DateTime.Now.AddMonths(期限切迫期間)
                            select x).ToList();


            var OrderDeadorExp = orderDead.Concat(orderExp).Distinct(new OrderScheduledListEntityComparer()).OrderBy(x => x.医薬品名).ToList();


            end = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("D:" + (end - start).TotalMilliseconds);


            start = DateTime.Now;

            // デッド品で該当抽出
            var Dead = (
                from x in SENDとMEDIS結合
                join z in 自店舗を外した店舗ごと不動品list
                on
                    x.medis.レセプト電算コード equals z.レセプト電算コード
                where
                    FirstDayThisMonth <= z.使用期限 // 期限が切れていないもの
                select new ExpDeadListEntity
                {
                    店名 = z.店名,
                    レセプト電算コード = x.medis.レセプト電算コード,
                    医薬品名 = z.医薬品名,//医薬品名 = x.medis.医薬品名,
                    在庫数 = z.在庫数,
                    薬価 = z.薬価,
                    包装形態 = x.medis.包装形態,
                    包装単位数 = x.medis.包装単位数,
                    包装単位 = x.medis.包装単位,
                    包装総量 = x.medis.包装総量,
                    剤形区分 = x.medis.剤形区分,
                    使用期限 = z.使用期限,
                    名称２ = z.名称２,
                    一包単位量 = z.一包単位量,
                    Isデッド品 = true,
                    Is期限切迫 = false
                }
                ).ToList();



            // 期限切迫で該当抽出
            var Exp = (
                            from x in SENDとMEDIS結合
                            join z in 自店舗を外した店舗ごと現在庫list
                            on
                                x.medis.レセプト電算コード equals z.レセプト電算コード
                            where
                                FirstDayThisMonth <= z.使用期限 && // 期限が切れていないもの
                                z.使用期限 <= DateTime.Now.AddMonths(期限切迫期間) // 期限切迫
                            select new ExpDeadListEntity
                            {
                                店名 = z.店名,
                                レセプト電算コード = x.medis.レセプト電算コード,
                                医薬品名 = z.医薬品名,
                                //医薬品名 = string.IsNullOrEmpty(z.名称２) ? z.医薬品名 : string.Format("{0}（{1}）", z.医薬品名, z.名称２),
                                //医薬品名 = x.medis.医薬品名,
                                在庫数 = z.在庫数,
                                薬価 = z.薬価,
                                包装形態 = x.medis.包装形態,
                                包装単位数 = x.medis.包装単位数,
                                包装単位 = x.medis.包装単位,
                                包装総量 = x.medis.包装総量,
                                剤形区分 = x.medis.剤形区分,
                                Isデッド品 = false,
                                Is期限切迫 = true,
                                名称２ = z.名称２,
                                使用期限 = z.使用期限
                            }
                            ).ToList();


            // デッドかつ期限切迫で該当抽出
            var DeadAndExp = (
                            from x in Dead
                            join y in Exp
                                on new { a = x.店名, b = x.レセプト電算コード } equals new { a = y.店名, b = y.レセプト電算コード }
                            where
                                x.Isデッド品 == true &&
                                y.Is期限切迫 == true &&
                                FirstDayThisMonth <= x.使用期限 && // 期限が切れていないもの
                                x.使用期限 <= DateTime.Now.AddMonths(期限切迫期間) // 期限切迫
                            select new ExpDeadListEntity
                            {
                                店名 = x.店名,
                                レセプト電算コード = y.レセプト電算コード,
                                医薬品名 = x.医薬品名,
                                在庫数 = y.在庫数,
                                薬価 = x.薬価,
                                包装形態 = y.包装形態,
                                包装単位数 = y.包装単位数,
                                包装単位 = y.包装単位,
                                包装総量 = y.包装総量,
                                剤形区分 = y.剤形区分,
                                Isデッド品 = true,
                                Is期限切迫 = true,
                                名称２ = x.名称２,
                                一包単位量 = x.一包単位量,
                                使用期限 = y.使用期限
                            }
                            ).ToList();


            // 一度Dead/ExpからDeadAndExpを削除してからDeadAndExpを追加する
            var stockDeadorExp = Dead.Concat(Exp).Except(DeadAndExp, new ExpDeadListEntity店名医薬品名一致Comparer()).Concat(DeadAndExp).OrderBy(x => x.医薬品名).ThenBy(x => x.店名).ToList();


            end = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("E:" + (end - start).TotalMilliseconds);


            start = DateTime.Now;


            // 保護リストを削除する
            var RemoveProtect =
                            (from x in stockDeadorExp
                             join y in 保護リストAll
                                 on new { a = x.店名, b = x.レセプト電算コード } equals new { a = y.店舗名, b = y.レセプト電算コード } into gj
                             where
                                 gj.FirstOrDefault() == null // 該当なしのものだけ
                             select x)
                            .ToList();


            var 新追加する優先リスト = new List<ExpDeadListEntity>();

            // 優先移動リストを追加
            foreach (var row in 優先移動リストAll)
            {
                var res = (from x in RemoveProtect
                           where
                             x.店名 == row.店舗名
                             &&
                             x.レセプト電算コード == row.レセプト電算コード
                           select x).ToList();

                // デッドまたは期限切迫で含まれている場合
                if (res.Count() != 0)
                {
                    res.ForEach(delegate(ExpDeadListEntity e)
                    {
                        e.Is優先移動 = true;
                        e.優先移動コメント = row.コメント;
                    });

                }
                // デッドまたは期限切迫で含まれていない場合
                else
                {
                    // デッド切迫該当なしだけど、現在庫であれば追加
                    var addlist = (
                              from x in SENDとMEDIS結合
                              join z in 自店舗を外した店舗ごと現在庫list
                              on
                                  x.medis.レセプト電算コード equals z.レセプト電算コード
                              join u in 優先移動リストAll
                                  on new { a = z.店名, b = z.レセプト電算コード } equals new { a = u.店舗名, b = u.レセプト電算コード }
                              where
                                  FirstDayThisMonth <= z.使用期限 // 期限が切れていないもの
                                  &&
                                  z.レセプト電算コード == row.レセプト電算コード
                                  &&
                                  u.店舗名 == row.店舗名
                              select new ExpDeadListEntity
                              {
                                  店名 = z.店名,
                                  レセプト電算コード = x.medis.レセプト電算コード,
                                  医薬品名 = z.医薬品名と名称２連結,
                                  //医薬品名 = string.IsNullOrEmpty(z.名称２) ? z.医薬品名 : string.Format("{0}（{1}）", z.医薬品名, z.名称２),
                                  //医薬品名 = x.medis.医薬品名,
                                  在庫数 = z.在庫数,
                                  薬価 = z.薬価,
                                  包装形態 = x.medis.包装形態,
                                  包装単位数 = x.medis.包装単位数,
                                  包装単位 = x.medis.包装単位,
                                  包装総量 = x.medis.包装総量,
                                  剤形区分 = x.medis.剤形区分,
                                  Isデッド品 = false,
                                  Is期限切迫 = false,
                                  Is優先移動 = true,
                                  優先移動コメント = u.コメント,
                                  名称２ = z.名称２,
                                  使用期限 = z.使用期限
                              }
                              ).ToList();



                    if (addlist.Count != 0)
                    {
                        新追加する優先リスト = 新追加する優先リスト.Concat(addlist).ToList();
                    }

                }
            }

            var AddPriority = RemoveProtect.Concat(新追加する優先リスト).OrderBy(x => x.医薬品名と名称２連結).ThenBy(x => x.店名).ToList();

            end = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("F:" + (end - start).TotalMilliseconds);


            win.lvOrderAllDisp.ItemsSource = order全て;
            win.lvOrderOnlyCheckBalancingFalse.ItemsSource = OrderOnlyCheckBalancingFalse;
            win.lvOrderDeadAndExpWithBalancingAccounts.ItemsSource = OrderDeadorExp;
            win.ExpDeadList = AddPriority;
            win.lvExpDeadAllDisp.ItemsSource = win.ExpDeadList;

        }
        public static List<現在庫Entity> Load現在庫Total()
        {
            List<現在庫Entity> 現在庫list = new List<現在庫Entity>();
            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.Download現在庫TotalFilePath, Encoding.GetEncoding(932)))
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

                    現在庫Entity ent = new 現在庫Entity();

                    var sepa = line.Split(',');


                    // 一時的に９桁と１０桁がまじるので
                    // 名称２追加の移行期間は終了したので、いまは全て10
                    if (sepa.Length != 9 && sepa.Length != 10)
                    {
                        throw new Exception("現在庫Totalにインデックスが１０ではない不正なデータが含まれて降ります。\r\nCounter: " + counter);
                    }

                    if (sepa.Length == 9)
                    {
                        // 1.店名
                        // 2.レセプト電算コード
                        // 3.医薬品名
                        // 4.在庫数
                        // 5.使用期限
                        // 6.薬価
                        // 7.販売会社
                        // 8.後発区分
                        // 9.最終更新日時


                        if (sepa[0] == "店舗名" || sepa[1] == "薬品コード" || sepa[2] == "薬品名" || sepa[3] == "在庫数" || sepa[4] == "使用期限" || sepa[5] == "薬価" || sepa[6] == "メーカー名" || sepa[8] == "最終更新日時")
                        {
                            continue;
                        }


                        ent.店名 = sepa[0];

                        if (sepa[1].Length == 13)
                        {
                            ent.レセプト電算コード = sepa[1].Substring(0, 9); //(10～13桁目の枝番は外す)
                        }
                        else
                        {
                            ent.レセプト電算コード = "";
                        }
                        ent.医薬品名 = sepa[2];

                        double result;
                        if (double.TryParse(sepa[3], out result) == false)
                        {
                            throw new Exception("現在庫Totalに数量が不正であるデータが含まれております。\r\n在庫数: " + sepa[3] + "\r\nCounter: " + counter);
                        }
                        ent.在庫数 = result;

                        DateTime result2;
                        if (DateTime.TryParse(sepa[4], out result2) == false)
                        {
                            ent.使用期限 = new DateTime(1900, 1, 1);
                            // throw new Exception("現在庫Totalに使用期限が不正であるデータが含まれております。\r\n使用期限: " + sepa[4] + "\r\nCounter: " + counter);
                        }
                        else
                        {
                            ent.使用期限 = ViewModel.Common.DateCenter.Converter.GetLastDay(result2);
                        }


                        double result3;
                        if (double.TryParse(sepa[5], out result3) == false)
                        {
                            throw new Exception("現在庫Totalに薬価が不正であるデータが含まれております。\r\n薬価: " + sepa[5] + "\r\nCounter: " + counter);
                        }
                        ent.薬価 = result3;


                        ent.販売会社 = sepa[6];

                        ent.Is後発品 = sepa[7] == "後発品" ? true : false;


                        DateTime result4;
                        if (DateTime.TryParse(sepa[8], out result4) == false)
                        {
                            throw new Exception("現在庫Totalに最終更新日時が不正であるデータが含まれております。\r\n最終更新日時: " + sepa[8] + "\r\nCounter: " + counter);
                        }

                        ent.最終更新日時 = result4;

                        現在庫list.Add(ent);
                    }

                    else if (sepa.Length == 10)
                    {
                        // 1.店名
                        // 2.レセプト電算コード
                        // 3.医薬品名
                        // 4.在庫数
                        // 5.使用期限
                        // 6.薬価
                        // 7.販売会社
                        // 8.後発区分
                        // 9.名称２
                        //10.最終更新日時


                        if (sepa[0] == "店舗名" || sepa[1] == "薬品コード" || sepa[2] == "薬品名" || sepa[3] == "在庫数" || sepa[4] == "使用期限" || sepa[5] == "薬価" || sepa[6] == "メーカー名" || sepa[8] == "名称２" || sepa[9] == "最終更新日時")
                        {
                            continue;
                        }


                        ent.店名 = sepa[0];

                        if (sepa[1].Length == 13)
                        {
                            ent.レセプト電算コード = sepa[1].Substring(0, 9); //(10～13桁目の枝番は外す)
                        }
                        else
                        {
                            ent.レセプト電算コード = "";
                        }
                        ent.医薬品名 = sepa[2];

                        double result;
                        if (double.TryParse(sepa[3], out result) == false)
                        {
                            throw new Exception("現在庫Totalに数量が不正であるデータが含まれております。\r\n在庫数: " + sepa[3] + "\r\nCounter: " + counter);
                        }
                        ent.在庫数 = result;

                        DateTime result2;
                        if (DateTime.TryParse(sepa[4], out result2) == false)
                        {
                            ent.使用期限 = new DateTime(1900, 1, 1);
                            // throw new Exception("現在庫Totalに使用期限が不正であるデータが含まれております。\r\n使用期限: " + sepa[4] + "\r\nCounter: " + counter);
                        }
                        else
                        {
                            ent.使用期限 = ViewModel.Common.DateCenter.Converter.GetLastDay(result2);
                        }


                        double result3;
                        if (double.TryParse(sepa[5], out result3) == false)
                        {
                            throw new Exception("現在庫Totalに薬価が不正であるデータが含まれております。\r\n薬価: " + sepa[5] + "\r\nCounter: " + counter);
                        }
                        ent.薬価 = result3;


                        ent.販売会社 = sepa[6];

                        ent.Is後発品 = sepa[7] == "後発品" ? true : false;

                        ent.名称２ = sepa[8];

                        DateTime result4;
                        if (DateTime.TryParse(sepa[9], out result4) == false)
                        {
                            throw new Exception("現在庫Totalに最終更新日時が不正であるデータが含まれております。\r\n最終更新日時: " + sepa[9] + "\r\nCounter: " + counter);
                        }

                        ent.最終更新日時 = result4;


                        現在庫list.Add(ent);

                    }
                }
            }

            return 現在庫list;

        }
        public static List<不動品Entity> Load不動品Total()
        {
            List<不動品Entity> 不動品list = new List<不動品Entity>();
            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.Download不動品TotalFilePath, Encoding.GetEncoding(932)))
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


                    不動品Entity ent = new 不動品Entity();

                    var sepa = line.Split(',');

                    // 0.店名
                    // 1.レセプト電算コード
                    // 2.医薬品名
                    // 3.在庫数
                    // 4.使用期限
                    // 5.薬価
                    // 6.１包単位量  　//2015.05.02追加   
                    // 7.名称２　　　　//2015.05.02追加
                    // 8.最終更新日時

                    if (sepa.Length != 9)
                    //if (sepa.Length != 7 && sepa.Length != 9)
                    {
                        throw new Exception(string.Format("不動品Totalに列数が９ではない不正なデータが含まれています。\r\n行番号:{0} ", counter));
                    }

                    //if (sepa[0] == "店舗名" || sepa[1] == "薬品コード" || sepa[2] == "薬品名" || sepa[3] == "現在庫d" || sepa[4] == "使用期限" || sepa[5] == "薬価b" || sepa[6] == "最終更新日時")
                    if (sepa[0] == "店舗名" || sepa[1] == "薬品コード" || sepa[2] == "薬品名" || sepa[3] == "現在庫d" || sepa[4] == "使用期限" || sepa[5] == "薬価b" || sepa[6] == "１包単位量" || sepa[7] == "名称２")
                    {
                        continue;
                    }

                    ent.店名 = sepa[0];
                    ent.レセプト電算コード = sepa[1];
                    ent.医薬品名 = sepa[2];

                    double result;
                    if (double.TryParse(sepa[3], out result) == false)
                    {
                        throw new Exception("不動品Totalに数量が不正であるデータが含まれております。\r\n在庫数: " + sepa[3] + "\r\nCounter: " + counter);
                    }
                    ent.在庫数 = result;

                    DateTime result2;
                    if (DateTime.TryParse(sepa[4], out result2) == false)
                    {

                        ent.使用期限 = new DateTime(1900, 1, 1);
                        // throw new Exception("不動品Totalに使用期限が不正であるデータが含まれております。\r\n使用期限: " + sepa[4] + "\r\nCounter: " + counter);
                    }
                    else
                    {
                        ent.使用期限 = ViewModel.Common.DateCenter.Converter.GetLastDay(result2);
                    }



                    double result3;
                    if (double.TryParse(sepa[5], out result3) == false)
                    {
                        throw new Exception("不動品Totalに薬価が不正であるデータが含まれております。\r\n薬価: " + sepa[5] + "\r\nCounter: " + counter);
                    }
                    ent.薬価 = result3;



                    double result5 = 0;
                    if (sepa[6] == "")
                    {
                        ent.一包単位量 = 0;
                    }
                    else
                    {
                        if (double.TryParse(sepa[6], out result3) == false)
                        {
                            throw new Exception("不動品Totalに一包単位量が不正であるデータが含まれております。\r\n一包単位量: " + sepa[7] + "\r\nCounter: " + counter);
                        }

                        ent.一包単位量 = result5;
                    }

                    ent.名称２ = sepa[7];



                    DateTime result4;
                    if (sepa.Length == 9)
                    {
                        if (DateTime.TryParse(sepa[8], out result4) == false)
                        {
                            throw new Exception("不動品Totalに最終更新日時が不正であるデータが含まれております。\r\n最終更新日時: " + sepa[8] + "\r\nCounter: " + counter);
                        }
                    }
                    else
                    {
                        if (DateTime.TryParse(sepa[sepa.Count() - 1], out result4) == false)
                        {
                            throw new Exception("不動品Totalに最終更新日時が不正であるデータが含まれております。\r\n最終更新日時: " + sepa[sepa.Count() - 1] + "\r\nCounter: " + counter);
                        }
                    }

                    //if (sepa.Length == 7)
                    //{
                    //    if (DateTime.TryParse(sepa[6], out result4) == false)
                    //    {
                    //        throw new Exception("不動品Totalに最終更新日時が不正であるデータが含まれております。\r\n最終更新日時: " + sepa[6] + "\r\nCounter: " + counter);
                    //    }
                    //}
                    //else
                    //{
                    //    if (DateTime.TryParse(sepa[8], out result4) == false)
                    //    {
                    //        throw new Exception("不動品Totalに最終更新日時が不正であるデータが含まれております。\r\n最終更新日時: " + sepa[8] + "\r\nCounter: " + counter);
                    //    }
                    //}


                    ent.最終更新日時 = result4;




                    不動品list.Add(ent);
                }
            }

            return 不動品list;

        }

        public static List<MEDIS_HOT13Entity> LoadMEDIS_HOT13And個別管理医薬品マスタ()
        {

            var retList = LoadMEDIS_HOT13();
            var 個別管理list = LoadCenter.Load個別管理医薬品マスタ();

            foreach (var row in 個別管理list)
            {
                MEDIS_HOT13Entity ent = new MEDIS_HOT13Entity();
                ent.JANコード = row.JANコード;
                ent.レセプト電算コード = "";
                ent.医薬品名 = row.医薬品名;
                ent.個別医薬品コード = "";
                ent.剤形区分 = row.剤形区分;
                ent.製薬会社 = row.製薬会社;
                ent.販売会社 = row.販売会社;
                ent.包装形態 = row.包装形態;
                ent.包装総量 = row.包装総量;
                ent.包装単位 = row.包装単位;
                ent.包装単位数 = row.包装単位数;

                retList.Add(ent);
            }

            return retList;



        }


        public static List<MEDIS_HOT13Entity> LoadMEDIS_HOT13()
        {
            List<MEDIS_HOT13Entity> MEDIS_HOT13list = new List<MEDIS_HOT13Entity>();
            using (StreamReader sr = new StreamReader(OASystem.Common.Settings.DownloadMEDIS_HOT13lFilePath, Encoding.GetEncoding(932)))
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

                    MEDIS_HOT13Entity ent = new MEDIS_HOT13Entity();

                    var sepa = line.Split(',');

                    //  6.JANコード
                    //  8.個別医薬品コード
                    //  9.レセプト電算コード
                    // 12.医薬品名
                    // 15.包装形態
                    // 16.包装単位数
                    // 17.包装単位
                    // 18.包装総量
                    // 20.区分
                    // 21.製薬会社
                    // 22.販売会社

                    // 今後増える可能性もあるので現状の24以下とする
                    if (sepa.Length < 24)
                    {
                        throw new Exception("MEDIS_HOT13にインデックスが２４ではない不正なデータが含まれて降ります。\r\nCounter: " + counter);
                    }


                    ent.JANコード = sepa[5].Replace("\"", "");
                    ent.個別医薬品コード = sepa[7].Replace("\"", "");
                    ent.レセプト電算コード = sepa[8].Replace("\"", "");
                    ent.医薬品名 = sepa[11].Replace("\"", "");
                    ent.包装形態 = sepa[14].Replace("\"", "");
                    ent.包装単位数 = sepa[15].Replace("\"", "");
                    ent.包装単位 = sepa[16].Replace("\"", "");
                    ent.包装総量 = sepa[17].Replace("\"", "");
                    ent.剤形区分 = DataConvert.漢字To剤形Enum(sepa[19].Replace("\"", ""));
                    ent.製薬会社 = sepa[20].Replace("\"", "");
                    ent.販売会社 = sepa[21].Replace("\"", "");


                    MEDIS_HOT13list.Add(ent);
                }
            }

            return MEDIS_HOT13list;
        }

        public static List<BalancingAccountsCheckResultEntity> CheckBalancingAccounts(List<SEND01DATEntity> sendList)
        {
            var MEDIS_HOT13listAnd個別管理 = LoadMEDIS_HOT13And個別管理医薬品マスタ()
                .Where(x => 12 <= x.JANコード.Length).ToArray();

            // 個別管理医薬品マスタをMEDIS_HOT13listに加える


            // 発注予定リストで全て表示用
            var order全て = (
                from x in sendList
                join y in MEDIS_HOT13listAnd個別管理
                    on
                    x.JANコード.Substring(0, 12) equals y.JANコード.Substring(0, 12)
                    //on
                //    x.JANコード equals y.JANコード
                select new BalancingAccountsCheckResultEntity
                {
                    JANコード = y.JANコード,
                    医薬品名 = y.医薬品名,
                    数量 = x.数量,
                    包装形態 = y.包装形態,
                    包装単位数 = y.包装単位数,
                    包装単位 = y.包装単位,
                    包装総量 = y.包装総量,
                    剤形区分 = y.剤形区分,
                    製薬会社 = y.製薬会社,
                    販売会社 = y.販売会社,
                    レセプト電算コード = y.レセプト電算コード,
                    個別医薬品コード = y.個別医薬品コード,
                    注文帳合先VANコード = x.卸VANコード
                }
                ).ToList();


            var 医薬品別マスタ = LoadCenter.Load帳合先チェックマスタ医薬品別();
            var メーカー別 = LoadCenter.Load帳合先チェックマスタメーカー別_その他のメーカー以外();
            var メーカー別その他 = LoadCenter.Load帳合先チェックマスタメーカー別_その他のメーカーのみ();


            List<BalancingAccountsCheckResultEntity> retult = new List<BalancingAccountsCheckResultEntity>();

            foreach (var row in order全て)
            {
                bool HasError = false;

                //医薬品別
                //①JAN管理のみチェック
                //個別管理医薬品マスタのチェックもここで
                var checkJan = (from x in 医薬品別マスタ
                                where
                                     x.JANコード == row.JANコード &&
                                     x.IsJAN管理 == true
                                //   &&  x.修正後帳合先 != row.注文帳合先VANコード // 修正後帳合先　はVANコードで
                                select x).ToList();


                if (checkJan.Count != 0)
                {

                    if (checkJan.Count == 1)
                    {
                        //JANが一緒で帳合先が違うもの
                        if (checkJan[0].修正後帳合先 != row.注文帳合先VANコード)
                        {
                            HasError = true;
                            //row.エラー内容 = "帳合先が設定と違います。\r\nこのJANコードは帳合先が指定されております。";
                            row.エラー内容 = "帳合先が正しくありません。\r\nこの医薬品は商品ごとに帳合先が設定されております。";
                            row.設定帳合先VANコード = checkJan[0].修正後帳合先;
                        }
                        //帳合先が正しかった場合は次へ
                        else
                        {
                            continue;
                        }

                    }
                    else if (2 <= checkJan.Count)
                    {
                        HasError = true;
                        row.エラー内容 = "この医薬品はJAN管理の設定データが２件以上登録があった為、チェックできませんでした。\r\n管理者にご連絡ください。";
                        // row.設定帳合先VANコード = checkJan[0].修正後帳合先;
                    }

                }

                if (HasError)
                {
                    retult.Add(row);
                    continue;
                }

                //②JAN管理以外チェック

                var checkNotJan =
                               (from x in 医薬品別マスタ
                                where
                                     x.IsJAN管理 == false &&
                                     x.レセプト電算コード == row.レセプト電算コード
                                //x.修正後帳合先 != row.注文帳合先VANコード // 修正後帳合先　はVANコードで
                                select x).ToList();

                if (checkNotJan.Count != 0)
                {

                    if (checkNotJan.Count == 1)
                    {
                        //レセプト電算コードが一緒で帳合先が違うもの
                        if (checkNotJan[0].修正後帳合先 != row.注文帳合先VANコード)
                        {
                            HasError = true;
                            //row.エラー内容 = "帳合先が設定と違います。\r\n非JAN管理で帳合先が指定されております。";
                            row.エラー内容 = "帳合先が正しくありません。\r\nこの医薬品は商品ごとに帳合先が設定されております。";
                            row.設定帳合先VANコード = checkNotJan[0].修正後帳合先;
                        }
                        //帳合先が正しかった場合は次へ
                        else
                        {
                            continue;
                        }

                    }
                    else if (2 <= checkNotJan.Count)
                    {
                        HasError = true;
                        row.エラー内容 = "この医薬品は非JAN管理の設定データが２件以上登録があった為、チェックできませんでした。\r\n管理者にご連絡ください。";
                        // row.設定帳合先VANコード = checkJan[0].修正後帳合先;
                    }

                }



                //if (checkNotJan.Count != 0)
                //{
                //    HasError = true;
                //    row.エラー内容 = "帳合先が設定と違います。\r\n非JAN管理で帳合先が指定されております。";
                //    row.設定帳合先VANコード = checkNotJan[0].修正後帳合先;
                //}

                if (HasError)
                {
                    retult.Add(row);
                    continue;
                }

                // メーカー別
                //①リストにあるメーカー別
                //A.リストにあるメーカーで帳合一致していれば終了
                var checkMakerSort =
                               (from x in メーカー別
                                where
                                     x.Key == row.販売会社 &&
                                     x.Value.卸ＶＡＮコード == row.注文帳合先VANコード
                                select x).ToList();

                if (1 <= checkMakerSort.Count)
                {
                    continue;
                }

                //B.リストにあるメーカーで帳合一致していなければエラー
                checkMakerSort =
               (from x in メーカー別
                where
                     x.Key == row.販売会社 &&
                     x.Value.卸ＶＡＮコード != row.注文帳合先VANコード
                select x).ToList();


                if (checkMakerSort.Count != 0)
                {
                    HasError = true;
                    //row.エラー内容 = "帳合先が設定と違います。\r\nメーカー別で帳合先が指定されております。";
                    row.エラー内容 = "帳合先が正しくありません。";
                    row.設定帳合先VANコード = checkMakerSort[0].Value.卸ＶＡＮコード;
                }
                if (HasError)
                {
                    retult.Add(row);
                    continue;
                }


                //②その他のメーカー
                var checkMakerSortその他 =
                               (from x in メーカー別その他
                                where
                                     x.Value.卸ＶＡＮコード != row.注文帳合先VANコード
                                select x).ToList();
                if (checkMakerSortその他.Count != 0)
                {
                    HasError = true;
                    row.エラー内容 = "帳合先が正しくありません。";
                    //row.エラー内容 = "帳合先が設定と違います。\r\nその他のメーカーとして帳合先が指定されております。";
                    row.設定帳合先VANコード = checkMakerSortその他[0].Value.卸ＶＡＮコード;
                }
                if (HasError)
                {
                    retult.Add(row);
                    continue;
                }

            }

            return retult.OrderBy(x => x.医薬品名).ToList();
        }


    }
}
