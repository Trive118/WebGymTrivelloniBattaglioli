﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebGymTrivelloniBattaglioli.ServiceReferenceWCF {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://schemas.datacontract.org/2004/07/")]
    [System.SerializableAttribute()]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BoolValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StringValueField;
        
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
        public bool BoolValue {
            get {
                return this.BoolValueField;
            }
            set {
                if ((this.BoolValueField.Equals(value) != true)) {
                    this.BoolValueField = value;
                    this.RaisePropertyChanged("BoolValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue {
            get {
                return this.StringValueField;
            }
            set {
                if ((object.ReferenceEquals(this.StringValueField, value) != true)) {
                    this.StringValueField = value;
                    this.RaisePropertyChanged("StringValue");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ContractDTO", Namespace="http://schemas.datacontract.org/2004/07/")]
    [System.SerializableAttribute()]
    public partial class ContractDTO : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descrizioneField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int durataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double prezzoField;
        
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
        public string descrizione {
            get {
                return this.descrizioneField;
            }
            set {
                if ((object.ReferenceEquals(this.descrizioneField, value) != true)) {
                    this.descrizioneField = value;
                    this.RaisePropertyChanged("descrizione");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int durata {
            get {
                return this.durataField;
            }
            set {
                if ((this.durataField.Equals(value) != true)) {
                    this.durataField = value;
                    this.RaisePropertyChanged("durata");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double prezzo {
            get {
                return this.prezzoField;
            }
            set {
                if ((this.prezzoField.Equals(value) != true)) {
                    this.prezzoField = value;
                    this.RaisePropertyChanged("prezzo");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReferenceWCF.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IService/GetDataUsingDataContractResponse")]
        WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType GetDataUsingDataContract(WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IService/GetDataUsingDataContractResponse")]
        System.Threading.Tasks.Task<WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType> GetDataUsingDataContractAsync(WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/InserisciCliente", ReplyAction="http://tempuri.org/IService/InserisciClienteResponse")]
        bool InserisciCliente(string codice_fiscale, string nome, string cognome, string mail, string data_nascita, string telefono, string password, string sesso);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/InserisciCliente", ReplyAction="http://tempuri.org/IService/InserisciClienteResponse")]
        System.Threading.Tasks.Task<bool> InserisciClienteAsync(string codice_fiscale, string nome, string cognome, string mail, string data_nascita, string telefono, string password, string sesso);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetAvailableContracts", ReplyAction="http://tempuri.org/IService/GetAvailableContractsResponse")]
        WebGymTrivelloniBattaglioli.ServiceReferenceWCF.ContractDTO[] GetAvailableContracts();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetAvailableContracts", ReplyAction="http://tempuri.org/IService/GetAvailableContractsResponse")]
        System.Threading.Tasks.Task<WebGymTrivelloniBattaglioli.ServiceReferenceWCF.ContractDTO[]> GetAvailableContractsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/AddContractToClient", ReplyAction="http://tempuri.org/IService/AddContractToClientResponse")]
        bool AddContractToClient(int id, string codice_fiscale, string startDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/AddContractToClient", ReplyAction="http://tempuri.org/IService/AddContractToClientResponse")]
        System.Threading.Tasks.Task<bool> AddContractToClientAsync(int id, string codice_fiscale, string startDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/CercaPersonalTrainerNelDB", ReplyAction="http://tempuri.org/IService/CercaPersonalTrainerNelDBResponse")]
        bool CercaPersonalTrainerNelDB(string email, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/CercaPersonalTrainerNelDB", ReplyAction="http://tempuri.org/IService/CercaPersonalTrainerNelDBResponse")]
        System.Threading.Tasks.Task<bool> CercaPersonalTrainerNelDBAsync(string email, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/AlreadyRegistered", ReplyAction="http://tempuri.org/IService/AlreadyRegisteredResponse")]
        bool AlreadyRegistered(string codice_fiscale, string mail);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/AlreadyRegistered", ReplyAction="http://tempuri.org/IService/AlreadyRegisteredResponse")]
        System.Threading.Tasks.Task<bool> AlreadyRegisteredAsync(string codice_fiscale, string mail);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ConvalidLogIn", ReplyAction="http://tempuri.org/IService/ConvalidLogInResponse")]
        bool ConvalidLogIn(string mail, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ConvalidLogIn", ReplyAction="http://tempuri.org/IService/ConvalidLogInResponse")]
        System.Threading.Tasks.Task<bool> ConvalidLogInAsync(string mail, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : WebGymTrivelloniBattaglioli.ServiceReferenceWCF.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<WebGymTrivelloniBattaglioli.ServiceReferenceWCF.IService>, WebGymTrivelloniBattaglioli.ServiceReferenceWCF.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType GetDataUsingDataContract(WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType composite) {
            return base.Channel.GetDataUsingDataContract(composite);
        }
        
        public System.Threading.Tasks.Task<WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType> GetDataUsingDataContractAsync(WebGymTrivelloniBattaglioli.ServiceReferenceWCF.CompositeType composite) {
            return base.Channel.GetDataUsingDataContractAsync(composite);
        }
        
        public bool InserisciCliente(string codice_fiscale, string nome, string cognome, string mail, string data_nascita, string telefono, string password, string sesso) {
            return base.Channel.InserisciCliente(codice_fiscale, nome, cognome, mail, data_nascita, telefono, password, sesso);
        }
        
        public System.Threading.Tasks.Task<bool> InserisciClienteAsync(string codice_fiscale, string nome, string cognome, string mail, string data_nascita, string telefono, string password, string sesso) {
            return base.Channel.InserisciClienteAsync(codice_fiscale, nome, cognome, mail, data_nascita, telefono, password, sesso);
        }
        
        public WebGymTrivelloniBattaglioli.ServiceReferenceWCF.ContractDTO[] GetAvailableContracts() {
            return base.Channel.GetAvailableContracts();
        }
        
        public System.Threading.Tasks.Task<WebGymTrivelloniBattaglioli.ServiceReferenceWCF.ContractDTO[]> GetAvailableContractsAsync() {
            return base.Channel.GetAvailableContractsAsync();
        }
        
        public bool AddContractToClient(int id, string codice_fiscale, string startDate) {
            return base.Channel.AddContractToClient(id, codice_fiscale, startDate);
        }
        
        public System.Threading.Tasks.Task<bool> AddContractToClientAsync(int id, string codice_fiscale, string startDate) {
            return base.Channel.AddContractToClientAsync(id, codice_fiscale, startDate);
        }
        
        public bool CercaPersonalTrainerNelDB(string email, string password) {
            return base.Channel.CercaPersonalTrainerNelDB(email, password);
        }
        
        public System.Threading.Tasks.Task<bool> CercaPersonalTrainerNelDBAsync(string email, string password) {
            return base.Channel.CercaPersonalTrainerNelDBAsync(email, password);
        }
        
        public bool AlreadyRegistered(string codice_fiscale, string mail) {
            return base.Channel.AlreadyRegistered(codice_fiscale, mail);
        }
        
        public System.Threading.Tasks.Task<bool> AlreadyRegisteredAsync(string codice_fiscale, string mail) {
            return base.Channel.AlreadyRegisteredAsync(codice_fiscale, mail);
        }
        
        public bool ConvalidLogIn(string mail, string password) {
            return base.Channel.ConvalidLogIn(mail, password);
        }
        
        public System.Threading.Tasks.Task<bool> ConvalidLogInAsync(string mail, string password) {
            return base.Channel.ConvalidLogInAsync(mail, password);
        }
    }
}
