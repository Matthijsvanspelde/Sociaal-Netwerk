﻿using SocialNetwork.DAL.IContexts;
using SocialNetwork.DAL.IRepositories;
using SocialNetwork.Models;
using System.Collections.Generic;

namespace SocialNetwork.DAL.Repositories
{
    public class UserRepository : IUserRepository 
    {
        private readonly IUserContext _IUserContext;

        public UserRepository(IUserContext IUserContext)
        {
            _IUserContext = IUserContext;
        }

        public bool RegisterUser(User user)
        {
            return _IUserContext.RegisterUser(user);
        }

        public User GetSessionId(User user)
        {
            return _IUserContext.GetSessionId(user);
        }

        public User GetUserDetails(User user)
        {
            return _IUserContext.GetUserDetails(user);
        }

        public bool EditProfileDetails(User user)
        {
            return _IUserContext.EditProfileDetails(user);
        }

        public IEnumerable<User> GetSearchResult(string Searchterm)
        {
            return _IUserContext.GetSearchResult(Searchterm);
        }

        public IEnumerable<User> GetFollowers(User user)
        {
            return _IUserContext.GetFollowers(user);
        }

        public IEnumerable<User> GetFollowing(User user)
        {
            return _IUserContext.GetFollowing(user);
        }

        public bool DoesUsernameExist(User user)
        {
            bool DoesExist = _IUserContext.DoesUsernameExist(user);
            return DoesExist;
        }

        public bool DoesUserCombinationMatch(User user)
        {
            bool DoesMatch = _IUserContext.DoesUserCombinationMatch(user);
            return DoesMatch; 
        }

        public bool DoesProfileExist(int Id)
        {
            bool DoesExist = _IUserContext.DoesProfileExist(Id);
            return DoesExist;
        }

        public int GetUserCount()
        {
            int userCount = _IUserContext.GetUserCount();
            return userCount;
        }

        public int GetFollowerCount()
        {
            int followerCount = _IUserContext.GetFollowerCount();
            return followerCount;
        }

        public void DeleteUserAfterUnitTest(string username)
        {
            _IUserContext.DeleteUserAfterUnitTest(username);
        }
    }
}
