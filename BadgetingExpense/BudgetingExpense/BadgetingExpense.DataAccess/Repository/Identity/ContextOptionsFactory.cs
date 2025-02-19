using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class ContextOptionsFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BudgetingAndExpenseTracker;Integrated Security=true;");
        return new AppDbContext(optionsBuilder.Options);
    }
}
