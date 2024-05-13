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

        public bool CheckUser(string email)
        {
            return userRepo.CheckUser(email);
        }

        public ForgotPasswordModel ForgetPassword(string email)
        {
            return userRepo.ForgetPassword(email);
        }

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {
            return userRepo.ResetPassword(email, resetPasswordModel);
        }

        //---------------------------------------------------
        public List<UserEntity> GetAllUsers()
        {
            return userRepo.GetAllUsers();
        }

        public UserEntity GetUserByUserId(int userId)
        {
            return userRepo.GetUserByUserId(userId);
        }

        public List<UserEntity> GetUsersByName(string userName)
        {
            return userRepo.GetUsersByName(userName);
        }

        public UserEntity UpdateUser(int userId, RegisterModel registerModel)
        {
            return userRepo.UpdateUser(userId, registerModel);
        }
        //---------------------------------------------------
    }
}
