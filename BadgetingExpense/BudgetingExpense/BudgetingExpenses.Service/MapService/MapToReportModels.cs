using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;

namespace BudgetingExpenses.Service.MapService;

public static class MapToReportModels
{
    public static RecordCurrency Map(this GetRecordsCurrencyDto dto, string userId)
    {
        return new RecordCurrency
        {
            Currency = dto.Currency,
            Period = dto.Period,
            UserId = userId
        };
    }

    public static RecordsPeriod Map(this TopExpenseDto dto, string userId)
    {
        return new RecordsPeriod
        {
            Currency = dto.Currency,
            Period = dto.Period,
            UserId = userId
        };
    }

    public static RecordCategory Map(this GetRecordsCategoryDto dto, string userId)
    {
        return new RecordCategory
        {
            Category = dto.CategoryName,
            Period = dto.Period,
            UserId = userId
        };
    }


}
