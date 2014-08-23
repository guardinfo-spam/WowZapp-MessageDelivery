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
using System.Web.Hosting;
using System.IO;
using LOLCodeLibrary.DataConversion;
//using LOLCodeLibrary.ErrorsManagement;

namespace LOLMessageDelivery
{    
    [DataContract]
    public sealed class PollingStep
    {

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private Guid _MessageID;
        private int _StepNumber;
        private string _PollingQuestion;
        private string _PollingAnswer1;
        private byte[] _PollingData1;
        private string _PollingAnswer2;
        private byte[] _PollingData2;
        private string _PollingAnswer3;
        private byte[] _PollingData3;
        private string _PollingAnswer4;
        private byte[] _PollingData4;


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
        public int StepNumber
        {
            get { return this._StepNumber; }
            set { this._StepNumber = value; }
        }

        [DataMember]
        public string PollingQuestion
        {
            get { return this._PollingQuestion; }
            set { this._PollingQuestion = value; }
        }

        [DataMember]
        public string PollingAnswer1
        {
            get { return this._PollingAnswer1; }
            set { this._PollingAnswer1 = value; }
        }

        [DataMember]
        public byte[] PollingData1
        {
            get { return this._PollingData1; }
            set { this._PollingData1 = value; }
        }

        [DataMember]
        public string PollingAnswer2
        {
            get { return this._PollingAnswer2; }
            set { this._PollingAnswer2 = value; }
        }

        [DataMember]
        public byte[] PollingData2
        {
            get { return this._PollingData2; }
            set { this._PollingData2 = value; }
        }

        [DataMember]
        public string PollingAnswer3
        {
            get { return this._PollingAnswer3; }
            set { this._PollingAnswer3 = value; }
        }

        [DataMember]
        public byte[] PollingData3
        {
            get { return this._PollingData3; }
            set { this._PollingData3 = value; }
        }

        [DataMember]
        public string PollingAnswer4
        {
            get { return this._PollingAnswer4; }
            set { this._PollingAnswer4 = value; }
        }

        [DataMember]
        public byte[] PollingData4
        {
            get { return this._PollingData4; }
            set { this._PollingData4 = value; }
        }

        [DataMember]
        public bool HasResponded {get; set;}



        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public PollingStep()
        {
            _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
            _StepNumber = 0;
            _PollingQuestion = string.Empty;
            _PollingAnswer1 = string.Empty;
            _PollingData1 = null;
            _PollingAnswer2 = string.Empty;
            _PollingData2 = null;
            _PollingAnswer3 = string.Empty;
            _PollingData3 = null;
            _PollingAnswer4 = string.Empty;
            _PollingData4 = null;

        }

        /// <summary>
        /// Initializes a new instance of the PollingStep class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public PollingStep(Guid messageID, int stepNumber)
        {

            _MessageID = messageID;
            _StepNumber = stepNumber;
            _PollingQuestion = string.Empty;
            _PollingAnswer1 = string.Empty;
            _PollingData1 = null;
            _PollingAnswer2 = string.Empty;
            _PollingData2 = null;
            _PollingAnswer3 = string.Empty;
            _PollingData3 = null;
            _PollingAnswer4 = string.Empty;
            _PollingData4 = null;

            this.Get();

        }


