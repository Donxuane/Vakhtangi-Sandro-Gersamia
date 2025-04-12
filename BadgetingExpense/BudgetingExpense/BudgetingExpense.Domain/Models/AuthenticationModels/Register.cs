using BudgetingExpense.Domain.CustomBinder;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BudgetingExpense.Domain.CustomValidationAttributes;

namespace BudgetingExpense.Domain.Models.AuthenticationModels;
[ModelBinder(typeof(CustomModelBinder<Register>))]
public class Register
{
    [Required(ErrorMessage ="Name is required!")]
    [ValidateInput]
    public string Name { get; set; }
    [Required(ErrorMessage ="Surname is required!")]
    [ValidateInput]
    public string Surname { get; set; }
    [Required(ErrorMessage ="Email is required!")]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password, ErrorMessage = "Enter password")]
    public string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Password does not much!")]
    [DataType(DataType.Password, ErrorMessage = "Repeat password!")]
    public string RepeatPassword { get; set; }
}
