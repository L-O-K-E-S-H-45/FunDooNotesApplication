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

        public bool CheckEmail(string email);

        public string UserLogin(LoginModel model);

    }
}
