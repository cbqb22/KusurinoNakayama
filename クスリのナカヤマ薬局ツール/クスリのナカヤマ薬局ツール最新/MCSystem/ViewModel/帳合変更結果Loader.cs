using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCSystem.Model;
using MCSystem.Properties;

namespace MCSystem.ViewModel
{
    public static class 帳合変更結果Loader
    {
        public static List<帳合変更結果Entity> Load帳合変更結果(string 帳合変更結果filepath)
        {
            List<帳合変更結果Entity> listEnt = new List<帳合変更結果Entity>();

            using (StreamReader sr = new StreamReader(帳合変更結果filepath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {

                    counter++;

                    //1,2行目はヘッダー
                    if (counter < 3)
                    {
                        continue;
                    }

                    var sepa = line.Split(',');

                    // sepa[0]=処理結果
                    // sepa[1]=商品コード
                    // sepa[2]=医薬品名
                    // sepa[3]=通常仕入先名
                    // sepa[4]=新帳合名
                    // sepa[5]=新帳合先コード

                    var issucceeded = sepa[0] == "成功" ? true : false;

                    var cleansepa0 = sepa[1].Replace("\"", "").Replace(" ", "");

                    if (cleansepa0.Length != 13)
                    {
                        continue;
                    }

                    int 新帳合コード;
                    if (int.TryParse(sepa[5], out 新帳合コード) == false)
                    {
                        continue;
                    }

                    帳合変更結果Entity ent = new 帳合変更結果Entity();

                    ent.IsSucceeded = issucceeded;
                    ent.商品コード = cleansepa0;
                    ent.医薬品名 = sepa[2];
                    ent.通常仕入先名 = sepa[3];
                    ent.新帳合先名 = sepa[4];
                    ent.新帳合先コード = 新帳合コード.ToString();

                    listEnt.Add(ent);

                }
            }

            return listEnt;
        }
    }
}
