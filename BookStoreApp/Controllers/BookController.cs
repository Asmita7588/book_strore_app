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
                string role = User.FindFirst("Role").Value;
                if (role != null && role == "admin")
                {
                    await bookManager.UploadBooksFromCsvAsync(file, userId);

                    return Ok("Books uploaded successfully.");
                }
                else
                {
                    return BadRequest($"Upload failed ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("add-book")]
        public async Task<IActionResult> CreateBook(BookModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role").Value;
                if (role != null && role == "admin")
                {
                    bool result = await bookManager.AddBook(userId, model);
                    if (result)
                    {
                        return Ok(new ResponseModel<bool> { Success = true, Message = "Book Added Successfully", Data = result });
                    }
                    else
                    {
                        return BadRequest("Book data is null");
                    }
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed To Add Book" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("{id}")]

        public IActionResult UpdateBook( int bookId, BookModel model)
        {
            try
            {
                string role = User.FindFirst("Role").Value;
                if (role != null && role == "admin")
                {
                    var result = bookManager.UpdateBook(bookId, model);
                    if (result != null)
                    {
                        return Ok(new ResponseModel<BookEntity> { Success = true, Message = "Book Updated Successfully", Data = result });
                    }
                    else
                    {
                        return BadRequest("Book not found");
                    }
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed To Update Book" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        [HttpDelete("{id}")]

        public IActionResult DeleteBook(int bookId)
        {
            try
            {   int adminId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role").Value;
                if (role != null && role == "admin")
                {
                    var result = bookManager.DeleteBook(bookId,adminId);
                    if (result != null)
                    {
                        return Ok(new ResponseModel<bool> { Success = true, Message = "Book deleted Successfully" });
                    }
                    else
                    {
                        return BadRequest("Book not found");
                    }
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed To Delete Book" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                string role = User.FindFirst("Role").Value;
                var books = bookManager.GetAllBooks();
                if (books != null && (role =="admin" || role =="user"))
                {
                    return Ok(books);
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed To Get All Book" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving books: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int bookId)
        {
            try
            {
                string role = User.FindFirst("Role").Value;
                BookEntity book = bookManager.GetBookById(bookId);
                if (book != null && (role == "admin" || role == "user"))
                {
                    return Ok(book);
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed To Get All Book" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving books by id: {ex.Message}");
            }
        }


    }
}
