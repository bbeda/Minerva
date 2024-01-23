using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.Infrastructure;
internal class TaskItemEntityConfiguration : EntityTypeConfigurationBase<TaskItem>
{
    public const string TableName = "TaskItems";

    public override void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName);
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Status).IsRequired();
        builder.HasMany(t => t.PlanEntries).WithOne().HasForeignKey(fk => fk.TaskItemId).IsRequired();
    }
}

internal class TaskItemPlanEntryEntityConfiguration : EntityTypeConfigurationBase<TaskItemPlanEntry>
{
    public const string TableName = "TaskItemPlanEntries";

    public override void Configure(EntityTypeBuilder<TaskItemPlanEntry> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName);
        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.Status).IsRequired();
        builder.Property(t => t.StartDate).IsRequired();
        builder.Property(t => t.EndDate).IsRequired();
        builder.HasIndex(t => t.TaskItemId);
        builder.HasIndex(t => new { t.TaskItemId, t.Type, t.Status }).HasFilter("""("Status" = 0)""").IsUnique();
    }
}
