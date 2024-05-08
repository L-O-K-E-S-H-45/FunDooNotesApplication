using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {

        private readonly IUserRepo userRepo;

        public UserBusiness (IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {

            return userRepo.UserRegistration(model);

        }

        public string UserLogin(LoginModel model)
        {
            return userRepo.UserLogin(model);
        }

        public bool CheckEmail(string email)
        {
            return userRepo.CheckEmail(email);
        }
    }
}
