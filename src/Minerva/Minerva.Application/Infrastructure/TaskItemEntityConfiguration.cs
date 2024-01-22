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
    }
}
