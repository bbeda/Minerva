using MediatR;
using Minerva.Application.Common;

namespace Minerva.Application.Features.TaskItems;
public class RemoveTaskItemPlanCommand : IRequest<CommandResult<TaskItemPlanning>>
{
    public Guid TaskId { get; init; }

    public TaskItemPlanningType Types { get; init; }
}

internal class RemoveTaskItemPlanCommandHandler : IRequestHandler<RemoveTaskItemPlanCommand, CommandResult<TaskItemPlanning>>
{
    public Task<CommandResult<TaskItemPlanning>> Handle(RemoveTaskItemPlanCommand request, CancellationToken cancellationToken) => throw new NotImplementedException();
}
