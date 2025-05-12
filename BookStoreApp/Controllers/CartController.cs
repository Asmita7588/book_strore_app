using ManagerLayer.Services;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;
using System.Collections.Generic;
using ManagerLayer.Interfaces;
using System.Linq;

namespace BookStoreApp.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartManager cartManager;
        public CartController(ICartManager cartManager)
        {
            this.cartManager = cartManager;
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;

                if (role != null && role == "admin" || role == "user")
                {
                    var cartItem = await cartManager.AddToCart(userId, bookId);
                    if (cartItem != null)
                    {
                        return Ok(new ResponseModel<CartModel>
                        {
                            Success = true,
                            Message = "Book added to cart successfully",
                            Data = cartItem
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                        {
                            Success = false,
                            Message = "Failed to add book to cart.",
                            Data = null
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Access denied: Only customers can add to cart."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while adding the book to the cart.",
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFromCart(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "admin" || role == "user")
                {
                    var result = await cartManager.RemoveFromCart(userId, bookId);
                    if (result)
                    {
                        return Ok(new ResponseModel<string>
                        {
                            Success = true,
                            Message = "Book removed from cart successfully",
                            Data = null
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<string>
                        {
                            Success = false,
                            Message = "Book not found in cart.",
                            Data = null
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Access denied: Only customers can remove from cart."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while removing the book from the cart.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("{cartId}")]
        public async Task<IActionResult> UpdateCart(int cartId, [FromBody] UpdateCartModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "admin" || role == "user")
                {
                    var updatedCartItem = await cartManager.UpdateCartById(userId, cartId, model);
                    if (updatedCartItem != null)
                    {
                        return Ok(new ResponseModel<CartModel>
                        {
                            Success = true,
                            Message = "Cart updated successfully",
                            Data = updatedCartItem
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<string>
                        {
                            Success = false,
                            Message = "Cart item not found.",
                            Data = null
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Access denied: Only customers can update the cart."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the cart.",
                    Data = ex.Message
                });
            }


        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "admin" || role == "user")
                {
                    var cartItems = await cartManager.GetCartItems(userId);
                    decimal grandTotal = cartItems?.Sum(item => item.TotalPrice) ?? 0;

                    if (cartItems == null || cartItems.Count == 0)
                    {
                        return Ok(new ResponseModel<List<CartModel>>
                        {
                            Success = true,
                            Message = "Your cart is empty",
                            Data = new List<CartModel>(),
                        });
                    }
                    return Ok(new CartResponseModel<List<CartModel>>
                    {
                        Success = true,
                        Message = "Cart items retrieved successfully",
                        Data = cartItems,
                        TotalPrice = grandTotal

                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Access denied: Only customers can view the cart."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the cart items.",
                    Data = ex.Message
                });
            }
        }
    }
}
