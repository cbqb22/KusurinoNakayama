﻿#pragma checksum "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2CBE0BAAD08180E979016F1975424F8B043C5568"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
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


namespace StockManagement.View.ProgressBar {
    
    
    /// <summary>
    /// CompletedCountProgress
    /// </summary>
    public partial class CompletedCountProgress : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar pbCompletedCount;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbOperationIndicator;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spPercentage;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbUpper;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbPercentageMark;
        
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
            System.Uri resourceLocater = new System.Uri("/デッド品管理ツール;component/view/progressbar/completedcountprogress.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml"
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
            this.pbCompletedCount = ((System.Windows.Controls.ProgressBar)(target));
            
            #line 10 "..\..\..\..\..\View\ProgressBar\CompletedCountProgress.xaml"
            this.pbCompletedCount.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.pbCompletedCount_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbOperationIndicator = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.spPercentage = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.tbUpper = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.tbPercentageMark = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

