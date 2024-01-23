using Microsoft.EntityFrameworkCore;
using Minerva.Application.Infrastructure;

namespace Minerva.Application.Features.TaskItems;
internal class TaskItemRepository(DataContext dataContext) : ITaskItemRepository
{
    public void Add(TaskItem taskItem) => dataContext.Add(taskItem);
    public async Task<TaskItem?> FindAsync(Guid taskItemId) =>
        await dataContext
        .TaskItems
        .Include(x => x.PlanEntries.Where(pe => pe.Status == TaskItemPlanEntryStatus.Planned))
        .FirstOrDefaultAsync(x => x.Id == taskItemId);
}
