using Microsoft.EntityFrameworkCore;
using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.Infrastructure;
internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<TaskItem> TaskItems { get; set; }

    public DbSet<TaskItemPlanningResultItem> TaskItemPlanningResultItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await base.SaveChangesAsync(cancellationToken);
}
