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
    /// RowHeaderWorkAssignment.xaml の相互作用ロジック
    /// </summary>
    public partial class RowHeaderWorkAssignment : UserControl
    {
        public RowHeaderWorkAssignment()
        {
            InitializeComponent();

            this.DataContext = this;

        }


        #region Value 依存プロパティ


        public static readonly DependencyProperty KubunProperty = DependencyProperty.Register("Kubun", typeof(string), typeof(RowHeaderWorkAssignment));

        public string Kubun
        {
            get { return (string)this.GetValue(KubunProperty); }
            set { this.SetValue(KubunProperty, value); }
        }


        public static readonly DependencyProperty GdBackgroundBrushProperty = DependencyProperty.Register("GdBackgroundBrush", typeof(Brush), typeof(RowHeaderWorkAssignment));

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
