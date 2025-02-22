using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models;

public class Category
{
    public int? Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int? Type {  get; set; }
}
