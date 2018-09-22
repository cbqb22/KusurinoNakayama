﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using Ex = Microsoft.Office.Interop.Excel;


namespace ExcelControllerOffice11
{
    public class ExcelControllerForReceiving
    {

        private Ex.Application controlledExcel = null;

        //public void CreatExcel()
        //{
        //    if (controlledExcel == null)
        //    {
        //        controlledExcel = new Ex.Application();
        //        controlledExcel.Visible = false;
        //        controlledExcel.DisplayAlerts = false;  // 確認メッセージはオフにする

        //        Ex.Workbook wb = controlledExcel.Workbooks.Add();

        //        // 1～3シートは削除してまっさらにしておく
        //        Ex.Worksheet ws = ((Ex.Worksheet)wb.Sheets[1]);
        //        ws.Delete();
        //        ws = ((Ex.Worksheet)wb.Sheets[1]);
        //        ws.Delete();

        //        Marshal.ReleaseComObject(ws);
        //        Marshal.ReleaseComObject(wb);

        //    }
        //    else
        //    {
        //        //MessageBox.Show("既に操作するExcelが存在します。");
        //    }
        //}
        public bool CreatExcel()
        {
            if (controlledExcel == null)
            {
                controlledExcel = new Ex.Application();
                controlledExcel.Visible = false;
                //controlledExcel.DisplayAlerts = false;  // 確認メッセージはオフにする

                Ex.Workbook wb = controlledExcel.Workbooks.Add();

                var shts = wb.Sheets;
                int count = shts.Count - 1;

                if (count < 0)
                {
                    return false;
                }

                for (int i = 0; i < count; i++)
                {
                    Ex.Worksheet ws = ((Ex.Worksheet)wb.Sheets[1]);
                    ws.Delete();
                    Marshal.ReleaseComObject(ws);
                }

                Marshal.ReleaseComObject(shts);
                Marshal.ReleaseComObject(wb);

            }
            else
            {
                return false;
            }

            return true;
        }

        public void AddSheet(int totalSheetCount)
        {
            Ex.Sheets shts = controlledExcel.ActiveWorkbook.Sheets;
            while (shts.Count != totalSheetCount)
            {
                shts.Add(Type.Missing, Type.Missing, 1, Type.Missing);
            }

            Marshal.ReleaseComObject(shts);

        }

        public void RenameSheet(string sheetName, int sheetNo)
        {

            Ex.Workbook wb = controlledExcel.ActiveWorkbook;
            Ex.Sheets shts = wb.Sheets;
            Ex.Worksheet ws = shts[sheetNo] as Ex.Worksheet;

            ws.Name = sheetName;

        }

        public void AddSheet(string sheetName, bool delete)
        {

            Ex.Workbook wb = controlledExcel.ActiveWorkbook;
            Ex.Sheets shts = wb.Sheets;

            shts.Add(Type.Missing, Type.Missing, 1, Type.Missing);

            if (delete)
            {
                Ex.Worksheet ws = wb.Sheets[1] as Ex.Worksheet;

                ws.Delete();
            }

            Ex.Worksheet ws2 = shts[1] as Ex.Worksheet;

            ws2.Name = sheetName;

            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(shts);
            Marshal.ReleaseComObject(ws2);



        }

        public void PrintOut(int SheetNo)
        {
            //プリンタ名の取得
            PrintDocument pd = new PrintDocument();
            string defaultPrinterName = pd.PrinterSettings.PrinterName;

            var book = controlledExcel.ActiveWorkbook as Ex.Workbook;

            var shts = book.Sheets as Ex.Sheets;

            foreach (Ex.Worksheet ws in shts)
            {
                ws.PrintOut(Type.Missing, Type.Missing, Type.Missing, Type.Missing, defaultPrinterName, Type.Missing, Type.Missing, Type.Missing);
                Marshal.ReleaseComObject(ws);
            }

            Marshal.ReleaseComObject(shts);
            Marshal.ReleaseComObject(book);
        }



