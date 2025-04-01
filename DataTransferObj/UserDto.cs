namespace IMS.DataTransferObj {
  public class UserDto {
    public int Account_Id { get; set; }

    // Assume Username is required — only make nullable if you expect nulls
    public string Username { get; set; } = string.Empty;

    public bool Is_IT_User { get; set; }

    // Optional: Add roles or claims support later if needed
  }
}
