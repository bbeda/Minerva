using MediatR;
using Minerva.Application.Common;

namespace Minerva.Application.Features.TaskItems;
public record CompleteTaskItemCommand : IRequest<CommandResult>
{
    public required Guid TaskItemId { get; init; }
}

internal class CompleteTaskItemCommandHandler(
    ITaskItemRepository taskItemRepository,
    IUnitOfWork unitOfWork)
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
        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CommandResult();
    }
}
