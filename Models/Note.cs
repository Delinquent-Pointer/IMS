namespace IMS.Models {
  public class Note {
    public int Id { get; set; }  // Add primary key if using a database
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; }
  }
}
