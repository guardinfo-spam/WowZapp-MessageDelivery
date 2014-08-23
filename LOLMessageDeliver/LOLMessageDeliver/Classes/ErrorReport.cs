using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LOLMessageDelivery.Classes
{
    [Serializable, DataContract]
    public class ErrorReport
    {

        [DataMember]
        public Guid ErrorReportId { get; set; }

        [DataMember]
        public string ApplicationName { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
        public string CommandLine { get; set; }

        [DataMember]
        public string OsVersion { get; set; }

        [DataMember]
        public string SystemUserName { get; set; }

        [DataMember]
        public string ClrVersion { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public List<string> LoadedAssemblies { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public string ExceptionType { get; set; }

        [DataMember]
        public string SubSystem { get; set; }

    }
}