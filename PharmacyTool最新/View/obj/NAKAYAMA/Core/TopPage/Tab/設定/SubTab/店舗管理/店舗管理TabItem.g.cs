﻿#pragma checksum "C:\Users\poohace\Desktop\PharmacyTool最新\View\Core\TopPage\Tab\設定\SubTab\店舗管理\店舗管理TabItem.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E58605198D439CF066175B56325F6D49"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
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
using View.Core.TopPage.Tab.設定.SubTab.店舗管理;


namespace View.Core.TopPage.Tab.設定.SubTab.店舗管理 {
    
    
    public partial class 店舗管理TabItem : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TabItem 新規店舗作成TabItem;
        
        internal View.Core.TopPage.Tab.設定.SubTab.店舗管理.新規店舗名作成 店舗管理;
        
        internal System.Windows.Controls.TabItem 店舗名削除TabItem;
        
        internal View.Core.TopPage.Tab.設定.SubTab.店舗管理.店舗名削除 uc店舗名削除;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/View;component/Core/TopPage/Tab/%E8%A8%AD%E5%AE%9A/SubTab/%E5%BA%97%E8%88%97%E7%" +
                        "AE%A1%E7%90%86/%E5%BA%97%E8%88%97%E7%AE%A1%E7%90%86TabItem.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.新規店舗作成TabItem = ((System.Windows.Controls.TabItem)(this.FindName("新規店舗作成TabItem")));
            this.店舗管理 = ((View.Core.TopPage.Tab.設定.SubTab.店舗管理.新規店舗名作成)(this.FindName("店舗管理")));
            this.店舗名削除TabItem = ((System.Windows.Controls.TabItem)(this.FindName("店舗名削除TabItem")));
            this.uc店舗名削除 = ((View.Core.TopPage.Tab.設定.SubTab.店舗管理.店舗名削除)(this.FindName("uc店舗名削除")));
        }
    }
}
