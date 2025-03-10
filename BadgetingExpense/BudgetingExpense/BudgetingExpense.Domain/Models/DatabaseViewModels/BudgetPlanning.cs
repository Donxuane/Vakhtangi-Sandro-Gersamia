using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.Models.DatabaseViewModels;

public class BudgetPlanning
{
    public string UserId { get; set; }

    public double LimitAmount { get; set; }
    public int LimitPeriod { get; set; }
    public DateTime DateAdded { get; set; }
    public double ExpenseAmount { get; set; }
    public Currencies Currency { get; set; }
    public DateTime Date { get; set; }

}
