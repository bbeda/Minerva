using MediatR;
using Minerva.Application.Common;

namespace Minerva.Application.Features.TaskItems;
public class CreateTaskItemCommand : IRequest<CommandResult<TaskItem>>
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateOnly? DueDate { get; set; }
}

internal class CreateTaskItemCommandHandler(
    ITaskItemRepository taskItemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskItemCommand, CommandResult<TaskItem>>
{
    public async Task<CommandResult<TaskItem>> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        TaskItem taskItem = new(request.Title, request.Description, request.DueDate);
        taskItemRepository.Add(taskItem);

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);
        return new CommandResult<TaskItem>(taskItem);
    }
}
