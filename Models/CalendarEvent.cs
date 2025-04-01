using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models {
  public class CalendarEvent {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    [Required]
    public int Account_Id { get; set; }

    [ForeignKey("Account_Id")]
    public UserAccount Account { get; set; } = null!;
  }
}
