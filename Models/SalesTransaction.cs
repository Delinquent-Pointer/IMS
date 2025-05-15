public class SalesTransaction
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Account_Id { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<TransactionItem> Items { get; set; } = new List<TransactionItem>();
}
