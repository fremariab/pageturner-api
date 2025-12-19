using Microsoft.EntityFrameworkCore;
using PageTurner.Api.Models.Entities;

namespace PageTurner.Api.Data
{
    public class PageTurnerDbContext : DbContext
    {
        public PageTurnerDbContext(DbContextOptions<PageTurnerDbContext> options)
            : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Review> Reviews => Set<Review>();
    }
}
