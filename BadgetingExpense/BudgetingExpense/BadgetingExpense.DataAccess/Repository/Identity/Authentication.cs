using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class Authentication : IAuthentication
{
    private readonly UserManager<IdentityModel> _userManager;

    public Authentication(UserManager<IdentityModel> userManager)
    {
        _userManager = userManager;
    }
    public async Task<bool> CheckUserAsync(string email,string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var checkedPassword = _userManager.CheckPasswordAsync(user, password);
        if (user == null || checkedPassword == null)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        var userDetails = new IdentityModel {
            UserName = user.UserName, 
            Email = user.Email,
            UserSurname = user.UserSurname,
            RegisterDate = DateTime.Now,
            Notifications = false
        };
        var result = await _userManager.CreateAsync(userDetails, user.Password);
        if (result.Succeeded)
            return true;
        return false;
    }
}
