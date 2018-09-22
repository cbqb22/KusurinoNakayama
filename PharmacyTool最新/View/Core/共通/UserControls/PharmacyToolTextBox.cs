using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace View.Core.共通.UserControls
{
    public class PharmacyToolTextBox : TextBox
    {
        public PharmacyToolTextBox()
            : base()
        {
            this.GotFocus += new RoutedEventHandler(PharmacyToolTextBox_GotFocus);
        }

        /// <summary>
        /// XPのバグ対策
        /// IME制御がOffになった場合の対処
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PharmacyToolTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PharmacyToolTextBox ptb = sender as PharmacyToolTextBox;
            if (ptb == null)
            {
                return;
            }

            bool IME制御が有効か = InputMethod.GetIsInputMethodEnabled(ptb);
            if (!IME制御が有効か)
            {
                InputMethod.SetIsInputMethodEnabled(ptb, true);
            }
        }
    }
}
