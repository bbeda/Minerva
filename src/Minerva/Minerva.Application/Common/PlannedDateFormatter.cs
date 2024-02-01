using Minerva.Application.Features.TaskItems;
using System.Globalization;

namespace Minerva.Application.Common;
public static class PlannedDateFormatter
{
    public static string? Format(this DateOnly? value, TaskItemPlanningType planningType, TimeProvider timeProvider)
    {
        return planningType switch
        {
            TaskItemPlanningType.None => null,
            TaskItemPlanningType.Day => FormatDayPlanning(value, timeProvider),
            TaskItemPlanningType.Week => FormatWeekPlanning(value, timeProvider),
            TaskItemPlanningType.Month => FormatMonthPlanning(value, timeProvider),
            _ => throw new ArgumentOutOfRangeException(nameof(planningType), planningType, null)
        };
    }

    public static string? FormatMonthPlanning(this DateOnly? value, TimeProvider timeProvider)
    {
        if (value is null)
        {
            return null;
        }

        var monthsDiff = Math.Abs(12 * (value!.Value.Year - timeProvider.GetUtcNow().Year) + value!.Value.Month - timeProvider.GetUtcNow().Month);
        return monthsDiff switch
        {
            0 => "This month",
            1 => "Next month",
            -1 => "Last month",
            var d when d < 12 => $"In {new DateTime(1, d + 1, 1).ToString("MMMM", CultureInfo.InvariantCulture)}",
            var d => $"In {d} months"
        };
    }

    public static string? FormatWeekPlanning(this DateOnly? value, TimeProvider timeProvider)
    {
        if (value is null)
        {
            return null;
        }

        var valueWeekStart = GetWeekStart(value!.Value);
        var currentWeekStart = GetWeekStart(DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime));
        var weekDiff = (valueWeekStart.DayNumber - currentWeekStart.DayNumber) / 7;

        return weekDiff switch
        {
            0 => "This week",
            1 => "Next week",
            -1 => "Last week",
            var d => $"In {d} weeks"
        };
    }

    private static DateOnly GetWeekStart(DateOnly value)
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

    public static string? FormatDayPlanning(this DateOnly? value, TimeProvider timeProvider)
    {
        if (value is null)
        {
            return null;
        }

        var dayDiff = value.Value.DayNumber - DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime).DayNumber;
        return dayDiff switch
        {
            0 => "Today",
            1 => "Tomorrow",
            -1 => "Yesterday",
            var d => $"In {d} days"
        };
    }
}
