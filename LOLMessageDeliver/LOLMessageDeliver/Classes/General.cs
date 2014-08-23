using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LOLCodeLibrary.ErrorsManagement;

namespace LOLMessageDelivery
{
    [DataContract]
    public class General
    {
        [DataContract]
        public struct Error
        {
            [DataMember]
            public string ErrorNumber { get; set; }
            [DataMember]
            public string ErrorTitle { get; set; }
            [DataMember]
            public string ErrorDescription { get; set; }
            [DataMember]
            public string ErrorLocation { get; set; }
            [DataMember]
            public SystemTypes.ErrorMessage ErrorType { get; set; }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Title: " + this.ErrorTitle)
                    .Append(". Number: " + this.ErrorNumber)
                    .Append(". Description: " + this.ErrorDescription)
                    .Append(". Location: " + this.ErrorLocation);

                return sb.ToString();
            }
        }

    }
}
