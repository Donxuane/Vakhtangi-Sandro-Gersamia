using Microsoft.AspNetCore.Identity;

namespace BudgetingExpense.api.Configuration;

public class ConfigureRoles
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public ConfigureRoles(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task RoleCeeder()
    {
        List<string> roles = ["Admin", "User"];
        foreach (var item in roles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(item);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(item));
            }
        }
    }
}
