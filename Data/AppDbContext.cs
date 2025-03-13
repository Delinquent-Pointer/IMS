using Microsoft.EntityFrameworkCore;
using IMS.Models;

namespace IMS.Data {
  public class AppDbContext:DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<AdminKeys> AdminKeys { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Product>()
          .Property(p => p.Price)
          .HasColumnType("decimal(18,2)"); // Explicitly defining precision
      
      modelBuilder.Entity<UserAccount>()
          .Property(u => u.Password_Hash)
          .IsRequired();

      modelBuilder.Entity<UserAccount>()
          .HasKey(u => u.Account_Id);

    
      modelBuilder.Entity<AdminKeys>()
          .Property(u => u.AdminKey)
          .IsRequired();

      
      modelBuilder.Entity<AdminKeys>()
          .HasKey(u => u.It_Id);
    }
  }
}
