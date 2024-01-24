
namespace Minerva.Application.Features.TaskItems;
public class TaskItemListItem
{
    public Guid Id { get; internal set; }
    public string Title { get; internal set; }
    public string? Description { get; internal set; }
    public DateOnly? DueDate { get; internal set; }
    public DateTimeOffset CreatedOn { get; internal set; }
    public bool IsCompleted { get; internal set; }
    public TaskItemPlanning? Planning { get; internal set; }
}

public record TaskItemPlanning(TaskItemPlanningOption? Daily, TaskItemPlanningOption? Weekly, TaskItemPlanningOption? Monthly)
{
    public IReadOnlyCollection<TaskItemPlanningOption> Options => new[] { Daily, Weekly, Monthly }.Where(x => x is not null).Select(x => x!).ToArray();
}

public record TaskItemPlanningOption(TaskItemPlanningPeriond Type, DateOnly StartDate)
{
    public static TaskItemPlanningOption? From(TaskItemPlanEntry? entry)
    {
        if (entry is null)
        {
            return null;
        }

        return new TaskItemPlanningOption(entry.Type, entry.StartDate);
    }
}
