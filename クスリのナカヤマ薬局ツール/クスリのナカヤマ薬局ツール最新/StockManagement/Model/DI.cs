using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockManagement.ViewModel.File;


namespace StockManagement.Model
{
    public static class DI
    {

        /// <summary>
        /// Version名
        /// </summary>
        private static string _VersionName;

        public static string VersionName
        {
            get { return DI._VersionName; }
            set { DI._VersionName = value; }
        }


    }
}
