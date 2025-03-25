
using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels;

public class UpdateExpenseDto
{
    public int Id { get; set; }
    public Currencies? Currency { get; set; }
    public double? Amount { get; set; }
    [CategoryTypeValidation(FinancialTypes.Expense)]
    public int? CategoryId { get; set; }
    public DateTime? Date { get; set; }
}