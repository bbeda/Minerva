namespace Minerva.WebApp.Components.TaskItems.List;

public class TaskItemListItemModel
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public string? Description { get; init; }

    public DateOnly? DueDate { get; init; }

    public required DateTimeOffset CreatedOn { get; init; }
}
