using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManagerLayer.Interfaces;
using RepositoryLayer.Migrations;
using RepositoryLayer.Models;
using RepositoryLayer.Entity;
using static MassTransit.ValidationResultExtensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using System.Collections.Generic;

namespace BookStoreApp.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookManager bookManager;

        public BookController(IBookManager bookService)
        {
            bookManager = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "admin")
                {
                    await bookManager.UploadBooksFromCsvAsync(file, userId);

                    return Ok("Books uploaded successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<bool>
                    {
                        Success = false,
                        Message = "Access denied: Only admin can upload file to add books."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while adding the book.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("add-book")]
        public async Task<IActionResult> CreateBook(BookModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "admin")
                {
                    bool result = await bookManager.AddBook(userId, model);
                    if (result)
                    {
                        return Ok(new ResponseModel<bool> { Success = true, Message = "Book Added Successfully", Data = result });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<bool>
                        {
                            Success = false,
                            Message = "Failed to add book due to internal error."
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<bool>
                    {
                        Success = false,
                        Message = "Access denied: Only admin can add books."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while adding the book.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("{bookId}")]
        public IActionResult UpdateBook(int bookId, BookModel model)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "admin")
                {
                    var result = bookManager.UpdateBook(bookId, model);
                    if (result != null)
                    {
                        return Ok(new ResponseModel<BookEntity> { Success = true, Message = "Book Updated Successfully", Data = result });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool>
                        {
                            Success = false,
                            Message = $"Book with ID {bookId} not found."
                        });

                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new ResponseModel<bool> { 
                            Success = false, 
                            Message = "Access denied: Only admins can update books." 
                        });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new ResponseModel<string> { Success = false, Message = "An unexpected error occurred.", Data = ex.Message });
            }
        }

        [HttpDelete("{bookId}")]

        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                int adminId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "admin")
                {
                    var result = bookManager.DeleteBook(bookId, adminId);
                    if (result != null)
                    {
                        return Ok(new ResponseModel<bool> { Success = true, Message = "Book deleted Successfully" });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool>
                        {
                            Success = false,
                            Message = $"Book with ID {bookId} not found."
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new ResponseModel<bool> { 
                            Success = false, 
                            Message = "Access denied: Only admins can delete books."
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ResponseModel<string> { 
                        Success = false,
                        Message = "An unexpected error occurred.", 
                        Data = ex.Message 
                    });
            }
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                var books = bookManager.GetAllBooks();
                if (books != null && (role == "admin" || role == "user"))
                {
                    return Ok(books);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<bool>
                    {
                        Success = false,
                        Message = "Access denied: Only admin or user can view books."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving books: {ex.Message}");
            }
        }

        [HttpGet("{bookId}")]
        public IActionResult GetBookById(int bookId)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                BookEntity book = bookManager.GetBookById(bookId);
                if (book != null && (role == "admin" || role == "user"))
                {
                    return Ok(book);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, 
                        new ResponseModel<bool> { 
                            Success = false, 
                            Message = "Access denied: Only admin or user can view book details." 
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving books by id: {ex.Message}");
            }
        }

        [HttpGet("sort-book-by-price-in-Assending-order")]
        public async Task<IActionResult> SearchBooksByPriceAsync()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "admin" || role == "user")
                {
                    var books = await bookManager.GetBooksByPriceAscendingAsync();

                    if (books != null)
                    {
                        return Ok(new ResponseModel<List<BookEntity>>{ 
                              Success = true,
                              Message = "Books retrieved and sorted by price in ascending order", Data = books 
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { Success = false, Message = "No books found" });
                    }
                }
                else
                {
                    return StatusCode(403, 
                        new ResponseModel<bool>
                        { 
                            Success = false, 
                            Message = "Access denied. Only admin or user can view books" 
                        });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, 
                    new ResponseModel<string> 
                    { 
                       Success = false,
                       Message = $"Server error: {ex.Message}", Data = null
                    });
            }
        }

        [HttpGet("sort-book-by-price-in-Descending-order")]
        public async Task<IActionResult> SearchBooksByPriceDescendingAsync()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "admin" || role == "user")
                {
                    var books = await bookManager.GetBooksByPriceDescendingAsync();

                    if (books != null)
                    {
                        return Ok(new ResponseModel<List<BookEntity>>
                        { 
                            Success = true, 
                            Message = "Books retrieved and sorted by price in Descending order",
                            Data = books });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { Success = false, Message = "No books found" });
                    }
                }
                else
                {
                    return StatusCode(403, new ResponseModel<bool> 
                    { 
                        Success = false,
                        Message = "Access denied. Only admin or user can view books" 
                    });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, 
                    new ResponseModel<string> 
                    { 
                        Success = false,
                        Message = $"Server error: {ex.Message}", Data = null 
                    });
            }
        }


        [HttpGet("recent-book")]
        public async Task<IActionResult> GetMostRecentBookAsync()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "admin" || role == "user")
                {
                    var book = await bookManager.GetMostRecentBookAsync();

                    if (book != null)
                    {
                        return Ok(new ResponseModel<List<BookEntity>> { Success = true, Message = "Most recent book retrieved successfully", Data = book });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { Success = false, Message = "No recent book found", Data = false });
                    }
                }
                else
                {
                    return StatusCode(403, new ResponseModel<bool> 
                    {  
                        Success = false, 
                        Message = "Access denied. Only admin or user can access this", Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, 
                    new ResponseModel<string> 
                    {
                        Success = false, Message = $"Server error: {ex.Message}" 
                    });
            }
        }

        //[HttpGet("search-book-by-name")]

        [HttpGet("search-by-name/{bookName}")]
        public async Task<IActionResult> SearchBooksByName(string bookName)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "admin" || role == "user")
                {
                    var books = await bookManager.SearchBooksByNameAsync(bookName);

                    if (books != null && books.Count > 0)
                        return Ok(new ResponseModel<List<BookEntity>> 
                        { 
                            Success = true, 
                            Message = $"Books containing '{bookName}' " +
                            $"found successfully.", Data = books 
                        });
                    else
                        return NotFound(new ResponseModel<bool> { Success = false, Message = "No books found with the given name." });
                }
                else
                    return StatusCode(403, 
                        new ResponseModel<bool> 
                        {
                            Success = false, 
                            Message = "Access denied. Only admin or user can search for books."
                        });
            }
            catch (Exception ex)
            {
                return StatusCode(500, 
                    new ResponseModel<string>
                    { 
                        Success = false,
                        Message = $"Internal server error: {ex.Message}" 
                    });
            }
        }


        [HttpGet("search-by-author/{authorName}")]
        public async Task<IActionResult> SearchBooksByAuthor(string authorName)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "admin" || role == "user")
                {
                    var books = await bookManager.SearchBooksByAuthorAsync(authorName);

                    if (books != null && books.Count > 0)
                        return Ok(new ResponseModel<List<BookEntity>> { Success = true, Message = $"Books by '{authorName}' found successfully.", Data = books });
                    else
                        return NotFound(new ResponseModel<bool> { Success = false, Message = "No books found for the given author." });
                }
                else
                    return StatusCode(403, 
                        new ResponseModel<bool> 
                        { 
                            Success = false, 
                            Message = "Access denied. Only admin or user can search for books."
                        });
            }
            catch (Exception ex)
            {
                return StatusCode(500, 
                    new ResponseModel<string> 
                    {
                        Success = false, Message = $"Internal server error: {ex.Message}"
                    });
            }
        }


        [HttpGet("search-book")]
        public async Task<IActionResult> SearchBooks(string book)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "admin" || role == "user")
                {
                    var books = await bookManager.SearchBooksAsync(book);

                    if (books != null && books.Count > 0)
                        return Ok(new ResponseModel<List<BookEntity>> { Success = true, Message = $"Books by '{book}' found successfully.", Data = books });
                    else
                        return NotFound(new ResponseModel<bool> { Success = false, Message = "No books found for the given author or book name." });
                }
                else
                    return StatusCode(403,
                        new ResponseModel<bool>
                        {
                            Success = false,
                            Message = "Access denied. Only admin or user can search for books."
                        });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseModel<string>
                    {
                        Success = false,
                        Message = $"Internal server error: {ex.Message}"
                    });
            }
        }





    }
}
