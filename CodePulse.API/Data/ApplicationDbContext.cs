using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPost> blogPosts { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<BlogImage> blogImages { get; set; }
    }
}
