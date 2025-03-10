using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.Models.DatabaseViewModels;

public class ExpenseRecord
{
    public string UserId {  get; set; }
    public double Amount { get; set; }
    public Currencies Currency { get; set; }
    public DateTime Date {  get; set; }
    public string? CategoryName {  get; set; }
}
