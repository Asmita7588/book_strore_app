using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using RepositoryLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IBookRepo
    {
       Task UploadBooksFromCsvAsync(IFormFile file, int UserId);

       Task<bool> AddBook(int userId, BookModel model);
        public BookEntity UpdateBook(int bookId, BookModel model);
        public string DeleteBook(int bookId, int userId);

        public List<BookEntity> GetAllBooks();
        public BookEntity GetBookById(int id);
    }
}
