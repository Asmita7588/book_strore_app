<<<<<<< Updated upstream
﻿using CommonLayer.Models;
=======
﻿using System;
>>>>>>> Stashed changes
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using RepositoryLayer.Models;

namespace BookStoreApp.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminManager adminManager;

        public AdminController(IAdminManager adminManager)
        {
           this.adminManager = adminManager;

        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {

            var check = adminManager.CheckMail(model.Email);
            if (check)
            {
                return BadRequest(new ResponseModel<RegisterModel> { Success = true, Message = "email already Exists" });

            }
            else
            {
                var result = adminManager.RegisterAdmin(model);
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
                return BadRequest(new ResponseModel<AdminEntity> { Success = false, Message = "Register failed" });
            }
        }

        [HttpPost]
        [Route("login")]

        public IActionResult LoginUser(LoginModel loginModel)
        {

            var user = adminManager.LoginAdmin(loginModel);
            if (user != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "login successfully", Data = user });
            }
            return BadRequest(new ResponseModel<string> { Success = false, Message = "login failed", Data = user });

        }
<<<<<<< Updated upstream
=======


        [HttpPost]
        [Route("forgot-password")]
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
        [Route("reset-password")]

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
>>>>>>> Stashed changes
    }
}
