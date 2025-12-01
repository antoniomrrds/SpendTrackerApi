using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories;

public interface IBaseRepositoryMarker;

public abstract class BaseRepository(AppDbContext context) : IBaseRepositoryMarker
{
    protected AppDbContext Context => context;
}
