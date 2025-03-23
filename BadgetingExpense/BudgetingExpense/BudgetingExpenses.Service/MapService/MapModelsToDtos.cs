using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;

namespace BudgetingExpenses.Service.MapService;

public static class MapModelsToDtos
{
    public static RecordsDto Map(this IncomeRecord model)
    {
        return new RecordsDto
        {
            Amount = model.Amount,
            CategoryName = model.CategoryName,
            Currency = model.Currency.ToString(),
            Date = model.IncomeDate
        };
    }

    public static RecordsDto Map(this ExpenseRecord model)
    {
        return new RecordsDto
        {
            Amount = model.Amount,
            CategoryName = model.CategoryName,
            Currency = model.Currency.ToString(),
            Date = model.Date
        };
    }
}
