using System.Threading.Tasks;
using System;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using ManagerLayer.Services;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager  userManager;
        private readonly IBus bus;
        private readonly IJwtManager jwtManager;

        public UserController( IUserManager userManager, IJwtManager jwtManager)
        {
            this.userManager = userManager;
            this.jwtManager = jwtManager;
        }

        [HttpPost]
        [Route("registerUser")]

        public IActionResult Register(RegisterModel model)
        {

            try
            {
                var check = userManager.CheckMail(model.Email);
                if (check)
                {
                    return BadRequest(new ResponseModel<AdminEntity> { Success = true, Message = "email already Exists" });

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
            catch (Exception ex) {
                throw ex;
            }

        }

        [HttpPost]
        [Route("loginUser")]

        public IActionResult LoginUser(LoginModel loginModel)
        {
            try
            {
                var user = userManager.LoginUser(loginModel);
                if (user != null)
                {
                    return Ok(new ResponseModel<string> { Success = true, Message = "login successfully", Data = user });
                }
                else
                {

                    return BadRequest(new ResponseModel<string> { Success = false, Message = "login failed", Data = user });
                }

            }catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassowod(string Email)
        {
            try
            {
                if (userManager.CheckMail(Email))
                {
                    Send sendEmail = new Send();
                    ForgotPasswordModel forgot = userManager.ForgotPassword(Email);
                    sendEmail.SendEmail(forgot.Email, forgot.Token);

                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Mail sent successfully",
                        Data = forgot.Token
                    });
                }
                else
                {
                    return Ok(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Email not found"
                    });
                }
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]

        public ActionResult ResetPassword(ResetPasswordModel reset)
        {
            try
            {
                string Email = User.FindFirst("Email").Value;

                if (userManager.ResetPassword(Email, reset))
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Password Changed successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Password Changed to failed " });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}
