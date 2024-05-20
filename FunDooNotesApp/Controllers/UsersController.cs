using BusinessLayer.Interfaces;
using ExcepionHandling.CustomExceptions;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserBusiness  userBusiness;
        private readonly IBus bus;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserBusiness userBusiness, IBus bus, ILogger<UsersController> logger)
        {
            this.userBusiness = userBusiness;
            this.bus = bus;
            this._logger = logger;
        }

        [HttpPost("register")]
        //[Route("user")]

        public IActionResult Registration(RegisterModel model)
        {
            //if (!userBusiness.CheckUser(model.Email))
            //{
            //    var response = userBusiness.UserRegistration(model);
            //    if (response != null)
            //    {
            //        return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "User Registration is successfull!!!", Data = response });
            //    }
            //    else
            //        return BadRequest(new ResponseModel<UserEntity> { IsSuccess = false, Message = "User Registration failed!!!", Data = response });
            //}
            //else
            //    return BadRequest(new ResponseModel<UserEntity> { IsSuccess = false, Message = "User Registration failed b/z Email already exists!!!" });

            try
            {
                var response = userBusiness.UserRegistration(model);
                return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "User Registration is successfull!!!", Data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User Registration failed!!!", Data = ex.Message });
            }

        }
        

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            //var result = userBusiness.UserLogin(model);
            //if (result != null)
            //    return Ok(new ResponseModel<string>(){IsSuccess = true, Message = "User login successfull!!!", Data = result});
            //else
            //    return BadRequest(new ResponseModel<string>{IsSuccess = false, Message = "User login failed!!!", Data = result});

            try
            {
                var response = userBusiness.UserLogin(model);
                return Ok(new ResponseModel<string>() { IsSuccess = true, Message = "User login successfull!!!", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User login failed!!!", Data = ex.Message });
            }

        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            //if (userBusiness.CheckUser(email))
            //{
            //    ForgotPasswordModel forgotPasswordModel = userBusiness.ForgetPassword(email);
            //    Send send = new Send();
            //    send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
            //    Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");
            //    var endPoint = await bus.GetSendEndpoint(uri);
            //   await  endPoint.Send(forgotPasswordModel);
            //    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Mail sent successfully", 
            //                    Data = "Token has been sent to your mail to reset password" });
            //}
            //else
            //    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Please provide valid email!!!", Data = "Sending mail failed" });

            try
            {
                ForgotPasswordModel forgotPasswordModel = userBusiness.ForgetPassword(email);
                Send send = new Send();
                send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");
                var endPoint = await bus.GetSendEndpoint(uri);
                await endPoint.Send(forgotPasswordModel);
                return Ok(new ResponseModel<string> {IsSuccess = true, Message = "Mail sent successfully", Data = "Token has been sent to your mail to reset password"});
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Please provide valid email!!!", Data = ex.Message });
            }

        }

        //[HttpPut("reset")]
        [Authorize]
        [HttpPost]
        [Route("ResetPasword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            //var response = userBusiness.ResetPassword(email, password, confirmPassword);
            //if (response != null)
            //    return Ok(new ResponseModel<UserEntity> {IsSuccess = true, Message = "User reset password is successfull", Data = response });
            //else
            //    return BadRequest(new ResponseModel<UserEntity> { IsSuccess = false, Message = "User reset password is failed", Data = response });

            try
            {
                if (resetPasswordModel.Password == resetPasswordModel.ConfirmPassword)
                {
                    string Email = User.FindFirstValue("Email");
                    if (userBusiness.ResetPassword(Email, resetPasswordModel))
                    {
                        return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "User reset password is successfull", Data = true });
                    }
                    else
                        return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "User reset password is failed", Data = false });
                }
                else
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, 
                        Message = "User reset password is failed", Data = "Password missmatch" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User reset password is failed", Data = ex.Message });
            }
        }

        // ------------------------------------------------------------------

        [HttpGet("")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var response = userBusiness.GetAllUsers();
                return Ok(new ResponseModel<List<UserEntity>> { IsSuccess = true, Message = "Users found", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> {IsSuccess = false, Message = "Users not found", Data = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            try
            {
                var response = userBusiness.GetUserByUserId(userId);
                return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "User found successfully", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find user", Data = ex.Message });
            }
        }

        [HttpGet("UserName")]
        public IActionResult GetUserByName(string userName)
        {
            try
            {
                var response = userBusiness.GetUsersByName(userName);
                return Ok(new ResponseModel<List<UserEntity>> { IsSuccess = true, Message = "User found successfully", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find user", Data = ex.Message });
            }
        }

        [HttpPut("{userId}")]
        public ActionResult UpdateUser(int userId, RegisterModel registerModel)
        {
            try
            {
                var response = userBusiness.UpdateUser(userId, registerModel);
                return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "User updated successfully", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update user", Data = ex.Message });
            }
        }

        // ------------------------------------------------------------------

        [HttpPut("update/{email}")]
        public IActionResult UpdateUserByEmail(string email, RegisterModel registerModel)
        {
            try
            {
                var response = userBusiness.UpdateUserByEmail(email, registerModel);
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "User updated successfully", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update user", Data = ex.Message });
            }
        }


    }
}
