using Microsoft.EntityFrameworkCore;
using IMS.Models;

namespace IMS.Data {
  public class AppDbContext:DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ✅ Database Tables
    public DbSet<Product> Products { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<UserProfile> UserProfile { get; set; } // ✅ Ensure no "s"
    public DbSet<CalendarEvent> CalendarEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      // ✅ Explicitly Set Table Names to Prevent Pluralization Issues
      modelBuilder.Entity<Product>().ToTable("Products");
      modelBuilder.Entity<UserAccount>().ToTable("UserAccounts");
      modelBuilder.Entity<UserProfile>().ToTable("UserProfile"); // ✅ Fixed
      modelBuilder.Entity<CalendarEvent>().ToTable("CalendarEvents");

      // ✅ Define Product Properties
      modelBuilder.Entity<Product>()
          .Property(p => p.Price)
          .HasColumnType("decimal(18,2)"); // Define price precision

      // ✅ Define UserAccount Properties
      modelBuilder.Entity<UserAccount>()
          .Property(u => u.Password_Hash)
          .IsRequired();

      modelBuilder.Entity<UserAccount>()
          .HasKey(u => u.Account_Id);

      // ✅ Define UserProfile Properties & Ensure Proper Indexing
      modelBuilder.Entity<UserProfile>()
          .HasIndex(up => up.Email)
          .IsUnique(); // Ensure unique emails

      modelBuilder.Entity<UserProfile>()
          .Property(up => up.FirstName)
          .HasMaxLength(100);

      modelBuilder.Entity<UserProfile>()
          .Property(up => up.LastName)
          .HasMaxLength(100);

      modelBuilder.Entity<UserProfile>()
          .Property(up => up.City)
          .HasMaxLength(100);

      modelBuilder.Entity<UserProfile>()
          .Property(up => up.TimeZone)
          .HasMaxLength(50);

      modelBuilder.Entity<UserProfile>()
          .Property(up => up.State)
          .HasMaxLength(50);

      // ✅ One-to-One Relationship: UserAccount → UserProfile
      modelBuilder.Entity<UserProfile>()
          .HasOne(up => up.UserAccount)
          .WithOne(ua => ua.UserProfile)
          .HasForeignKey<UserProfile>(up => up.Account_Id)
          .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascade delete
    }
  }
}
