using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MCSystem.View.Enum;

namespace MCSystem.Model
{
    public class OriginalMacroDetailEntity
    {
        private MacroOperationEnum _OperationEnum;
        public MacroOperationEnum OperationEnum
        {
            get { return _OperationEnum; }
            set { _OperationEnum = value; }
        }

        private Rect _操作座標;

        public Rect 操作座標
        {
            get { return _操作座標; }
            set { _操作座標 = value; }
        }

        private DragMeasure _Drag座標;

        public DragMeasure Drag座標
        {
            get { return _Drag座標; }
            set { _Drag座標 = value; }
        }

        private int _InputDataColumnNumber;
        public int InputDataColumnNumber
        {
            get { return _InputDataColumnNumber; }
            set { _InputDataColumnNumber = value; }
        }

        private string _入力データ;

        public string 入力データ
        {
            get { return _入力データ; }
            set { _入力データ = value; }
        }

        //不一致時の操作か
        private bool _IsUnMatchOperation;

        public bool IsUnMatchOperation
        {
            get { return _IsUnMatchOperation; }
            set { _IsUnMatchOperation = value; }
        }

        private int _待機時間;

        public int 待機時間
        {
            get { return _待機時間; }
            set { _待機時間 = value; }
        }

    }
}
