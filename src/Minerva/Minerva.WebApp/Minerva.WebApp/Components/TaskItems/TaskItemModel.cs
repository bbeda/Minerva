namespace Minerva.WebApp.Components.TaskItems;

public class TaskItemModel
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string? Title { get; set; }
}
