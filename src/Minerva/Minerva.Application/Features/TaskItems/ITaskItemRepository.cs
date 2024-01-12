namespace Minerva.Application.Features.TaskItems;
internal interface ITaskItemRepository
{
    Task<TaskItem?> FindAsync(Guid taskItemId);

    void Add(TaskItem taskItem);
}
