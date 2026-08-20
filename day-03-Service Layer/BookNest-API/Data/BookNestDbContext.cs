using Microsoft.EntityFrameworkCore;
using BookNest_API.Models;

namespace BookNest_API.Data
{
    public class BookNestDbContext : DbContext
    {
        public BookNestDbContext(DbContextOptions<BookNestDbContext> options) : base(options)
        {
        }

        public DbSet<book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);
        }
    }
}