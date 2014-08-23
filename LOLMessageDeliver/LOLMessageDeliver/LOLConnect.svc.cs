using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using LOLMessageDelivery.Classes;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.ServiceModel.Channels;
using System.Drawing;
using System.Web.Hosting;
using LOLCodeLibrary.ErrorsManagement;
using LOLCodeLibrary.DataConversion;
using LOLMessageDelivery.Classes.ErrorsMgmt;
using LOLCodeLibrary;

namespace LOLMessageDelivery
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
     Guid("23618A5F-A903-4612-96F7-E28F962CE303")]
    public class LOLMessage
    {
        DataTools _dt = new DataTools();


        #region AuthenticationToken

        //[OperationContract]
        private Guid AuthenticationTokenGet(string deviceID)
        {
            AuthenticationToken tmpReturn = new AuthenticationToken();
            tmpReturn.AuthenticationCreatedDate = DateTime.Now;
            tmpReturn.AuthenticationDeviceID = deviceID;

            OperationContext context = OperationContext.Current;

            MessageProperties messageProperties = context.IncomingMessageProperties;

            RemoteEndpointMessageProperty endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            tmpReturn.AuthenticationIPAddress = endpointProperty.Address;
            tmpReturn.Save();

            return tmpReturn.AuthenticationTokenID;
        }

        private bool AuthenticationTokenValidate(Guid authenticationToken)
        {
            return true;
        }

        #endregion AuthenticationToken

        #region Messages

        [OperationContract]
        public Message MessageCreate(Message message, List<MessageStep> messageSteps, List<Guid> toAccountIDs, Guid AuthenticationToken)
        {
            LolMessagesRepository repo = new Classes.LolMessagesRepository(this._dt);
            General.Error tmpError = new General.Error();

            List<MethodParameter> parameters = new List<Classes.MethodParameter>();
            parameters.Add(new Classes.MethodParameter("MessageData", GenericFunctionality.ToXML(message)));
            parameters.Add(new Classes.MethodParameter("MessageStepsData", GenericFunctionality.ToXML(messageSteps)));
            parameters.Add(new Classes.MethodParameter("ToAccountList", GenericFunctionality.ToXML(toAccountIDs)));
            parameters.Add(new Classes.MethodParameter("AuthenticationToken", AuthenticationToken.ToString()));
            
            if ( message.Errors == null )
                message.Errors = new List<General.Error>();

            if (messageSteps == null || messageSteps.Count == 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.MessageStepsMissing, "MessageCreate");
                message.Errors.Add(tmpError);
                repo.LogError(parameters, "MessageCreate", tmpError);
                return message;
            }

            if (toAccountIDs == null || toAccountIDs.Count == 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ToAccountListEmpty, "MessageCreate");
                message.Errors.Add(tmpError);
                repo.LogError(parameters, "MessageCreate", tmpError);
                return message;
            }

            bool foundEmptyGuid = false;
            for (int i = 0; i < toAccountIDs.Count; i++)
            {
                if (toAccountIDs[i].Equals(Guid.Empty))
                {
                    foundEmptyGuid = true;
                    break;
                }
            }

            if (foundEmptyGuid)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ToAccountIdIsEmptyGuid, "MessageCreate");
                message.Errors.Add(tmpError);
                repo.LogError(parameters, "MessageCreate", tmpError);
                return message;
            }

            message.Save();

            if (message.Errors.Count > 0)
            {
                repo.LogError(parameters, "MessageCreate", message.Errors[0]);
                return message;
            }

            foreach (MessageStep ms in messageSteps)
            {
                ms.MessageID = message.MessageID;
                ms.Save();
                if (ms.Errors != null && ms.Errors.Count > 0)
                {
                    repo.LogError(parameters, "MessageCreate_MessageStepSave", ms.Errors[0]);
                }
            }            

            foreach (Guid toAccount in toAccountIDs)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO MessageDelivery (MessageID, AccountID, DeviceID, Delivered) SELECT @MessageID, @AccountID, DeviceID, 0 FROM LOLAccounts.dbo.Devices WHERE AccountID = @AccountID";
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = message.MessageID;
                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = toAccount;

                try
                {
                    int result = _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
                }
                catch(Exception ex)
                {
                    General.Error error = new General.Error();
                    error.ErrorDescription = ex.Message;
                    repo.LogError(parameters, "MessageCreate_MessageDeliveryItem", error);
                }
            }

            message = new Message(message.MessageID);

            return message;
        }

        [OperationContract]
        public bool MessageConfirmSend(Guid messageID, Guid accountID, Guid authenticationToken)
        {

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Messages SET MessageConfirmed = 1 WHERE MessageID = @MessageID";
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;

                _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
                return true;
            }
            catch
            {
                return false;
            }

        }

        [OperationContract]
        public List<Guid> MessageGetReplyAll(Guid messageID, Guid authenticationToken)
        {

            List<Guid> tmpReturn = new List<Guid>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT DISTINCT AccountID FROM MessageDelivery WHERE MessageID = @MessageID";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tmpReturn.Add((Guid)dr["AccountID"]);
            }

            return tmpReturn;

        }

        [OperationContract]
        public List<Message> MessageGetNew(Guid accountID, string deviceID, List<Guid> excludeMessages, Guid AuthenticationToken)
        {

            List<Message> tmpReturn = new List<Message>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT DISTINCT * FROM Messages (nolock) WHERE MessageConfirmed = 1 AND MessageID IN (SELECT MessageID FROM MessageDelivery (nolock) WHERE AccountID = @AccountID GROUP BY MessageID HAVING MAX(CONVERT(int,Delivered)) = 0) AND FromAccountID <> @AccountID ORDER BY MessageSent DESC";
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;
            cmd.Parameters.Add("@DeviceID", SqlDbType.NVarChar, 100).Value = deviceID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool add = true;
                if (excludeMessages != null)
                {
                    foreach (Guid messageid in excludeMessages)
                    {
                        if ((Guid)dr["MessageID"] == messageid)
                        {
                            add = false;
                            break;
                        }
                    }
                }

                if(add)
                    tmpReturn.Add(new Message(dr));
            }

            return tmpReturn;
        }

        [OperationContract]
        public List<MessageStep> MessageGetSteps(Guid messageID, Guid AuthenticationToken)
        {

            List<MessageStep> tmpReturn = new List<MessageStep>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM MessageSteps (nolock) WHERE MessageID = @MessageID ORDER BY StepNumber";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tmpReturn.Add(new MessageStep(dr));
            }

            return tmpReturn;
        }


        [OperationContract]
        public General.Error MessageMarkRead(Guid messageID, Guid accountID, string deviceID, Guid AuthenticationToken)
        {
            SqlCommand cmd = new SqlCommand();
            General.Error error = new General.Error();
            cmd.CommandText = "UPDATE MessageDelivery SET Delivered = 1 WHERE MessageID = @MessageID ANd AccountID = @AccountID AND DeviceID = @DeviceID";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;
            cmd.Parameters.Add("@DeviceID", SqlDbType.NVarChar,100).Value = deviceID;

            int rowsAffected = _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
            if (rowsAffected != 1)
                error = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.CouldNotIdentifyRow, "MessageMarkRead");
            else
                error = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.NoErrorDetected, "");
            return error;
        }

        [OperationContract]
        public List<Message> MessageGetFrom(Guid toAccountID, Guid fromAccountID, DateTime startDate, DateTime endDate, int maxMessages, List<Guid> excludeMessages, Guid AuthenticationToken)
        {

            List<Message> tmpReturn = new List<Message>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT DISTINCT * FROM Messages (nolock) WHERE MessageConfirmed = 1 AND ((FromAccountID = @FromAccountID AND MessageSent BETWEEN @StartDate AND @EndDate AND MessageID IN (SELECT MessageID FROM MessageDelivery (nolock) WHERE AccountID = @ToAccountID)) OR (FromAccountID = @ToAccountID AND MessageSent BETWEEN @StartDate AND @EndDate AND MessageID IN (SELECT MessageID FROM MessageDelivery (nolock) WHERE AccountID = @FromAccountID))) ORDER BY MessageSent DESC";
            cmd.Parameters.Add("@FromAccountID", SqlDbType.UniqueIdentifier).Value = fromAccountID;
            cmd.Parameters.Add("@ToAccountID", SqlDbType.UniqueIdentifier).Value = toAccountID;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate;


            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool add = true;
                if (excludeMessages != null)
                {
                    foreach (Guid messageid in excludeMessages)
                    {
                        if ((Guid)dr["MessageID"] == messageid)
                        {
                            add = false;
                            break;
                        }
                    }
                }

                if (add)
                    tmpReturn.Add(new Message(dr));
            }

            if (tmpReturn.Count > maxMessages)
            {
                tmpReturn.RemoveRange(maxMessages, tmpReturn.Count - maxMessages);
            }



            return tmpReturn;
        }

        [OperationContract]
        public List<Message> MessageGetConversations(Guid accountID, DateTime startDate, DateTime endDate, int maxMessages, List<Guid> excludeMessages, Guid AuthenticationToken)
        {

            List<Message> tmpReturn = new List<Message>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT DISTINCT * FROM Messages (nolock) WHERE MessageConfirmed = 1 AND ((FromAccountID = @AccountID AND MessageSent BETWEEN @StartDate AND @EndDate) OR (MessageSent BETWEEN @StartDate AND @EndDate AND MessageID IN (SELECT MessageID FROM MessageDelivery (nolock) WHERE AccountID = @AccountID))) ORDER BY MessageSent DESC";
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate;


            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool add = true;
                if (excludeMessages != null)
                {
                    foreach (Guid messageid in excludeMessages)
                    {
                        if ((Guid)dr["MessageID"] == messageid)
                        {
                            add = false;
                            break;
                        }
                    }
                }

                if (add)
                    tmpReturn.Add(new Message(dr));
            }

            if (tmpReturn.Count > maxMessages)
            {
                tmpReturn.RemoveRange(maxMessages, tmpReturn.Count - maxMessages);
            }



            return tmpReturn;
        }



        [OperationContract]
        public byte[] MessageGetStepData(Guid messageID, int stepNumber, Guid AuthenticationToken)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM MessageStepData (nolock) WHERE  MessageID = @MessageID AND StepNumber = @StepNumber";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;
            cmd.Parameters.Add("@StepNumber", SqlDbType.Int).Value = stepNumber;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return (byte[])ds.Tables[0].Rows[0]["StepData"];
            }
            else
                return new byte[0];
        }

        [OperationContract]
        public General.Error MessageStepDataSave(Guid messageID, int stepNumber, byte[] messageStepData, Guid authenticationToken)
        {

            General.Error tmpReturn = new General.Error();
            tmpReturn = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.NoErrorDetected, "");
            tmpReturn.ErrorDescription = "";
            LolMessagesRepository repo = new LolMessagesRepository(this._dt);
            repo.MethodName = "MessageStepDataSave";
            repo.LastErrorMessage = "nothing yet";
            List<MethodParameter> mps = new List<MethodParameter>();
            mps.Add(new MethodParameter("MessageID", messageID.ToString()));
            mps.Add(new MethodParameter("StepNumber", stepNumber.ToString()));
            mps.Add(new MethodParameter("MessageStepDataCount", messageStepData.Count().ToString()));
            //repo.LogError(mps, "MessageStepDataSave", tmpReturn);

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT MessageID FROM MessageStepData WHERE MessageID = @MessageID AND StepNumber = @StepNumber";
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;
                cmd.Parameters.Add("@StepNumber", SqlDbType.Int).Value = stepNumber;

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmd.CommandText = "UPDATE MessageStepData SET StepData = @StepData, StepProcessed = 0 WHERE MessageID = @MessageID AND StepNumber = @StepNumber";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO MessageStepData (MessageID, StepNumber, StepData, StepProcessed) VALUES (@MessageID, @StepNumber, @StepData, 0)";
                }

                WavToMp3Conversion wc = new WavToMp3Conversion(messageStepData, Guid.NewGuid() + ".wav", Guid.NewGuid() + ".mp3", "SoundDecoder", "lame.exe");
                byte[] processedData = wc.RunMainBody(HostingEnvironment.ApplicationPhysicalPath, Directory.GetCurrentDirectory());

                cmd.Parameters.Add("@StepData", SqlDbType.VarBinary).Value = processedData;
                _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

                return tmpReturn;

            }
            catch (Exception ex)
            {
                tmpReturn.ErrorDescription = ex.Message;
                tmpReturn.ErrorLocation = "MessageStepDataSave";
                tmpReturn.ErrorNumber = "1";
                tmpReturn.ErrorTitle = "Unable to save message data";

                repo.LogError(mps, "MessageStepDataSave", tmpReturn);

                return tmpReturn;
            }
        }

        [OperationContract]
        public List<Guid> MessagesGetListSentToUser(Guid authenticationToken, Guid accountID)
        {
            List<Guid> results = new List<Guid>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT MessageID FROM MessageDelivery (nolock) WHERE AccountID = @AccountID AND MessageID NOT IN (SELECT * FROM Messages WHERE MessageConfirmed = 0)";
            
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;
            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)                
                    results.Add(new Guid(dr["MessageID"].ToString()));
            }

            return results;

        }

        #endregion Messages


        #region PollingMessages

        [OperationContract]
        public PollingStep PollingStepGet(Guid messageID, int stepNumber, Guid accountID, Guid authenticationToken)
        {

            PollingStep tmpReturn = new PollingStep();
            tmpReturn = new PollingStep(messageID, stepNumber);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT StepID FROM MessageSteps WHERE MessageID = @MessageID and StepNumber = @StepNumber";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;
            cmd.Parameters.Add("@StepNumber", SqlDbType.Int).Value = stepNumber;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            if(ds.Tables[0].Rows.Count > 0)
            {
                tmpReturn.HasResponded = PollingStepHasResponded((Guid)ds.Tables[0].Rows[0]["StepID"], accountID, authenticationToken);
            }

            return tmpReturn;
        }

        [OperationContract]
        public List<General.Error> PollingStepSave(PollingStep pollingStep, Guid authenticationToken)
        {
            pollingStep.Save();
            return pollingStep.Errors;
        }

        [OperationContract]
        public General.Error PollingStepResponse(Guid stepID, Guid accountID, int responseNumber, Guid authenticationToken)
        {

            General.Error tmpReturn = new General.Error();
            tmpReturn = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.NoErrorDetected, "");

            if (!PollingStepHasResponded(stepID, accountID, authenticationToken))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO PollingResponses (StepID, AccountID, Response) VALUES (@StepID, @AccountID, @Response);";
                    cmd.Parameters.Add("@StepID", SqlDbType.UniqueIdentifier).Value = stepID;
                    cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;
                    cmd.Parameters.Add("@Response", SqlDbType.Int).Value = responseNumber;

                    _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

                }
                catch(Exception ex)
                {
                    tmpReturn.ErrorDescription = ex.Message;
                    tmpReturn.ErrorLocation = "PollingStepResponse";
                    tmpReturn.ErrorNumber = "1";
                    tmpReturn.ErrorTitle = "Unable to save response";
                }
            }

            return tmpReturn;

        }

        [OperationContract]
        public bool PollingStepHasResponded (Guid stepID, Guid accountID, Guid authenticationToken)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT StepID FROM PollingResponses WHERE StepID = @StepID AND AccountID = @AccountID";
            cmd.Parameters.Add("@StepID", SqlDbType.UniqueIdentifier).Value = stepID;
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

            DataSet ds = _dt.GetDataSet(cmd,DataTools.DataSources.LOLMessageDelivery);
            if(ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [DataContract]
        public struct SurveyResult
        {
            [DataMember]
            public Guid StepID { get; set; }

            [DataMember]
            public int StepNumber { get; set; }

            [DataMember]
            public decimal Answer1Percent { get; set; }

            [DataMember]
            public int Answer1Count { get; set; }

            [DataMember]
            public decimal Answer2Percent { get; set; }

            [DataMember]
            public int Answer2Count { get; set; }

            [DataMember]
            public decimal Answer3Percent { get; set; }

            [DataMember]
            public int Answer3Count { get; set; }

            [DataMember]
            public decimal Answer4Percent { get; set; }

            [DataMember]
            public int Answer4Count { get; set; }

            [DataMember]
            public int Responses { get; set; }
        }



        [OperationContract]
        public SurveyResult PollingStepGetResults(Guid stepID, Guid authenticationToken)
        {

            //Used a non-traditional way to get count, instead of putting burden on SQL Server, moved
            //it to web server.  Web servers are cheaper and can take the additional load easier.

            SurveyResult tmpReturn = new SurveyResult();
            tmpReturn.Answer1Count = 0;
            tmpReturn.Answer1Percent = 0;
            tmpReturn.Answer2Count = 0;
            tmpReturn.Answer2Percent = 0;
            tmpReturn.Answer3Count = 0;
            tmpReturn.Answer3Percent = 0;
            tmpReturn.Answer4Count = 0;
            tmpReturn.Answer4Percent = 0;
            tmpReturn.Responses = 0;
            tmpReturn.StepID = stepID;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT Response FROM PollingResponses WHERE StepID = @StepID";
            cmd.Parameters.Add("@StepID", SqlDbType.UniqueIdentifier).Value = stepID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch((int)dr["Response"])
                {
                    case 1:
                        tmpReturn.Answer1Count++;
                        break;
                    case 2:
                        tmpReturn.Answer2Count++;
                        break;
                    case 3:
                        tmpReturn.Answer3Count++;
                        break;
                    case 4:
                        tmpReturn.Answer4Count++;
                        break;
                }
            }

            tmpReturn.Responses = ds.Tables[0].Rows.Count;

            if(tmpReturn.Responses >0)
            {
                if(tmpReturn.Answer1Count > 0)
                    tmpReturn.Answer1Percent = (decimal)((decimal)tmpReturn.Responses / (decimal)100) * (decimal)tmpReturn.Answer1Count;

                if(tmpReturn.Answer2Count > 0)
                    tmpReturn.Answer2Percent = (decimal)((decimal)tmpReturn.Responses / (decimal)100) * (decimal)tmpReturn.Answer2Count;

                if(tmpReturn.Answer3Count > 0)
                    tmpReturn.Answer3Percent = (decimal)((decimal)tmpReturn.Responses / (decimal)100) * (decimal)tmpReturn.Answer3Count;

                if(tmpReturn.Answer4Count > 0)
                    tmpReturn.Answer4Percent = (decimal)((decimal)tmpReturn.Responses / (decimal)100) * (decimal)tmpReturn.Answer4Count;

            }

            cmd.CommandText = "SELECT StepNumber FROM MessageSteps WHERE StepID = @StepID";

            tmpReturn.StepNumber = (int)_dt.ExecuteScalar(cmd, DataTools.DataSources.LOLMessageDelivery);

            return tmpReturn;

        }

        [OperationContract]
        public List<SurveyResult> PollingStepGetResultsList(Guid messageID, Guid authenticationToken)
        {

            List<SurveyResult> tmpReturn = new List<SurveyResult>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM MessageSteps WHERE MessageID = @MessageID AND StepType = 6";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = messageID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tmpReturn.Add(PollingStepGetResults((Guid)dr["StepID"], authenticationToken));
            }

            return tmpReturn;

        }



        #endregion PollingMessages



        [OperationContract]
        public void DeleteTestData()
        {
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "DELETE FROM AuthenticationTokens WHERE AuthenticationDeviceID = '12345678-1234-1234-1234-123456789012'";
            //_dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

            // Added by AD 18/07/2012
            cmd.CommandText = "DELETE FROM PollingSteps WHERE MessageID in ( SELECT MessageID from Messages m where m.FromAccountID = '87298106-7344-4676-97AA-00BA342BA650')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

            cmd.CommandText = "DELETE FROM PollingResponses WHERE StepID in ( SELECT StepID from MessageSteps ms where ms.MessageID in (SELECT MessageID from Messages m where m.FromAccountID = '87298106-7344-4676-97AA-00BA342BA650'))";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

            cmd.CommandText = "DELETE FROM MessageStepData WHERE MessageID in ( SELECT MessageID from Messages m where m.FromAccountID = '87298106-7344-4676-97AA-00BA342BA650')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

            cmd.CommandText = "DELETE FROM MessageSteps WHERE MessageID in ( SELECT MessageID from Messages m where m.FromAccountID = '87298106-7344-4676-97AA-00BA342BA650')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

            cmd.CommandText = "DELETE FROM MessageDelivery WHERE MessageID in ( SELECT MessageID from Messages m where m.FromAccountID = '87298106-7344-4676-97AA-00BA342BA650')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);

            cmd.CommandText = "DELETE FROM Messages WHERE FromAccountID = '87298106-7344-4676-97AA-00BA342BA650'";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
        }

        [OperationContract]
        public string MessageGetStepObject(Guid stepID)
        {
            var myObject = new ReturnObjectAsJson(stepID);
            return myObject.ToJson();
        }




    }
}
