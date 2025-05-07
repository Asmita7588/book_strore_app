using System.Threading.Tasks;
using System;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using ManagerLayer.Services;
using RepositoryLayer.Models;
using RepositoryLayer.Migrations;

namespace BookStoreApp.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager  userManager;
        private readonly IBus bus;
       

        public UserController( IUserManager userManager)
        {
            this.userManager = userManager;
            
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {

            try
            {
                var check = userManager.CheckMail(model.Email);
                if (check)
                {
                    return BadRequest(new ResponseModel<RegisterModel> { Success = true, Message = "email already Exists" });

                }
                else
                {
                    var result = userManager.RegisterUser(model);
                    var response = new RegistrationResponseModel
                    {
                        FullName = result.FullName,
                        Email = result.Email,
                        MobileNumber = result.MobileNumber
                    };
                    if (result != null)
                    {
                        return Ok(new ResponseModel<RegistrationResponseModel> { Success = true, Message = "Register successfully", Data = response });
                        
                    }
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Register failed" });
                }
            }
            catch (Exception ex) {
                throw ex;
            }

        }

        [HttpPost]
        [Route("login")]

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
        [Route("Forgot-password")]
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
        [Route("reset-password")]

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


        [HttpPost]
        [Route("access-token-login")]

        public IActionResult AccessTokenLoginUser(LoginModel loginModel)
        {
            try
            {
                var user = userManager.AccessTokenLogin(loginModel);
                if (user != null)
                {
                    return Ok(new ResponseModel<RefreshLoginResponse> { Success = true, Message = "login successfully", Data = user });
                }
                else
                {

                    return BadRequest(new ResponseModel<string> { Success = false, Message = " access token login failed" });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult RefreshToken(RefreshRequestModel model)
        {
            try
            {
                var result = userManager.RefreshAccessToken(model.RefreshToken);

                if (result != null) {

                  return Ok(new ResponseModel<RefreshLoginResponse> { Success = true, Message = "Refresh token Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = " failed to get refresh token"});
                }

            }catch(Exception ex)
            {
              throw ex;
            }
        }
        

    }


}
