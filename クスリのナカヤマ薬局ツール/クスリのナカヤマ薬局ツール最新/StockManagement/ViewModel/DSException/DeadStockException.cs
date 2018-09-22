using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManagement.ViewModel.DSException
{
    class DeadStockException : Exception
    {
        private bool _IsCancel;

        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; }
        }
        private string _DisplayMessage;

        public string DisplayMessage
        {
            get { return _DisplayMessage; }
            set { _DisplayMessage = value; }
        }

        public DeadStockException(bool iscancel, string dispmassage)
        {
            IsCancel = iscancel;
            DisplayMessage = dispmassage;
        }
    }
}
