using MediatR;
using Microsoft.EntityFrameworkCore;
using Minerva.Application.Common;
using Minerva.Application.Infrastructure;

namespace Minerva.Application.Features.TaskItems;
public class CheckAndUpdateTaskItemPlanStatusCommand : IRequest<CommandResult>
{
}

internal class CheckAndUpdateTaskItemPlanStatusCommandHandler(DataContext dataContext, IUnitOfWork unitOfWork) : IRequestHandler<CheckAndUpdateTaskItemPlanStatusCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CheckAndUpdateTaskItemPlanStatusCommand request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekStart = PlanningCalculator.GetWeekStart(today);
        var monthStart = PlanningCalculator.GetMonthStart(today);

        var itemsIterator = dataContext.TaskItems
            .Where(x => x.Status != TaskItemStatus.Complete && ((x.Planning.DayValue != null && x.Planning.DayValue.Value < today) || (x.Planning.WeekValue != null && x.Planning.WeekValue < weekStart) || x.Planning.MonthValue != null && x.Planning.MonthValue < monthStart)).AsAsyncEnumerable();

        await foreach (var item in itemsIterator)
        {
            if (item.Planning.DayValue != null && item.Planning.DayValue.Value < today)
            {
                dataContext.TaskItemPlanningResultItems.Add(new TaskItemPlanningResultItem()
                {
                    PlanningDate = item.Planning.DayValue.Value,
                    PlanningType = TaskItemPlanningType.Day,
                    Result = TaskItemPlanningResultOption.Failed,
                    TaskItemId = item.Id,
                    TenantId = item.TenantId
                });
                item.RemovePlans(TaskItemPlanningType.Day);
            }

            if (item.Planning.WeekValue != null && item.Planning.WeekValue < weekStart)
            {
                dataContext.TaskItemPlanningResultItems.Add(new TaskItemPlanningResultItem()
                {
                    PlanningDate = item.Planning.WeekValue.Value,
                    PlanningType = TaskItemPlanningType.Week,
                    Result = TaskItemPlanningResultOption.Failed,
                    TaskItemId = item.Id,
                    TenantId = item.TenantId
                });
                item.RemovePlans(TaskItemPlanningType.Week);
            }

            if (item.Planning.MonthValue != null && item.Planning.MonthValue < monthStart)
            {
                dataContext.TaskItemPlanningResultItems.Add(new TaskItemPlanningResultItem()
                {
                    PlanningDate = item.Planning.MonthValue.Value,
                    PlanningType = TaskItemPlanningType.Month,
                    Result = TaskItemPlanningResultOption.Failed,
                    TaskItemId = item.Id,
                    TenantId = item.TenantId
                });
                item.RemovePlans(TaskItemPlanningType.Month);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CommandResult.Success;
    }
}