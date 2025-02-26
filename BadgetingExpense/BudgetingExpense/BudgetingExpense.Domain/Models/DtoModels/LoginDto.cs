using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class LoginDto
{
    [Required(AllowEmptyStrings = false,ErrorMessage ="Email Is Required!")]
    [EmailAddress]
    public string Email {  get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password Is Required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
