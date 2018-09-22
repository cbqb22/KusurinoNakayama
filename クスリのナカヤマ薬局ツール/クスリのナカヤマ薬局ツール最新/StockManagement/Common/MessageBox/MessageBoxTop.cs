using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using StockManagement.View;

namespace StockManagement.ViewModel.Common.MessageBox
{
    /// <summary>
    /// 常に最前面に結果表示するMessageBox
    /// </summary>
    public static class MessageBoxTop
    {
        // 非同期処理がある場合はStaticイニシャライザを使うとエラーになる可能性がある為、このプロジェクトでははずしている。
        //static MessageBoxTop()
        //{
        //    if (App.Current.MainWindow == null)
        //    {
        //        throw new Exception("MessageBoxTopはMainWindowを初期化してから使用してください。\r\n先に使用すると、MainWindow扱いされてしまいます。");
        //    }
        //}

        public static void Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                mbt.Show();
                mbt.Topmost = true;
                System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button);
                mbt.Topmost = false;
                mbt.Close();

            }
            else
            {
                Action action = delegate()
                {
                    MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                    mbt.Show();
                    mbt.Topmost = true;
                    System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button);
                    mbt.Topmost = false;
                    mbt.Close();
                };

                dispatcher.Invoke(action);
            }


        }



        public static MessageBoxResult? Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBoxResult? ret = null;
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                mbt.Show();
                mbt.Topmost = true;
                MessageBoxResult result = System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button, icon);
                mbt.Topmost = false;
                mbt.Close();

                ret = result;
            }
            else
            {
                Action action = delegate()
                {
                    MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                    mbt.Show();
                    mbt.Topmost = true;
                    MessageBoxResult result = System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button, icon);
                    mbt.Topmost = false;
                    mbt.Close();

                    ret = result;

                };

                dispatcher.Invoke(action);



            }

            return ret;


        }


        public static MessageBoxResult? Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon,MessageBoxResult res)
        {

            MessageBoxResult? ret = null;
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                mbt.Show();
                mbt.Topmost = true;
                var result = System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button, icon, res);
                mbt.Topmost = false;
                mbt.Close();

                ret = result;
            }
            else
            {
                Action action = delegate()
                {
                    MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                    mbt.Show();
                    mbt.Topmost = true;
                    var result = System.Windows.MessageBox.Show(mbt, messageBoxText, caption, button, icon, res);
                    mbt.Topmost = false;
                    mbt.Close();

                    ret = result;

                };

                dispatcher.Invoke(action);



            }

            return ret;


        }


        public static void Show(string messageBoxText)
        {



            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                mbt.Show();
                mbt.Topmost = true;
                System.Windows.MessageBox.Show(mbt, messageBoxText);
                mbt.Topmost = false;
                mbt.Close();

            }
            else
            {
                Action action = delegate()
                {
                    MessageBoxTopWindow mbt = new MessageBoxTopWindow();
                    mbt.Show();
                    mbt.Topmost = true;
                    System.Windows.MessageBox.Show(mbt, messageBoxText);
                    mbt.Topmost = false;
                    mbt.Close();
                };

                dispatcher.Invoke(action);
            }



        }

    }
}
