using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels.ReportsDtoModels;

public class GetRecordsCurrencyDto
{
    public int Period { get; set; }
    public Currencies Currency { get; set; }
}
