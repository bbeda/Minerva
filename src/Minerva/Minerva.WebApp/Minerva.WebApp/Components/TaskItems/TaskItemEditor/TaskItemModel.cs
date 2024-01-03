using System.ComponentModel.DataAnnotations;

namespace Minerva.WebApp.Components.TaskItems.TaskItemEditor;

public class TaskItemModel
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MinLength(3)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
}
