using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace View.Util.Common
{
    /// <summary>
    /// ドキュメントクラス用のNotifyPropertyChangedイベントを実装したベースクラス
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        #region public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// プロパティ変更通知イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// プロパティ変更通知イベント起動用
        /// </summary>
        /// <param name="propertyName"></param>
        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private object InvokeVoid(object arg)
        {
            PropertyChanged(this, new PropertyChangedEventArgs((string)arg));
            return null;
        }

        #endregion

    }
}
