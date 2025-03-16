using Microsoft.AspNetCore.Identity;

namespace BudgetingExpense.api.Configuration;

public class ConfigureSeeding
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigureSeeding> _logger;

    public ConfigureSeeding(RoleManager<IdentityRole> roleManager, IConfiguration configuration,
        ILogger<ConfigureSeeding> logger)
    {
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedRoles()
    {
        try
        {
            var roles = _configuration.GetSection("Roles").Get<string[]>();
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    var check = await _roleManager.RoleExistsAsync(role);
                    if (!check)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                        _logger.LogInformation("New Role Added: {role}", role);
                    }
                    _logger.LogInformation("Role Already Exists: {role}", role);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
    }
}
