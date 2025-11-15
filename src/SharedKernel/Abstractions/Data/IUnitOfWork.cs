namespace SharedKernel.Abstractions.Data;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
