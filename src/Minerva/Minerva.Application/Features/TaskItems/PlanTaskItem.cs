using MediatR;
using Minerva.Application.Common;

namespace Minerva.Application.Features.TaskItems;
public class PlanTaskItemCommand : IRequest<CommandResult<TaskItemPlanning>>
{
    public Guid TaskItemId { get; init; }
    public TaskItemPlanningType PlanType { get; init; }
    public DateOnly Date { get; init; }
}

internal class PlanTaskItemCommandHandler(
       ITaskItemRepository taskItemRepository,
       IUnitOfWork unitOfWork)
    : IRequestHandler<PlanTaskItemCommand, CommandResult<TaskItemPlanning>>
{
    public async Task<CommandResult<TaskItemPlanning>> Handle(PlanTaskItemCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await taskItemRepository.FindAsync(request.TaskItemId);

        if (taskItem is null)
        {
            return new CommandResult<TaskItemPlanning>("Not found");
        }

        taskItem.Plan(request.PlanType, request.Date);

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CommandResult<TaskItemPlanning>(taskItem.Planning);
    }
}