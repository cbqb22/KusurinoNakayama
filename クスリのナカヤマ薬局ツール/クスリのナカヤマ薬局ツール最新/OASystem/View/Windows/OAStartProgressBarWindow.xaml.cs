using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;


namespace OASystem.View.Windows
{
    /// <summary>
    /// OAStartProgressBarWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class OAStartProgressBarWindow : Window
    {
        public OAStartProgressBarWindow()
        {
            InitializeComponent();
        }


        //Create a Delegate that matches 
        //the Signature of the ProgressBar's SetValue method
        private delegate void UpdateProgressBarDelegate(
                System.Windows.DependencyProperty dp, Object value);



        /// <summary>
        /// 起動前のプログレスバーをUIを更新する
        /// </summary>
        /// <param name="pbw"></param>
        /// <param name="value"></param>
        /// <param name="operationtext"></param>
        public void SetProgressBarValueAndText(double value, string operationtext)
        {

            if (string.IsNullOrEmpty(operationtext) == false)
            {
                oaspb.tbOperationIndicator.Text = operationtext;
            }

            oaspb.tbProgressPercentage.Text = value.ToString();
            oaspb.pbOAStarting.Value = value;
            oaspb.pbOAStarting.Refresh(); //UI更新
            Thread.Sleep(100);

        }


        /// <summary>
        /// 起動前のプログレスバーをUIを更新する
        /// メッセージ変更なし
        /// </summary>
        /// <param name="pbw"></param>
        /// <param name="value"></param>
        /// <param name="operationtext"></param>
        public void SetProgressBarValueAndText(double value)
        {
            oaspb.tbProgressPercentage.Text = value.ToString();
            oaspb.pbOAStarting.Value = value;
            oaspb.pbOAStarting.Refresh(); //UI更新
            Thread.Sleep(100);
        }


    }

    /// <summary>
    /// 拡張メソッド
    /// </summary>
    public static class ExtensionMethods
    {

        /// <summary>
        /// UIを更新する
        /// </summary>
        private static Action<UIElement> EmptyDelegate = delegate(UIElement ui) { ui.UpdateLayout(); };
        //private static Action<UIElement> EmptyDelegate = delegate() {}; もともとはこっち。こっちでも正常に動いていた。
        public static void Refresh(this UIElement uiElement)
        {

            //uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
            uiElement.Dispatcher.Invoke(DispatcherPriority.Input, EmptyDelegate,uiElement);
        }
    }




}
