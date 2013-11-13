﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace Hylasoft.OrdersGui.EventMonitor {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ErrorObject", Namespace="http://schemas.datacontract.org/2004/07/EventMonitorService")]
    public partial class ErrorObject : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ExceptionField;
        
        private bool WasItSuccessfullField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Exception {
            get {
                return this.ExceptionField;
            }
            set {
                if ((object.ReferenceEquals(this.ExceptionField, value) != true)) {
                    this.ExceptionField = value;
                    this.RaisePropertyChanged("Exception");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool WasItSuccessfull {
            get {
                return this.WasItSuccessfullField;
            }
            set {
                if ((this.WasItSuccessfullField.Equals(value) != true)) {
                    this.WasItSuccessfullField = value;
                    this.RaisePropertyChanged("WasItSuccessfull");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserException", Namespace="http://schemas.datacontract.org/2004/07/EventMonitorService")]
    public partial class UserException : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ExceptionMessageField;
        
        private string UserMessageField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExceptionMessage {
            get {
                return this.ExceptionMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ExceptionMessageField, value) != true)) {
                    this.ExceptionMessageField = value;
                    this.RaisePropertyChanged("ExceptionMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserMessage {
            get {
                return this.UserMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.UserMessageField, value) != true)) {
                    this.UserMessageField = value;
                    this.RaisePropertyChanged("UserMessage");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EventMonitor.IEventMonitor")]
    public interface IEventMonitor {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IEventMonitor/releaseOrderOPC", ReplyAction="http://tempuri.org/IEventMonitor/releaseOrderOPCResponse")]
        System.IAsyncResult BeginreleaseOrderOPC(long orderId, System.AsyncCallback callback, object asyncState);
        
        Hylasoft.OrdersGui.EventMonitor.ErrorObject EndreleaseOrderOPC(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IEventMonitor/WriteOPCOrderStatus", ReplyAction="http://tempuri.org/IEventMonitor/WriteOPCOrderStatusResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Hylasoft.OrdersGui.EventMonitor.UserException), Action="http://tempuri.org/IEventMonitor/WriteOPCOrderStatusUserExceptionFault", Name="UserException", Namespace="http://schemas.datacontract.org/2004/07/EventMonitorService")]
        System.IAsyncResult BeginWriteOPCOrderStatus(long orderId, int orderStatus, System.AsyncCallback callback, object asyncState);
        
        void EndWriteOPCOrderStatus(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IEventMonitor/GetOpcServerState", ReplyAction="http://tempuri.org/IEventMonitor/GetOpcServerStateResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Hylasoft.OrdersGui.EventMonitor.UserException), Action="http://tempuri.org/IEventMonitor/GetOpcServerStateUserExceptionFault", Name="UserException", Namespace="http://schemas.datacontract.org/2004/07/EventMonitorService")]
        System.IAsyncResult BeginGetOpcServerState(System.AsyncCallback callback, object asyncState);
        
        string EndGetOpcServerState(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEventMonitorChannel : Hylasoft.OrdersGui.EventMonitor.IEventMonitor, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class releaseOrderOPCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public releaseOrderOPCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Hylasoft.OrdersGui.EventMonitor.ErrorObject Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Hylasoft.OrdersGui.EventMonitor.ErrorObject)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetOpcServerStateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetOpcServerStateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public partial class EventMonitorClient : System.ServiceModel.ClientBase<Hylasoft.OrdersGui.EventMonitor.IEventMonitor>, Hylasoft.OrdersGui.EventMonitor.IEventMonitor {
        
        private BeginOperationDelegate onBeginreleaseOrderOPCDelegate;
        
        private EndOperationDelegate onEndreleaseOrderOPCDelegate;
        
        private System.Threading.SendOrPostCallback onreleaseOrderOPCCompletedDelegate;
        
        private BeginOperationDelegate onBeginWriteOPCOrderStatusDelegate;
        
        private EndOperationDelegate onEndWriteOPCOrderStatusDelegate;
        
        private System.Threading.SendOrPostCallback onWriteOPCOrderStatusCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetOpcServerStateDelegate;
        
        private EndOperationDelegate onEndGetOpcServerStateDelegate;
        
        private System.Threading.SendOrPostCallback onGetOpcServerStateCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public EventMonitorClient() {
        }
        
        public EventMonitorClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EventMonitorClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventMonitorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventMonitorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<releaseOrderOPCCompletedEventArgs> releaseOrderOPCCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> WriteOPCOrderStatusCompleted;
        
        public event System.EventHandler<GetOpcServerStateCompletedEventArgs> GetOpcServerStateCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hylasoft.OrdersGui.EventMonitor.IEventMonitor.BeginreleaseOrderOPC(long orderId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginreleaseOrderOPC(orderId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Hylasoft.OrdersGui.EventMonitor.ErrorObject Hylasoft.OrdersGui.EventMonitor.IEventMonitor.EndreleaseOrderOPC(System.IAsyncResult result) {
            return base.Channel.EndreleaseOrderOPC(result);
        }
        
        private System.IAsyncResult OnBeginreleaseOrderOPC(object[] inValues, System.AsyncCallback callback, object asyncState) {
            long orderId = ((long)(inValues[0]));
            return ((Hylasoft.OrdersGui.EventMonitor.IEventMonitor)(this)).BeginreleaseOrderOPC(orderId, callback, asyncState);
        }
        
        private object[] OnEndreleaseOrderOPC(System.IAsyncResult result) {
            Hylasoft.OrdersGui.EventMonitor.ErrorObject retVal = ((Hylasoft.OrdersGui.EventMonitor.IEventMonitor)(this)).EndreleaseOrderOPC(result);
            return new object[] {
                    retVal};
        }
        
        private void OnreleaseOrderOPCCompleted(object state) {
            if ((this.releaseOrderOPCCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.releaseOrderOPCCompleted(this, new releaseOrderOPCCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void releaseOrderOPCAsync(long orderId) {
            this.releaseOrderOPCAsync(orderId, null);
        }
        
        public void releaseOrderOPCAsync(long orderId, object userState) {
            if ((this.onBeginreleaseOrderOPCDelegate == null)) {
                this.onBeginreleaseOrderOPCDelegate = new BeginOperationDelegate(this.OnBeginreleaseOrderOPC);
            }
            if ((this.onEndreleaseOrderOPCDelegate == null)) {
                this.onEndreleaseOrderOPCDelegate = new EndOperationDelegate(this.OnEndreleaseOrderOPC);
            }
            if ((this.onreleaseOrderOPCCompletedDelegate == null)) {
                this.onreleaseOrderOPCCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnreleaseOrderOPCCompleted);
            }
            base.InvokeAsync(this.onBeginreleaseOrderOPCDelegate, new object[] {
                        orderId}, this.onEndreleaseOrderOPCDelegate, this.onreleaseOrderOPCCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hylasoft.OrdersGui.EventMonitor.IEventMonitor.BeginWriteOPCOrderStatus(long orderId, int orderStatus, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginWriteOPCOrderStatus(orderId, orderStatus, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void Hylasoft.OrdersGui.EventMonitor.IEventMonitor.EndWriteOPCOrderStatus(System.IAsyncResult result) {
            base.Channel.EndWriteOPCOrderStatus(result);
        }
        
        private System.IAsyncResult OnBeginWriteOPCOrderStatus(object[] inValues, System.AsyncCallback callback, object asyncState) {
            long orderId = ((long)(inValues[0]));
            int orderStatus = ((int)(inValues[1]));
            return ((Hylasoft.OrdersGui.EventMonitor.IEventMonitor)(this)).BeginWriteOPCOrderStatus(orderId, orderStatus, callback, asyncState);
        }
        
        private object[] OnEndWriteOPCOrderStatus(System.IAsyncResult result) {
            ((Hylasoft.OrdersGui.EventMonitor.IEventMonitor)(this)).EndWriteOPCOrderStatus(result);
            return null;
        }
        
        private void OnWriteOPCOrderStatusCompleted(object state) {
            if ((this.WriteOPCOrderStatusCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.WriteOPCOrderStatusCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void WriteOPCOrderStatusAsync(long orderId, int orderStatus) {
            this.WriteOPCOrderStatusAsync(orderId, orderStatus, null);
        }
        
        public void WriteOPCOrderStatusAsync(long orderId, int orderStatus, object userState) {
            if ((this.onBeginWriteOPCOrderStatusDelegate == null)) {
                this.onBeginWriteOPCOrderStatusDelegate = new BeginOperationDelegate(this.OnBeginWriteOPCOrderStatus);
            }
            if ((this.onEndWriteOPCOrderStatusDelegate == null)) {
                this.onEndWriteOPCOrderStatusDelegate = new EndOperationDelegate(this.OnEndWriteOPCOrderStatus);
            }
            if ((this.onWriteOPCOrderStatusCompletedDelegate == null)) {
                this.onWriteOPCOrderStatusCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnWriteOPCOrderStatusCompleted);
            }
            base.InvokeAsync(this.onBeginWriteOPCOrderStatusDelegate, new object[] {
                        orderId,
                        orderStatus}, this.onEndWriteOPCOrderStatusDelegate, this.onWriteOPCOrderStatusCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hylasoft.OrdersGui.EventMonitor.IEventMonitor.BeginGetOpcServerState(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetOpcServerState(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string Hylasoft.OrdersGui.EventMonitor.IEventMonitor.EndGetOpcServerState(System.IAsyncResult result) {
            return base.Channel.EndGetOpcServerState(result);
        }
        
        private System.IAsyncResult OnBeginGetOpcServerState(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((Hylasoft.OrdersGui.EventMonitor.IEventMonitor)(this)).BeginGetOpcServerState(callback, asyncState);
        }
        
        private object[] OnEndGetOpcServerState(System.IAsyncResult result) {
            string retVal = ((Hylasoft.OrdersGui.EventMonitor.IEventMonitor)(this)).EndGetOpcServerState(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetOpcServerStateCompleted(object state) {
            if ((this.GetOpcServerStateCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetOpcServerStateCompleted(this, new GetOpcServerStateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetOpcServerStateAsync() {
            this.GetOpcServerStateAsync(null);
        }
        
        public void GetOpcServerStateAsync(object userState) {
            if ((this.onBeginGetOpcServerStateDelegate == null)) {
                this.onBeginGetOpcServerStateDelegate = new BeginOperationDelegate(this.OnBeginGetOpcServerState);
            }
            if ((this.onEndGetOpcServerStateDelegate == null)) {
                this.onEndGetOpcServerStateDelegate = new EndOperationDelegate(this.OnEndGetOpcServerState);
            }
            if ((this.onGetOpcServerStateCompletedDelegate == null)) {
                this.onGetOpcServerStateCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetOpcServerStateCompleted);
            }
            base.InvokeAsync(this.onBeginGetOpcServerStateDelegate, null, this.onEndGetOpcServerStateDelegate, this.onGetOpcServerStateCompletedDelegate, userState);
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
        
        protected override Hylasoft.OrdersGui.EventMonitor.IEventMonitor CreateChannel() {
            return new EventMonitorClientChannel(this);
        }
        
        private class EventMonitorClientChannel : ChannelBase<Hylasoft.OrdersGui.EventMonitor.IEventMonitor>, Hylasoft.OrdersGui.EventMonitor.IEventMonitor {
            
            public EventMonitorClientChannel(System.ServiceModel.ClientBase<Hylasoft.OrdersGui.EventMonitor.IEventMonitor> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginreleaseOrderOPC(long orderId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = orderId;
                System.IAsyncResult _result = base.BeginInvoke("releaseOrderOPC", _args, callback, asyncState);
                return _result;
            }
            
            public Hylasoft.OrdersGui.EventMonitor.ErrorObject EndreleaseOrderOPC(System.IAsyncResult result) {
                object[] _args = new object[0];
                Hylasoft.OrdersGui.EventMonitor.ErrorObject _result = ((Hylasoft.OrdersGui.EventMonitor.ErrorObject)(base.EndInvoke("releaseOrderOPC", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginWriteOPCOrderStatus(long orderId, int orderStatus, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = orderId;
                _args[1] = orderStatus;
                System.IAsyncResult _result = base.BeginInvoke("WriteOPCOrderStatus", _args, callback, asyncState);
                return _result;
            }
            
            public void EndWriteOPCOrderStatus(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("WriteOPCOrderStatus", _args, result);
            }
            
            public System.IAsyncResult BeginGetOpcServerState(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetOpcServerState", _args, callback, asyncState);
                return _result;
            }
            
            public string EndGetOpcServerState(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("GetOpcServerState", _args, result)));
                return _result;
            }
        }
    }
}
