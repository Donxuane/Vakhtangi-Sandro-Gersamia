using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels.ReportsDtoModels;

public class RecordsDto
{
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; }
    public string? CategoryName { get; set; }
}
