using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.AuthenticationModels;

public class Login
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email Is Required!")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password Is Required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
