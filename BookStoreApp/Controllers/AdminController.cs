using CommonLayer.Models;
using ManagerLayer.Interfaces;
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
    }
}
