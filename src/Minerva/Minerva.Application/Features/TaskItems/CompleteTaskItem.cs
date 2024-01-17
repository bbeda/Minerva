using MediatR;
using Minerva.Application.Common;

namespace Minerva.Application.Features.TaskItems;
public record CompleteTaskItemCommand : IRequest<CommandResult>
{
    public required Guid TaskItemId { get; init; }
}

public record TaskCompletedNotification : INotification
{
    public Guid TaskItemId { get; init; }
}

internal class CompleteTaskItemCommandHandler(
    ITaskItemRepository taskItemRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<CompleteTaskItemCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CompleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await taskItemRepository.FindAsync(request.TaskItemId);

        if (taskItem is null)
        {
            return new CommandResult("Not found");
        }

        taskItem.Complete();
        await mediator.Publish(new TaskCompletedNotification { TaskItemId = taskItem.Id }, cancellationToken);

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CommandResult();
    }
}
