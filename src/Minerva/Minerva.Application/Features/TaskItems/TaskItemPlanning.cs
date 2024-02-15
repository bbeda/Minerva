using Minerva.Application.Common;
using BoundaryCheckParams = (Minerva.Application.Features.TaskItems.TaskItemPlanningType type, System.DateOnly date, Minerva.Application.Features.TaskItems.TaskItemPlanningType targetType, System.DateOnly targetDate);

namespace Minerva.Application.Features.TaskItems;
public record TaskItemPlanning
{
    public TaskItemPlanning()
    {
        PlanningType = TaskItemPlanningType.None;
    }

    public TaskItemPlanning(DateOnly? dayValue, DateOnly? weekValue, DateOnly? monthValue)
    {
        if (dayValue.HasValue)
        {
            PlanningType |= TaskItemPlanningType.Day;
            DayValue = dayValue.Value;
        }

        if (weekValue.HasValue)
        {
            PlanningType |= TaskItemPlanningType.Week;
            WeekValue = PlanningCalculator.GetWeekStart(weekValue.Value);
        }

        if (monthValue.HasValue)
        {
            PlanningType |= TaskItemPlanningType.Month;
            MonthValue = PlanningCalculator.GetMonthStart(monthValue.Value);
        }
    }

    public TaskItemPlanningType PlanningType { get; init; }

    public DateOnly? DayValue { get; }

    public DateOnly? MonthValue { get; }

    public DateOnly? WeekValue { get; }


    public static TaskItemPlanning None { get; } = new(null, null, null);

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

    public DateOnly? GetDate(TaskItemPlanningType planningType) => planningType switch
    {
        TaskItemPlanningType.Day => DayValue,
        TaskItemPlanningType.Week => WeekValue,
        TaskItemPlanningType.Month => MonthValue,
        _ => throw new NotSupportedException()
    };

}

public class TaskItemPlanningBuilder
{
    private readonly Dictionary<TaskItemPlanningType, DateOnly> planningOptions = [];

    public TaskItemPlanningBuilder(TaskItemPlanning taskItemPlanning)
    {
        foreach (var (type, date) in taskItemPlanning.EnumeratePlannedOptions())
        {
            planningOptions.Add(type, date);
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

            if (!PlanningCalculator.IsInBoundary(boundaryCheckParams.type, boundaryCheckParams.date, boundaryCheckParams.targetType, boundaryCheckParams.targetDate))
            {
                _ = planningOptions.Remove(type.Key);

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
        return new TaskItemPlanning(TryResolveDate(TaskItemPlanningType.Day), TryResolveDate(TaskItemPlanningType.Week), TryResolveDate(TaskItemPlanningType.Month));

        DateOnly? TryResolveDate(TaskItemPlanningType type) => planningOptions.TryGetValue(type, out var date) ? date : null;
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