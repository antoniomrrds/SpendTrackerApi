using SharedKernel.Abstractions.Data;
using WebApi.Infrastructure.Persistence.Repositories;

namespace WebApi.Infrastructure.Persistence.Data;

internal class UnitOfWork(AppDbContext context) : BaseRepository(context), IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}
