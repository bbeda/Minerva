using Microsoft.EntityFrameworkCore;
using Minerva.Application.Common;

namespace Minerva.Application.Infrastructure;
internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await SaveChangesAsync(cancellationToken);
}
