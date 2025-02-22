using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname {  get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password,ErrorMessage ="Enter Password")]
    public string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Password does not much!")]
    [DataType(DataType.Password, ErrorMessage ="Repeat Password!")]
    public string RepeatPassword { get; set; }
}
