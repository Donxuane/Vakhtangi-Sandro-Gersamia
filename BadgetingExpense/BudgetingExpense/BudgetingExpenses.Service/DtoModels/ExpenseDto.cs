using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class ExpenseDto
{
    [Required]
    public Currencies Currency {  get; set; }
    [Required]
    public double Amount {  get; set; }
    [CategoryType(FinancialTypes.Expense)]
    public int? CategoryId {  get; set; }
    [Required]
    public DateTime Date {  get; set; }
}
