using BudgetingExpense.Domain.CustomAttributes;
using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class UpdateExpenseDto
{
    [Required]
    public int Id { get; set; }
    public Currencies Currency { get; set; }
    public double Amount { get; set; }
    [CategoryTypeValidation(FinancialTypes.Expense)]
    public int CategoryId { get; set; }
    public DateTime Date { get; set; }
}