using System.Threading.Tasks;
using System;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreApp.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListManager wishListManager;
        public WishListController(IWishListManager wishListManager)
        {
            this.wishListManager = wishListManager;
        }

        [HttpPost("{bookId}")]
        public async Task<IActionResult> AddToWishlist(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var result = await wishListManager.AddToWishlist(userId, bookId);

                if (result != null)
                {
                    return Ok(new ResponseModel<BookModel>
                    {
                        Success = true,
                        Message = "Book added to wishlist successfully.",
                        Data = result
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Book or user not found."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while adding the book to wishlist.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var wishlist = await wishListManager.GetWishlistByUser(userId);


                if (wishlist == null || !wishlist.Any(w => !w.IsPurchased))
                {
                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Your wishlist is empty.",
                        Data = null
                    });
                }


                return Ok(new ResponseModel<List<BookModel>>
                {
                    Success = true,
                    Message = "Wishlist fetched successfully.",
                    Data = wishlist
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while fetching the wishlist.",
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("{bookId}")]
       
        public async Task<IActionResult> RemoveFromWishlist(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var removedBook = await wishListManager.RemoveFromWishlist(userId, bookId);

                if (removedBook != null)
                {
                    return Ok(new ResponseModel<BookModel>
                    {
                        Success = true,
                        Message = "Book removed from wishlist.",
                        Data = removedBook
                    });
                }

                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Wishlist item not found or already purchased."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Error removing from wishlist.",
                    Data = ex.Message
                });
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateWishlist(WishListUpdateModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var updatedItem = await wishListManager.UpdateWishlist(model, userId);

                if (updatedItem != null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Wishlist updated successfully.",
                        Data = new
                        {
                            updatedItem.WishListId,
                            updatedItem.BookId,
                            //updatedItem.IsPurchased,
                            BookDetails = new
                            {
                                updatedItem.Book.BookId,
                                updatedItem.Book.BookName,
                                updatedItem.Book.Author,
                                updatedItem.Book.Price
                            }
                        }
                    });
                }

                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Wishlist item not found."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the wishlist.",
                    Data = ex.Message
                });
            }
        }




    }
}
