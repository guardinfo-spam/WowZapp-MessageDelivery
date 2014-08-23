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
    public sealed class Test_MessageCreate : TestBase, ITestable
    {
        //MessageCreate(Guid AccountID, LOLConnect.AccountOAuth.OAuthTypes, string OAuthID, string OAuthType, Guid token)
        //we are testing the following scenarios :
        //1. pass a token which is not authenticated yet        - should return AuthenticationTokenNotLoggedIn
        //2. pass a token which has expired                     - should return AuthenticationTokenExpired
        //3. pass a token which is already logged out           - should return AuthenticationTokenLoggedOut
        //4. pass a token which does not exist in the database  - should return AuthenticationTokenNotFound
        //5. pass an accountID which is not linked to the token - should return AuthenticationTokenDoesNotMatchAccountID
        //6. pass valid data - should return valid object and no errors

        #region ITestable
        public LOLConnect2.LOLMessageClient _ws { get; set; }
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.MessageCreate_MessageStepsNull_ShouldFail();
            this.MessageCreate_MessageStepsHasLengthZero_ShouldFail();
            this.MessageCreate_ToAccountIdHasLengthZero_ShouldFail();
            this.MessageCreate_ToAccountIdIsNull_ShouldFail();
            this.MessageCreate_ValidInput_ShouldSucceed();
            this.MessageCreate_TokenNotAuthenticated_ShouldFail();
            this.MessageCreate_AccountIdNotLinkedToToken_ShouldFail();
            this.MessageCreate_TokenExpired_ShouldFail();
            this.MessageCreate_TokenLoggedOut_ShouldFail();
            this.MessageCreate_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_MessageCreate(LOLConnect2.LOLMessageClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void MessageCreate_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_TokenNotAuthenticated_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_TokenLoggedOut_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_TokenNotInDatabase_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_AccountIdNotLinkedToToken_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing MessageCreate_ValidInput_ShouldSucceed ...", true);
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

            var elapsed = Stopwatch.StartNew();
            var messageSaved = _ws.MessageCreate(message, steps, toAccounts, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (messageSaved.Errors.Count == 0 && !messageSaved.MessageID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);


            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_MessageStepsNull_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_StepsMissing_ShouldFail ...", true);

            LOLMessageDelivery.Message message = new LOLMessageDelivery.Message();
            
            message.FromAccountID = this.GenericFromAccountID;

            var elapsed = Stopwatch.StartNew();
            var messageSaved = _ws.MessageCreate(message, null, null, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (messageSaved.Errors.Count == 1 && !messageSaved.Errors[0].Equals(SystemTypes.ErrorMessage.MessageStepsMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_MessageStepsHasLengthZero_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_MessageStepsHasLengthZero_ShouldFail ...", true);

            LOLMessageDelivery.Message message = new LOLMessageDelivery.Message();

            message.FromAccountID = this.GenericFromAccountID;

            var elapsed = Stopwatch.StartNew();
            var messageSaved = _ws.MessageCreate(message, new List<LOLMessageDelivery.MessageStep>(), null, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (messageSaved.Errors.Count == 1 && !messageSaved.Errors[0].Equals(SystemTypes.ErrorMessage.MessageStepsMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_ToAccountIdIsNull_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_ToAccountIdIsNull_ShouldFail ...", true);
            LOLMessageDelivery.Message message = new LOLMessageDelivery.Message();

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

            var elapsed = Stopwatch.StartNew();
            var messageSaved = _ws.MessageCreate(message, steps, null, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (messageSaved.Errors.Count == 1 && messageSaved.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ToAccountListEmpty.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);


            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void MessageCreate_ToAccountIdHasLengthZero_ShouldFail()
        {
            this.Logger.LogMessage("Testing MessageCreate_ToAccountIdIsNull_ShouldFail ...", true);
            LOLMessageDelivery.Message message = new LOLMessageDelivery.Message();

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

            var elapsed = Stopwatch.StartNew();
            var messageSaved = _ws.MessageCreate(message, steps, new List<Guid>(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (messageSaved.Errors.Count == 1 && messageSaved.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ToAccountListEmpty.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);


            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
