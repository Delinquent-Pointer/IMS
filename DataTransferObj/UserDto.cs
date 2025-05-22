namespace IMS.DataTransferObj {
  public class UserDto {
    public int Account_Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public bool Is_IT_User { get; set; }

    public static UserDto FromJson(string json){
            
           UserDto? dto = System.Text.Json.JsonSerializer.Deserialize<UserDto>(json);

            return dto ?? throw new InvalidOperationException("Failed to deserialize UserDto from JSON.");
    }
    public string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
    }
}
