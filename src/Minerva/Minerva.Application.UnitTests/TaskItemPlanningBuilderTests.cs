using Minerva.Application.Features.TaskItems;

namespace Minerva.Application.UnitTests;
public class TaskItemPlanningBuilderTests
{
    [Fact]
    public void WithDay_WhenNoPreviousOptions_ShouldCreateDayPlanning()
    {
        var builder = new TaskItemPlanningBuilder(TaskItemPlanning.None);

        var result = builder.WithDay(new DateOnly(2024, 1, 1)).Build();

        Assert.Equal(new DateOnly(2024, 1, 1), result.DayValue);
        Assert.Equal(TaskItemPlanningType.Day, result.PlanningType);
        Assert.Null(result.WeekValue);
        Assert.Null(result.MonthValue);
    }

    [Fact]
    public void WithDay_WhenPreviousOptionInBoundary_ShouldUpateDayAndKeepWMPlanning()
    {
        var taskItemPlanning = new TaskItemPlanning(new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 15));
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithDay(new DateOnly(2024, 1, 7));
        var result = builder.Build();

        Assert.Equal(new DateOnly(2024, 1, 7), result.DayValue);
        Assert.Equal(TaskItemPlanningType.Day | TaskItemPlanningType.Week | TaskItemPlanningType.Month, result.PlanningType);
        Assert.Equal(new DateOnly(2024, 1, 1), result.WeekValue);
        Assert.Equal(new DateOnly(2024, 1, 1), result.MonthValue);
    }

    [Fact]
    public void WithDay_WhenPreviousOptionOutsideWeekBoundary_ShouldUpateDayAndKeepMonthPlanning()
    {
        var taskItemPlanning = new TaskItemPlanning(new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 15));
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithDay(new DateOnly(2024, 1, 10));
        var result = builder.Build();

        Assert.Equal(new DateOnly(2024, 1, 10), result.DayValue);
        Assert.Equal(TaskItemPlanningType.Day | TaskItemPlanningType.Month, result.PlanningType);
        Assert.Null(result.WeekValue);
        Assert.Equal(new DateOnly(2024, 1, 1), result.MonthValue);
    }

    [Fact]
    public void WithWeek_WhenNoPreviousOption_ShouldCreatePlanning()
    {
        var taskItemPlanning = TaskItemPlanning.None;
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithWeek(new DateOnly(2024, 1, 10));
        var result = builder.Build();

        Assert.Null(result.DayValue);
        Assert.Equal(TaskItemPlanningType.Week, result.PlanningType);
        Assert.Equal(new DateOnly(2024, 1, 8), result.WeekValue);
        Assert.Null(result.MonthValue);
    }

    [Fact]
    public void WithWeek_WhenPreviousOptionInBoundary_ShouldUpdatePlanning()
    {
        var taskItemPlanning = new TaskItemPlanning(new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 6), new DateOnly(2024, 1, 15));
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithWeek(new DateOnly(2024, 1, 10));
        var result = builder.Build();

        Assert.Null(result.DayValue);
        Assert.Equal(TaskItemPlanningType.Week | TaskItemPlanningType.Month, result.PlanningType);
        Assert.Equal(new DateOnly(2024, 1, 8), result.WeekValue);
        Assert.Equal(new DateOnly(2024, 1, 1), result.MonthValue);
    }

    [Fact]
    public void WithWeek_WhenPreviousOptionOutsideBoundary_ShouldUpdatePlanning()
    {
        var taskItemPlanning = new TaskItemPlanning(new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 6), new DateOnly(2024, 1, 15));
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithWeek(new DateOnly(2024, 2, 10));
        var result = builder.Build();

        Assert.Null(result.DayValue);
        Assert.Equal(TaskItemPlanningType.Week, result.PlanningType);
        Assert.Equal(new DateOnly(2024, 2, 5), result.WeekValue);
        Assert.Null(result.MonthValue);
    }

    [Fact]
    public void WithWeek_WhenNullPreviousWeekInBoundary_ShouldUpdatePlanning()
    {
        var taskItemPlanning = new TaskItemPlanning(new DateOnly(2024, 1, 5), null, new DateOnly(2024, 1, 15));
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithWeek(new DateOnly(2024, 1, 7));
        var result = builder.Build();

        Assert.Equal(new DateOnly(2024, 1, 5), result.DayValue);
        Assert.Equal(TaskItemPlanningType.Week | TaskItemPlanningType.Month | TaskItemPlanningType.Day, result.PlanningType);
        Assert.Equal(new DateOnly(2024, 1, 1), result.WeekValue);
        Assert.Equal(new DateOnly(2024, 1, 1), result.MonthValue);
    }

    [Fact]
    public void WithMonth_WhenNullPreviousMonthInBoundary_ShouldUpdatePlanning()
    {
        var taskItemPlanning = new TaskItemPlanning(new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 5), null);
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithMonth(new DateOnly(2024, 1, 7));
        var result = builder.Build();

        Assert.Equal(new DateOnly(2024, 1, 5), result.DayValue);
        Assert.Equal(TaskItemPlanningType.Week | TaskItemPlanningType.Month | TaskItemPlanningType.Day, result.PlanningType);
        Assert.Equal(new DateOnly(2024, 1, 1), result.WeekValue);
        Assert.Equal(new DateOnly(2024, 1, 1), result.MonthValue);
    }

    [Fact]
    public void WithMonth_WhenNullPreviousMonthOutsideBoundary_ShouldUpdatePlanning()
    {
        var taskItemPlanning = new TaskItemPlanning(new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 5), null);
        var builder = new TaskItemPlanningBuilder(taskItemPlanning);
        builder.WithMonth(new DateOnly(2024, 2, 7));
        var result = builder.Build();

        Assert.Equal(new DateOnly(2024, 2, 1), result.MonthValue);
        Assert.Equal(TaskItemPlanningType.Month, result.PlanningType);
        Assert.Null(result.DayValue);
        Assert.Null(result.WeekValue);
    }
}
