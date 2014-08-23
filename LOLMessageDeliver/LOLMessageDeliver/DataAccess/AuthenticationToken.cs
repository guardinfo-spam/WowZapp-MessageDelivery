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
    public sealed class AuthenticationToken
    {

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private Guid _AuthenticationToken;
        private string _AuthenticationDeviceID;
        private string _AuthenticationIPAddress;
        private DateTime _AuthenticationCreatedDate;

        #endregion PrivateVariables

        #region Properties

        [DataMember]
        public List<General.Error> Errors = new List<General.Error>();

        [DataMember]
        public Guid AuthenticationTokenID
        {
            get { return this._AuthenticationToken; }
            set { this._AuthenticationToken = value; }
        }

        [DataMember]
        public string AuthenticationDeviceID
        {
            get { return this._AuthenticationDeviceID; }
            set { this._AuthenticationDeviceID = value; }
        }

        [DataMember]
        public string AuthenticationIPAddress
        {
            get { return this._AuthenticationIPAddress; }
            set { this._AuthenticationIPAddress = value; }
        }

        [DataMember]
        public DateTime AuthenticationCreatedDate
        {
            get { return this._AuthenticationCreatedDate; }
            set { this._AuthenticationCreatedDate = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public AuthenticationToken()
        {
            _AuthenticationToken = new Guid("00000000-0000-0000-0000-000000000000");
            _AuthenticationDeviceID = string.Empty;
            _AuthenticationIPAddress = string.Empty;
            _AuthenticationCreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the AuthenticationToken class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public AuthenticationToken(Guid authenticationToken)
        {

            _AuthenticationToken = authenticationToken;
            _AuthenticationDeviceID = string.Empty;
            _AuthenticationIPAddress = string.Empty;
            _AuthenticationCreatedDate = DateTime.Now;

            this.Get();

        }


        public AuthenticationToken(DataRow dr)
        {


            try
            {

                _AuthenticationToken = new Guid("00000000-0000-0000-000000000000");
                _AuthenticationDeviceID = string.Empty;
                _AuthenticationIPAddress = string.Empty;
                _AuthenticationCreatedDate = DateTime.Now;

                if (!DBNull.Value.Equals(dr["AuthenticationToken"])) { this._AuthenticationToken = (Guid)dr["AuthenticationToken"]; }
                if (!DBNull.Value.Equals(dr["AuthenticationDeviceID"])) { this._AuthenticationDeviceID = (string)dr["AuthenticationDeviceID"]; }
                if (!DBNull.Value.Equals(dr["AuthenticationIPAddress"])) { this._AuthenticationIPAddress = (string)dr["AuthenticationIPAddress"]; }
                if (!DBNull.Value.Equals(dr["AuthenticationCreatedDate"])) { this._AuthenticationCreatedDate = (DateTime)dr["AuthenticationCreatedDate"]; }

            }
            catch (Exception ex)
            {

                _AuthenticationToken = new Guid("00000000-0000-0000-000000000000");
                _AuthenticationDeviceID = string.Empty;
                _AuthenticationIPAddress = string.Empty;
                _AuthenticationCreatedDate = DateTime.Now;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999001";
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
                cmd.CommandText = "SELECT * FROM AuthenticationTokens WHERE AuthenticationToken = @AuthenticationToken";
                cmd.Parameters.Add("@AuthenticationToken", SqlDbType.UniqueIdentifier).Value = _AuthenticationToken; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["AuthenticationDeviceID"])) { this._AuthenticationDeviceID = (string)dr["AuthenticationDeviceID"]; }
                    if (!DBNull.Value.Equals(dr["AuthenticationIPAddress"])) { this._AuthenticationIPAddress = (string)dr["AuthenticationIPAddress"]; }
                    if (!DBNull.Value.Equals(dr["AuthenticationCreatedDate"])) { this._AuthenticationCreatedDate = (DateTime)dr["AuthenticationCreatedDate"]; }


                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "999002";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999003";
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
                if (_AuthenticationToken == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    cmd.CommandText = "INSERT INTO AuthenticationTokens (AuthenticationToken, AuthenticationDeviceID, AuthenticationIPAddress, AuthenticationCreatedDate) VALUES (@AuthenticationToken, @AuthenticationDeviceID, @AuthenticationIPAddress, @AuthenticationCreatedDate);";
                    
                    _AuthenticationToken = System.Guid.NewGuid();
                }
                else
                {
                    cmd.CommandText = "UPDATE AuthenticationTokens SET AuthenticationDeviceID = @AuthenticationDeviceID, AuthenticationIPAddress = @AuthenticationIPAddress, AuthenticationCreatedDate = @AuthenticationCreatedDate WHERE AuthenticationToken = @AuthenticationToken;";
                }

                cmd.Parameters.Add("@AuthenticationToken", SqlDbType.UniqueIdentifier).Value = this._AuthenticationToken;
                cmd.Parameters.Add("@AuthenticationDeviceID", SqlDbType.NVarChar, 100).Value = this.AuthenticationDeviceID;
                cmd.Parameters.Add("@AuthenticationIPAddress", SqlDbType.NVarChar, 50).Value = this.AuthenticationIPAddress;
                cmd.Parameters.Add("@AuthenticationCreatedDate", SqlDbType.DateTime).Value = this.AuthenticationCreatedDate;

                dt.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);


            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999004";
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
                //This function doesn't support delete
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "999005";
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
