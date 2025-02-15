using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class AppDbContext : IdentityDbContext<IdentityModel>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
