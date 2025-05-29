using Microsoft.EntityFrameworkCore;
using IMS.Models;

namespace IMS.Data {
  public class AppDbContext:DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<AdminKeys> AdminKeys { get; set; }
    public DbSet<CalendarEvent> CalendarEvents { get; set; }
    public DbSet<Note> Notes { get; set; }

    public DbSet<WatchedProduct> WatchedProducts { get; set; }
    public DbSet<SalesTransaction> SalesTransactions { get; set; }
    public DbSet<TransactionItem> TransactionItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<UserAccount>().ToTable("UserAccounts");
      modelBuilder.Entity<Product>().ToTable("Products");
      modelBuilder.Entity<AdminKeys>().ToTable("AdminKeys");
      modelBuilder.Entity<CalendarEvent>().ToTable("CalendarEvents");
      modelBuilder.Entity<Note>().ToTable("Notes");

      // Define precision for Product price
      modelBuilder.Entity<Product>()
          .Property(p => p.Price)
          .HasColumnType("decimal(18,2)");

      // UserAccount setup
      modelBuilder.Entity<UserAccount>()
          .HasKey(u => u.Account_Id);

      modelBuilder.Entity<UserAccount>()
          .Property(u => u.Password_Hash)
          .IsRequired();

      // AdminKeys setup
      modelBuilder.Entity<AdminKeys>()
          .HasKey(u => u.It_Id);

      modelBuilder.Entity<AdminKeys>()
          .Property(u => u.AdminKey)
          .IsRequired();

      // CalendarEvent setup
      modelBuilder.Entity<CalendarEvent>()
          .HasKey(e => e.Id);

      // Note setup
      modelBuilder.Entity<Note>()
          .HasKey(n => n.Id);


    }
  }
}
