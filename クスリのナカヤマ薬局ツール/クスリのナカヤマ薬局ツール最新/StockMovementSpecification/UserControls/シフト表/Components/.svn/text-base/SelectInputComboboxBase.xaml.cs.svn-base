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

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表.Components
{
    /// <summary>
    /// SelectInputComboboxBase.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectInputComboboxBase : UserControl,INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public SelectInputComboboxBase()
        {
            InitializeComponent();

            this.LostFocus += new RoutedEventHandler(SelectInputComboboxBase_LostFocus);
            this.GotFocus += new RoutedEventHandler(SelectInputComboboxBase_GotFocus);

            textbox.DataContext = this;
        }


        void SelectInputComboboxBase_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!this.IsFirstFocus)
            {
                this.IsFirstFocus = true;
            }
            else
            {
                this.IsKeyboardFocus = true;
            }

            System.Diagnostics.Debug.WriteLine("aaaa");

            

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


        void SelectInputComboboxBase_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsFirstFocus = false;
            this.IsKeyboardFocus = false;
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
