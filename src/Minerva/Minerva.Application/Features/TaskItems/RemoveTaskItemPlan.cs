using MediatR;
using Minerva.Application.Common;

namespace Minerva.Application.Features.TaskItems;
public class RemoveTaskItemPlanCommand : IRequest<CommandResult<TaskItemPlanning>>
{
    public Guid TaskItemId { get; init; }

    public TaskItemPlanningType Types { get; init; }
}

internal class RemoveTaskItemPlanCommandHandler(ITaskItemRepository taskItemRepository, IUnitOfWork unitOfWork) : IRequestHandler<RemoveTaskItemPlanCommand, CommandResult<TaskItemPlanning>>
{
    public async Task<CommandResult<TaskItemPlanning>> Handle(RemoveTaskItemPlanCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await taskItemRepository.FindAsync(request.TaskItemId);

        if (taskItem is null)
        {
            return new CommandResult<TaskItemPlanning>("Not found");
        }

        taskItem.RemovePlans(request.Types);

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CommandResult<TaskItemPlanning>(taskItem.Planning);
    }
}
