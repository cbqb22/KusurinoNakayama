﻿#pragma checksum "C:\Users\poohace\Desktop\PharmacyTool最新 - コピー\View\Core\TopPage\Tab\在庫管理\現在庫DataGrid.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "34515F97CD99E1AFBB9AECF5F8A4C921"
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


namespace View.Core.TopPage.Tab.在庫管理 {
    
    
    public partial class 現在庫DataGrid : System.Windows.Controls.UserControl {
        
        internal System.Windows.DataTemplate Template店名;
        
        internal System.Windows.DataTemplate Template個別医薬品コード;
        
        internal System.Windows.DataTemplate Template医薬品名;
        
        internal System.Windows.DataTemplate Template在庫数;
        
        internal System.Windows.DataTemplate Template使用期限;
        
        internal System.Windows.DataTemplate Template薬価;
        
        internal System.Windows.DataTemplate Templateメーカー;
        
        internal System.Windows.DataTemplate Template最終更新日時;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DataGrid name現在庫DataGrid;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/View;component/Core/TopPage/Tab/%E5%9C%A8%E5%BA%AB%E7%AE%A1%E7%90%86/%E7%8F%BE%E" +
                        "5%9C%A8%E5%BA%ABDataGrid.xaml", System.UriKind.Relative));
            this.Template店名 = ((System.Windows.DataTemplate)(this.FindName("Template店名")));
            this.Template個別医薬品コード = ((System.Windows.DataTemplate)(this.FindName("Template個別医薬品コード")));
            this.Template医薬品名 = ((System.Windows.DataTemplate)(this.FindName("Template医薬品名")));
            this.Template在庫数 = ((System.Windows.DataTemplate)(this.FindName("Template在庫数")));
            this.Template使用期限 = ((System.Windows.DataTemplate)(this.FindName("Template使用期限")));
            this.Template薬価 = ((System.Windows.DataTemplate)(this.FindName("Template薬価")));
            this.Templateメーカー = ((System.Windows.DataTemplate)(this.FindName("Templateメーカー")));
            this.Template最終更新日時 = ((System.Windows.DataTemplate)(this.FindName("Template最終更新日時")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.name現在庫DataGrid = ((System.Windows.Controls.DataGrid)(this.FindName("name現在庫DataGrid")));
        }
    }
}
