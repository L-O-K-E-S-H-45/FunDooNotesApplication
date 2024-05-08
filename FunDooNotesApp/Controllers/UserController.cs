using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBusiness  userBusiness;
        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }

        [HttpPost("user")]
        //[Route("user")]

        public IActionResult Registration(RegisterModel model)
        {
            if (!userBusiness.CheckEmail(model.Email))
            {
                var response = userBusiness.UserRegistration(model);

                if (response != null)
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "User Registration is successfull!!!", Data = response });
                }
                else
                    return BadRequest(new ResponseModel<UserEntity> { IsSuccess = false, Message = "User Registration failed!!!", Data = response });
            }
            else
                return BadRequest(new ResponseModel<UserEntity> { IsSuccess = false, Message = "User Registration failed b/z Email already exists!!!" });
        }


        [HttpGet("login")]
        public IActionResult Login(LoginModel model)
        {
            var result = userBusiness.UserLogin(model);
            if (result != null)
                return Ok(new ResponseModel<string>() { IsSuccess = true, Message = "User login successfull!!!", Data = result });
            else
                return BadRequest(new ResponseModel<string>() {IsSuccess = false, Message = "User login failed!!!", Data = result });
        }

    }
}
