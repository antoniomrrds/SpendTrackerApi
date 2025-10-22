namespace Domain.Errors;

public class DomainException : Exception
{
    public string Field { get; }

    public DomainException(string message, string field = "") : base(message)
    {
        Field = field;
    }
}