using Microsoft.EntityFrameworkCore;
using Taller2.Models;

namespace Taller2.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Producto { get; set; } = null!;
    }
}
