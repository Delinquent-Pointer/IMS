using System;
using System.ComponentModel.DataAnnotations;

namespace IMS.Models {
  public class CalendarEvent {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; } // Nullable for single-day events

    public string Description { get; set; } // Optional event details
  }
}
