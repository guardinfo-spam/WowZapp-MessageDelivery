using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;

namespace LOLMessageDelivery.Classes
{
    [DataContractAttribute]
    public class ReturnObjectAsJson
    {
        public string SomeData { get; set; }

        public ReturnObjectAsJson(Guid stepID)
        {
            this.SomeData = "hello there";
        }
    }

    public static class JSonHelper
    {
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string result = serializer.Serialize(obj);
            return result;
        }
    }
}