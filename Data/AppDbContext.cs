using Microsoft.EntityFrameworkCore;
using IMS.Models;

namespace IMS.Data {
  public class AppDbContext:DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductBin> ProductsBin { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<AdminKeys> AdminKeys { get; set; }
    public DbSet<CalendarEvent> CalendarEvents { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      // Explicitly map UserProfile to correct table name
      modelBuilder.Entity<UserProfile>().ToTable("UserProfile");
      modelBuilder.Entity<UserAccount>().ToTable("UserAccounts");
      modelBuilder.Entity<Product>().ToTable("Products");
      modelBuilder.Entity<AdminKeys>().ToTable("AdminKeys");
      modelBuilder.Entity<CalendarEvent>().ToTable("CalendarEvents");
      modelBuilder.Entity<Note>().ToTable("Notes");
     modelBuilder.Entity<ProductBin>().ToTable("ProductsBin");

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

      // UserProfile setup
      modelBuilder.Entity<UserProfile>()
          .HasKey(u => u.Profile_Id);

      modelBuilder.Entity<UserProfile>()
          .Property(u => u.Email)
          .IsRequired()
          .HasMaxLength(255);

      modelBuilder.Entity<UserProfile>()
          .Property(u => u.FirstName)
          .IsRequired()
          .HasMaxLength(100);

      modelBuilder.Entity<UserProfile>()
          .Property(u => u.LastName)
          .IsRequired()
          .HasMaxLength(100);

      modelBuilder.Entity<UserProfile>()
          .HasOne(u => u.UserAccount)
          .WithOne()
          .HasForeignKey<UserProfile>(u => u.Account_Id)
          .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductBin>()
        .HasKey(p => p.Id);
    }
  }
}
