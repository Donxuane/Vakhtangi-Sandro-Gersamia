using BudgetingExpense.DataAccess.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Configuration;

public static class ConfigureDatabaseCredentials
{
    public static void ConfigureDatabaseRules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("default"), b => b.MigrationsAssembly("BudgetingExpense.DataAccess")));

        services.AddIdentity<IdentityModel, IdentityRole>(options =>
        {
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<DbConnection>(sp =>
            new SqlConnection(configuration.GetConnectionString("default")));
    }
}
