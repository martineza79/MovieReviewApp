using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MovieReviewAPILibrary.DatabaseAccess;

//reference from: https://www.youtube.com/watch?v=E_4c72gy2Xc&t=957s

namespace MovieReviewAPILibrary.DAL
{
    public class UserDataAccessLayer : IDisposable
    {
        public bool Create(string username, string pwd, string email)
        {
            bool retVal = false;
            SqlCommand sqlCmd = new SqlCommand("CreateUser");
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@Username", username);
            sqlCmd.Parameters.AddWithValue("@Pwd", pwd);
            sqlCmd.Parameters.AddWithValue("@Email", email);
            int result = new DatabaseLayer().ExecuteNonQuery(sqlCmd);
            if (result != Int32.MaxValue)
                retVal = true;

            return retVal;
        }

        public bool Create(string username, string pwd, string email, int roleId)
        {
            bool retVal = false;
            SqlCommand sqlCmd = new SqlCommand("CreateUser");
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@Username", username);
            sqlCmd.Parameters.AddWithValue("@Pwd", pwd);
            sqlCmd.Parameters.AddWithValue("@Email", email);
            int result = new DatabaseLayer().ExecuteNonQuery(sqlCmd);
            if (result != Int32.MaxValue)
                retVal = true;

            return retVal;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public User GetUserInfo(string username, string pwd)
        {
            User user = null;
            DatabaseLayer dbLayer = new DatabaseLayer();
            
            try
            {
                SqlCommand sqlCmd = new SqlCommand("GetUserInfo");
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@Username", username);
                sqlCmd.Parameters.AddWithValue("@Pwd", pwd);
                user = dbLayer.GetEntityList<User>(sqlCmd).FirstOrDefault();
            }
            
            finally
            {
                dbLayer.Dispose();
            }
            return user;
        }
    }
}
