using System;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminManager adminManager;

        public AdminController(IAdminManager adminManager)
        {
           this.adminManager = adminManager;

        }

        [HttpPost]
        [Route("registerAdmin")]

        public IActionResult Register(RegisterModel model)
        {

            var check = adminManager.CheckMail(model.Email);
            if (check)
            {
                return BadRequest(new ResponseModel<AdminEntity> { Success = true, Message = "email already Exists" });

            }
            else
            {
                var result = adminManager.RegisterAdmin(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<AdminEntity> { Success = true, Message = "Register successfully", Data = result });

                }
                return BadRequest(new ResponseModel<AdminEntity> { Success = false, Message = "Register failed", Data = result });
            }
        }

        [HttpPost]
        [Route("loginAdmin")]

        public IActionResult LoginUser(LoginModel loginModel)
        {

            var user = adminManager.LoginAdmin(loginModel);
            if (user != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "login successfully", Data = user });
            }
            return BadRequest(new ResponseModel<string> { Success = false, Message = "login failed", Data = user });

        }


        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassowod(string Email)
        {
            try
            {
                if (adminManager.CheckMail(Email))
                {
                    Send sendEmail = new Send();
                    ForgotPasswordModel forgot = adminManager.ForgotPassword(Email);
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

                if (adminManager.ResetPassword(Email, reset))
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
