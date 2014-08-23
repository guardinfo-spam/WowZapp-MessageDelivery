using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using LOLMessageDelivery.Classes.ErrorsMgmt;
using LOLCodeLibrary.ErrorsManagement;

namespace LOLMessageDelivery
{
    [DataContract]
    public sealed class Message
    {

        public struct MessageRecipient
        {
            public Guid AccountID { get; set; }
            public bool IsRead { get; set; }
        }


        #region PrivateVariables

        private DataTools dt = new DataTools();
        private Guid _MessageID;
        private Guid _FromAccountID;
        private DateTime _MessageSent;
        private List<MessageStep> _MessageSteps;
        private List<MessageRecipient> _MessageRecipients;
        private bool _MessageConfirmed;

        #endregion PrivateVariables

        #region Properties

        [DataMember]
        public List<General.Error> Errors = new List<General.Error>();

        [DataMember]
        public Guid MessageID
        {
            get { return this._MessageID; }
            set { this._MessageID = value; }
        }

        [DataMember]
        public Guid FromAccountID
        {
            get { return this._FromAccountID; }
            set { this._FromAccountID = value; }
        }


        [DataMember]
        public DateTime MessageSent
        {
            get { return this._MessageSent; }
            set { this._MessageSent = value; }
        }

        [DataMember]
        public List<MessageStep> MessageSteps
        {
            get { return this._MessageSteps; }
            set { this._MessageSteps = value; }
        }

        [DataMember]
        public List<MessageRecipient> MessageRecipients
        {
            get { return this._MessageRecipients; }
            set { this._MessageRecipients = value; }
        }

        [DataMember]
        public bool MessageConfirmed
        {
            get { return this._MessageConfirmed; }
            set { this._MessageConfirmed = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public Message()
        {
            _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
            _FromAccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _MessageSent = DateTime.Now;
            MessageRecipients = new List<MessageRecipient>();
            MessageConfirmed = false;

        }

        /// <summary>
        /// Initializes a new instance of the Message class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public Message(Guid messageID)
        {

            _MessageID = messageID;
            _FromAccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _MessageSent = DateTime.Now;
            MessageRecipients = new List<MessageRecipient>();
            MessageConfirmed = false;

            this.Get();
            GetSteps();
        }


        public Message(DataRow dr)
        {


            try
            {

                _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
                _FromAccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _MessageSent = DateTime.Now;
                MessageRecipients = new List<MessageRecipient>();
                MessageConfirmed = false;

                if (!DBNull.Value.Equals(dr["MessageID"])) { this._MessageID = (Guid)dr["MessageID"]; }
                if (!DBNull.Value.Equals(dr["FromAccountID"])) { this._FromAccountID = (Guid)dr["FromAccountID"]; }
                if (!DBNull.Value.Equals(dr["MessageSent"])) { this._MessageSent = (DateTime)dr["MessageSent"]; }
                if (!DBNull.Value.Equals(dr["MessageConfirmed"])) { this._MessageConfirmed = (bool)dr["MessageConfirmed"]; }
                GetSteps();
                IsMessageRead();
            }
            catch (Exception ex)
            {

                _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
                _FromAccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _MessageSent = DateTime.Now;
                MessageRecipients = new List<MessageRecipient>();
                MessageConfirmed = false;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999016";
                tmpError.ErrorLocation = this.ToString() + "(DataRow dataRow)" + this.ToString();
                tmpError.ErrorTitle = "Unable To Load From DataRow";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }

        }

        #endregion Constructors

        #region Methods

        public void Get()
        {
            this.dt = new DataTools();

            this.Errors = new List<General.Error>();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Messages (nolock) WHERE MessageID = @MessageID";
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = _MessageID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["MessageID"])) { this._MessageID = (Guid)dr["MessageID"]; }
                    if (!DBNull.Value.Equals(dr["FromAccountID"])) { this._FromAccountID = (Guid)dr["FromAccountID"]; }
                    if (!DBNull.Value.Equals(dr["MessageSent"])) { this._MessageSent = (DateTime)dr["MessageSent"]; }
                    if (!DBNull.Value.Equals(dr["MessageConfirmed"])) { this._MessageConfirmed = (bool)dr["MessageConfirmed"]; }
                    IsMessageRead();
                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "999017";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999018";
                tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                tmpError.ErrorTitle = "Unable To Load";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }



        public void Save()
        {
            this.DataValidation();

            if (this.Errors.Count != 0) { return; }

            this.dt = new DataTools();

            try
            {

                SqlCommand cmd = new SqlCommand();
                if (this._MessageID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    _MessageID = System.Guid.NewGuid();
                    cmd.CommandText = "INSERT INTO Messages (MessageID, FromAccountID, MessageConfirmed) VALUES (@MessageID, @FromAccountID, 0);";
                }
                else
                {
                    cmd.CommandText = "UPDATE Messages SET FromAccountID = @FromAccountID, MessageSent = @MessageSent WHERE MessageID = @MessageID;";
                    
                }
                if (MessageSent == new DateTime())
                    MessageSent = DateTime.Now;
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = this._MessageID;
                cmd.Parameters.Add("@FromAccountID", SqlDbType.UniqueIdentifier).Value = this.FromAccountID;
                cmd.Parameters.Add("@MessageSent", SqlDbType.DateTime).Value = this.MessageSent;

                object tmpResults = this.dt.ExecuteScalar(cmd, DataTools.DataSources.LOLMessageDelivery);
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.MessageSaveFatalError, "MessageCreate");
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }

        public void Delete()
        {

            SqlCommand cmd = new SqlCommand();
            dt = new DataTools();
            Errors = new List<General.Error>();

            try
            {
                cmd.CommandText = "DELETE FROM Messages WHERE MessageID = @MessageID";
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = MessageID;
                dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999020";
                tmpError.ErrorLocation = this.ToString() + ".Delete()";
                tmpError.ErrorTitle = "Unable To Delete";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }

        }
        /// <summary>
        /// Performs data validation prior to saving record. Resets and increments error collection count
        /// </summary>
        private void DataValidation()
        {
            this.Errors = new List<General.Error>();

            this.dt = new DataTools();

        }

        private void GetSteps()
        {
            MessageSteps = new List<MessageStep>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM MessageSteps (nolock) WHERE MessageID = @MessageID ORDER BY StepNumber";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = MessageID;

            DataSet ds = dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MessageSteps.Add(new MessageStep(dr));
            }

        }


        public void IsMessageRead()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT AccountID, Convert(bit,MAX(Convert(int,Delivered))) Delivered FROM MessageDelivery (nolock) WHERE MessageID = @MessageID GROUP BY AccountID";
            cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = _MessageID;

            MessageRecipients = new List<MessageRecipient>();

            DataSet ds = dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MessageRecipient tmpRecipient = new MessageRecipient();
                tmpRecipient.AccountID = (Guid)dr["AccountID"];
                tmpRecipient.IsRead = (bool)dr["Delivered"];
                MessageRecipients.Add(tmpRecipient);
            }


        }

        #endregion Methods
    }
}
