﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 3.0.40818.0
// 
namespace View.Service.DAO.PharmacyTool {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ログインチェック結果", Namespace="http://schemas.datacontract.org/2004/07/PharmacyTool.Web.Service.DAO.PharmacyTool" +
        "")]
    public partial class ログインチェック結果 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int アクセス権限Field;
        
        private string エラーメッセージField;
        
        private bool チェック成功かField;
        
        private string ユーザーIDField;
        
        private string 表示名称Field;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int アクセス権限 {
            get {
                return this.アクセス権限Field;
            }
            set {
                if ((this.アクセス権限Field.Equals(value) != true)) {
                    this.アクセス権限Field = value;
                    this.RaisePropertyChanged("アクセス権限");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string エラーメッセージ {
            get {
                return this.エラーメッセージField;
            }
            set {
                if ((object.ReferenceEquals(this.エラーメッセージField, value) != true)) {
                    this.エラーメッセージField = value;
                    this.RaisePropertyChanged("エラーメッセージ");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool チェック成功か {
            get {
                return this.チェック成功かField;
            }
            set {
                if ((this.チェック成功かField.Equals(value) != true)) {
                    this.チェック成功かField = value;
                    this.RaisePropertyChanged("チェック成功か");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ユーザーID {
            get {
                return this.ユーザーIDField;
            }
            set {
                if ((object.ReferenceEquals(this.ユーザーIDField, value) != true)) {
                    this.ユーザーIDField = value;
                    this.RaisePropertyChanged("ユーザーID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string 表示名称 {
            get {
                return this.表示名称Field;
            }
            set {
                if ((object.ReferenceEquals(this.表示名称Field, value) != true)) {
                    this.表示名称Field = value;
                    this.RaisePropertyChanged("表示名称");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.DAO.PharmacyTool.ILoginCheck")]
    public interface ILoginCheck {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ILoginCheck/LoginCheck実行", ReplyAction="http://tempuri.org/ILoginCheck/LoginCheck実行Response")]
        System.IAsyncResult BeginLoginCheck実行(string 入力ユーザーID, string 入力コンフィデンシャル, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果> EndLoginCheck実行(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILoginCheckChannel : View.Service.DAO.PharmacyTool.ILoginCheck, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoginCheck実行CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public LoginCheck実行CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoginCheckClient : System.ServiceModel.ClientBase<View.Service.DAO.PharmacyTool.ILoginCheck>, View.Service.DAO.PharmacyTool.ILoginCheck {
        
        private BeginOperationDelegate onBeginLoginCheck実行Delegate;
        
        private EndOperationDelegate onEndLoginCheck実行Delegate;
        
        private System.Threading.SendOrPostCallback onLoginCheck実行CompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public LoginCheckClient() {
        }
        
        public LoginCheckClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LoginCheckClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoginCheckClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoginCheckClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("CookieContainer を設定できません。バインドに HttpCookieContainerBindingElement が含まれていることを確認してくだ" +
                            "さい。");
                }
            }
        }
        
        public event System.EventHandler<LoginCheck実行CompletedEventArgs> LoginCheck実行Completed;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult View.Service.DAO.PharmacyTool.ILoginCheck.BeginLoginCheck実行(string 入力ユーザーID, string 入力コンフィデンシャル, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginLoginCheck実行(入力ユーザーID, 入力コンフィデンシャル, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果> View.Service.DAO.PharmacyTool.ILoginCheck.EndLoginCheck実行(System.IAsyncResult result) {
            return base.Channel.EndLoginCheck実行(result);
        }
        
        private System.IAsyncResult OnBeginLoginCheck実行(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string 入力ユーザーID = ((string)(inValues[0]));
            string 入力コンフィデンシャル = ((string)(inValues[1]));
            return ((View.Service.DAO.PharmacyTool.ILoginCheck)(this)).BeginLoginCheck実行(入力ユーザーID, 入力コンフィデンシャル, callback, asyncState);
        }
        
        private object[] OnEndLoginCheck実行(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果> retVal = ((View.Service.DAO.PharmacyTool.ILoginCheck)(this)).EndLoginCheck実行(result);
            return new object[] {
                    retVal};
        }
        
        private void OnLoginCheck実行Completed(object state) {
            if ((this.LoginCheck実行Completed != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.LoginCheck実行Completed(this, new LoginCheck実行CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void LoginCheck実行Async(string 入力ユーザーID, string 入力コンフィデンシャル) {
            this.LoginCheck実行Async(入力ユーザーID, 入力コンフィデンシャル, null);
        }
        
        public void LoginCheck実行Async(string 入力ユーザーID, string 入力コンフィデンシャル, object userState) {
            if ((this.onBeginLoginCheck実行Delegate == null)) {
                this.onBeginLoginCheck実行Delegate = new BeginOperationDelegate(this.OnBeginLoginCheck実行);
            }
            if ((this.onEndLoginCheck実行Delegate == null)) {
                this.onEndLoginCheck実行Delegate = new EndOperationDelegate(this.OnEndLoginCheck実行);
            }
            if ((this.onLoginCheck実行CompletedDelegate == null)) {
                this.onLoginCheck実行CompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLoginCheck実行Completed);
            }
            base.InvokeAsync(this.onBeginLoginCheck実行Delegate, new object[] {
                        入力ユーザーID,
                        入力コンフィデンシャル}, this.onEndLoginCheck実行Delegate, this.onLoginCheck実行CompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override View.Service.DAO.PharmacyTool.ILoginCheck CreateChannel() {
            return new LoginCheckClientChannel(this);
        }
        
        private class LoginCheckClientChannel : ChannelBase<View.Service.DAO.PharmacyTool.ILoginCheck>, View.Service.DAO.PharmacyTool.ILoginCheck {
            
            public LoginCheckClientChannel(System.ServiceModel.ClientBase<View.Service.DAO.PharmacyTool.ILoginCheck> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginLoginCheck実行(string 入力ユーザーID, string 入力コンフィデンシャル, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = 入力ユーザーID;
                _args[1] = 入力コンフィデンシャル;
                System.IAsyncResult _result = base.BeginInvoke("LoginCheck実行", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果> EndLoginCheck実行(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果> _result = ((System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.ログインチェック結果>)(base.EndInvoke("LoginCheck実行", _args, result)));
                return _result;
            }
        }
    }
}
