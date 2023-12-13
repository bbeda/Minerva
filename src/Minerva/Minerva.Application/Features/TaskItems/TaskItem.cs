using Minerva.Application.Common;
using System.Diagnostics.CodeAnalysis;

namespace Minerva.Application.Features.TaskItems;
public class TaskItem : Entity
{
    public required string Title { get; set; }

    public required string Description { get; set; }

    public required DateOnly? DueDate { get; set; }

    [SetsRequiredMembers]
    public TaskItem(string title, string description, DateOnly? dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
    }
}
