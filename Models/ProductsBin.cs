namespace IMS.Models {
  public class ProductBin : Product {
    public DateOnly DeleteDate { get; set; }
    public TimeOnly DeleteTime { get; set; }    
  }
}
