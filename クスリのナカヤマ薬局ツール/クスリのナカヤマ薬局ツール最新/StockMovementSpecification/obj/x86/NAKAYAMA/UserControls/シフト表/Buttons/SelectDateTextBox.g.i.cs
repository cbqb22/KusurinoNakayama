﻿#pragma checksum "..\..\..\..\..\..\UserControls\シフト表\Buttons\SelectDateTextBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0F4C971C7FA62B2AF668D977454A74C9"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.18444
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using クスリのナカヤマ薬局ツール.UserControls.Calendaer;
using クスリのナカヤマ薬局ツール.UserControls.シフト表;


namespace クスリのナカヤマ薬局ツール.UserControls.シフト表.Buttons {
    
    
    /// <summary>
    /// SelectDateTextBox
    /// </summary>
    public partial class SelectDateTextBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\..\..\UserControls\シフト表\Buttons\SelectDateTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal クスリのナカヤマ薬局ツール.UserControls.Calendaer.DateUpDown 年月日DateUpDown;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\..\UserControls\シフト表\Buttons\SelectDateTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup 年月日カレンダポップアップ;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\..\UserControls\シフト表\Buttons\SelectDateTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal クスリのナカヤマ薬局ツール.UserControls.Calendaer.MonthCalendar 年月日カレンダ;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/在庫HP更新ツール;component/usercontrols/%e3%82%b7%e3%83%95%e3%83%88%e8%a1%a8/buttons/se" +
                    "lectdatetextbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\UserControls\シフト表\Buttons\SelectDateTextBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.年月日DateUpDown = ((クスリのナカヤマ薬局ツール.UserControls.Calendaer.DateUpDown)(target));
            return;
            case 2:
            
            #line 15 "..\..\..\..\..\..\UserControls\シフト表\Buttons\SelectDateTextBox.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.年月日カレンダ表示クリック);
            
            #line default
            #line hidden
            return;
            case 3:
            this.年月日カレンダポップアップ = ((System.Windows.Controls.Primitives.Popup)(target));
            return;
            case 4:
            this.年月日カレンダ = ((クスリのナカヤマ薬局ツール.UserControls.Calendaer.MonthCalendar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

