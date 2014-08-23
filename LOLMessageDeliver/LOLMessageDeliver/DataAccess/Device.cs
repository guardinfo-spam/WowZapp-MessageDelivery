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
//using LOLCodeLibrary.ErrorsManagement;

namespace LOLMessageDelivery
{
    [DataContract]
    public sealed class Device
    {

        public enum DeviceTypes
        {
            Other = 0,
            iOS = 1,
            Android = 2,
            Windows = 3,
            OSX = 4,
            XBOX360 = 5
        }

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private string _DeviceID;
        private Guid _AccountID;
        private DeviceTypes _DeviceType;

        #endregion PrivateVariables

        #region Properties

        [DataMember]
        public List<General.Error> Errors = new List<General.Error>();

        [DataMember]
        public string DeviceID
        {
            get { return this._DeviceID; }
            set { this._DeviceID = value; }
        }

        [DataMember]
        public Guid AccountID
        {
            get { return this._AccountID; }
            set { this._AccountID = value; }
        }

        [DataMember]
        public DeviceTypes DeviceType
        {
            get { return this._DeviceType; }
            set { this._DeviceType = value; }
        }


        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public Device()
        {
            _DeviceID = string.Empty;
            _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _DeviceType = DeviceTypes.Other;
        }

        /// <summary>
        /// Initializes a new instance of the Device class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public Device(string deviceID)
        {

            _DeviceID = deviceID;
            _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _DeviceType = DeviceTypes.Other;

            this.Get();

        }


        public Device(DataRow dr)
        {


            try
            {

                _DeviceID = string.Empty;
                _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _DeviceType = DeviceTypes.Other;

                if (!DBNull.Value.Equals(dr["DeviceID"])) { this._DeviceID = (string)dr["DeviceID"]; }
                if (!DBNull.Value.Equals(dr["AccountID"])) { this._AccountID = (Guid)dr["AccountID"]; }
                if (!DBNull.Value.Equals(dr["DeviceTypeID"])) { this._DeviceType = (DeviceTypes)dr["DeviceTypeID"]; }

            }
            catch (Exception ex)
            {

                _DeviceID = string.Empty;
                _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _DeviceType = DeviceTypes.Other;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999011";
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
                cmd.CommandText = "SELECT * FROM Devices WHERE DeviceID = @DeviceID";
                cmd.Parameters.Add("@BlankID", SqlDbType.NVarChar,100).Value = _DeviceID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["AccountID"])) { this._AccountID = (Guid)dr["AccountID"]; }
                    if (!DBNull.Value.Equals(dr["DeviceTypeID"])) { this._DeviceType = (DeviceTypes)dr["DeviceTypeID"]; }

                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "999012";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999013";
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
                cmd.CommandText = "SELECT DeviceID FROM Devices WHERE DeviceID = @DeviceID";
                cmd.Parameters.Add("@DeviceID", SqlDbType.NVarChar, 100).Value = _DeviceID;
                DataSet ds = dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    cmd.CommandText = "INSERT INTO Devices (DeviceID, AccountID, DeviceTypeID) VALUES (@DeviceID, @AccountID, @DeviceTypeID);";
                }
                else
                {
                    cmd.CommandText = "UPDATE Devices SET AccountID = @AccountID, DeviceTypeID = @DeviceTypeID WHERE DeviceID = @DeviceID;";
                }

                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = _AccountID;
                cmd.Parameters.Add("@DeviceTypeID", SqlDbType.Int).Value = (int)_DeviceType;


                this.dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);


            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999014";
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
                //Does Not Support Delete
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999015";
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
