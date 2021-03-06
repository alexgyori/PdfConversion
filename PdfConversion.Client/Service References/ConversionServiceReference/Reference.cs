﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PdfConversion.Client.ConversionServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FileState", Namespace="http://schemas.datacontract.org/2004/07/PdfConversionService.API")]
    public enum FileState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Queued = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Converting = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Converted = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Erroneous = 4,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FileStatus", Namespace="http://schemas.datacontract.org/2004/07/PdfConversion.Server.StatusService")]
    [System.SerializableAttribute()]
    public partial class FileStatus : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ConvertedFileNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FileNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private PdfConversion.Client.ConversionServiceReference.FileState FileStateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhysicalFileNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ConvertedFileName {
            get {
                return this.ConvertedFileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ConvertedFileNameField, value) != true)) {
                    this.ConvertedFileNameField = value;
                    this.RaisePropertyChanged("ConvertedFileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FileName {
            get {
                return this.FileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FileNameField, value) != true)) {
                    this.FileNameField = value;
                    this.RaisePropertyChanged("FileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public PdfConversion.Client.ConversionServiceReference.FileState FileState {
            get {
                return this.FileStateField;
            }
            set {
                if ((this.FileStateField.Equals(value) != true)) {
                    this.FileStateField = value;
                    this.RaisePropertyChanged("FileState");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhysicalFileName {
            get {
                return this.PhysicalFileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.PhysicalFileNameField, value) != true)) {
                    this.PhysicalFileNameField = value;
                    this.RaisePropertyChanged("PhysicalFileName");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServerState", Namespace="http://schemas.datacontract.org/2004/07/PdfConversionService.API")]
    public enum ServerState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Offline = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Waiting = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Processing = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ConversionServiceReference.IConversionService")]
    public interface IConversionService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConversionService/GetFileState", ReplyAction="http://tempuri.org/IConversionService/GetFileStateResponse")]
        PdfConversion.Client.ConversionServiceReference.FileState GetFileState(string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConversionService/GetFileState", ReplyAction="http://tempuri.org/IConversionService/GetFileStateResponse")]
        System.Threading.Tasks.Task<PdfConversion.Client.ConversionServiceReference.FileState> GetFileStateAsync(string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConversionService/GetAvailableFiles", ReplyAction="http://tempuri.org/IConversionService/GetAvailableFilesResponse")]
        PdfConversion.Client.ConversionServiceReference.FileStatus[] GetAvailableFiles();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConversionService/GetAvailableFiles", ReplyAction="http://tempuri.org/IConversionService/GetAvailableFilesResponse")]
        System.Threading.Tasks.Task<PdfConversion.Client.ConversionServiceReference.FileStatus[]> GetAvailableFilesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConversionService/GetServerStatus", ReplyAction="http://tempuri.org/IConversionService/GetServerStatusResponse")]
        PdfConversion.Client.ConversionServiceReference.ServerState GetServerStatus();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConversionService/GetServerStatus", ReplyAction="http://tempuri.org/IConversionService/GetServerStatusResponse")]
        System.Threading.Tasks.Task<PdfConversion.Client.ConversionServiceReference.ServerState> GetServerStatusAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IConversionServiceChannel : PdfConversion.Client.ConversionServiceReference.IConversionService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConversionServiceClient : System.ServiceModel.ClientBase<PdfConversion.Client.ConversionServiceReference.IConversionService>, PdfConversion.Client.ConversionServiceReference.IConversionService {
        
        public ConversionServiceClient() {
        }
        
        public ConversionServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ConversionServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConversionServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConversionServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public PdfConversion.Client.ConversionServiceReference.FileState GetFileState(string fileName) {
            return base.Channel.GetFileState(fileName);
        }
        
        public System.Threading.Tasks.Task<PdfConversion.Client.ConversionServiceReference.FileState> GetFileStateAsync(string fileName) {
            return base.Channel.GetFileStateAsync(fileName);
        }
        
        public PdfConversion.Client.ConversionServiceReference.FileStatus[] GetAvailableFiles() {
            return base.Channel.GetAvailableFiles();
        }
        
        public System.Threading.Tasks.Task<PdfConversion.Client.ConversionServiceReference.FileStatus[]> GetAvailableFilesAsync() {
            return base.Channel.GetAvailableFilesAsync();
        }
        
        public PdfConversion.Client.ConversionServiceReference.ServerState GetServerStatus() {
            return base.Channel.GetServerStatus();
        }
        
        public System.Threading.Tasks.Task<PdfConversion.Client.ConversionServiceReference.ServerState> GetServerStatusAsync() {
            return base.Channel.GetServerStatusAsync();
        }
    }
}
