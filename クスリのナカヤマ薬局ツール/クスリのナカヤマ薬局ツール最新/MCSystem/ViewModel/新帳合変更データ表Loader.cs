using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCSystem.Model;
using MCSystem.Properties;

namespace MCSystem.ViewModel
{
    public static class 新帳合変更データ表Loader
    {
        public static List<新帳合変更データ表Entity> Load新帳合変更データ表()
        {
            List<新帳合変更データ表Entity> listEnt = new List<新帳合変更データ表Entity>();

            using (StreamReader sr = new StreamReader(DI.新帳合変更データ表パス,Encoding.GetEncoding(932)))
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

                    // sepa[0]=商品コード
                    // sepa[1]=医薬品名
                    // sepa[2]=通常仕入先名
                    // sepa[3]=新帳合名称
                    // sepa[4]=新帳合コード

                    var cleansepa0 = sepa[0].Replace("\"","").Replace(" ","");

                    if (cleansepa0.Length != 13)
                    {
                        IsErrorData = true;
                    }

                    int 新帳合コード;
                    if (int.TryParse(sepa[4], out 新帳合コード) == false)
                    {
                        IsErrorData = true;
                    }

                    新帳合変更データ表Entity ent = new 新帳合変更データ表Entity();

                    if (!IsErrorData)
                    {
                        ent.商品コード = cleansepa0;
                        ent.医薬品名 = sepa[1];
                        ent.通常仕入先名 = sepa[2];
                        ent.新帳合先名 = sepa[3];
                        ent.新帳合先コード = 新帳合コード.ToString();
                        ent.IsErrorData = false;

                        listEnt.Add(ent);
                    }
                    else
                    {
                        ent.商品コード = cleansepa0;
                        ent.医薬品名 = sepa[1];
                        ent.通常仕入先名 = sepa[2];
                        ent.新帳合先名 = sepa[3];
                        ent.新帳合先コード = sepa[4];
                        ent.IsErrorData = true;

                    }


                }
            }

            return listEnt;
        }
    }
}
