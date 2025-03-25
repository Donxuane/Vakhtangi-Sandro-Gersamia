using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.AuthenticationModels;

public class Register
{
    [Required(ErrorMessage ="Name is required!")]
    public required string Name { get; set; }
    [Required(ErrorMessage ="Surname is required!")]
    public required string Surname { get; set; }
    [Required(ErrorMessage ="Email is required!")]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [DataType(DataType.Password, ErrorMessage = "Enter password")]
    public required string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Password does not much!")]
    [DataType(DataType.Password, ErrorMessage = "Repeat password!")]
    public required string RepeatPassword { get; set; }
}
