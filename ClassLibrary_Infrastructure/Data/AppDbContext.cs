using ClassLibrary_Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary_Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> companies { get; set; }
    public DbSet<Product> products { get; set; }
}
