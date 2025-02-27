using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.AspNetCore.Identity;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class Authentication : IAuthentication
{
    private readonly UserManager<IdentityModel> _userManager;

    public Authentication(UserManager<IdentityModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task AddUserRoleAsync(string email, string role)
    {
        var model = await GetUserPrivatelyForCheckingPurposes(email);
        await _userManager.AddToRoleAsync(model, role);
    }
    private async Task<IdentityModel> GetUserPrivatelyForCheckingPurposes(string email)
    {
        var model = await _userManager.FindByEmailAsync(email);
        if (model != null)
            return model;
        return null;
    }
    public async Task<bool> CheckUserAsync(string email,string password)
    {
        var user = await GetUserPrivatelyForCheckingPurposes(email);
        if (user != null)
        {
            var checkedPassword = _userManager.CheckPasswordAsync(user, password);
            if (checkedPassword != null)
                return true;
        }
        return false;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var result =  await _userManager.FindByEmailAsync(email);
        if (result != null)
        {
            var user = new User
            {
                Id = result.Id,
                Name = result.Name,
                Surname = result.Surname,
                Email = result.Email,
                Notifications = result.Notifications,
                RegisterDate = result.RegisterDate
            };
            return user;
        }
        return null;
    }

    public async Task<IList<string>?> GetUserRolesAsync(string email)
    {
        var model = await GetUserPrivatelyForCheckingPurposes(email);
        var roles = await _userManager.GetRolesAsync(model);
        return roles;
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        var userDetails = new IdentityModel {
            UserName = user.Email,
            Email = user.Email,
            Name = user.Name, 
            Surname = user.Surname,
            RegisterDate = DateTime.Now,
            Notifications = false
        };
        var result = await _userManager.CreateAsync(userDetails, user.Password);
        if (result.Succeeded)
            return true;
        return false;
    }
}
