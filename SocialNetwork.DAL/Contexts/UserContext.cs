﻿using Microsoft.Data.SqlClient;
using SocialNetwork.DAL.App_data;
using SocialNetwork.DAL.IContexts;
using SocialNetwork.Models;
using System.Collections.Generic;
using System.Data;

namespace SocialNetwork.DAL.Contexts
{
    public class UserContext : IUserContext
    {
        private readonly Connection _connection;

        public UserContext(Connection connection)
        {
            _connection = connection;
        }

        //User Authentication
        public User RegisterUser(User user)
        {
            _connection.SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("RegisterUser", _connection.SqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Username", user.Username);
            sqlCommand.Parameters.AddWithValue("@Password", user.Password);
            sqlCommand.Parameters.AddWithValue("@Firstname", user.Firstname);
            sqlCommand.Parameters.AddWithValue("@Middlename", user.Middlename);
            sqlCommand.Parameters.AddWithValue("@Lastname", user.Lastname);
            sqlCommand.Parameters.AddWithValue("@Birthdate", user.Birthdate);
            sqlCommand.Parameters.AddWithValue("@Country", "Unknown");
            sqlCommand.Parameters.AddWithValue("@City", "Unknown");
            sqlCommand.ExecuteNonQuery();
            _connection.SqlConnection.Close();
            return user;
        }

        public int CheckDublicate(User user)
        {
            _connection.SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("CheckDublicate", _connection.SqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Username", user.Username);
            var returnParameter = sqlCommand.Parameters.Add("@Count", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            sqlCommand.ExecuteNonQuery();
            int UserCount = (int)returnParameter.Value;
            _connection.SqlConnection.Close();
            return UserCount;
        }

        public int VerifyUser(User user)
        {
            _connection.SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("VerifyUser", _connection.SqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Username", user.Username);
            sqlCommand.Parameters.AddWithValue("@Password", user.Password);
            var returnParameter = sqlCommand.Parameters.Add("@Count", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            sqlCommand.ExecuteNonQuery();
            int UserCount = (int)returnParameter.Value;
            _connection.SqlConnection.Close();
            return UserCount;
        }

        public User GetSessionId(User user)
        {
            _connection.SqlConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("GetSessionId", _connection.SqlConnection);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@Username", user.Username);
            sqlCmd.ExecuteNonQuery();
            using (SqlDataReader reader = sqlCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Id = reader.GetInt32(0);
                }
            }
            _connection.SqlConnection.Close();
            return user;
        }

        //User Profile
        public User GetUserDetails(User user)
        {
            _connection.SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("GetUserDetails", _connection.SqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", user.Id);
            sqlCommand.ExecuteNonQuery();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Firstname = reader.GetString(0);
                    user.Middlename = reader.GetString(1);
                    user.Lastname = reader.GetString(2);
                    user.Birthdate = reader.GetDateTime(3);
                    user.Country = reader.GetString(4);
                    user.City = reader.GetString(5);
                }
            }
            _connection.SqlConnection.Close();
            return user;
        }

        public User EditProfileDetails(User user)
        {
            _connection.SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("EditProfileDetails", _connection.SqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", user.Id);
            sqlCommand.Parameters.AddWithValue("@Firstname", user.Firstname);
            sqlCommand.Parameters.AddWithValue("@Middlename", user.Middlename);
            sqlCommand.Parameters.AddWithValue("@Lastname", user.Lastname);
            sqlCommand.Parameters.AddWithValue("@Birthdate", user.Birthdate);
            sqlCommand.Parameters.AddWithValue("@Country", user.Country);
            sqlCommand.Parameters.AddWithValue("@City", user.City);
            sqlCommand.ExecuteNonQuery();
            _connection.SqlConnection.Close();
            return user;
        }

        //User Search
        public IEnumerable<User> GetSearchResult(string Searchterm)
        {
            _connection.SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SearchUsers", _connection.SqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Searchterm", Searchterm);
            var SearchResult = new List<User>();
            sqlCommand.ExecuteNonQuery();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new User
                    {
                        Firstname = reader.GetString(0),
                        Middlename = reader.GetString(1),
                        Lastname = reader.GetString(2)
                    };
                    SearchResult.Add(user);
                }
            }
            _connection.SqlConnection.Close();
            return SearchResult;
        }
    }
}
