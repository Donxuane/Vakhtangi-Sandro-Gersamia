using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.Models.GetModel.Reports;

public class SavingsPeriod
{
    public Currencies Currency { get; set; }
    public double AverageIncome { get; set; }
    public double AverageExpense { get; set; }
    public double Savings {  get; set; }
    public double Percentage { get; set; }
}
