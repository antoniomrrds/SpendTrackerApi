namespace WebApi.Domain.Errors;

public class DomainException(string message, string field = "") : Exception(message)
{
    public string Field { get; } = field;
}
