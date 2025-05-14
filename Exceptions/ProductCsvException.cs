public class ProductCsvException : Exception
{
    public List<string> Errors { get; }

    public ProductCsvException(IEnumerable<string> errors)
    {
        Errors = errors.ToList();
    }
    public ProductCsvException (string error)
    {
        Errors = [error];
    }
}