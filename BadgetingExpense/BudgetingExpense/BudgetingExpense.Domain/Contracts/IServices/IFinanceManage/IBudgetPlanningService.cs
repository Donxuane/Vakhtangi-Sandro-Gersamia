namespace BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;

public interface IBudgetPlanningService
{
    public Task SendMessageAsync(string UserId);
    public Task AllExpenses(string UserId, int CategoryId);
}
