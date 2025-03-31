using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpense.DataAccess.Configuration;

public class ConfigureSeeding
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigureSeeding> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityModel> _userManager;

    public ConfigureSeeding(RoleManager<IdentityRole> roleManager, IConfiguration configuration,
        ILogger<ConfigureSeeding> logger, IUnitOfWork unitOfWork, UserManager<IdentityModel> userManager)
    {
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
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

    public async Task SeedData()
    {
        var users = _configuration.GetSection("Users").Get<List<User>>();
        var category = _configuration.GetSection("Category").Get<List<Category>>();
        var incomes = _configuration.GetSection("Income").Get<List<Income>>();
        var expenses = _configuration.GetSection("Expenses").Get<List<Expense>>();
        var limits = _configuration.GetSection("Limits").Get<List<Limits>>();
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            foreach (var item in category)
            {
                if (item.Type == 1)
                {

                    await _unitOfWork.IncomeManage.AddCategoryAsync(item);

                    _logger.LogInformation("New income category added {item}", item.Name);
                }

                else
                {

                    await _unitOfWork.ExpenseManage.AddCategoryAsync(item);

                    _logger.LogInformation("New expense category added {item}", item.Name);
                }
            }

            foreach (var user in users)
            {
                var result = await _unitOfWork.Authentication.RegisterUserAsync(user);

                await _unitOfWork.Authentication.AddUserRoleAsync(user.Email, "User");

                if (result)
                {
                    
                    var userModel = await _userManager.FindByEmailAsync(user.Email);
                    
                    var userId = userModel.Id;
                    await _unitOfWork.ToggleNotificationsRepository.ToggleNotification(userId, true);
                    foreach (var item in incomes)
                    {

                        item.UserId = userId;
                        await _unitOfWork.IncomeManage.AddAsync(item);

                        _logger.LogInformation("New incomes  added{item} ", item);
                    }

                    foreach (var item in expenses)
                    {

                        item.UserId = userId;
                        await _unitOfWork.ExpenseManage.AddAsync(item);

                        _logger.LogInformation("New expenses  added{item} ", item);
                    }

                    foreach (var item in limits)
                    {

                        item.UserId = userId;
                        await _unitOfWork.LimitsRepository.AddLimitAsync(item);

                        _logger.LogInformation("New limits  added{item} ", item);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _unitOfWork.RollBackAsync();
        }
    }
}
