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
using クスリのナカヤマ薬局ツール.共通;
using クスリのナカヤマ薬局ツール.UserControls.Calendaer;

namespace クスリのナカヤマ薬局ツール.UserControls.シフト表.Buttons
{
    /// <summary>
    /// SelectDateTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectDateTextBox : UserControl    {
        public SelectDateTextBox()
        {
            InitializeComponent();
        }

        //メンバ
        #region public DateTime SelectedDate



        /// <summary>
        /// 選択日
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(SelectDateTextBox), new FrameworkPropertyMetadata(NakayamaConst.NAKAYAMA_DATE_MAX));
        ///// <summary>
        ///// 選択日
        ///// </summary>
        public DateTime SelectedDate
        {
            get
            {
                return (DateTime)this.GetValue(SelectedDateProperty);
            }
            set
            {
                try
                {
                    this.SetValue(SelectedDateProperty, value);
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion

        #region public DateTime Maximum/Minimum
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("MaximumDate", typeof(DateTime), typeof(SelectDateTextBox), new PropertyMetadata(NakayamaConst.NAKAYAMA_DATE_MAX));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("MinimumDate", typeof(DateTime), typeof(SelectDateTextBox), new PropertyMetadata(NakayamaConst.NAKAYAMA_DATE_MIN));

        public DateTime Maximum
        {
            get
            {
                return (DateTime)this.GetValue(MaximumProperty);
            }
            set
            {
                if (value < NakayamaConst.NAKAYAMA_DATE_MIN)
                {
                    return;
                }
                if (value > NakayamaConst.NAKAYAMA_DATE_MAX)
                {
                    return;
                }
                this.SetValue(MaximumProperty, value);
                this.年月日DateUpDown.Maximum = value;
                if (value < this.Minimum)
                {
                    this.Minimum = value;
                }
                if (value < this.SelectedDate)
                {
                    this.SelectedDate = value;
                }
            }
        }

        public DateTime Minimum
        {
            get
            {
                return (DateTime)this.GetValue(MinimumProperty);
            }
            set
            {
                if (value < NakayamaConst.NAKAYAMA_DATE_MIN)
                {
                    return;
                }
                if (value > NakayamaConst.NAKAYAMA_DATE_MAX)
                {
                    return;
                }
                this.SetValue(MinimumProperty, value);
                this.年月日DateUpDown.Minimum = value;
                if (this.Maximum < value)
                {
                    this.Maximum = value;
                }
                if (this.SelectedDate < value)
                {
                    this.SelectedDate = value;
                }
            }
        }
        #endregion



        //イベント処理
        #region private void 開始日カレンダ表示クリック(object sender, RoutedEventArgs e)
        /// <summary>
        /// 開始日カレンダを表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 年月日カレンダ表示クリック(object sender, RoutedEventArgs e)
        {
            this.年月日カレンダ.SelectedMonth = this.SelectedDate;
            this.年月日カレンダ.SelectedDate = this.SelectedDate;
            this.年月日カレンダポップアップ.IsOpen = true;
        }

        #endregion
        #region private void 開始日カレンダ選択(object sender, DateSelectedEventArgs e)
        /// <summary>
        /// 出産予定日のカレンダを選択した後処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 年月日カレンダ選択(object sender, DateSelectedEventArgs e)
        {
            if (e.SelectedDate < this.Minimum)
            {
                this.SelectedDate = this.Minimum;
            }
            else if (this.Maximum < e.SelectedDate)
            {
                this.SelectedDate = NakayamaConst.NAKAYAMA_DATE_MAX;
            }
            else
            {
                this.SelectedDate = e.SelectedDate;
            }
            this.年月日カレンダポップアップ.IsOpen = false;
        }

        #endregion
        #region protected override void OnGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        /// <summary>
        /// フォーカスが当たったらImeOffにする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            InputMethod im = InputMethod.Current;
            if (im.ImeState == InputMethodState.On)
            {
                im.ImeState = InputMethodState.Off;
            }

            base.OnGotKeyboardFocus(e);
        }
        #endregion


 
    }


}