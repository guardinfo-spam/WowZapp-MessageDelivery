using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using LOLCodeLibrary.ErrorsManagement;
using LOLMessageDelivery;

namespace LOLMessageDelivery.Classes
{
    public class LolMessagesRepository
    {
        private DataTools dataTools { get; set; }        
        public List<MethodParameter> MethodParameters { get; set; }
        public string MethodName { get; set; }
        public string LastErrorMessage { get; set; }

        public LolMessagesRepository(DataTools dt)
        {
            this.dataTools = dt;
        }

        public General.Error ValidateToken(Guid authenticationToken, Guid accountID)
        {
            return new General.Error();
        }

        public Guid GetRandomAccountID()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = new StringBuilder()
                                .Append("SELECT TOP 1 AccountID FROM Users u ")
                                .Append("where u.AccountActive = 1 ")
                                .Append("ORDER BY NEWID()").ToString();

            DataSet ds = this.dataTools.GetDataSet(cmd, DataTools.DataSources.LOLMessageDelivery);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                return new Guid(ds.Tables[0].Rows[0]["AccountID"].ToString());
            else
                throw new Exception("Could not return a a valid, active AccountID");
        }

        public void LogError()
        {
            if (!string.IsNullOrEmpty(this.LastErrorMessage) && !string.IsNullOrEmpty(this.MethodName))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = new StringBuilder()
                                    .Append("INSERT INTO ErrorLogging (MethodName,ErrorMessage,MethodParameters)")
                                    .Append("VALUES (@MethodName, @ErrorMessage, @MethodParameters)")
                                    .ToString();

                StringBuilder parametersData = new StringBuilder();
                if (this.MethodParameters != null && this.MethodParameters.Count > 0)
                {
                    foreach (MethodParameter mp in this.MethodParameters)
                    {
                        parametersData.Append("Name : ").Append(mp.Name).Append(" Value : ").Append(mp.Value).Append("|");
                    }
                }

                cmd.Parameters.Add("@MethodName", SqlDbType.VarChar).Value = this.MethodName;
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.Text).Value = this.LastErrorMessage;
                cmd.Parameters.Add("@MethodParameters", SqlDbType.Text).Value = parametersData.ToString().TrimEnd('|');
                this.dataTools.ExecuteCommand(cmd, DataTools.DataSources.LOLMessageDelivery);
            }
            else
                throw new Exception("Please pass in LastErrorMessage and MethodName before raising the logger");
        }

        public void LogError(List<MethodParameter> parameters, string methodName, General.Error errorObject)
        {
            this.MethodParameters = parameters;
            this.MethodName = methodName;
            this.LastErrorMessage = errorObject.ErrorDescription;
            LogError();
        }        
    }
}