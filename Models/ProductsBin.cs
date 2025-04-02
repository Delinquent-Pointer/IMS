namespace IMS.Models {
  public class ProductBin {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }

    // New inventory management fields
    public int Quantity { get; set; }          // Current stock level
    public int ReorderLevel { get; set; }        // When stock falls below this, consider reordering
    public string? SKU { get; set; }             // Unique identifier for the product
    public string? Category { get; set; }        // Product category
    public string? Location { get; set; }        // Warehouse or storage location
    public byte[]? Image { get; set; }           // Product image
    public DateOnly DeleteDate { get; set; }
    public TimeOnly DeleteTime { get; set; }    
  }
}
