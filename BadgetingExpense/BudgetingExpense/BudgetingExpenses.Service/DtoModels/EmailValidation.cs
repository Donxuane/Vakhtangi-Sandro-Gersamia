using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class EmailValidation
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Code { get; set; }
}
