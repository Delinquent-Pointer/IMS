using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models {
  public class UserAccount {
    [Key]
    public int Account_Id { get; set; } // Primary Key

    [Required]
    [MaxLength(100)]
    public string Username { get; set; }

    [Required]
    public string Password_Hash { get; set; }

    public bool Is_IT_User { get; set; } = false;
    public bool Verified { get; set; } = false;

    // ✅ One-to-One Relationship with UserProfile
    public virtual UserProfile UserProfile { get; set; }
  }
}
