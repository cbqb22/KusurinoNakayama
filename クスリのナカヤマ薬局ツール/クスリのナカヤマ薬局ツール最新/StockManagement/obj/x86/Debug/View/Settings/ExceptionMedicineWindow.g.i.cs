﻿#pragma checksum "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BB657AFC47B37958E27538448038D77D"
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


namespace StockManagement.View.Settings {
    
    
    /// <summary>
    /// ExceptionMedicineWindow
    /// </summary>
    public partial class ExceptionMedicineWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 109 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvExceptionMedicine;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExceptionMedicineAddDelete;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExceptionMedicineAddAdd;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
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
            System.Uri resourceLocater = new System.Uri("/デッド品管理ツール;component/view/settings/exceptionmedicinewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.lvExceptionMedicine = ((System.Windows.Controls.ListView)(target));
            return;
            case 2:
            this.btnExceptionMedicineAddDelete = ((System.Windows.Controls.Button)(target));
            
            #line 122 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
            this.btnExceptionMedicineAddDelete.Click += new System.Windows.RoutedEventHandler(this.btnExceptionMedicineAddDelete_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnExceptionMedicineAddAdd = ((System.Windows.Controls.Button)(target));
            
            #line 123 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
            this.btnExceptionMedicineAddAdd.Click += new System.Windows.RoutedEventHandler(this.btnExceptionMedicineAdd_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 124 "..\..\..\..\..\View\Settings\ExceptionMedicineWindow.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

