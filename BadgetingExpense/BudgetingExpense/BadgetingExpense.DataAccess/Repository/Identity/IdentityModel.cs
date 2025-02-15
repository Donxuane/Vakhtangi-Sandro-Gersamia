using Microsoft.AspNetCore.Identity;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class IdentityModel : IdentityUser
{
    public string UserSurname { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool? Notifications {  get; set; }
}
