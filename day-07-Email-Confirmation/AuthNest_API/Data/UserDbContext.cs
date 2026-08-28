using Microsoft.EntityFrameworkCore;
using AuthNest_API.Model;


namespace AuthNest_API.Data
{
    public class UserDbContext : DbContext
    {

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }


}
