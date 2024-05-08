using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
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

        private readonly FunDooDBContext context;
        private readonly IConfiguration config;

        public UserRepo(FunDooDBContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
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

        public bool CheckEmail(string email)
        {
            //var result = context.Users.Any(user => user.Email == email);
            return context.Users.Any(user => user.Email == email);
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
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {

            //if (CheckEmail(model.Email))
            //{
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = model.FirstName;
                userEntity.LastName = model.LastName;
                userEntity.Email = model.Email;
                userEntity.Password = EncodePassword(model.Password);
                userEntity.CreatedAt = DateTime.Now;
                userEntity.ChangedAt = DateTime.Now;

                context.Users.Add(userEntity);
                context.SaveChanges();

                return userEntity;
            //}
            //else
            //    throw new Exception("Email already exists!!!");

        }

        public string UserLogin(LoginModel model)
        {
            //var userEntity = context.Users.FirstOrDefault(user => user.Email == model.Email
            //                                && user.Password == EncodePassword(model.Password));
            //return (userEntity != null) ? true : false;

            var result = context.Users.FirstOrDefault(user => user.Email == model.Email
                                            && user.Password == EncodePassword(model.Password));
            if (result != null)
            {
                var token = GenerateToken(result.Email, result.UserId);
                return token;
            }
            else
                return null;

        }

        //private readonly IConfiguration config;
        private string GenerateToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("email", email),
                new Claim("userId", userId.ToString())
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
