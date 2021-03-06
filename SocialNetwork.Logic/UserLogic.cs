﻿using SocialNetwork.DAL.IRepositories;
using SocialNetwork.Logic.ILogic;
using SocialNetwork.Models;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool RegisterUser(User user)
        {
            if (user.Middlename == null)
            {
                user.Middlename = "";
            }
            return _userRepository.RegisterUser(user);
        }

        public User GetSessionId(User user)
        {
            return _userRepository.GetSessionId(user);
        }

        public User GetUserDetails(User user)
        {
            return _userRepository.GetUserDetails(user);
        }

        public bool EditProfileDetails(User user)
        {
            return _userRepository.EditProfileDetails(user);
        }

        public IEnumerable<User> GetSearchResult(string Searchterm)
        {
            return _userRepository.GetSearchResult(Searchterm);
        }

        public IEnumerable<User> GetFollowers(User user)
        {
            return _userRepository.GetFollowers(user);
        }

        public IEnumerable<User> GetFollowing(User user)
        {
            return _userRepository.GetFollowing(user);
        }

        public bool DoesUsernameExist(User user)
        {
            bool DoesExist = _userRepository.DoesUsernameExist(user);
            return DoesExist;
        }

        public bool DoesUserCombinationMatch(User user)
        {
            bool DoesMatch = _userRepository.DoesUserCombinationMatch(user);
            return DoesMatch;
        }

        public bool DoesProfileExist(int Id)
        {
            bool DoesExist = _userRepository.DoesProfileExist(Id);
            return DoesExist;
        }

        public void DeleteUserAfterUnitTest(string username)
        {
            _userRepository.DeleteUserAfterUnitTest(username);
        }
    }
}
