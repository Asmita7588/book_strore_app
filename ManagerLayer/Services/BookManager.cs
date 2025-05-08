using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using RepositoryLayer.Services;

namespace ManagerLayer.Services
{
    public class BookManager :IBookManager

    {
        private readonly IBookRepo bookRepo;
        public BookManager(IBookRepo bookRepo)
        {
            this.bookRepo = bookRepo;
        }
        public async Task UploadBooksFromCsvAsync(IFormFile file, int UserId)
        {
            await bookRepo.UploadBooksFromCsvAsync(file, UserId);
        }
        public async Task<bool> AddBook(int userId, BookModel model)
        {
             return await bookRepo.AddBook(userId, model);
        }
        public BookEntity UpdateBook(int bookId, BookModel model)
        {
            return bookRepo.UpdateBook(bookId, model);
        }

        public string DeleteBook(int bookId, int userId)
        {
            return bookRepo.DeleteBook(bookId, userId);
        }

        public List<BookEntity> GetAllBooks()
        {
            return bookRepo.GetAllBooks();
        }

        public BookEntity GetBookById(int bookId)
        {
            return bookRepo.GetBookById(bookId);
        }

    }
}
