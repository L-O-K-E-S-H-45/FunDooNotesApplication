using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {

        public UserEntity UserRegistration(RegisterModel model);

        public bool CheckUser(string email);

        public string UserLogin(LoginModel model);

        public ForgotPasswordModel ForgetPassword(string email);

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);

        //---------------------------------------------------
        List<UserEntity> GetAllUsers();
        UserEntity GetUserByUserId(int userId);
        List<UserEntity> GetUsersByName(string userName);
        UserEntity UpdateUser(int userId, RegisterModel registerModel);

        //---------------------------------------------------

        public object UpdateUserByEmail(string email, RegisterModel registerModel);

    }
}
