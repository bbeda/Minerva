using Minerva.Application.Common;
using System.Diagnostics.CodeAnalysis;

namespace Minerva.Application.Features.TaskItems;
public class TaskItem : Entity
{
    public required string Title { get; set; }

    public required string? Description { get; set; }

    public required DateOnly? DueDate { get; set; }

    public TaskItemStatus Status { get; private set; }

    public DateTimeOffset? CompletedOn { get; private set; }

    public TaskItemPlanning Planning { get; set; } = TaskItemPlanning.None;

    public ICollection<TaskItemPlanningResultItem> Plans { get; private set; } = [];

    [SetsRequiredMembers]
    public TaskItem(
        string title,
        string? description,
        DateOnly? dueDate,
        TaskItemStatus status = TaskItemStatus.Open)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
    }

    public void Complete()
    {
        Status = TaskItemStatus.Complete;
        CompletedOn = DateTime.UtcNow;
        foreach (var plan in Planning.EnumeratePlannedOptions())
        {
            var status = PlanningCalculator.IsInBoundary(plan.type, DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime), plan.type, plan.date) switch
            {
                true => TaskItemPlanningResultOption.Success,
                false => TaskItemPlanningResultOption.Failed
            };

            var planResult = new TaskItemPlanningResultItem()
            {
                PlanningDate = plan.date,
                PlanningType = plan.type,
                Result = status,
                TaskItemId = Id,
                TenantId = TenantId,
                Id = Guid.NewGuid()
            };

            Plans.Add(planResult);
        }
    }

    public void Plan(TaskItemPlanningType taskItemPlanningType, DateOnly date)
    {
        var builder = new TaskItemPlanningBuilder(Planning);
        builder.WithPlan(taskItemPlanningType, date);

        Planning = builder.Build();
    }

    public void RemovePlans(TaskItemPlanningType taskItemPlanningType)
    {
        var builder = new TaskItemPlanningBuilder(Planning);
        builder.RemovePlans(taskItemPlanningType);

        Planning = builder.Build();
    }
}

public enum TaskItemStatus
{
    Open,
    Complete
}
