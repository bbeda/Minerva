using MediatR;

namespace Minerva.Application.Features.TaskItems;
public class TaskItemUpdated : INotification
{
    public required TaskItem TaskItem { get; init; }
}
