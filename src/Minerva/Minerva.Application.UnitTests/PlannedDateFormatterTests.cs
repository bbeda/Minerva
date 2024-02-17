using Microsoft.Extensions.Time.Testing;
using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.UnitTests;
public class PlannedDateFormatterTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("2024-01-01", "Today")]
    [InlineData("2024-01-02", "Tomorrow")]
    [InlineData("2024-01-03", "In 2 days")]
    [InlineData("2024-01-10", "In 9 days")]
    public void TestDayFormatter(string? date, string? expected)
    {
        var provider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));

        DateOnly? value = date is null ? null : DateOnly.FromDateTime(DateTime.Parse(date));
        var result = value.Format(TaskItemPlanningType.Day, provider);

        Assert.Equal(result, expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("2024-01-01", "This week")]
    [InlineData("2024-01-05", "This week")]
    [InlineData("2024-01-07", "This week")]
    [InlineData("2024-01-10", "Next week")]
    [InlineData("2024-01-14", "Next week")]
    [InlineData("2024-01-15", "In 2 weeks")]

    public void TestWeekFormatter(string? date, string? expected)
    {
        var provider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 3, 0, 0, 0, TimeSpan.Zero));

        DateOnly? value = date is null ? null : DateOnly.FromDateTime(DateTime.Parse(date));
        var result = value.Format(TaskItemPlanningType.Week, provider);

        Assert.Equal(result, expected);
    }


    [Theory]
    [InlineData(null, null)]
    [InlineData("2024-01-01", "This month")]
    [InlineData("2024-01-05", "This month")]
    [InlineData("2024-01-07", "This month")]
    [InlineData("2024-02-10", "Next month")]
    [InlineData("2024-02-14", "Next month")]
    [InlineData("2024-03-15", "In March")]
    [InlineData("2024-04-23", "In April")]
    [InlineData("2025-02-23", "In 13 months")]

    public void TestMonthFormatter(string? date, string? expected)
    {
        var provider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 3, 0, 0, 0, TimeSpan.Zero));

        DateOnly? value = date is null ? null : DateOnly.FromDateTime(DateTime.Parse(date));
        var result = value.Format(TaskItemPlanningType.Month, provider);

        Assert.Equal(expected, result);
    }
}
