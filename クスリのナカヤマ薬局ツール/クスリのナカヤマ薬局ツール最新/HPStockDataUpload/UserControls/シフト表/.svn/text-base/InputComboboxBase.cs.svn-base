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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:クスリのナカヤマ薬局ツール.UserControls.シフト表"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:クスリのナカヤマ薬局ツール.UserControls.シフト表;assembly=クスリのナカヤマ薬局ツール.UserControls.シフト表"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:InputComboboxBase/>
    ///
    /// </summary>
    public class InputComboboxBase : Control, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        static InputComboboxBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputComboboxBase), new FrameworkPropertyMetadata(typeof(InputComboboxBase)));


        }



        #region Value 依存プロパティ


        public static readonly DependencyProperty CanvasWidthProperty = DependencyProperty.Register("CanvasWidth", typeof(double), typeof(InputComboboxBase));

        public double CanvasWidth
        {
            get { return (double)this.GetValue(CanvasWidthProperty); }
            set { this.SetValue(CanvasWidthProperty, value); }
        }


        public static readonly DependencyProperty CanvasHeightProperty = DependencyProperty.Register("CanvasHeight", typeof(double), typeof(InputComboboxBase));

        public double CanvasHeight
        {
            get { return (double)this.GetValue(CanvasHeightProperty); }
            set { this.SetValue(CanvasHeightProperty, value); }
        }

        public static readonly DependencyProperty TextBoxWidthProperty = DependencyProperty.Register("TextBoxWidth", typeof(double), typeof(InputComboboxBase));

        public double TextBoxWidth
        {
            get { return (double)this.GetValue(TextBoxWidthProperty); }
            set { this.SetValue(TextBoxWidthProperty, value); }
        }


        public static readonly DependencyProperty TextBoxHeightProperty = DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(InputComboboxBase));

        public double TextBoxHeight
        {
            get { return (double)this.GetValue(TextBoxHeightProperty); }
            set { this.SetValue(TextBoxHeightProperty, value); }
        }




        #endregion


        void InputComboboxBase_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!this.IsFirstFocus)
            {
                this.IsFirstFocus = true;
                this.button.Visibility = System.Windows.Visibility.Visible;
                    
            }
            else
            {
                this.IsKeyboardFocus = true;
            }


            var fe = sender as FrameworkElement;
            InputShiftCell isf = null;
            while(fe != null)
            {

               fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;

               if (fe != null)
               {
                   if(fe is InputShiftCell)
                   {
                       isf = ((InputShiftCell)fe) as InputShiftCell;
                       continue;
                   }

                   if (fe is ShiftTable)
                   {
                       if (isf != null)
                       {
                           ((ShiftTable)fe).CurrentFocus = isf;
                           break;
                       }
                   }
               }


            }

        }


        void InputComboboxBase_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.textBox != null && this.textBox.IsFocused)
            {
                return;
            }

            if (this.button != null && this.button.IsFocused)
            {
                return;
            }

            if (this.listbox != null && this.listbox.IsFocused)
            {
                return;
            }

            this.button.Visibility = System.Windows.Visibility.Collapsed;
            this.listbox.Visibility = System.Windows.Visibility.Collapsed;

            this.IsFirstFocus = false;
            this.IsKeyboardFocus = false;
            this.IsListBoxDisplay = false;
        }





        /// <summary>
        /// コントロールするアイテム
        /// </summary>
        protected TextBox textBox;

        protected Button button;

        protected ListBox listbox;




        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();



            this.LostFocus += new RoutedEventHandler(InputComboboxBase_LostFocus);
            this.GotFocus += new RoutedEventHandler(InputComboboxBase_GotFocus);

            button = (Button)GetTemplateChild("button");
            button.DataContext = this;
            button.Click += new RoutedEventHandler(button_Click);


            listbox = (ListBox)GetTemplateChild("listbox");
            listbox.DataContext = this;
            listbox.SelectionChanged += new SelectionChangedEventHandler(listbox_SelectionChanged);

            textBox = (TextBox)GetTemplateChild("textbox");
            textBox.DataContext = this;


            this.listbox.Visibility = System.Windows.Visibility.Collapsed;
            this.button.Visibility = System.Windows.Visibility.Collapsed;


        }



        void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb == null)
            {
                return;
            }

            //this.表示用文字列 = lb.SelectedItems[lb.SelectedIndex].ToString();
            this.IsListBoxDisplay = false;
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsListBoxDisplay)
            {
                this.IsListBoxDisplay = false;
                this.listbox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.IsListBoxDisplay = true;
                this.listbox.Visibility = System.Windows.Visibility.Visible;
            }
        }



        private string _表示用文字列;

        public string 表示用文字列
        {
            get { return _表示用文字列; }
            set
            {
                _表示用文字列 = value;
                OnPropertyChanged("表示用文字列");
            }
        }


        // ListBoxを表示しているかどうか
        private bool _IsListBoxDisplay;

        public bool IsListBoxDisplay
        {
            get { return _IsListBoxDisplay; }
            set
            {
                _IsListBoxDisplay = value;
                OnPropertyChanged("IsListBoxDisplay");
            }
        }


        // 一回目にフォーカスを得た時
        private bool _IsFirstFocus;

        public bool IsFirstFocus
        {
            get { return _IsFirstFocus; }
            set
            {
                _IsFirstFocus = value;
                OnPropertyChanged("IsFirstFocus");
            }
        }

        private bool _IsKeyboardFocus;

        public bool IsKeyboardFocus
        {
            get { return _IsKeyboardFocus; }
            set
            {
                _IsKeyboardFocus = value;
                OnPropertyChanged("IsKeyboardFocus");
            }
        }



        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }




    }
}
