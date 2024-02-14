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
}
