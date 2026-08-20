using BookNest_API.DTOs;
using BookNest_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookNest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto bookDto)
        {
            var book = await _bookService.CreateBook(bookDto);

            if (book == null)
            {
                return BadRequest("this book already exists.");
            }

            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> Getbooks()
        {
            var books = await _bookService.GetAllBooks();

            return Ok(books);
        }
    }
}