        public PollingStep(DataRow dr)
        {


            try
            {

                _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
                _StepNumber = 0;
                _PollingQuestion = string.Empty;
                _PollingAnswer1 = string.Empty;
                _PollingData1 = null;
                _PollingAnswer2 = string.Empty;
                _PollingData2 = null;
                _PollingAnswer3 = string.Empty;
                _PollingData3 = null;
                _PollingAnswer4 = string.Empty;
                _PollingData4 = null;

                if (!DBNull.Value.Equals(dr["MessageID"])) { this._MessageID = (Guid)dr["MessageID"]; }
                if (!DBNull.Value.Equals(dr["StepNumber"])) { this._StepNumber = (int)dr["StepNumber"]; }
                if (!DBNull.Value.Equals(dr["PollingQuestion"])) { this._PollingQuestion = (string)dr["PollingQuestion"]; }
                if (!DBNull.Value.Equals(dr["PollingAnswer1"])) { this._PollingAnswer1 = (string)dr["PollingAnswer1"]; }
                if (!DBNull.Value.Equals(dr["PollingData1"])) { this._PollingData1 = (byte[])dr["PollingData1"]; }
                if (!DBNull.Value.Equals(dr["PollingAnswer2"])) { this._PollingAnswer2 = (string)dr["PollingAnswer2"]; }
                if (!DBNull.Value.Equals(dr["PollingData2"])) { this._PollingData2 = (byte[])dr["PollingData2"]; }
                if (!DBNull.Value.Equals(dr["PollingAnswer3"])) { this._PollingAnswer3 = (string)dr["PollingAnswer3"]; }
                if (!DBNull.Value.Equals(dr["PollingData3"])) { this._PollingData3 = (byte[])dr["PollingData3"]; }
                if (!DBNull.Value.Equals(dr["PollingAnswer4"])) { this._PollingAnswer4 = (string)dr["PollingAnswer4"]; }
                if (!DBNull.Value.Equals(dr["PollingData4"])) { this._PollingData4 = (byte[])dr["PollingData4"]; }

            }
            catch (Exception ex)
            {

                _MessageID = new Guid("00000000-0000-0000-0000-000000000000");
                _StepNumber = 0;
                _PollingQuestion = string.Empty;
                _PollingAnswer1 = string.Empty;
                _PollingData1 = null;
                _PollingAnswer2 = string.Empty;
                _PollingData2 = null;
                _PollingAnswer3 = string.Empty;
                _PollingData3 = null;
                _PollingAnswer4 = string.Empty;
                _PollingData4 = null;


                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999006";
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
                cmd.CommandText = "SELECT * FROM PollingSteps WHERE MessageID = @MessageID";
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = _MessageID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["MessageID"])) { this._MessageID = (Guid)dr["MessageID"]; }
                    if (!DBNull.Value.Equals(dr["StepNumber"])) { this._StepNumber = (int)dr["StepNumber"]; }
                    if (!DBNull.Value.Equals(dr["PollingQuestion"])) { this._PollingQuestion = (string)dr["PollingQuestion"]; }
                    if (!DBNull.Value.Equals(dr["PollingAnswer1"])) { this._PollingAnswer1 = (string)dr["PollingAnswer1"]; }
                    if (!DBNull.Value.Equals(dr["PollingData1"])) { this._PollingData1 = (byte[])dr["PollingData1"]; }
                    if (!DBNull.Value.Equals(dr["PollingAnswer2"])) { this._PollingAnswer2 = (string)dr["PollingAnswer2"]; }
                    if (!DBNull.Value.Equals(dr["PollingData2"])) { this._PollingData2 = (byte[])dr["PollingData2"]; }
                    if (!DBNull.Value.Equals(dr["PollingAnswer3"])) { this._PollingAnswer3 = (string)dr["PollingAnswer3"]; }
                    if (!DBNull.Value.Equals(dr["PollingData3"])) { this._PollingData3 = (byte[])dr["PollingData3"]; }
                    if (!DBNull.Value.Equals(dr["PollingAnswer4"])) { this._PollingAnswer4 = (string)dr["PollingAnswer4"]; }
                    if (!DBNull.Value.Equals(dr["PollingData4"])) { this._PollingData4 = (byte[])dr["PollingData4"]; }

                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "999007";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999008";
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
                cmd.CommandText = "SELECT MessageID FROM PollingSteps WHERE MessageID = @MessageID AND StepNumber = @StepNumber";
                cmd.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier).Value = _MessageID;
                cmd.Parameters.Add("@StepNumber", SqlDbType.Int).Value = _StepNumber;
                DataSet ds = dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

                //process the images data and cut them down in size
                byte[] pd1 = new byte[0];
                byte[] pd2 = new byte[0];
                byte[] pd3 = new byte[0];
                byte[] pd4 = new byte[0];

                string appPhysPath = HostingEnvironment.ApplicationPhysicalPath;
                string curDir = Directory.GetCurrentDirectory();

                if (this.PollingData1 != null)
                {
                    JpegConversion jgc = new JpegConversion(this.PollingData1, Guid.NewGuid() + ".jpg", Guid.NewGuid() + ".jpg", "ImageDecoder", "i_view32.exe");
                    pd1 = jgc.RunMainBody(appPhysPath, curDir);
                }
                else
                    pd1 = null;

