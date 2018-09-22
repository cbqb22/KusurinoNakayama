using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;


namespace OASystem.ViewModel.Common.Validator
{
    public static class PreValidation
    {

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static void AllTextBoxFocusAndLostFocus(DependencyObject depObj,DependencyObject lastfocus)
        {
            var cc = FindVisualChildren<TextBox>(depObj);
            foreach (var c in cc)
            {
                TextBox p = null;

                if (c is TextBox)
                    p = c as TextBox;

                p.Focus();
            }

            var fe = lastfocus as FrameworkElement;
            fe.Focus();

        }

        public static void Validate(DependencyObject depObj)
        {
            foreach (var c in FindVisualChildren<FrameworkElement>(depObj))
            {
                DependencyProperty p = null;

                if (c is TextBlock)
                    p = TextBlock.TextProperty;
                else if (c is TextBox)
                    p = TextBox.TextProperty;

                if (p != null && c.GetBindingExpression(p) != null) c.GetBindingExpression(p).UpdateSource();
            }

        }
    }
}
