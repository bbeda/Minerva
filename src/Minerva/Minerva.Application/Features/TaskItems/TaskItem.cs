using Minerva.Application.Common;
using System.Diagnostics.CodeAnalysis;

namespace Minerva.Application.Features.TaskItems;
public class TaskItem : Entity
{
    public required string Title { get; set; }

    public required string? Description { get; set; }

    public required DateOnly? DueDate { get; set; }

    public TaskItemStatus Status { get; private set; }

    public DateTimeOffset? CompletedOn { get; private set; }

    public TaskItemPlanning Planning { get; set; } = TaskItemPlanning.None;

    [SetsRequiredMembers]
    public TaskItem(
        string title,
        string? description,
        DateOnly? dueDate,
        TaskItemStatus status = TaskItemStatus.Open)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
    }

    public void Complete()
    {
        Status = TaskItemStatus.Complete;
        CompletedOn = DateTime.UtcNow;
    }
}

public enum TaskItemStatus
{
    Open,
    Complete
}
