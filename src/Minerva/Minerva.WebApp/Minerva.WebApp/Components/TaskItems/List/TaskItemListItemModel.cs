﻿using Minerva.Application.Features.TaskItems;

namespace Minerva.WebApp.Components.TaskItems.List;

public class TaskItemListItemModel
{
    public required Guid Id { get; init; }

    public required string Title { get; init; }

    public string? Description { get; init; }

    public DateOnly? DueDate { get; init; }

    public required DateTimeOffset CreatedOn { get; init; }

    public bool IsCompleted { get; set; }

    public IReadOnlyCollection<PlanningOption> Planning { get; set; } = [];

    public bool CanPlan => Planning is null || Planning.Count < 3;
}

public record PlanningOption(TaskItemPlanningPeriond Period, DateOnly StartDate);
