using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOLMessageDelivery.Classes
{
    public class MethodParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MethodParameter()
        {
            this.Name = string.Empty;
            this.Value = string.Empty;
        }

        public MethodParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class MethodParameters
    {
        public List<MethodParameter> ParametersList { get; set; }

        public MethodParameters()
        {
            this.ParametersList = new List<MethodParameter>();
            //this.ParametersList = new List<MethodParameter>();
            //this.ParametersList.Add(new MethodParameter("DeviceID", deviceID));
        }       
    }
}