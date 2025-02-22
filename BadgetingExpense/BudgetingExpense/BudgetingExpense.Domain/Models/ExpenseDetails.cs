using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models;

public class ExpenseDetails
{
    [Key]
    public int Id { get; set; }
    [Required]
    public decimal ExpenseAmount { get; set; }
    [Required]
    public DateOnly ExpenseDate { get; set; }
    [Required]
    public int UserExpenseId {  get; set; }
}
