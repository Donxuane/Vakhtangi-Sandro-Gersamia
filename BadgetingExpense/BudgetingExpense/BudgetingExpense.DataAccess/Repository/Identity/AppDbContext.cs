using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class AppDbContext : IdentityDbContext<IdentityModel>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityModel>()
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}
