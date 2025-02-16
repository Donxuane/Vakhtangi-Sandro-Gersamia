using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class LoginDto
{
    [Required(AllowEmptyStrings = false,ErrorMessage ="Email Is Required!")]
    public string Email {  get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password Is Required!")]
    public string Password { get; set; }
}
