using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public  class LimitsDto
{
    [Required]
    [CategoryTypeValidation(FinancialTypes.Expense)]
    public int CategoryId { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public int Period { get; set; }
    [Required]
    public DateTime DateAdded { get; set; }
    [Required]
    public Currencies Currencies { get; set; }
}
