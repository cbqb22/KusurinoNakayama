﻿#pragma checksum "C:\Users\poohace\Desktop\PharmacyTool最新 - コピー\View\Core\Login\LoginCheckDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "102F785A9FC7457F2546748147465637"
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
using View.Core.共通.UserControls;


namespace View.Core.Login {
    
    
    public partial class LoginCheckDialog : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal View.Core.共通.UserControls.PharmacyToolTextBox ユーザーIDTextBox;
        
        internal System.Windows.Controls.PasswordBox パスワードTextBox;
        
        internal System.Windows.Controls.Button LoginButton;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.TextBlock UserIDTextBlock;
        
        internal System.Windows.Controls.TextBlock PasswordTextBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/View;component/Core/Login/LoginCheckDialog.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ユーザーIDTextBox = ((View.Core.共通.UserControls.PharmacyToolTextBox)(this.FindName("ユーザーIDTextBox")));
            this.パスワードTextBox = ((System.Windows.Controls.PasswordBox)(this.FindName("パスワードTextBox")));
            this.LoginButton = ((System.Windows.Controls.Button)(this.FindName("LoginButton")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.UserIDTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("UserIDTextBlock")));
            this.PasswordTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PasswordTextBlock")));
        }
    }
}
