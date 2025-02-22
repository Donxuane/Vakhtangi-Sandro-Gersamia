using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int Currency { get; set; }
    [Required]
    public double Amount {  get; set; } 
    public int? CategoryId {  get; set; }
    [Required]
    public DateTime Date {  get; set; }
    [Required]
    public string UserId { get; set; }
}
