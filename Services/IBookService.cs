using BookNest_API.Data;
using BookNest_API.DTOs;
using BookNest_API.Models;
using Microsoft.EntityFrameworkCore;


namespace BookNest_API.Services
{

        public interface IBookService
        {
            Task<IEnumerable<book>> GetAllBooks();
            Task<book?> CreateBook(CreateBookDto bookDto);
        }
    }
