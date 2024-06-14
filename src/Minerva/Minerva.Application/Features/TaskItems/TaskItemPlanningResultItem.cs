using Minerva.Application.Common;
using System.Diagnostics.CodeAnalysis;

namespace Minerva.Application.Features.TaskItems;
public class TaskItemPlanningResultItem : Entity
{
    [SetsRequiredMembers]
    public TaskItemPlanningResultItem() : base()
    {
    }

    [SetsRequiredMembers]
    public TaskItemPlanningResultItem(Guid id) : base(id)
    {
    }

    public Guid TaskItemId { get; init; }

    public TaskItemPlanningResultOption Result { get; set; }

    public TaskItemPlanningType PlanningType { get; set; }

    public DateOnly PlanningDate { get; set; }

    public TaskItem TaskItem { get; set; } = default!;
}

public enum TaskItemPlanningResultOption
{
    Success = 1,
    Failed = 2
}
