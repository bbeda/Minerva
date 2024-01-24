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

    public ICollection<TaskItemPlanEntry> PlanEntries { get; private set; } = [];

    public TaskItemPlanEntry? DayPlan => PlanEntries?.FirstOrDefault(x => x.Type == TaskItemPlanningPeriond.Day);

    public TaskItemPlanEntry? WeekPlan => PlanEntries?.FirstOrDefault(x => x.Type == TaskItemPlanningPeriond.Week);

    public TaskItemPlanEntry? MonthPlan => PlanEntries?.FirstOrDefault(x => x.Type == TaskItemPlanningPeriond.Month);


    [SetsRequiredMembers]
    public TaskItem(
        string title,
        string? description,
        DateOnly? dueDate,
        TaskItemStatus status = TaskItemStatus.Open) : base()
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
    }

    public void Plan(TaskItemPlanningPeriond planType, DateOnly date)
    {
        var existingPlanEntry = PlanEntries.FirstOrDefault(x => x.Type == planType);
        if (existingPlanEntry != null)
        {
            existingPlanEntry.UpdateDates(date);
            return;
        }

        var entry = new TaskItemPlanEntry(planType, date);
        PlanEntries.Add(entry);
    }
}

public enum TaskItemStatus
{
    Open,
    Complete
}

public class TaskItemPlanEntry : Entity
{
    [SetsRequiredMembers]
    public TaskItemPlanEntry(Guid taskItemId, Guid id) : base(id) => TaskItemId = taskItemId;

    [SetsRequiredMembers]
    public TaskItemPlanEntry(TaskItemPlanningPeriond planEntryType, DateOnly startDate)
    {
        Type = planEntryType;
        SetDates(planEntryType, startDate);
    }

    public void UpdateDates(DateOnly date) => SetDates(Type, date);

    private void SetDates(TaskItemPlanningPeriond planEntryType, DateOnly startDate)
    {
        switch (planEntryType)
        {
            case TaskItemPlanningPeriond.Day:
                StartDate = startDate;
                EndDate = startDate;
                break;
            case TaskItemPlanningPeriond.Week:
                var weekDay = startDate.DayOfWeek;
                var daysToSubtract = weekDay == DayOfWeek.Sunday ? 6 : (int)weekDay - 1;
                StartDate = startDate.AddDays(-daysToSubtract);
                EndDate = StartDate.AddDays(6);
                break;
            case TaskItemPlanningPeriond.Month:
                StartDate = new DateOnly(startDate.Year, startDate.Month, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(planEntryType), planEntryType, null);
        }
    }

    [SetsRequiredMembers]
    public TaskItemPlanEntry(
        TaskItemPlanningPeriond planEntryType,
        DateOnly startDate,
        TaskItemPlanEntryStatus status)
        : this(planEntryType, startDate) => Status = status;

    public Guid TaskItemId { get; private set; }

    public TaskItemPlanningPeriond Type { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public TaskItemPlanEntryStatus Status { get; private set; } = TaskItemPlanEntryStatus.Planned;
}

[Flags]
public enum TaskItemPlanningPeriond
{
    Day = 1,
    Week = 2,
    Month = 4
}

public enum TaskItemPlanEntryStatus
{
    Planned,
    Completed,
    Failed
}

