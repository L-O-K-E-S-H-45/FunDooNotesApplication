//using ExcepionHandling.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {

        private readonly FunDooDBContext funDooDbContext;
        private readonly IConfiguration config;
        private readonly ILogger<UserRepo> _logger;
        public UserRepo(FunDooDBContext funDooDbContext, IConfiguration config, ILogger<UserRepo> _logger)
        {
            this.funDooDbContext = funDooDbContext;
            this.config = config;
            this._logger = _logger;
        }

        bool ValidateUserInputData<T>(T userObject)
        {
            ValidationContext validationContext = new ValidationContext(userObject, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(userObject, validationContext, validationResults, true);
            if (!valid)
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
                return false;
            }
            else
            {
                Console.WriteLine("User input data is valid.");
                return true;
            }
        }

        public bool CheckUser(string email)
        {
            var result = funDooDbContext.Users.Any(user => user.Email == email);
            return result;
        }

        private string EncodePassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in password encode using base64Encode: " + ex.Message);
            }
        }

        private string DecodePassword(string encodedData)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encodedData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char); 
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in password decode using base64Encode: " + ex.Message);
            }
        }

        public UserEntity UserRegistration(RegisterModel model)
        {
            if (!CheckUser(model.Email))
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = model.FirstName;
                userEntity.LastName = model.LastName;
                userEntity.Email = model.Email;
                userEntity.Password = EncodePassword(model.Password);
                userEntity.CreatedAt = DateTime.Now;
                userEntity.ChangedAt = DateTime.Now;

                funDooDbContext.Users.Add(userEntity);
                funDooDbContext.SaveChanges();

                return userEntity;
            }
            else
                throw new Exception("User already exist for given email!!!");
        }

        public string UserLogin(LoginModel model)
        {
            var user = funDooDbContext.Users.FirstOrDefault(user => user.Email == model.Email
                                            && user.Password == EncodePassword(model.Password));
            if (user != null)
            {
                var token = GenerateToken(user.Email, user.UserId);
                return token;
            }
            else
                throw new Exception("User Not found b/z user credentials are invalid!!!");

            //else
            //    throw new NotFoundException("User Not found b/z user credentials are invalid!!!");

        }

        private string GenerateToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", email),
                new Claim("UserId", userId.ToString())
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserEntity GetUserByEmail(string email)
        {
            var userEntity = funDooDbContext.Users.ToList().Find(user => user.Email == email);
            return userEntity;
        }

        public ForgotPasswordModel ForgetPassword(string email)
        {
            //var result = funDooDbContext.Users.FirstOrDefault(user => user.Email == email);
            //return (result != null) ? true : false;

            //UserEntity user = funDooDbContext.Users.ToList().Find(user => user.Email == email);

            UserEntity user = GetUserByEmail(email);
            if (user != null)
            {
                ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
                forgotPasswordModel.UserId = user.UserId;
                forgotPasswordModel.Email = user.Email;
                forgotPasswordModel.Token = GenerateToken(user.Email, user.UserId);
                return forgotPasswordModel;
            }
            else
                throw new Exception("User Not Exist for requested email!!!");

        }

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {
            //var user = funDooDbContext.Users.FirstOrDefault(user => user.Email == email);

            UserEntity user = GetUserByEmail(email);
            if (user != null)
            {
                    user.Password = EncodePassword(resetPasswordModel.Password);
                    user.ChangedAt = DateTime.Now;
                    funDooDbContext.SaveChanges();

                    return true;
            }
            else
                throw new Exception("User Not Exist for requested email!!!");
        }

        //---------------------------------------------------------------
        public List<UserEntity> GetAllUsers()
        {
            List<UserEntity> users = funDooDbContext.Users.ToList();
            
                return users.Any() ? users : throw new Exception("Users list is empty");
        }

        public UserEntity GetUserByUserId(int userId)
        {
            var userEntity = funDooDbContext.Users.FirstOrDefault(user => user.UserId == userId);

            return (userEntity != null) ? userEntity : throw new Exception("User not found for requested Id: "+ userId);
        }

        public List<UserEntity> GetUsersByName(string userName)
        {
            var users = funDooDbContext.Users.ToList().FindAll(user => user.FirstName == userName);

            return (users.Any()) ? users : throw new Exception("Users not found for requested userName: " + userName);
        }

        public UserEntity UpdateUser(int userId, RegisterModel registerModel)
        {
            UserEntity userEntity = funDooDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (userEntity != null)
            {
                userEntity.FirstName = registerModel.FirstName;
                userEntity.LastName = registerModel.LastName;
                userEntity.Email = registerModel.Email;
                userEntity.Password = EncodePassword(registerModel.Password);
                userEntity.ChangedAt = DateTime.Now;

                funDooDbContext.SaveChanges();
                return userEntity;
            }
            else
                throw new Exception("User not exist for requested user Id: " + userId);
        }

        //----------------------------------------------------------------


    }
}
