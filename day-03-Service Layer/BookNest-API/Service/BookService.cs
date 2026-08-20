using BookNest_API.Data;
using BookNest_API.DTOs;
using BookNest_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookNest_API.Services
{
    public class BookService : IBookService
    {
        private readonly BookNestDbContext _context;

        public BookService(BookNestDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> CreateBook(CreateBookDto bookDto)
        {
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b =>
                    b.Title == bookDto.Title &&
                    b.Author == bookDto.Author);

            if (existingBook != null)
            {
                return null;
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }
    }
}