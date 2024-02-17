using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.Infrastructure;
internal class TaskItemEntityConfiguration : EntityTypeConfigurationBase<TaskItem>
{
    public override void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        base.Configure(builder);

        builder.ComplexProperty(builder => builder.Planning);
    }
}
