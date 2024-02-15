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

    [Theory]
    [InlineData(TaskItemPlanningType.Day, "2024-01-01", TaskItemPlanningType.Day, "2024-01-01")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-03", TaskItemPlanningType.Week, "2024-01-01")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-03", TaskItemPlanningType.Week, "2024-01-05")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-07", TaskItemPlanningType.Week, "2024-01-05")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-01", TaskItemPlanningType.Month, "2024-01-01")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-15", TaskItemPlanningType.Month, "2024-01-20")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-31", TaskItemPlanningType.Month, "2024-01-15")]
    [InlineData(TaskItemPlanningType.Week, "2024-01-01", TaskItemPlanningType.Week, "2024-01-03")]
    [InlineData(TaskItemPlanningType.Week, "2024-01-03", TaskItemPlanningType.Week, "2024-01-01")]
    [InlineData(TaskItemPlanningType.Week, "2024-03-03", TaskItemPlanningType.Month, "2024-02-29")]
    [InlineData(TaskItemPlanningType.Week, "2024-02-29", TaskItemPlanningType.Month, "2024-03-02")]
    public void IsInBoundary_WhenInBoundary_ReturnsTrue(TaskItemPlanningType type, string date, TaskItemPlanningType boundaryType, string boundaryDate)
    {
        var inputDateValue = DateOnly.FromDateTime(DateTime.Parse(date));
        var boundaryDateValue = DateOnly.FromDateTime(DateTime.Parse(boundaryDate));

        var result = PlanningCalculator.IsInBoundary(type, inputDateValue, boundaryType, boundaryDateValue);

        Assert.True(result);
    }


    [Theory]
    [InlineData(TaskItemPlanningType.Day, "2024-01-01", TaskItemPlanningType.Day, "2024-01-02")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-03", TaskItemPlanningType.Week, "2024-01-08")]
    [InlineData(TaskItemPlanningType.Day, "2024-01-01", TaskItemPlanningType.Month, "2024-02-01")]
    [InlineData(TaskItemPlanningType.Day, "2023-12-15", TaskItemPlanningType.Month, "2024-01-20")]
    [InlineData(TaskItemPlanningType.Week, "2024-01-02", TaskItemPlanningType.Day, "2024-01-09")]
    [InlineData(TaskItemPlanningType.Week, "2024-01-01", TaskItemPlanningType.Week, "2024-01-09")]
    [InlineData(TaskItemPlanningType.Week, "2024-03-06", TaskItemPlanningType.Month, "2024-02-25")]
    [InlineData(TaskItemPlanningType.Week, "2024-02-29", TaskItemPlanningType.Month, "2024-04-02")]
    [InlineData(TaskItemPlanningType.Month, "2024-01-03", TaskItemPlanningType.Day, "2024-02-07")]
    [InlineData(TaskItemPlanningType.Month, "2024-01-01", TaskItemPlanningType.Week, "2024-01-09")]
    [InlineData(TaskItemPlanningType.Month, "2024-01-01", TaskItemPlanningType.Week, "2024-01-04")]
    [InlineData(TaskItemPlanningType.Month, "2024-02-29", TaskItemPlanningType.Week, "2024-03-02")]
    [InlineData(TaskItemPlanningType.Month, "2024-03-03", TaskItemPlanningType.Week, "2024-02-29")]
    [InlineData(TaskItemPlanningType.Month, "2024-01-05", TaskItemPlanningType.Week, "2024-01-07")]
    [InlineData(TaskItemPlanningType.Month, "2022-12-26", TaskItemPlanningType.Week, "2023-01-01")]

    public void IsInBoundary_WhenNotInBoundary_ReturnsFalse(TaskItemPlanningType type, string date, TaskItemPlanningType boundaryType, string boundaryDate)
    {
        var inputDateValue = DateOnly.FromDateTime(DateTime.Parse(date));
        var boundaryDateValue = DateOnly.FromDateTime(DateTime.Parse(boundaryDate));

        var result = PlanningCalculator.IsInBoundary(type, inputDateValue, boundaryType, boundaryDateValue);

        Assert.False(result);
    }

}
