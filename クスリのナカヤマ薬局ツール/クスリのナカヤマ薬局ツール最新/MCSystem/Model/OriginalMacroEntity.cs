using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MCSystem.View.Enum;

namespace MCSystem.Model
{
    public class OriginalMacroEntity
    {
        private List<OriginalMacroDetailEntity> _ListDetail;
        public List<OriginalMacroDetailEntity> ListDetail
        {
            get { return _ListDetail; }
            set { _ListDetail = value; }
        }

        private string _データファイルパス;

        public string データファイルパス
        {
            get { return _データファイルパス; }
            set { _データファイルパス = value; }
        }

    }
}
