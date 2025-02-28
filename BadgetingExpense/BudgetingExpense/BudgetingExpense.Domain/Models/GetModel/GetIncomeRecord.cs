namespace BudgetingExpense.Domain.Models.GetModel;

public class GetIncomeRecord
{
    public string UserId { get; set; }
    public int Period { get; set; }
    public int Currency { get; set; }
}
