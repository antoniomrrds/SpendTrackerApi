namespace SpendTracker.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task CommitAsync();
}