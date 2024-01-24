using MediatR;
using Minerva.Application.Common;
using Minerva.Application.Infrastructure;

namespace Minerva.Application.Features.TaskItems;
public class PlanTaskItemCommand : IRequest<CommandResult>
{
    public Guid TaskItemId { get; init; }
    public TaskItemPlanningPeriond PlanType { get; init; }
    public DateOnly Date { get; init; }
}

internal class PlanTaskItemCommandHandler(
       ITaskItemRepository taskItemRepository,
       IUnitOfWork unitOfWork)
    : IRequestHandler<PlanTaskItemCommand, CommandResult>
{
    public async Task<CommandResult> Handle(PlanTaskItemCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await taskItemRepository.FindAsync(request.TaskItemId);

        if (taskItem is null)
        {
            return new CommandResult("Not found");
        }

        taskItem.Plan(request.PlanType, request.Date);

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CommandResult();
    }
}
