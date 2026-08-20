using BookNest_API.DTOs;
using BookNest_API.Models;

namespace BookNest_API.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book?> CreateBook(CreateBookDto bookDto);
    }
}