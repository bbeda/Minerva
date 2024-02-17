using MediatR;
using Microsoft.EntityFrameworkCore;
using Minerva.Application.Infrastructure;

namespace Minerva.Application.Features.TaskItems;
public class QueryBacklogRequest : IStreamRequest<TaskItemListItem>
{
    public required TaskItemStatus[] Statuses { get; set; }
}

internal class QueryBacklogRequestHandler(DataContext dataContext) : IStreamRequestHandler<QueryBacklogRequest, TaskItemListItem>
{
    public IAsyncEnumerable<TaskItemListItem> Handle(QueryBacklogRequest request, CancellationToken cancellationToken)
    {
        return dataContext.TaskItems
            .Where(ti => request.Statuses.Contains(ti.Status))
            .Select(ti => new TaskItemListItem
            {
                Id = ti.Id,
                Title = ti.Title,
                Description = ti.Description,
                DueDate = ti.DueDate,
                CreatedOn = ti.CreatedAt,
                IsCompleted = ti.Status == TaskItemStatus.Complete,
                Planning = ti.Planning
            }).AsAsyncEnumerable();
    }
}
