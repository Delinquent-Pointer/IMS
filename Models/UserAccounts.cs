using System.ComponentModel.DataAnnotations;

namespace IMS.Models {
  public class UserAccount {
    [Key]
    public int Account_Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password_Hash { get; set; }
    public bool Is_IT_User { get; set; } = false;
    public bool Verified { get; set; } = false;
  }
}
