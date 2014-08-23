using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using LOLCodeLibrary.LoggingSystem;

namespace ConsoleApplication1
{
    public abstract class TestBase
    {
        /// <summary>
        /// Andrei Me
        /// </summary>
        public Guid GenericFromAccountID = new Guid("87298106-7344-4676-97AA-00BA342BA650");
        public string GenericFromDeviceID = "02-73-CB-C0-C3-59-01";

        /// <summary>
        /// Andrei Test
        /// </summary>
        public Guid GenericToAccountID = new Guid("186194FF-3CF1-4E7D-93B8-54A23B8BBE6B");
        public string GenericFromDeviceID2 = "02-73-CB-C0-C3-59-02";

        public virtual string ImageFilePath { get; set; }
        public virtual string RandomFirstName { get; set; }
        public virtual string RandomLastName { get; set; }
        public virtual string RandomPassword { get; set; }
        public virtual string RandomOAuthID { get; set; }
        public virtual string RandomOAuthToken { get; set; }

        public virtual string RandomDeviceID { get; set; }
        /// <summary>
        /// used when 2 users need to be created by the same Test method
        /// </summary>
        public virtual string RandomSecondaryDeviceID { get; set; }

        public virtual string TestSuccessMessage { get; set; }
        public virtual string TestFailMessage { get; set; }

        public virtual string Delimiter { get; set; }

        /// <summary>
        /// base constructor setting the default values to be used by all Test classes
        /// </summary>
        public TestBase()
        {
            this.ImageFilePath    = @"E:\Andi-Small.jpg";
            this.RandomFirstName  = "RandomFName";
            this.RandomLastName   = "RandomLName";
            this.RandomPassword   = "Satori-1155";
            this.RandomOAuthID    = "1234567890";
            this.RandomOAuthToken = "0987654321";
            this.RandomDeviceID   = "12345678-1234-1234-1234-123456789012";
            this.RandomSecondaryDeviceID = "12345678-1234-1234-1234-123456789013";

            this.TestSuccessMessage = "Test Succesfull !";
            this.TestFailMessage    = "Test Failed !";

            this.Delimiter = "--------------------------------------";
        }

        /// <summary>
        /// generates a random email address to be used by functions like UserCreate.
        /// </summary>
        /// <returns></returns>
        public virtual string GetRandomEmail()
        {
            return Guid.NewGuid().ToString() + "@random.com";
        }

        /// <summary>
        /// formats the Elapsed time output for use by all Test classes. This is the only place that needs to be changed should the format require changing
        /// </summary>
        /// <param name="swObject"></param>
        /// <returns></returns>
        public virtual string PrepareElapsedTimeOutput(Stopwatch swObject)
        {
            return "Execution time was - hours :" + swObject.Elapsed.Hours + ", minutes :" + swObject.Elapsed.Minutes + ", seconds :" + swObject.Elapsed.Seconds + ", milliseconds :" + swObject.ElapsedMilliseconds;
        }

        /// <summary>
        /// override in the implementing class and call the specific test methods required
        /// </summary>
        public virtual void RunTests()
        {
        }

        public virtual void CleanAfterTest(LOLConnect2.LOLMessageClient ws)
        {
            ws.DeleteTestData();
        }
    }
}
