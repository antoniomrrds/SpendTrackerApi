using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories.Categories;

public abstract class BaseRepository(AppDbContext context)
{
    protected AppDbContext Context => context;
}
