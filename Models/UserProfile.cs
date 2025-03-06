using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models {
  public class UserProfile {
    [Key]
    public int Profile_Id { get; set; }  // Primary Key

    [Required]
    [ForeignKey("UserAccount")]
    public int Account_Id { get; set; }  // Foreign Key to UserAccounts

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }  // Employee email

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }  // Optional phone number

    [MaxLength(100)]
    public string? City { get; set; }  // Employee's city

    [MaxLength(50)]
    public string? State { get; set; }  // Employee's state

    [MaxLength(50)]
    public string? TimeZone { get; set; }  // Employee's timezone

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Auto-filled timestamp

    // Navigation property
    public virtual UserAccount UserAccount { get; set; }
  }
}
