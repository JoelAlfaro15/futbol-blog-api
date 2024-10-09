using futapi.Models;
using Microsoft.EntityFrameworkCore;

namespace futapi.Data
{
    public class FutblogContext : DbContext
    {
        public FutblogContext(DbContextOptions<FutblogContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
