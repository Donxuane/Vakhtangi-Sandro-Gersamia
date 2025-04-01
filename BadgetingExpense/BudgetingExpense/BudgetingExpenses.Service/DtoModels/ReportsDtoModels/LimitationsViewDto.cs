using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels.ReportsDtoModels;

public class LimitationsViewDto
{
    public string Category { get; set; }
    public string Currency { get; set; }
    public double LimitAmount { get; set; }
    public double TotalExpenses { get; set; }
    public int RecordsCount { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime ValidTo { get; set; }   
}
