using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.UnitTests;

public class PlanningCalculatorTests
{
    [Theory]
    [InlineData("2024-01-01", "2024-01-01")]
    [InlineData("2024-02-02", "2024-02-02")]
    [InlineData("2024-02-29", "2024-02-29")]
    public void GetBoundaries_WhenDayType_ReturnsSingleDay(string input, string expected)
    {
        var inputDate = DateOnly.FromDateTime(DateTime.Parse(input));
        var expectedDate = DateOnly.FromDateTime(DateTime.Parse(expected));

        var result = PlanningCalculator.GetBoundaries(TaskItemPlanningType.Day, inputDate);

        Assert.Equal(expectedDate, result.start);
        Assert.Equal(expectedDate, result.end);
    }

    [Theory]
    [InlineData("2024-01-01", "2024-01-01", "2024-01-07")]
    [InlineData("2024-01-02", "2024-01-01", "2024-01-07")]
    [InlineData("2024-01-03", "2024-01-01", "2024-01-07")]
    [InlineData("2024-01-04", "2024-01-01", "2024-01-07")]
    [InlineData("2024-01-05", "2024-01-01", "2024-01-07")]
    [InlineData("2024-01-06", "2024-01-01", "2024-01-07")]
    [InlineData("2024-01-07", "2024-01-01", "2024-01-07")]
    [InlineData("2024-03-01", "2024-02-26", "2024-03-03")]
    [InlineData("2024-02-29", "2024-02-26", "2024-03-03")]
    public void GetBoundaries_WhenWeekTye_ReturnsStartAndEndOfWeek(string input, string expetedStart, string expectedEnd)
    {
        var inputDate = DateOnly.FromDateTime(DateTime.Parse(input));
        var expectedStartDate = DateOnly.FromDateTime(DateTime.Parse(expetedStart));
        var expectedEndDate = DateOnly.FromDateTime(DateTime.Parse(expectedEnd));

        var result = PlanningCalculator.GetBoundaries(TaskItemPlanningType.Week, inputDate);

        Assert.Equal(expectedStartDate, result.start);
        Assert.Equal(expectedEndDate, result.end);
    }

    //generate test for GetBoundaries when MonthType with the same approach as above
    [Theory]
    [InlineData("2024-01-01", "2024-01-01", "2024-01-31")]
    [InlineData("2024-02-02", "2024-02-01", "2024-02-29")]
    [InlineData("2024-02-29", "2024-02-01", "2024-02-29")]
    [InlineData("2024-04-15", "2024-04-01", "2024-04-30")]
    [InlineData("2024-05-15", "2024-05-01", "2024-05-31")]
    public void GetBoundaries_WhenMonthType_ReturnsStartAndEndOfMonth(string input, string expetedStart, string expectedEnd)
    {
        var inputDate = DateOnly.FromDateTime(DateTime.Parse(input));
        var expectedStartDate = DateOnly.FromDateTime(DateTime.Parse(expetedStart));
        var expectedEndDate = DateOnly.FromDateTime(DateTime.Parse(expectedEnd));

        var result = PlanningCalculator.GetBoundaries(TaskItemPlanningType.Month, inputDate);

        Assert.Equal(expectedStartDate, result.start);
        Assert.Equal(expectedEndDate, result.end);
    }

}
