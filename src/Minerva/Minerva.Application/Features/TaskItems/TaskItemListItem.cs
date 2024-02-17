
namespace Minerva.Application.Features.TaskItems;
public class TaskItemListItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required DateOnly? DueDate { get; init; }
    public required DateTimeOffset CreatedOn { get; init; }
    public required bool IsCompleted { get; init; }
    public required TaskItemPlanning Planning { get; set; }
}
