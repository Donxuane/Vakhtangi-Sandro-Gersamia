using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.MapService;

public static class MapToMainFinanceModels
{
    public static Income Map(this IncomeDto dto,string userId)
    {
        return new Income
        {
            Amount = dto.Amount,
            CategoryId = dto.CategoryId,
            Currency = dto.Currency,
            Date = dto.Date,
            UserId = userId,
        };
    }

    public static Update Map(this UpdateIncomeDto dto, string userId)
    {
        return new Update
        {
            Amount = dto.Amount,
            CategoryId = dto.CategoryId,
            Currency = dto.Currency,
            Date = dto.Date,
            UserId = userId,
            Id = dto.Id,
        };
    }

    public static Update Map(this UpdateExpenseDto dto,string userId)
    {
        return new Update
        {
            Amount = dto.Amount,
            Date = dto.Date,
            CategoryId = dto.CategoryId,
            Currency = dto.Currency,
            Id = dto.Id,
            UserId = userId
        };
    }

    public static Expense Map(this ExpenseDto dto, string userId)
    {
        return new Expense
        {
            Amount = dto.Amount,
            Date = dto.Date,
            CategoryId = dto.CategoryId,
            Currency = dto.Currency,
            UserId = userId
        };
    }

    public static Limits Map(this LimitsDto dto, string userId)
    {
        return new Limits
        {
            Amount = dto.Amount,
            CategoryId = dto.CategoryId,
            DateAdded = dto.DateAdded,
            PeriodCategory = dto.Period,
            Currency = dto.Currencies,
            UserId = userId
        };
    }

    public static Limits Map(this UpdateLimitDto dto, string userId)
    {
        return new Limits
        {
            Id = dto.Id,
            Amount = dto.Amount,
            CategoryId = dto.CategoryId,
            PeriodCategory = dto.PeriodCategory,
            DateAdded = dto.StartupDate,
            UserId = userId
        };
    }
}
