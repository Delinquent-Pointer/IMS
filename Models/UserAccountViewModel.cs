namespace IMS.Models {
  public class UserAccountViewModel {
    public int Account_Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool Is_IT_User { get; set; }
    public bool Verified { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
  }
}
