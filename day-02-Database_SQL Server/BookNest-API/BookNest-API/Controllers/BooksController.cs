using BookNest_API.DTOs;
using BookNest_API.Models;
using Microsoft.AspNetCore.Mvc;


namespace BookNest_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private static List<book> books = new List<book>();

        [HttpPost]
        public IActionResult CreateBook(CreateBookDto bookDto)

        {
            var book = new book
            {
                ID = books.Count + 1,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Price = bookDto.Price,
                Category = bookDto.Category,
                Stock = bookDto.Stock
            };

            books.Add(book);
            return Ok(bookDto);
        }
        [HttpGet]
        public IActionResult Getbooks()
        {
            return Ok(books);
        }

    }
}
