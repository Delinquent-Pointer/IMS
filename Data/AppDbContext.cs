using Microsoft.EntityFrameworkCore;
using IMS.Models;

namespace IMS.Data {
  public class AppDbContext:DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Product>()
          .Property(p => p.Price)
          .HasColumnType("decimal(18,2)"); // Explicitly defining precision
      
      modelBuilder.Entity<UserAccount>()
          .Property(u => u.Password_Hash)
          .IsRequired();

      // Ensure the primary key is recognized
      modelBuilder.Entity<UserAccount>()
          .HasKey(u => u.Account_Id);
    }
  }
}
