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

namespace クスリのナカヤマ薬局ツール.UserControls.作業割り当て表
{
    /// <summary>
    /// ColumnHeaderWorkAssignment.xaml の相互作用ロジック
    /// </summary>
    public partial class ColumnHeaderWorkAssignment : UserControl
    {
        public ColumnHeaderWorkAssignment()
        {
            InitializeComponent();

            this.DataContext = this;

        }


        #region Value 依存プロパティ


        public static readonly DependencyProperty KubunPersonCountProperty = DependencyProperty.Register("KubunPersonCount", typeof(string), typeof(ColumnHeaderWorkAssignment));

        public string KubunPersonCount
        {
            get { return (string)this.GetValue(KubunPersonCountProperty); }
            set { this.SetValue(KubunPersonCountProperty, value); }
        }


        public static readonly DependencyProperty GdBackgroundBrushProperty = DependencyProperty.Register("GdBackgroundBrush", typeof(Brush), typeof(ColumnHeaderWorkAssignment));

        /// <summary>
        /// RootGridのバックグラウンド
        /// </summary>
        public Brush GdBackgroundBrush
        {
            get { return (Brush)this.GetValue(GdBackgroundBrushProperty); }
            set { this.SetValue(GdBackgroundBrushProperty, value); }
        }



        #endregion





    }
}
