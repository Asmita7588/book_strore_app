using ManagerLayer.Services;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager customerManager;

        public CustomerController(ICustomerManager customerManager)
        {
            this.customerManager = customerManager;
        }


        [HttpPost]
        public async Task<IActionResult> AddCustomerDetails(CustomerDetailModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;

                if (role != null && (role == "admin" || role == "user"))
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(new ResponseModel<string>
                        {
                            Success = false,
                            Message = "Invalid input. Please check the entered details.",
                            Data = null
                        });
                    }

                    var customer = await customerManager.AddCustomerDetailsAsync(userId, model);

                    return Ok(new ResponseModel<CustomerDetails>
                    {
                        Success = true,
                        Message = "Customer details added successfully.",
                        Data = customer
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Access denied: Only authenticated users can add customer details."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while adding customer details.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomerDetails()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;

                if (role != null && (role == "admin" || role == "user"))
                {
                    var customerList = await customerManager.GetAllCustomerDetailsAsync();

                    if (customerList == null || customerList.Count == 0)
                    {
                        return NotFound(new ResponseModel<string>
                        {
                            Success = false,
                            Message = "No customer details found.",
                            Data = null
                        });
                    }

                    return Ok(new ResponseModel<List<CustomerResponse>>
                    {
                        Success = true,
                        Message = "Customer details fetched successfully.",
                        Data = customerList
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Access denied: Only authenticated users can view customer details.",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving customer details.",
                    Data = ex.Message
                });
            }
        }


    }
}
