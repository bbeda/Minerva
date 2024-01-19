using Microsoft.EntityFrameworkCore;
using Minerva.Application.Infrastructure;

namespace Minerva.Application.Features.TaskItems;
internal class TaskItemRepository(DataContext dataContext) : ITaskItemRepository
{
    public void Add(TaskItem taskItem) => dataContext.Add(taskItem);
    public async Task<TaskItem?> FindAsync(Guid taskItemId) => await dataContext.TaskItems.FirstOrDefaultAsync(x => x.Id == taskItemId);
}
