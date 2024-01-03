using MediatR;
using Microsoft.EntityFrameworkCore;
using Minerva.Application.Infrastructure;

namespace Minerva.Application.Features.TaskItems;
public class QueryBacklogRequest : IStreamRequest<TaskItemListItem>
{
}

internal class QueryBacklogRequestHandler(DataContext dataContext) : IStreamRequestHandler<QueryBacklogRequest, TaskItemListItem>
{
    public IAsyncEnumerable<TaskItemListItem> Handle(QueryBacklogRequest request, CancellationToken cancellationToken)
    {
        return dataContext.TaskItems.Select(ti => new TaskItemListItem
        {
            Id = ti.Id,
            Title = ti.Title,
            Description = ti.Description,
            DueDate = ti.DueDate,
            CreatedOn = ti.CreatedAt,
            IsCompleted = false
        }).AsAsyncEnumerable();
    }
}
