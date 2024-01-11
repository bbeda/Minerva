
namespace Minerva.Application.Features.TaskItems;
public class TaskItemListItem
{
    public Guid Id { get; internal set; }
    public string Title { get; internal set; }
    public string? Description { get; internal set; }
    public DateOnly? DueDate { get; internal set; }
    public DateTimeOffset CreatedOn { get; internal set; }
    public bool IsCompleted { get; internal set; }
}
