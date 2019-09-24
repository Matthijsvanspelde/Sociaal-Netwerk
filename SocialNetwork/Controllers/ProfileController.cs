﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Logic.ILogic;
using SocialNetwork.Models;
using SocialNetwork.ViewModels;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserLogic _userLogic;
        private readonly IPostLogic _postLogic;
        private readonly IFriendRequestLogic _friendRequestLogic;

        public ProfileController(IUserLogic userLogic, IPostLogic postLogic, IFriendRequestLogic friendRequestLogic)
        {
            _userLogic = userLogic;
            _postLogic = postLogic;
            _friendRequestLogic = friendRequestLogic;
        }

        public IActionResult Overview(User user)
        {
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {               
                user.Id = (int)HttpContext.Session.GetInt32("Id");
                _userLogic.GetUserDetails(user);
                ProfileViewModel profileViewModel = new ProfileViewModel()
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Middlename = user.Middlename,
                    Lastname = user.Lastname,
                    Birthdate = user.Birthdate,
                    Country = user.Country,
                    City = user.City,
                    Biography = user.Biography,
                };
                profileViewModel.Posts = new List<Post>();
                profileViewModel.Posts.AddRange(_postLogic.GetPost(user));
                return View(profileViewModel);
            }            
        }

        public IActionResult Edit(User user)
        {
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                user.Id = (int)HttpContext.Session.GetInt32("Id");               
                _userLogic.GetUserDetails(user);
                ProfileViewModel profileViewModel = new ProfileViewModel()
                {
                    Firstname = user.Firstname,
                    Middlename = user.Middlename,
                    Lastname = user.Lastname,
                    Birthdate = user.Birthdate,
                    Country = user.Country,
                    City = user.City,
                    Biography = user.Biography
                };                
                return View(profileViewModel);
            }            
        }

        public IActionResult Post()
        {
            return View();
        }

        public IActionResult EditProfile(User user)
        {
            if (user.Middlename == null)
            {
                user.Middlename = "";
            }
            user.Id = (int)HttpContext.Session.GetInt32("Id");            
            _userLogic.EditProfileDetails(user);
            return RedirectToAction("Overview", "Profile");
        }

        public IActionResult SearchedProfile(int Id)
        {
            User user = new User();
            user.Id = Id;
            _userLogic.GetUserDetails(user);
            ProfileViewModel profileViewModel = new ProfileViewModel()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Middlename = user.Middlename,
                Lastname = user.Lastname,
                Birthdate = user.Birthdate,
                Country = user.Country,
                City = user.City,
                Biography = user.Biography,
            };

            FriendRequest friendRequest = new FriendRequest();
            friendRequest.SenderId = (int)HttpContext.Session.GetInt32("Id");
            friendRequest.RevieverId = Id;
            if (_friendRequestLogic.CheckDublicateFriendRequest(friendRequest) == 0)
            {
                profileViewModel.Added = false;
            }
            else
            {
                profileViewModel.Added = true;
            }
            profileViewModel.Posts = new List<Post>();
            profileViewModel.Posts.AddRange(_postLogic.GetPost(user));
            return View("Overview", profileViewModel);
        }

        public IActionResult SetPost(Post post, User user)
        {
            user.Id = (int)HttpContext.Session.GetInt32("Id");
            post.Posted = DateTime.Now;
            _postLogic.SetPost(post, user);
            return RedirectToAction("Overview", "Profile");
        }

        public IActionResult SendFriendRequest(FriendRequest friendRequest, int RecieverId)
        {
            friendRequest.SenderId = (int)HttpContext.Session.GetInt32("Id");
            friendRequest.RevieverId = RecieverId;
            friendRequest.Recieved = DateTime.Now;
            if (_friendRequestLogic.CheckDublicateFriendRequest(friendRequest) == 0)
            {
                _friendRequestLogic.SendFriendRequest(friendRequest);
                return RedirectToAction("SearchedProfile/" + RecieverId, "Profile");
            }
            else
            {
                return RedirectToAction("SearchedProfile/" + RecieverId, "Profile");
            }         
        }
    }
}