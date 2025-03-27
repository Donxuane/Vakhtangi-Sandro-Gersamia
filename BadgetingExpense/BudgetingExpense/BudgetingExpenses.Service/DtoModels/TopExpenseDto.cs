using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels;

public class TopExpenseDto
{
    public int Period { get; set; }
    public Currencies Currency { get; set; }
}
