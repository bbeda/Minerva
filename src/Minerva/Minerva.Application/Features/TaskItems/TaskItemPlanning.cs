using Humanizer;
using Minerva.Application.Common;
using BoundaryCheckParams = (Minerva.Application.Features.TaskItems.TaskItemPlanningType type, System.DateOnly date, Minerva.Application.Features.TaskItems.TaskItemPlanningType targetType, System.DateOnly targetDate);

namespace Minerva.Application.Features.TaskItems;
public record TaskItemPlanning(TaskItemPlanningType PlanningType, DateOnly? DayValue, DateOnly? WeekValue, DateOnly? MonthValue)
{
    public static TaskItemPlanning None { get; } = new(TaskItemPlanningType.None, null, null, null);

    public string? DayDisplayValue => DayValue.HasValue ? DayValue.Format(TaskItemPlanningType.Day, TimeProvider.System) : null;

    public string? WeekDisplayValue => WeekValue.HasValue ? WeekValue.Format(TaskItemPlanningType.Week, TimeProvider.System) : null;

    public string? MonthDisplayName => MonthValue.HasValue ? MonthValue.Format(TaskItemPlanningType.Month, TimeProvider.System) : null;

    public IEnumerable<(TaskItemPlanningType type, DateOnly date)> EnumeratePlannedOptions()
    {
        var setTypes = Enum.GetValues<TaskItemPlanningType>();
        foreach (var type in setTypes.Where(t => t != TaskItemPlanningType.None).OrderBy(v => v))
        {
            if ((type & PlanningType) == type)
            {
                yield return (type, GetDate(type)!.Value);
            }
        }
    }

    public DateOnly? GetDate(TaskItemPlanningType planningType)
    {
        return planningType switch
        {
            TaskItemPlanningType.Day => DayValue,
            TaskItemPlanningType.Week => WeekValue,
            TaskItemPlanningType.Month => MonthValue,
            _ => throw new NotSupportedException()
        };
    }

}

public class TaskItemPlanningBuilder
{
    private readonly Dictionary<TaskItemPlanningType, DateOnly> planningOptions = new();

    public TaskItemPlanningBuilder(TaskItemPlanning taskItemPlanning)
    {
        foreach (var plan in taskItemPlanning.EnumeratePlannedOptions())
        {
            planningOptions.Add(plan.type, plan.date);
        }
    }

    public TaskItemPlanningBuilder WithDay(DateOnly date)
    {
        UpdatePlanningOptions(date, TaskItemPlanningType.Day);

        return this;
    }

    public TaskItemPlanningBuilder WithWeek(DateOnly date)
    {
        UpdatePlanningOptions(date, TaskItemPlanningType.Week);

        return this;
    }

    public TaskItemPlanningBuilder WithMonth(DateOnly date)
    {
        UpdatePlanningOptions(date, TaskItemPlanningType.Month);

        return this;
    }

    public TaskItemPlanningBuilder WithPlan(TaskItemPlanningType type, DateOnly date)
    {
        UpdatePlanningOptions(date, type);

        return this;
    }

    private void UpdatePlanningOptions(DateOnly date, TaskItemPlanningType targetType)
    {
        var (startDate, endDate) = PlanningCalculator.GetBoundaries(targetType, date);
        planningOptions[targetType] = startDate;

        foreach (var type in planningOptions.Where(kvp => kvp.Key != targetType))
        {
            var boundaryCheckParams = GetBoundaryCheckParameters(type.Key, type.Value);
            if (boundaryCheckParams == default)
            {
                continue;
            }

            if (PlanningCalculator.IsInBoundary(boundaryCheckParams.type, boundaryCheckParams.date, boundaryCheckParams.targetType, boundaryCheckParams.targetDate))
            {
                planningOptions.Remove(type.Key);
            }
        }

        BoundaryCheckParams GetBoundaryCheckParameters(TaskItemPlanningType type, DateOnly plannedDate)
        {
            if (type < targetType)
            {
                return (type, plannedDate, targetType, startDate);
            }
            else if (type > targetType)
            {
                return (targetType, startDate, type, plannedDate);
            }
            return default;
        }
    }

    public TaskItemPlanning Build()
    {
        var type = planningOptions.Keys.Aggregate(TaskItemPlanningType.None, (current, planningType) => current | planningType);
        return new TaskItemPlanning(type, TryResolveDate(TaskItemPlanningType.Day), TryResolveDate(TaskItemPlanningType.Week), TryResolveDate(TaskItemPlanningType.Month));

        DateOnly? TryResolveDate(TaskItemPlanningType type)
        {
            if (planningOptions.TryGetValue(type, out var date))
            {
                return date;
            }

            return null;
        }
    }


}

[Flags]
public enum TaskItemPlanningType
{
    None = 0,
    Day = 1,
    Week = 2,
    Month = 4
}