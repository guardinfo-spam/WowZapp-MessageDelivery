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
    public sealed class Test_MessageMarkRead : TestBase, ITestable
    {
        #region ITestable
        public LOLConnect2.LOLMessageClient _ws { get; set; }
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            MessageMarkRead_ValidInput_ShouldSucceed();
            MessageMarkRead_TokenNotAuthenticated_ShouldFail();
            MessageMarkRead_AccountIdNotLinkedToToken_ShouldFail();
            MessageMarkRead_TokenExpired_ShouldFail();
            MessageMarkRead_TokenLoggedOut_ShouldFail();
            MessageMarkRead_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_MessageMarkRead(LOLConnect2.LOLMessageClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void MessageMarkRead_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageMarkRead_TokenNotAuthenticated_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageMarkRead_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageMarkRead_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageMarkRead_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageMarkRead_TokenLoggedOut_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageMarkRead_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageMarkRead_TokenNotInDatabase_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageMarkRead_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageMarkRead_AccountIdNotLinkedToToken_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageMarkRead_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing MessageMarkRead_ValidInput_ShouldSucceed ...", true);
            
            LOLMessageDelivery.Message message = new LOLMessageDelivery.Message();
            
            //Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
                        
            message.FromAccountID = this.GenericFromAccountID;

            LOLMessageDelivery.MessageStep tmpStep = new LOLMessageDelivery.MessageStep();
            List<LOLMessageDelivery.MessageStep> steps = new List<LOLMessageDelivery.MessageStep>();

            tmpStep.MessageText = "This is a test message";
            tmpStep.StepNumber = 1;
            tmpStep.StepType = LOLMessageDelivery.MessageStep.StepTypes.Text;

            steps.Add(tmpStep);

            tmpStep = new LOLMessageDelivery.MessageStep();
            tmpStep.StepNumber = 2;
            tmpStep.StepType = LOLMessageDelivery.MessageStep.StepTypes.Comix;
            tmpStep.ContentPackItemID = 6;

            steps.Add(tmpStep);

            tmpStep = new LOLMessageDelivery.MessageStep();
            tmpStep.StepNumber = 3;
            tmpStep.StepType = LOLMessageDelivery.MessageStep.StepTypes.Polling;

            steps.Add(tmpStep);

            List<Guid> toAccounts = new List<Guid>();
            toAccounts.Add(this.GenericToAccountID);

            var messageSaved = _ws.MessageCreate(message, steps, toAccounts, Guid.NewGuid());

            var elapsed = Stopwatch.StartNew();
            //var readError =  _ws.MessageMarkRead(messageSaved.MessageID, this.GenericToAccountID, this.GenericFromDeviceID, new Guid());
            LOLMessageDelivery.General.Error readError = _ws.MessageMarkRead(new Guid("4afd7da6-5dde-4b49-aef1-64c9475788de"), new Guid("d01ecc3c-c460-4bc3-8b49-3dfd39054bfa"), "06-CE-8F-2D-71-3E", new Guid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);
            
            if (string.IsNullOrEmpty(readError.ErrorNumber))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);


            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
