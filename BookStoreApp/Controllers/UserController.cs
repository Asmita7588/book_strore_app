using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager  userManager;

        public UserController( IUserManager userManager)
        {
            this.userManager = userManager;
            
        }

        [HttpPost]
        [Route("registerUser")]

        public IActionResult Register(RegisterModel model)
        {

            var check = userManager.CheckMail(model.Email);
            if (check)
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = true, Message = "email already Exists" });

            }
            else
            {
                var result = userManager.RegisterUser(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "Register successfully", Data = result });

                }
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Register failed", Data = result });
            }
        }


        [HttpPost]
        [Route("loginUser")]

        public IActionResult LoginUser(LoginModel loginModel)
        {

            var user = userManager.LoginUser(loginModel);
            if (user != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "login successfully", Data = user });
            }
            return BadRequest(new ResponseModel<string> { Success = false, Message = "login failed", Data = user });

        }
    }


}
