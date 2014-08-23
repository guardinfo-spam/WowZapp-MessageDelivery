using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LOLMessageDelivery.Classes;

namespace LOLMessageDelivery
{
    public class DataTools
    {
        public enum DataSources
        {
            LOLMessageDelivery
        }

        private SqlConnection GetConnection(DataSources source)
        {
            string connString = string.Empty;

            connString = ConfigurationManager.ConnectionStrings[source.ToString()].ToString();

            SqlConnection tmpReturn = new SqlConnection();
            tmpReturn.ConnectionString = connString;
            tmpReturn.Open();

            return tmpReturn;
        }

        public DataSet GetDataSet(SqlCommand _sqlCommand, DataSources source)
        {
            try
            {
                DataSet tmpReturn = new DataSet();

                SqlDataAdapter daTemp = new SqlDataAdapter();

                SqlConnection connTemp = this.GetConnection(source);

                _sqlCommand.Connection = connTemp;

                daTemp.SelectCommand = _sqlCommand;
                daTemp.Fill(tmpReturn);
                                
                if (connTemp.State == ConnectionState.Open)
                {
                    connTemp.Close();
                    connTemp.Dispose();
                }

                return tmpReturn;
            }
            catch (Exception ex)
            {
                CommonLogger.LogException(ex);
                throw new Exception(ex.Message);
            }
        }

        public int ExecuteCommand(SqlCommand _sqlCommand, DataSources source)
        {
            int tmpReturn = 0;
            SqlConnection connTemp = this.GetConnection(source);

            _sqlCommand.Connection = connTemp;

            tmpReturn = _sqlCommand.ExecuteNonQuery();

            if (connTemp.State == ConnectionState.Open)
            {
                connTemp.Close();
                connTemp.Dispose();
            }

            return tmpReturn;
        }

        public object ExecuteScalar(SqlCommand _sqlCommand, DataSources source)
        {
            object tmpReturn;

            SqlConnection connTemp = this.GetConnection(source);

            _sqlCommand.Connection = connTemp;

            tmpReturn = _sqlCommand.ExecuteScalar();

            if (connTemp.State == ConnectionState.Open)
            {
                connTemp.Close();
                connTemp.Dispose();
            }

            return tmpReturn;
        }

        public int ExecuteNonQuery(SqlCommand _sqlCommand, DataSources source)
        {
            int tmpReturn;

            SqlConnection connTemp = this.GetConnection(source);

            _sqlCommand.Connection = connTemp;

            tmpReturn = _sqlCommand.ExecuteNonQuery();

            if (connTemp.State == ConnectionState.Open)
            {
                connTemp.Close();
                connTemp.Dispose();
            }

            return tmpReturn;
        }

        public DateTime GetMinDate()
        {
            return DateTime.Parse("1900/1/1");
        }

        public void ExecuteBatch(List<SqlCommand> cmdList, DataSources source)
        {

            using (SqlConnection connTemp = this.GetConnection(source))
            {
                SqlTransaction trans = connTemp.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    foreach (SqlCommand cmd in cmdList)
                    { 
                        cmd.ExecuteNonQuery(); 
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }        
    }
}
