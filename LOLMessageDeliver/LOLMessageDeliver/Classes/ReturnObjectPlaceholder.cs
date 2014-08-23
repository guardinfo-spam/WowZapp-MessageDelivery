using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LOLMessageDelivery;
using System.Runtime.Serialization;

namespace LOLMessageDelivery.Classes
{
    [DataContractAttribute]
    public class Comicon
    {
        public string SomeProperty { get; set; }
    }

    [DataContractAttribute]
    public class Animation
    {
        public object SomeData { get; set; }
    }

    [DataContractAttribute]
    public class ReturnObjectPlaceholder
    {
        public Comicon ComiconObject { get; set; }
        public Animation AnimationObject { get; set; }        

        public ReturnObjectPlaceholder(Guid StepID)
        {
            //populate the object, get the data from database and assign it to a proper StrongTyped object
            this.AnimationObject = new Animation();
            this.AnimationObject.SomeData = "this is some data";
            this.ComiconObject = null;                        

        }
    }
}