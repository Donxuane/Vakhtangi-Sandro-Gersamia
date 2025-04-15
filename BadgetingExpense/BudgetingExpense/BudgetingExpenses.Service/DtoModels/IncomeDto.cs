using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class IncomeDto
{
    [Required]
    public Currencies Currency { get; set; }
    [Required]
    public double Amount {  get; set; }
    [Required]
    [CategoryType(FinancialTypes.Income)]
    public int CategoryId {  get; set; }
    [Required]
    public DateTime Date { get; set; }
}
