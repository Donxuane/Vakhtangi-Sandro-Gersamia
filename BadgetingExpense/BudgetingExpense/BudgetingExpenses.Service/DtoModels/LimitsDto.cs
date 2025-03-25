using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels;

public  class LimitsDto
{
    [CategoryTypeValidation(FinancialTypes.Expense)]
    public int CategoryId { get; set; }
    public double Amount { get; set; }
    public int Period { get; set; }
    public DateTime DateAdded { get; set; }
}
