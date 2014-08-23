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
using LOLCodeLibrary.ErrorsManagement;
using LOLMessageDelivery.Classes.ErrorsMgmt;

namespace LOLMessageDelivery
{
    [DataContract]
    public sealed class MessageStep
    {

        public enum StepTypes
        {
            Text = 1,
            Comix = 2,
            Comicon= 3,
            Animation = 4,
            Video = 5,
            Polling = 6,
            SoundFX = 7,
            Voice = 8,
            Emoticon = 9
        }

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private Guid _StepID;
        private int _StepNumber;
        private Guid _MessageID;
        private StepTypes _StepType;
        private string _MessageText;
        private int _ContentPackItemID;

        #endregion PrivateVariables

        #region Properties

        [DataMember]
        public List<General.Error> Errors = new List<General.Error>();

        [DataMember]
        public Guid StepID
        {
            get { return this._StepID; }
            set { this._StepID = value; }
        }

        [DataMember]
        public int StepNumber
        {
            get { return this._StepNumber; }
            set { this._StepNumber = value; }
        }

        [DataMember]
        public Guid MessageID
        {
            get { return this._MessageID; }
            set { this._MessageID = value; }
        }

        [DataMember]
        public StepTypes StepType
        {
            get { return this._StepType; }
            set { this._StepType = value; }
        }

        [DataMember]
        public string MessageText
        {
            get { return this._MessageText; }
            set { this._MessageText = value; }
        }

        [DataMember]
        public int ContentPackItemID
        {
            get { return this._ContentPackItemID; }
            set { this._ContentPackItemID = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public MessageStep()
        {
            _StepID = new Guid("00000000-0000-0000-0000-000000000000");
            _StepNumber = 0;
            _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
            _StepType = StepTypes.Text;
            _MessageText = string.Empty;
            _ContentPackItemID = 0;
        }

        /// <summary>
        /// Initializes a new instance of the MessageStep class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public MessageStep(Guid stepID)
        {

            _StepID = stepID;
            _StepNumber = 0;
            _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
            _StepType = StepTypes.Text;
            _MessageText = string.Empty;
            _ContentPackItemID = 0;

            this.Get();

        }


        public MessageStep(DataRow dr)
        {


            try
            {

                _StepID = new Guid("00000000-0000-0000-0000-000000000000");
                _StepNumber = 0;
                _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
                _StepType = StepTypes.Text;
                _MessageText = string.Empty;
                _ContentPackItemID = 0;

                if (!DBNull.Value.Equals(dr["StepID"])) { this._StepID = (Guid)dr["StepID"]; }
                if (!DBNull.Value.Equals(dr["StepNumber"])) { this._StepNumber = (int)dr["StepNumber"]; }
                if (!DBNull.Value.Equals(dr["MessageID"])) { this._MessageID = (Guid)dr["MessageID"]; }
                if (!DBNull.Value.Equals(dr["StepType"])) { this._StepType = (StepTypes)dr["StepType"]; }
                if (!DBNull.Value.Equals(dr["MessageText"])) { this._MessageText = (string)dr["MessageText"]; }
                if (!DBNull.Value.Equals(dr["ContentPackItemID"])) { this._ContentPackItemID = (int)dr["ContentPackItemID"]; }
            }
            catch (Exception ex)
            {

                _StepID = new Guid("00000000-0000-0000-0000-000000000000");
                _StepNumber = 0;
                _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
                _StepType = StepTypes.Text;
                _MessageText = string.Empty;
                _ContentPackItemID = 0;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999021";
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
                cmd.CommandText = "SELECT * FROM MessageSteps WHERE StepID = @StepID";
                cmd.Parameters.Add("@StepID", SqlDbType.UniqueIdentifier).Value = _StepID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["StepID"])) { this._StepID = (Guid)dr["StepID"]; }
                    if (!DBNull.Value.Equals(dr["StepNumber"])) { this._StepNumber = (int)dr["StepNumber"]; }
                    if (!DBNull.Value.Equals(dr["_MessageID"])) { this._MessageID = (Guid)dr["_MessageID"]; }
                    if (!DBNull.Value.Equals(dr["_StepType"])) { this._StepType = (StepTypes)dr["_StepType"]; }
                    if (!DBNull.Value.Equals(dr["_MessageText"])) { this._MessageText = (string)dr["_MessageText"]; }
                    if (!DBNull.Value.Equals(dr["_ContentPackItemID"])) { this._ContentPackItemID = (int)dr["_ContentPackItemID"]; }

                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "999022";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999023";
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
                if (this._StepID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    _StepID = System.Guid.NewGuid();
                    cmd.CommandText = "INSERT INTO MessageSteps (StepID, StepNumber, MessageID, StepType, MessageText, ContentPackItemID) VALUES (@StepID, @StepNumber, @MessageID, @StepType, @MessageText, @ContentPackItemID);";
                }
                else                
                    cmd.CommandText = "UPDATE MessageSteps SET StepNumber = @StepNumber, MessageID = @MessageID, StepType = @StepType, MessageText = @MessageText, ContentPackItemID = @ContentPackItemID WHERE StepID = @StepID;";

                cmd.Parameters.Add("@StepID", SqlDbType.UniqueIdentifier).Value = this._StepID;
                cmd.Parameters.Add("@StepNumber", SqlDbType.Int).Value = this._StepNumber;
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = this._MessageID;
                cmd.Parameters.Add("@StepType", SqlDbType.Int).Value = this._StepType;
                if (this._MessageText != null)
                    cmd.Parameters.Add("@MessageText", SqlDbType.NVarChar, 500).Value = this._MessageText;
                else
                    cmd.Parameters.Add("@MessageText", SqlDbType.NVarChar, 500).Value = DBNull.Value;

                if(_ContentPackItemID != null)
                    cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = this._ContentPackItemID;
                else
                    cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = DBNull.Value;


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
                cmd.CommandText = "DELETE FROM MessageSteps WHERE StepID = @StepID";
                cmd.Parameters.Add("@MessageStepID", SqlDbType.UniqueIdentifier).Value = StepID;
                dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999025";
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

        #endregion Methods
    }
}
