using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models {
  public class Note {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    public int Account_Id { get; set; }

    [ForeignKey("Account_Id")]
    public UserAccount Account { get; set; } = null!;
  }
}
