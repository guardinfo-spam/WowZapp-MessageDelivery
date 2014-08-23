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
    public sealed class BlankDataAccess
    {

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private int _BlankID;
        private string _BlankData;

        #endregion PrivateVariables

        #region Properties

        [DataMember]
        public List<General.Error> Errors = new List<General.Error>();

        [DataMember]
        public int BlankID
        {
            get { return this._BlankID; }
            set { this._BlankID = value; }
        }

        [DataMember]
        public string BlankData
        {
            get { return this._BlankData; }
            set { this._BlankData = value; }
        }


        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public BlankDataAccess()
        {
            _BlankID = 0;
            _BlankData = "";

        }

        /// <summary>
        /// Initializes a new instance of the BlankDataAccess class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public BlankDataAccess(int blankID)
        {

            _BlankID = blankID;
            _BlankData = "";

            this.Get();

        }


        public BlankDataAccess(DataRow dr)
        {


            try
            {

                _BlankID = 0;
                _BlankData = "";

                if (!DBNull.Value.Equals(dr["BlankID"])) { this._BlankID = (int)dr["BlankID"]; }
                if (!DBNull.Value.Equals(dr["BlankData"])) { this._BlankData = (string)dr["BlankData"]; }

            }
            catch (Exception ex)
            {

                _BlankID = 0;
                _BlankData = "";

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
                cmd.CommandText = "SELECT * FROM rep_BlankDataAccesss WHERE BlankID = @BlankID";
                cmd.Parameters.Add("@BlankID", SqlDbType.Int).Value = _BlankID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["BlankData"])) { this._BlankData = (string)dr["BlankData"]; }

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
                if (this._BlankID == 0)
                {
                    cmd.CommandText = "INSERT INTO rep_BlankDataAccesss (BlankData) VALUES (@BlankData); SELECT @@IDENTITY;";
                }
                else
                {
                    cmd.CommandText = "UPDATE rep_BlankDataAccesss SET BlankData = @BlankData WHERE BlankID = @BlankID;";
                    cmd.Parameters.Add("@BlankID", SqlDbType.Int).Value = this._BlankID;
                }

                cmd.Parameters.Add("@BlankData", SqlDbType.NVarChar, 100).Value = this.BlankData;


                object tmpResults = this.dt.ExecuteScalar(cmd, DataTools.DataSources.LOLMessageDelivery);

                if (this._BlankID == 0)
                {
                    this._BlankID = int.Parse(tmpResults.ToString());
                }


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
                cmd.CommandText = "DELETE FROM rep_BlankDataAccesss WHERE BlankID = @BlankID";
                cmd.Parameters.Add("@BlankDataAccessID", SqlDbType.Int).Value = BlankID;
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
