using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace クスリのナカヤマ薬局ツール.Model
{
    public static class DI
    {
   /// <summary>
        /// Version名
        /// </summary>
        private static string _在庫HP更新ツールVersionName;

        public static string 在庫HP更新ツールVersionName
        {
            get { return DI._在庫HP更新ツールVersionName; }
            set { DI._在庫HP更新ツールVersionName = value; }
        }

    }
}
     