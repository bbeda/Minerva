using Microsoft.EntityFrameworkCore;
using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.Infrastructure;
internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await SaveChangesAsync(cancellationToken);
}
