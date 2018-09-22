using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace クスリのナカヤマ薬局ツール.共通.Exception
{
    /// <summary>
    /// クスリのナカヤマ薬局ツール用の例外クラス
    /// </summary>
    public class ExtendException : System.Exception
    {
        public ExtendException(string ErrorArea,string ErrorMessage,string filePath,string Stacktrace) : base()
        {
            this.ErrorArea = ErrorArea;
            this.ErrorMessageExtend = ErrorMessage;
            this.StacktraceExtend = Stacktrace;
            this.FilePath = filePath;

        }

        /// <summary>
        /// エラーが発生した場所
        /// </summary>
        private string _ErrorArea;

        public string ErrorArea
        {
            get { return _ErrorArea; }
            set { _ErrorArea = value; }
        }

        /// <summary>
        /// エラーとなったファイルの場所
        /// </summary>
        private string _FilePath;

        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        /// <summary>
        /// 拡張のエラーメッセージ
        /// </summary>
        private string _ErrorMessageExtend;

        public string ErrorMessageExtend
        {
            get { return _ErrorMessageExtend; }
            set { _ErrorMessageExtend = value; }
        }

        /// <summary>
        /// 拡張のスタックトレース
        /// </summary>
        private string _StacktraceExtend;

        public string StacktraceExtend
        {
            get { return _StacktraceExtend; }
            set { _StacktraceExtend = value; }
        }


    }
}
