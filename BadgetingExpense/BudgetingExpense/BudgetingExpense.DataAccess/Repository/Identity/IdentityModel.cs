using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class IdentityModel : IdentityUser
{
    [Required]
    public string Name {  get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public DateTime RegisterDate { get; set; }
    public bool? Notifications {  get; set; }
}
