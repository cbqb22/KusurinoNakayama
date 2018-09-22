using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;
using System.Text.RegularExpressions;
using OASystem.Model.Entity;

namespace OASystem.ViewModel.File
{
    public static class SEND01DATAnalyzer
    {
        public static List<SEND01DATEntity> DoAnalyze(string filepath)
        {
            List<SEND01DATEntity> list = new List<SEND01DATEntity>();
            int 注文番号 = 0;

            string data = "";
            using (IO.StreamReader sr = new IO.StreamReader(filepath, Encoding.GetEncoding(932)))
            {
                data = sr.ReadToEnd();
            }


            //var Spattern = @"S\d{21}"; // S群
            var Spattern = @"S[0-9 ]{26}"; // S群
            var Smatches = Regex.Matches(data, Spattern);

            var Dpattern = @"D\d{18,}"; // D群
            foreach (Match Smatch in Smatches)
            {
                var レセ発注No = Smatch.ToString().Substring(23, 4);
                var vanコード = Smatch.Value.Substring(13, 9);

                var NextSmatch = Smatch.NextMatch();
                var Sstartindex = Smatch.Index;
                var Snextstartindex = NextSmatch.Success ? NextSmatch.Index : data.Length;
                var dataS = data.Substring(Sstartindex, Snextstartindex - Sstartindex);
                var Dmatches = Regex.Matches(dataS, Dpattern);


                foreach (Match Dmatch in Dmatches)
                {
                    var JANPattern = @"14987\d{9}"; //JANコード 医薬品は4987+xxxxxxxxx　で1がSEND用で頭につく 14桁

                    var NextDmatch = Dmatch.NextMatch();
                    var Dstartindex = Dmatch.Index;
                    var Dnextstartindex = NextDmatch.Success ? NextDmatch.Index : dataS.Length;
                    var dataD = dataS.Substring(Dstartindex, Dnextstartindex - Dstartindex);
                    var JANmatches = Regex.Matches(dataD, JANPattern);



                    foreach (Match JANmatch in JANmatches)
                    {
                        var JANコード = JANmatch.Value.Substring(1, 13); // 4987は2番目からなので

                        //throw new Exception("一つのD記号に複数のJANが存在しました。\r\n" +"JANｺｰﾄﾞ:" + JANコード) 

                        var NextJANmatch = JANmatch.NextMatch();
                        var JANstartindex = JANmatch.Index;
                        var JANnextstartindex = NextJANmatch.Success ? NextJANmatch.Index : dataD.Length;
                        var dataJAN = dataD.Substring(JANstartindex, JANnextstartindex - JANstartindex);


                        var 数量コード = dataJAN.Substring(54, 10); // JANの44番目からが数量

                        var 数量 = 数量コード.Insert(5, "."); // 5番目に小数点を入れる
                        while (true)
                        {
                            // 頭の0をとっていく
                            if (数量.Substring(0, 1) == "0")
                            {
                                数量 = 数量.Remove(0, 1);
                            }
                            else
                            {
                                break;
                            }
                        }

                        SEND01DATEntity ent = new SEND01DATEntity();

                        double result;
                        if (double.TryParse(数量, out result) == false)
                        {
                            throw new Exception("SEND01.DAT内の発注数量にエラーがある為、処理を中断します。\r\n" + "該当医薬品JAN:" + JANコード + "\r\n" + "該当数量コード:" + 数量コード);
                        }

                        var d = Dmatch.ToString();
                        if (d.Length < 20) // ２０以下はありえない様
                        {
                            continue;
                        }
                        var レセ発注伝票No = d.Substring(19); // １９番目から後ろ全部が伝票No ３桁以上がありえる。例)960,1011

                        ent.JANコード = JANコード;
                        ent.数量 = result;
                        ent.卸VANコード = vanコード;
                        ent.レセ発注伝票No = レセ発注No;
                        //ent.レセ発注伝票No = レセ発注伝票No;
                        ent.注文番号 = ++注文番号;
                        list.Add(ent);

                    }
                }

            }

            return list;

        }

        //private static void TestData()
        //{
        //List<SEND01DATEntity> list = new List<SEND01DATEntity>();
        //SEND01DATEntity ent = new SEND01DATEntity();
        //ent.レセ発注伝票No = "1011";
        //ent.JANコード = "4987042353201";
        //list.Add(ent);

        //SEND01DATEntity ent2 = new SEND01DATEntity();
        //ent2.レセ発注伝票No = "1016";
        //ent2.JANコード = "4987155077070";
        //list.Add(ent2);

        //SEND01DATAnalyzer.DeleteMedicineFromList(list);

        //}

