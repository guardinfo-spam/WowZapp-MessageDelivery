using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;
using LOLMessageDelivery.Classes;
using LOLMessageDelivery;

namespace ConsoleApplication1
{
    public sealed class Test_MessageGetNew : TestBase, ITestable
    {
        #region ITestable
        public LOLConnect2.LOLMessageClient _ws { get; set; }
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            MessageGetNew_ValidInput_ShouldSucceed();
            MessageGetNew_TokenNotAuthenticated_ShouldFail();
            MessageGetNew_AccountIdNotLinkedToToken_ShouldFail();
            MessageGetNew_TokenExpired_ShouldFail();
            MessageGetNew_TokenLoggedOut_ShouldFail();
            MessageGetNew_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_MessageGetNew(LOLConnect2.LOLMessageClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void MessageGetNew_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageGetNew_TokenNotAuthenticated_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageGetNew_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageGetNew_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageGetNew_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageGetNew_TokenLoggedOut_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageGetNew_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageGetNew_TokenNotInDatabase_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageGetNew_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageGetNew_AccountIdNotLinkedToToken_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageGetNew_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing MessageGetNew_ValidInput_ShouldSucceed ...", true);
            LOLMessageDelivery.Message message = new LOLMessageDelivery.Message();

            var results = this._ws.MessageGetNew(new Guid("986573A3-C97D-4C18-939D-0EE77F3CCF22"), "358150048080940", null, Guid.NewGuid());
            int i = 1;

            //Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            //message.FromAccountID = this.GenericFromAccountID;

            //LOLConnect2.MessageStep tmpStep = new LOLConnect2.MessageStep();
            //List<LOLConnect2.MessageStep> steps = new List<LOLConnect2.MessageStep>();

            //tmpStep.MessageText = "This is a test message";
            //tmpStep.StepNumber = 1;
            //tmpStep.StepType = LOLConnect2.MessageStep.StepTypes.Text;

            //steps.Add(tmpStep);

            //tmpStep = new LOLConnect2.MessageStep();
            //tmpStep.StepNumber = 2;
            //tmpStep.StepType = LOLConnect2.MessageStep.StepTypes.Comix;
            //tmpStep.ContentPackItemID = 6;

            //steps.Add(tmpStep);

            //tmpStep = new LOLConnect2.MessageStep();
            //tmpStep.StepNumber = 3;
            //tmpStep.StepType = LOLConnect2.MessageStep.StepTypes.Polling;

            //steps.Add(tmpStep);

            //List<Guid> toAccounts = new List<Guid>();
            //toAccounts.Add(this.GenericToAccountID);

            //var elapsed = Stopwatch.StartNew();
            //var messageSaved = _ws.MessageGetNew(message, steps, toAccounts, Guid.NewGuid());
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //if (messageSaved.Errors.Count == 0 && !messageSaved.MessageID.Equals(Guid.Empty))
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);


            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
