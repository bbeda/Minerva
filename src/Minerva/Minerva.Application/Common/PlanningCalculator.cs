using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.Common;
internal class PlanningCalculator
{
    public static bool IsInBoundary(TaskItemPlanningType planningType, DateOnly date, TaskItemPlanningType boundaryType, DateOnly boundaryValue)
    {
        var boundary = GetBoundaries(boundaryType, boundaryValue);
        var checkInterval = GetBoundaries(planningType, date);

        return checkInterval.end > boundary.start || checkInterval.start < boundary.end;
    }

    public static (DateOnly start, DateOnly end) GetBoundaries(TaskItemPlanningType planningType, DateOnly date)
    {
        return planningType switch
        {
            TaskItemPlanningType.Day => (date, date),
            TaskItemPlanningType.Week => GetWeekBoundary(date),
            TaskItemPlanningType.Month => GetMonthBoundary(date),
            _ => throw new NotSupportedException()
        };
    }

    public static (DateOnly start, DateOnly end) GetMonthBoundary(DateOnly date) => (GetMonthStart(date), GetMonthEnd(date));

    public static DateOnly GetMonthEnd(DateOnly date) => new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

    public static DateOnly GetMonthStart(DateOnly date) => new DateOnly(date.Year, date.Month, 1);

    public static (DateOnly start, DateOnly end) GetWeekBoundary(DateOnly date)
    {
        var weekStart = GetWeekStart(date);
        var weekEnd = weekStart.AddDays(6);

        return (weekStart, weekEnd);
    }

    public static DateOnly GetWeekStart(DateOnly value)
    {
        var currentWeekStart = value.DayOfWeek switch
        {
            DayOfWeek.Monday => value,
            DayOfWeek.Tuesday => value.AddDays(-1),
            DayOfWeek.Wednesday => value.AddDays(-2),
            DayOfWeek.Thursday => value.AddDays(-3),
            DayOfWeek.Friday => value.AddDays(-4),
            DayOfWeek.Saturday => value.AddDays(-5),
            DayOfWeek.Sunday => value.AddDays(-6),
            _ => throw new ArgumentOutOfRangeException()
        };
        return currentWeekStart;
    }
}
