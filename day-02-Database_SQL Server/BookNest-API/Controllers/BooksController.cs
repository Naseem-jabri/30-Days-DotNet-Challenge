using BookNest_API.Data;
using BookNest_API.DTOs;
using BookNest_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookNest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookNestDbContext _db;

        // حقن قاعدة البيانات هنا
        public BooksController(BookNestDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult CreateBook(CreateBookDto bookDto)
        {
            var book = new book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Price = bookDto.Price,
                Category = bookDto.Category,
                Stock = bookDto.Stock
            };

            _db.Books.Add(book);
            _db.SaveChanges();

            return Ok(book);
        }

        [HttpGet]
        public IActionResult Getbooks()
        {
            var books = _db.Books.ToList();
            return Ok(books);
        }
    }
}