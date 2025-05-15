public class TransactionItem
{
    public int Id { get; set; }
    public int SalesTransactionId { get; set; }
    public SalesTransaction SalesTransaction { get; set; }

    public string SKU { get; set; }
    public string ProductName { get; set; }
    public int QuantityRemoved { get; set; }
    public decimal PriceAtSale { get; set; }
}
