using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieReviewAPILibrary.DatabaseAccess
{
    public class DatabaseLayer
    {
        public SqlConnection sqlConnection;

        public DatabaseLayer()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public DatabaseLayer(string connectionStringName)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
        }

        public void Dispose()
        {
            if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        ~DatabaseLayer()
        {
            Dispose();
        }

        public DataSet FillDataSet(SqlCommand sqlCommand)
        {
            if (sqlCommand.Connection == null)
                sqlCommand.Connection = this.sqlConnection;
            int retryCount =3;
            int retrySleepTimeInSeconds = 1000;
            DataSet dataSet = null;
            while (retryCount >= 1)
            {
                try
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);
                    return dataSet;
                }
                catch (Exception ex)
                {
                    retryCount--;
                    Thread.Sleep(retrySleepTimeInSeconds * 1000);
                    dataSet = null;
                }
            }
            return dataSet;
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            if (sqlCommand.Connection == null)
                sqlCommand.Connection = this.sqlConnection;
            int retryCount = 3;
            int retrySleepTimeInSeconds = 1000;
            int sqlRetVal = Int32.MaxValue;
            while (retryCount >= 1)
            {
                try
                {
                    sqlConnection.Open();
                    sqlRetVal = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    sqlRetVal = Int32.MaxValue;
                    retryCount--;
                    Thread.Sleep(retrySleepTimeInSeconds * 1000);
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open)
                        sqlConnection.Close();
                }
                if (sqlRetVal != Int32.MaxValue)
                    return sqlRetVal;
            }
            return sqlRetVal;
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand, bool getReturnValue)
        {
            SqlParameter retParam = new SqlParameter();
            if (sqlCommand.Connection == null)
                sqlCommand.Connection = this.sqlConnection;

            if (getReturnValue)
            {
                retParam.Direction = ParameterDirection.ReturnValue;
                sqlCommand.Parameters.Add(retParam);
            }

            int retryCount = 3;
            int retrySleepTimeInSeconds = 1000;
            int sqlRetVal = Int32.MaxValue;
            while (retryCount >= 1)
            {
                try
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlRetVal = Convert.ToInt32(retParam.Value);
                    sqlConnection.Close();
                }

                catch (Exception ex)
                {
                    sqlRetVal = Int32.MaxValue;
                    retryCount--;
                    Thread.Sleep(retrySleepTimeInSeconds * 1000);
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open)
                        sqlConnection.Close();
                }
                if (sqlRetVal != Int32.MaxValue)
                    return sqlRetVal;
            }
            return sqlRetVal;
        }

        public List<T> GetEntityList<T>(SqlCommand sqlCommand) where T : new()
        {
            return GetEntityList<T>(sqlCommand, 0);
        }

        public List<T> GetEntityList<T>(SqlCommand sqlCommand, int tableIndexInDataSet) where T : new()
        {
            try
            {
                return Utility.ConvertDataTableToEntityList<T>(FillDataSet(sqlCommand).Tables[tableIndexInDataSet]);
            }
            catch
            {
                return null;
            }
        }
    }
}

