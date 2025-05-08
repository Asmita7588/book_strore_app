using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System.Threading.Tasks;
using RepositoryLayer.Interfaces;
using static RepositoryLayer.Services.BookRepo;
using RepositoryLayer.Context;
using RepositoryLayer.Jwt;
using RepositoryLayer.Migrations;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Models;
using System.Linq;
using System.Net;

namespace RepositoryLayer.Services
{
    public class BookRepo : IBookRepo
    {
        private readonly BookStoreContext context;
        private readonly JwtFile jwtFile;
        public BookRepo(BookStoreContext context, JwtFile jwtFile)
        {
            this.context = context;
            this.jwtFile = jwtFile;

        }

        public async Task UploadBooksFromCsvAsync(IFormFile file, int UserId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("Invalid file.");

                var books = new List<BookEntity>();

                using var reader = new StreamReader(file.OpenReadStream());
                string line;
                int lineNumber = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lineNumber++;
                    if (lineNumber == 1) continue;

                    var columns = line.Split(',');

                    if (columns.Length < 4) continue;

                    books.Add(new BookEntity
                    {
                        BookName = columns[1],
                        Author = columns[2],
                        Description = columns[3],
                        Price = int.TryParse(columns[4], out var price) ? price : 0,
                        DiscountPrice = int.TryParse(columns[5], out var discountPrice) ? discountPrice : 0,
                        Quantity = int.TryParse(columns[6], out var quantity) ? quantity : 0,
                        BookImage = columns[7],
                        UserId = UserId,
                        createdAtDate = DateTime.TryParse(columns[9], out var parsedDate) ? parsedDate : DateTime.MinValue,
                        updatedAtDate = DateTime.TryParse(columns[10], out var parsedDate2) ? parsedDate2 : DateTime.MinValue,

                    });
                }

                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new Exception($"Failed to upload {ex.Message}");
            }
        }

        public Task<bool> AddBook(int userId, BookModel model)
        {
            try
            {
                var book = new BookEntity()
                {
                    Description = model.Description,
                    DiscountPrice = model.DiscountPrice,
                    BookImage = model.BookImage,
                    UserId = userId,
                    BookName = model.BookName,
                    Author = model.Author,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    createdAtDate = model.createdAtDate,
                    updatedAtDate = model.updatedAtDate
                };
                context.Books.Add(book);
                context.SaveChangesAsync();
                return Task.FromResult(true);
            }
            catch (Exception ex) {
                throw new Exception($"Error in Add Book {ex.Message}");
            }
        }

        public BookEntity UpdateBook(int bookId, BookModel model)
        {
            try
            {

                var book = context.Books.FirstOrDefault(b => b.BookId == bookId);
                if (book == null)
                    throw new Exception("Book not found.");

                book.BookName = model.BookName;
                book.Author = model.Author;
                book.Description = model.Description;
                book.Price = model.Price;
                book.DiscountPrice = model.DiscountPrice;
                book.Quantity = model.Quantity;
                book.BookImage = model.BookImage;
                book.updatedAtDate = DateTime.Now;

                context.SaveChanges();
                return book;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating book: {ex.Message}");
            }
        }

        public string DeleteBook(int bookId, int userId)
        {
            try
            {
                var book = context.Books.FirstOrDefault(b => b.BookId == bookId && b.UserId == userId);
                if (book != null)
                {
                    context.Books.Remove(book);
                    context.SaveChanges();
                    return "Book deleted successfully";
                }
                else
                {
                    return "Book not Found";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting book: {ex.Message}");
            }

        }

        public List<BookEntity> GetAllBooks()
        {
            try
            {
                    List<BookEntity> books = new List<BookEntity>();
                    books = context.Books.ToList();
                    return books;
               
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while Fetching book: {ex.Message}");
            }
        }

        public BookEntity GetBookById(int bookId)
        {
            try
            {
                BookEntity book = new BookEntity();
                book = context.Books.FirstOrDefault(u => u.BookId == bookId);
                return book;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while Fetching book by Id: {ex.Message}");
            }
        }





    }



}
