using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using MCSystem.Model;

namespace MCSystem.ViewModel
{
    public static class ControlBCSettings
    {

        public static BCSettingsEntity LoadSettingsFromFile()
        {
            var folder = System.IO.Directory.GetCurrentDirectory();
            var inipath = Path.Combine(folder, "Settings.ini");

            BCSettingsEntity ent = new BCSettingsEntity();

            using (StreamReader sr = new StreamReader(inipath, Encoding.GetEncoding(932)))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if(line.StartsWith("[検索名称XY座標]="))
                    {
                        var str = line.Substring(11);
                        var sepa = str.Split(',');
                        if (sepa.Count() != 2)
                        {
                            continue;
                        }

                        int resultX;
                        if (int.TryParse(sepa[0], out resultX) == false)
                        {
                            continue;
                        }
                        int resultY;
                        if (int.TryParse(sepa[1], out resultY) == false)
                        {
                            continue;
                        }
                        ent.検索名称XY座標 = new System.Windows.Rect((double)resultX,(double)resultY,0,0);

                    }
                    else if(line.StartsWith("[検索名称完了ボタンXY座標]="))
                    {

                        var str = line.Substring(16);
                        var sepa = str.Split(',');
                        if (sepa.Count() != 2)
                        {
                            continue;
                        }

                        int resultX;
                        if (int.TryParse(sepa[0], out resultX) == false)
                        {
                            continue;
                        }
                        int resultY;
                        if (int.TryParse(sepa[1], out resultY) == false)
                        {
                            continue;
                        }
                        ent.検索名称完了ボタンXY座標 = new System.Windows.Rect((double)resultX, (double)resultY, 0, 0);

                    }
                    else if(line.StartsWith("[通常仕入先XY座標]="))
                    {
                        var str = line.Substring(12);
                        var sepa = str.Split(',');
                        if (sepa.Count() != 2)
                        {
                            continue;
                        }

                        int resultX;
                        if (int.TryParse(sepa[0], out resultX) == false)
                        {
                            continue;
                        }
                        int resultY;
                        if (int.TryParse(sepa[1], out resultY) == false)
                        {
                            continue;
                        }
                        ent.通常仕入先XY座標 = new System.Windows.Rect((double)resultX, (double)resultY, 0, 0);

                    }

                    else if(line.StartsWith("[個別入力完了ボタンXY座標]="))
                    {
                        var str = line.Substring(16);
                        var sepa = str.Split(',');
                        if (sepa.Count() != 2)
                        {
                            continue;
                        }

                        int resultX;
                        if (int.TryParse(sepa[0], out resultX) == false)
                        {
                            continue;
                        }
                        int resultY;
                        if (int.TryParse(sepa[1], out resultY) == false)
                        {
                            continue;
                        }
                        ent.個別入力完了ボタンXY座標 = new System.Windows.Rect((double)resultX, (double)resultY, 0, 0);

                    }

                    else if (line.StartsWith("[在庫メンテナンス受付範囲]="))
                    {

                        var str = line.Substring(15);
                        var sepa = str.Split(',');
                        if (sepa.Count() != 4)
                        {
                            continue;
                        }

                        int resultX;
                        if (int.TryParse(sepa[0], out resultX) == false)
                        {
                            continue;
                        }
                        int resultY;
                        if (int.TryParse(sepa[1], out resultY) == false)
                        {
                            continue;
                        }

                        double width;
                        if (double.TryParse(sepa[2], out width) == false)
                        {
                            continue;
                        }
                        double height;
                        if (double.TryParse(sepa[3], out height) == false)
                        {
                            continue;
                        }

                        ent.在庫メンテナンス受付範囲 = new System.Windows.Rect((double)resultX, (double)resultY, width, height);

                    }
                    else if (line.StartsWith("[在庫メンテナンス範囲]="))
                    {

                        var str = line.Substring(13);
                        var sepa = str.Split(',');
                        if (sepa.Count() != 4)
                        {
                            continue;
                        }

                        int resultX;
                        if (int.TryParse(sepa[0], out resultX) == false)
                        {
                            continue;
                        }
                        int resultY;
                        if (int.TryParse(sepa[1], out resultY) == false)
                        {
                            continue;
                        }

                        double width;
                        if (double.TryParse(sepa[2], out width) == false)
                        {
                            continue;
                        }
                        double height;
                        if (double.TryParse(sepa[3], out height) == false)
                        {
                            continue;
                        }

                        ent.在庫メンテナンス範囲 = new System.Windows.Rect((double)resultX, (double)resultY, width, height);

                    }
                    else if (line.StartsWith("[新帳合変更データ表パス]="))
                    {
                        var str = line.Substring(14);
                        ent.新帳合変更データ表パス = str;

                    }
                    else if (line.StartsWith("[在庫テーブルCSVパス]="))
                    {
                        var str = line.Substring(14);
                        ent.在庫テーブルCSVパス = str;

                    }
                    else if (line.StartsWith("[メディセオ]="))
                    {
                        var str = line.Substring(8);
                        int result;
                        if (int.TryParse(str, out result) == false)
                        {
                            ent.メディセオコード = -1;
                        }
                        else
                        {
                            ent.メディセオコード = result;
                        }

                    }
                    else if(line.StartsWith("[スズケン]="))
                    {
                        var str = line.Substring(7);
                        int result;
                        if (int.TryParse(str, out result) == false)
                        {
                            ent.スズケンコード = -1;
                        }
                        else
                        {
                            ent.スズケンコード = result;
                        }
                    }
                    else if(line.StartsWith("[東邦薬品]="))
                    {
                        var str = line.Substring(7);
                        int result;
                        if (int.TryParse(str, out result) == false)
                        {
                            ent.東邦薬品コード = -1;
                        }
                        else
                        {
                            ent.東邦薬品コード = result;
                        }

                    }
                    else if(line.StartsWith("[東和薬品]="))
                    {
                        var str = line.Substring(7);
                        int result;
                        if (int.TryParse(str, out result) == false)
                        {
                            ent.東和薬品コード = -1;
                        }
                        else
                        {
                            ent.東和薬品コード = result;
                        }

                    }
                    else if(line.StartsWith("[アルフレッサ]="))
                    {
                        var str = line.Substring(9);
                        int result;
                        if (int.TryParse(str, out result) == false)
                        {
                            ent.アルフレッサコード = -1;
                        }
                        else
                        {
                            ent.アルフレッサコード = result;
                        }

                    }
                    else if (line.StartsWith("[酒井薬品]="))
                    {
                        var str = line.Substring(7);
                        int result;
                        if (int.TryParse(str, out result) == false)
                        {
                            ent.酒井薬品コード = -1;
                        }
                        else
                        {
                            ent.酒井薬品コード = result;
                        }

                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return ent;
        }

        public static void SetSettingsToDI(BCSettingsEntity bcEnt)
        {
            DI.検索名称XY座標 = bcEnt.検索名称XY座標;
            DI.検索名称完了ボタンXY座標 = bcEnt.検索名称完了ボタンXY座標;
            DI.通常仕入先XY座標 = bcEnt.通常仕入先XY座標;
            DI.個別入力完了ボタンXY座標 = bcEnt.個別入力完了ボタンXY座標;
            DI.在庫メンテナンス受付範囲 = bcEnt.在庫メンテナンス受付範囲;
            DI.在庫メンテナンス範囲 = bcEnt.在庫メンテナンス範囲;
            DI.新帳合変更データ表パス = bcEnt.新帳合変更データ表パス;
            DI.在庫テーブルCSVパス = bcEnt.在庫テーブルCSVパス;
            DI.メディセオコード = bcEnt.メディセオコード;
            DI.スズケンコード = bcEnt.スズケンコード;
            DI.アルフレッサコード = bcEnt.アルフレッサコード;
            DI.東邦薬品コード = bcEnt.東邦薬品コード;
            DI.東和薬品コード = bcEnt.東和薬品コード;
            DI.酒井薬品コード = bcEnt.酒井薬品コード;

        }

        public static void WriteSettings(BCSettingsEntity bcEnt)
        {
            //var folder = Assembly.GetEntryAssembly().Location;
            var folder = System.IO.Directory.GetCurrentDirectory();
            var inipath = Path.Combine(folder, "Settings.ini");

            using(StreamWriter sw = new StreamWriter(inipath,false,Encoding.GetEncoding(932)))
            {
                sw.WriteLine(string.Format("[検索名称XY座標]={0},{1}",(int)bcEnt.検索名称XY座標.X,(int)bcEnt.検索名称XY座標.Y));
                sw.WriteLine(string.Format("[検索名称完了ボタンXY座標]={0},{1}", (int)bcEnt.検索名称完了ボタンXY座標.X, (int)bcEnt.検索名称完了ボタンXY座標.Y));
                sw.WriteLine(string.Format("[通常仕入先XY座標]={0},{1}", (int)bcEnt.通常仕入先XY座標.X, (int)bcEnt.通常仕入先XY座標.Y));
                sw.WriteLine(string.Format("[個別入力完了ボタンXY座標]={0},{1}", (int)bcEnt.個別入力完了ボタンXY座標.X, (int)bcEnt.個別入力完了ボタンXY座標.Y));
                sw.WriteLine(string.Format("[在庫メンテナンス受付範囲]={0},{1},{2},{3}", (int)bcEnt.在庫メンテナンス受付範囲.X, (int)bcEnt.在庫メンテナンス受付範囲.Y,bcEnt.在庫メンテナンス受付範囲.Width,bcEnt.在庫メンテナンス受付範囲.Height));
                sw.WriteLine(string.Format("[在庫メンテナンス範囲]={0},{1},{2},{3}", (int)bcEnt.在庫メンテナンス範囲.X, (int)bcEnt.在庫メンテナンス範囲.Y, bcEnt.在庫メンテナンス範囲.Width, bcEnt.在庫メンテナンス範囲.Height));
                sw.WriteLine(string.Format("[在庫テーブルCSVパス]={0}", bcEnt.在庫テーブルCSVパス));
                sw.WriteLine(string.Format("[新帳合変更データ表パス]={0}", bcEnt.新帳合変更データ表パス));
                sw.WriteLine(string.Format("[メディセオ]={0}", bcEnt.メディセオコード));
                sw.WriteLine(string.Format("[スズケン]={0}", bcEnt.スズケンコード));
                sw.WriteLine(string.Format("[東邦薬品]={0}", bcEnt.東邦薬品コード));
                sw.WriteLine(string.Format("[東和薬品]={0}", bcEnt.東和薬品コード));
                sw.WriteLine(string.Format("[アルフレッサ]={0}", bcEnt.アルフレッサコード));
                sw.WriteLine(string.Format("[酒井薬品]={0}", bcEnt.酒井薬品コード));

                sw.Flush();
            }
        }
    }
}
