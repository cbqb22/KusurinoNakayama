using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCSystem.Model;
using MCSystem.Properties;

namespace MCSystem.ViewModel
{
    public static class 在庫テーブルLoader
    {
        public static List<在庫テーブルEntity> Load在庫テーブル()
        {
            List<在庫テーブルEntity> listEnt = new List<在庫テーブルEntity>();

            using (StreamReader sr = new StreamReader(DI.在庫テーブルCSVパス, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {

                    bool IsErrorData = false;
                    counter++;
                    if (counter == 1)
                    {
                        continue;
                    }

                    var sepa = line.Split(',');

                    var cleansepa0 = sepa[0].Replace("\"", "").Replace(" ", "");

                    // sepa[0]=商品コード
                    // sepa[1]=医薬品名
                    // sepa[12]=通常仕入先コード
                    // sepa[13]=通常仕入先名

                    if (cleansepa0.Length != 13)
                    {
                        IsErrorData = true;
                    }

                    在庫テーブルEntity ent = new 在庫テーブルEntity();
                    int 通常仕入先コード = 0;
                    if (sepa[12] != "")
                    {
                        if (int.TryParse(sepa[12], out 通常仕入先コード) == false)
                        {
                            IsErrorData = true;
                        }

                    }




                    if (!IsErrorData)
                    {
                        ent.商品コード = cleansepa0;
                        ent.医薬品名 = sepa[1];
                        ent.通常仕入先名 = sepa[12];
                        ent.通常仕入先コード = 通常仕入先コード.ToString();
                        ent.IsErrorData = false;

                        listEnt.Add(ent);
                    }
                    else
                    {
                        ent.商品コード = cleansepa0;
                        ent.医薬品名 = sepa[1];
                        ent.通常仕入先名 = sepa[12];
                        ent.通常仕入先コード = sepa[13];
                        ent.IsErrorData = true;
                    }


                }
            }

            return listEnt;
        }
        public static List<在庫テーブルEntity> Load在庫テーブル複数行取得(int 合計取得行数)
        {

            List<在庫テーブルEntity> list = new List<在庫テーブルEntity>();

            using (StreamReader sr = new StreamReader(DI.在庫テーブルCSVパス, Encoding.GetEncoding(932)))
            {
                string line = "";
                int rowcounter = 0;
                int 取得数 = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    rowcounter++;
                    if (rowcounter == 1)
                    {
                        continue;
                    }

                    line = line.Replace("\"","");

                    var sepa = line.Split(',');

                    // sepa[0]=商品コード
                    // sepa[1]=医薬品名
                    // sepa[12]=通常仕入先コード
                    // sepa[13]=通常仕入先名
                    // sepa[16]=JANコード

                    var cleansepa0 = sepa[0].Replace("\"", "").Replace(" ", "");
                    if (cleansepa0.Length != 13)
                    {
                        continue;
                    }

                    int 通常仕入先コード = 0;
                    if (sepa[12] != "")
                    {
                        if (int.TryParse(sepa[12], out 通常仕入先コード) == false)
                        {
                            continue;
                        }

                    }

                    if (通常仕入先コード <= 0)
                    {
                        continue;
                    }

                    //// JANコードが入っていたら外す 
                    //// マクロTESTはF12完了しないことにしたので、JANコード未登録エラーにはならなくなった。
                    //if (sepa[16] != "")
                    //{
                    //    continue;
                    //}

                    在庫テーブルEntity ent = new 在庫テーブルEntity();

                    ent.商品コード = cleansepa0;
                    ent.医薬品名 = sepa[1];
                    ent.通常仕入先名 = sepa[12];
                    ent.通常仕入先コード = 通常仕入先コード.ToString();
                    ent.IsErrorData = false;

                    list.Add(ent);
                    取得数++;

                    if (合計取得行数 == 取得数)
                    {
                        break;
                    }


                }
            }

            return list;

        }

    }
}
