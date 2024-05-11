using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {

        public UserEntity UserRegistration(RegisterModel model);

        public bool CheckUser(string email);

        public string UserLogin(LoginModel model);

        public ForgotPasswordModel ForgetPassword(string email);

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);

    }
}
