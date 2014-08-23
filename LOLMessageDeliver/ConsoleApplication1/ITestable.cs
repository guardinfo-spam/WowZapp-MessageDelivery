using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLCodeLibrary.LoggingSystem;

namespace ConsoleApplication1
{
    public interface ITestable
    {
        LOLConnect2.LOLMessageClient _ws { get; set; }
        ILogger Logger { get; set; }        
    }
}
