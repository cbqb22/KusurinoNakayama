﻿#pragma checksum "C:\Users\poohace\Desktop\PharmacyTool最新 - コピー\View\Core\TopPage\Tab\設定\SubTab\SubTabItemControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DCE6862EA7FF40AAF640CDDFF5A5FE07"
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
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace View.Core.TopPage.Tab.設定.SubTab {
    
    
    public partial class SubTabItemControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TabItem tiユーザー管理;
        
        internal System.Windows.Controls.TabItem ti店舗管理;
        
        internal System.Windows.Controls.TabItem tiその他;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/View;component/Core/TopPage/Tab/%E8%A8%AD%E5%AE%9A/SubTab/SubTabItemControl.xaml" +
                        "", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tiユーザー管理 = ((System.Windows.Controls.TabItem)(this.FindName("tiユーザー管理")));
            this.ti店舗管理 = ((System.Windows.Controls.TabItem)(this.FindName("ti店舗管理")));
            this.tiその他 = ((System.Windows.Controls.TabItem)(this.FindName("tiその他")));
        }
    }
}
