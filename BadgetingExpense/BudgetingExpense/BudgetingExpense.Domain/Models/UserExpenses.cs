using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models;

public class UserExpenses
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(10)]
    public string Currency { get; set; }
    [Required]
    [MaxLength(50)]
    public string ExpenseType { get; set; }
    [Required]
    public string UserId { get; set; }
}
