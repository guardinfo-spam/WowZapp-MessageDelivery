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
    public sealed class Test_MessageStepDataSave : TestBase, ITestable
    {
        #region ITestable
        public LOLConnect2.LOLMessageClient _ws { get; set; }
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            MessageStepDataSave_ValidInput_ShouldSucceed();
            MessageStepDataSave_TokenNotAuthenticated_ShouldFail();
            MessageStepDataSave_AccountIdNotLinkedToToken_ShouldFail();
            MessageStepDataSave_TokenExpired_ShouldFail();
            MessageStepDataSave_TokenLoggedOut_ShouldFail();
            MessageStepDataSave_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_MessageStepDataSave(LOLConnect2.LOLMessageClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void MessageStepDataSave_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageStepDataSave_TokenNotAuthenticated_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageStepDataSave_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageStepDataSave_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageStepDataSave_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageStepDataSave_TokenLoggedOut_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageStepDataSave_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageStepDataSave_TokenNotInDatabase_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageStepDataSave_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageStepDataSave_AccountIdNotLinkedToToken_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageStepDataSave_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing MessageStepDataSave_ValidInput_ShouldSucceed ...", true);
            LOLMessageDelivery.Message message = new LOLMessageDelivery.Message();

            string WavFilePath = @"J:\test.wav";
            System.IO.FileStream fs = System.IO.File.OpenRead(WavFilePath);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            fs.Close();

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

            LOLMessageDelivery.Message msgResult = _ws.MessageCreate(message, steps, toAccounts, Guid.NewGuid());

            var elapsed = Stopwatch.StartNew();
            var messageSaved = _ws.MessageStepDataSave(msgResult.MessageID, 1, bytes, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (string.IsNullOrEmpty(messageSaved.ErrorNumber))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);


            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
