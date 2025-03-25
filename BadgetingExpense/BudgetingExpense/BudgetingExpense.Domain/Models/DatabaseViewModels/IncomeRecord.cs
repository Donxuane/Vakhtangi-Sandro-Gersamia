using BudgetingExpense.Domain.Enums;
namespace BudgetingExpense.Domain.Models.DatabaseViewModels;

public class IncomeRecord
{
    public string UserId { get; set; }
    public DateTime IncomeDate { get; set; }
    public double Amount { get; set; }
    public Currencies Currency { get; set; }
    public string? CategoryName { get; set; }
}
