using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Helpers
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Fault", Namespace = "http://schemas.microsoft.com/dynamics/2013/01/datacontracts")]
    [System.SerializableAttribute()]
    public partial class Fault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ExceptionMessageField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ExceptionTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid RequestIdField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExceptionMessage
        {
            get
            {
                return this.ExceptionMessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ExceptionMessageField, value) != true))
                {
                    this.ExceptionMessageField = value;
                    this.RaisePropertyChanged("ExceptionMessage");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExceptionType
        {
            get
            {
                return this.ExceptionTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ExceptionTypeField, value) != true))
                {
                    this.ExceptionTypeField = value;
                    this.RaisePropertyChanged("ExceptionType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid RequestId
        {
            get
            {
                return this.RequestIdField;
            }
            set
            {
                if ((this.RequestIdField.Equals(value) != true))
                {
                    this.RequestIdField = value;
                    this.RaisePropertyChanged("RequestId");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
