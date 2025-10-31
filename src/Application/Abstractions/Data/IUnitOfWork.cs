namespace Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task CommitAsync();
}
