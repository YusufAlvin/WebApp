using Microsoft.EntityFrameworkCore;
using WebServer.Model;

namespace WebServer;

public class AppDbContext: DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Product>? Products { get; set; }
    
    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string
        options.UseNpgsql(_configuration.GetConnectionString("WebApiDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
    }
}