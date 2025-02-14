using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Identity;
namespace BudgetingAndExpenseTracker.DataAccess.IdentityDbContext;


public class AppDbContext : IdentityDbContext<>
{
}
