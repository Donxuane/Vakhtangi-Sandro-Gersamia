using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.Models.DatabaseViewModels;

public class LimitationsView
{
    public double LimitAmount { get; set; }
    public int LimitPeriod { get; set; }
    public DateTime DateAdded { get; set; }
    public int CategoryId { get; set; }
    public double TotalExpenses { get; set; }  
    public int ExpenseCount {  get; set; }
    public Currencies Currency { get; set; }
}