        public void CloseExcel(bool withSave, string ExcelBookFileName)
        {

            Ex.Workbook wb = controlledExcel.ActiveWorkbook;


            // 全シートをA1選択状態にする
            SelectA1WithAllSheet();

            if (withSave)
            {
                // 2003=xlAddIn
                // 2007=xlOpenXMLWorkbook
                try
                {
                    wb.SaveAs(ExcelBookFileName, Ex.XlFileFormat.xlAddIn);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("HRESULT からの例外: 0x800A03EC") == false)
                    {
                        //MessageBox.Show("Excelの保存に失敗しました。\r\n再度操作を行ってください。\r\n" + ex.Message + ex.StackTrace);
                    }
                }
            }




            wb.Close(false);
            controlledExcel.Quit();


            // COMの解放　メモリーリークを防止
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(controlledExcel);

        }

        public void SaveExcel(string savePath)
        {
        }

        public void InsertTableData(int SheetNo,
                                    int No,
                                    string Name,
                                    double usedAmount,
                                    double deadAmount,
                                    DateTime expireDate,
                                    bool Acceptable)
        {
            // Workbook&Sheetの選択
            Ex.Worksheet ws = controlledExcel.ActiveWorkbook.Sheets[SheetNo] as Ex.Worksheet;
            ws.Select(Type.Missing);


            // 最終行を取得
            //int EndRow = ws.get_Range("A1", Type.Missing).get_End(Ex.XlDirection.xlDown).Row;

            ////シートの最終セルを選択します
            Ex.Range rgnSelect = ws.Cells as Ex.Range;
            Ex.Range rgnSelect_1 = rgnSelect.SpecialCells(Ex.XlCellType.xlCellTypeLastCell) as Ex.Range;
            rgnSelect_1.Select();
            ////最終行を表示しています
            int EndRow = controlledExcel.ActiveCell.Row;

            // テーブルヘッダーを(A7,B7,C7,D7)設定
            Ex.Range rgn1 = ws.Cells[EndRow + 1, 1] as Ex.Range;
            rgn1.Value2 = No.ToString();
            Ex.Range rgn2 = ws.Cells[EndRow + 1, 2] as Ex.Range;
            rgn2.Value2 = Name;
            Ex.Range rgn3 = ws.Cells[EndRow + 1, 3] as Ex.Range;
            rgn3.Value2 = usedAmount;
            Ex.Range rgn4 = ws.Cells[EndRow + 1, 4] as Ex.Range;
            rgn4.Value2 = deadAmount;
            Ex.Range rgn5 = ws.Cells[EndRow + 1, 5] as Ex.Range;
            rgn5.Value2 = expireDate.ToString("yyyy.MM");
            Ex.Range rgn6 = ws.Cells[EndRow + 1, 6] as Ex.Range;
            rgn6.Value2 = Acceptable ? "○" : "";

            // 薬品名は縮小して全体を表示する
            rgn2.ShrinkToFit = true;

            // デッド数量は太字
            Ex.Font rgn4f = rgn4.Font;
            rgn4f.Bold = true;


            // 罫線を引く
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn1.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn2.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn3.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn4.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn5.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn6.Borders);

            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(rgnSelect);
            Marshal.ReleaseComObject(rgnSelect_1);
            Marshal.ReleaseComObject(rgn1);
            Marshal.ReleaseComObject(rgn2);
            Marshal.ReleaseComObject(rgn3);
            Marshal.ReleaseComObject(rgn4);
            Marshal.ReleaseComObject(rgn5);
            Marshal.ReleaseComObject(rgn6);


        }


        public void SetBasicalForm(int SheetNo,
                                   DateTime datetime, string to, string from,
                                   DateTime opponentStartDate, DateTime opponentEndDate,
                                   DateTime ownStartDate)
        {
            // Workbook&Sheetの選択
            Ex.Worksheet ws = controlledExcel.ActiveWorkbook.Sheets[SheetNo] as Ex.Worksheet;
            ws.Select(Type.Missing);

            // フッターの設定(現在のページ/全ページ)
            ws.PageSetup.CenterFooter = "&P / &N";

            // 印刷タイトル(行の設定)
            ws.PageSetup.PrintTitleRows = @"$7:$7";

            // カラムの幅設定
            Ex.Range r1 = ws.Columns[1] as Ex.Range;
            r1.ColumnWidth = 4.38;
            Ex.Range r2 = ws.Columns[2] as Ex.Range;
            r2.ColumnWidth = 38.88;
            Ex.Range r3 = ws.Columns[3] as Ex.Range;
            r3.ColumnWidth = 10.13;
            Ex.Range r4 = ws.Columns[4] as Ex.Range;
            r4.ColumnWidth = 12.88;
            Ex.Range r5 = ws.Columns[5] as Ex.Range;
            r5.ColumnWidth = 10.13;
            Ex.Range r6 = ws.Columns[6] as Ex.Range;
            r6.ColumnWidth = 8.38;



            // 期限を小数点２桁表示 2012.1→2012.10への対応
            r5.NumberFormat = "0.00";


            // 日付を右上(F1)に設定
            Ex.Range rgn = ws.Cells[1, 6] as Ex.Range;
            rgn.Value2 = "本書類作成日時：" + datetime.ToString("yyyy/MM/dd HH:mm");
            Ex.Range rgn_1 = ws.get_Range("A1:F1", Type.Missing);
            rgn_1.MergeCells = true;
            rgn_1.HorizontalAlignment = Ex.Constants.xlRight;

            // 宛名を(A2)設定
            Ex.Range rgn2 = ws.Cells[2, 1] as Ex.Range;
            rgn2.Value2 = string.Format("{0} 御中", to);
            rgn2 = ws.get_Range("A2:F2", Type.Missing);
            rgn2.MergeCells = true;
            //rgn2.Font.Bold = true;
            Ex.Font f2 = rgn2.Font;
            f2.Size = 22;
            rgn2.HorizontalAlignment = Ex.Constants.xlCenter;
            f2.ColorIndex = 2;
            rgn2.Interior.Color = 1; // カラーコード 黒:000000

            // 差出店を(E3)設定
            Ex.Range rgn3 = ws.Cells[3, 5] as Ex.Range;
            rgn3.Value2 = string.Format("{0}より", from);
            Ex.Range rgn3_1 = ws.get_Range("E3:F3", Type.Missing);
            rgn3_1.MergeCells = true;
            rgn3_1.HorizontalAlignment = Ex.Constants.xlRight;


            // 差出店を(E4)設定
            Ex.Range rgn4 = ws.Cells[4, 5] as Ex.Range;
            rgn4.Value2 = string.Format("担当者：　　　　　", from);
            Ex.Range rgn4_1 = ws.get_Range("E4:F4", Type.Missing);
            rgn4_1.MergeCells = true;
            rgn4_1.HorizontalAlignment = Ex.Constants.xlRight;
            rgn4_1.Font.Underline = Ex.XlUnderlineStyle.xlUnderlineStyleSingle;

            // 概要を(AD5)設定
            Ex.Range rgn5 = ws.Cells[5, 1] as Ex.Range;
            rgn5.Value2 = string.Format("当店({0}～{1})の使用量に対する、貴店のデッド品で引き取り可能なリスト"
                                           , opponentStartDate.ToString("yyyy年MM月")
                                           , opponentEndDate.ToString("yyyy年MM月"));
            Ex.Range rgn5_1 = ws.get_Range("A5:F5", Type.Missing);
            rgn5_1.MergeCells = true;
            rgn5_1.HorizontalAlignment = Ex.Constants.xlLeft;



            // テーブルヘッダーを(A7,B7,C7,D7)設定
            Ex.Range rgn7_1 = ws.Cells[7, 1] as Ex.Range;
            rgn7_1.Value2 = "No";
            //Ex.Font rgn7_1f = rgn7_1.Font;
            //rgn7_1f.Bold = true;

            Ex.Range rgn7_2 = ws.Cells[7, 2] as Ex.Range;
            rgn7_2.Value2 = "薬品名";
            //Ex.Font rgn7_2f = rgn7_2.Font;
            //rgn7_2f.Bold = true;

            Ex.Range rgn7_3 = ws.Cells[7, 3] as Ex.Range;
            rgn7_3.Value2 = "当店使用量";
            //Ex.Font rgn7_3f = rgn7_3.Font;
            //rgn7_3f.Bold = true;


            Ex.Range rgn7_4 = ws.Cells[7, 4] as Ex.Range;
            rgn7_4.Value2 = "貴店デッド数量";
            Ex.Font rgn7_4f = rgn7_4.Font;
            rgn7_4f.Bold = true;

            Ex.Range rgn7_5 = ws.Cells[7, 5] as Ex.Range;
            rgn7_5.Value2 = "期限";
            //Ex.Font rgn7_5f = rgn7_5.Font;
            //rgn7_5f.Bold = true;

            Ex.Range rgn7_6 = ws.Cells[7, 6] as Ex.Range;
            rgn7_6.Value2 = "引取希望";
            //Ex.Font rgn7_6f = rgn7_6.Font;
            //rgn7_6f.Bold = true;


            // 罫線
            // Weight = Ex.XlBorderWeight
            // Borders[Ex.XlBordersIndex.xlEdgeLeft]　→　セルの左
            // Borders[Ex.XlBordersIndex.xlEdgeRight]　→　セルの右
            // Borders[Ex.XlBordersIndex.xlEdgeTop]　→　セルの上
            // Borders[Ex.XlBordersIndex.xlEdgeBottom]　→　セルの下
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn7_1.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn7_2.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn7_3.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn7_4.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn7_5.Borders);
            SetRuledLine(Ex.XlBorderWeight.xlThin, Ex.XlLineStyle.xlContinuous, rgn7_6.Borders);

        }

        public void SetRuledLine(Ex.XlBorderWeight weight, Ex.XlLineStyle lineStyle, Ex.Borders borders)
        {
            Ex.Border borderLeft = borders[Ex.XlBordersIndex.xlEdgeLeft];
            borderLeft.Weight = weight;
            borderLeft.LineStyle = lineStyle;

            Ex.Border borderTop = borders[Ex.XlBordersIndex.xlEdgeTop];
            borderTop.Weight = weight;
            borderTop.LineStyle = lineStyle;

            Ex.Border borderRight = borders[Ex.XlBordersIndex.xlEdgeRight];
            borderRight.Weight = weight;
            borderRight.LineStyle = lineStyle;

            Ex.Border borderBottom = borders[Ex.XlBordersIndex.xlEdgeBottom];
            borderBottom.Weight = weight;
            borderBottom.LineStyle = lineStyle;
        }

        public void SelectA1WithAllSheet()
        {
            if (controlledExcel == null)
            {
                throw new Exception("Excelが初期化されていません。");
            }

            Ex.Workbook wb = controlledExcel.ActiveWorkbook;
            Ex.Sheets wss = wb.Sheets;

            foreach (Ex.Worksheet ws in wss)
            {
                ws.Select(Type.Missing);
                var rgn = ws.get_Range("A1") as Ex.Range;

                rgn.Select();
                Marshal.ReleaseComObject(rgn);

            }

            // １シート目を選択する
            Ex.Worksheet ws2 = wss[1] as Ex.Worksheet;
            ws2.Select(Type.Missing);

            Marshal.ReleaseComObject(ws2);
            Marshal.ReleaseComObject(wss);
            Marshal.ReleaseComObject(wb);

        }

    }

}