                if (this.PollingData2 != null)
                {
                    JpegConversion jgc = new JpegConversion(this.PollingData2, Guid.NewGuid() + ".jpg", Guid.NewGuid() + ".jpg", "ImageDecoder", "i_view32.exe");
                    pd2 = jgc.RunMainBody(appPhysPath, curDir);
                }
                else
                    pd2 = null;

                if (this.PollingData3 != null)
                {
                    JpegConversion jgc = new JpegConversion(this.PollingData3, Guid.NewGuid() + ".jpg", Guid.NewGuid() + ".jpg", "ImageDecoder", "i_view32.exe");
                    pd3 = jgc.RunMainBody(appPhysPath, curDir);
                }
                else
                    pd3 = null;

                if (this.PollingData4 != null)
                {
                    JpegConversion jgc = new JpegConversion(this.PollingData4, Guid.NewGuid() + ".jpg", Guid.NewGuid() + ".jpg", "ImageDecoder", "i_view32.exe");
                    pd4 = jgc.RunMainBody(appPhysPath, curDir);
                }
                else
                    pd4 = null;
                //-----------------------------------------------

                if (ds.Tables[0].Rows.Count == 0)
                {
                    cmd.CommandText = "INSERT INTO PollingSteps (MessageID, StepNumber, PollingQuestion, PollingAnswer1, PollingData1, PollingAnswer2, PollingData2, PollingAnswer3, PollingData3, PollingAnswer4, PollingData4) VALUES (@MessageID, @StepNumber, @PollingQuestion, @PollingAnswer1, @PollingData1, @PollingAnswer2, @PollingData2, @PollingAnswer3, @PollingData3, @PollingAnswer4, @PollingData4);";
                }
                else
                {
                    cmd.CommandText = "UPDATE PollingSteps SET PollingQuestion = @PollingQuestion, PollingAnswer1 = @PollingAnswer1, PollingData1 = @PollingData1, PollingAnswer2 = @PollingAnswer2, PollingData2 = @PollingData2, PollingAnswer3 = @PollingAnswer3, PollingData3 = @PollingData3, PollingAnswer4 = @PollingAnswer4, PollingData4 = @PollingData4 WHERE MessageID = @MessageID AND StepNumber = @StepNumber;";
                }

                if (PollingQuestion == null)
                    cmd.Parameters.Add("@PollingQuestion", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingQuestion", SqlDbType.NVarChar, 100).Value = this.PollingQuestion;


                if (PollingAnswer1 == null)
                    cmd.Parameters.Add("@PollingAnswer1", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingAnswer1", SqlDbType.NVarChar, 100).Value = this.PollingAnswer1;

                if (pd1 == null)
                    cmd.Parameters.Add("@PollingData1", SqlDbType.VarBinary).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingData1", SqlDbType.VarBinary).Value = pd1;


                if (PollingAnswer2 == null)
                    cmd.Parameters.Add("@PollingAnswer2", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingAnswer2", SqlDbType.NVarChar, 200).Value = this.PollingAnswer2;

                if (pd2 == null)
                    cmd.Parameters.Add("@PollingData2", SqlDbType.VarBinary).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingData2", SqlDbType.VarBinary).Value = pd2;


                if (PollingAnswer3 == null)
                    cmd.Parameters.Add("@PollingAnswer3", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingAnswer3", SqlDbType.NVarChar, 300).Value = this.PollingAnswer3;

                if (pd3 == null)
                    cmd.Parameters.Add("@PollingData3", SqlDbType.VarBinary).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingData3", SqlDbType.VarBinary).Value = pd3;


                if (PollingAnswer4 == null)
                    cmd.Parameters.Add("@PollingAnswer4", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingAnswer4", SqlDbType.NVarChar, 400).Value = this.PollingAnswer4;

                if (pd4 == null)
                    cmd.Parameters.Add("@PollingData4", SqlDbType.VarBinary).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@PollingData4", SqlDbType.VarBinary).Value = pd4;


                dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);



            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999009";
                tmpError.ErrorLocation = this.ToString() + ".Save()";
                tmpError.ErrorTitle = "Unable To Save";
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
                cmd.CommandText = "DELETE FROM PollingSteps WHERE MessageID = @MessageID";
                cmd.Parameters.Add("@MessageID", SqlDbType.Int).Value = MessageID;
                dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999010";
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
