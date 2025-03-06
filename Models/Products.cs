namespace IMS.Models {
  public class Product {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
    public string? SKU { get; set; }
    public string? Category { get; set; }
    public string? Location { get; set; }
  }
}
