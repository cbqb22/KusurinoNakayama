using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using MCSystem.Model;
using MCSystem.View.Enum;

namespace MCSystem.ViewModel
{
    public static class ControlOriginalMacro
    {

        public static OriginalMacroEntity LoadOriginalMacroFromFile(string filepath)
        {
            OriginalMacroEntity ent = new OriginalMacroEntity();
            using (StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding(932)))
            {
                string line = "";
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    if (counter == 1)
                    {
                        ent.データファイルパス = line;
                        continue;
                    }

                    OriginalMacroDetailEntity detail = new OriginalMacroDetailEntity();

                    var sepa = line.Split(',');

                    int macroEnum;
                    if (int.TryParse(sepa[0], out macroEnum) == false)
                    {
                        throw new Exception("Enumが変換できません。");
                    }

                    int IsUnmatchoperation;
                    if (int.TryParse(sepa[1], out IsUnmatchoperation) == false)
                    {
                        throw new Exception("IsUnmatchOperationが変換できません。");
                    }

                    int waittime;
                    if (int.TryParse(sepa[2], out waittime) == false)
                    {
                        throw new Exception("waittimeが変換できません。");
                    }

                    double x;
                    if (double.TryParse(sepa[3], out x) == false)
                    {
                        throw new Exception("X座標が変換できません。");
                    }

                    double y;
                    if (double.TryParse(sepa[4], out y) == false)
                    {
                        throw new Exception("Y座標が変換できません。");
                    }

                    double width;
                    if (double.TryParse(sepa[5], out width) == false)
                    {
                        throw new Exception("Widthが変換できません。");
                    }

                    double height;
                    if (double.TryParse(sepa[6], out height) == false)
                    {
                        throw new Exception("Heightが変換できません。");
                    }


                    int datacolumnnum;
                    if (int.TryParse(sepa[7], out datacolumnnum) == false)
                    {
                        throw new Exception("データ列が変換できません。");
                    }




                    detail.OperationEnum = (MacroOperationEnum)macroEnum;
                    detail.IsUnMatchOperation = IsUnmatchoperation == 1 ? true : false;


                    if (detail.OperationEnum == MacroOperationEnum.Drag || 
                        detail.OperationEnum == MacroOperationEnum.DragFast)
                    {
                        detail.Drag座標 = new DragMeasure(x, y, width, height);
                    }
                    else
                    {
                        detail.操作座標 = new System.Windows.Rect(x, y, width, height);
                    }
                    detail.InputDataColumnNumber = datacolumnnum;
                    detail.入力データ = sepa[8];
                    detail.待機時間 = waittime;

                    if (ent.ListDetail == null)
                    {
                        ent.ListDetail = new List<OriginalMacroDetailEntity>();
                    }

                    ent.ListDetail.Add(detail);


                }
            }

            return ent;
        }


        public static void WriteSettings(OriginalMacroEntity omEnt,string 保存先)
        {
            var path = string.Format(@"{0}\Macro{1}.csv",保存先,DateTime.Now.ToString("yyyyMMddHHmmss"));


            using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding(932)))
            {
                sw.WriteLine(omEnt.データファイルパス);

                foreach (var data in omEnt.ListDetail)
                {
                    if (data.OperationEnum == MacroOperationEnum.Click)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", (int)data.OperationEnum,data.IsUnMatchOperation ? 1 : 0, 0 ,(int)data.操作座標.X, (int)data.操作座標.Y,0,0, data.InputDataColumnNumber,""));
                    }
                    else if (data.OperationEnum == MacroOperationEnum.Drag || data.OperationEnum == MacroOperationEnum.DragFast)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", (int)data.OperationEnum, data.IsUnMatchOperation ? 1 : 0, 0, (int)data.Drag座標.StartX, (int)data.Drag座標.StartY, data.Drag座標.EndX, data.Drag座標.EndY, data.InputDataColumnNumber, ""));
                    }
                    else if (data.OperationEnum == MacroOperationEnum.ImageMatch)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", (int)data.OperationEnum, data.IsUnMatchOperation ? 1 : 0, 0, (int)data.操作座標.X, (int)data.操作座標.Y, data.操作座標.Width, data.操作座標.Height, data.InputDataColumnNumber, ""));
                    }
                    else
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", (int)data.OperationEnum, data.IsUnMatchOperation ? 1 : 0, data.待機時間, (int)data.操作座標.X, (int)data.操作座標.Y, 0, 0, data.InputDataColumnNumber, data.OperationEnum == MacroOperationEnum.Input || data.OperationEnum == MacroOperationEnum.ScreenShot ? data.入力データ : ""));
                    }
                }

                sw.Flush();
            }
        }
    }
}