        /// <summary>
        /// 指定のレセ伝票Noとレセプト電算コードに該当する医薬品をSEND01.DATから削除する
        /// </summary>
        /// <param name="deleteList"></param>
        /// <returns>全て削除された場合はtrue</returns>
        public static bool DeleteMedicineFromList(List<SEND01DATEntity> deleteList)
        {
            if (!IO.File.Exists(OASystem.Properties.Settings.Default.SENDO1DATFilePath))
            {
                throw new Exception("SEND01.DATが存在しない為、卸の注文データを削除できませんでした。");
            }

            string data = "";
            using (IO.StreamReader sr = new IO.StreamReader(OASystem.Properties.Settings.Default.SENDO1DATFilePath, Encoding.GetEncoding(932)))
            {
                data = sr.ReadToEnd();
            }

            //S群ごとに分けて文字列を格納しておく
            var Spattern = @"S\d{21}"; // S群
            var Smatches = Regex.Matches(data, Spattern);
            List<string> SList = new List<string>();
            foreach (Match Smatch in Smatches)
            {
                //var vanコード = Smatch.Value.Substring(13, 9);
                var NextSmatch = Smatch.NextMatch();
                var Sstartindex = Smatch.Index;
                var Snextstartindex = NextSmatch.Success ? NextSmatch.Index : data.Length;
                var dataS = data.Substring(Sstartindex, Snextstartindex - Sstartindex);
                SList.Add(dataS);
            }

            // 削除する伝票番号の医薬品ごとに処理

            List<string> removedSdata = new List<string>();

            foreach (var sdata in SList)
            {
                //1.03aで対応
                //VANコードが1から始まるもの(1XXXXXXXX)は含めない(削除する)
                var vanコード = sdata.Substring(13, 9);
                if (vanコード.StartsWith("1"))
                {
                    continue;
                }

                string tempS = sdata; // 編集用にコピー
                foreach (var ent in deleteList)
                {
                    var Dpattern = @"D\d{18,}"; // D群
                    //var Dpattern = @"D\d{18,}" + ent.レセ発注伝票No; // D群
                    var Dmatches = Regex.Matches(tempS, Dpattern);

                    var Epattern = @"E1[05]\d{12,}";
                    //var Epattern = @"E\d{24,}";
                    var Ematches = Regex.Matches(tempS, Epattern);
                    var EstartIndex = -1;
                    if (Ematches.Count == 1)
                    {
                        EstartIndex = Ematches[0].Index;
                    }
                    else
                    {
                        throw new Exception("SEND01.DAT内のS群に群終了を示すEコードが１つではありませんでした。。\r\n" + "該当医薬品JAN:" + ent.JANコード + "\r\n" + "該当伝票No:" + ent.レセ発注伝票No);
                    }


                    foreach (Match Dmatch in Dmatches)
                    {
                        var JANPattern = @"1" + ent.JANコード.Substring(0, 12); // MEDIS-DCのJANをセットしているので12桁一致させる
                        //var JANPattern = @"1" + ent.JANコード; //JANコード 医薬品は4987+xxxxxxxxx　で1がSEND用で頭につく 14桁

                        var NextDmatch = Dmatch.NextMatch();
                        var Dstartindex = Dmatch.Index;
                        var DorEnextstartindex = NextDmatch.Success ? NextDmatch.Index : EstartIndex;
                        var dataD = tempS.Substring(Dstartindex, DorEnextstartindex - Dstartindex);
                        var JANmatches = Regex.Matches(dataD, JANPattern);

                        //一致するものがあれば
                        if (JANmatches.Count != 0)
                        {
                            //そのDを削除
                            //削除を繰り返すとIndexがずれる為、一致を置換で消す 
                            tempS = tempS.Replace(dataD, "");
                            //tempS = tempS.Remove(Dstartindex, DorEnextstartindex - Dstartindex);
                        }
                    }
                }

                // D群が含まれていないS群はエラーになるので、含めない
                var Dpattern2 = @"D\d{18,}";// D群
                var Dmatches2 = Regex.Matches(tempS, Dpattern2);
                if (Dmatches2.Count == 0)
                {
                    continue;
                }


                removedSdata.Add(tempS);

                //S群の中から対象の伝票番号のJANのDを探す

                //Dのスタート～エンドを削除
                //複数ある場合は繰り返す
                //最後のEの場合もあるので気をつける
                //S群の内容が１つも無ければ、S群ごと削除する

                //S群がひとつもなければ、ファイルを全部クリア



            }

            string writedata = "";
            foreach (var dat in removedSdata)
            {
                writedata += dat;
            }

            using (IO.StreamWriter sw = new IO.StreamWriter(OASystem.Properties.Settings.Default.SENDO1DATFilePath, false, Encoding.GetEncoding(932)))
            {
                sw.Write(writedata);
                sw.Flush();
            }

            if (writedata == "")
            {
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
