namespace IMS.Models {
  public class WatchedProduct {
    public int Id { get; set; }
    public int Account_Id { get; set; }
    public int ProductId { get; set; }
    public int Threshold { get; set; }

    public Product Product { get; set; } = null!;
  }
}

