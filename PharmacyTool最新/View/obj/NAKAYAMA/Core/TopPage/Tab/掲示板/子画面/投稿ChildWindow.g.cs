﻿#pragma checksum "C:\Users\poohace\Desktop\PharmacyTool最新\View\Core\TopPage\Tab\掲示板\子画面\投稿ChildWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "980895A1DEE46E71DCA4FF2F76B3BDBC"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
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
using View.Core.共通.UserControls;


namespace View.Core.TopPage.Tab.掲示板.子画面 {
    
    
    public partial class 投稿ChildWindow : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.ChildWindow cw入力画面;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid gdTopTitle;
        
        internal System.Windows.Controls.Grid gdInnerTopLeft;
        
        internal System.Windows.Controls.Grid gdAsterisk;
        
        internal System.Windows.Controls.Grid gdTag;
        
        internal System.Windows.Controls.StackPanel stpTag;
        
        internal System.Windows.Controls.TextBlock tbMainTitle;
        
        internal System.Windows.Controls.TextBlock tbSubTitle;
        
        internal System.Windows.Controls.TextBlock tbHomepageUrl;
        
        internal System.Windows.Controls.TextBlock tbEmail;
        
        internal System.Windows.Controls.TextBlock tb添付File;
        
        internal System.Windows.Controls.TextBlock tbl暗証キー;
        
        internal System.Windows.Controls.Grid gdInnerTopRight;
        
        internal View.Core.共通.UserControls.PharmacyToolTextBox tbInputTitle;
        
        internal View.Core.共通.UserControls.PharmacyToolTextBox tbInputPerson;
        
        internal View.Core.共通.UserControls.PharmacyToolTextBox tbInputHomepageUrl;
        
        internal View.Core.共通.UserControls.PharmacyToolTextBox tbInputEmail;
        
        internal System.Windows.Controls.StackPanel stp添付File;
        
        internal System.Windows.Controls.Button bt添付File;
        
        internal System.Windows.Controls.ComboBox cmb添付Files;
        
        internal System.Windows.Controls.Button bt添付削除;
        
        internal System.Windows.Controls.StackPanel stp暗証キー;
        
        internal System.Windows.Controls.PasswordBox pb暗証キー;
        
        internal System.Windows.Controls.TextBlock tbl暗証キーヒント;
        
        internal System.Windows.Controls.Grid gdMainCenter;
        
        internal Microsoft.Windows.Controls.WatermarkedTextBox wtbInputArticle;
        
        internal System.Windows.Controls.Grid gdLine;
        
        internal System.Windows.Controls.ComboBox cmbSelectColors;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/View;component/Core/TopPage/Tab/%E6%8E%B2%E7%A4%BA%E6%9D%BF/%E5%AD%90%E7%94%BB%E" +
                        "9%9D%A2/%E6%8A%95%E7%A8%BFChildWindow.xaml", System.UriKind.Relative));
            this.cw入力画面 = ((System.Windows.Controls.ChildWindow)(this.FindName("cw入力画面")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.gdTopTitle = ((System.Windows.Controls.Grid)(this.FindName("gdTopTitle")));
            this.gdInnerTopLeft = ((System.Windows.Controls.Grid)(this.FindName("gdInnerTopLeft")));
            this.gdAsterisk = ((System.Windows.Controls.Grid)(this.FindName("gdAsterisk")));
            this.gdTag = ((System.Windows.Controls.Grid)(this.FindName("gdTag")));
            this.stpTag = ((System.Windows.Controls.StackPanel)(this.FindName("stpTag")));
            this.tbMainTitle = ((System.Windows.Controls.TextBlock)(this.FindName("tbMainTitle")));
            this.tbSubTitle = ((System.Windows.Controls.TextBlock)(this.FindName("tbSubTitle")));
            this.tbHomepageUrl = ((System.Windows.Controls.TextBlock)(this.FindName("tbHomepageUrl")));
            this.tbEmail = ((System.Windows.Controls.TextBlock)(this.FindName("tbEmail")));
            this.tb添付File = ((System.Windows.Controls.TextBlock)(this.FindName("tb添付File")));
            this.tbl暗証キー = ((System.Windows.Controls.TextBlock)(this.FindName("tbl暗証キー")));
            this.gdInnerTopRight = ((System.Windows.Controls.Grid)(this.FindName("gdInnerTopRight")));
            this.tbInputTitle = ((View.Core.共通.UserControls.PharmacyToolTextBox)(this.FindName("tbInputTitle")));
            this.tbInputPerson = ((View.Core.共通.UserControls.PharmacyToolTextBox)(this.FindName("tbInputPerson")));
            this.tbInputHomepageUrl = ((View.Core.共通.UserControls.PharmacyToolTextBox)(this.FindName("tbInputHomepageUrl")));
            this.tbInputEmail = ((View.Core.共通.UserControls.PharmacyToolTextBox)(this.FindName("tbInputEmail")));
            this.stp添付File = ((System.Windows.Controls.StackPanel)(this.FindName("stp添付File")));
            this.bt添付File = ((System.Windows.Controls.Button)(this.FindName("bt添付File")));
            this.cmb添付Files = ((System.Windows.Controls.ComboBox)(this.FindName("cmb添付Files")));
            this.bt添付削除 = ((System.Windows.Controls.Button)(this.FindName("bt添付削除")));
            this.stp暗証キー = ((System.Windows.Controls.StackPanel)(this.FindName("stp暗証キー")));
            this.pb暗証キー = ((System.Windows.Controls.PasswordBox)(this.FindName("pb暗証キー")));
            this.tbl暗証キーヒント = ((System.Windows.Controls.TextBlock)(this.FindName("tbl暗証キーヒント")));
            this.gdMainCenter = ((System.Windows.Controls.Grid)(this.FindName("gdMainCenter")));
            this.wtbInputArticle = ((Microsoft.Windows.Controls.WatermarkedTextBox)(this.FindName("wtbInputArticle")));
            this.gdLine = ((System.Windows.Controls.Grid)(this.FindName("gdLine")));
            this.cmbSelectColors = ((System.Windows.Controls.ComboBox)(this.FindName("cmbSelectColors")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
        }
    }
}
