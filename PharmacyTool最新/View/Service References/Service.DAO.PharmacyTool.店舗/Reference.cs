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
namespace View.Service.DAO.PharmacyTool.店舗 {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PT店舗名リターンデータセット", Namespace="http://schemas.datacontract.org/2004/07/PharmacyTool.Web.Service.DAO.PharmacyTool" +
        ".%E5%BA%97%E8%88%97")]
    public partial class PT店舗名リターンデータセット : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int IDField;
        
        private string 店舗名Field;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string 店舗名 {
            get {
                return this.店舗名Field;
            }
            set {
                if ((object.ReferenceEquals(this.店舗名Field, value) != true)) {
                    this.店舗名Field = value;
                    this.RaisePropertyChanged("店舗名");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.DAO.PharmacyTool.店舗.IStoreData")]
    public interface IStoreData {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IStoreData/店舗名取得", ReplyAction="http://tempuri.org/IStoreData/店舗名取得Response")]
        System.IAsyncResult Begin店舗名取得(System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット> End店舗名取得(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IStoreData/新規店舗名作成", ReplyAction="http://tempuri.org/IStoreData/新規店舗名作成Response")]
        System.IAsyncResult Begin新規店舗名作成(string 作成店舗名, System.AsyncCallback callback, object asyncState);
        
        string End新規店舗名作成(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IStoreData/店舗名削除", ReplyAction="http://tempuri.org/IStoreData/店舗名削除Response")]
        System.IAsyncResult Begin店舗名削除(string 削除店舗名, System.AsyncCallback callback, object asyncState);
        
        string End店舗名削除(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStoreDataChannel : View.Service.DAO.PharmacyTool.店舗.IStoreData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class 店舗名取得CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public 店舗名取得CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class 新規店舗名作成CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public 新規店舗名作成CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class 店舗名削除CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public 店舗名削除CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StoreDataClient : System.ServiceModel.ClientBase<View.Service.DAO.PharmacyTool.店舗.IStoreData>, View.Service.DAO.PharmacyTool.店舗.IStoreData {
        
        private BeginOperationDelegate onBegin店舗名取得Delegate;
        
        private EndOperationDelegate onEnd店舗名取得Delegate;
        
        private System.Threading.SendOrPostCallback on店舗名取得CompletedDelegate;
        
        private BeginOperationDelegate onBegin新規店舗名作成Delegate;
        
        private EndOperationDelegate onEnd新規店舗名作成Delegate;
        
        private System.Threading.SendOrPostCallback on新規店舗名作成CompletedDelegate;
        
        private BeginOperationDelegate onBegin店舗名削除Delegate;
        
        private EndOperationDelegate onEnd店舗名削除Delegate;
        
        private System.Threading.SendOrPostCallback on店舗名削除CompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public StoreDataClient() {
        }
        
        public StoreDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StoreDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StoreDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StoreDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<店舗名取得CompletedEventArgs> 店舗名取得Completed;
        
        public event System.EventHandler<新規店舗名作成CompletedEventArgs> 新規店舗名作成Completed;
        
        public event System.EventHandler<店舗名削除CompletedEventArgs> 店舗名削除Completed;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult View.Service.DAO.PharmacyTool.店舗.IStoreData.Begin店舗名取得(System.AsyncCallback callback, object asyncState) {
            return base.Channel.Begin店舗名取得(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット> View.Service.DAO.PharmacyTool.店舗.IStoreData.End店舗名取得(System.IAsyncResult result) {
            return base.Channel.End店舗名取得(result);
        }
        
        private System.IAsyncResult OnBegin店舗名取得(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((View.Service.DAO.PharmacyTool.店舗.IStoreData)(this)).Begin店舗名取得(callback, asyncState);
        }
        
        private object[] OnEnd店舗名取得(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット> retVal = ((View.Service.DAO.PharmacyTool.店舗.IStoreData)(this)).End店舗名取得(result);
            return new object[] {
                    retVal};
        }
        
        private void On店舗名取得Completed(object state) {
            if ((this.店舗名取得Completed != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.店舗名取得Completed(this, new 店舗名取得CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void 店舗名取得Async() {
            this.店舗名取得Async(null);
        }
        
        public void 店舗名取得Async(object userState) {
            if ((this.onBegin店舗名取得Delegate == null)) {
                this.onBegin店舗名取得Delegate = new BeginOperationDelegate(this.OnBegin店舗名取得);
            }
            if ((this.onEnd店舗名取得Delegate == null)) {
                this.onEnd店舗名取得Delegate = new EndOperationDelegate(this.OnEnd店舗名取得);
            }
            if ((this.on店舗名取得CompletedDelegate == null)) {
                this.on店舗名取得CompletedDelegate = new System.Threading.SendOrPostCallback(this.On店舗名取得Completed);
            }
            base.InvokeAsync(this.onBegin店舗名取得Delegate, null, this.onEnd店舗名取得Delegate, this.on店舗名取得CompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult View.Service.DAO.PharmacyTool.店舗.IStoreData.Begin新規店舗名作成(string 作成店舗名, System.AsyncCallback callback, object asyncState) {
            return base.Channel.Begin新規店舗名作成(作成店舗名, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string View.Service.DAO.PharmacyTool.店舗.IStoreData.End新規店舗名作成(System.IAsyncResult result) {
            return base.Channel.End新規店舗名作成(result);
        }
        
        private System.IAsyncResult OnBegin新規店舗名作成(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string 作成店舗名 = ((string)(inValues[0]));
            return ((View.Service.DAO.PharmacyTool.店舗.IStoreData)(this)).Begin新規店舗名作成(作成店舗名, callback, asyncState);
        }
        
        private object[] OnEnd新規店舗名作成(System.IAsyncResult result) {
            string retVal = ((View.Service.DAO.PharmacyTool.店舗.IStoreData)(this)).End新規店舗名作成(result);
            return new object[] {
                    retVal};
        }
        
        private void On新規店舗名作成Completed(object state) {
            if ((this.新規店舗名作成Completed != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.新規店舗名作成Completed(this, new 新規店舗名作成CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void 新規店舗名作成Async(string 作成店舗名) {
            this.新規店舗名作成Async(作成店舗名, null);
        }
        
        public void 新規店舗名作成Async(string 作成店舗名, object userState) {
            if ((this.onBegin新規店舗名作成Delegate == null)) {
                this.onBegin新規店舗名作成Delegate = new BeginOperationDelegate(this.OnBegin新規店舗名作成);
            }
            if ((this.onEnd新規店舗名作成Delegate == null)) {
                this.onEnd新規店舗名作成Delegate = new EndOperationDelegate(this.OnEnd新規店舗名作成);
            }
            if ((this.on新規店舗名作成CompletedDelegate == null)) {
                this.on新規店舗名作成CompletedDelegate = new System.Threading.SendOrPostCallback(this.On新規店舗名作成Completed);
            }
            base.InvokeAsync(this.onBegin新規店舗名作成Delegate, new object[] {
                        作成店舗名}, this.onEnd新規店舗名作成Delegate, this.on新規店舗名作成CompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult View.Service.DAO.PharmacyTool.店舗.IStoreData.Begin店舗名削除(string 削除店舗名, System.AsyncCallback callback, object asyncState) {
            return base.Channel.Begin店舗名削除(削除店舗名, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string View.Service.DAO.PharmacyTool.店舗.IStoreData.End店舗名削除(System.IAsyncResult result) {
            return base.Channel.End店舗名削除(result);
        }
        
        private System.IAsyncResult OnBegin店舗名削除(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string 削除店舗名 = ((string)(inValues[0]));
            return ((View.Service.DAO.PharmacyTool.店舗.IStoreData)(this)).Begin店舗名削除(削除店舗名, callback, asyncState);
        }
        
        private object[] OnEnd店舗名削除(System.IAsyncResult result) {
            string retVal = ((View.Service.DAO.PharmacyTool.店舗.IStoreData)(this)).End店舗名削除(result);
            return new object[] {
                    retVal};
        }
        
        private void On店舗名削除Completed(object state) {
            if ((this.店舗名削除Completed != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.店舗名削除Completed(this, new 店舗名削除CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void 店舗名削除Async(string 削除店舗名) {
            this.店舗名削除Async(削除店舗名, null);
        }
        
        public void 店舗名削除Async(string 削除店舗名, object userState) {
            if ((this.onBegin店舗名削除Delegate == null)) {
                this.onBegin店舗名削除Delegate = new BeginOperationDelegate(this.OnBegin店舗名削除);
            }
            if ((this.onEnd店舗名削除Delegate == null)) {
                this.onEnd店舗名削除Delegate = new EndOperationDelegate(this.OnEnd店舗名削除);
            }
            if ((this.on店舗名削除CompletedDelegate == null)) {
                this.on店舗名削除CompletedDelegate = new System.Threading.SendOrPostCallback(this.On店舗名削除Completed);
            }
            base.InvokeAsync(this.onBegin店舗名削除Delegate, new object[] {
                        削除店舗名}, this.onEnd店舗名削除Delegate, this.on店舗名削除CompletedDelegate, userState);
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
        
        protected override View.Service.DAO.PharmacyTool.店舗.IStoreData CreateChannel() {
            return new StoreDataClientChannel(this);
        }
        
        private class StoreDataClientChannel : ChannelBase<View.Service.DAO.PharmacyTool.店舗.IStoreData>, View.Service.DAO.PharmacyTool.店舗.IStoreData {
            
            public StoreDataClientChannel(System.ServiceModel.ClientBase<View.Service.DAO.PharmacyTool.店舗.IStoreData> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult Begin店舗名取得(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("店舗名取得", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット> End店舗名取得(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット> _result = ((System.Collections.ObjectModel.ObservableCollection<View.Service.DAO.PharmacyTool.店舗.PT店舗名リターンデータセット>)(base.EndInvoke("店舗名取得", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult Begin新規店舗名作成(string 作成店舗名, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = 作成店舗名;
                System.IAsyncResult _result = base.BeginInvoke("新規店舗名作成", _args, callback, asyncState);
                return _result;
            }
            
            public string End新規店舗名作成(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("新規店舗名作成", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult Begin店舗名削除(string 削除店舗名, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = 削除店舗名;
                System.IAsyncResult _result = base.BeginInvoke("店舗名削除", _args, callback, asyncState);
                return _result;
            }
            
            public string End店舗名削除(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("店舗名削除", _args, result)));
                return _result;
            }
        }
    }
}
