using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models {
  public class UserProfile {
    [Key]
    public int Profile_Id { get; set; }

    [Required]
    public int Account_Id { get; set; }

    [ForeignKey("Account_Id")]
    public UserAccount UserAccount { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(50)]
    public string? TimeZone { get; set; }

    [MaxLength(50)]
    public string? State { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
  }
}
