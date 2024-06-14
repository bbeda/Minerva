using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minerva.Application.Infrastructure;
internal class TaskItemPlanningResultItemConfiguration : EntityTypeConfigurationBase<TaskItemPlanningResultItem>
{
    public override void Configure(EntityTypeBuilder<TaskItemPlanningResultItem> builder)
    {
        base.Configure(builder);

        builder.Property(builder => builder.Result).IsRequired();
        builder.Property(builder => builder.PlanningType).IsRequired();
        builder.Property(builder => builder.PlanningDate).IsRequired();

        builder.HasOne(builder => builder.TaskItem).WithMany(b=>b.Plans).HasForeignKey(builder => builder.TaskItemId).IsRequired();
    }
}
