using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MCSystem.View.Windows;

namespace MCSystem.ViewModel.Common.MessageBox
{
    /// <summary>
    /// 常に最前面に結果表示するMessageBox
    /// </summary>
    public static class MessageBoxTop
    {
        static MessageBoxTop()
        {
            if (App.Current.MainWindow == null)
            {
                throw new Exception("MessageBoxTopはMainWindowを初期化してから使用してください。\r\n先に使用すると、MainWindow扱いされてしまいます。");
            }

        }

        public static void Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageBoxTopWindow mbt = new MessageBoxTopWindow();
            mbt.Show();
            mbt.Topmost = true;
            System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button);
            mbt.Topmost = false;
            mbt.Close();

        }





        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBoxTopWindow mbt = new MessageBoxTopWindow();
            mbt.Show();
            mbt.Topmost = true;
            var result = System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button, icon);
            mbt.Topmost = false;
            mbt.Close();

            return result;

        }

        public static void Show(string messageBoxText)
        {

            MessageBoxTopWindow mbt = new MessageBoxTopWindow();
            mbt.Show();
            mbt.Topmost = true;
            System.Windows.MessageBox.Show(mbt, messageBoxText);
            mbt.Topmost = false;
            mbt.Close();

        }

    }
}
