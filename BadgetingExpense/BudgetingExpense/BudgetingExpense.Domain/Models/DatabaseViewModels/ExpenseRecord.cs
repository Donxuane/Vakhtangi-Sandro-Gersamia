namespace BudgetingExpense.Domain.Models.DatabaseViewModels;

public class ExpenseRecord
{
    public string UserId {  get; set; }
    public double Amount { get; set; }
    public int Currency { get; set; }
    public DateTime Date {  get; set; }
    public string? CategoryName {  get; set; }
}
