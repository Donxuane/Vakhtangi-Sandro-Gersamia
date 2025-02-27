using BudgetingExpense.Domain.Contracts.IUnitOfWork;

using BudgetingExpenses.Service.IServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpense.Domain.Models.AuthenticationModels;

namespace BudgetingExpenses.Service.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task AddUserRolesAsync(string email, string role)
    {
        try
        {
            await _unitOfWork.Authentication.AddUserRoleAsync(email, role);
        }
        catch (Exception)
        {

        }
    }

    public  Task<string> GenerateJwtTokenAsync(string userId, string userRole)
    {
        return Task.Run(() =>
        {
            var Jwt = _configuration.GetSection("Jwt");
            var JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt["Key"]));

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userId),
            new Claim(ClaimTypes.Role, userRole)
            };
            var token = new JwtSecurityToken(
                issuer: Jwt["Issuer"],
                audience: Jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(Jwt["ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(JwtKey, SecurityAlgorithms.HmacSha256)
               
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        });
    }

    public async Task<IList<string>?> GetRoleAsync(string email)
    {
        try
        {
            var roles = await _unitOfWork.Authentication.GetUserRolesAsync(email);
            return roles;
        }
        catch (Exception)
        {

        }
        return null;
    }

    public async Task<User?> GetUserAsync(string email)
    {
        try
        {
            var user = await _unitOfWork.Authentication.GetUserByEmailAsync(email);
            return user;
        }
        catch (Exception)
        {

        }
        return null;
    }

    public async Task<bool> LoginUserServiceAsync(Login user)
    {
        try
        {
            var check = await _unitOfWork.Authentication.CheckUserAsync(user.Email, user.Password);

            if (check == true)
            {
                return true;
            }
        }
        catch(Exception)
        { 

        }
        return false;
    }

    public async Task<bool> RegisterUserServiceAsync(Register user)
    {
        try
        {
            var userMapped = new User
            {
                Email = user.Email,
                Password = user.Password,
                Surname = user.Surname,
                Name = user.Name
            };
            var result = await _unitOfWork.Authentication.RegisterUserAsync(userMapped);
            if (result == true)
            {
                return true;
            }
        }
        catch(Exception)
        {

        }
        return false;
    }
}
