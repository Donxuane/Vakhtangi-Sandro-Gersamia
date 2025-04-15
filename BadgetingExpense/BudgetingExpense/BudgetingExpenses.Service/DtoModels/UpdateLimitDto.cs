using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels;

public class UpdateLimitDto
{
    public int Id { get; set; }
    [CategoryType(FinancialTypes.Expense)]
    public int CategoryId { get; set; }
    public double Amount { get; set; }
    public int PeriodCategory { get; set; }
    public DateTime StartupDate {  get; set; }
}
