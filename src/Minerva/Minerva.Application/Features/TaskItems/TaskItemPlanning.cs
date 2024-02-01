using Humanizer;

namespace Minerva.Application.Features.TaskItems;
public record TaskItemPlanning(TaskItemPlanningType PlanningType, DateOnly? DayValue, DateOnly? WeekValue, DateOnly? MonthValue)
{
    public static TaskItemPlanning None { get; } = new(TaskItemPlanningType.None, null, null, null);

    public string? DayDisplayValue => DayValue.HasValue ? DayValue.Value.Humanize() : null;

    public string? WeekDisplayValue => WeekValue.HasValue ? WeekValue.Value.Humanize() : null;

    public string? MonthDisplayName => MonthValue.HasValue ? MonthValue.Value.Humanize() : null;

}

[Flags]
public enum TaskItemPlanningType
{
    None = 0,
    Day = 1,
    Week = 2,
    Month = 4
}